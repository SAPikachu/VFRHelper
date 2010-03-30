Imports System.Collections.Generic
Imports System.IO
Imports System.Drawing
Imports System.Text
Imports SAPStudio.Utils
Imports SAPStudio.VFRHelper.VideoProviders
Imports System.Runtime.InteropServices


Partial Public Class NativeMethods

    '''Return Type: LRESULT->LONG_PTR->int
    '''hWnd: HWND->HWND__*
    '''Msg: UINT->unsigned int
    '''wParam: WPARAM->UINT_PTR->unsigned int
    '''lParam: LPARAM->LONG_PTR->int
    <DllImport("user32.dll", EntryPoint:="SendMessageW")> _
    Public Shared Function SendMessage( _
        <[In]()> ByVal hWnd As HandleRef, _
        ByVal Msg As UInteger, _
        ByVal wParam As IntPtr, _
        ByVal lParam As IntPtr) As IntPtr
    End Function

    <DllImport("user32.dll", EntryPoint:="SendMessageW")> _
    Public Shared Function SendMessage( _
        <[In]()> ByVal hWnd As HandleRef, _
        ByVal Msg As UInteger, _
        ByVal wParam As IntPtr, _
        ByRef lParam As rect) As IntPtr
    End Function
End Class

<System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)> _
Public Structure RECT

    '''LONG->int
    Public left As Integer

    '''LONG->int
    Public top As Integer

    '''LONG->int
    Public right As Integer

    '''LONG->int
    Public bottom As Integer
End Structure
