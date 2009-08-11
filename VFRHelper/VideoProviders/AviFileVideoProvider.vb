Imports System.Drawing.Imaging
Imports System.IO

Namespace VideoProviders

    Public Class AviFileVideoProvider
        Inherits VideoProviderBase

        Dim _avi As AviFile.AviManager, _video As AviFile.VideoStream

        Public Overrides ReadOnly Property CurrentFrameType() As FrameType
            Get
                CheckDisposed()
                Return CType(CInt(_video.GetFrameType(CurrentFrameNumber)), FrameType)
            End Get
        End Property

        Public Overrides ReadOnly Property FrameCount() As Integer
            Get
                CheckDisposed()
                Return _video.CountFrames
            End Get
        End Property

        Public Overrides ReadOnly Property FrameRate() As Double
            Get
                CheckDisposed()
                Return _video.FrameRate
            End Get
        End Property

        Public Overrides Sub GetFrame(ByRef output As Bitmap)
            CheckDisposed()
            Dim address As IntPtr, bih As AviFile.Avi.BITMAPINFOHEADER, dataLength As Integer
            address = _video.GetRawBitmap(CurrentFrameNumber, bih, dataLength)
            Dim pixelFormat = _video.ConvertBitCountToPixelFormat(bih.biBitCount)
            'postive height = bottom-up bitmap, negative height = top-down bitmap
            Dim stride As Integer = (((bih.biBitCount \ 8) * bih.biWidth + 3) And (Not 3)) * If((bih.biHeight And &H80000000) = 0, -1, 1)
            If (stride And &H80000000) = &H80000000 Then 'bottom-up bitmap, relocate pointer to flip upside down
                address = New IntPtr(address.ToInt64() - stride * (bih.biHeight - 1))
            End If
            If output Is Nothing OrElse output.PixelFormat <> pixelFormat OrElse _
               output.Width <> bih.biWidth OrElse output.Height <> bih.biHeight Then
                If output IsNot Nothing Then
                    output.Dispose()
                End If
                output = New Bitmap(bih.biWidth, bih.biHeight And &H7FFFFFFF, stride, pixelFormat, address)
                Return
            End If
            Dim bitmapData = New BitmapData
            bitmapData.Width = bih.biWidth
            bitmapData.Height = bih.biHeight And &H7FFFFFFF 'absolute value
            bitmapData.PixelFormat = pixelFormat
            bitmapData.Stride = stride
            bitmapData.Scan0 = address
            'use the buffer directly
            output.LockBits(New Rectangle(0, 0, output.Width, output.Height), ImageLockMode.WriteOnly Or ImageLockMode.UserInputBuffer, pixelFormat, bitmapData)
            output.UnlockBits(bitmapData)
        End Sub

        Public Overrides Sub Open(ByVal filePath As String)
            CheckDisposed()
            Cleanup()
            If Not File.Exists(filePath) Then
                Throw New FileNotFoundException
            End If
            Try
                _avi = New AviFile.AviManager(filePath, True)
                _video = _avi.GetVideoStream()
                _video.GetFrameOpen()
                SetTimecodes(Timecodes.Generate(_video.FrameRate, FrameCount))
                SeekTo(0)
            Catch ex As Exception
                Cleanup()
                Throw
            End Try
        End Sub

        Public Overrides Function SeekToNextKeyFrame() As Boolean
            CheckDisposed()
            Return SeekTo(_video.GetNextKeyFrame(CurrentFrameNumber))
        End Function

        Public Overrides Function SeekToPrevKeyFrame() As Boolean
            CheckDisposed()
            Return SeekTo(_video.GetPrevKeyFrame(CurrentFrameNumber))
        End Function

        Public Overrides Function SeekToNextFrame() As Boolean
            CheckDisposed()
            Return SeekTo(_video.GetNextNonEmptyFrame(CurrentFrameNumber))
        End Function

        Public Overrides Function SeekToPrevFrame() As Boolean
            CheckDisposed()
            Return SeekTo(_video.GetPrevNonEmptyFrame(CurrentFrameNumber))
        End Function

        Private Sub Cleanup()
            If _video IsNot Nothing Then
                Try
                    '_video.GetFrameClose()
                    '_video.Close()
                    _video.Dispose()
                Catch
                End Try
                _video = Nothing
            End If
            If _avi IsNot Nothing Then
                Try
                    '_avi.Close()
                    _avi.Dispose()
                Catch
                End Try
                _avi = Nothing
            End If
        End Sub

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
                Cleanup()
            End If
            MyBase.Dispose(disposing)
        End Sub

    End Class

End Namespace