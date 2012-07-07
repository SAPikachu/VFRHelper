Imports SAPStudio.VFRHelper.VideoProviders
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions

Public Class TfmVideoProvider
    Inherits AviSynthVideoProvider
    Shared ReadOnly matchCodes() As String = {"p", "c", "n", "b", "u"}
    Dim _frameOptions As New SortedDictionary(Of Integer, FrameOption)
    Public Event Seek As EventHandler
    Dim _keyFrames As List(Of Integer)
    Private Shared ReadOnly _frameNumberRe As New Regex("^\#\s*(?<frameNumber>\d+)\s+(\(\d+\)|[pcnbu])\s*$", RegexOptions.Singleline Or RegexOptions.ExplicitCapture)
    Public Overrides ReadOnly Property CurrentFrameType() As VideoProviders.FrameType
        Get
            If _keyFrames Is Nothing Then Return MyBase.CurrentFrameType
            Return If(_keyFrames.Contains(CurrentFrameNumber), VideoProviders.FrameType.Key, VideoProviders.FrameType.Normal)
        End Get
    End Property
    Public Overrides Function SeekToNextKeyFrame() As Boolean
        If _keyFrames Is Nothing Then Return MyBase.SeekToNextKeyFrame()
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
        If _keyFrames Is Nothing Then Return MyBase.SeekToPrevKeyFrame()
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
    Private Sub ReadFrameList(list As IList(Of Integer), sr As StreamReader, header As String)
        Dim line As String
        sr.BaseStream.Seek(0, SeekOrigin.Begin)
        Do
            line = sr.ReadLine()
            If sr.EndOfStream Then
                Return
            End If
        Loop Until line.Substring(1).Trim() = header
        Do
            line = sr.ReadLine()
            Dim match = _frameNumberRe.Match(line)
            If match.Success Then
                list.Add(Integer.Parse(match.Groups("frameNumber").Value))
            End If
        Loop Until line.Substring(1).Trim().StartsWith("[") OrElse sr.EndOfStream
    End Sub
    Public Sub ReadTFMAnalysisFile(
        ByVal filePath As String,
        ByVal readCombedFrames As Boolean,
        ByVal readPossiblyCombedFrames As Boolean,
        ByVal readUBNFrames As Boolean)
        Try
            Using sr = File.OpenText(filePath)
                If Not sr.ReadLine().StartsWith("#TFM") Then
                    Throw New InvalidDataException("Not a valid TFM analysis file.")
                End If
                _keyFrames = New List(Of Integer)
                If readCombedFrames Then
                    ReadFrameList(_keyFrames, sr, "[Individual Frames]")
                End If
                If readPossiblyCombedFrames Then
                    ReadFrameList(_keyFrames, sr, "[POSSIBLE MISSED COMBED FRAMES]")
                End If
                If readUBNFrames Then
                    ReadFrameList(_keyFrames, sr, "[u, b, AND AGAINST ORDER (n) MATCHES]")
                End If
                _keyFrames.Sort()
            End Using
        Catch ex As NullReferenceException
            Throw New InvalidOperationException("Unable to open the TFM analysis file, it may be locked by other application.")
        End Try
    End Sub
    Public Sub SetFrameOptionsFromOptionGroups(ByVal ogs As IList(Of FrameOptionGroup))
        For Each group In ogs
            For i = group.Start To group.End
                Dim [option] As FrameOption = If(_frameOptions.ContainsKey(i), _frameOptions(i), New FrameOption)
                If Not String.IsNullOrEmpty(group.Option.MatchCode) Then
                    [option].MatchCode = group.Option.MatchCode.Chars((i - group.Start) Mod group.Option.MatchCode.Length)
                End If
                If group.Option.IsCombed IsNot Nothing Then
                    [option].IsCombed = New Boolean() {group.Option.IsCombed((i - group.Start) Mod group.Option.IsCombed.Length)}
                End If
                If group.Option.OtherOptions IsNot Nothing Then
                    If [option].OtherOptions Is Nothing Then
                        [option].OtherOptions = group.Option.OtherOptions
                    Else
                        Dim temp As New List(Of String)
                        temp.AddRange([option].OtherOptions)
                        temp.AddRange(group.Option.OtherOptions)
                        [option].OtherOptions = temp.ToArray()
                    End If
                End If
                _frameOptions(i) = [option]
            Next
        Next
    End Sub
    'Private Function PrepareOptionGroupsForOutput() As FrameOptionGroup()
    '    Dim groupList As New List(Of FrameOptionGroup)
    '    Dim combingStatus As Boolean(), combingStatusStartFrame As Integer
    '    Dim otherOptions As String() = Nothing, otherOptionsStartFrame As Integer
    '    Dim lastFrame = -1
    '    For Each pair In _frameOptions
    '        Dim addCombing = False, addOtherOptions = False
    '        If pair.Key - 1 > lastFrame Then
    '            addCombing = combingStatus IsNot Nothing
    '            addOtherOptions = otherOptions IsNot Nothing
    '        Else
    '            addCombing = combingStatus IsNot Nothing AndAlso Not ArrayEquals(combingStatus, pair.Value.IsCombed)
    '            addOtherOptions = otherOptions IsNot Nothing AndAlso Not ArrayEquals(otherOptions, pair.Value.OtherOptions)
    '        End If
    '        If addCombing Then
    '            groupList.Add(New FrameOptionGroup With { _
    '                                                .Start = combingStatusStartFrame, _
    '                                                .End = lastFrame, _
    '                                                .Option = New FrameOption With {.IsCombed = combingStatus}})
    '            combingStatus = Nothing
    '        End If
    '        If addOtherOptions Then
    '            For Each opt In otherOptions
    '                groupList.Add(New FrameOptionGroup With { _
    '                                                    .Start = otherOptionsStartFrame, _
    '                                                    .End = lastFrame, _
    '                                                    .Option = New FrameOption With {.OtherOptions = New String() {opt}}})
    '            Next
    '            otherOptions = Nothing
    '        End If
    '        If Not String.IsNullOrEmpty(pair.Value.MatchCode) Then
    '            groupList.Add(New FrameOptionGroup With { _
    '                                                .Start = pair.Key, _
    '                                                .End = pair.Key, _
    '                                                .Option = New FrameOption With {.MatchCode = pair.Value.MatchCode}})
    '        End If
    '        If combingStatus Is Nothing Then
    '            combingStatus = pair.Value.IsCombed
    '            combingStatusStartFrame = pair.Key
    '        End If
    '        If otherOptions Is Nothing Then
    '            otherOptions = pair.Value.OtherOptions
    '            otherOptionsStartFrame = pair.Key
    '        End If
    '        lastFrame = pair.Key
    '    Next
    '    If combingStatus IsNot Nothing Then
    '        groupList.Add(New FrameOptionGroup With { _
    '                                            .Start = combingStatusStartFrame, _
    '                                            .End = lastFrame, _
    '                                            .Option = New FrameOption With {.IsCombed = combingStatus}})
    '        combingStatus = Nothing
    '    End If
    '    If otherOptions IsNot Nothing Then
    '        For Each opt In otherOptions
    '            groupList.Add(New FrameOptionGroup With { _
    '                                                .Start = otherOptionsStartFrame, _
    '                                                .End = lastFrame, _
    '                                                .Option = New FrameOption With {.OtherOptions = New String() {opt}}})
    '        Next
    '        otherOptions = Nothing
    '    End If
    '    groupList.Sort(Function(x, y) x.Start - y.Start)
    '    Return groupList.ToArray()
    'End Function
    Public Function ReadOverrideFile(ByVal filePath As String) As List(Of FrameOptionGroup)
        Dim groupList As New List(Of FrameOptionGroup)
        Dim ovrLine As New Regex("^(?<startFrame>\d+)(,(?<endFrame>\d+))? ((?<matchCodes>[pcnbu]+)|(?<isCombed>(\+|-)+)|(?<otherOption>[fomMPi] -?\d+))$", RegexOptions.Singleline Or RegexOptions.ExplicitCapture)
        Using sr As StreamReader = File.OpenText(filePath)
            Do Until sr.EndOfStream
                Dim line = sr.ReadLine().TrimEnd()
                If String.IsNullOrEmpty(line) OrElse line.StartsWith("#") OrElse line.StartsWith(";") Then
                    Continue Do
                End If
                Dim match = ovrLine.Match(line)
                If Not match.Success Then
                    Throw New ApplicationException("Invalid override line: " & line)
                End If
                Dim group As New FrameOptionGroup
                group.Start = Integer.Parse(match.Groups("startFrame").Value)
                If match.Groups("endFrame").Length = 0 Then
                    group.End = group.Start
                ElseIf match.Groups("endFrame").Value = "0" Then
                    group.End = FrameCount - 1
                Else
                    group.End = Integer.Parse(match.Groups("endFrame").Value)
                End If
                group.Option = New FrameOption
                If match.Groups("matchCodes").Length > 0 Then
                    group.Option.MatchCode = match.Groups("matchCodes").Value
                ElseIf match.Groups("isCombed").Length > 0 Then
                    group.Option.IsCombed = Array.ConvertAll(Of Char, Boolean)(match.Groups("isCombed").Value.ToCharArray(), Function(ch) ch = "+"c)
                ElseIf match.Groups("otherOption").Length > 0 Then
                    group.Option.OtherOptions = New String() {match.Groups("otherOption").Value}
                End If
                groupList.Add(group)
            Loop
        End Using
        SetFrameOptionsFromOptionGroups(groupList.ToArray())
        Return groupList
    End Function

    'Public Sub WriteOverrideFile(ByVal filePath As String)
    '    Dim groupList = PrepareOptionGroupsForOutput()
    '    Using fs As New FileStream(filePath, FileMode.Create), swr As New StreamWriter(fs, Encoding.ASCII)
    '        For Each group In groupList
    '            swr.WriteLine(group.ToString())
    '        Next
    '    End Using
    'End Sub

    Public Sub SetCurrentFrameOption(ByVal matchCode As String, ByVal combed As Boolean?)
        Debug.Assert(matchCode Is Nothing OrElse matchCode.Length = 1)
        Dim [option] As FrameOption = If(_frameOptions.ContainsKey(CurrentFrameNumber), _frameOptions(CurrentFrameNumber), New FrameOption)
        [option].MatchCode = matchCode
        [option].IsCombed = If(combed.HasValue, New Boolean() {combed.Value}, Nothing)
        _frameOptions(CurrentFrameNumber) = [option]
        SeekTo(CurrentFrameNumber)
    End Sub
    Public Sub SetFrameRangeOptions(ByVal optionGroup As FrameOptionGroup)
        For i = optionGroup.Start To optionGroup.End
            Dim [option] As FrameOption = If(_frameOptions.ContainsKey(i), _frameOptions(i), New FrameOption)
            [option].MatchCode = If(String.IsNullOrEmpty(optionGroup.Option.MatchCode), Nothing, optionGroup.Option.MatchCode.Chars((i - optionGroup.Start) Mod optionGroup.Option.MatchCode.Length))
            [option].IsCombed = If(optionGroup.Option.IsCombed Is Nothing, Nothing, New Boolean() {optionGroup.Option.IsCombed((i - optionGroup.Start) Mod optionGroup.Option.IsCombed.Length)})
            _frameOptions(i) = [option]
        Next
        SeekTo(CurrentFrameNumber)
    End Sub
    Public Sub ResetFrameOptions(ByVal startFrame As Integer, ByVal endFrame As Integer)
        For i = startFrame To endFrame
            _frameOptions.Remove(i)
        Next
        SeekTo(CurrentFrameNumber)
    End Sub
    Public Function GetCurrentFrameOption() As FrameOption?
        If Not _frameOptions.ContainsKey(CurrentFrameNumber) Then
            Return Nothing
        End If
        Return _frameOptions(CurrentFrameNumber)
    End Function
    Public Overrides ReadOnly Property CurrentFrameNumber() As Integer
        Get
            Dim caller As New StackFrame(1, False)
            If caller.GetMethod().DeclaringType Is GetType(AviSynthVideoProvider) Then
                Return MyBase.CurrentFrameNumber
            End If
            Return MyBase.CurrentFrameNumber Mod FrameCount
        End Get
    End Property
    Public Overrides ReadOnly Property FrameCount() As Integer
        Get
            Return MyBase.FrameCount \ 18
        End Get
    End Property
    Public Overrides Sub Open(ByVal filePath As String)
        Throw New NotImplementedException
    End Sub
    Private Function GetVideoSourceStatement(filePath As String, pluginDir As String) As String
        Dim formats As New Dictionary(Of String, String()) From {
            {".avs", {"Import", ""}},
            {".d2v", {"DGDecode_MPEG2Source", "DGDecode.dll"}},
            {".dga", {"AVCSource", "DGAVCDecode.dll"}},
            {".dgi", {"DGSource", "DGDecodeNV.dll"}}
        }
        Dim result As String = ""
        Dim sourceExtension As String = Path.GetExtension(filePath).ToLowerInvariant()
        Dim sourceImporter As String() = Nothing
        If Not formats.TryGetValue(sourceExtension, sourceImporter) Then
            Throw New InvalidOperationException("Input is not supported.")
        End If
        Dim importStatement = sourceImporter(0)
        Dim sourcePlugin = sourceImporter(1)
        If sourcePlugin <> "" Then
            Dim pluginPath As String = Path.Combine(pluginDir, sourcePlugin)
            If File.Exists(pluginPath) Then
                result += String.Format("LoadPlugin(""{0}""){1}", pluginPath, vbCrLf)
            End If
        End If
        result += String.Format("{0}(""{1}"")", importStatement, filePath)
        Return result
    End Function
    Public Overloads Sub Open(ByVal filePath As String, ByVal tfmParameters As String)
        Dim fileName = Path.GetTempFileName()
        Try
            'generate dummy overrides file
            File.WriteAllText(String.Format("{0}.combed", fileName), "0,0 +", Encoding.ASCII)
            File.WriteAllText(String.Format("{0}.notcombed", fileName), "0,0 -", Encoding.ASCII)
            For Each code In matchCodes
                File.WriteAllText(String.Format("{0}.match_{1}", fileName, code), String.Format("0,0 {0}", code), Encoding.ASCII)
                File.WriteAllText(String.Format("{0}.match_{1}_combed", fileName, code), String.Format("0,0 {0}{1}0,0 +", code, ControlChars.CrLf), Encoding.ASCII)
                File.WriteAllText(String.Format("{0}.match_{1}_notcombed", fileName, code), String.Format("0,0 {0}{1}0,0 -", code, ControlChars.CrLf), Encoding.ASCII)
            Next
            'generate avs
            filePath = Path.GetFullPath(filePath)
            Using swr As New StreamWriter(fileName, False, Encoding.Default)
                Dim pluginDir As String = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
                swr.WriteLine("SetWorkingDir(""{0}"")", Path.GetDirectoryName(filePath))
                If File.Exists(Path.Combine(pluginDir, "TIVTC.dll")) Then
                    swr.WriteLine("LoadPlugin(""{0}"")", Path.Combine(pluginDir, "TIVTC.dll"))
                End If
                swr.WriteLine(GetVideoSourceStatement(filePath, pluginDir))
                swr.WriteLine("source.tfm( {1})", filePath, tfmParameters)
                swr.WriteLine("last+source.tfm( ovr=""{1}.combed"", {2})", filePath, fileName, tfmParameters)
                swr.WriteLine("last+source.tfm( ovr=""{1}.notcombed"", {2})", filePath, fileName, tfmParameters)
                For Each code In matchCodes
                    swr.WriteLine("last+source.tfm( ovr=""{1}.match_{3}"", {2})", filePath, fileName, tfmParameters, code)
                    swr.WriteLine("last+source.tfm( ovr=""{1}.match_{3}_combed"", {2})", filePath, fileName, tfmParameters, code)
                    swr.WriteLine("last+source.tfm( ovr=""{1}.match_{3}_notcombed"", {2})", filePath, fileName, tfmParameters, code)
                Next
            End Using
            MyBase.Open(fileName)
        Finally
            Try
                'clean up
                File.Delete(fileName)
                File.Delete(String.Format("{0}.combed", fileName))
                File.Delete(String.Format("{0}.notcombed", fileName))
                For Each code In matchCodes
                    File.Delete(String.Format("{0}.match_{1}", fileName, code))
                    File.Delete(String.Format("{0}.match_{1}_combed", fileName, code))
                    File.Delete(String.Format("{0}.match_{1}_notcombed", fileName, code))
                Next
            Catch
            End Try
        End Try
    End Sub
    Public Overrides Function SeekTo(ByVal frameNumber As Integer) As Boolean
        If frameNumber < 0 OrElse frameNumber > FrameCount - 1 Then
            Return False
        End If
        If _frameOptions.ContainsKey(frameNumber) Then
            Dim [option] = _frameOptions(frameNumber)
            If Not String.IsNullOrEmpty([option].MatchCode) Then
                frameNumber += FrameCount * (Array.IndexOf(matchCodes, [option].MatchCode) + 1) * 3
            End If
            If [option].IsCombed IsNot Nothing Then
                frameNumber += FrameCount * If([option].IsCombed(0), 1, 2)
            End If
        End If
        ForceSeek(frameNumber)
        RaiseEvent Seek(Me, EventArgs.Empty)
        Return True
    End Function
    Private Function ArrayEquals(Of T)(ByVal a As T(), ByVal b As T()) As Boolean
        If a Is Nothing OrElse b Is Nothing Then
            Return a Is Nothing AndAlso b Is Nothing
        End If
        If a.Length <> b.Length Then
            Return False
        End If
        For i = 0 To a.Length - 1
            If Not EqualityComparer(Of T).Default.Equals(a(i), b(i)) Then
                Return False
            End If
        Next
        Return True
    End Function
End Class
