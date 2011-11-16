Imports SAPStudio.VFRHelper.Plugins
Imports System.Text
Imports System.IO

<Assembly: VFRHelperPlugin(GetType(TfmOverrideEditorPlugin))> 
<PluginClass(FriendlyPluginName:="TFM Override Editor")> _
Public Class TfmOverrideEditorPlugin
    Inherits PluginBase

    Dim _settingsForm As TfmSettingsForm
    Dim _editorForm As EditorForm
    Dim WithEvents _provider As TfmVideoProvider
    Dim _frameOptionGroups As List(Of FrameOptionGroup)
    Dim _seeking As Boolean = False
    Dim _dirty As Boolean = False

    Private Delegate Function Func(Of R)() As R

    Protected Overloads Overrides Sub Initialize()
        RegisterPluginFunction(-1, AddressOf Main)
        RegisterHotKeyAction("ToggleMatch", AddressOf Action_ToggleMatch)
        RegisterHotKeyAction("SetMatchDefault", Action_SetMatch(Function() _editorForm.rdoMatchNotSpecified))
        RegisterHotKeyAction("SetMatchP", Action_SetMatch(Function() _editorForm.rdoMatchP))
        RegisterHotKeyAction("SetMatchC", Action_SetMatch(Function() _editorForm.rdoMatchC))
        RegisterHotKeyAction("SetMatchN", Action_SetMatch(Function() _editorForm.rdoMatchN))
        RegisterHotKeyAction("SetMatchB", Action_SetMatch(Function() _editorForm.rdoMatchB))
        RegisterHotKeyAction("SetMatchU", Action_SetMatch(Function() _editorForm.rdoMatchU))
        RegisterHotKeyAction("ToggleCombed", AddressOf Action_ToggleCombed)
        RegisterHotKeyAction("SetLockRangeStartFrame", AddressOf Action_SetLockRangeStartFrame)
        RegisterHotKeyAction("SetLockRangeEndFrame", AddressOf Action_SetLockRangeEndFrame)
        RegisterHotKeyAction("EnterFrameRangePattern", AddressOf Action_EnterFrameRangePattern)
        RegisterHotKeyAction("ToggleFrameRangeCombed", AddressOf Action_ToggleFrameRangeCombed)
        RegisterHotKeyAction("ApplyFrameRange", AddressOf Action_ApplyFrameRange)
    End Sub

    Private Sub OpenD2V()
        Dim newProvider = New TfmVideoProvider
        newProvider.Open(_settingsForm.txtD2V.Text, _settingsForm.MakeTfmParam())
        If _provider IsNot Nothing Then
            _provider.Dispose()
        End If
        _provider = newProvider
        Host.VideoProvider = _provider
        If _settingsForm.txtTfmAnalysisFile.Text <> "" Then
            Try
                _provider.ReadTFMAnalysisFile(_settingsForm.txtTfmAnalysisFile.Text, _settingsForm.chkMarkCombedFramesAsKeyFrame.Checked, _settingsForm.chkMarkPossiblyCombedFramesAsKeyFrame.Checked)
            Catch ex As Exception
                _settingsForm.txtTfmAnalysisFile.Text = ""
                MessageBox.Show(ex.Message, "TFM Overrides Editor", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End Try
        End If
    End Sub
    Private Sub Main()
        _settingsForm = New TfmSettingsForm
        _settingsForm.ShowDialog()
        If _settingsForm.DialogResult = Windows.Forms.DialogResult.Cancel Then
            Dispose()
            Return
        End If
        Host.NotifyMessage("Loading video, please wait...")
        OpenD2V()
        AddHandler Host.VideoProviderChanged, AddressOf OnHostVideoProviderChange
        _editorForm = New EditorForm
        _editorForm.SetPlugin(Me)
        FillEditorListBox()
        _editorForm.Show()
        Host.DockWindow(_editorForm)
        AddHandler Host.BeforePluginUnload, AddressOf OnBeforePluginUnload
        Host.NotifyMessage(Nothing)
        Host.UpdateView()
    End Sub
    Private Sub FillEditorListBox()
        Try
            Dim ovr As String = _settingsForm.txtExistingOverrides.Text
            If Not String.IsNullOrEmpty(ovr) Then
                _frameOptionGroups = _provider.ReadOverrideFile(ovr)
            Else
                _frameOptionGroups = New List(Of FrameOptionGroup)
            End If
            _editorForm.lstOverrideEntries.DataSource = _frameOptionGroups
        Catch ex As Exception
            Dispose()
            Throw
        End Try
    End Sub
    Sub ChangeTfmSettings()
        If _settingsForm.ShowDialog() = DialogResult.Cancel Then
            Return
        End If
        Dim orgFrame = _provider.CurrentFrameNumber
        Try
            OpenD2V()
        Catch ex As Exception
            _settingsForm.RestoreState()
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try
        _provider.SetFrameOptionsFromOptionGroups(_frameOptionGroups)
        _provider.SeekTo(orgFrame)
        Host.UpdateView()
    End Sub
    Private Sub OnHostVideoProviderChange(ByVal sender As Object, ByVal e As EventArgs)
        If Host.VideoProvider IsNot _provider Then
            Dispose()
        End If
    End Sub
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If _dirty Then
                Dim result = MessageBox.Show("Your overrides file has not been saved, save it now?", "TFM Overrides Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
               If result = DialogResult.Yes Then
                    SaveOverridesFile()
                End If
            End If
            RemoveHandler Host.VideoProviderChanged, AddressOf OnHostVideoProviderChange
            RemoveHandler Host.BeforePluginUnload, AddressOf OnBeforePluginUnload
            If _settingsForm IsNot Nothing Then
                _settingsForm.Dispose()
                _settingsForm = Nothing
            End If
            If _editorForm IsNot Nothing Then
                Host.UndockWindow(_editorForm)
                _editorForm.Dispose()
                _editorForm = Nothing
            End If
            If _provider IsNot Nothing AndAlso Host.VideoProvider Is _provider Then
                Host.VideoProvider.Dispose()
                Host.VideoProvider = Nothing
            End If
            _provider = Nothing
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Private Sub UpdateRangeTextBoxes()
    '    If Not _editorForm.chkLockStartFrame.Checked Then
    '        _editorForm.txtFrameRangeStart.Text = _provider.CurrentFrameNumber.ToString()
    '    End If
    '    If Not _editorForm.chkLockEndFrame.Checked Then
    '        _editorForm.txtFrameRangeEnd.Text = _provider.CurrentFrameNumber.ToString()
    '    End If
    'End Sub
    Private Sub _provider_Seek(ByVal sender As Object, ByVal e As System.EventArgs) Handles _provider.Seek
        _seeking = True
        If _editorForm IsNot Nothing Then
            Dim currentFrameOption = _provider.GetCurrentFrameOption()
            If currentFrameOption Is Nothing Then
                _editorForm.chkCombed.CheckState = Windows.Forms.CheckState.Indeterminate
                _editorForm.rdoMatchNotSpecified.Checked = True
            Else
                If currentFrameOption.Value.IsCombed Is Nothing Then
                    _editorForm.chkCombed.CheckState = Windows.Forms.CheckState.Indeterminate
                ElseIf currentFrameOption.Value.IsCombed(0) Then
                    _editorForm.chkCombed.CheckState = Windows.Forms.CheckState.Checked
                Else
                    _editorForm.chkCombed.CheckState = Windows.Forms.CheckState.Unchecked
                End If
                If String.IsNullOrEmpty(currentFrameOption.Value.MatchCode) OrElse currentFrameOption.Value.MatchCode = ChrW(0) Then
                    _editorForm.rdoMatchNotSpecified.Checked = True
                Else
                    CType(_editorForm.gbMatch.Controls.Find("rdoMatch" & currentFrameOption.Value.MatchCode.ToUpper(), False)(0), RadioButton).Checked = True
                End If
            End If
        End If
        _seeking = False
    End Sub

    Public Sub SetFrameOptionCurrentFrame()
        If _seeking Then
            Return
        End If
        Dim combed As Boolean?
        combed = _editorForm.GetCombedStatusFromCheckBox(_editorForm.chkCombed)
        Dim matchCode As String = Nothing
        If Not _editorForm.rdoMatchNotSpecified.Checked Then
            For Each rdoBox In _editorForm._matchRadioBoxes
                If rdoBox.Checked Then
                    matchCode = rdoBox.Name.Substring(rdoBox.Name.Length - 1, 1).ToLower()
                    Exit For
                End If
            Next
        End If
        Dim currentFrame As Integer = _provider.CurrentFrameNumber
        Dim optionGroup = _frameOptionGroups.Find(Function(og) og.Start <= currentFrame AndAlso og.End >= currentFrame)
        If optionGroup Is Nothing Then
            optionGroup = New FrameOptionGroup With { _
                                                .Start = currentFrame, _
                                                .End = currentFrame, _
                                                .Option = New FrameOption}
            _frameOptionGroups.Add(optionGroup)
        End If
        BreakApartOptionGroupIfNecessary(optionGroup, currentFrame, currentFrame)
        optionGroup.Option.MatchCode = matchCode
        optionGroup.Option.IsCombed = If(combed.HasValue, New Boolean() {combed.Value}, Nothing)
        _editorForm.RefreshListBoxDataSource()
        _editorForm.lstOverrideEntries.SelectedItem = optionGroup
        _provider.SetCurrentFrameOption(matchCode, combed)
        Host.UpdateView()
        Host.ActivateMainWindow()
        _dirty = True
    End Sub
    Public Sub SetFrameOptionFrameRange()
        Dim combed As Boolean? = _editorForm.GetCombedStatusFromCheckBox(_editorForm.chkRangeCombed)
        Dim startFrame = Integer.Parse(_editorForm.txtFrameRangeStart.Text)
        Dim endFrame = Integer.Parse(_editorForm.txtFrameRangeEnd.Text)
        If endFrame > _provider.FrameCount - 1 Then
            endFrame = _provider.FrameCount - 1
        End If
        For Each group In _frameOptionGroups.FindAll(Function(og) og.Start >= startFrame AndAlso og.Start <= endFrame)
            BreakApartOptionGroupIfNecessary(group, startFrame, endFrame)
            _frameOptionGroups.Remove(group)
        Next
        For Each group In _frameOptionGroups.FindAll(Function(og) og.End >= startFrame AndAlso og.End <= endFrame)
            BreakApartOptionGroupIfNecessary(group, startFrame, endFrame)
            _frameOptionGroups.Remove(group)
        Next
        For Each group In _frameOptionGroups.FindAll(Function(og) og.Start <= startFrame AndAlso og.End >= endFrame)
            BreakApartOptionGroupIfNecessary(group, startFrame, endFrame)
            _frameOptionGroups.Remove(group)
        Next
        Dim optionGroup = New FrameOptionGroup With { _
                                            .Start = startFrame, _
                                            .End = endFrame, _
                                            .Option = New FrameOption With { _
                                                        .MatchCode = If(_editorForm.txtPattern.Text = "", Nothing, _editorForm.txtPattern.Text), _
                                                        .IsCombed = If(combed.HasValue, New Boolean() {combed.Value}, Nothing)}}
        _frameOptionGroups.Add(optionGroup)
        _editorForm.RefreshListBoxDataSource()
        _editorForm.lstOverrideEntries.SelectedItem = optionGroup
        _provider.SetFrameRangeOptions(optionGroup)
        Host.UpdateView()
        Host.ActivateMainWindow()
        _dirty = True
    End Sub
    Sub ResetFrameOptions()
        If _editorForm.lstOverrideEntries.SelectedItem IsNot Nothing Then
            Dim optionGroup = CType(_editorForm.lstOverrideEntries.SelectedItem, FrameOptionGroup)
            _provider.ResetFrameOptions(optionGroup.Start, optionGroup.End)
            _frameOptionGroups.Remove(optionGroup)
            _editorForm.RefreshListBoxDataSource()
            Host.UpdateView()
            _dirty = True
        End If
    End Sub
    Sub JumpToSelectedFrame()
        If _editorForm.lstOverrideEntries.SelectedItem IsNot Nothing Then
            _provider.SeekTo(CType(_editorForm.lstOverrideEntries.SelectedItem, FrameOptionGroup).Start)
            Host.ActivateMainWindow()
            Host.UpdateView()
        End If
    End Sub
    Private Sub BreakApartOptionGroupIfNecessary(ByVal optionGroup As FrameOptionGroup, ByVal reserveStart As Integer, ByVal reserveEnd As Integer)
        Dim orgStart = optionGroup.Start
        If reserveStart > optionGroup.Start Then
            _frameOptionGroups.Add(New FrameOptionGroup With { _
                                            .Start = optionGroup.Start, _
                                            .End = reserveStart - 1, _
                                            .Option = CType(optionGroup.Option.Clone(), FrameOption)})
            optionGroup.Start = reserveStart
        End If
        If reserveEnd < optionGroup.End Then
            Dim newGroup = New FrameOptionGroup With { _
                                            .Start = reserveEnd + 1, _
                                            .End = optionGroup.End, _
                                            .Option = CType(optionGroup.Option.Clone(), FrameOption)}
            If Not String.IsNullOrEmpty(optionGroup.Option.MatchCode) AndAlso (newGroup.Start - orgStart) Mod optionGroup.Option.MatchCode.Length > 0 Then
                Dim pos = (newGroup.Start - orgStart) Mod optionGroup.Option.MatchCode.Length
                newGroup.Option.MatchCode = optionGroup.Option.MatchCode.Substring(pos) & optionGroup.Option.MatchCode.Substring(0, pos)
            End If
            _frameOptionGroups.Add(newGroup)
            optionGroup.End = reserveEnd
        End If
    End Sub

    Public Function SaveOverridesFile() As Boolean
        Dim filePath = Host.BrowseSave("TFM overrides file(*.txt)|*.txt")
        If String.IsNullOrEmpty(filePath) Then
            Return False
        End If
        _frameOptionGroups.Sort(Function(x, y) x.Start - y.Start)
        _editorForm.RefreshListBoxDataSource()
        Using fs As New FileStream(filePath, FileMode.Create), swr As New StreamWriter(fs, Encoding.ASCII)
            For Each group In _frameOptionGroups
                If Not String.IsNullOrEmpty(group.Option.MatchCode) Then
                    WriteStartAndEndFrame(swr, group)
                    swr.WriteLine(" {0}", group.Option.MatchCode)
                End If
                If group.Option.IsCombed IsNot Nothing Then
                    WriteStartAndEndFrame(swr, group)
                    swr.WriteLine(" {0}", New String(Array.ConvertAll(Of Boolean, Char)(group.Option.IsCombed, Function(c) If(c, "+"c, "-"c))))
                End If
                If group.Option.OtherOptions IsNot Nothing Then
                    For Each opt In group.Option.OtherOptions
                        WriteStartAndEndFrame(swr, group)
                        swr.WriteLine(" {0}", opt)
                    Next
                End If
            Next
        End Using
        _dirty = False
        Return True
    End Function
    Private Shared Sub WriteStartAndEndFrame(ByVal swr As StreamWriter, ByVal group As SAPStudio.VFRHelper.Plugins.TfmOverrideEditorPlugin.FrameOptionGroup)
        swr.Write(group.Start)
        If group.End <> group.Start Then
            swr.Write(",{0}", group.End)
        End If
    End Sub
    Private Sub OnBeforePluginUnload(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
        If _dirty Then
            Dim result = MessageBox.Show("Your overrides file has not been saved, save it now?", "TFM Overrides Editor", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
            If result = DialogResult.Cancel Then
                e.Cancel = True
            ElseIf result = DialogResult.Yes Then
                e.Cancel = Not SaveOverridesFile()
            Else
                _dirty = False
            End If
        End If
    End Sub
    Private Function Action_ToggleMatch() As Boolean
        If _editorForm.rdoMatchNotSpecified.Checked Then
            _editorForm.rdoMatchP.Checked = True
        ElseIf _editorForm.rdoMatchU.Checked Then
            _editorForm.rdoMatchNotSpecified.Checked = True
        Else
            For i = 0 To 3
                If _editorForm._matchRadioBoxes(i).Checked Then
                    _editorForm._matchRadioBoxes(i + 1).Checked = True
                    Exit For
                End If
            Next
        End If
        Return False
    End Function
    Private Function Action_SetMatch(targetGetter As Func(Of RadioButton)) As HotKeyActionDelegate
        Return Function() As Boolean
                   targetGetter().Checked = True
                   Return False
               End Function
    End Function
    Private Function Action_ToggleCombed() As Boolean
        Select Case _editorForm.chkCombed.CheckState
            Case CheckState.Indeterminate
                _editorForm.chkCombed.CheckState = CheckState.Unchecked
            Case CheckState.Unchecked
                _editorForm.chkCombed.CheckState = CheckState.Checked
            Case CheckState.Checked
                _editorForm.chkCombed.CheckState = CheckState.Indeterminate
        End Select
        Return False
    End Function
    'Private Function Action_ToggleLockRangeStartFrame() As Boolean
    '    _editorForm.chkLockStartFrame.Checked = Not _editorForm.chkLockStartFrame.Checked
    '    UpdateRangeTextBoxes()
    '    Return False
    'End Function
    'Private Function Action_ToggleLockRangeEndFrame() As Boolean
    '    _editorForm.chkLockEndFrame.Checked = Not _editorForm.chkLockEndFrame.Checked
    '    UpdateRangeTextBoxes()
    '    Return False
    'End Function
    Private Function Action_EnterFrameRangePattern() As Boolean
        _editorForm.BringToFront()
        _editorForm.txtPattern.SelectAll()
        _editorForm.txtPattern.Focus()
        Return False
    End Function
    Private Function Action_ToggleFrameRangeCombed() As Boolean
        Select Case _editorForm.chkRangeCombed.CheckState
            Case CheckState.Indeterminate
                _editorForm.chkRangeCombed.CheckState = CheckState.Unchecked
            Case CheckState.Unchecked
                _editorForm.chkRangeCombed.CheckState = CheckState.Checked
            Case CheckState.Checked
                _editorForm.chkRangeCombed.CheckState = CheckState.Indeterminate
        End Select
        Return False
    End Function
    Private Function Action_ApplyFrameRange() As Boolean
        _editorForm.btnApplyRange.PerformClick()
        Return False
    End Function
    Public Function Action_SetLockRangeStartFrame() As Boolean
        _editorForm.txtFrameRangeStart.Text = Host.VideoProvider.CurrentFrameNumber.ToString()
        Return False
    End Function
    Public Function Action_SetLockRangeEndFrame() As Boolean
        _editorForm.txtFrameRangeEnd.Text = Host.VideoProvider.CurrentFrameNumber.ToString()
        Return False
    End Function
End Class
