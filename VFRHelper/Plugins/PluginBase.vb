Namespace Plugins

    Public Delegate Function HotKeyActionDelegate() As Boolean

    Public MustInherit Class PluginBase
        Implements IPlugin

        Public Event BeforeDispose(ByVal sender As Object, ByVal e As System.EventArgs) Implements IPlugin.BeforeDispose


        Protected MustOverride Sub Initialize()

        Private _host As IPluginHost

        ReadOnly _functions As New Dictionary(Of Integer, MethodInvoker)
        ReadOnly _hotKeyActions As New Dictionary(Of String, HotKeyActionDelegate)

        Dim _initialized As Boolean = False

        Public ReadOnly Property HotKeyActionNames() As System.Collections.Generic.IEnumerable(Of String) Implements IPlugin.HotKeyActionNames
            Get
                Return _hotKeyActions.Keys
            End Get
        End Property

        Public Sub Initialize(ByVal host As IPluginHost, ByVal functionID As Integer) Implements IPlugin.Initialize
            If _initialized Then
                Throw New InvalidOperationException("Already initialized.")
            End If
            If Not _functions.ContainsKey(functionID) Then
                Throw New NotSupportedException("Unknown function ID.")
            End If
            _host = host
            _functions(functionID)()
            _initialized = True
        End Sub

        Public Function DoHotKeyAction(ByVal actionName As String) As Boolean Implements IPlugin.DoHotKeyAction
            If _hotKeyActions.ContainsKey(actionName) Then
                Return _hotKeyActions(actionName)()
            Else
                Return False
            End If
        End Function

        Protected Sub RegisterHotKeyAction(ByVal actionName As String, ByVal method As HotKeyActionDelegate)
            _hotKeyActions(actionName) = method
        End Sub

        Protected Sub RegisterPluginFunction(ByVal functionID As Integer, ByVal method As MethodInvoker)
            _functions(functionID) = method
        End Sub

        Protected ReadOnly Property Host() As IPluginHost
            Get
                Return _host
            End Get
        End Property

        Public Sub New()
            Initialize()
        End Sub

        Private _disposedValue As Boolean = False        ' 检测冗余的调用

        Protected Sub CheckDisposed()
            If _disposedValue Then
                Throw New ObjectDisposedException([GetType]().Name)
            End If
        End Sub

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me._disposedValue Then
                RaiseEvent BeforeDispose(Me, EventArgs.Empty)
                If disposing Then
                    ' TODO: 释放其他状态(托管对象)。
                End If

                ' TODO: 释放您自己的状态(非托管对象)。
                ' TODO: 将大型字段设置为 null。
            End If
            Me._disposedValue = True
        End Sub

#Region " IDisposable Support "
        ' Visual Basic 添加此代码是为了正确实现可处置模式。
        Public Sub Dispose() Implements IDisposable.Dispose
            ' 不要更改此代码。请将清理代码放入上面的 Dispose(ByVal disposing As Boolean) 中。
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class

End Namespace