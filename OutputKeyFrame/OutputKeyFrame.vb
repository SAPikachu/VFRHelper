Imports SAPStudio.VFRHelper.Plugins
Imports System.IO
<Assembly: VFRHelperPlugin(GetType(OutputKeyFrame))> 

<PluginClass(FriendlyPluginName:="Output timecodes of all keyframes")> _
Public Class OutputKeyFrame
    Inherits PluginBase

    Protected Overloads Overrides Sub Initialize()
        RegisterPluginFunction(-1, AddressOf Main)
    End Sub

    Private Sub Main()
        Try
            If Host.VideoProvider Is Nothing Then
                Return
            End If
            Dim outFile = Host.BrowseSave("*.txt|*.txt")
            If String.IsNullOrEmpty(outFile) Then
                Return
            End If
            Using s = File.Create(outFile), swr As New StreamWriter(s)
                With Host.VideoProvider
                    Dim orgFrame = .CurrentFrameNumber
                    .SeekTo(0)
                    Do
                        swr.WriteLine("{0}|{1}", .CurrentFrameNumber, .GetTimecodeOfCurrentFrame())
                    Loop While .SeekToNextKeyFrame()
                    .SeekTo(orgFrame)
                End With
            End Using
        Finally
            Dispose()
        End Try
    End Sub
End Class
