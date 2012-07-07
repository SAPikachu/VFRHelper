Imports System.Collections.Generic
Imports System.IO
Imports System.Drawing
Imports System.Text
Imports SAPStudio.Utils
Imports SAPStudio.VFRHelper.VideoProviders
Imports System.Runtime.InteropServices

Public Class MainForm
    Implements IVideoProviderHelper

    Private Const SIGNATURE_TIMECODEV2 As String = "# timecode format v2"
    Private Const TIMECODE_MASK As String = "00:00:00.000"
    Dim _videoProvider As VideoProviders.IVideoProvider
    Dim _viewUpdating As Boolean
    Dim _aviFN, _tcFN As String
    Dim _shortcutManager As New ShortcutKeyManager(Of Boolean)
    Dim _tempFiles As New List(Of String)

    Dim _pluginHost As PluginHost

    Sub SetTitle()
        Dim builder As New StringBuilder
        builder.Append(My.Application.Info.ProductName)
        If Not String.IsNullOrEmpty(_aviFN) Then
            builder.AppendFormat(" - {0}", _aviFN)
        End If
        If Not String.IsNullOrEmpty(_tcFN) Then
            builder.AppendFormat(" - {0}", _tcFN)
        End If
        'If Not String.IsNullOrEmpty(_chapFN) Then
        '    builder.AppendFormat(" - {0}", _chapFN)
        'End If
        Me.Text = builder.ToString()
    End Sub
    Sub EnableControls()
        txtTimecode.Enabled = True
        NumericUpDown1.Maximum = _videoProvider.FrameCount - 1
        NumericUpDown1.Enabled = True
        TrackBar1.Maximum = _videoProvider.FrameCount - 1
        TrackBar1.Enabled = True
        TrackBar1.Focus()
    End Sub
    Sub DisableControls()
        txtTimecode.Enabled = False
        NumericUpDown1.Enabled = False
        TrackBar1.Enabled = False
    End Sub
    Sub OpenTC(ByVal fileName As String)
        Dim tcs = Timecodes.OpenV2(fileName)
        If tcs Is Nothing Then
            MessageBox.Show("Not a valid timecode v2 file", "Error")
            Return
        End If
        If _videoProvider Is Nothing Then
            SetVideoProvider(New NullVideoProvider())
        End If
        If Not _videoProvider.IsSuitableTimecodes(tcs) Then
            If MessageBox.Show("Timecode file doesn't match the video, do you want to close the video?", "Info", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
                Return
            End If
            _videoProvider.Dispose()
            SetVideoProvider(New NullVideoProvider())
        End If
        _videoProvider.SetTimecodes(tcs)
        _tcFN = Path.GetFileName(fileName)
        UpdateView()
        SetTitle()
    End Sub
    Private Sub RegisterTempFile(ByVal fileName As String) Implements IVideoProviderHelper.RegisterTempFile
        If Not _tempFiles.Contains(fileName) Then
            _tempFiles.Add(fileName)
        End If
    End Sub
    Sub Benchmark()
        Me.Text = "Please wait..."
        Dim orgFrameNum = _videoProvider.CurrentFrameNumber
        Dim bm As Bitmap = Nothing
        For i As Integer = 0 To 999
            '_video.GetBitmap(i).Dispose()
            _videoProvider.SeekTo(i)
            _videoProvider.GetFrame(bm)
        Next
        bm = Nothing
        For i As Integer = 0 To 999
            '_video.GetBitmap(i).Dispose()
            _videoProvider.SeekTo(i)
            _videoProvider.GetFrame(bm)
        Next
        bm = Nothing
        Dim watch As New Diagnostics.Stopwatch
        watch.Start()
        For i As Integer = 0 To 999
            '_video.GetBitmap(i).Dispose()
            _videoProvider.SeekTo(i)
            _videoProvider.GetFrame(bm)
        Next
        watch.Stop()
        _videoProvider.SeekTo(orgFrameNum)
        MessageBox.Show(String.Format("{0} fps", 1000 / (watch.ElapsedMilliseconds / 1000)))
        SetTitle()
    End Sub
    Sub UpdateView()
        _viewUpdating = True
        Try
            'Dim orgImage As Image = picFrame.Image
            txtTimecode.Mask = String.Empty
            Dim bm As Bitmap = Nothing
            If picFrame.Image IsNot Nothing Then
                bm = CType(picFrame.Image, Bitmap)
            End If
            If _videoProvider Is Nothing Then ' _avi Is Nothing Then
                picFrame.Image = Nothing
                txtFrameType.Text = ""
            Else
                Try
                    _videoProvider.GetFrame(bm)
                Catch ex As Exception
                    MessageBox.Show(Me, ex.Message, "Unable to decode frame", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End Try

                picFrame.Image = bm
                picFrame.Update()
                Dim currentFrameType As FrameType = _videoProvider.CurrentFrameType
                If (currentFrameType And FrameType.CustomAsciiChar) = FrameType.CustomAsciiChar Then
                    txtFrameType.Text = String.Format("[{0}]", Encoding.ASCII.GetString(New Byte() {CByte(currentFrameType And &HFF)}))
                Else
                    Select Case currentFrameType
                        Case FrameType.Key
                            txtFrameType.Text = "[K]"
                        Case FrameType.Normal
                            txtFrameType.Text = "[ ]"
                        Case FrameType.Null
                            txtFrameType.Text = "[D]"
                    End Select
                End If
                txtFrameType.Update()
                'If orgImage IsNot Nothing Then orgImage.Dispose()
                TrackBar1.Value = _videoProvider.CurrentFrameNumber
                NumericUpDown1.Value = _videoProvider.CurrentFrameNumber
                NumericUpDown1.Update()
                Dim selStart, selLen As Integer
                selStart = txtTimecode.SelectionStart
                selLen = txtTimecode.SelectionLength
                txtTimecode.Text = _videoProvider.GetTimecodeOfCurrentFrame()
                txtTimecode.Update()
                txtTimecode.Select(selStart, selLen)
            End If
        Finally
            _viewUpdating = False
        End Try
    End Sub

    Sub ShowOpen(ByVal filter As String, ByVal callback As Action(Of String))
        OpenFileDialog1.FileName = ""
        OpenFileDialog1.Filter = filter
        If OpenFileDialog1.ShowDialog(Me) = Windows.Forms.DialogResult.Cancel Then
            Return
        End If
        callback(OpenFileDialog1.FileName)
    End Sub
    Private Sub btnOpenTC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenTC.Click
        ShowOpen("Timecode V2 File|*.txt", AddressOf OpenTC)
    End Sub

    Private Sub NumericUpDown1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown1.ValueChanged
        If _viewUpdating Then Return
        _videoProvider.SeekTo(Decimal.ToInt32(NumericUpDown1.Value))
        UpdateView()
    End Sub

    Private Sub Buttons_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles btnOpenTC.DragDrop, btnOpenVideo.DragDrop
        If e.Effect = DragDropEffects.Copy AndAlso e.Data.GetDataPresent(DataFormats.FileDrop) Then
            Dim files As String() = CType(e.Data.GetData(DataFormats.FileDrop), String())
            If sender Is btnOpenTC Then
                OpenTC(files(0))
            ElseIf sender Is btnOpenVideo Then
                OpenVideo(files(0))
            Else
                Throw New ApplicationException("Buttons_DragDrop: Unknown sender")
            End If
        End If
    End Sub

    Private Sub Buttons_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles btnOpenTC.DragEnter, btnOpenVideo.DragEnter
        If (e.AllowedEffect And DragDropEffects.Copy) = DragDropEffects.Copy AndAlso e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private Sub SetVideoProvider(ByVal newProvider As VideoProviders.IVideoProvider)
        If _videoProvider Is newProvider Then
            Return
        End If
        _aviFN = ""
        _videoProvider = newProvider
        If newProvider Is Nothing Then
            DisableControls()
        Else
            EnableControls()
            Dim videoSize = newProvider.VideoSize
            If Not videoSize.IsEmpty Then
                Dim workingArea As Rectangle = Screen.FromControl(Me).WorkingArea
                Me.Size = New Size(Math.Min(workingArea.Width, Me.Width - picFrame.Width + videoSize.Width), Math.Min(workingArea.Height, Me.Height - picFrame.Height + videoSize.Height))
                If Me.Right > workingArea.Right OrElse Me.Bottom > workingArea.Bottom Then
                    Me.CenterToScreen()
                End If
            End If
        End If
        If _pluginHost IsNot Nothing Then
            _pluginHost.OnVideoProviderChanged()
        End If
    End Sub
    Private Sub Cleanup()
        If _videoProvider IsNot Nothing Then
            _videoProvider.Dispose()
            SetVideoProvider(Nothing)
        End If
        If picFrame.Image IsNot Nothing Then
            picFrame.Image.Dispose()
            picFrame.Image = Nothing
        End If
    End Sub
    Sub OpenVideo(ByVal fileName As String)
        Dim newProvider As IVideoProvider
        Select Case Path.GetExtension(fileName).ToLowerInvariant()
            Case ".avi"
                newProvider = New AviFileVideoProvider
            Case ".avs"
                newProvider = New AviSynthVideoProvider
            Case ".mkv", ".mp4", ".flv"
                newProvider = New FFMS2VideoProvider
            Case Else
                Throw New NotSupportedException("Unknown file format")
        End Select
        Me.Enabled = False
        newProvider.SetHelper(Me)
        Try
            Try
                newProvider.Open(fileName)
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Unable to open the file", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End Try
            If _videoProvider IsNot Nothing Then
                If newProvider.IsSuitableTimecodes(_videoProvider.Timecodes) Then
                    newProvider.SetTimecodes(_videoProvider.Timecodes)
                Else
                    _tcFN = ""
                End If
                Cleanup()
            End If
            SetVideoProvider(newProvider)
            _aviFN = Path.GetFileName(fileName)
        Finally
            Me.Enabled = True
        End Try
        SetTitle()
        UpdateView()
    End Sub

    Private Sub btnOpenVideo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenVideo.Click
        ShowOpen("Supported Video File|*.avi;*.avs;*.mkv;*.mp4;*.flv", AddressOf OpenVideo)
    End Sub

    Private Sub Form1_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Cleanup()
        If _tempFiles.Count > 0 Then
            If MessageBox.Show("Some cache files are created to speed up further processing, do you want to keep them in your hard disk?", My.Application.Info.Title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
                For Each fi In _tempFiles
                    Try
                        File.Delete(fi)
                    Catch ex As Exception

                    End Try
                Next
            End If
        End If
    End Sub


    Dim _prevTrackBarValue As Integer

    Private Sub TrackBar1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TrackBar1.MouseDown
        Dim channelRect As New RECT
        Dim thumbRect As New RECT
        NativeMethods.SendMessage(New HandleRef(Me, TrackBar1.Handle), NativeConstants.TBM_GETCHANNELRECT, IntPtr.Zero, channelRect)
        NativeMethods.SendMessage(New HandleRef(Me, TrackBar1.Handle), NativeConstants.TBM_GETTHUMBRECT, IntPtr.Zero, thumbRect)
        Dim thumbWidth = thumbRect.right - thumbRect.left
        Dim value = CInt((e.X + TrackBar1.ClientRectangle.X - channelRect.left - thumbWidth / 2) / (channelRect.right - channelRect.left - thumbWidth) * (TrackBar1.Maximum - TrackBar1.Minimum + 1) + TrackBar1.Minimum)
        value = Math.Max(TrackBar1.Minimum, Math.Min(TrackBar1.Maximum, value))
        TrackBar1.Value = value
        _videoProvider.SeekTo(value)
        TrackBar1_Scroll(sender, EventArgs.Empty)
    End Sub
    Private Sub TrackBar1_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar1.Scroll
        If _viewUpdating Then
            _prevTrackBarValue = TrackBar1.Value
            Return
        End If
        If Timer1.Enabled = True Then
            TrackBar1.Value = _prevTrackBarValue
            Return
        End If
        _prevTrackBarValue = TrackBar1.Value
        _videoProvider.SeekTo(TrackBar1.Value)
        Timer1.Enabled = True
        'NOTE: Timer is used to discard mouse clicks during update
        'UpdateView()
    End Sub

    Const KEYMAPPINGFILENAME As String = "keymappings.xml"
    ReadOnly _keyMappingFilePath As String = Path.Combine(My.Application.Info.DirectoryPath, KEYMAPPINGFILENAME)
    Private Sub WriteDefaultKeyMappingFile()
        Using newFile As New FileStream(_keyMappingFilePath, FileMode.Create), res = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(String.Format("{0}.{1}", Me.GetType().Namespace, KEYMAPPINGFILENAME))
            Dim buffer(16383) As Byte, bytesRead As Integer
            bytesRead = res.Read(buffer, 0, 16384)
            Do Until bytesRead = 0
                newFile.Write(buffer, 0, bytesRead)
                bytesRead = res.Read(buffer, 0, 16384)
            Loop
        End Using
    End Sub
    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _shortcutManager.RegisterAction("NextKeyFrame", AddressOf Action_NextKeyFrame)
        _shortcutManager.RegisterAction("PrevKeyFrame", AddressOf Action_PrevKeyFrame)
        _shortcutManager.RegisterAction("NextFrame", AddressOf Action_NextFrame)
        _shortcutManager.RegisterAction("PrevFrame", AddressOf Action_PrevFrame)
        _shortcutManager.RegisterAction("Benchmark", AddressOf Action_Benchmark)
        '_shortcutManager.RegisterAction("SetTimecode", AddressOf Action_SetTimecode)
        '_shortcutManager.RegisterAction("ChapterListMoveUp", AddressOf Action_ChapterListMoveUp)
        '_shortcutManager.RegisterAction("ChapterListMoveDown", AddressOf Action_ChapterListMoveDown)
        _shortcutManager.RegisterAction("DiscordKey", AddressOf Action_DiscordKey)
        _shortcutManager.RegisterAction("JumpToFrame", AddressOf Action_JumpToFrame)
        _shortcutManager.SetDefaultAction(Function() False)
        If Not File.Exists(_keyMappingFilePath) Then
            WriteDefaultKeyMappingFile()
        End If

        Try
            _shortcutManager.LoadKeyMappingFile(_keyMappingFilePath)
        Catch ex As Exception
            MessageBox.Show("The key mapping file is invalid, resetting to default.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            WriteDefaultKeyMappingFile()
            _shortcutManager.LoadKeyMappingFile(_keyMappingFilePath)
        End Try
    End Sub
#Region "KeyDown Processor"
    Private Function Action_JumpToFrame(ByVal e As KeyEventArgs) As Boolean
        NumericUpDown1.Focus()
        NumericUpDown1.Select(0, NumericUpDown1.Value.ToString.Length)
        e.SuppressKeyPress = True
        Return False
    End Function
    Private Function Action_NextFrame(ByVal e As KeyEventArgs) As Boolean
        If _videoProvider Is Nothing Then ' _video Is Nothing Then
            Return False
        End If
        e.SuppressKeyPress = True

        Return _videoProvider.SeekToNextFrame()
    End Function
    Private Function Action_NextKeyFrame(ByVal e As KeyEventArgs) As Boolean
        If _videoProvider Is Nothing Then ' _video Is Nothing Then
            Return False
        End If
        e.SuppressKeyPress = True

        Return _videoProvider.SeekToNextKeyFrame()
    End Function
    Private Function Action_PrevFrame(ByVal e As KeyEventArgs) As Boolean
        If _videoProvider Is Nothing Then ' _video Is Nothing Then
            Return False
        End If
        e.SuppressKeyPress = True

        Return _videoProvider.SeekToPrevFrame()
    End Function
    Private Function Action_PrevKeyFrame(ByVal e As KeyEventArgs) As Boolean
        If _videoProvider Is Nothing Then ' _video Is Nothing Then
            Return False
        End If
        e.SuppressKeyPress = True

        Return _videoProvider.SeekToPrevKeyFrame()
    End Function

    Private Function Action_Benchmark(ByVal e As System.Windows.Forms.KeyEventArgs) As Boolean
        e.SuppressKeyPress = True
        Benchmark()
        Return False
    End Function
    Private Function Action_DiscordKey(ByVal e As System.Windows.Forms.KeyEventArgs) As Boolean
        e.SuppressKeyPress = True
        Return False
    End Function

#End Region
    Private Sub Form1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If _shortcutManager.ProcessKeyEvent(e) Then
            UpdateView()
        End If
    End Sub

    Private Sub Buttons_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOpenVideo.GotFocus, btnTogglePlugins.GotFocus, btnOpenTC.GotFocus
        TrackBar1.Focus()
    End Sub


    Private Sub Form1_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        picFrame.AllowDrop = True
        If My.Application.CommandLineArgs.Count > 0 Then
            OpenFiles(My.Application.CommandLineArgs)
        End If
    End Sub

    Private Shared Function CheckDroppedFileExt(ByVal files As String()) As Boolean
        Dim extensions As New List(Of String)(New String() {".avi", ".avs", ".xml", ".txt", ".mkv", ".mp4", ".flv"})
        For Each file As String In files
            If file.Length < 4 OrElse (Not extensions.Contains(Path.GetExtension(file).ToLowerInvariant())) Then
                Return False
            End If
        Next
        Return True
    End Function

    Private Sub picFrame_DoubleClick(sender As Object, e As System.EventArgs) Handles picFrame.DoubleClick
        FrameSizeMode = If(FrameSizeMode = FrameSizeMode.Stretched, FrameSizeMode.Original, FrameSizeMode.Stretched)
    End Sub
    Private Sub picFrame_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles picFrame.DragEnter
        If (e.AllowedEffect And DragDropEffects.Copy) = DragDropEffects.Copy AndAlso e.Data.GetDataPresent(DataFormats.FileDrop) Then
            Dim files As String() = CType(e.Data.GetData(DataFormats.FileDrop), String())
            If CheckDroppedFileExt(files) Then
                e.Effect = DragDropEffects.Copy
                Return
            End If
        End If
        e.Effect = DragDropEffects.None
    End Sub

    Private Sub OpenFiles(ByVal files As IList(Of String))
        For Each file As String In files
            If Not System.IO.File.Exists(file) Then Continue For
            Select Case Path.GetExtension(file).ToLowerInvariant()
                Case ".avi", ".avs", ".mkv", ".mp4", ".flv"
                    OpenVideo(file)
                    'Case ".xml"
                    '    OpenChap(file)
                Case ".txt"
                    Dim isTC As Boolean = False
                    Using sr As StreamReader = System.IO.File.OpenText(file)
                        If sr.ReadLine().ToLowerInvariant() = SIGNATURE_TIMECODEV2 Then
                            isTC = True
                        End If
                    End Using
                    If isTC Then
                        OpenTC(file)
                        'Else
                        '    OpenChap(file)
                    End If
            End Select
        Next
    End Sub
    Private Sub picFrame_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles picFrame.DragDrop
        If e.Effect = DragDropEffects.Copy AndAlso e.Data.GetDataPresent(DataFormats.FileDrop) Then
            Dim files As String() = CType(e.Data.GetData(DataFormats.FileDrop), String())
            If Not CheckDroppedFileExt(files) Then
                MessageBox.Show("Couldn't determine type of one or more files", "Error")
                Return
            End If
            BeginInvoke(New Action(Of IList(Of String))(AddressOf OpenFiles), New Object() {files})
        End If
    End Sub


    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        UpdateView()
        Timer1.Enabled = False
    End Sub

    Public Sub Notify(ByVal message As String) Implements VideoProviders.IVideoProviderHelper.Notify
        If String.IsNullOrEmpty(message) Then
            SetTitle()
        Else
            Text = String.Format("{0} - {1}", My.Application.Info.ProductName, message)
        End If
        Application.DoEvents()
    End Sub

    Private Sub txtTimecode_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTimecode.KeyDown
        txtTimecode.Mask = TIMECODE_MASK
        If e.KeyCode = Keys.Return Then
            Dim index = _videoProvider.Timecodes.BinarySearch(txtTimecode.Text)
            If index < 0 Then
                index = Not index
                index -= 1
            End If
            _videoProvider.SeekTo(index)
            UpdateView()
        End If
    End Sub

    Private Sub txtTimecode_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txtTimecode.MouseDown
        txtTimecode.Mask = TIMECODE_MASK
    End Sub

    Private Sub btnTogglePlugins_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTogglePlugins.Click
        If _pluginHost Is Nothing Then
            _pluginHost = New PluginHost
        End If
        If _pluginHost.PluginLoaded Then
            _pluginHost.UnloadPlugin()
        Else
            _pluginHost.SetVisiblePluginsPanel(Not gbPlugins.Visible)
        End If
    End Sub

    Property FrameSizeMode As FrameSizeMode
        Get
            Return If(picFrame.SizeMode = PictureBoxSizeMode.Zoom, FrameSizeMode.Stretched, FrameSizeMode.Original)
        End Get
        Set(value As FrameSizeMode)
            If value = FrameSizeMode.Stretched Then
                picFrame.SizeMode = PictureBoxSizeMode.Zoom
                picFrame.Dock = DockStyle.Fill
            Else
                picFrame.Dock = DockStyle.None
                picFrame.SizeMode = PictureBoxSizeMode.AutoSize
                picFrame.Location = New Point(0, 0)
                UpdateFramePosition()
            End If
        End Set
    End Property

    Sub UpdateFramePosition()
        UpdateFramePosition(Point.Empty)
    End Sub
    Sub UpdateFramePosition(delta As Point)
        If FrameSizeMode = FrameSizeMode.Stretched Then
            Return
        End If
        panFrameContainer.SuspendLayout()
        If picFrame.Width < panFrameContainer.Width Then
            picFrame.Left = (panFrameContainer.Width - picFrame.Width) \ 2
        Else
            picFrame.Left = Math.Max(panFrameContainer.Width - picFrame.Width, Math.Min(0, picFrame.Left + delta.X))
        End If
        If picFrame.Height < panFrameContainer.Height Then
            picFrame.Top = (panFrameContainer.Height - picFrame.Height) \ 2
        Else
            picFrame.Top = Math.Max(panFrameContainer.Height - picFrame.Height, Math.Min(0, picFrame.Top + delta.Y))
        End If
        panFrameContainer.ResumeLayout()
    End Sub

    Private Sub MainForm_Resize(sender As Object, e As System.EventArgs) Handles Me.Resize
        UpdateFramePosition()
    End Sub

    Dim _lastMousePosition As Point?
    Private Sub picFrame_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles picFrame.MouseDown
        If FrameSizeMode = VFRHelper.FrameSizeMode.Original And e.Button = Windows.Forms.MouseButtons.Left Then
            _lastMousePosition = picFrame.PointToScreen(e.Location)
            picFrame.Capture = True
        End If
    End Sub

    Private Sub picFrame_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles picFrame.MouseUp
        _lastMousePosition = Nothing
        picFrame.Capture = False
    End Sub

    Private Sub picFrame_MouseMove(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles picFrame.MouseMove
        If _lastMousePosition.HasValue Then
            Dim newLocation = picFrame.PointToScreen(e.Location)
            UpdateFramePosition(newLocation - New Size(_lastMousePosition.Value))
            _lastMousePosition = newLocation
        End If
    End Sub
End Class
