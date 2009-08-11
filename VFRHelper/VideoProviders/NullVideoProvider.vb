Namespace VideoProviders

    Public Class NullVideoProvider
        Inherits VideoProviderBase
        Dim _frameCount As Integer

        Public Overrides ReadOnly Property CurrentFrameType() As FrameType
            Get
                Return FrameType.Key
            End Get
        End Property

        Public Overrides ReadOnly Property FrameCount() As Integer
            Get
                Return _frameCount
            End Get
        End Property

        Public Overrides ReadOnly Property FrameRate() As Double
            Get
                Return 25
            End Get
        End Property

        Public Overrides Sub GetFrame(ByRef output As System.Drawing.Bitmap)
            output = Nothing
        End Sub

        Public Overrides Sub Open(ByVal filePath As String)
            'do nothing
        End Sub

        Public Overrides Function SeekToNextKeyFrame() As Boolean
            Return SeekTo(CurrentFrameNumber + 1)
        End Function

        Public Overrides Function SeekToPrevKeyFrame() As Boolean
            Return SeekTo(CurrentFrameNumber - 1)
        End Function

        Protected Overrides Function IsSuitableTimecodes(ByVal timecodes As Timecodes) As Boolean
            Return True
        End Function

        Public Overrides Sub SetTimecodes(ByVal timecodes As Timecodes)
            _frameCount = timecodes.Count
            MyBase.SetTimecodes(timecodes)
        End Sub
    End Class

End Namespace