Imports System.Reflection
Imports SAPStudio.VFRHelper.Plugins
Imports System.IO
Imports System.ComponentModel

Partial Class MainForm

    Private Class PluginHost
        Implements Plugins.IPluginHost

        Private Class ActionDispatcher
            Dim _actionName As String, _plugin As IPlugin
            Public Function Process(ByVal e As System.Windows.Forms.KeyEventArgs) As Boolean
                e.SuppressKeyPress = True
                Return _plugin.DoHotKeyAction(_actionName)
            End Function

            Sub New(ByVal plugin As IPlugin, ByVal actionName As String)
                _actionName = actionName
                _plugin = plugin
            End Sub
        End Class

        Public Event VideoProviderChanged(ByVal sender As Object, ByVal e As System.EventArgs) Implements Plugins.IPluginHost.VideoProviderChanged
        Public Event Seek(ByVal sender As Object, ByVal e As System.EventArgs) Implements Plugins.IPluginHost.Seek
        Public Event BeforePluginUnload(ByVal sender As Object, ByVal e As CancelEventArgs) Implements Plugins.IPluginHost.BeforePluginUnload

        Private WithEvents _mainForm As MainForm
        Dim _dockedWindow As Form
        Dim _pluginsLoaded As Boolean = False
        Dim _runningPlugin As IPlugin
        Sub AdjustDockedWindowSizeAndLocation()
            If _dockedWindow IsNot Nothing Then
                With _dockedWindow
                    .Left = _mainForm.Left + _mainForm.Width
                    .Top = _mainForm.Top
                    .Height = _mainForm.Height
                End With
            End If
        End Sub
        Private Function ShowDialog(ByVal dialog As FileDialog, ByVal filter As String) As String
            With dialog
                .FileName = ""
                .Filter = filter
                If .ShowDialog(_mainForm) = Windows.Forms.DialogResult.OK Then
                    Return .FileName
                Else
                    Return Nothing
                End If
            End With
        End Function
        Public Function BrowseOpen(ByVal filter As String) As String Implements Plugins.IPluginHost.BrowseOpen
            Return ShowDialog(_mainForm.OpenFileDialog1, filter)
        End Function

        Public Function BrowseSave(ByVal filter As String) As String Implements Plugins.IPluginHost.BrowseSave
            Return ShowDialog(_mainForm.SaveFileDialog1, filter)
        End Function

        Public Sub DockWindow(ByVal window As System.Windows.Forms.Form) Implements Plugins.IPluginHost.DockWindow
            If _dockedWindow IsNot Nothing Then
                Throw New InvalidOperationException("A window has been already docked to main window.")
            End If
            _dockedWindow = window
            AdjustDockedWindowSizeAndLocation()
            window.Owner = _mainForm
            _mainForm.BringToFront()
        End Sub


        Public Sub UndockWindow(ByVal window As System.Windows.Forms.Form) Implements Plugins.IPluginHost.UndockWindow
            If _dockedWindow Is window Then
                ActivateMainWindow()
                _dockedWindow = Nothing
                window.Owner = Nothing
            End If
        End Sub

        Public Property VideoProvider() As VideoProviders.IVideoProvider Implements Plugins.IPluginHost.VideoProvider
            Get
                Return _mainForm._videoProvider
            End Get
            Set(ByVal value As VideoProviders.IVideoProvider)
                _mainForm.SetVideoProvider(value)
                _mainForm.UpdateView()
            End Set
        End Property


        Public Sub UpdateView() Implements Plugins.IPluginHost.UpdateView
            _mainForm.UpdateView()
        End Sub

        Public Sub OnVideoProviderChanged()
            RaiseEvent VideoProviderChanged(_mainForm, EventArgs.Empty)
        End Sub

        Private Sub OnSeek(ByVal sender As System.Object, ByVal e As System.EventArgs)
            RaiseEvent Seek(_mainForm, EventArgs.Empty)
        End Sub

        Public Sub SetVisiblePluginsPanel(ByVal visible As Boolean)
            If Not _pluginsLoaded Then
                LoadPlugins()
            End If
            If _mainForm.gbPlugins.Visible = visible Then
                Return
            End If
            _mainForm.SuspendLayout()
            'Dim needRestoreAnchor As New List(Of Control)
            Dim adjustment = _mainForm.gbPlugins.Height + 20
            For Each ctl As Control In _mainForm.Controls
                If (ctl.Anchor And AnchorStyles.Bottom) <> 0 AndAlso (ctl.Anchor And AnchorStyles.Top) = 0 Then
                    ctl.Top += If(visible, -adjustment, adjustment)
                Else
                    ctl.Height += If(visible, -adjustment, adjustment)
                End If
            Next
            _mainForm.gbPlugins.Visible = visible
            'If visible Then
            '    _mainForm.Height += _mainForm.gbPlugins.Height + 20
            '    _mainForm.gbPlugins.Visible = True
            'Else
            '    _mainForm.Height -= _mainForm.gbPlugins.Height + 20
            '    _mainForm.gbPlugins.Visible = False
            'End If
            'For Each ctl In needRestoreAnchor
            '    ctl.Anchor = ctl.Anchor Or AnchorStyles.Bottom
            'Next
            'If visible Then
            '    _mainForm.Height -= _mainForm.gbPlugins.Height + 20
            'Else
            '    _mainForm.Height += _mainForm.gbPlugins.Height + 20
            'End If
            _mainForm.ResumeLayout()
            _mainForm.Refresh()
        End Sub

        Private Sub AddTab(ByVal key As String, ByVal name As String)
            _mainForm.tabPlugins.TabPages.Insert(0, key, name)
            _mainForm.tabPlugins.TabPages.Item(key).Controls.Add(New FlowLayoutPanel With { _
                                          .Dock = DockStyle.Fill, _
                                          .WrapContents = True, _
                                          .FlowDirection = FlowDirection.LeftToRight, _
                                          .AutoScroll = True})
        End Sub
        Private Sub LoadPlugins()
            Const TABKEY_OTHER As String = "##Other"
            Dim assemblies As New List(Of Assembly)
            Dim pluginsDir = Path.Combine(My.Application.Info.DirectoryPath, "plugins")
            If Directory.Exists(pluginsDir) Then
                For Each file In Directory.GetFiles(pluginsDir, "*.dll", SearchOption.AllDirectories)
                    Try
                        assemblies.Add(Assembly.LoadFrom(file))
                    Catch ex As Exception
                    End Try
                Next
            End If
            assemblies.Add(Assembly.GetEntryAssembly())
            With _mainForm.tabPlugins
                .SuspendLayout()
                .TabPages.Clear()

                AddTab(TABKEY_OTHER, "Other")

                For Each asm In assemblies
                    Dim pluginAttributes As VFRHelperPluginAttribute() = CType(asm.GetCustomAttributes(GetType(VFRHelperPluginAttribute), True), VFRHelperPluginAttribute())
                    For Each attr In pluginAttributes
                        Dim key As String
                        If attr.FunctionID = -1 Then
                            key = TABKEY_OTHER
                        Else
                            key = attr.PluginType.FullName
                        End If
                        With .TabPages
                            Dim friendlyName As String
                            If attr.PluginType.IsDefined(GetType(PluginClassAttribute), False) Then
                                friendlyName = CType(attr.PluginType.GetCustomAttributes(GetType(PluginClassAttribute), False)(0), PluginClassAttribute).FriendlyPluginName
                            Else
                                friendlyName = attr.PluginType.Name
                            End If
                            If Not .ContainsKey(key) Then
                                AddTab(key, friendlyName)
                            End If
                            Dim container = .Item(key).Controls(0)
                            Dim button As New Button With { _
                                                        .AutoSize = True, _
                                                        .Text = If(attr.FunctionName, friendlyName), _
                                                        .Tag = attr}
                            AddHandler button.Click, AddressOf OnPluginButtonClicked
                            container.Controls.Add(button)
                        End With
                    Next
                Next
                .SelectedIndex = 0
                .ResumeLayout()
            End With
            _pluginsLoaded = True
        End Sub

        Private Sub OnMainFormMoveResize(ByVal sender As Object, ByVal e As System.EventArgs) Handles _mainForm.Move, _mainForm.Resize
            AdjustDockedWindowSizeAndLocation()
        End Sub

        Private Sub OnPluginButtonClicked(ByVal sender As Object, ByVal e As EventArgs)
            SetVisiblePluginsPanel(False)
            Try
                Dim attr = CType(CType(sender, Control).Tag, VFRHelperPluginAttribute)
                _runningPlugin = CType(Activator.CreateInstance(attr.PluginType), IPlugin)
                AddHandler _runningPlugin.BeforeDispose, AddressOf OnPluginDispose
                _mainForm._shortcutManager.PushSettings()
                Dim keyMappingFile As String = Path.ChangeExtension(attr.PluginType.Assembly.Location, ".keymappings.xml")
                If File.Exists(keyMappingFile) Then
                    Using fs = File.OpenRead(keyMappingFile)
                        Try
                            _mainForm._shortcutManager.LoadKeyMappingFile(fs, False)
                        Catch ex As Exception
                        End Try
                    End Using
                End If
                For Each actionName In _runningPlugin.HotKeyActionNames
                    _mainForm._shortcutManager.RegisterAction(actionName, AddressOf New ActionDispatcher(_runningPlugin, actionName).Process)
                Next
                _mainForm.gbPlugins.Enabled = False
                _mainForm.btnTogglePlugins.Text = "Unload Plugin"
                _runningPlugin.Initialize(Me, attr.FunctionID)
            Catch ex As Exception
                If _runningPlugin IsNot Nothing Then
                    Try
                        _runningPlugin.Dispose()
                    Catch
                    End Try
                End If
                ResetState()
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub ResetState()
            _dockedWindow = Nothing
            _runningPlugin = Nothing
            _mainForm.btnTogglePlugins.Text = "Load Plugin"
            _mainForm.gbPlugins.Enabled = True
        End Sub
        Private Sub OnPluginDispose(ByVal sender As Object, ByVal e As EventArgs)
            _mainForm._shortcutManager.PopSettings()
            ResetState()
        End Sub

        Public ReadOnly Property PluginLoaded() As Boolean
            Get
                Return _runningPlugin IsNot Nothing
            End Get
        End Property

        Public Sub UnloadPlugin()
            If _runningPlugin IsNot Nothing Then
                Dim cancelEventArgs As New CancelEventArgs
                RaiseEvent BeforePluginUnload(Me, cancelEventArgs)
                If cancelEventArgs.Cancel Then
                    Return
                End If
                _runningPlugin.Dispose()
            End If
        End Sub
        Public Sub New()
            _mainForm = My.Forms.MainForm
            AddHandler _mainForm.TrackBar1.ValueChanged, AddressOf OnSeek
        End Sub

        Public Sub ActivateMainWindow() Implements Plugins.IPluginHost.ActivateMainWindow
            _mainForm.BringToFront()
        End Sub

        Public Sub NotifyMessage(ByVal str As String) Implements Plugins.IPluginHost.NotifyMessage
            _mainForm.Notify(str)
        End Sub
    End Class

End Class