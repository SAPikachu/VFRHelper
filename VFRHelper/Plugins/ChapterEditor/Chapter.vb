Imports System.IO
Imports System.Text
Imports SAPStudio.Utils
Namespace Plugins.ChapterEditor

    Public MustInherit Class Chapter
        Public MustOverride Overrides Function ToString() As String
        Public MustOverride Sub Parse(ByVal content As String)

        Private ReadOnly _entries As New List(Of ChapterEntry)()
        Private ReadOnly _data As New Dictionary(Of String, String)()
        Private _encoding As Encoding = Encoding.UTF8

        Public ReadOnly Property Encoding() As Encoding
            Get
                Return _encoding
            End Get
        End Property
        Public ReadOnly Property Data() As Dictionary(Of String, String)
            Get
                Return _data
            End Get
        End Property
        Public ReadOnly Property Entries() As List(Of ChapterEntry)
            Get
                Return _entries
            End Get
        End Property
        Public Shared Function Open(ByVal filePath As String) As Chapter
            Dim raw As Byte() = File.ReadAllBytes(filePath)
            Dim enc As Encoding = DetectEncoding(raw)
            Dim contents As String
            Using reader As New StreamReader(New MemoryStream(raw), enc)
                contents = reader.ReadToEnd()
            End Using
            Dim c As Chapter
            If filePath.ToLowerInvariant().EndsWith(".xml") Then
                c = New MKVChapter
            Else
                c = New OGMChapter
            End If
            c._encoding = enc
            c.Parse(contents)
            Return c
        End Function
        Shared Function Open(ByVal filePath As String, ByVal encoding As Encoding) As Chapter
            Dim contents As String = File.ReadAllText(filePath, encoding)
            Dim c As Chapter
            If filePath.ToLowerInvariant().EndsWith(".xml") Then
                c = New MKVChapter
            Else
                c = New OGMChapter
            End If
            c._encoding = encoding
            c.Parse(contents)
            Return c
        End Function
        Public Sub Save(ByVal filePath As String, ByVal enc As Encoding)
            Dim orgEncoding As Encoding = _encoding
            _encoding = enc
            Save(filePath)
            _encoding = orgEncoding
        End Sub
        Public Sub Save(ByVal filePath As String)
            File.WriteAllText(filePath, ToString(), Encoding)
        End Sub
    End Class

End Namespace