Imports System
Imports System.Windows.Forms
Imports SAPStudio.VFRHelper.Plugins.TfmOverrideEditorPlugin
Imports System.Collections.Generic
Imports System.Text.RegularExpressions
Public Class EditorForm
    Dim _plugin As TfmOverrideEditorPlugin
    Friend _matchRadioBoxes As RadioButton()
    Public Sub SetPlugin(ByVal plugin As TfmOverrideEditorPlugin)
        _plugin = plugin
    End Sub

    Private Sub btnApplyCurrent_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        _plugin.SetFrameOptionCurrentFrame()
    End Sub

    Private Sub EditorForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _matchRadioBoxes = New RadioButton() {rdoMatchP, rdoMatchC, rdoMatchN, rdoMatchB, rdoMatchU}
    End Sub

    Private Sub rdoMatch_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoMatchNotSpecified.CheckedChanged, rdoMatchB.CheckedChanged, rdoMatchC.CheckedChanged, rdoMatchN.CheckedChanged, rdoMatchP.CheckedChanged, rdoMatchU.CheckedChanged
        If _plugin Is Nothing Then
            Return
        End If
        If CType(sender, RadioButton).Checked Then
            _plugin.SetFrameOptionCurrentFrame()
        End If
    End Sub

    Private Sub chkCombed_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCombed.CheckStateChanged
        If _plugin Is Nothing Then
            Return
        End If
        _plugin.SetFrameOptionCurrentFrame()
    End Sub
    Public Function GetSelectedMatchCode() As String
        If rdoMatchNotSpecified.Checked Then
            Return String.Empty
        End If
        For Each rdoBox In _matchRadioBoxes
            If rdoBox.Checked Then
                Return rdoBox.Name.Substring(rdoBox.Name.Length - 1, 1).ToLower()
            End If
        Next
        Debug.Fail("All boxes are unselected")
        Return ""
    End Function
    Public Function GetCombedStatusFromCheckBox(ByVal cb As CheckBox) As Boolean?
        Dim combed As Boolean?
        Select Case cb.CheckState
            Case CheckState.Checked
                combed = True
            Case CheckState.Unchecked
                combed = False
            Case CheckState.Indeterminate
                combed = Nothing
        End Select
        Return combed
    End Function
    Public Sub RefreshListBoxDataSource()
        CType(lstOverrideEntries.BindingContext(lstOverrideEntries.DataSource), CurrencyManager).Refresh()
    End Sub
    Public Sub SetFrameInfo(info As String)
        lblFrameInfo.Text = info
    End Sub

    Private Sub txtFrameRangeStart_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtFrameRangeStart.Validating, txtFrameRangeEnd.Validating
        If Not Regex.IsMatch(CType(sender, TextBox).Text, "^\d+$", RegexOptions.Singleline) Then
            e.Cancel = True
        End If
    End Sub

    Private Sub txtPattern_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPattern.KeyDown
        If e.KeyCode = Keys.Return Then
            btnApplyRange.PerformClick()
        End If
    End Sub

    Private Sub txtPattern_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtPattern.Validating
        If Not Regex.IsMatch(CType(sender, TextBox).Text, "^[pcnbu]*$", RegexOptions.Singleline) Then
            e.Cancel = True
        End If
    End Sub

    Private Sub btnApplyRange_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApplyRange.Click
        _plugin.SetFrameOptionFrameRange()
    End Sub

    Private Sub btnSettings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSettings.Click
        _plugin.ChangeTfmSettings()
    End Sub

    Private Sub lstOverrideEntries_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lstOverrideEntries.MouseClick
        _plugin.JumpToSelectedFrame()
    End Sub

    Private Sub lstOverrideEntries_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lstOverrideEntries.MouseDoubleClick
        _plugin.ResetFrameOptions()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        _plugin.SaveOverridesFile()
    End Sub

    Private Sub btnSetRangeStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSetRangeStart.Click
        _plugin.Action_SetLockRangeStartFrame()
    End Sub

    Private Sub btnSetRangeEnd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSetRangeEnd.Click
        _plugin.Action_SetLockRangeEndFrame()
    End Sub
End Class