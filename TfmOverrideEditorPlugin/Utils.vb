Class Utils
    Private Shared Function CharToCombed(ch As Char) As Boolean
        If ch = "+"c Then
            Return True
        ElseIf ch = "-"c Then
            Return False
        End If
        Throw New ArgumentOutOfRangeException("ch", "ch must be either + or -")
    End Function

    Public Shared Function CombedStringToArray(s As String) As Boolean()
        If String.IsNullOrEmpty(s) Then
            Return Nothing
        End If
        Return Array.ConvertAll(s.ToCharArray(), Function(ch) CharToCombed(ch))
    End Function

    Public Shared Function CombedArrayToString(combed As Boolean()) As String
        If combed Is Nothing Then
            Return Nothing
        End If
        Return New String(Array.ConvertAll(combed, Function(c) If(c, "+"c, "-"c)))
    End Function

    Private Sub New()

    End Sub
End Class
