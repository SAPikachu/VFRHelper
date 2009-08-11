Namespace Plugins.ChapterEditor
    Public Class ChapterEditorForm
        Dim _chap As Chapter
        Dim _template As ChapterEntry
        Private Sub btnSet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSet.Click
            Dim tc As TimeSpan
            If Not TimeSpan.TryParse(txtTC.Text, tc) Then
                MessageBox.Show("Invalid timecode", "Error")
                Return
            End If
            SetTimecode(tc)
        End Sub

        Private Sub RefreshDataSource()
            CType(ListBox1.BindingContext(ListBox1.DataSource), CurrencyManager).Refresh()
        End Sub
        Public Sub SetTimecode(ByVal tc As TimeSpan)
            If ListBox1.SelectedItem Is Nothing Then
                MessageBox.Show("Please select a chapter entry first")
                Return
            End If
            Dim entry As ChapterEntry = CType(ListBox1.SelectedItem, ChapterEntry)
            If rdoStart.Checked Then
                entry.Start = tc
            Else
                entry.End = tc
            End If
            Dim index As Integer = ListBox1.SelectedIndex + 1
            RefreshDataSource()
            If rdoEnd.Checked OrElse rdoEnd.Enabled = False Then
                If index >= ListBox1.Items.Count Then
                    ListBox1.SelectedIndex = -1
                    btnSave.Enabled = True
                Else
                    ListBox1.SelectedIndex = index
                End If
                rdoStart.Checked = True
            Else
                rdoEnd.Checked = True
            End If
        End Sub
        Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
            If TypeOf _chap Is MKVChapter Then
                SaveFileDialog1.Filter = "MKV chapter file|*.xml"
            ElseIf TypeOf _chap Is OGMChapter Then
                SaveFileDialog1.Filter = "OGM chapter file|*.txt"
            End If
            SaveFileDialog1.FileName = ""
            If SaveFileDialog1.ShowDialog() = Windows.Forms.DialogResult.Cancel Then
                Return
            End If
            _chap.Save(SaveFileDialog1.FileName)
            Me.Close()
        End Sub
        Public Sub Open(ByVal chap As Chapter)
            For Each entry As ChapterEntry In chap.Entries
                entry.Start = TimeSpan.Zero
                entry.End = TimeSpan.Zero
            Next
            _chap = chap
            rdoStart.Checked = True
            If TypeOf chap Is OGMChapter Then
                rdoEnd.Enabled = False
            Else
                rdoEnd.Enabled = True
            End If
            _template = If(chap.Entries.Count > 0, CType(_chap.Entries(0).Clone(), ChapterEntry), New ChapterEntry)
            ListBox1.DataSource = _chap.Entries
            If _chap.Entries.Count > 0 Then
                ListBox1.SelectedIndex = 0
            End If
            Me.Show()
            My.Forms.MainForm.BringToFront()
        End Sub
        Sub UpdateView()
            If ListBox1.SelectedIndex > -1 Then
                Dim entry As ChapterEntry = CType(ListBox1.SelectedItem, ChapterEntry), newVal As TimeSpan
                If rdoStart.Checked Then
                    newVal = entry.Start
                Else
                    newVal = entry.End
                End If
                If newVal = TimeSpan.Zero AndAlso ListBox1.SelectedIndex <> 0 Then
                    txtTC.Text = ""
                Else
                    txtTC.Text = newVal.ToString()
                End If
            End If
        End Sub
        Private Sub OnUpdateView(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListBox1.SelectedIndexChanged, rdoStart.CheckedChanged, rdoEnd.CheckedChanged
            UpdateView()
        End Sub

        Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddBefore.Click, btnAddAfter.Click
            If txtNewName.Text = "" Then
                MessageBox.Show("Please enter a name for the new chapter")
                Return
            End If
            Dim index As Integer = ListBox1.SelectedIndex
            If sender Is btnAddBefore Then
                index = If(index = -1, 0, index)
            Else
                index = If(index = -1, _chap.Entries.Count, index + 1)
            End If
            Dim newObj As ChapterEntry = CType(_template.Clone(), ChapterEntry)
            newObj.Name = txtNewName.Text
            _chap.Entries.Insert(index, newObj)
            RefreshDataSource()
            ListBox1.SelectedIndex = index
            txtNewName.Focus()
            txtNewName.Select(0, txtNewName.Text.Length)
        End Sub

        Private Sub btnRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemove.Click
            If ListBox1.SelectedIndex = -1 Then
                Return
            End If
            _chap.Entries.RemoveAt(ListBox1.SelectedIndex)
            RefreshDataSource()
        End Sub

        Private Sub txtNewName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtNewName.KeyPress
            If e.KeyChar = ControlChars.Cr Then
                e.Handled = True
            End If
        End Sub

        Private Sub txtNewName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtNewName.KeyUp
            If e.KeyCode = Keys.Enter Then
                e.SuppressKeyPress = True
                btnAddAfter.PerformClick()
            End If
        End Sub

        Public Sub MoveUp()
            If ListBox1.SelectedIndex > 0 Then
                ListBox1.SelectedIndex -= 1
                UpdateView()
            End If
        End Sub
        Public Sub MoveDown()
            If ListBox1.SelectedIndex > -1 AndAlso ListBox1.SelectedIndex < _chap.Entries.Count - 1 Then
                ListBox1.SelectedIndex += 1
                UpdateView()
            End If
        End Sub
    End Class

End Namespace