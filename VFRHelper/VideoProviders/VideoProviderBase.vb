Namespace VideoProviders

    Public MustInherit Class VideoProviderBase
        Implements IVideoProvider

        Dim _currentFrameNumber As Integer
        Dim _timecodes As Timecodes

        Private _helper As IVideoProviderHelper

        Public ReadOnly Property Helper() As IVideoProviderHelper
            Get
                Return _helper
            End Get
        End Property

        Public Overridable ReadOnly Property CurrentFrameNumber() As Integer Implements IVideoProvider.CurrentFrameNumber
            Get
                CheckDisposed()
                Return _currentFrameNumber
            End Get
        End Property

        Public MustOverride ReadOnly Property CurrentFrameType() As FrameType Implements IVideoProvider.CurrentFrameType

        Public MustOverride ReadOnly Property FrameCount() As Integer Implements IVideoProvider.FrameCount

        Public MustOverride ReadOnly Property FrameRate() As Double Implements IVideoProvider.FrameRate

        Public MustOverride Sub GetFrame(ByRef output As System.Drawing.Bitmap) Implements IVideoProvider.GetFrame

        Public MustOverride Sub Open(ByVal filePath As String) Implements IVideoProvider.Open

        Public Overridable ReadOnly Property VideoSize() As Size Implements IVideoProvider.VideoSize
            Get
                Return Size.Empty
            End Get
        End Property

        Protected Sub ForceSeek(ByVal frameNumber As Integer)
            CheckDisposed()
            _currentFrameNumber = frameNumber
        End Sub
        Public Overridable Function SeekTo(ByVal frameNumber As Integer) As Boolean Implements IVideoProvider.SeekTo
            CheckDisposed()
            If frameNumber < 0 OrElse frameNumber > FrameCount - 1 Then
                Return False
            End If
            _currentFrameNumber = frameNumber
            Return True
        End Function

        Public Overridable Function SeekToNextFrame() As Boolean Implements IVideoProvider.SeekToNextFrame
            Return SeekTo(CurrentFrameNumber + 1)
        End Function

        Public Overridable Function SeekToPrevFrame() As Boolean Implements IVideoProvider.SeekToPrevFrame
            Return SeekTo(CurrentFrameNumber - 1)
        End Function

        Public MustOverride Function SeekToNextKeyFrame() As Boolean Implements IVideoProvider.SeekToNextKeyFrame

        Public MustOverride Function SeekToPrevKeyFrame() As Boolean Implements IVideoProvider.SeekToPrevKeyFrame

        Protected Overridable Function IsSuitableTimecodes(ByVal timecodes As Timecodes) As Boolean Implements IVideoProvider.IsSuitableTimecodes
            CheckDisposed()
            Return timecodes.Count = FrameCount
        End Function

        Public Overridable Function GetTimecodeOfCurrentFrame() As String Implements IVideoProvider.GetTimecodeOfCurrentFrame
            CheckDisposed()
            Return _timecodes(CurrentFrameNumber)
        End Function

        Public Overridable ReadOnly Property Timecodes() As Timecodes Implements IVideoProvider.Timecodes
            Get
                Return _timecodes
            End Get
        End Property

        Public Overridable Sub SetTimecodes(ByVal timecodes As Timecodes) Implements IVideoProvider.SetTimecodes
            CheckDisposed()
            If IsSuitableTimecodes(timecodes) Then
                _timecodes = timecodes
            Else
                Throw New InvalidOperationException("Timecodes doesn't match the video.")
            End If
        End Sub

        Private disposedValue As Boolean = False        ' 检测冗余的调用

        Protected Sub CheckDisposed()
            If disposedValue Then
                Throw New ObjectDisposedException(Me.GetType().Name)
            End If
        End Sub

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: 释放其他状态(托管对象)。
                End If

                ' TODO: 释放您自己的状态(非托管对象)。
                ' TODO: 将大型字段设置为 null。
            End If
            Me.disposedValue = True
        End Sub

#Region " IDisposable Support "
        ' Visual Basic 添加此代码是为了正确实现可处置模式。
        Public Sub Dispose() Implements IDisposable.Dispose
            ' 不要更改此代码。请将清理代码放入上面的 Dispose(ByVal disposing As Boolean) 中。
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region
        Private Class NullHelper
            Implements IVideoProviderHelper

            Public Sub RegisterTempFile(ByVal fileName As String) Implements IVideoProviderHelper.RegisterTempFile

            End Sub

            Public Sub Notify(ByVal message As String) Implements IVideoProviderHelper.Notify

            End Sub
        End Class
        Public Sub SetHelper(ByVal helper As IVideoProviderHelper) Implements IVideoProvider.SetHelper
            If helper Is Nothing Then
                Throw New ArgumentNullException("helper", "helper is null.")
            End If
            _helper = helper
        End Sub

        Public Sub New()
            SetHelper(New NullHelper)
        End Sub
    End Class

End Namespace
