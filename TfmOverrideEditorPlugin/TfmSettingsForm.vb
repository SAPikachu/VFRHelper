Imports System.Text
Imports System.Windows.Forms
Imports System.IO
Public Class TfmSettingsForm
    Dim _stateStorage As New Dictionary(Of Control, Object)
    Dim _dropAcceptExtensions As New Dictionary(Of Object, IList(Of String))
    Public Sub RestoreState()
        RestoreStateRecursive(Me)
    End Sub
    Private Sub RestoreStateRecursive(ByVal container As Control)
        For Each ctl As Control In container.Controls
            If TypeOf ctl Is CheckBox Then
                CType(ctl, CheckBox).Checked = CBool(_stateStorage(ctl))
            ElseIf TypeOf ctl Is TextBox Then
                CType(ctl, TextBox).Text = CStr(_stateStorage(ctl))
            ElseIf TypeOf ctl Is ComboBox Then
                CType(ctl, ComboBox).SelectedIndex = CInt(_stateStorage(ctl))
            ElseIf TypeOf ctl Is RadioButton Then
                CType(ctl, RadioButton).Checked = CBool(_stateStorage(ctl))
            Else
                RestoreStateRecursive(ctl)
            End If
        Next
    End Sub
    Public Sub SaveState()
        SaveStateRecursive(Me)
    End Sub
    Private Sub SaveStateRecursive(ByVal container As Control)
        For Each ctl As Control In container.Controls
            If TypeOf ctl Is CheckBox Then
                _stateStorage(ctl) = CType(ctl, CheckBox).Checked
            ElseIf TypeOf ctl Is TextBox Then
                _stateStorage(ctl) = CType(ctl, TextBox).Text
            ElseIf TypeOf ctl Is ComboBox Then
                _stateStorage(ctl) = CType(ctl, ComboBox).SelectedIndex
            ElseIf TypeOf ctl Is RadioButton Then
                _stateStorage(ctl) = CType(ctl, RadioButton).Checked
            Else
                SaveStateRecursive(ctl)
            End If
        Next
    End Sub
    Private Sub FormSettings_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If _stateStorage.Count > 0 Then
            RestoreState()
        End If
    End Sub

    Sub New()

        ' 此调用是 Windows 窗体设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        cboMode.SelectedIndex = 1
        cboFieldOrder.SelectedIndex = 0
        cboPPMode.SelectedIndex = 1
        cboMicmatching.SelectedIndex = 1

        AddDragHandler(txtD2V, ".d2v|.dga|.dgi|.avs")
        AddDragHandler(txtExistingOverrides, ".txt")
        AddDragHandler(txtTfmAnalysisFile, ".txt")
    End Sub

    Private Sub chkUseCustomParams_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUseCustomParams.CheckedChanged
        If chkUseCustomParams.Checked Then
            TabControl1.TabPages.Remove(tabBasicSettings)
            TabControl1.TabPages.Remove(tabAdvancedSettings)
        Else
            TabControl1.TabPages.Insert(1, tabBasicSettings)
            TabControl1.TabPages.Insert(2, tabAdvancedSettings)
        End If
        txtParams.ReadOnly = Not chkUseCustomParams.Checked
    End Sub

    Public Function MakeTfmParam() As String
        If chkUseCustomParams.Checked Then
            Return txtParams.Text
        End If
        Dim sb As New StringBuilder
        'sb.AppendFormat("d2v=""{0}""", txtD2V.Text)
        sb.AppendFormat("mode={0}, order={1}, pp={2}", cboMode.SelectedIndex, cboFieldOrder.SelectedIndex - 1, cboPPMode.SelectedIndex)
        sb.Append(", slow=2")
        If chkDisplayFrameInfo.Checked Then
            sb.Append(", display=true")
        End If
        If Not chkMChroma.Checked Then
            sb.Append(", mchroma=false")
        End If
        If txtScthresh.Text <> "" Then
            sb.AppendFormat(", scthresh={0}", txtScthresh.Text)
        End If
        If Not chkUbsco.Checked Then
            sb.Append(", ubsco=false")
        End If
        If cboMicmatching.SelectedIndex <> 1 Then
            sb.AppendFormat(", micmatching={0}", cboMicmatching.SelectedIndex)
        End If
        If Not chkMmsco.Checked Then
            sb.Append(", mmsco=false")
        End If
        If txtCthresh.Text <> "" Then
            sb.AppendFormat(", cthresh={0}", txtCthresh.Text)
        End If
        If chkChroma.Checked Then
            sb.Append(", chroma=true")
        End If
        If txtBlockX.Text <> "" Then
            sb.AppendFormat(", blockx={0}", txtBlockX.Text)
        End If
        If txtBlockY.Text <> "" Then
            sb.AppendFormat(", blocky={0}", txtBlockY.Text)
        End If
        If txtMI.Text <> "" Then
            sb.AppendFormat(", mi={0}", txtMI.Text)
        End If
        If txtMthresh.Text <> "" Then
            sb.AppendFormat(", mthresh={0}", txtMthresh.Text)
        End If
        'If txtExistingOverrides.Text <> "" Then
        '    sb.AppendFormat(", ovr=""{0}""", txtExistingOverrides.Text)
        'End If
        Return sb.ToString()
    End Function

    Private Sub TabControl1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabControl1.SelectedIndexChanged
        If TabControl1.SelectedTab Is tabParameters AndAlso chkUseCustomParams.Checked = False Then
            txtParams.Text = MakeTfmParam()
        End If
    End Sub

    Private Sub txtD2V_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtD2V.TextChanged
        btnOK.Enabled = (txtD2V.Text <> "")
    End Sub

    Function ShowOpen(ByVal filter As String) As String
        OpenFileDialog1.FileName = ""
        OpenFileDialog1.Filter = filter
        Return If(OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK, OpenFileDialog1.FileName, Nothing)
    End Function

    Private Sub btnBrowseD2V_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseD2V.Click
        Dim fileName = ShowOpen("Supported DG projects(*.d2v, *.dga, *.dgi)|*.d2v;*.dga;*.dgi|AVISynth script(*.avs)|*.avs")
        If Not String.IsNullOrEmpty(fileName) Then
            txtD2V.Text = fileName
        End If
    End Sub

    Private Sub btnBrowseExistingOverrides_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseExistingOverrides.Click
        Dim fileName = ShowOpen("TFM overrides file(*.txt)|*.txt")
        If Not String.IsNullOrEmpty(fileName) Then
            txtExistingOverrides.Text = fileName
        End If
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Not File.Exists(txtD2V.Text) Then
            MessageBox.Show("The D2V file doesn't exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
            Return
        End If
        If txtExistingOverrides.Text <> "" AndAlso Not File.Exists(txtD2V.Text) Then
            MessageBox.Show("The overrides file doesn't exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
            Return
        End If
        If txtTfmAnalysisFile.Text <> "" AndAlso Not File.Exists(txtTfmAnalysisFile.Text) Then
            MessageBox.Show("The TFM analysis file doesn't exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
            Return
        End If
        TabControl1.TabPages.Remove(tabFile)
        SaveState()
        Me.Hide()
        DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub btnBrowseTfmAnalysisFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseTfmAnalysisFile.Click
        Dim fileName = ShowOpen("TFM analysis file(*.txt)|*.txt")
        If Not String.IsNullOrEmpty(fileName) Then
            txtTfmAnalysisFile.Text = fileName
        End If
    End Sub

    Private Sub txtTfmAnalysisFile_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTfmAnalysisFile.TextChanged
        gbTfmAnalysisFileOption.Enabled = (txtTfmAnalysisFile.Text <> "")
    End Sub

    Private Sub OnCtlDragEnter(ByVal sender As Object, ByVal e As DragEventArgs)
        e.Effect = DragDropEffects.None
        If Not _dropAcceptExtensions.ContainsKey(sender) Then
            Return
        End If
        If (e.AllowedEffect And DragDropEffects.Copy) = DragDropEffects.Copy AndAlso e.Data.GetDataPresent(DataFormats.FileDrop) Then
            Dim files As String() = CType(e.Data.GetData(DataFormats.FileDrop), String())
            If _dropAcceptExtensions(sender).Contains(Path.GetExtension(files(0)).ToLowerInvariant()) Then
                e.Effect = DragDropEffects.Copy
            End If
        End If
    End Sub
    Private Sub OnCtlDragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs)
        If Not _dropAcceptExtensions.ContainsKey(sender) Then
            Return
        End If
        If e.Effect = DragDropEffects.Copy AndAlso e.Data.GetDataPresent(DataFormats.FileDrop) Then
            Dim files As String() = CType(e.Data.GetData(DataFormats.FileDrop), String())
            If _dropAcceptExtensions(sender).Contains(Path.GetExtension(files(0)).ToLowerInvariant()) Then
                CType(sender, Control).Text = files(0)
            End If
        End If
    End Sub
    Private Sub AddDragHandler(ByVal ctl As Control, ByVal ext As String)
        If ctl Is Nothing Then
            Throw New ArgumentNullException("ctl", "ctl is nothing.")
        End If
        If String.IsNullOrEmpty(ext) Then
            Throw New ArgumentException("ext is nothing or empty.", "ext")
        End If
        AddDragHandler(ctl, ext.Split("|"c))
    End Sub
    Private Sub AddDragHandler(ByVal ctl As Control, ByVal ext As IList(Of String))
        If ctl Is Nothing Then
            Throw New ArgumentNullException("ctl", "ctl is nothing.")
        End If
        If ext Is Nothing Then
            Throw New ArgumentNullException("ext", "ext is nothing.")
        End If
        For i = 0 To ext.Count - 1
            ext(i) = ext(i).ToLowerInvariant()
        Next
        _dropAcceptExtensions(ctl) = ext
        ctl.AllowDrop = True
        AddHandler ctl.DragEnter, AddressOf OnCtlDragEnter
        AddHandler ctl.DragDrop, AddressOf OnCtlDragDrop
    End Sub

End Class