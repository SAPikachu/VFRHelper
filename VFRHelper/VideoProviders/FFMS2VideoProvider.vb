Imports System.Runtime.InteropServices
Imports System.Text
Imports System.IO
Imports System.Drawing.Imaging
Imports System.Runtime.Serialization
Imports System.Security.Cryptography

Namespace VideoProviders

    Public Class FFMS2VideoProvider
        Inherits VideoProviderBase

        '''Return Type: int
        '''Current: __int64
        '''Total: __int64
        '''ICPrivate: void*
        <UnmanagedFunctionPointer(CallingConvention.StdCall)> _
        Private Delegate Function TIndexCallback(ByVal Current As Long, ByVal Total As Long, ByVal ICPrivate As System.IntPtr) As Integer

        <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)> _
        Public Structure FrameInfo

            '''__int64
            Public PTS As Long

            '''int
            Public RepeatPict As Integer

            '''int
            Public KeyFrame As Integer
        End Structure

        Private Enum Resizers

            '''FFMS_RESIZER_FAST_BILINEAR -> 0x0001
            FFMS_RESIZER_FAST_BILINEAR = 1

            '''FFMS_RESIZER_BILINEAR -> 0x0002
            FFMS_RESIZER_BILINEAR = 2

            '''FFMS_RESIZER_BICUBIC -> 0x0004
            FFMS_RESIZER_BICUBIC = 4

            '''FFMS_RESIZER_X -> 0x0008
            FFMS_RESIZER_X = 8

            '''FFMS_RESIZER_POINT -> 0x0010
            FFMS_RESIZER_POINT = 16

            '''FFMS_RESIZER_AREA -> 0x0020
            FFMS_RESIZER_AREA = 32

            '''FFMS_RESIZER_BICUBLIN -> 0x0040
            FFMS_RESIZER_BICUBLIN = 64

            '''FFMS_RESIZER_GAUSS -> 0x0080
            FFMS_RESIZER_GAUSS = 128

            '''FFMS_RESIZER_SINC -> 0x0100
            FFMS_RESIZER_SINC = 256

            '''FFMS_RESIZER_LANCZOS -> 0x0200
            FFMS_RESIZER_LANCZOS = 512

            '''FFMS_RESIZER_SPLINE -> 0x0400
            FFMS_RESIZER_SPLINE = 1024
        End Enum


        Private Enum FFPixelFormat

            '''FFMS_PIX_FMT_NONE -> -1
            PIX_FMT_NONE = -1

            PIX_FMT_YUV420P

            PIX_FMT_YUYV422

            PIX_FMT_RGB24

            PIX_FMT_BGR24

            PIX_FMT_YUV422P

            PIX_FMT_YUV444P

            PIX_FMT_RGB32

            PIX_FMT_YUV410P

            PIX_FMT_YUV411P

            PIX_FMT_RGB565

            PIX_FMT_RGB555

            PIX_FMT_GRAY8

            PIX_FMT_MONOWHITE

            PIX_FMT_MONOBLACK

            PIX_FMT_PAL8

            PIX_FMT_YUVJ420P

            PIX_FMT_YUVJ422P

            PIX_FMT_YUVJ444P

            PIX_FMT_XVMC_MPEG2_MC

            PIX_FMT_XVMC_MPEG2_IDCT

            PIX_FMT_UYVY422

            PIX_FMT_UYYVYY411

            PIX_FMT_BGR32

            PIX_FMT_BGR565

            PIX_FMT_BGR555

            PIX_FMT_BGR8

            PIX_FMT_BGR4

            PIX_FMT_BGR4_BYTE

            PIX_FMT_RGB8

            PIX_FMT_RGB4

            PIX_FMT_RGB4_BYTE

            PIX_FMT_NV12

            PIX_FMT_NV21

            PIX_FMT_RGB32_1

            PIX_FMT_BGR32_1

            PIX_FMT_GRAY16BE

            PIX_FMT_GRAY16LE

            PIX_FMT_YUV440P

            PIX_FMT_YUVJ440P

            PIX_FMT_YUVA420P
        End Enum


        <StructLayout(LayoutKind.Sequential)> _
        Private Structure ErrorInfo

            '''int
            Public ErrorType As Errors

            '''int
            Public SubType As SubErrors

            '''int
            Public BufferSize As Integer

            '''char*
            Public Buffer As IntPtr

            Public Shared Function Create() As ErrorInfo
                Return New ErrorInfo With {.BufferSize = 1000, .Buffer = Marshal.AllocCoTaskMem(1001)}
            End Function
        End Structure

        Private Enum Errors

            '''FFMS_ERROR_SUCCESS -> 0
            ERROR_SUCCESS = 0

            '''FFMS_ERROR_INDEX -> 1
            ERROR_INDEX = 1

            ERROR_INDEXING

            ERROR_POSTPROCESSING

            ERROR_SCALING

            ERROR_DECODING

            ERROR_SEEKING

            ERROR_PARSER

            ERROR_TRACK

            ERROR_WAVE_WRITER

            ERROR_CANCELLED

        End Enum
        Private Enum SubErrors

            '''FFMS_ERROR_UNKNOWN -> 20
            ERROR_UNKNOWN = 20

            ERROR_UNSUPPORTED

            ERROR_FILE_READ

            ERROR_FILE_WRITE

            ERROR_NO_FILE

            ERROR_VERSION

            ERROR_ALLOCATION_FAILED

            ERROR_INVALID_ARGUMENT

            ERROR_CODEC

            ERROR_NOT_AVAILABLE

            ERROR_FILE_MISMATCH

            ERROR_USER
        End Enum

        Public Enum IndexErrorHandling

            '''FFMS_IEH_ABORT -> 0
            IEH_ABORT = 0

            '''FFMS_IEH_CLEAR_TRACK -> 1
            IEH_CLEAR_TRACK = 1

            '''FFMS_IEH_STOP_TRACK -> 2
            IEH_STOP_TRACK = 2

            '''FFMS_IEH_IGNORE -> 3
            IEH_IGNORE = 3
        End Enum

        Public Enum TrackType

            '''FFMS_TYPE_UNKNOWN -> -1
            TYPE_UNKNOWN = -1

            TYPE_VIDEO

            TYPE_AUDIO

            TYPE_DATA

            TYPE_SUBTITLE

            TYPE_ATTACHMENT
        End Enum

        <StructLayout(LayoutKind.Sequential)> _
        Public Structure VideoProperties

            '''int
            Public FPSDenominator As Integer

            '''int
            Public FPSNumerator As Integer

            '''int
            Public RFFDenominator As Integer

            '''int
            Public RFFNumerator As Integer

            '''int
            Public NumFrames As Integer

            '''int
            Public SARNum As Integer

            '''int
            Public SARDen As Integer

            '''int
            Public CropTop As Integer

            '''int
            Public CropBottom As Integer

            '''int
            Public CropLeft As Integer

            '''int
            Public CropRight As Integer

            '''int
            Public TopFieldFirst As Integer

            '''int
            Public ColorSpace As Integer

            '''int
            Public ColorRange As Integer

            '''double
            Public FirstTime As Double

            '''double
            Public LastTime As Double
        End Structure

        <StructLayout(LayoutKind.Sequential)> _
        Private Structure FFMSFrame

            '''unsigned byte*[4]
            <MarshalAs(UnmanagedType.ByValArray, SizeConst:=4, ArraySubType:=UnmanagedType.SysUInt)> _
            Public Data() As System.IntPtr

            '''int[4]
            <MarshalAs(UnmanagedType.ByValArray, SizeConst:=4, ArraySubType:=UnmanagedType.I4)> _
            Public Linesize() As Integer

            '''int
            Public EncodedWidth As Integer

            '''int
            Public EncodedHeight As Integer

            '''int
            Public EncodedPixelFormat As FFPixelFormat

            '''int
            Public ScaledWidth As Integer

            '''int
            Public ScaledHeight As Integer

            '''int
            Public ConvertedPixelFormat As FFPixelFormat

            '''int
            Public KeyFrame As Integer

            '''int
            Public RepeatPict As Integer

            '''int
            Public InterlacedFrame As Integer

            '''int
            Public TopFieldFirst As Integer

            '''char
            Public PictType As Byte

            ReadOnly Property OutputWidth() As Integer
                Get
                    Dim ret = ScaledWidth
                    If ret = -1 Then
                        ret = EncodedWidth
                    End If
                    Return ret
                End Get
            End Property

            ReadOnly Property OutputHeight() As Integer
                Get
                    Dim ret = ScaledHeight
                    If ret = -1 Then
                        ret = EncodedHeight
                    End If
                    Return ret
                End Get
            End Property


        End Structure

        Partial Private Class FFMS

            '''Return Type: void
            '''CPUFeatures: int
            <DllImport("ffms2.dll", EntryPoint:="FFMS_Init")> _
            Public Shared Sub Init(ByVal CPUFeatures As Integer)
            End Sub

            '''Return Type: void*
            '''SourceFile: char*
            '''Track: int
            '''Index: void*
            '''Threads: int
            '''SeekMode: int
            '''ErrorInfo: ErrorInfo*
            <DllImport("ffms2.dll", EntryPoint:="FFMS_CreateVideoSource")> _
            Public Shared Function CreateVideoSource(<[In](), MarshalAs(UnmanagedType.LPStr)> ByVal SourceFile As String, ByVal Track As Integer, ByVal Index As System.IntPtr, ByVal Threads As Integer, ByVal SeekMode As Integer, ByRef ErrorInfo As ErrorInfo) As System.IntPtr
            End Function

            '''Return Type: void
            '''V: void*
            <DllImport("ffms2.dll", EntryPoint:="FFMS_DestroyVideoSource")> _
            Public Shared Sub DestroyVideoSource(ByVal V As System.IntPtr)
            End Sub

            '''Return Type: VideoProperties*
            '''V: void*
            <DllImport("ffms2.dll", EntryPoint:="FFMS_GetVideoProperties")> _
            Public Shared Function GetVideoProperties(ByVal V As System.IntPtr) As System.IntPtr
            End Function

            '''Return Type: Frame*
            '''V: void*
            '''n: int
            '''ErrorInfo: ErrorInfo*
            <DllImport("ffms2.dll", EntryPoint:="FFMS_GetFrame")> _
            Public Shared Function GetFrame(ByVal V As System.IntPtr, ByVal n As Integer, ByRef ErrorInfo As ErrorInfo) As System.IntPtr
            End Function


            '''Return Type: FFMS_FrameInfo*
            '''T: void*
            '''Frame: int
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_GetFrameInfo")> _
            Public Shared Function GetFrameInfo(ByVal T As System.IntPtr, ByVal Frame As Integer) As System.IntPtr
            End Function


            '''Return Type: int
            '''V: void*
            '''TargetFormats: __int64
            '''Width: int
            '''Height: int
            '''Resizer: int
            '''ErrorInfo: ErrorInfo*
            <DllImport("ffms2.dll", EntryPoint:="FFMS_SetOutputFormatV")> _
            Public Shared Function SetOutputFormatV(ByVal V As System.IntPtr, ByVal TargetFormats As Long, ByVal Width As Integer, ByVal Height As Integer, ByVal Resizer As Integer, ByRef ErrorInfo As ErrorInfo) As Integer
            End Function

            '''Return Type: void
            '''Index: void*
            <DllImport("ffms2.dll", EntryPoint:="FFMS_DestroyIndex")> _
            Public Shared Sub DestroyIndex(ByVal Index As System.IntPtr)
            End Sub

            '''Return Type: int
            '''T: void*
            '''TimecodeFile: char*
            '''ErrorInfo: ErrorInfo*
            <DllImport("ffms2.dll", EntryPoint:="FFMS_WriteTimecodes")> _
            Public Shared Function WriteTimecodes(ByVal T As System.IntPtr, <[In](), MarshalAs(UnmanagedType.LPStr)> ByVal TimecodeFile As String, ByRef ErrorInfo As ErrorInfo) As Integer
            End Function

            '''Return Type: void*
            '''SourceFile: char*
            '''IndexMask: int
            '''DumpMask: int
            '''ANC: void*
            '''ANCPrivate: void*
            '''ErrorHandling: int
            '''IC: TIndexCallback
            '''ICPrivate: void*
            '''ErrorInfo: ErrorInfo*
            <DllImport("ffms2.dll", EntryPoint:="FFMS_MakeIndex")> _
            Public Shared Function MakeIndex(<[In](), MarshalAs(UnmanagedType.LPStr)> ByVal SourceFile As String, ByVal IndexMask As Integer, ByVal DumpMask As Integer, ByVal ANC As System.IntPtr, ByVal ANCPrivate As System.IntPtr, ByVal ErrorHandling As IndexErrorHandling, ByVal IC As TIndexCallback, ByVal ICPrivate As System.IntPtr, ByRef ErrorInfo As ErrorInfo) As System.IntPtr
            End Function

            '''Return Type: void*
            '''IndexFile: char*
            '''ErrorInfo: ErrorInfo*
            <DllImport("ffms2.dll", EntryPoint:="FFMS_ReadIndex")> _
            Public Shared Function ReadIndex(<[In](), MarshalAs(UnmanagedType.LPStr)> ByVal IndexFile As String, ByRef ErrorInfo As ErrorInfo) As System.IntPtr
            End Function

            '''Return Type: int
            '''IndexFile: char*
            '''Index: void*
            '''ErrorInfo: ErrorInfo*
            <DllImport("ffms2.dll", EntryPoint:="FFMS_WriteIndex")> _
            Public Shared Function WriteIndex(<[In](), MarshalAs(UnmanagedType.LPStr)> ByVal IndexFile As String, ByVal Index As System.IntPtr, ByRef ErrorInfo As ErrorInfo) As Integer
            End Function

            '''Return Type: void*
            '''Index: void*
            '''Track: int
            <DllImport("ffms2.dll", EntryPoint:="FFMS_GetTrackFromIndex")> _
            Public Shared Function GetTrackFromIndex(ByVal Index As System.IntPtr, ByVal Track As Integer) As System.IntPtr
            End Function

            '''Return Type: int
            '''Index: void*
            '''TrackType: int
            '''ErrorInfo: ErrorInfo*
            <DllImport("ffms2.dll", EntryPoint:="FFMS_GetFirstTrackOfType")> _
            Public Shared Function GetFirstTrackOfType(ByVal Index As System.IntPtr, ByVal TrackType As Integer, ByRef ErrorInfo As ErrorInfo) As Integer
            End Function
        End Class

        Dim _videoProperties As VideoProperties
        ''' <summary>
        ''' VideoBase
        ''' </summary>
        ''' <remarks></remarks>
        Dim _VS As IntPtr
        Dim _currentFrameCache As FFMSFrame
        Dim _keyFrames As List(Of Integer)
        Dim _errorInfo As ErrorInfo = ErrorInfo.Create()

        Shared Sub New()
            FFMS.Init(0)
        End Sub
        Public Overrides ReadOnly Property CurrentFrameType() As FrameType
            Get
                CheckDisposed()
                DecodeCurrentFrame()
                Return CType(FrameType.CustomAsciiChar Or _currentFrameCache.PictType, FrameType)
            End Get
        End Property

        Public Overrides ReadOnly Property FrameCount() As Integer
            Get
                CheckDisposed()
                Return _videoProperties.NumFrames
            End Get
        End Property

        Public Overrides ReadOnly Property FrameRate() As Double
            Get
                CheckDisposed()
                Return _videoProperties.FPSNumerator / _videoProperties.FPSDenominator
            End Get
        End Property

        Private Sub DecodeCurrentFrame()
            If _currentFrameCache.Data IsNot Nothing AndAlso _currentFrameCache.Data(0) <> IntPtr.Zero Then 'position hasn't been changed
                Return
            End If
            Dim framePtr = FFMS.GetFrame(_VS, CurrentFrameNumber, _errorInfo)
            If framePtr = IntPtr.Zero Then
                ThrowException()
            End If
            _currentFrameCache = CType(Marshal.PtrToStructure(framePtr, GetType(FFMSFrame)), FFMSFrame)
        End Sub
        Public Overrides Sub GetFrame(ByRef output As System.Drawing.Bitmap)
            CheckDisposed()
            DecodeCurrentFrame()

            Dim frameWidth As Integer = _currentFrameCache.OutputWidth
            Dim frameHeight As Integer = _currentFrameCache.OutputHeight
            If output Is Nothing OrElse output.PixelFormat <> Imaging.PixelFormat.Format24bppRgb OrElse _
               output.Width <> frameWidth OrElse output.Height <> frameHeight Then
                If output IsNot Nothing Then
                    output.Dispose()
                End If
                output = New Bitmap(frameWidth, frameHeight, _currentFrameCache.Linesize(0), PixelFormat.Format24bppRgb, _currentFrameCache.Data(0))
                Return
            End If
            Dim bitmapData = New BitmapData
            bitmapData.Width = frameWidth
            bitmapData.Height = frameHeight
            bitmapData.PixelFormat = Imaging.PixelFormat.Format24bppRgb
            bitmapData.Stride = _currentFrameCache.Linesize(0)
            bitmapData.Scan0 = _currentFrameCache.Data(0)

            output.LockBits(New Rectangle(0, 0, output.Width, output.Height), ImageLockMode.WriteOnly Or ImageLockMode.UserInputBuffer, Imaging.PixelFormat.Format24bppRgb, bitmapData)
            output.UnlockBits(bitmapData)
        End Sub

        Private Function MakeIndex(ByVal filePath As String) As IntPtr
            Dim index As IntPtr
            index = FFMS.MakeIndex(filePath, 0, 0, IntPtr.Zero, IntPtr.Zero, IndexErrorHandling.IEH_ABORT, AddressOf IndexCallback, IntPtr.Zero, _errorInfo)
            CheckResult()
            Return index
        End Function
        Private Shared Sub DestroyIndex(ByVal index As IntPtr)
            FFMS.DestroyIndex(index)
        End Sub
        Private Shared Function GetTrackFromIndex(ByVal index As IntPtr, ByVal trackNumber As Integer) As IntPtr
            Return FFMS.GetTrackFromIndex(index, trackNumber)
        End Function
        Private Sub SetCorrectOutputFormat()
            DecodeCurrentFrame()
            FFMS.SetOutputFormatV(_VS, 1L << FFPixelFormat.PIX_FMT_BGR24, _currentFrameCache.OutputWidth, _currentFrameCache.OutputHeight, Resizers.FFMS_RESIZER_FAST_BILINEAR, _errorInfo)
            CheckResult()
            _currentFrameCache = Nothing
        End Sub
        Private Sub GetVideoProperties()
            Dim vpPtr As IntPtr = FFMS.GetVideoProperties(_VS)
            _videoProperties = CType(Marshal.PtrToStructure(vpPtr, GetType(VideoProperties)), VideoProperties)
        End Sub
        Public Overrides Sub Open(ByVal filePath As String)
            CheckDisposed()
            Cleanup()
            If Not File.Exists(filePath) Then
                Throw New FileNotFoundException
            End If
            Dim hasher = MD5.Create()
            Dim indexFile As String = Path.Combine(Path.GetTempPath(), String.Format("{0}.{1:X}.ffmsindex", String.Concat(Array.ConvertAll(hasher.ComputeHash(Encoding.UTF8.GetBytes(Path.GetFileName(filePath))), Function(b) b.ToString("X2"))), File.GetCreationTime(filePath).ToBinary() Xor File.GetLastWriteTime(filePath).ToBinary() Xor New FileInfo(filePath).Length))
            Dim tcFile = Path.ChangeExtension(indexFile, ".tc")
            Dim index As IntPtr = IntPtr.Zero

            '-----make or read index
            index = FFMS.ReadIndex(indexFile, _errorInfo)
            If index = IntPtr.Zero Then
                index = MakeIndex(filePath)

                Try
                    FFMS.WriteIndex(indexFile, index, _errorInfo)
                    CheckResult()
                Catch
                    FFMS.DestroyIndex(index)
                    Throw
                End Try
                Helper.RegisterTempFile(indexFile)
            End If
            Try
                Dim trackNumber = FFMS.GetFirstTrackOfType(index, TrackType.TYPE_VIDEO, _errorInfo)
                CheckResult()
                If trackNumber < 0 Then
                    Throw New FFMSException("No video track found")
                End If

                Dim track As IntPtr = GetTrackFromIndex(index, trackNumber)
                FFMS.WriteTimecodes(track, tcFile, _errorInfo)
                CheckResult()

                '-----open video
                _VS = FFMS.CreateVideoSource(filePath, trackNumber, index, Environment.ProcessorCount, 1, _errorInfo)
                CheckResult()
                GetVideoProperties()

                GetKeyFrameList(track, _videoProperties.NumFrames)
                '-----change output format if necessary
                DecodeCurrentFrame()
                If _currentFrameCache.EncodedPixelFormat <> FFPixelFormat.PIX_FMT_BGR24 Then
                    SetCorrectOutputFormat()
                End If
            Finally
                DestroyIndex(index)
            End Try
            SetTimecodes(Timecodes.OpenV2(tcFile))
            File.Delete(tcFile)
        End Sub

        Private Sub GetKeyFrameList(ByVal trackPtr As IntPtr, ByVal frameCount As Integer)
            _keyFrames = New List(Of Integer)
            For i = 0 To frameCount - 1
                Dim frameInfoPtr = FFMS.GetFrameInfo(trackPtr, i)
                If frameInfoPtr = IntPtr.Zero Then
                    Throw New FFMSException("Can't get info for frame " & i.ToString())
                End If
                Dim frameInfo = CType(Marshal.PtrToStructure(frameInfoPtr, GetType(FrameInfo)), FrameInfo)
                If frameInfo.KeyFrame <> 0 Then
                    _keyFrames.Add(i)
                End If
            Next
        End Sub

        Public Overrides Function SeekToNextKeyFrame() As Boolean
            Dim index As Integer = _keyFrames.BinarySearch(CurrentFrameNumber)
            If index >= 0 Then
                index += 1
            Else
                index = Not index
                If index = _keyFrames.Count Then
                    Return False
                End If
            End If
            If index < 0 OrElse index >= _keyFrames.Count Then
                Return False
            End If

            Return SeekTo(_keyFrames(index))
        End Function

        Public Overrides Function SeekToPrevKeyFrame() As Boolean
            Dim index As Integer = _keyFrames.BinarySearch(CurrentFrameNumber)
            If index < 0 Then
                index = Not index
            End If
            index -= 1
            If index < 0 OrElse index >= _keyFrames.Count Then
                Return False
            End If
            Return SeekTo(_keyFrames(index))
        End Function

        Public Overrides Function SeekTo(ByVal frameNumber As Integer) As Boolean
            Dim result = MyBase.SeekTo(frameNumber)
            If result Then
                _currentFrameCache = Nothing
            End If
            Return result
        End Function

        Private Sub NotifyIndexProgress(ByVal Current As Long, ByVal Total As Long)
            Helper.Notify(String.Format("Indexing video file, progress: {0}/{1}", Current, Total))
        End Sub
        Private Function IndexCallback(ByVal Current As Long, ByVal Total As Long, ByVal [Private] As System.IntPtr) As Integer
            NotifyIndexProgress(Current, Total)
        End Function
        Private Sub ThrowException()
            Throw New FFMSException(String.Format("{0} Type={1}, SubType={2}", Marshal.PtrToStringAnsi(_errorInfo.Buffer), _errorInfo.ErrorType.ToString, _errorInfo.SubType.ToString))
        End Sub
        Private Sub CheckResult()
            If _errorInfo.ErrorType <> Errors.ERROR_SUCCESS Then
                Cleanup()
                ThrowException()
            End If
        End Sub
        Private Sub Cleanup()
            _currentFrameCache = Nothing
            _keyFrames = Nothing
            _videoProperties = Nothing
            If _VS <> IntPtr.Zero Then
                FFMS.DestroyVideoSource(_VS)
                _VS = IntPtr.Zero
            End If
        End Sub
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Cleanup()
            _errorInfo.BufferSize = 0
            If _errorInfo.Buffer <> IntPtr.Zero Then
                Marshal.FreeCoTaskMem(_errorInfo.Buffer)
                _errorInfo.Buffer = IntPtr.Zero
            End If
            MyBase.Dispose(disposing)
        End Sub

        Protected Overrides Sub Finalize()
            Dispose(False)
            MyBase.Finalize()
        End Sub

        Public Overrides ReadOnly Property VideoSize() As System.Drawing.Size
            Get
                DecodeCurrentFrame()
                Return New Size(_currentFrameCache.OutputWidth, _currentFrameCache.OutputHeight)
            End Get
        End Property
    End Class
End Namespace
