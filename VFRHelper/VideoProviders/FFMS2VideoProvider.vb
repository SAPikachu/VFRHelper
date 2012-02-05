Imports System.Runtime.InteropServices
Imports System.Text
Imports System.IO
Imports System.Drawing.Imaging
Imports System.Runtime.Serialization
Imports System.Security.Cryptography

Namespace VideoProviders

    Public Class FFMS2VideoProvider
        Inherits VideoProviderBase

        '<System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)> _
        'Public Structure FFMS_ErrorInfo

        '    '''int
        '    Public ErrorType As FFMS_Errors

        '    '''int
        '    Public SubType As Integer

        '    '''int
        '    Public BufferSize As Integer

        '    '''char*
        '    <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=1000, ArraySubType:=UnmanagedType.U1)> _
        '    Public Buffer() As Byte
        'End Structure

        Public Enum FFMS_Errors

            '''FFMS_ERROR_SUCCESS -> 0
            FFMS_ERROR_SUCCESS = 0

            '''FFMS_ERROR_INDEX -> 1
            FFMS_ERROR_INDEX = 1

            FFMS_ERROR_INDEXING

            FFMS_ERROR_POSTPROCESSING

            FFMS_ERROR_SCALING

            FFMS_ERROR_DECODING

            FFMS_ERROR_SEEKING

            FFMS_ERROR_PARSER

            FFMS_ERROR_TRACK

            FFMS_ERROR_WAVE_WRITER

            FFMS_ERROR_CANCELLED

            '''FFMS_ERROR_UNKNOWN -> 20
            FFMS_ERROR_UNKNOWN = 20

            FFMS_ERROR_UNSUPPORTED

            FFMS_ERROR_FILE_READ

            FFMS_ERROR_FILE_WRITE

            FFMS_ERROR_NO_FILE

            FFMS_ERROR_VERSION

            FFMS_ERROR_ALLOCATION_FAILED

            FFMS_ERROR_INVALID_ARGUMENT

            FFMS_ERROR_CODEC

            FFMS_ERROR_NOT_AVAILABLE

            FFMS_ERROR_FILE_MISMATCH

            FFMS_ERROR_USER
        End Enum

        Public Enum FFMS_Sources

            '''FFMS_SOURCE_DEFAULT -> 0x00
            FFMS_SOURCE_DEFAULT = 0

            '''FFMS_SOURCE_LAVF -> 0x01
            FFMS_SOURCE_LAVF = 1

            '''FFMS_SOURCE_MATROSKA -> 0x02
            FFMS_SOURCE_MATROSKA = 2

            '''FFMS_SOURCE_HAALIMPEG -> 0x04
            FFMS_SOURCE_HAALIMPEG = 4

            '''FFMS_SOURCE_HAALIOGG -> 0x08
            FFMS_SOURCE_HAALIOGG = 8
        End Enum

        Public Enum FFMS_CPUFeatures

            '''FFMS_CPU_CAPS_MMX -> 0x01
            FFMS_CPU_CAPS_MMX = 1

            '''FFMS_CPU_CAPS_MMX2 -> 0x02
            FFMS_CPU_CAPS_MMX2 = 2

            '''FFMS_CPU_CAPS_3DNOW -> 0x04
            FFMS_CPU_CAPS_3DNOW = 4

            '''FFMS_CPU_CAPS_ALTIVEC -> 0x08
            FFMS_CPU_CAPS_ALTIVEC = 8

            '''FFMS_CPU_CAPS_BFIN -> 0x10
            FFMS_CPU_CAPS_BFIN = 16

            '''FFMS_CPU_CAPS_SSE2 -> 0x20
            FFMS_CPU_CAPS_SSE2 = 32
        End Enum

        Public Enum FFMS_SeekMode

            '''FFMS_SEEK_LINEAR_NO_RW -> -1
            FFMS_SEEK_LINEAR_NO_RW = -1

            '''FFMS_SEEK_LINEAR -> 0
            FFMS_SEEK_LINEAR = 0

            '''FFMS_SEEK_NORMAL -> 1
            FFMS_SEEK_NORMAL = 1

            '''FFMS_SEEK_UNSAFE -> 2
            FFMS_SEEK_UNSAFE = 2

            '''FFMS_SEEK_AGGRESSIVE -> 3
            FFMS_SEEK_AGGRESSIVE = 3
        End Enum

        Public Enum FFMS_IndexErrorHandling

            '''FFMS_IEH_ABORT -> 0
            FFMS_IEH_ABORT = 0

            '''FFMS_IEH_CLEAR_TRACK -> 1
            FFMS_IEH_CLEAR_TRACK = 1

            '''FFMS_IEH_STOP_TRACK -> 2
            FFMS_IEH_STOP_TRACK = 2

            '''FFMS_IEH_IGNORE -> 3
            FFMS_IEH_IGNORE = 3
        End Enum

        Public Enum FFMS_TrackType

            '''FFMS_TYPE_UNKNOWN -> -1
            FFMS_TYPE_UNKNOWN = -1

            FFMS_TYPE_VIDEO

            FFMS_TYPE_AUDIO

            FFMS_TYPE_DATA

            FFMS_TYPE_SUBTITLE

            FFMS_TYPE_ATTACHMENT
        End Enum

        Public Enum FFMS_SampleFormat

            '''FFMS_FMT_U8 -> 0
            FFMS_FMT_U8 = 0

            FFMS_FMT_S16

            FFMS_FMT_S32

            FFMS_FMT_FLT

            FFMS_FMT_DBL
        End Enum

        Public Enum FFMS_AudioChannel

            '''FFMS_CH_FRONT_LEFT -> 0x00000001
            FFMS_CH_FRONT_LEFT = 1

            '''FFMS_CH_FRONT_RIGHT -> 0x00000002
            FFMS_CH_FRONT_RIGHT = 2

            '''FFMS_CH_FRONT_CENTER -> 0x00000004
            FFMS_CH_FRONT_CENTER = 4

            '''FFMS_CH_LOW_FREQUENCY -> 0x00000008
            FFMS_CH_LOW_FREQUENCY = 8

            '''FFMS_CH_BACK_LEFT -> 0x00000010
            FFMS_CH_BACK_LEFT = 16

            '''FFMS_CH_BACK_RIGHT -> 0x00000020
            FFMS_CH_BACK_RIGHT = 32

            '''FFMS_CH_FRONT_LEFT_OF_CENTER -> 0x00000040
            FFMS_CH_FRONT_LEFT_OF_CENTER = 64

            '''FFMS_CH_FRONT_RIGHT_OF_CENTER -> 0x00000080
            FFMS_CH_FRONT_RIGHT_OF_CENTER = 128

            '''FFMS_CH_BACK_CENTER -> 0x00000100
            FFMS_CH_BACK_CENTER = 256

            '''FFMS_CH_SIDE_LEFT -> 0x00000200
            FFMS_CH_SIDE_LEFT = 512

            '''FFMS_CH_SIDE_RIGHT -> 0x00000400
            FFMS_CH_SIDE_RIGHT = 1024

            '''FFMS_CH_TOP_CENTER -> 0x00000800
            FFMS_CH_TOP_CENTER = 2048

            '''FFMS_CH_TOP_FRONT_LEFT -> 0x00001000
            FFMS_CH_TOP_FRONT_LEFT = 4096

            '''FFMS_CH_TOP_FRONT_CENTER -> 0x00002000
            FFMS_CH_TOP_FRONT_CENTER = 8192

            '''FFMS_CH_TOP_FRONT_RIGHT -> 0x00004000
            FFMS_CH_TOP_FRONT_RIGHT = 16384

            '''FFMS_CH_TOP_BACK_LEFT -> 0x00008000
            FFMS_CH_TOP_BACK_LEFT = 32768

            '''FFMS_CH_TOP_BACK_CENTER -> 0x00010000
            FFMS_CH_TOP_BACK_CENTER = 65536

            '''FFMS_CH_TOP_BACK_RIGHT -> 0x00020000
            FFMS_CH_TOP_BACK_RIGHT = 131072

            '''FFMS_CH_STEREO_LEFT -> 0x20000000
            FFMS_CH_STEREO_LEFT = 536870912

            '''FFMS_CH_STEREO_RIGHT -> 0x40000000
            FFMS_CH_STEREO_RIGHT = 1073741824
        End Enum

        Public Enum FFMS_Resizers

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

        Public Enum FFMS_AudioDelayModes

            '''FFMS_DELAY_NO_SHIFT -> -3
            FFMS_DELAY_NO_SHIFT = -3

            '''FFMS_DELAY_TIME_ZERO -> -2
            FFMS_DELAY_TIME_ZERO = -2

            '''FFMS_DELAY_FIRST_VIDEO_TRACK -> -1
            FFMS_DELAY_FIRST_VIDEO_TRACK = -1
        End Enum

        <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)> _
        Public Structure FFMS_Frame

            '''uint8_t*[4]
            <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=4, ArraySubType:=System.Runtime.InteropServices.UnmanagedType.SysUInt)> _
            Public Data() As System.IntPtr

            '''int[4]
            <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=4, ArraySubType:=System.Runtime.InteropServices.UnmanagedType.I4)> _
            Public Linesize() As Integer

            '''int
            Public EncodedWidth As Integer

            '''int
            Public EncodedHeight As Integer

            '''int
            Public EncodedPixelFormat As Integer

            '''int
            Public ScaledWidth As Integer

            '''int
            Public ScaledHeight As Integer

            '''int
            Public ConvertedPixelFormat As Integer

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
        End Structure

        <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)> _
        Public Structure FFMS_TrackTimeBase

            '''int64_t->__int64
            Public Num As Long

            '''int64_t->__int64
            Public Den As Long
        End Structure

        <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)> _
        Public Structure FFMS_FrameInfo

            '''int64_t->__int64
            Public PTS As Long

            '''int
            Public RepeatPict As Integer

            '''int
            Public KeyFrame As Integer
        End Structure

        <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)> _
        Public Structure FFMS_VideoProperties

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

        <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)> _
        Public Structure FFMS_AudioProperties

            '''int
            Public SampleFormat As Integer

            '''int
            Public SampleRate As Integer

            '''int
            Public BitsPerSample As Integer

            '''int
            Public Channels As Integer

            '''int64_t->__int64
            Public ChannelLayout As Long

            '''int64_t->__int64
            Public NumSamples As Long

            '''double
            Public FirstTime As Double

            '''double
            Public LastTime As Double
        End Structure

        '''Return Type: int
        '''Current: int64_t->__int64
        '''Total: int64_t->__int64
        '''ICPrivate: void*
        <System.Runtime.InteropServices.UnmanagedFunctionPointerAttribute(System.Runtime.InteropServices.CallingConvention.StdCall)> _
        Public Delegate Function TIndexCallback(ByVal Current As Long, ByVal Total As Long, ByVal ICPrivate As System.IntPtr) As Integer

        '''Return Type: int
        '''SourceFile: char*
        '''Track: int
        '''AP: FFMS_AudioProperties*
        '''FileName: char*
        '''FNSize: int
        '''Private: void*
        <System.Runtime.InteropServices.UnmanagedFunctionPointerAttribute(System.Runtime.InteropServices.CallingConvention.StdCall)> _
        Public Delegate Function TAudioNameCallback(<System.Runtime.InteropServices.InAttribute(), System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)> ByVal SourceFile As String, ByVal Track As Integer, ByRef AP As FFMS_AudioProperties, ByVal FileName As System.IntPtr, ByVal FNSize As Integer, ByVal [Private] As System.IntPtr) As Integer



        Partial Public Class FFMS

            '''Return Type: void
            '''CPUFeatures: int
            '''UseUTF8Paths: int
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_Init", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Sub Init(ByVal CPUFeatures As Integer, ByVal UseUTF8Paths As Integer)
            End Sub

            '''Return Type: int
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_GetVersion", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Function GetVersion() As Integer
            End Function

            '''Return Type: int
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_GetLogLevel", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Function GetLogLevel() As Integer
            End Function

            '''Return Type: void
            '''Level: int
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_SetLogLevel", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Sub SetLogLevel(ByVal Level As Integer)
            End Sub

            '''Return Type: FFMS_VideoSource*
            '''SourceFile: char*
            '''Track: int
            '''Index: FFMS_Index*
            '''Threads: int
            '''SeekMode: int
            '''ErrorInfo: FFMS_ErrorInfo*
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_CreateVideoSource", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Function CreateVideoSource(<System.Runtime.InteropServices.InAttribute(), System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)> ByVal SourceFile As String, ByVal Track As Integer, ByVal Index As System.IntPtr, ByVal Threads As Integer, ByVal SeekMode As Integer, ByVal ErrorInfo As IntPtr) As System.IntPtr
            End Function

            '''Return Type: FFMS_AudioSource*
            '''SourceFile: char*
            '''Track: int
            '''Index: FFMS_Index*
            '''DelayMode: int
            '''ErrorInfo: FFMS_ErrorInfo*
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_CreateAudioSource", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Function CreateAudioSource(<System.Runtime.InteropServices.InAttribute(), System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)> ByVal SourceFile As String, ByVal Track As Integer, ByVal Index As System.IntPtr, ByVal DelayMode As Integer, ByVal ErrorInfo As IntPtr) As System.IntPtr
            End Function

            '''Return Type: void
            '''V: FFMS_VideoSource*
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_DestroyVideoSource", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Sub DestroyVideoSource(ByVal V As System.IntPtr)
            End Sub

            '''Return Type: void
            '''A: FFMS_AudioSource*
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_DestroyAudioSource", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Sub DestroyAudioSource(ByVal A As System.IntPtr)
            End Sub

            '''Return Type: FFMS_VideoProperties*
            '''V: FFMS_VideoSource*
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_GetVideoProperties", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Function GetVideoProperties(ByVal V As System.IntPtr) As System.IntPtr
            End Function

            '''Return Type: FFMS_AudioProperties*
            '''A: FFMS_AudioSource*
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_GetAudioProperties", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Function GetAudioProperties(ByVal A As System.IntPtr) As System.IntPtr
            End Function

            '''Return Type: FFMS_Frame*
            '''V: FFMS_VideoSource*
            '''n: int
            '''ErrorInfo: FFMS_ErrorInfo*
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_GetFrame", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Function GetFrame(ByVal V As System.IntPtr, ByVal n As Integer, ByVal ErrorInfo As IntPtr) As System.IntPtr
            End Function

            '''Return Type: FFMS_Frame*
            '''V: FFMS_VideoSource*
            '''Time: double
            '''ErrorInfo: FFMS_ErrorInfo*
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_GetFrameByTime", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Function GetFrameByTime(ByVal V As System.IntPtr, ByVal Time As Double, ByVal ErrorInfo As IntPtr) As System.IntPtr
            End Function

            '''Return Type: int
            '''A: FFMS_AudioSource*
            '''Buf: void*
            '''Start: int64_t->__int64
            '''Count: int64_t->__int64
            '''ErrorInfo: FFMS_ErrorInfo*
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_GetAudio", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Function GetAudio(ByVal A As System.IntPtr, ByVal Buf As System.IntPtr, ByVal Start As Long, ByVal Count As Long, ByVal ErrorInfo As IntPtr) As Integer
            End Function

            '''Return Type: int
            '''V: FFMS_VideoSource*
            '''TargetFormats: int64_t->__int64
            '''Width: int
            '''Height: int
            '''Resizer: int
            '''ErrorInfo: FFMS_ErrorInfo*
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_SetOutputFormatV", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Function SetOutputFormatV(ByVal V As System.IntPtr, ByVal TargetFormats As Long, ByVal Width As Integer, ByVal Height As Integer, ByVal Resizer As Integer, ByVal ErrorInfo As IntPtr) As Integer
            End Function

            '''Return Type: int
            '''V: FFMS_VideoSource*
            '''TargetFormats: int*
            '''Width: int
            '''Height: int
            '''Resizer: int
            '''ErrorInfo: FFMS_ErrorInfo*
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_SetOutputFormatV2", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Function SetOutputFormatV2(ByVal V As System.IntPtr, ByRef TargetFormats As Integer, ByVal Width As Integer, ByVal Height As Integer, ByVal Resizer As Integer, ByVal ErrorInfo As IntPtr) As Integer
            End Function

            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_SetOutputFormatV2", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Function SetOutputFormatV2(ByVal V As System.IntPtr, ByRef TargetFormats As Long, ByVal Width As Integer, ByVal Height As Integer, ByVal Resizer As Integer, ByVal ErrorInfo As IntPtr) As Integer
            End Function

            '''Return Type: void
            '''V: FFMS_VideoSource*
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_ResetOutputFormatV", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Sub ResetOutputFormatV(ByVal V As System.IntPtr)
            End Sub

            '''Return Type: int
            '''V: FFMS_VideoSource*
            '''PP: char*
            '''ErrorInfo: FFMS_ErrorInfo*
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_SetPP", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Function SetPP(ByVal V As System.IntPtr, <System.Runtime.InteropServices.InAttribute(), System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)> ByVal PP As String, ByVal ErrorInfo As IntPtr) As Integer
            End Function

            '''Return Type: void
            '''V: FFMS_VideoSource*
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_ResetPP", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Sub ResetPP(ByVal V As System.IntPtr)
            End Sub

            '''Return Type: void
            '''Index: FFMS_Index*
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_DestroyIndex", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Sub DestroyIndex(ByVal Index As System.IntPtr)
            End Sub

            '''Return Type: int
            '''Index: FFMS_Index*
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_GetSourceType", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Function GetSourceType(ByVal Index As System.IntPtr) As Integer
            End Function

            '''Return Type: int
            '''Indexer: FFMS_Indexer*
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_GetSourceTypeI", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Function GetSourceTypeI(ByVal Indexer As System.IntPtr) As Integer
            End Function

            '''Return Type: int
            '''Index: FFMS_Index*
            '''TrackType: int
            '''ErrorInfo: FFMS_ErrorInfo*
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_GetFirstTrackOfType", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Function GetFirstTrackOfType(ByVal Index As System.IntPtr, ByVal TrackType As Integer, ByVal ErrorInfo As IntPtr) As Integer
            End Function

            '''Return Type: int
            '''Index: FFMS_Index*
            '''TrackType: int
            '''ErrorInfo: FFMS_ErrorInfo*
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_GetFirstIndexedTrackOfType", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Function GetFirstIndexedTrackOfType(ByVal Index As System.IntPtr, ByVal TrackType As Integer, ByVal ErrorInfo As IntPtr) As Integer
            End Function

            '''Return Type: int
            '''Index: FFMS_Index*
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_GetNumTracks", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Function GetNumTracks(ByVal Index As System.IntPtr) As Integer
            End Function

            '''Return Type: int
            '''Indexer: FFMS_Indexer*
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_GetNumTracksI", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Function GetNumTracksI(ByVal Indexer As System.IntPtr) As Integer
            End Function

            '''Return Type: int
            '''T: FFMS_Track*
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_GetTrackType", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Function GetTrackType(ByVal T As System.IntPtr) As Integer
            End Function

            '''Return Type: int
            '''Indexer: FFMS_Indexer*
            '''Track: int
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_GetTrackTypeI", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Function GetTrackTypeI(ByVal Indexer As System.IntPtr, ByVal Track As Integer) As Integer
            End Function

            '''Return Type: char*
            '''Indexer: FFMS_Indexer*
            '''Track: int
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_GetCodecNameI", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Function GetCodecNameI(ByVal Indexer As System.IntPtr, ByVal Track As Integer) As System.IntPtr
            End Function

            '''Return Type: char*
            '''Indexer: FFMS_Indexer*
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_GetFormatNameI", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Function GetFormatNameI(ByVal Indexer As System.IntPtr) As System.IntPtr
            End Function

            '''Return Type: int
            '''T: FFMS_Track*
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_GetNumFrames", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Function GetNumFrames(ByVal T As System.IntPtr) As Integer
            End Function

            '''Return Type: FFMS_FrameInfo*
            '''T: FFMS_Track*
            '''Frame: int
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_GetFrameInfo", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Function GetFrameInfo(ByVal T As System.IntPtr, ByVal Frame As Integer) As System.IntPtr
            End Function

            '''Return Type: FFMS_Track*
            '''Index: FFMS_Index*
            '''Track: int
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_GetTrackFromIndex", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Function GetTrackFromIndex(ByVal Index As System.IntPtr, ByVal Track As Integer) As System.IntPtr
            End Function

            '''Return Type: FFMS_Track*
            '''V: FFMS_VideoSource*
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_GetTrackFromVideo", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Function GetTrackFromVideo(ByVal V As System.IntPtr) As System.IntPtr
            End Function

            '''Return Type: FFMS_Track*
            '''A: FFMS_AudioSource*
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_GetTrackFromAudio", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Function GetTrackFromAudio(ByVal A As System.IntPtr) As System.IntPtr
            End Function

            '''Return Type: FFMS_TrackTimeBase*
            '''T: FFMS_Track*
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_GetTimeBase", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Function GetTimeBase(ByVal T As System.IntPtr) As System.IntPtr
            End Function

            '''Return Type: int
            '''T: FFMS_Track*
            '''TimecodeFile: char*
            '''ErrorInfo: FFMS_ErrorInfo*
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_WriteTimecodes", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Function WriteTimecodes(ByVal T As System.IntPtr, <System.Runtime.InteropServices.InAttribute(), System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)> ByVal TimecodeFile As String, ByVal ErrorInfo As IntPtr) As Integer
            End Function

            '''Return Type: FFMS_Index*
            '''SourceFile: char*
            '''IndexMask: int
            '''DumpMask: int
            '''ANC: TAudioNameCallback
            '''ANCPrivate: void*
            '''ErrorHandling: int
            '''IC: TIndexCallback
            '''ICPrivate: void*
            '''ErrorInfo: FFMS_ErrorInfo*
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_MakeIndex", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Function MakeIndex(<System.Runtime.InteropServices.InAttribute(), System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)> ByVal SourceFile As String, ByVal IndexMask As Integer, ByVal DumpMask As Integer, ByVal ANC As TAudioNameCallback, ByVal ANCPrivate As System.IntPtr, ByVal ErrorHandling As Integer, ByVal IC As TIndexCallback, ByVal ICPrivate As System.IntPtr, ByVal ErrorInfo As IntPtr) As System.IntPtr
            End Function

            '''Return Type: int
            '''SourceFile: char*
            '''Track: int
            '''AP: FFMS_AudioProperties*
            '''FileName: char*
            '''FNSize: int
            '''Private: void*
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_DefaultAudioFilename", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Function DefaultAudioFilename(<System.Runtime.InteropServices.InAttribute(), System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)> ByVal SourceFile As String, ByVal Track As Integer, ByRef AP As FFMS_AudioProperties, ByVal FileName As System.IntPtr, ByVal FNSize As Integer, ByVal [Private] As System.IntPtr) As Integer
            End Function

            '''Return Type: FFMS_Indexer*
            '''SourceFile: char*
            '''ErrorInfo: FFMS_ErrorInfo*
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_CreateIndexer", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Function CreateIndexer(<System.Runtime.InteropServices.InAttribute(), System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)> ByVal SourceFile As String, ByVal ErrorInfo As IntPtr) As System.IntPtr
            End Function

            '''Return Type: FFMS_Indexer*
            '''SourceFile: char*
            '''Demuxer: int
            '''ErrorInfo: FFMS_ErrorInfo*
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_CreateIndexerWithDemuxer", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Function CreateIndexerWithDemuxer(<System.Runtime.InteropServices.InAttribute(), System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)> ByVal SourceFile As String, ByVal Demuxer As Integer, ByVal ErrorInfo As IntPtr) As System.IntPtr
            End Function

            '''Return Type: FFMS_Index*
            '''Indexer: FFMS_Indexer*
            '''IndexMask: int
            '''DumpMask: int
            '''ANC: TAudioNameCallback
            '''ANCPrivate: void*
            '''ErrorHandling: int
            '''IC: TIndexCallback
            '''ICPrivate: void*
            '''ErrorInfo: FFMS_ErrorInfo*
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_DoIndexing", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Function DoIndexing(ByVal Indexer As System.IntPtr, ByVal IndexMask As Integer, ByVal DumpMask As Integer, ByVal ANC As TAudioNameCallback, ByVal ANCPrivate As System.IntPtr, ByVal ErrorHandling As Integer, ByVal IC As TIndexCallback, ByVal ICPrivate As System.IntPtr, ByVal ErrorInfo As IntPtr) As System.IntPtr
            End Function

            '''Return Type: void
            '''Indexer: FFMS_Indexer*
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_CancelIndexing", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Sub CancelIndexing(ByVal Indexer As System.IntPtr)
            End Sub

            '''Return Type: FFMS_Index*
            '''IndexFile: char*
            '''ErrorInfo: FFMS_ErrorInfo*
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_ReadIndex", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Function ReadIndex(<System.Runtime.InteropServices.InAttribute(), System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)> ByVal IndexFile As String, ByVal ErrorInfo As IntPtr) As System.IntPtr
            End Function

            '''Return Type: int
            '''Index: FFMS_Index*
            '''SourceFile: char*
            '''ErrorInfo: FFMS_ErrorInfo*
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_IndexBelongsToFile", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Function IndexBelongsToFile(ByVal Index As System.IntPtr, <System.Runtime.InteropServices.InAttribute(), System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)> ByVal SourceFile As String, ByVal ErrorInfo As IntPtr) As Integer
            End Function

            '''Return Type: int
            '''IndexFile: char*
            '''Index: FFMS_Index*
            '''ErrorInfo: FFMS_ErrorInfo*
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_WriteIndex", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Function WriteIndex(<System.Runtime.InteropServices.InAttribute(), System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)> ByVal IndexFile As String, ByVal Index As System.IntPtr, ByVal ErrorInfo As IntPtr) As Integer
            End Function

            '''Return Type: int
            '''Name: char*
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_GetPixFmt", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Function GetPixFmt(<System.Runtime.InteropServices.InAttribute(), System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)> ByVal Name As String) As Integer
            End Function

            '''Return Type: int
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_GetPresentSources", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Function GetPresentSources() As Integer
            End Function

            '''Return Type: int
            <System.Runtime.InteropServices.DllImportAttribute("ffms2.dll", EntryPoint:="FFMS_GetEnabledSources", CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)> _
            Public Shared Function GetEnabledSources() As Integer
            End Function
        End Class

        Partial Structure FFMS_Frame

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

        Dim _videoProperties As FFMS_VideoProperties
        ''' <summary>
        ''' VideoBase
        ''' </summary>
        ''' <remarks></remarks>
        Dim _VS As IntPtr
        Dim _currentFrameCache As FFMS_Frame
        Dim _keyFrames As List(Of Integer)
        Dim _errorInfo As IntPtr
        Const ERRORINFO_BUFFER_SIZE As Integer = 2048

        Shared Sub New()
            FFMS.Init(0, 0)
        End Sub
        Public Sub New()
            _errorInfo = Marshal.AllocCoTaskMem(ERRORINFO_BUFFER_SIZE)
            Marshal.WriteInt32(_errorInfo, 0, FFMS_Errors.FFMS_ERROR_SUCCESS)
            Marshal.WriteInt32(_errorInfo, 4, 0)
            Marshal.WriteInt32(_errorInfo, 8, ERRORINFO_BUFFER_SIZE - 64)
            Marshal.WriteIntPtr(_errorInfo, 12, New IntPtr(_errorInfo.ToInt64() + 32))
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
            _currentFrameCache = CType(Marshal.PtrToStructure(framePtr, GetType(FFMS_Frame)), FFMS_Frame)
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
            index = FFMS.MakeIndex(filePath, 0, 0, Nothing, IntPtr.Zero, FFMS_IndexErrorHandling.FFMS_IEH_ABORT, AddressOf IndexCallback, IntPtr.Zero, _errorInfo)
            CheckResult()
            Return index
        End Function
        Private Shared Sub DestroyIndex(ByVal index As IntPtr)
            FFMS.DestroyIndex(index)
        End Sub
        Private Shared Function GetTrackFromIndex(ByVal index As IntPtr, ByVal trackNumber As Integer) As IntPtr
            Return FFMS.GetTrackFromIndex(index, trackNumber)
        End Function
        Private Function MakeV2SinglePixelFormat(name As String) As Long
            Dim fmt = FFMS.GetPixFmt(name)
            If fmt = -1 Then
                Throw New ArgumentException(name & " is not a valid pixel format.", "name")
            End If
            Return fmt Or &HFFFFFFFF00000000L
        End Function
        Private Sub SetCorrectOutputFormat()
            DecodeCurrentFrame()
            Dim pixelFormat = MakeV2SinglePixelFormat("bgr24")
            FFMS.SetOutputFormatV2(_VS, pixelFormat, _currentFrameCache.OutputWidth, _currentFrameCache.OutputHeight, FFMS_Resizers.FFMS_RESIZER_FAST_BILINEAR, _errorInfo)
            CheckResult()
            _currentFrameCache = Nothing
        End Sub
        Private Sub GetVideoProperties()
            Dim vpPtr As IntPtr = FFMS.GetVideoProperties(_VS)
            _videoProperties = CType(Marshal.PtrToStructure(vpPtr, GetType(FFMS_VideoProperties)), FFMS_VideoProperties)
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
                Dim trackNumber = FFMS.GetFirstTrackOfType(index, FFMS_TrackType.FFMS_TYPE_VIDEO, _errorInfo)
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
                SetCorrectOutputFormat()
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
                Dim frameInfo = CType(Marshal.PtrToStructure(frameInfoPtr, GetType(FFMS_FrameInfo)), FFMS_FrameInfo)
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
            Throw New FFMSException(String.Format("{0} Type={1}, SubType={2}", Marshal.PtrToStringAnsi(New IntPtr(_errorInfo.ToInt64() + 32)), CType(Marshal.ReadInt32(_errorInfo), FFMS_Errors).ToString, Marshal.ReadInt32(_errorInfo, 4).ToString))
        End Sub
        Private Sub CheckResult()
            If Marshal.ReadInt32(_errorInfo) <> FFMS_Errors.FFMS_ERROR_SUCCESS Then
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
            Marshal.FreeCoTaskMem(_errorInfo)
            _errorInfo = IntPtr.Zero
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
