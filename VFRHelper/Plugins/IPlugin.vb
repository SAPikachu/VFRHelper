Namespace Plugins

    Public Interface IPlugin
        Inherits IDisposable

        Sub Initialize(ByVal host As IPluginHost, ByVal functionID As Integer)
        Function DoHotKeyAction(ByVal actionName As String) As Boolean
        ReadOnly Property HotKeyActionNames() As IEnumerable(Of String)
        Event BeforeDispose As EventHandler
    End Interface

End Namespace