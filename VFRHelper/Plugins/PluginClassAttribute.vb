Namespace Plugins
    <AttributeUsage(AttributeTargets.Class)> _
    Public Class PluginClassAttribute
        Inherits Attribute
        Public FriendlyPluginName As String = Nothing
    End Class

End Namespace