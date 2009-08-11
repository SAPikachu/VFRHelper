Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Drawing.Imaging
Imports System.IO

Namespace VideoProviders

    Public Class AviSynthVideoProvider
        Inherits VideoProviderBase
        Private Enum AviSynthColorspace
            ' Fields
            I420 = &HA0000010
            IYUV = &HA0000010
            RGB24 = &H50000001
            RGB32 = &H50000002
            Unknown = &H0
            YUY2 = &H9FFFFFFC
            YV12 = &HA0000008
        End Enum
        <StructLayout(LayoutKind.Sequential)> _
        Private Structure AVSDLLVideoInfo
            Public width As Integer
            Public height As Integer
            Public raten As Integer
            Public rated As Integer
            Public aspectn As Integer
            Public aspectd As Integer
            Public interlaced_frame As Integer
            Public top_field_first As Integer
            Public num_frames As Integer
            Public pixel_type As AviSynthColorspace
            Public audio_samples_per_second As Integer
            Public sample_type As Integer ' AudioSampleType
            Public nchannels As Integer
            Public num_audio_frames As Integer
            Public num_audio_samples As Long
        End Structure



        <DllImport("AvisynthWrapper", CharSet:=CharSet.Ansi, ExactSpelling:=True)> _
Private Shared Function dimzon_avs_destroy(ByRef avs As IntPtr) As Integer
        End Function
        <DllImport("AvisynthWrapper", CharSet:=CharSet.Ansi, ExactSpelling:=True)> _
        Private Shared Function dimzon_avs_getlasterror(ByVal avs As IntPtr, <MarshalAs(UnmanagedType.LPStr)> ByVal sb As StringBuilder, ByVal len As Integer) As Integer
        End Function
        <DllImport("AvisynthWrapper", CharSet:=CharSet.Ansi, ExactSpelling:=True)> _
        Private Shared Function dimzon_avs_getvframe(ByVal avs As IntPtr, ByVal buf As IntPtr, ByVal stride As Integer, ByVal frm As Integer) As Integer
        End Function
        <DllImport("AvisynthWrapper", CharSet:=CharSet.Ansi, ExactSpelling:=True)> _
        Private Shared Function dimzon_avs_init(ByRef avs As IntPtr, ByVal func As String, ByVal arg As String, ByRef vi As AVSDLLVideoInfo, ByRef originalColorspace As AviSynthColorspace, ByRef originalSampleType As Int32, ByVal cs As String) As Integer
        End Function

        Dim _avs As IntPtr = IntPtr.Zero
        Dim _vi As AVSDLLVideoInfo

        Public Overrides ReadOnly Property CurrentFrameType() As FrameType
            Get
                CheckDisposed()
                Return FrameType.Key
            End Get
        End Property

        Public Overrides ReadOnly Property FrameCount() As Integer
            Get
                CheckDisposed()
                Return _vi.num_frames
            End Get
        End Property

        Public Overrides ReadOnly Property FrameRate() As Double
            Get
                CheckDisposed()
                Return _vi.raten / _vi.rated
            End Get
        End Property

        Public Overrides Sub GetFrame(ByRef output As System.Drawing.Bitmap)
            CheckDisposed()
            If output Is Nothing OrElse output.PixelFormat <> PixelFormat.Format24bppRgb OrElse _
               output.Width <> _vi.width OrElse output.Height <> _vi.height Then
                If output IsNot Nothing Then
                    output.Dispose()
                End If
                output = New Bitmap(_vi.width, _vi.height, PixelFormat.Format24bppRgb)
            End If
            Dim bitmapData = output.LockBits(New Rectangle(0, 0, output.Width, output.Height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb)
            Try
                'always bottom-up bitmap
                If dimzon_avs_getvframe(_avs, New IntPtr(bitmapData.Scan0.ToInt64() + bitmapData.Stride * (output.Height - 1)), -bitmapData.Stride, CurrentFrameNumber) <> 0 Then
                    ThrowAvsExecption()
                End If
            Finally
                output.UnlockBits(bitmapData)
            End Try
        End Sub

        Public Overrides Sub Open(ByVal filePath As String)
            CheckDisposed()
            Cleanup()
            If Not File.Exists(filePath) Then
                Throw New FileNotFoundException
            End If
            Dim cs As AviSynthColorspace, sr As Integer
            If dimzon_avs_init(_avs, "Import", filePath, _vi, cs, sr, AviSynthColorspace.RGB24.ToString()) <> 0 Then
                ThrowAvsExecption()
            End If
            SetTimecodes(Timecodes.Generate(FrameRate, FrameCount))
        End Sub

        Public Overrides Function SeekToNextKeyFrame() As Boolean
            Return SeekToNextFrame()
        End Function

        Public Overrides Function SeekToPrevKeyFrame() As Boolean
            Return SeekToPrevFrame()
        End Function

        Private Sub Cleanup()
            If _avs <> IntPtr.Zero Then
                dimzon_avs_destroy(_avs)
                _avs = IntPtr.Zero
            End If
        End Sub

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Cleanup()
            MyBase.Dispose(disposing)
        End Sub

        Private Sub ThrowAvsExecption()
            Dim sb As New StringBuilder(&H400)
            sb.Length = dimzon_avs_getlasterror(_avs, sb, &H400)
            Cleanup()
            Throw New AviSynthException(sb.ToString())
        End Sub

        Protected Overrides Sub Finalize()
            Dispose(False)
            MyBase.Finalize()
        End Sub
    End Class

End Namespace