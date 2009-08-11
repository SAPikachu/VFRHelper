Imports System.Text
Namespace Plugins.ChapterEditor

    Public Class ChapterEntry
        Implements ICloneable

        Private _start As TimeSpan
        Private _end As TimeSpan
        Private _data As New Dictionary(Of String, String)()
        Private _name As String
        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property
        Public Property Start() As TimeSpan
            Get
                Return _start
            End Get
            Set(ByVal value As TimeSpan)
                _start = value
            End Set
        End Property
        Public Property [End]() As TimeSpan
            Get
                Return _end
            End Get
            Set(ByVal value As TimeSpan)
                _end = value
            End Set
        End Property
        Public ReadOnly Property Data() As Dictionary(Of String, String)
            Get
                Return _data
            End Get
        End Property
        Public Sub New()

        End Sub
        Public Sub New(ByVal name As String, ByVal start As TimeSpan)
            MyClass.New(name, start, TimeSpan.Zero)
        End Sub
        Public Sub New(ByVal name As String, ByVal start As TimeSpan, ByVal [end] As TimeSpan)
            Me._name = name
            Me._start = start
            Me._end = [end]
        End Sub
        Public Overrides Function ToString() As String
            Dim builder As New StringBuilder
            builder.Append(Utils.TimeSpanToOGMTimecode(_start))
            If _end <> TimeSpan.Zero Then
                builder.Append("->")
                builder.Append(Utils.TimeSpanToOGMTimecode(_end))
            End If
            builder.Append(" ")
            builder.Append(_name)
            Return builder.ToString()
        End Function

        Public Function Clone() As Object Implements System.ICloneable.Clone
            Dim ret As New ChapterEntry(_name, _start, _end)
            ret._data = New Dictionary(Of String, String)(_data)
            Return ret
        End Function
    End Class

End Namespace