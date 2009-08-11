Public NotInheritable Class Utils
    Private Sub New()

    End Sub

    Shared Function GenerateTimecodes(ByVal video As AviFile.VideoStream) As List(Of String)
        If video Is Nothing Then
            Throw New ArgumentNullException("video is null.", "video")
        End If
        Dim timecodes As New List(Of String)
        Dim msPerFrame As Double = 1 / video.FrameRate * 1000
        For i As Integer = 0 To video.CountFrames - 1
            timecodes.Add(MillisecondToTimecodeString(msPerFrame * i))
        Next
        Return timecodes
    End Function

    Shared Function MillisecondToTimecodeString(ByVal ms As Double) As String
        Dim time As TimeSpan = TimeSpan.FromMilliseconds(Math.Round(ms))
        Return String.Format("{0:D2}:{1:D2}:{2:D2}.{3:D3}", time.Hours, time.Minutes, time.Seconds, time.Milliseconds)
    End Function

    Shared Function TimeSpanToOGMTimecode(ByVal ts As TimeSpan) As String
        Dim r As String = ts.ToString()
        If r.Contains(".") Then
            Return r.Substring(0, r.Length - 4)
        Else
            Return r & ".000"
        End If
    End Function
    Shared Function TimeSpanToMKVTimecode(ByVal ts As TimeSpan) As String
        Dim r As String = ts.ToString()
        If r.Contains(".") Then
            Return r & "00"
        Else
            Return r & ".000000000"
        End If
    End Function
End Class
