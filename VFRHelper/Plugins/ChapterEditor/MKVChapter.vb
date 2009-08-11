Imports System.Xml
Imports System.Text
Imports System.IO
Namespace Plugins.ChapterEditor

    Public Class MKVChapter
        Inherits Chapter

        Public Overrides Sub Parse(ByVal content As String)
            Dim doc As New XmlDocument
            Entries.Clear()
            Try
                doc.LoadXml(content)
                If doc.DocumentElement.LocalName <> "Chapters" Then Throw New Exception("The root element must be <Chapters>")
                Dim otherNode As New StringBuilder
                For Each node As XmlNode In doc.DocumentElement.SelectSingleNode("/Chapters/EditionEntry").ChildNodes
                    If node.LocalName <> "ChapterAtom" Then
                        otherNode.Append(node.OuterXml)
                    End If
                Next
                Data.Add("OtherNode", otherNode.ToString())

                For Each node As XmlNode In doc.DocumentElement.SelectNodes("/Chapters/EditionEntry/ChapterAtom")
                    Dim entry As New ChapterEntry(node.SelectSingleNode("ChapterDisplay/ChapterString").InnerText, TimeSpan.Parse(node("ChapterTimeStart").InnerText.TrimEnd("0"c)), TimeSpan.Parse(node("ChapterTimeEnd").InnerText.TrimEnd("0"c)))
                    entry.Data.Add("Node", node.InnerXml)
                    Entries.Add(entry)
                Next
            Catch ex As Exception
                Throw New Exception("Not a valid matroska chapter file", ex)
            End Try
        End Sub

        Public Overrides Function ToString() As String
            Dim doc As New XmlDocument
            doc.LoadXml("<Chapters><EditionEntry></EditionEntry></Chapters>")
            Dim editionEntry As System.Xml.XmlNode = doc.DocumentElement.SelectSingleNode("/Chapters/EditionEntry")
            If Data.ContainsKey("OtherNode") Then
                editionEntry.InnerXml = Data("OtherNode")
            End If
            For Each entry As ChapterEntry In Entries
                Dim atom As XmlElement = doc.CreateElement("ChapterAtom")
                If entry.Data.ContainsKey("Node") Then
                    atom.InnerXml = entry.Data("Node")
                    atom.SelectSingleNode("ChapterDisplay/ChapterString").InnerText = entry.Name
                    atom("ChapterTimeStart").InnerText = Utils.TimeSpanToMKVTimecode(entry.Start)
                    atom("ChapterTimeEnd").InnerText = Utils.TimeSpanToMKVTimecode(entry.End)
                Else
                    atom.InnerXml = String.Format("<ChapterDisplay><ChapterString></ChapterString><ChapterLanguage>und</ChapterLanguage></ChapterDisplay><ChapterTimeStart>{0}00</ChapterTimeStart><ChapterTimeEnd>{1}00</ChapterTimeEnd>", Utils.TimeSpanToMKVTimecode(entry.Start), Utils.TimeSpanToMKVTimecode(entry.End))
                    atom.SelectSingleNode("ChapterDisplay/ChapterString").InnerText = entry.Name
                End If
                editionEntry.AppendChild(atom)
            Next
            Return String.Format("<?xml version=""1.0"" encoding=""{0}""?>{1}{2}", Encoding.WebName, ControlChars.CrLf, doc.DocumentElement.OuterXml)
        End Function
    End Class

End Namespace