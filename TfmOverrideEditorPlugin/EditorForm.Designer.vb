<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EditorForm
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
        Me.lstOverrideEntries = New System.Windows.Forms.ListBox()
        Me.gbMatch = New System.Windows.Forms.GroupBox()
        Me.rdoMatchU = New System.Windows.Forms.RadioButton()
        Me.rdoMatchB = New System.Windows.Forms.RadioButton()
        Me.rdoMatchN = New System.Windows.Forms.RadioButton()
        Me.rdoMatchC = New System.Windows.Forms.RadioButton()
        Me.rdoMatchP = New System.Windows.Forms.RadioButton()
        Me.rdoMatchNotSpecified = New System.Windows.Forms.RadioButton()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.chkCombed = New System.Windows.Forms.CheckBox()
        Me.gbFrameRange = New System.Windows.Forms.GroupBox()
        Me.btnSetRangeEnd = New System.Windows.Forms.Button()
        Me.btnSetRangeStart = New System.Windows.Forms.Button()
        Me.chkRangeCombed = New System.Windows.Forms.CheckBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtPattern = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnApplyRange = New System.Windows.Forms.Button()
        Me.txtFrameRangeEnd = New System.Windows.Forms.TextBox()
        Me.txtFrameRangeStart = New System.Windows.Forms.TextBox()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnSettings = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lblFrameInfo = New System.Windows.Forms.Label()
        Me.gbMatch.SuspendLayout()
        Me.gbFrameRange.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'lstOverrideEntries
        '
        Me.lstOverrideEntries.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstOverrideEntries.FormattingEnabled = True
        Me.lstOverrideEntries.Location = New System.Drawing.Point(12, 13)
        Me.lstOverrideEntries.Name = "lstOverrideEntries"
        Me.lstOverrideEntries.Size = New System.Drawing.Size(180, 290)
        Me.lstOverrideEntries.TabIndex = 0
        Me.ToolTip1.SetToolTip(Me.lstOverrideEntries, "Double click to remove selected entry")
        '
        'gbMatch
        '
        Me.gbMatch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbMatch.Controls.Add(Me.rdoMatchU)
        Me.gbMatch.Controls.Add(Me.rdoMatchB)
        Me.gbMatch.Controls.Add(Me.rdoMatchN)
        Me.gbMatch.Controls.Add(Me.rdoMatchC)
        Me.gbMatch.Controls.Add(Me.rdoMatchP)
        Me.gbMatch.Controls.Add(Me.rdoMatchNotSpecified)
        Me.gbMatch.Location = New System.Drawing.Point(12, 413)
        Me.gbMatch.Name = "gbMatch"
        Me.gbMatch.Size = New System.Drawing.Size(180, 77)
        Me.gbMatch.TabIndex = 1
        Me.gbMatch.TabStop = False
        Me.gbMatch.Text = "Match"
        '
        'rdoMatchU
        '
        Me.rdoMatchU.AutoSize = True
        Me.rdoMatchU.Location = New System.Drawing.Point(146, 46)
        Me.rdoMatchU.Name = "rdoMatchU"
        Me.rdoMatchU.Size = New System.Drawing.Size(31, 17)
        Me.rdoMatchU.TabIndex = 5
        Me.rdoMatchU.Text = "u"
        Me.ToolTip1.SetToolTip(Me.rdoMatchU, "Match to next field (opposite parity)")
        Me.rdoMatchU.UseVisualStyleBackColor = True
        '
        'rdoMatchB
        '
        Me.rdoMatchB.AutoSize = True
        Me.rdoMatchB.Location = New System.Drawing.Point(111, 46)
        Me.rdoMatchB.Name = "rdoMatchB"
        Me.rdoMatchB.Size = New System.Drawing.Size(31, 17)
        Me.rdoMatchB.TabIndex = 4
        Me.rdoMatchB.Text = "b"
        Me.ToolTip1.SetToolTip(Me.rdoMatchB, "Match to previous field (opposite parity)")
        Me.rdoMatchB.UseVisualStyleBackColor = True
        '
        'rdoMatchN
        '
        Me.rdoMatchN.AutoSize = True
        Me.rdoMatchN.Location = New System.Drawing.Point(76, 46)
        Me.rdoMatchN.Name = "rdoMatchN"
        Me.rdoMatchN.Size = New System.Drawing.Size(31, 17)
        Me.rdoMatchN.TabIndex = 3
        Me.rdoMatchN.Text = "n"
        Me.ToolTip1.SetToolTip(Me.rdoMatchN, "Match to next field")
        Me.rdoMatchN.UseVisualStyleBackColor = True
        '
        'rdoMatchC
        '
        Me.rdoMatchC.AutoSize = True
        Me.rdoMatchC.Location = New System.Drawing.Point(41, 46)
        Me.rdoMatchC.Name = "rdoMatchC"
        Me.rdoMatchC.Size = New System.Drawing.Size(31, 17)
        Me.rdoMatchC.TabIndex = 2
        Me.rdoMatchC.Text = "c"
        Me.ToolTip1.SetToolTip(Me.rdoMatchC, "Match to current field")
        Me.rdoMatchC.UseVisualStyleBackColor = True
        '
        'rdoMatchP
        '
        Me.rdoMatchP.AutoSize = True
        Me.rdoMatchP.Location = New System.Drawing.Point(6, 46)
        Me.rdoMatchP.Name = "rdoMatchP"
        Me.rdoMatchP.Size = New System.Drawing.Size(31, 17)
        Me.rdoMatchP.TabIndex = 1
        Me.rdoMatchP.Text = "p"
        Me.ToolTip1.SetToolTip(Me.rdoMatchP, "Match to previous field")
        Me.rdoMatchP.UseVisualStyleBackColor = True
        '
        'rdoMatchNotSpecified
        '
        Me.rdoMatchNotSpecified.AutoSize = True
        Me.rdoMatchNotSpecified.Checked = True
        Me.rdoMatchNotSpecified.Location = New System.Drawing.Point(6, 22)
        Me.rdoMatchNotSpecified.Name = "rdoMatchNotSpecified"
        Me.rdoMatchNotSpecified.Size = New System.Drawing.Size(59, 17)
        Me.rdoMatchNotSpecified.TabIndex = 0
        Me.rdoMatchNotSpecified.TabStop = True
        Me.rdoMatchNotSpecified.Text = "Default"
        Me.ToolTip1.SetToolTip(Me.rdoMatchNotSpecified, "Use TFM's decision")
        Me.rdoMatchNotSpecified.UseVisualStyleBackColor = True
        '
        'chkCombed
        '
        Me.chkCombed.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkCombed.AutoSize = True
        Me.chkCombed.Checked = True
        Me.chkCombed.CheckState = System.Windows.Forms.CheckState.Indeterminate
        Me.chkCombed.Location = New System.Drawing.Point(18, 496)
        Me.chkCombed.Name = "chkCombed"
        Me.chkCombed.Size = New System.Drawing.Size(65, 17)
        Me.chkCombed.TabIndex = 2
        Me.chkCombed.Text = "Combed"
        Me.chkCombed.ThreeState = True
        Me.chkCombed.UseVisualStyleBackColor = True
        '
        'gbFrameRange
        '
        Me.gbFrameRange.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbFrameRange.Controls.Add(Me.btnSetRangeEnd)
        Me.gbFrameRange.Controls.Add(Me.btnSetRangeStart)
        Me.gbFrameRange.Controls.Add(Me.chkRangeCombed)
        Me.gbFrameRange.Controls.Add(Me.Label3)
        Me.gbFrameRange.Controls.Add(Me.txtPattern)
        Me.gbFrameRange.Controls.Add(Me.Label2)
        Me.gbFrameRange.Controls.Add(Me.Label1)
        Me.gbFrameRange.Controls.Add(Me.btnApplyRange)
        Me.gbFrameRange.Controls.Add(Me.txtFrameRangeEnd)
        Me.gbFrameRange.Controls.Add(Me.txtFrameRangeStart)
        Me.gbFrameRange.Location = New System.Drawing.Point(12, 528)
        Me.gbFrameRange.Name = "gbFrameRange"
        Me.gbFrameRange.Size = New System.Drawing.Size(180, 169)
        Me.gbFrameRange.TabIndex = 4
        Me.gbFrameRange.TabStop = False
        Me.gbFrameRange.Text = "Frame Range"
        '
        'btnSetRangeEnd
        '
        Me.btnSetRangeEnd.Location = New System.Drawing.Point(127, 51)
        Me.btnSetRangeEnd.Name = "btnSetRangeEnd"
        Me.btnSetRangeEnd.Size = New System.Drawing.Size(47, 23)
        Me.btnSetRangeEnd.TabIndex = 11
        Me.btnSetRangeEnd.Text = "Set"
        Me.btnSetRangeEnd.UseVisualStyleBackColor = True
        '
        'btnSetRangeStart
        '
        Me.btnSetRangeStart.Location = New System.Drawing.Point(127, 22)
        Me.btnSetRangeStart.Name = "btnSetRangeStart"
        Me.btnSetRangeStart.Size = New System.Drawing.Size(47, 23)
        Me.btnSetRangeStart.TabIndex = 10
        Me.btnSetRangeStart.Text = "Set"
        Me.btnSetRangeStart.UseVisualStyleBackColor = True
        '
        'chkRangeCombed
        '
        Me.chkRangeCombed.AutoSize = True
        Me.chkRangeCombed.Checked = True
        Me.chkRangeCombed.CheckState = System.Windows.Forms.CheckState.Indeterminate
        Me.chkRangeCombed.Location = New System.Drawing.Point(8, 109)
        Me.chkRangeCombed.Name = "chkRangeCombed"
        Me.chkRangeCombed.Size = New System.Drawing.Size(65, 17)
        Me.chkRangeCombed.TabIndex = 3
        Me.chkRangeCombed.Text = "Combed"
        Me.chkRangeCombed.ThreeState = True
        Me.chkRangeCombed.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 83)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(41, 13)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Pattern"
        '
        'txtPattern
        '
        Me.txtPattern.Location = New System.Drawing.Point(60, 80)
        Me.txtPattern.Name = "txtPattern"
        Me.txtPattern.Size = New System.Drawing.Size(114, 20)
        Me.txtPattern.TabIndex = 2
        Me.txtPattern.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(27, 54)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(26, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "End"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(15, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(29, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Start"
        '
        'btnApplyRange
        '
        Me.btnApplyRange.AutoSize = True
        Me.btnApplyRange.Location = New System.Drawing.Point(17, 133)
        Me.btnApplyRange.Name = "btnApplyRange"
        Me.btnApplyRange.Size = New System.Drawing.Size(135, 25)
        Me.btnApplyRange.TabIndex = 4
        Me.btnApplyRange.Text = "Apply to frame range"
        Me.btnApplyRange.UseVisualStyleBackColor = True
        '
        'txtFrameRangeEnd
        '
        Me.txtFrameRangeEnd.Location = New System.Drawing.Point(60, 51)
        Me.txtFrameRangeEnd.Name = "txtFrameRangeEnd"
        Me.txtFrameRangeEnd.Size = New System.Drawing.Size(61, 20)
        Me.txtFrameRangeEnd.TabIndex = 1
        Me.txtFrameRangeEnd.Text = "0"
        Me.txtFrameRangeEnd.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtFrameRangeStart
        '
        Me.txtFrameRangeStart.Location = New System.Drawing.Point(60, 22)
        Me.txtFrameRangeStart.Name = "txtFrameRangeStart"
        Me.txtFrameRangeStart.Size = New System.Drawing.Size(61, 20)
        Me.txtFrameRangeStart.TabIndex = 0
        Me.txtFrameRangeStart.Text = "0"
        Me.txtFrameRangeStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.Location = New System.Drawing.Point(18, 703)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(75, 25)
        Me.btnSave.TabIndex = 5
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnSettings
        '
        Me.btnSettings.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSettings.Location = New System.Drawing.Point(112, 703)
        Me.btnSettings.Name = "btnSettings"
        Me.btnSettings.Size = New System.Drawing.Size(75, 25)
        Me.btnSettings.TabIndex = 6
        Me.btnSettings.Text = "Settings"
        Me.btnSettings.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.lblFrameInfo)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 312)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(180, 95)
        Me.GroupBox1.TabIndex = 7
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Info"
        '
        'lblFrameInfo
        '
        Me.lblFrameInfo.Location = New System.Drawing.Point(3, 16)
        Me.lblFrameInfo.Name = "lblFrameInfo"
        Me.lblFrameInfo.Size = New System.Drawing.Size(171, 76)
        Me.lblFrameInfo.TabIndex = 0
        '
        'EditorForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(204, 741)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.btnSettings)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.gbFrameRange)
        Me.Controls.Add(Me.chkCombed)
        Me.Controls.Add(Me.gbMatch)
        Me.Controls.Add(Me.lstOverrideEntries)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "EditorForm"
        Me.ShowInTaskbar = False
        Me.Text = "TFM Overrides Editor"
        Me.gbMatch.ResumeLayout(False)
        Me.gbMatch.PerformLayout()
        Me.gbFrameRange.ResumeLayout(False)
        Me.gbFrameRange.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lstOverrideEntries As System.Windows.Forms.ListBox
    Friend WithEvents gbMatch As System.Windows.Forms.GroupBox
    Friend WithEvents rdoMatchN As System.Windows.Forms.RadioButton
    Friend WithEvents rdoMatchC As System.Windows.Forms.RadioButton
    Friend WithEvents rdoMatchP As System.Windows.Forms.RadioButton
    Friend WithEvents rdoMatchNotSpecified As System.Windows.Forms.RadioButton
    Friend WithEvents rdoMatchU As System.Windows.Forms.RadioButton
    Friend WithEvents rdoMatchB As System.Windows.Forms.RadioButton
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents chkCombed As System.Windows.Forms.CheckBox
    Friend WithEvents gbFrameRange As System.Windows.Forms.GroupBox
    Friend WithEvents txtFrameRangeEnd As System.Windows.Forms.TextBox
    Friend WithEvents txtFrameRangeStart As System.Windows.Forms.TextBox
    Friend WithEvents btnApplyRange As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnSettings As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtPattern As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents chkRangeCombed As System.Windows.Forms.CheckBox
    Friend WithEvents btnSetRangeEnd As System.Windows.Forms.Button
    Friend WithEvents btnSetRangeStart As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents lblFrameInfo As System.Windows.Forms.Label
End Class
