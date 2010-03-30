<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
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
        Me.components = New System.ComponentModel.Container
        Me.btnOpenTC = New System.Windows.Forms.Button
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.NumericUpDown1 = New System.Windows.Forms.NumericUpDown
        Me.btnOpenVideo = New System.Windows.Forms.Button
        Me.picFrame = New System.Windows.Forms.PictureBox
        Me.TrackBar1 = New System.Windows.Forms.TrackBar
        Me.txtFrameType = New System.Windows.Forms.TextBox
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.txtTimecode = New System.Windows.Forms.MaskedTextBox
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog
        Me.tabPlugins = New System.Windows.Forms.TabControl
        Me.gbPlugins = New System.Windows.Forms.GroupBox
        Me.btnTogglePlugins = New System.Windows.Forms.Button
        CType(Me.NumericUpDown1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picFrame, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TrackBar1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbPlugins.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnOpenTC
        '
        Me.btnOpenTC.AllowDrop = True
        Me.btnOpenTC.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnOpenTC.Location = New System.Drawing.Point(141, 596)
        Me.btnOpenTC.Name = "btnOpenTC"
        Me.btnOpenTC.Size = New System.Drawing.Size(75, 25)
        Me.btnOpenTC.TabIndex = 0
        Me.btnOpenTC.Text = "Open TC"
        Me.btnOpenTC.UseVisualStyleBackColor = True
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.Filter = "V2 Timecode File|*.txt"
        Me.OpenFileDialog1.RestoreDirectory = True
        '
        'NumericUpDown1
        '
        Me.NumericUpDown1.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.NumericUpDown1.Enabled = False
        Me.NumericUpDown1.Location = New System.Drawing.Point(372, 597)
        Me.NumericUpDown1.Name = "NumericUpDown1"
        Me.NumericUpDown1.Size = New System.Drawing.Size(120, 20)
        Me.NumericUpDown1.TabIndex = 1
        '
        'btnOpenVideo
        '
        Me.btnOpenVideo.AllowDrop = True
        Me.btnOpenVideo.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnOpenVideo.Location = New System.Drawing.Point(30, 596)
        Me.btnOpenVideo.Name = "btnOpenVideo"
        Me.btnOpenVideo.Size = New System.Drawing.Size(75, 25)
        Me.btnOpenVideo.TabIndex = 3
        Me.btnOpenVideo.Text = "Open Video"
        Me.btnOpenVideo.UseVisualStyleBackColor = True
        '
        'picFrame
        '
        Me.picFrame.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.picFrame.Location = New System.Drawing.Point(12, 13)
        Me.picFrame.Name = "picFrame"
        Me.picFrame.Size = New System.Drawing.Size(640, 520)
        Me.picFrame.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.picFrame.TabIndex = 5
        Me.picFrame.TabStop = False
        '
        'TrackBar1
        '
        Me.TrackBar1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TrackBar1.AutoSize = False
        Me.TrackBar1.Enabled = False
        Me.TrackBar1.LargeChange = 0
        Me.TrackBar1.Location = New System.Drawing.Point(12, 540)
        Me.TrackBar1.Name = "TrackBar1"
        Me.TrackBar1.Size = New System.Drawing.Size(640, 49)
        Me.TrackBar1.TabIndex = 6
        Me.TrackBar1.TickStyle = System.Windows.Forms.TickStyle.None
        '
        'txtFrameType
        '
        Me.txtFrameType.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.txtFrameType.Location = New System.Drawing.Point(604, 597)
        Me.txtFrameType.Name = "txtFrameType"
        Me.txtFrameType.ReadOnly = True
        Me.txtFrameType.Size = New System.Drawing.Size(25, 20)
        Me.txtFrameType.TabIndex = 7
        '
        'Timer1
        '
        Me.Timer1.Interval = 1
        '
        'txtTimecode
        '
        Me.txtTimecode.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.txtTimecode.AsciiOnly = True
        Me.txtTimecode.CutCopyMaskFormat = System.Windows.Forms.MaskFormat.IncludePromptAndLiterals
        Me.txtTimecode.Enabled = False
        Me.txtTimecode.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite
        Me.txtTimecode.Location = New System.Drawing.Point(498, 597)
        Me.txtTimecode.Mask = "00:00:00.000"
        Me.txtTimecode.Name = "txtTimecode"
        Me.txtTimecode.PromptChar = Global.Microsoft.VisualBasic.ChrW(48)
        Me.txtTimecode.Size = New System.Drawing.Size(100, 20)
        Me.txtTimecode.TabIndex = 9
        Me.txtTimecode.TextMaskFormat = System.Windows.Forms.MaskFormat.IncludePromptAndLiterals
        '
        'SaveFileDialog1
        '
        Me.SaveFileDialog1.RestoreDirectory = True
        '
        'tabPlugins
        '
        Me.tabPlugins.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabPlugins.Location = New System.Drawing.Point(3, 16)
        Me.tabPlugins.Name = "tabPlugins"
        Me.tabPlugins.SelectedIndex = 0
        Me.tabPlugins.Size = New System.Drawing.Size(634, 62)
        Me.tabPlugins.TabIndex = 10
        '
        'gbPlugins
        '
        Me.gbPlugins.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbPlugins.Controls.Add(Me.tabPlugins)
        Me.gbPlugins.Location = New System.Drawing.Point(12, 635)
        Me.gbPlugins.Name = "gbPlugins"
        Me.gbPlugins.Size = New System.Drawing.Size(640, 81)
        Me.gbPlugins.TabIndex = 11
        Me.gbPlugins.TabStop = False
        Me.gbPlugins.Text = "Plugins"
        Me.gbPlugins.Visible = False
        '
        'btnTogglePlugins
        '
        Me.btnTogglePlugins.AllowDrop = True
        Me.btnTogglePlugins.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnTogglePlugins.Location = New System.Drawing.Point(252, 596)
        Me.btnTogglePlugins.Name = "btnTogglePlugins"
        Me.btnTogglePlugins.Size = New System.Drawing.Size(100, 25)
        Me.btnTogglePlugins.TabIndex = 12
        Me.btnTogglePlugins.Text = "Load Plugin"
        Me.btnTogglePlugins.UseVisualStyleBackColor = True
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(664, 632)
        Me.Controls.Add(Me.btnTogglePlugins)
        Me.Controls.Add(Me.gbPlugins)
        Me.Controls.Add(Me.txtTimecode)
        Me.Controls.Add(Me.txtFrameType)
        Me.Controls.Add(Me.TrackBar1)
        Me.Controls.Add(Me.picFrame)
        Me.Controls.Add(Me.btnOpenVideo)
        Me.Controls.Add(Me.NumericUpDown1)
        Me.Controls.Add(Me.btnOpenTC)
        Me.DoubleBuffered = True
        Me.KeyPreview = True
        Me.Name = "MainForm"
        Me.Text = "VFRHelper"
        CType(Me.NumericUpDown1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picFrame, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TrackBar1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbPlugins.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnOpenTC As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents NumericUpDown1 As System.Windows.Forms.NumericUpDown
    Friend WithEvents btnOpenVideo As System.Windows.Forms.Button
    Friend WithEvents picFrame As System.Windows.Forms.PictureBox
    Friend WithEvents TrackBar1 As System.Windows.Forms.TrackBar
    Friend WithEvents txtFrameType As System.Windows.Forms.TextBox
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents txtTimecode As System.Windows.Forms.MaskedTextBox
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents tabPlugins As System.Windows.Forms.TabControl
    Friend WithEvents gbPlugins As System.Windows.Forms.GroupBox
    Friend WithEvents btnTogglePlugins As System.Windows.Forms.Button

End Class
