Imports SAPStudio.VFRHelper.VideoProviders
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions


Public Structure FrameOption
    Implements ICloneable

    Public MatchCode As String
    Public IsCombed As Boolean()
    Public OtherOptions As String()

    Public Function Clone() As Object Implements System.ICloneable.Clone
        Dim ret As New FrameOption
        ret.MatchCode = MatchCode
        If IsCombed IsNot Nothing Then ret.IsCombed = CType(IsCombed.Clone(), Boolean())
        If OtherOptions IsNot Nothing Then ret.OtherOptions = CType(OtherOptions.Clone(), String())
        Return ret
    End Function
End Structure

Public Class FrameOptionGroup
    Public Start, [End] As Integer
    Public [Option] As FrameOption
    Public Overrides Function ToString() As String
        Dim sb As New StringBuilder
        sb.Append(Start)
        If [End] <> Start Then
            sb.AppendFormat(",{0}", [End])
        End If
        If Not String.IsNullOrEmpty(Me.Option.MatchCode) Then
            sb.AppendFormat(" {0}", Me.Option.MatchCode)
        End If
        If Me.Option.IsCombed IsNot Nothing Then
            sb.AppendFormat(" {0}", New String(Array.ConvertAll(Of Boolean, Char)(Me.Option.IsCombed, Function(c) If(c, "+"c, "-"c))))
        End If
        If Me.Option.OtherOptions IsNot Nothing Then
            For Each opt In Me.Option.OtherOptions
                sb.AppendFormat(" {0}", opt)
            Next
        End If
        Return sb.ToString()
    End Function
End Class