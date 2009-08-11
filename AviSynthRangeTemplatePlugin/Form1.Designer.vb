<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.txtScript = New System.Windows.Forms.TextBox
        Me.txtTemplate = New System.Windows.Forms.TextBox
        Me.txtPending = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'txtScript
        '
        Me.txtScript.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtScript.Location = New System.Drawing.Point(12, 12)
        Me.txtScript.Multiline = True
        Me.txtScript.Name = "txtScript"
        Me.txtScript.Size = New System.Drawing.Size(260, 118)
        Me.txtScript.TabIndex = 0
        '
        'txtTemplate
        '
        Me.txtTemplate.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTemplate.Location = New System.Drawing.Point(12, 136)
        Me.txtTemplate.Multiline = True
        Me.txtTemplate.Name = "txtTemplate"
        Me.txtTemplate.Size = New System.Drawing.Size(260, 54)
        Me.txtTemplate.TabIndex = 1
        Me.txtTemplate.Text = "FR(###,###,"""")"
        '
        'txtPending
        '
        Me.txtPending.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPending.Location = New System.Drawing.Point(12, 196)
        Me.txtPending.Multiline = True
        Me.txtPending.Name = "txtPending"
        Me.txtPending.ReadOnly = True
        Me.txtPending.Size = New System.Drawing.Size(260, 54)
        Me.txtPending.TabIndex = 2
        Me.txtPending.Text = "FR(###,###,"""")"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 262)
        Me.Controls.Add(Me.txtPending)
        Me.Controls.Add(Me.txtTemplate)
        Me.Controls.Add(Me.txtScript)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtScript As System.Windows.Forms.TextBox
    Friend WithEvents txtTemplate As System.Windows.Forms.TextBox
    Friend WithEvents txtPending As System.Windows.Forms.TextBox
End Class
