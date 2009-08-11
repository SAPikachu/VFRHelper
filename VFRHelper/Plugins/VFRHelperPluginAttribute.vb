Namespace Plugins
    <AttributeUsage(AttributeTargets.Assembly, AllowMultiple:=True)> _
    Public Class VFRHelperPluginAttribute
        Inherits Attribute
        Private _pluginType As Type
        Private _functionName As String = Nothing
        Private _functionID As Integer = -1

        Public Property FunctionID() As Integer
            Get
                Return _functionID
            End Get
            Set(ByVal value As Integer)
                _functionID = value
            End Set
        End Property

        Public Property FunctionName() As String
            Get
                Return _functionName
            End Get
            Set(ByVal value As String)
                _functionName = value
            End Set
        End Property

        Public ReadOnly Property PluginType() As Type
            Get
                Return _pluginType
            End Get
        End Property
        Sub New(ByVal pluginType As Type)
            _pluginType = pluginType
        End Sub


    End Class

End Namespace