
Namespace VideoProviders

    Public Interface IVideoProvider
        Inherits IDisposable

        Sub SetHelper(ByVal helper As IVideoProviderHelper)
        Sub Open(ByVal filePath As String)
        Sub GetFrame(ByRef output As Bitmap)
        Function GetTimecodeOfCurrentFrame() As String
        Function IsSuitableTimecodes(ByVal timecodes As Timecodes) As Boolean
        Sub SetTimecodes(ByVal timecodes As Timecodes)
        Function SeekToNextKeyFrame() As Boolean
        Function SeekToPrevKeyFrame() As Boolean
        Function SeekToNextFrame() As Boolean
        Function SeekToPrevFrame() As Boolean
        Function SeekTo(ByVal frameNumber As Integer) As Boolean
        ReadOnly Property Timecodes() As Timecodes
        ReadOnly Property CurrentFrameNumber() As Integer
        ReadOnly Property CurrentFrameType() As FrameType
        ReadOnly Property FrameCount() As Integer
        ReadOnly Property FrameRate() As Double
        ReadOnly Property VideoSize() As Size
    End Interface

End Namespace