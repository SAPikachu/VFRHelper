
Public Class Form1

    Private Sub txtTemplate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTemplate.TextChanged
        txtPending.Text = txtTemplate.Text
    End Sub

End Class