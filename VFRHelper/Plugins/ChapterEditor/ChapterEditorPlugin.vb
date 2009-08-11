Imports SAPStudio.VFRHelper.Plugins
Imports SAPStudio.VFRHelper.Plugins.ChapterEditor
<Assembly: VFRHelperPlugin(GetType(ChapterEditorPlugin), FunctionName:="New chapter", FunctionID:=ChapterEditorPlugin.FUNCTION_NEW_CHAPTER)> 
<Assembly: VFRHelperPlugin(GetType(ChapterEditorPlugin), FunctionName:="Open chapter", FunctionID:=ChapterEditorPlugin.FUNCTION_OPEN_CHAPTER)> 
Namespace Plugins.ChapterEditor
    <PluginClass(FriendlyPluginName:="Chapter Editor")> _
    Public Class ChapterEditorPlugin
        Inherits PluginBase

        Public Const FUNCTION_NEW_CHAPTER As Integer = 0
        Public Const FUNCTION_OPEN_CHAPTER As Integer = 1

        Dim _chapEditor As ChapterEditorForm

        Protected Overloads Overrides Sub Initialize()
            RegisterPluginFunction(FUNCTION_NEW_CHAPTER, AddressOf Function_NewChapter)
            RegisterPluginFunction(FUNCTION_OPEN_CHAPTER, AddressOf Function_OpenChapter)
            RegisterHotKeyAction("ChapterSetTimecode", AddressOf Action_SetTimecode)
            RegisterHotKeyAction("ChapterListMoveUp", AddressOf Action_ChapterListMoveUp)
            RegisterHotKeyAction("ChapterListMoveDown", AddressOf Action_ChapterListMoveDown)
        End Sub

        Sub OpenChap(ByVal fileName As String)
            Dim chap As Chapter
            Try
                chap = If(String.IsNullOrEmpty(fileName), New OGMChapter, Chapter.Open(fileName))
            Catch ex As Exception
                Throw New ApplicationException(String.Format("Unable to open chapter file:{0}{1}", ControlChars.CrLf, ex.Message), ex)
                Return
            End Try
            If _chapEditor IsNot Nothing Then
                Try
                    _chapEditor.Dispose()
                Catch
                End Try
            End If
            _chapEditor = New ChapterEditorForm
            AddHandler _chapEditor.FormClosed, AddressOf OnChapterEditorClosed
            _chapEditor.Open(chap)
            Host.DockWindow(_chapEditor)
        End Sub

        Private Sub OnChapterEditorClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs)
            Dispose()
        End Sub

        Private Sub Function_NewChapter()
            OpenChap(Nothing)
        End Sub

        Private Sub Function_OpenChapter()
            Dim fileName = Host.BrowseOpen("Chapter File|*.txt;*.xml")
            If fileName IsNot Nothing Then
                OpenChap(fileName)
            Else
                Dispose()
            End If
        End Sub

        Private Function Action_SetTimecode() As Boolean
            CheckDisposed()
            _chapEditor.SetTimecode(TimeSpan.Parse(Host.VideoProvider.GetTimecodeOfCurrentFrame()))
            Return False
        End Function
        Private Function Action_ChapterListMoveUp() As Boolean
            CheckDisposed()
            _chapEditor.MoveUp()
            Return False
        End Function
        Private Function Action_ChapterListMoveDown() As Boolean
            CheckDisposed()
            _chapEditor.MoveDown()
            Return False
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
                Try
                    If _chapEditor IsNot Nothing Then _chapEditor.Dispose()
                Catch
                End Try
                _chapEditor = Nothing
            End If
            MyBase.Dispose(disposing)
        End Sub
    End Class

End Namespace