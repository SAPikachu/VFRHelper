Imports System.Text.RegularExpressions

Public Class TfmFrameInfoManager
    Implements IDisposable

    Dim _infoCache As New Dictionary(Of String, String)
    Dim _outputCache As String
    Dim _outputCacheFrameNumber As Integer = -1
    Dim _currentProcessId As Integer = Process.GetCurrentProcess().Id

    Dim _contentRe As New Regex("^TFM:\s+frame (?<frameNumber>\d+)\s+-\s+(?<content>(diffp|final match|mode|mics|CLEAN|COMBED).+)$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Dim _scenecutRe As New Regex(".*diffmaxsc = \d+\s+(?<isSC>T|F).*", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Dim _d2vFilmRe As New Regex(".*d2vfilm = (?<d2vfilm>T|F).*", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Dim _ovrMatchRe As New Regex("final match = (?<match>[pcnbu]) .*OVR.*", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Dim _ovrCombedRe As New Regex("(?<isCombed>CLEAN|COMBED) FRAME .*forced.*", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)

    Public Event FrameInfoUpdated As EventHandler

    Public Sub New()
        AddHandler DebugMonitor.OnOutputDebugString, AddressOf OnOutputDebugString
        DebugMonitor.Start()
    End Sub

    Public Sub ClearCache()
        _infoCache.Clear()
    End Sub

    Public Function GetFrameInfo(ByVal frameNumber As Integer, ByVal ovrMatch As String, ByVal ovrCombed As Boolean?) As String
        Dim key = GetCacheKey(frameNumber, ovrMatch, ovrCombed)
        Dim result = ""
        SyncLock _infoCache
            If Not _infoCache.TryGetValue(key, result) Then
                Return ""
            End If
        End SyncLock
        Return result
    End Function
    Private Shared Function GetCacheKey(ByVal frameNumber As Integer, ByVal ovrMatch As String, ByVal ovrCombed As Boolean?) As String
        Return String.Format("{0}_{1}_{2}", frameNumber, ovrMatch, If(ovrCombed.HasValue, ovrCombed.Value.ToString(), ""))
    End Function
    Private Sub UpdateInfo()
        Dim ovrMatch = ""
        Dim ovrCombed As Boolean?
        Dim info = ""
        Dim additionalLine = ""
        For Each line In _outputCache.Split({vbCrLf}, StringSplitOptions.RemoveEmptyEntries)
            Dim m = _scenecutRe.Match(line)
            If m.Success Then
                additionalLine += String.Format("Scenecut: {0} ", m.Groups("isSC").Value)
                Continue For
            End If
            m = _d2vFilmRe.Match(line)
            If m.Success Then
                additionalLine += String.Format("d2vfilm: {0} ", m.Groups("d2vfilm").Value)
                Continue For
            End If
            info += line + vbCrLf
            m = _ovrMatchRe.Match(line)
            If m.Success Then
                ovrMatch = m.Groups("match").Value
                Continue For
            End If
            m = _ovrCombedRe.Match(line)
            If m.Success Then
                ovrCombed = m.Groups("isCombed").Value = "COMBED"
                Continue For
            End If
        Next
        If Not String.IsNullOrEmpty(additionalLine) Then
            info += additionalLine
        End If
        Dim key = GetCacheKey(_outputCacheFrameNumber, ovrMatch, ovrCombed)
        SyncLock _infoCache
            _infoCache(key) = info.Trim()
        End SyncLock
        _outputCacheFrameNumber = -1
        _outputCache = ""
        RaiseEvent FrameInfoUpdated(Me, EventArgs.Empty)
    End Sub
    Private Sub OnOutputDebugString(pid As Integer, s As String)
        If pid <> _currentProcessId Then
            Return
        End If
        If Not s.StartsWith("TFM:") Then
            Return
        End If
        Dim m = _contentRe.Match(s)
        If m.Success Then
            Dim frameNumber = Integer.Parse(m.Groups("frameNumber").Value)
            If _outputCacheFrameNumber <> -1 AndAlso frameNumber <> _outputCacheFrameNumber Then
                UpdateInfo()
            End If
            _outputCacheFrameNumber = frameNumber
            Dim content As String = m.Groups("content").Value.Trim()
            _outputCache += content + vbCrLf
            If content.IndexOf("FRAME") > -1 Then
                UpdateInfo()
            End If
        End If
    End Sub

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                RemoveHandler DebugMonitor.OnOutputDebugString, AddressOf OnOutputDebugString
                DebugMonitor.Stop()
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
