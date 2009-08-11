Namespace Plugins.ChapterEditor
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class ChapterEditorForm
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
            Me.ListBox1 = New System.Windows.Forms.ListBox
            Me.rdoStart = New System.Windows.Forms.RadioButton
            Me.rdoEnd = New System.Windows.Forms.RadioButton
            Me.txtTC = New System.Windows.Forms.TextBox
            Me.btnSet = New System.Windows.Forms.Button
            Me.btnSave = New System.Windows.Forms.Button
            Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog
            Me.txtNewName = New System.Windows.Forms.TextBox
            Me.btnAddBefore = New System.Windows.Forms.Button
            Me.btnAddAfter = New System.Windows.Forms.Button
            Me.btnRemove = New System.Windows.Forms.Button
            Me.SuspendLayout()
            '
            'ListBox1
            '
            Me.ListBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.ListBox1.FormattingEnabled = True
            Me.ListBox1.ItemHeight = 12
            Me.ListBox1.Location = New System.Drawing.Point(12, 12)
            Me.ListBox1.Name = "ListBox1"
            Me.ListBox1.Size = New System.Drawing.Size(299, 220)
            Me.ListBox1.TabIndex = 0
            '
            'rdoStart
            '
            Me.rdoStart.Anchor = System.Windows.Forms.AnchorStyles.Bottom
            Me.rdoStart.AutoSize = True
            Me.rdoStart.Checked = True
            Me.rdoStart.Location = New System.Drawing.Point(46, 323)
            Me.rdoStart.Name = "rdoStart"
            Me.rdoStart.Size = New System.Drawing.Size(53, 16)
            Me.rdoStart.TabIndex = 1
            Me.rdoStart.TabStop = True
            Me.rdoStart.Text = "Start"
            Me.rdoStart.UseVisualStyleBackColor = True
            '
            'rdoEnd
            '
            Me.rdoEnd.Anchor = System.Windows.Forms.AnchorStyles.Bottom
            Me.rdoEnd.AutoSize = True
            Me.rdoEnd.Location = New System.Drawing.Point(105, 323)
            Me.rdoEnd.Name = "rdoEnd"
            Me.rdoEnd.Size = New System.Drawing.Size(41, 16)
            Me.rdoEnd.TabIndex = 2
            Me.rdoEnd.Text = "End"
            Me.rdoEnd.UseVisualStyleBackColor = True
            '
            'txtTC
            '
            Me.txtTC.Anchor = System.Windows.Forms.AnchorStyles.Bottom
            Me.txtTC.Location = New System.Drawing.Point(152, 322)
            Me.txtTC.Name = "txtTC"
            Me.txtTC.Size = New System.Drawing.Size(124, 21)
            Me.txtTC.TabIndex = 3
            '
            'btnSet
            '
            Me.btnSet.Anchor = System.Windows.Forms.AnchorStyles.Bottom
            Me.btnSet.Location = New System.Drawing.Point(76, 351)
            Me.btnSet.Name = "btnSet"
            Me.btnSet.Size = New System.Drawing.Size(75, 23)
            Me.btnSet.TabIndex = 4
            Me.btnSet.Text = "Set"
            Me.btnSet.UseVisualStyleBackColor = True
            '
            'btnSave
            '
            Me.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom
            Me.btnSave.Location = New System.Drawing.Point(172, 351)
            Me.btnSave.Name = "btnSave"
            Me.btnSave.Size = New System.Drawing.Size(75, 23)
            Me.btnSave.TabIndex = 5
            Me.btnSave.Text = "Save As"
            Me.btnSave.UseVisualStyleBackColor = True
            '
            'SaveFileDialog1
            '
            Me.SaveFileDialog1.RestoreDirectory = True
            '
            'txtNewName
            '
            Me.txtNewName.Anchor = System.Windows.Forms.AnchorStyles.Bottom
            Me.txtNewName.Location = New System.Drawing.Point(46, 247)
            Me.txtNewName.Name = "txtNewName"
            Me.txtNewName.Size = New System.Drawing.Size(230, 21)
            Me.txtNewName.TabIndex = 6
            Me.txtNewName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'btnAddBefore
            '
            Me.btnAddBefore.Anchor = System.Windows.Forms.AnchorStyles.Bottom
            Me.btnAddBefore.Location = New System.Drawing.Point(43, 277)
            Me.btnAddBefore.Name = "btnAddBefore"
            Me.btnAddBefore.Size = New System.Drawing.Size(75, 23)
            Me.btnAddBefore.TabIndex = 7
            Me.btnAddBefore.Text = "Add Before"
            Me.btnAddBefore.UseVisualStyleBackColor = True
            '
            'btnAddAfter
            '
            Me.btnAddAfter.Anchor = System.Windows.Forms.AnchorStyles.Bottom
            Me.btnAddAfter.Location = New System.Drawing.Point(124, 277)
            Me.btnAddAfter.Name = "btnAddAfter"
            Me.btnAddAfter.Size = New System.Drawing.Size(75, 23)
            Me.btnAddAfter.TabIndex = 8
            Me.btnAddAfter.Text = "Add After"
            Me.btnAddAfter.UseVisualStyleBackColor = True
            '
            'btnRemove
            '
            Me.btnRemove.Anchor = System.Windows.Forms.AnchorStyles.Bottom
            Me.btnRemove.Location = New System.Drawing.Point(205, 277)
            Me.btnRemove.Name = "btnRemove"
            Me.btnRemove.Size = New System.Drawing.Size(75, 23)
            Me.btnRemove.TabIndex = 9
            Me.btnRemove.Text = "Remove"
            Me.btnRemove.UseVisualStyleBackColor = True
            '
            'ChapterEditorForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(323, 386)
            Me.Controls.Add(Me.btnRemove)
            Me.Controls.Add(Me.btnAddAfter)
            Me.Controls.Add(Me.btnAddBefore)
            Me.Controls.Add(Me.txtNewName)
            Me.Controls.Add(Me.btnSave)
            Me.Controls.Add(Me.btnSet)
            Me.Controls.Add(Me.txtTC)
            Me.Controls.Add(Me.rdoEnd)
            Me.Controls.Add(Me.rdoStart)
            Me.Controls.Add(Me.ListBox1)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
            Me.Name = "ChapterEditorForm"
            Me.ShowInTaskbar = False
            Me.Text = "ChapterEditor"
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
        Friend WithEvents rdoStart As System.Windows.Forms.RadioButton
        Friend WithEvents rdoEnd As System.Windows.Forms.RadioButton
        Friend WithEvents txtTC As System.Windows.Forms.TextBox
        Friend WithEvents btnSet As System.Windows.Forms.Button
        Friend WithEvents btnSave As System.Windows.Forms.Button
        Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
        Friend WithEvents txtNewName As System.Windows.Forms.TextBox
        Friend WithEvents btnAddBefore As System.Windows.Forms.Button
        Friend WithEvents btnAddAfter As System.Windows.Forms.Button
        Friend WithEvents btnRemove As System.Windows.Forms.Button
    End Class


End Namespace