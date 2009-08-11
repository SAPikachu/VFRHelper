Imports System.Runtime.InteropServices
Imports System.Text
Imports System.IO
Imports System.Drawing.Imaging

Namespace VideoProviders

    Public Class FFMSVideoProvider
        Inherits VideoProviderBase

        '''Return Type: int
        '''State: int
        '''Current: __int64
        '''Total: __int64
        '''Private: void*
        <UnmanagedFunctionPointer(CallingConvention.StdCall)> _
        Private Delegate Function IndexCallbackDelegate(ByVal State As Integer, ByVal Current As Long, ByVal Total As Long, ByVal [Private] As System.IntPtr) As Integer

        <UnmanagedFunctionPointer(CallingConvention.StdCall)> _
        Private Delegate Function IndexCallbackDelegate2(ByVal Current As Long, ByVal Total As Long, ByVal [Private] As System.IntPtr) As Integer

        Private Enum TrackType

            '''FFMS_TYPE_VIDEO -> 0
            TYPE_VIDEO = 0

            '''FFMS_TYPE_AUDIO -> 1
            TYPE_AUDIO = 1
        End Enum

        Private Enum PixelFormat

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
        Private Structure AVFrameLite

            '''BYTE*[4]
            Public Data0 As System.IntPtr
            Public Data1 As System.IntPtr
            Public Data2 As System.IntPtr
            Public Data3 As System.IntPtr

            '''int[4]
            Public Linesize0 As Integer
            Public Linesize1 As Integer
            Public Linesize2 As Integer
            Public Linesize3 As Integer

            '''BYTE*[4]
            Public Base0 As System.IntPtr
            Public Base1 As System.IntPtr
            Public Base2 As System.IntPtr
            Public Base3 As System.IntPtr

            '''int
            Public KeyFrame As Integer

            '''int
            Public PictType As Integer
        End Structure

        <StructLayout(LayoutKind.Sequential)> _
        Structure FFMS_Frame

            '''BYTE*[4]
            Public Data0 As System.IntPtr
            Public Data1 As System.IntPtr
            Public Data2 As System.IntPtr
            Public Data3 As System.IntPtr

            '''int[4]
            Public Linesize0 As Integer
            Public Linesize1 As Integer
            Public Linesize2 As Integer
            Public Linesize3 As Integer

            Public EncodedWidth As Integer
            Public EncodedHeight As Integer
            Public EncodedPixelFormat As Integer
            Public ScaledWidth As Integer
            Public ScaledHeight As Integer
            Public ConvertedPixelFormat As Integer
            Public KeyFrame As Integer
            Public RepeatPict As Integer
            Public InterlacedFrame As Integer
            Public TopFieldFirst As Integer
            Public PictType As Byte
        End Structure


        <StructLayout(LayoutKind.Sequential)> _
        Private Structure VideoProperties

            '''int
            Public Width As Integer

            '''int
            Public Height As Integer

            '''int
            Public FPSDenominator As Integer

            '''int
            Public FPSNumerator As Integer

            '''int
            Public NumFrames As Integer

            '''int
            Public PixelFormat As PixelFormat

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

            '''double
            Public FirstTime As Double

            '''double
            Public LastTime As Double
        End Structure

        <StructLayout(LayoutKind.Sequential)> _
        Private Structure VideoProperties2

            '''int
            Public Width As Integer

            '''int
            Public Height As Integer

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
            Public PixelFormat As PixelFormat

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

            '''double
            Public FirstTime As Double

            '''double
            Public LastTime As Double
        End Structure




        Partial Private Class FFMS

            '''Return Type: void
            <DllImport("ffms2.dll", EntryPoint:="_FFMS_Init@4", CallingConvention:=CallingConvention.StdCall)> _
            Public Shared Sub Init(ByVal cpuFeatures As Integer)
            End Sub
            <DllImport("ffms2.dll", EntryPoint:="_FFMS_Init@0", CallingConvention:=CallingConvention.StdCall)> _
            Public Shared Sub Init()
            End Sub

            '''Return Type: VideoBase*
            '''SourceFile: char*
            '''Track: int
            '''TrackIndices: FrameIndex*
            '''PP: char*
            '''Threads: int
            '''SeekMode: int
            '''ErrorMsg: char*
            '''MsgSize: unsigned int
            <DllImport("ffms2.dll", EntryPoint:="_FFMS_CreateVideoSource@32", CallingConvention:=CallingConvention.StdCall)> _
            Public Shared Function CreateVideoSource(<[In](), MarshalAs(UnmanagedType.LPStr)> ByVal SourceFile As String, ByVal Track As Integer, ByVal TrackIndices As IntPtr, <[In](), MarshalAs(UnmanagedType.LPStr)> ByVal PP As String, ByVal Threads As Integer, ByVal SeekMode As Integer, <MarshalAs(UnmanagedType.LPStr)> ByVal ErrorMsg As StringBuilder, ByVal MsgSize As UInteger) As System.IntPtr
            End Function

            '''Return Type: void
            '''VB: VideoBase*
            <DllImport("ffms2.dll", EntryPoint:="_FFMS_DestroyVideoSource@4", CallingConvention:=CallingConvention.StdCall)> _
            Public Shared Sub DestroyVideoSource(ByVal VB As IntPtr)
            End Sub

            '''Return Type: VideoProperties*
            '''VB: VideoBase*
            <DllImport("ffms2.dll", EntryPoint:="_FFMS_GetVideoProperties@4", CallingConvention:=CallingConvention.StdCall)> _
            Public Shared Function GetVideoProperties(ByVal VB As IntPtr) As System.IntPtr
            End Function

            '''Return Type: AVFrameLite*
            '''VB: VideoBase*
            '''n: int
            '''ErrorMsg: char*
            '''MsgSize: unsigned int
            <DllImport("ffms2.dll", EntryPoint:="_FFMS_GetFrame@16", CallingConvention:=CallingConvention.StdCall)> _
            Public Shared Function GetFrame(ByVal VB As IntPtr, ByVal n As Integer, <MarshalAs(UnmanagedType.LPStr)> ByVal ErrorMsg As StringBuilder, ByVal MsgSize As UInteger) As System.IntPtr
            End Function

            '''Return Type: void
            '''FI: FrameIndex*
            <DllImport("ffms2.dll", EntryPoint:="_FFMS_DestroyFrameIndex@4", CallingConvention:=CallingConvention.StdCall)> _
            Public Shared Sub DestroyFrameIndex(ByVal FI As IntPtr)
            End Sub
            <DllImport("ffms2.dll", EntryPoint:="_FFMS_DestroyIndex@4", CallingConvention:=CallingConvention.StdCall)> _
            Public Shared Sub DestroyIndex(ByVal FI As IntPtr)
            End Sub

            '''Return Type: int
            '''VB: VideoBase*
            '''TargetFormat: int
            '''Width: int
            '''Height: int
            '''ErrorMsg: char*
            '''MsgSize: unsigned int
            <DllImport("ffms2.dll", EntryPoint:="_FFMS_SetOutputFormat@24", CallingConvention:=CallingConvention.StdCall)> _
            Public Shared Function SetOutputFormat(ByVal VB As IntPtr, _
                ByVal TargetFormat As PixelFormat, _
                ByVal Width As Integer, _
                ByVal Height As Integer, _
                <MarshalAs(UnmanagedType.LPStr)> ByVal ErrorMsg As StringBuilder, _
                ByVal MsgSize As UInteger) As Integer
            End Function


            Enum Resizers As Integer
                FFMS_RESIZER_FAST_BILINEAR = &H1
                FFMS_RESIZER_BILINEAR = &H2
                FFMS_RESIZER_BICUBIC = &H4
                FFMS_RESIZER_X = &H8
                FFMS_RESIZER_POINT = &H10
                FFMS_RESIZER_AREA = &H20
                FFMS_RESIZER_BICUBLIN = &H40
                FFMS_RESIZER_GAUSS = &H80
                FFMS_RESIZER_SINC = &H100
                FFMS_RESIZER_LANCZOS = &H200
                FFMS_RESIZER_SPLINE = &H400
            End Enum

            <DllImport("ffms2.dll", EntryPoint:="_FFMS_SetOutputFormatV@32", CallingConvention:=CallingConvention.StdCall)> _
            Public Shared Function SetOutputFormatV(ByVal VB As IntPtr, _
                ByVal TargetFormats As Long, _
                ByVal Width As Integer, _
                ByVal Height As Integer, _
                ByVal Resizer As Resizers, _
                <MarshalAs(UnmanagedType.LPStr)> ByVal ErrorMsg As StringBuilder, _
                ByVal MsgSize As UInteger) As Integer
            End Function


            '''Return Type: int
            '''TrackIndices: FrameIndex*
            '''TrackType: int
            '''ErrorMsg: char*
            '''MsgSize: unsigned int
            <DllImport("ffms2.dll", EntryPoint:="_FFMS_GetFirstTrackOfType@16", CallingConvention:=CallingConvention.StdCall)> _
            Public Shared Function GetFirstTrackOfType(ByVal TrackIndices As IntPtr, ByVal TrackType As TrackType, <MarshalAs(UnmanagedType.LPStr)> ByVal ErrorMsg As StringBuilder, ByVal MsgSize As UInteger) As Integer
            End Function
            '''Return Type: FrameInfoVector *
            '''TrackIndices: FrameIndex*
            '''TrackType: int
            '''ErrorMsg: char*
            '''MsgSize: unsigned int
            <DllImport("ffms2.dll", EntryPoint:="_FFMS_GetTITrackIndex@16", CallingConvention:=CallingConvention.StdCall)> _
            Public Shared Function GetTITrackIndex(ByVal TrackIndices As IntPtr, ByVal Track As Integer, <MarshalAs(UnmanagedType.LPStr)> ByVal ErrorMsg As StringBuilder, ByVal MsgSize As UInteger) As IntPtr
            End Function

            <DllImport("ffms2.dll", EntryPoint:="_FFMS_GetTrackFromIndex@8", CallingConvention:=CallingConvention.StdCall)> _
            Public Shared Function GetTrackFromIndex(ByVal Index As IntPtr, ByVal Track As Integer) As IntPtr
            End Function

            '''Return Type: int
            '''FIV: FrameInfoVector*
            '''TimecodeFile: char*
            '''ErrorMsg: char*
            '''MsgSize: unsigned int
            <DllImport("ffms2.dll", EntryPoint:="_FFMS_WriteTimecodes@16", CallingConvention:=CallingConvention.StdCall)> _
            Public Shared Function WriteTimecodes(ByVal FIV As IntPtr, <[In](), MarshalAs(UnmanagedType.LPStr)> ByVal TimecodeFile As String, <MarshalAs(UnmanagedType.LPStr)> ByVal ErrorMsg As StringBuilder, ByVal MsgSize As UInteger) As Integer
            End Function

            '''Return Type: FrameIndex*
            '''SourceFile: char*
            '''IndexMask: int
            '''DumpMask: int
            '''AudioFile: char*
            '''IgnoreDecodeErrors: boolean
            '''IP: IndexCallback
            '''Private: void*
            '''ErrorMsg: char*
            '''MsgSize: unsigned int
            <DllImport("ffms2.dll", EntryPoint:="_FFMS_MakeIndex@36", CallingConvention:=CallingConvention.StdCall)> _
            Public Shared Function MakeIndex(<[In](), MarshalAs(UnmanagedType.LPStr)> ByVal SourceFile As String, _
                ByVal IndexMask As Integer, _
                ByVal DumpMask As Integer, _
                <[In](), MarshalAs(UnmanagedType.LPStr)> ByVal AudioFile As String, _
                <MarshalAs(UnmanagedType.I1)> ByVal IgnoreDecodeErrors As Boolean, _
                ByVal IP As IndexCallbackDelegate, _
                ByVal [Private] As System.IntPtr, _
                <MarshalAs(UnmanagedType.LPStr)> ByVal ErrorMsg As StringBuilder, _
                ByVal MsgSize As UInteger) As System.IntPtr
            End Function


            <DllImport("ffms2.dll", EntryPoint:="_FFMS_MakeIndex@40", CallingConvention:=CallingConvention.StdCall)> _
            Public Shared Function MakeIndex(<[In](), MarshalAs(UnmanagedType.LPStr)> ByVal SourceFile As String, _
                ByVal IndexMask As Integer, _
                ByVal DumpMask As Integer, _
                ByVal ANC As IntPtr, _
                ByVal ANCPrivate As IntPtr, _
                <MarshalAs(UnmanagedType.I1)> ByVal IgnoreDecodeErrors As Boolean, _
                ByVal IP As IndexCallbackDelegate2, _
                ByVal [Private] As System.IntPtr, _
                <MarshalAs(UnmanagedType.LPStr)> ByVal ErrorMsg As StringBuilder, _
                ByVal MsgSize As UInteger) As System.IntPtr
            End Function

            '''Return Type: FrameIndex*
            '''IndexFile: char*
            '''ErrorMsg: char*
            '''MsgSize: unsigned int
            <DllImport("ffms2.dll", EntryPoint:="_FFMS_ReadIndex@12", CallingConvention:=CallingConvention.StdCall)> _
            Public Shared Function ReadIndex(<[In](), MarshalAs(UnmanagedType.LPStr)> ByVal IndexFile As String, <MarshalAs(UnmanagedType.LPStr)> ByVal ErrorMsg As StringBuilder, ByVal MsgSize As UInteger) As System.IntPtr
            End Function

            '''Return Type: int
            '''IndexFile: char*
            '''TrackIndices: FrameIndex*
            '''ErrorMsg: char*
            '''MsgSize: unsigned int
            <DllImport("ffms2.dll", EntryPoint:="_FFMS_WriteIndex@16", CallingConvention:=CallingConvention.StdCall)> _
            Public Shared Function WriteIndex(<[In](), MarshalAs(UnmanagedType.LPStr)> ByVal IndexFile As String, ByVal TrackIndices As IntPtr, <MarshalAs(UnmanagedType.LPStr)> ByVal ErrorMsg As StringBuilder, ByVal MsgSize As UInteger) As Integer
            End Function

        End Class

        Dim _videoProperties As VideoProperties
        ''' <summary>
        ''' VideoBase
        ''' </summary>
        ''' <remarks></remarks>
        Dim _VS As IntPtr
        Dim _currentFrameCache As AVFrameLite
        Dim _currentFrameCache2 As FFMS_Frame
        Dim _keyFrames As List(Of Integer)

        Shared ReadOnly _isBeta10OrLater As Boolean

        Shared Sub New()
            Try
                FFMS.Init()
            Catch ex As EntryPointNotFoundException
                FFMS.Init(0)
                _isBeta10OrLater = True
            End Try
        End Sub
        Public Overrides ReadOnly Property CurrentFrameType() As FrameType
            Get
                CheckDisposed()
                DecodeCurrentFrame()
                If Not _isBeta10OrLater Then
                    Return If(_currentFrameCache.KeyFrame = 1, FrameType.Key, FrameType.Normal)
                End If
                Return CType(FrameType.CustomAsciiChar Or _currentFrameCache2.PictType, FrameType)
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
            If _currentFrameCache.Data0 <> IntPtr.Zero Then 'position hasn't been changed
                Return
            End If
            Dim errorMsgCap = 1000US
            Dim errorMsg As New StringBuilder(errorMsgCap)
            Dim framePtr = FFMS.GetFrame(_VS, CurrentFrameNumber, errorMsg, errorMsgCap)
            If framePtr = IntPtr.Zero Then
                ThrowException(errorMsg)
            End If
            _currentFrameCache = CType(Marshal.PtrToStructure(framePtr, GetType(AVFrameLite)), AVFrameLite)
            If _isBeta10OrLater Then
                _currentFrameCache2 = CType(Marshal.PtrToStructure(framePtr, GetType(FFMS_Frame)), FFMS_Frame)
            End If
        End Sub
        Public Overrides Sub GetFrame(ByRef output As System.Drawing.Bitmap)
            CheckDisposed()
            DecodeCurrentFrame()

            If output Is Nothing OrElse output.PixelFormat <> Imaging.PixelFormat.Format24bppRgb OrElse _
               output.Width <> _videoProperties.Width OrElse output.Height <> _videoProperties.Height Then
                If output IsNot Nothing Then
                    output.Dispose()
                End If
                output = New Bitmap(_videoProperties.Width, _videoProperties.Height, _currentFrameCache.Linesize0, System.Drawing.Imaging.PixelFormat.Format24bppRgb, _currentFrameCache.Data0)
                Return
            End If
            Dim bitmapData = New BitmapData
            bitmapData.Width = _videoProperties.Width
            bitmapData.Height = _videoProperties.Height
            bitmapData.PixelFormat = Imaging.PixelFormat.Format24bppRgb
            bitmapData.Stride = _currentFrameCache.Linesize0
            bitmapData.Scan0 = _currentFrameCache.Data0

            output.LockBits(New Rectangle(0, 0, output.Width, output.Height), ImageLockMode.WriteOnly Or ImageLockMode.UserInputBuffer, Imaging.PixelFormat.Format24bppRgb, bitmapData)
            output.UnlockBits(bitmapData)
        End Sub

        Private Function MakeIndex(ByVal filePath As String, ByVal errorMsgCap As UShort, ByVal errorMsg As StringBuilder) As IntPtr
            Dim index As IntPtr
            If Not _isBeta10OrLater Then
                index = FFMS.MakeIndex(filePath, 0, 0, Nothing, True, AddressOf IndexCallback, IntPtr.Zero, errorMsg, errorMsgCap)
            Else
                index = FFMS.MakeIndex(filePath, 0, 0, IntPtr.Zero, IntPtr.Zero, True, AddressOf IndexCallback, IntPtr.Zero, errorMsg, errorMsgCap)
            End If
            Return index
        End Function
        Private Shared Sub DestroyIndex(ByVal index As IntPtr)
            If Not _isBeta10OrLater Then
                FFMS.DestroyFrameIndex(index)
            Else
                FFMS.DestroyIndex(index)
            End If
        End Sub
        Private Shared Function GetTrackFromIndex(ByVal index As IntPtr, ByVal errorMsgCap As UShort, ByVal errorMsg As StringBuilder, ByVal trackNumber As Integer) As IntPtr
            Dim track As IntPtr
            If Not _isBeta10OrLater Then
                track = FFMS.GetTITrackIndex(index, trackNumber, errorMsg, errorMsgCap)
            Else
                track = FFMS.GetTrackFromIndex(index, trackNumber)
            End If
            Return track
        End Function
        Private Sub SetOutputFormat(ByVal errorMsgCap As UShort, ByVal errorMsg As StringBuilder)
            If Not _isBeta10OrLater Then
                CheckResult(FFMS.SetOutputFormat(_VS, PixelFormat.PIX_FMT_BGR24, _videoProperties.Width, _videoProperties.Height, errorMsg, errorMsgCap), errorMsg)
            Else
                CheckResult(FFMS.SetOutputFormatV(_VS, 1L << PixelFormat.PIX_FMT_BGR24, _videoProperties.Width, _videoProperties.Height, FFMS.Resizers.FFMS_RESIZER_FAST_BILINEAR, errorMsg, errorMsgCap), errorMsg)
            End If
        End Sub
        Private Sub GetVideoProperties()
            Dim vpPtr As IntPtr = FFMS.GetVideoProperties(_VS)
            If Not _isBeta10OrLater Then
                _videoProperties = CType(Marshal.PtrToStructure(vpPtr, GetType(VideoProperties)), VideoProperties)
            Else
                Dim buffer(Marshal.SizeOf(GetType(VideoProperties)) - 1) As Byte
                Marshal.Copy(vpPtr, buffer, 0, 16)
                Marshal.Copy(New IntPtr(vpPtr.ToInt64() + 24), buffer, 16, buffer.Length - 16)
                Dim handle = GCHandle.Alloc(buffer, GCHandleType.Pinned)
                Try
                    _videoProperties = CType(Marshal.PtrToStructure(handle.AddrOfPinnedObject(), GetType(VideoProperties)), VideoProperties)
                Finally
                    handle.Free()
                End Try
            End If
        End Sub
        Public Overrides Sub Open(ByVal filePath As String)
            CheckDisposed()
            Cleanup()
            If Not File.Exists(filePath) Then
                Throw New FileNotFoundException
            End If
            Dim indexFile As String = Path.Combine(Path.GetTempPath(), String.Format("{0}.{1:X}.ffmsindex", Path.GetFileName(filePath), File.GetCreationTime(filePath).ToBinary() Xor File.GetLastWriteTime(filePath).ToBinary() Xor New FileInfo(filePath).Length))
            Dim tcFile = Path.ChangeExtension(indexFile, ".tc")
            Dim index As IntPtr = IntPtr.Zero
            Dim errorMsgCap = 1000US
            Dim errorMsg As New StringBuilder(errorMsgCap)
            '-----make or read index
            index = FFMS.ReadIndex(indexFile, errorMsg, errorMsgCap)
            If index = IntPtr.Zero Then
                errorMsg.Length = 0
                index = MakeIndex(filePath, errorMsgCap, errorMsg)
                If index = IntPtr.Zero Then
                    ThrowException(errorMsg)
                End If
                Try
                    CheckResult(FFMS.WriteIndex(indexFile, index, errorMsg, errorMsgCap), errorMsg)
                Catch
                    FFMS.DestroyFrameIndex(index)
                    Throw
                End Try
                Helper.RegisterTempFile(indexFile)
            End If
            Try
                Dim trackNumber = FFMS.GetFirstTrackOfType(index, TrackType.TYPE_VIDEO, errorMsg, errorMsgCap)
                If trackNumber < 0 Then
                    Throw New FFMSException("No video track found")
                End If

                Dim track As IntPtr = GetTrackFromIndex(index, errorMsgCap, errorMsg, trackNumber)
                CheckResult(FFMS.WriteTimecodes(track, tcFile, errorMsg, errorMsgCap), errorMsg)
                '-----open video
                _VS = FFMS.CreateVideoSource(filePath, trackNumber, index, String.Empty, Environment.ProcessorCount, 1, errorMsg, errorMsgCap)
                If _VS = IntPtr.Zero Then
                    ThrowException(errorMsg)
                End If
                GetVideoProperties()
                '-----change output format if necessary
                If _videoProperties.PixelFormat <> PixelFormat.PIX_FMT_BGR24 Then
                    SetOutputFormat(errorMsgCap, errorMsg)
                End If
            Finally
                DestroyIndex(index)
            End Try
            GetKeyFrameList(indexFile)
            SetTimecodes(Timecodes.OpenV2(tcFile))
            File.Delete(tcFile)
        End Sub

        Private Sub GetKeyFrameListGeneric(ByVal binr As BinaryReader, ByVal preProcessSeek As Integer, ByVal preProcessFrameSeek As Integer)
            binr.BaseStream.Seek(preProcessSeek, SeekOrigin.Current)
            Try
                For i = 0 To FrameCount - 1
                    binr.BaseStream.Seek(preProcessFrameSeek, SeekOrigin.Current)
                    If (binr.ReadInt32() And 1) = 1 Then
                        _keyFrames.Add(i)
                    End If
                Next
            Catch ex As Exception
                Cleanup()
                Throw New FFMSException("Incompatible version of FFMS2.dll.", ex)
            End Try
        End Sub

        Private Sub GetKeyFrameList(ByVal indexFile As String)
            _keyFrames = New List(Of Integer)
            Using fs = File.OpenRead(indexFile), binr As New BinaryReader(fs)
                binr.BaseStream.Seek(4, SeekOrigin.Current)
                Dim indexVersion As Integer = binr.ReadInt32()
                Select Case indexVersion
                    Case 8 To 11
                        GetKeyFrameListGeneric(binr, 24, 28)
                    Case 28 To 31
                        GetKeyFrameListGeneric(binr, 68, 36)
                    Case Else
                        Cleanup()
                        Throw New FFMSException("Incompatible version of FFMS2.dll.")
                End Select
            End Using
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
        Private Function IndexCallback(ByVal State As Integer, ByVal Current As Long, ByVal Total As Long, ByVal [Private] As System.IntPtr) As Integer
            NotifyIndexProgress(Current, Total)
        End Function
        Private Function IndexCallback(ByVal Current As Long, ByVal Total As Long, ByVal [Private] As System.IntPtr) As Integer
            NotifyIndexProgress(Current, Total)
        End Function
        Private Sub ThrowException(ByVal sb As StringBuilder)
            Throw New FFMSException(sb.ToString())
        End Sub
        Private Sub CheckResult(ByVal result As Integer, ByVal errorMsg As StringBuilder)
            If result <> 0 Then
                Cleanup()
                ThrowException(errorMsg)
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
            MyBase.Dispose(disposing)
        End Sub

        Protected Overrides Sub Finalize()
            Dispose(False)
            MyBase.Finalize()
        End Sub
    End Class

End Namespace
