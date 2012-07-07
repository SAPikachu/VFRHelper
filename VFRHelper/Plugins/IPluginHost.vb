Namespace Plugins

    Public Interface IPluginHost
        Sub DockWindow(ByVal window As Form)
        Sub UndockWindow(ByVal window As Form)
        Sub UpdateView()
        Sub ActivateMainWindow()
        Sub NotifyMessage(ByVal str As String)
        Function BrowseOpen(ByVal filter As String) As String
        Function BrowseSave(ByVal filter As String) As String
        Property VideoProvider() As VideoProviders.IVideoProvider
        Property FrameSizeMode As FrameSizeMode
        Event Seek As EventHandler
        Event VideoProviderChanged As EventHandler
        Event BeforePluginUnload As System.ComponentModel.CancelEventHandler
    End Interface

End Namespace
