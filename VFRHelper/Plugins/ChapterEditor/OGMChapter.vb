Imports System.IO
Imports System.Text.RegularExpressions
Namespace Plugins.ChapterEditor

    Public Class OGMChapter
        Inherits Chapter

        Public Overrides Sub Parse(ByVal content As String)
            content = content.Replace(ControlChars.CrLf & ControlChars.CrLf, ControlChars.CrLf)
            Entries.Clear()
            Using sr As New StringReader(content)
                Dim index As Integer
                Do
                    index += 1
                    Dim timeLine As String = "", nameLine As String = ""
                    While True
                        timeLine = sr.ReadLine()
                        If timeLine Is Nothing Then
                            Exit Do
                        End If
                        timeLine = timeLine.Trim()
                        If Not String.IsNullOrEmpty(timeLine) Then
                            Exit While
                        End If
                    End While
                    While True
                        nameLine = sr.ReadLine()
                        If nameLine Is Nothing Then
                            Exit Do
                        End If
                        nameLine = nameLine.Trim()
                        If Not String.IsNullOrEmpty(nameLine) Then
                            Exit While
                        End If
                    End While
                    If Not (timeLine.ToUpperInvariant().StartsWith(String.Format("CHAPTER{0:D2}=", index)) AndAlso nameLine.ToUpperInvariant().StartsWith(String.Format("CHAPTER{0:D2}NAME=", index))) Then
                        Throw New Exception("Not a valid OGM chapter file")
                    End If
                    Dim tc As String = timeLine.Split("="c)(1)
                    Dim time As TimeSpan
                    If Not TimeSpan.TryParse(tc, time) Then
                        Throw New Exception("Not a valid OGM chapter file")
                    End If
                    Entries.Add(New ChapterEntry(nameLine.Split("="c)(1), time))
                Loop
            End Using
        End Sub

        Public Overrides Function ToString() As String
            Dim builder As New System.Text.StringBuilder, index As Integer
            For Each entry As ChapterEntry In Entries
                index += 1
                builder.AppendFormat("CHAPTER{0:D2}={1}{2}", index, Utils.TimeSpanToOGMTimecode(entry.Start), ControlChars.CrLf)
                builder.AppendFormat("CHAPTER{0:D2}NAME={1}{2}", index, entry.Name, ControlChars.CrLf)
            Next
            Return builder.ToString()
        End Function
    End Class

End Namespace