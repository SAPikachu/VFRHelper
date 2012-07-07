<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TfmSettingsForm
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(TfmSettingsForm))
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.tabFile = New System.Windows.Forms.TabPage()
        Me.gbTfmAnalysisFileOption = New System.Windows.Forms.GroupBox()
        Me.chkMarkPossiblyCombedFramesAsKeyFrame = New System.Windows.Forms.CheckBox()
        Me.chkMarkCombedFramesAsKeyFrame = New System.Windows.Forms.CheckBox()
        Me.btnBrowseTfmAnalysisFile = New System.Windows.Forms.Button()
        Me.txtTfmAnalysisFile = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.btnBrowseExistingOverrides = New System.Windows.Forms.Button()
        Me.txtExistingOverrides = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnBrowseD2V = New System.Windows.Forms.Button()
        Me.txtD2V = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.tabBasicSettings = New System.Windows.Forms.TabPage()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cboPPMode = New System.Windows.Forms.ComboBox()
        Me.chkDisplayFrameInfo = New System.Windows.Forms.CheckBox()
        Me.cboFieldOrder = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cboMode = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.tabAdvancedSettings = New System.Windows.Forms.TabPage()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.txtMthresh = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtMI = New System.Windows.Forms.TextBox()
        Me.txtBlockY = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtBlockX = New System.Windows.Forms.TextBox()
        Me.chkChroma = New System.Windows.Forms.CheckBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtCthresh = New System.Windows.Forms.TextBox()
        Me.chkMmsco = New System.Windows.Forms.CheckBox()
        Me.cboMicmatching = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.chkUbsco = New System.Windows.Forms.CheckBox()
        Me.txtScthresh = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.chkMChroma = New System.Windows.Forms.CheckBox()
        Me.tabParameters = New System.Windows.Forms.TabPage()
        Me.chkUseCustomParams = New System.Windows.Forms.CheckBox()
        Me.txtParams = New System.Windows.Forms.TextBox()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.chkMarkUBNAsKeyFrame = New System.Windows.Forms.CheckBox()
        Me.TabControl1.SuspendLayout()
        Me.tabFile.SuspendLayout()
        Me.gbTfmAnalysisFileOption.SuspendLayout()
        Me.tabBasicSettings.SuspendLayout()
        Me.tabAdvancedSettings.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.tabParameters.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.tabFile)
        Me.TabControl1.Controls.Add(Me.tabBasicSettings)
        Me.TabControl1.Controls.Add(Me.tabAdvancedSettings)
        Me.TabControl1.Controls.Add(Me.tabParameters)
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(684, 183)
        Me.TabControl1.TabIndex = 0
        '
        'tabFile
        '
        Me.tabFile.Controls.Add(Me.gbTfmAnalysisFileOption)
        Me.tabFile.Controls.Add(Me.btnBrowseTfmAnalysisFile)
        Me.tabFile.Controls.Add(Me.txtTfmAnalysisFile)
        Me.tabFile.Controls.Add(Me.Label12)
        Me.tabFile.Controls.Add(Me.btnBrowseExistingOverrides)
        Me.tabFile.Controls.Add(Me.txtExistingOverrides)
        Me.tabFile.Controls.Add(Me.Label2)
        Me.tabFile.Controls.Add(Me.btnBrowseD2V)
        Me.tabFile.Controls.Add(Me.txtD2V)
        Me.tabFile.Controls.Add(Me.Label1)
        Me.tabFile.Location = New System.Drawing.Point(4, 22)
        Me.tabFile.Name = "tabFile"
        Me.tabFile.Padding = New System.Windows.Forms.Padding(3)
        Me.tabFile.Size = New System.Drawing.Size(676, 157)
        Me.tabFile.TabIndex = 0
        Me.tabFile.Text = "File"
        Me.tabFile.UseVisualStyleBackColor = True
        '
        'gbTfmAnalysisFileOption
        '
        Me.gbTfmAnalysisFileOption.Controls.Add(Me.chkMarkUBNAsKeyFrame)
        Me.gbTfmAnalysisFileOption.Controls.Add(Me.chkMarkPossiblyCombedFramesAsKeyFrame)
        Me.gbTfmAnalysisFileOption.Controls.Add(Me.chkMarkCombedFramesAsKeyFrame)
        Me.gbTfmAnalysisFileOption.Enabled = False
        Me.gbTfmAnalysisFileOption.Location = New System.Drawing.Point(389, 16)
        Me.gbTfmAnalysisFileOption.Name = "gbTfmAnalysisFileOption"
        Me.gbTfmAnalysisFileOption.Size = New System.Drawing.Size(277, 124)
        Me.gbTfmAnalysisFileOption.TabIndex = 9
        Me.gbTfmAnalysisFileOption.TabStop = False
        Me.gbTfmAnalysisFileOption.Text = "TFM analysis file options"
        '
        'chkMarkPossiblyCombedFramesAsKeyFrame
        '
        Me.chkMarkPossiblyCombedFramesAsKeyFrame.AutoSize = True
        Me.chkMarkPossiblyCombedFramesAsKeyFrame.Checked = True
        Me.chkMarkPossiblyCombedFramesAsKeyFrame.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkMarkPossiblyCombedFramesAsKeyFrame.Location = New System.Drawing.Point(10, 57)
        Me.chkMarkPossiblyCombedFramesAsKeyFrame.Name = "chkMarkPossiblyCombedFramesAsKeyFrame"
        Me.chkMarkPossiblyCombedFramesAsKeyFrame.Size = New System.Drawing.Size(228, 17)
        Me.chkMarkPossiblyCombedFramesAsKeyFrame.TabIndex = 1
        Me.chkMarkPossiblyCombedFramesAsKeyFrame.Text = "Mark possibly combed frames as key frame"
        Me.chkMarkPossiblyCombedFramesAsKeyFrame.UseVisualStyleBackColor = True
        '
        'chkMarkCombedFramesAsKeyFrame
        '
        Me.chkMarkCombedFramesAsKeyFrame.AutoSize = True
        Me.chkMarkCombedFramesAsKeyFrame.Checked = True
        Me.chkMarkCombedFramesAsKeyFrame.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkMarkCombedFramesAsKeyFrame.Location = New System.Drawing.Point(10, 30)
        Me.chkMarkCombedFramesAsKeyFrame.Name = "chkMarkCombedFramesAsKeyFrame"
        Me.chkMarkCombedFramesAsKeyFrame.Size = New System.Drawing.Size(188, 17)
        Me.chkMarkCombedFramesAsKeyFrame.TabIndex = 0
        Me.chkMarkCombedFramesAsKeyFrame.Text = "Mark combed frames as key frame"
        Me.chkMarkCombedFramesAsKeyFrame.UseVisualStyleBackColor = True
        '
        'btnBrowseTfmAnalysisFile
        '
        Me.btnBrowseTfmAnalysisFile.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseTfmAnalysisFile.Location = New System.Drawing.Point(297, 115)
        Me.btnBrowseTfmAnalysisFile.Name = "btnBrowseTfmAnalysisFile"
        Me.btnBrowseTfmAnalysisFile.Size = New System.Drawing.Size(75, 25)
        Me.btnBrowseTfmAnalysisFile.TabIndex = 8
        Me.btnBrowseTfmAnalysisFile.Text = "Browse"
        Me.btnBrowseTfmAnalysisFile.UseVisualStyleBackColor = True
        '
        'txtTfmAnalysisFile
        '
        Me.txtTfmAnalysisFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTfmAnalysisFile.Location = New System.Drawing.Point(10, 117)
        Me.txtTfmAnalysisFile.Name = "txtTfmAnalysisFile"
        Me.txtTfmAnalysisFile.Size = New System.Drawing.Size(281, 20)
        Me.txtTfmAnalysisFile.TabIndex = 7
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(8, 101)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(131, 13)
        Me.Label12.TabIndex = 6
        Me.Label12.Text = "TFM analysis file (optional)"
        '
        'btnBrowseExistingOverrides
        '
        Me.btnBrowseExistingOverrides.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseExistingOverrides.Location = New System.Drawing.Point(297, 73)
        Me.btnBrowseExistingOverrides.Name = "btnBrowseExistingOverrides"
        Me.btnBrowseExistingOverrides.Size = New System.Drawing.Size(75, 25)
        Me.btnBrowseExistingOverrides.TabIndex = 5
        Me.btnBrowseExistingOverrides.Text = "Browse"
        Me.btnBrowseExistingOverrides.UseVisualStyleBackColor = True
        '
        'txtExistingOverrides
        '
        Me.txtExistingOverrides.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtExistingOverrides.Location = New System.Drawing.Point(10, 75)
        Me.txtExistingOverrides.Name = "txtExistingOverrides"
        Me.txtExistingOverrides.Size = New System.Drawing.Size(281, 20)
        Me.txtExistingOverrides.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(8, 59)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(176, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Existing TFM overrides file (optional)"
        '
        'btnBrowseD2V
        '
        Me.btnBrowseD2V.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseD2V.Location = New System.Drawing.Point(297, 30)
        Me.btnBrowseD2V.Name = "btnBrowseD2V"
        Me.btnBrowseD2V.Size = New System.Drawing.Size(75, 25)
        Me.btnBrowseD2V.TabIndex = 2
        Me.btnBrowseD2V.Text = "Browse"
        Me.btnBrowseD2V.UseVisualStyleBackColor = True
        '
        'txtD2V
        '
        Me.txtD2V.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtD2V.Location = New System.Drawing.Point(10, 33)
        Me.txtD2V.Name = "txtD2V"
        Me.txtD2V.Size = New System.Drawing.Size(281, 20)
        Me.txtD2V.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(110, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "DG project or AVS file"
        '
        'tabBasicSettings
        '
        Me.tabBasicSettings.Controls.Add(Me.Label5)
        Me.tabBasicSettings.Controls.Add(Me.cboPPMode)
        Me.tabBasicSettings.Controls.Add(Me.chkDisplayFrameInfo)
        Me.tabBasicSettings.Controls.Add(Me.cboFieldOrder)
        Me.tabBasicSettings.Controls.Add(Me.Label4)
        Me.tabBasicSettings.Controls.Add(Me.cboMode)
        Me.tabBasicSettings.Controls.Add(Me.Label3)
        Me.tabBasicSettings.Location = New System.Drawing.Point(4, 22)
        Me.tabBasicSettings.Name = "tabBasicSettings"
        Me.tabBasicSettings.Padding = New System.Windows.Forms.Padding(3)
        Me.tabBasicSettings.Size = New System.Drawing.Size(676, 157)
        Me.tabBasicSettings.TabIndex = 1
        Me.tabBasicSettings.Text = "Basic settings"
        Me.tabBasicSettings.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(8, 77)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(111, 13)
        Me.Label5.TabIndex = 7
        Me.Label5.Text = "Post-processing mode"
        '
        'cboPPMode
        '
        Me.cboPPMode.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboPPMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPPMode.FormattingEnabled = True
        Me.cboPPMode.Items.AddRange(New Object() {"0 - nothing (don't even look for combed frames)", "1 - find/hint combed frames but don't deinterlace", "2 - dumb blend deinterlacing", "3 - dumb cubic interpolation deinterlacing", "4 - dumb modified-ela deinterlacing", "5 - motion-adaptive blend deinterlacing", "6 - motion-adaptive cubic interpolation deinterlacing", "7 - motion-adaptive modified-ela deinterlacing"})
        Me.cboPPMode.Location = New System.Drawing.Point(139, 74)
        Me.cboPPMode.Name = "cboPPMode"
        Me.cboPPMode.Size = New System.Drawing.Size(527, 21)
        Me.cboPPMode.TabIndex = 6
        Me.ToolTip1.SetToolTip(Me.cboPPMode, "Sets the post-processing mode. This controls how TFM should handle (or not handle" & _
        ")" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "any combed frames that come out of the field matching process.")
        '
        'chkDisplayFrameInfo
        '
        Me.chkDisplayFrameInfo.AutoSize = True
        Me.chkDisplayFrameInfo.Checked = True
        Me.chkDisplayFrameInfo.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkDisplayFrameInfo.Location = New System.Drawing.Point(10, 106)
        Me.chkDisplayFrameInfo.Name = "chkDisplayFrameInfo"
        Me.chkDisplayFrameInfo.Size = New System.Drawing.Size(189, 17)
        Me.chkDisplayFrameInfo.TabIndex = 5
        Me.chkDisplayFrameInfo.Text = "Overlay frame information on video"
        Me.chkDisplayFrameInfo.UseVisualStyleBackColor = True
        '
        'cboFieldOrder
        '
        Me.cboFieldOrder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFieldOrder.FormattingEnabled = True
        Me.cboFieldOrder.Items.AddRange(New Object() {"Auto", "BFF", "TFF"})
        Me.cboFieldOrder.Location = New System.Drawing.Point(139, 46)
        Me.cboFieldOrder.Name = "cboFieldOrder"
        Me.cboFieldOrder.Size = New System.Drawing.Size(69, 21)
        Me.cboFieldOrder.TabIndex = 3
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(62, 49)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(56, 13)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "Field order"
        '
        'cboMode
        '
        Me.cboMode.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboMode.FormattingEnabled = True
        Me.cboMode.Items.AddRange(New Object() {"0 - 2-way match (p/c)", "1 - 2-way match + 3rd match on combed (p/c + n)", "2 - 2-way match + 3rd match (same order) on combed (p/c + u)", "3 - 2-way match + 3rd match on combed + 4th/5th matches if still combed (p/c + n " & _
                "+ u/b)", "4 - 3-way match (p/c/n)", "5 - 3-way match + 4th/5th matches on combed (p/c/n + u/b)", "6 - mode 2 + extras if combed (p/c + u + n + b)", "7 - mode 0 + field switching based on previous matches"})
        Me.cboMode.Location = New System.Drawing.Point(139, 17)
        Me.cboMode.Name = "cboMode"
        Me.cboMode.Size = New System.Drawing.Size(527, 21)
        Me.cboMode.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.cboMode, resources.GetString("cboMode.ToolTip"))
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(50, 21)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(80, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Matching mode"
        '
        'tabAdvancedSettings
        '
        Me.tabAdvancedSettings.Controls.Add(Me.GroupBox1)
        Me.tabAdvancedSettings.Controls.Add(Me.chkMmsco)
        Me.tabAdvancedSettings.Controls.Add(Me.cboMicmatching)
        Me.tabAdvancedSettings.Controls.Add(Me.Label7)
        Me.tabAdvancedSettings.Controls.Add(Me.chkUbsco)
        Me.tabAdvancedSettings.Controls.Add(Me.txtScthresh)
        Me.tabAdvancedSettings.Controls.Add(Me.Label6)
        Me.tabAdvancedSettings.Controls.Add(Me.chkMChroma)
        Me.tabAdvancedSettings.Location = New System.Drawing.Point(4, 22)
        Me.tabAdvancedSettings.Name = "tabAdvancedSettings"
        Me.tabAdvancedSettings.Size = New System.Drawing.Size(676, 157)
        Me.tabAdvancedSettings.TabIndex = 2
        Me.tabAdvancedSettings.Text = "Advanced settings"
        Me.tabAdvancedSettings.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.txtMthresh)
        Me.GroupBox1.Controls.Add(Me.Label11)
        Me.GroupBox1.Controls.Add(Me.Label10)
        Me.GroupBox1.Controls.Add(Me.txtMI)
        Me.GroupBox1.Controls.Add(Me.txtBlockY)
        Me.GroupBox1.Controls.Add(Me.Label9)
        Me.GroupBox1.Controls.Add(Me.txtBlockX)
        Me.GroupBox1.Controls.Add(Me.chkChroma)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.txtCthresh)
        Me.GroupBox1.Location = New System.Drawing.Point(401, 4)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(272, 147)
        Me.GroupBox1.TabIndex = 9
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Combing detection options"
        '
        'txtMthresh
        '
        Me.txtMthresh.Location = New System.Drawing.Point(167, 86)
        Me.txtMthresh.Name = "txtMthresh"
        Me.txtMthresh.Size = New System.Drawing.Size(50, 20)
        Me.txtMthresh.TabIndex = 16
        Me.ToolTip1.SetToolTip(Me.txtMthresh, resources.GetString("txtMthresh.ToolTip"))
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(6, 89)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(129, 13)
        Me.Label11.TabIndex = 15
        Me.Label11.Text = "Motion adaption threshold"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(192, 24)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(19, 13)
        Me.Label10.TabIndex = 14
        Me.Label10.Text = "MI"
        '
        'txtMI
        '
        Me.txtMI.Location = New System.Drawing.Point(215, 21)
        Me.txtMI.Name = "txtMI"
        Me.txtMI.Size = New System.Drawing.Size(50, 20)
        Me.txtMI.TabIndex = 13
        Me.ToolTip1.SetToolTip(Me.txtMI, resources.GetString("txtMI.ToolTip"))
        '
        'txtBlockY
        '
        Me.txtBlockY.Location = New System.Drawing.Point(199, 53)
        Me.txtBlockY.Name = "txtBlockY"
        Me.txtBlockY.Size = New System.Drawing.Size(50, 20)
        Me.txtBlockY.TabIndex = 12
        Me.ToolTip1.SetToolTip(Me.txtBlockY, resources.GetString("txtBlockY.ToolTip"))
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(6, 56)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(113, 13)
        Me.Label9.TabIndex = 11
        Me.Label9.Text = "Detection window size"
        '
        'txtBlockX
        '
        Me.txtBlockX.Location = New System.Drawing.Point(143, 53)
        Me.txtBlockX.Name = "txtBlockX"
        Me.txtBlockX.Size = New System.Drawing.Size(50, 20)
        Me.txtBlockX.TabIndex = 10
        Me.ToolTip1.SetToolTip(Me.txtBlockX, resources.GetString("txtBlockX.ToolTip"))
        '
        'chkChroma
        '
        Me.chkChroma.AutoSize = True
        Me.chkChroma.Location = New System.Drawing.Point(6, 120)
        Me.chkChroma.Name = "chkChroma"
        Me.chkChroma.Size = New System.Drawing.Size(99, 17)
        Me.chkChroma.TabIndex = 9
        Me.chkChroma.Text = "Include chroma"
        Me.ToolTip1.SetToolTip(Me.chkChroma, resources.GetString("chkChroma.ToolTip"))
        Me.chkChroma.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(6, 24)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(94, 13)
        Me.Label8.TabIndex = 8
        Me.Label8.Text = "Combing threshold"
        '
        'txtCthresh
        '
        Me.txtCthresh.Location = New System.Drawing.Point(119, 21)
        Me.txtCthresh.Name = "txtCthresh"
        Me.txtCthresh.Size = New System.Drawing.Size(50, 20)
        Me.txtCthresh.TabIndex = 7
        Me.ToolTip1.SetToolTip(Me.txtCthresh, resources.GetString("txtCthresh.ToolTip"))
        '
        'chkMmsco
        '
        Me.chkMmsco.AutoSize = True
        Me.chkMmsco.Checked = True
        Me.chkMmsco.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkMmsco.Location = New System.Drawing.Point(8, 125)
        Me.chkMmsco.Name = "chkMmsco"
        Me.chkMmsco.Size = New System.Drawing.Size(286, 17)
        Me.chkMmsco.TabIndex = 6
        Me.chkMmsco.Text = "Allow micmatching around scenechanges only (mmsco)"
        Me.ToolTip1.SetToolTip(Me.chkMmsco, resources.GetString("chkMmsco.ToolTip"))
        Me.chkMmsco.UseVisualStyleBackColor = True
        '
        'cboMicmatching
        '
        Me.cboMicmatching.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboMicmatching.FormattingEnabled = True
        Me.cboMicmatching.Items.AddRange(New Object() {"Disabled", "Mode 1", "Mode 2", "Mode 3", "Mode 4"})
        Me.cboMicmatching.Location = New System.Drawing.Point(115, 96)
        Me.cboMicmatching.Name = "cboMicmatching"
        Me.cboMicmatching.Size = New System.Drawing.Size(121, 21)
        Me.cboMicmatching.TabIndex = 5
        Me.ToolTip1.SetToolTip(Me.cboMicmatching, resources.GetString("cboMicmatching.ToolTip"))
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(8, 100)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(96, 13)
        Me.Label7.TabIndex = 4
        Me.Label7.Text = "Micmatching mode"
        '
        'chkUbsco
        '
        Me.chkUbsco.AutoSize = True
        Me.chkUbsco.Checked = True
        Me.chkUbsco.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkUbsco.Location = New System.Drawing.Point(8, 67)
        Me.chkUbsco.Name = "chkUbsco"
        Me.chkUbsco.Size = New System.Drawing.Size(251, 17)
        Me.chkUbsco.TabIndex = 3
        Me.chkUbsco.Text = "u/b is only used around scene changes (ubsco)"
        Me.ToolTip1.SetToolTip(Me.chkUbsco, resources.GetString("chkUbsco.ToolTip"))
        Me.chkUbsco.UseVisualStyleBackColor = True
        '
        'txtScthresh
        '
        Me.txtScthresh.Location = New System.Drawing.Point(151, 38)
        Me.txtScthresh.Name = "txtScthresh"
        Me.txtScthresh.Size = New System.Drawing.Size(60, 20)
        Me.txtScthresh.TabIndex = 2
        Me.ToolTip1.SetToolTip(Me.txtScthresh, resources.GetString("txtScthresh.ToolTip"))
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(8, 41)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(123, 13)
        Me.Label6.TabIndex = 1
        Me.Label6.Text = "Scene change threshold"
        '
        'chkMChroma
        '
        Me.chkMChroma.AutoSize = True
        Me.chkMChroma.Checked = True
        Me.chkMChroma.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkMChroma.Location = New System.Drawing.Point(8, 14)
        Me.chkMChroma.Name = "chkMChroma"
        Me.chkMChroma.Size = New System.Drawing.Size(312, 17)
        Me.chkMChroma.TabIndex = 0
        Me.chkMChroma.Text = "Chroma is included during the match comparisons (mChroma)"
        Me.ToolTip1.SetToolTip(Me.chkMChroma, resources.GetString("chkMChroma.ToolTip"))
        Me.chkMChroma.UseVisualStyleBackColor = True
        '
        'tabParameters
        '
        Me.tabParameters.Controls.Add(Me.chkUseCustomParams)
        Me.tabParameters.Controls.Add(Me.txtParams)
        Me.tabParameters.Location = New System.Drawing.Point(4, 22)
        Me.tabParameters.Name = "tabParameters"
        Me.tabParameters.Size = New System.Drawing.Size(676, 157)
        Me.tabParameters.TabIndex = 3
        Me.tabParameters.Text = "Parameters"
        Me.tabParameters.UseVisualStyleBackColor = True
        '
        'chkUseCustomParams
        '
        Me.chkUseCustomParams.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkUseCustomParams.AutoSize = True
        Me.chkUseCustomParams.Location = New System.Drawing.Point(535, 128)
        Me.chkUseCustomParams.Name = "chkUseCustomParams"
        Me.chkUseCustomParams.Size = New System.Drawing.Size(138, 17)
        Me.chkUseCustomParams.TabIndex = 1
        Me.chkUseCustomParams.Text = "Use Custom parameters"
        Me.chkUseCustomParams.UseVisualStyleBackColor = True
        '
        'txtParams
        '
        Me.txtParams.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtParams.Location = New System.Drawing.Point(3, 3)
        Me.txtParams.Multiline = True
        Me.txtParams.Name = "txtParams"
        Me.txtParams.ReadOnly = True
        Me.txtParams.Size = New System.Drawing.Size(670, 113)
        Me.txtParams.TabIndex = 0
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(595, 192)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 25)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.Enabled = False
        Me.btnOK.Location = New System.Drawing.Point(514, 192)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 25)
        Me.btnOK.TabIndex = 2
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'ToolTip1
        '
        Me.ToolTip1.AutoPopDelay = 60000
        Me.ToolTip1.InitialDelay = 500
        Me.ToolTip1.ReshowDelay = 100
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        Me.OpenFileDialog1.RestoreDirectory = True
        '
        'chkMarkUBNAsKeyFrame
        '
        Me.chkMarkUBNAsKeyFrame.AutoSize = True
        Me.chkMarkUBNAsKeyFrame.Location = New System.Drawing.Point(10, 84)
        Me.chkMarkUBNAsKeyFrame.Name = "chkMarkUBNAsKeyFrame"
        Me.chkMarkUBNAsKeyFrame.Size = New System.Drawing.Size(207, 17)
        Me.chkMarkUBNAsKeyFrame.TabIndex = 2
        Me.chkMarkUBNAsKeyFrame.Text = "Mark u, b and n matches as key frame"
        Me.chkMarkUBNAsKeyFrame.UseVisualStyleBackColor = True
        '
        'TfmSettingsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(682, 228)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.TabControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "TfmSettingsForm"
        Me.ShowInTaskbar = False
        Me.Text = "TFM Settings"
        Me.TabControl1.ResumeLayout(False)
        Me.tabFile.ResumeLayout(False)
        Me.tabFile.PerformLayout()
        Me.gbTfmAnalysisFileOption.ResumeLayout(False)
        Me.gbTfmAnalysisFileOption.PerformLayout()
        Me.tabBasicSettings.ResumeLayout(False)
        Me.tabBasicSettings.PerformLayout()
        Me.tabAdvancedSettings.ResumeLayout(False)
        Me.tabAdvancedSettings.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.tabParameters.ResumeLayout(False)
        Me.tabParameters.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents tabFile As System.Windows.Forms.TabPage
    Friend WithEvents tabBasicSettings As System.Windows.Forms.TabPage
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnBrowseD2V As System.Windows.Forms.Button
    Friend WithEvents txtD2V As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents tabAdvancedSettings As System.Windows.Forms.TabPage
    Friend WithEvents tabParameters As System.Windows.Forms.TabPage
    Friend WithEvents btnBrowseExistingOverrides As System.Windows.Forms.Button
    Friend WithEvents txtExistingOverrides As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cboMode As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cboFieldOrder As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents cboPPMode As System.Windows.Forms.ComboBox
    Friend WithEvents chkDisplayFrameInfo As System.Windows.Forms.CheckBox
    Friend WithEvents chkMChroma As System.Windows.Forms.CheckBox
    Friend WithEvents chkUbsco As System.Windows.Forms.CheckBox
    Friend WithEvents txtScthresh As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents chkMmsco As System.Windows.Forms.CheckBox
    Friend WithEvents cboMicmatching As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtCthresh As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents txtBlockY As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtBlockX As System.Windows.Forms.TextBox
    Friend WithEvents chkChroma As System.Windows.Forms.CheckBox
    Friend WithEvents txtMI As System.Windows.Forms.TextBox
    Friend WithEvents txtMthresh As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents chkUseCustomParams As System.Windows.Forms.CheckBox
    Friend WithEvents txtParams As System.Windows.Forms.TextBox
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents btnBrowseTfmAnalysisFile As System.Windows.Forms.Button
    Friend WithEvents txtTfmAnalysisFile As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents gbTfmAnalysisFileOption As System.Windows.Forms.GroupBox
    Friend WithEvents chkMarkCombedFramesAsKeyFrame As System.Windows.Forms.CheckBox
    Friend WithEvents chkMarkPossiblyCombedFramesAsKeyFrame As System.Windows.Forms.CheckBox
    Friend WithEvents chkMarkUBNAsKeyFrame As System.Windows.Forms.CheckBox
End Class
