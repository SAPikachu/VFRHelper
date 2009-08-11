Imports System.Runtime.Serialization
Namespace VideoProviders
    <Serializable()> _
    Public Class AviSynthException
        Inherits ApplicationException
        ' Methods
        Public Sub New()
        End Sub

        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub

        Public Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
            MyBase.New(info, context)
        End Sub

        Public Sub New(ByVal message As String, ByVal innerException As Exception)
            MyBase.New(message, innerException)
        End Sub

    End Class

End Namespace