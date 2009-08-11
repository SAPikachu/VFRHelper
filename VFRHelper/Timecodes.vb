Imports System.IO

Public Class Timecodes
    Inherits List(Of String)

    Private Const SIGNATURE_TIMECODEV2 As String = "# timecode format v2"

    Shared Function Generate(ByVal fps As Double, ByVal frameCount As Integer) As Timecodes
        Dim timecodes As New Timecodes
        Dim msPerFrame As Double = 1 / fps * 1000
        For i As Integer = 0 To frameCount - 1
            timecodes.Add(Utils.MillisecondToTimecodeString(msPerFrame * i))
        Next
        Return timecodes
    End Function

    Shared Function OpenV2(ByVal fileName As String) As Timecodes
        Using sr As New StreamReader(fileName)
            If sr.ReadLine().Trim().ToLowerInvariant() <> SIGNATURE_TIMECODEV2 Then
                Return Nothing
            End If
            Dim list As New Timecodes
            Dim currentLine As String
            Do
                currentLine = sr.ReadLine().Trim()
                If currentLine.StartsWith("#") OrElse String.IsNullOrEmpty(currentLine) Then
                    Continue Do
                End If
                Dim timecode As Double
                If Not Double.TryParse(currentLine, timecode) Then
                    Return Nothing
                End If
                list.Add(Utils.MillisecondToTimecodeString(timecode))
            Loop Until sr.EndOfStream
            Return list
        End Using
    End Function
    Private Sub New()

    End Sub
End Class
