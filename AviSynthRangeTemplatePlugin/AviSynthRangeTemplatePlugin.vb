Imports SAPStudio.VFRHelper.Plugins
<Assembly: VFRHelperPlugin(GetType(AviSynthRangeTemplatePlugin))> 
<PluginClass(friendlypluginname:="AviSynth Range Templater")> _
Public Class AviSynthRangeTemplatePlugin
    Inherits PluginBase
    Dim _form As Form1

    Protected Overloads Overrides Sub Initialize()
        RegisterPluginFunction(-1, AddressOf Main)
        RegisterHotKeyAction("InsertFrameNumber", AddressOf InsertFrameNumber)
    End Sub

    Function InsertFrameNumber() As Boolean
        CheckDisposed()
        With _form
            .txtPending.Text = String.Join(Host.VideoProvider.CurrentFrameNumber.ToString(), .txtPending.Text.Split(New String() {"###"}, 2, StringSplitOptions.None))
            If Not .txtPending.Text.Contains("###") Then
                .txtScript.SelectedText = .txtPending.Text & ControlChars.CrLf
                .txtScript.Select(.txtScript.SelectionStart + .txtScript.SelectionLength, 0)
                .txtPending.Text = .txtTemplate.Text
            End If
        End With
        Return False
    End Function

    Sub Main()
        _form = New Form1
        _form.Show()
        Host.DockWindow(_form)
    End Sub

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If _form IsNot Nothing Then
                Host.UndockWindow(_form)
                _form.Dispose()
                _form = Nothing
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub
End Class
