Imports System.Collections.Generic
Imports System.IO
Imports System.Drawing
Imports System.Text
Imports SAPStudio.Utils
Imports SAPStudio.VFRHelper.VideoProviders


Partial Public Class NativeConstants

    '''WM_USER -> 0x0400
    Public Const WM_USER As Integer = 1024

    Public Const TBM_GETTICPOS As Integer = (WM_USER + 15)

    Public Const TBM_GETCHANNELRECT As Integer = (WM_USER + 26)

    Public Const TBM_GETTHUMBRECT As Integer = (WM_USER + 25)

End Class
