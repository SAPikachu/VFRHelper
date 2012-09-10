'Originally written by Christian Birkl
'http://www.codeproject.com/KB/trace/DbMonNET.aspx


Imports System
Imports System.Threading
Imports System.Runtime.InteropServices

''' <summary>
''' Delegate used when firing DebugMonitor.OnOutputDebug event
''' </summary>
Public Delegate Sub OnOutputDebugStringHandler(ByVal pid As Integer, ByVal text As String)

''' <summary>
''' This class captures all strings passed to <c>OutputDebugString</c> when
''' the application is not debugged.	
''' </summary>
''' <remarks>	
'''	This class is a port of Microsofts Visual Studio's C++ example "dbmon", which
'''	can be found at <c>http://msdn.microsoft.com/library/default.asp?url=/library/en-us/vcsample98/html/vcsmpdbmon.asp</c>.
'''		<code>
'''			public static void Main(string[] args) {
'''				DebugMonitor.Start();
'''				DebugMonitor.OnOutputDebugString += new OnOutputDebugStringHandler(OnOutputDebugString);
'''				Console.WriteLine("Press 'Enter' to exit.");
'''				Console.ReadLine();
'''				DebugMonitor.Stop();
'''			}
'''			
'''			private static void OnOutputDebugString(int pid, string text) {
'''				Console.WriteLine(DateTime.Now + ": " + text);
'''			}
'''		</code>
''' </remarks>
Public NotInheritable Class DebugMonitor

    ''' <summary>
    ''' Private constructor so no one can create a instance
    ''' of this static class
    ''' </summary>
    Private Sub New()


    End Sub

#Region "Win32 API Imports"

    <StructLayout(LayoutKind.Sequential)> _
    Private Structure SECURITY_DESCRIPTOR
        Public revision As Byte
        Public size As Byte
        Public control As Short
        Public owner As IntPtr
        Public group As IntPtr
        Public sacl As IntPtr
        Public dacl As IntPtr
    End Structure

    <StructLayout(LayoutKind.Sequential)> _
    Private Structure SECURITY_ATTRIBUTES
        Public nLength As Integer
        Public lpSecurityDescriptor As IntPtr
        Public bInheritHandle As Integer
    End Structure

    <Flags()> _
    Private Enum PageProtection As UInteger
        NoAccess = &H1
        [Readonly] = &H2
        ReadWrite = &H4
        WriteCopy = &H8
        Execute = &H10
        ExecuteRead = &H20
        ExecuteReadWrite = &H40
        ExecuteWriteCopy = &H80
        Guard = &H100
        NoCache = &H200
        WriteCombine = &H400
    End Enum


    Private Const WAIT_OBJECT_0 As Integer = 0
    Private Const INFINITE As UInteger = &HFFFFFFFFUI
    Private Const ERROR_ALREADY_EXISTS As Integer = 183

    Private Const SECURITY_DESCRIPTOR_REVISION As UInteger = 1

    Private Const SECTION_MAP_READ As UInteger = &H4

    <DllImport("kernel32.dll", SetLastError:=True)> _
    Private Shared Function MapViewOfFile(ByVal hFileMappingObject As IntPtr, ByVal dwDesiredAccess As UInteger, ByVal dwFileOffsetHigh As UInteger, ByVal dwFileOffsetLow As UInteger, ByVal dwNumberOfBytesToMap As UInteger) As IntPtr
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)> _
    Private Shared Function UnmapViewOfFile(ByVal lpBaseAddress As IntPtr) As Boolean
    End Function

    <DllImport("advapi32.dll", SetLastError:=True)> _
    Private Shared Function InitializeSecurityDescriptor(ByRef sd As SECURITY_DESCRIPTOR, ByVal dwRevision As UInteger) As Boolean
    End Function

    <DllImport("advapi32.dll", SetLastError:=True)> _
    Private Shared Function SetSecurityDescriptorDacl(ByRef sd As SECURITY_DESCRIPTOR, ByVal daclPresent As Boolean, ByVal dacl As IntPtr, ByVal daclDefaulted As Boolean) As Boolean
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)> _
    Private Shared Function CreateEvent(ByRef sa As SECURITY_ATTRIBUTES, ByVal bManualReset As Boolean, ByVal bInitialState As Boolean, ByVal lpName As String) As IntPtr
    End Function

    <DllImport("kernel32.dll")> _
    Private Shared Function PulseEvent(ByVal hEvent As IntPtr) As Boolean
    End Function

    <DllImport("kernel32.dll")> _
    Private Shared Function SetEvent(ByVal hEvent As IntPtr) As Boolean
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)> _
    Private Shared Function CreateFileMapping(ByVal hFile As IntPtr, lpFileMappingAttributes As IntPtr, ByVal flProtect As PageProtection, ByVal dwMaximumSizeHigh As UInteger, ByVal dwMaximumSizeLow As UInteger, ByVal lpName As String) As IntPtr
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)> _
    Private Shared Function OpenFileMapping(ByVal dwDesiredAccess As UInteger, bInheritHandle As Boolean, ByVal lpName As String) As IntPtr
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)> _
    Private Shared Function CloseHandle(ByVal hHandle As IntPtr) As Boolean
    End Function

    Private Declare Auto Function WaitForSingleObject Lib "kernel32" (ByVal handle As IntPtr, ByVal milliseconds As UInteger) As Int32
#End Region

    ''' <summary>
    ''' Fired if an application calls <c>OutputDebugString</c>
    ''' </summary>
    Public Shared Event OnOutputDebugString As OnOutputDebugStringHandler

    ''' <summary>
    ''' Event handle for slot 'DBWIN_BUFFER_READY'
    ''' </summary>
    Private Shared m_AckEvent As EventWaitHandle

    ''' <summary>
    ''' Event handle for slot 'DBWIN_DATA_READY'
    ''' </summary>
    Private Shared m_ReadyEvent As EventWaitHandle

    ''' <summary>
    ''' Handle for our shared file
    ''' </summary>
    Private Shared m_SharedFile As IntPtr = IntPtr.Zero

    ''' <summary>
    ''' Handle for our shared memory
    ''' </summary>
    Private Shared m_SharedMem As IntPtr = IntPtr.Zero

    ''' <summary>
    ''' Our capturing thread
    ''' </summary>
    Private Shared m_Capturer As Thread = Nothing

    ''' <summary>
    ''' Our synchronization root
    ''' </summary>
    Private Shared m_SyncRoot As New Object()

    ''' <summary>
    ''' Starts this debug monitor
    ''' </summary>
    Public Shared Sub Start()
        SyncLock m_SyncRoot
            If m_Capturer IsNot Nothing Then
                Throw New ApplicationException("This DebugMonitor is already started.")
            End If

            ' Check for supported operating system. Mono (at least with *nix) won't support
            ' our P/Invoke calls.
            If Environment.OSVersion.ToString().IndexOf("Microsoft") = -1 Then
                Throw New NotSupportedException("This DebugMonitor is only supported on Microsoft operating systems.")
            End If

            Dim createdNew As Boolean

            ' Create the event for slot 'DBWIN_BUFFER_READY'
            m_AckEvent = New EventWaitHandle(False, EventResetMode.AutoReset, "DBWIN_BUFFER_READY", createdNew)

            ' Create the event for slot 'DBWIN_DATA_READY'
            m_ReadyEvent = New EventWaitHandle(False, EventResetMode.AutoReset, "DBWIN_DATA_READY", createdNew)

            ' Get a handle to the readable shared memory at slot 'DBWIN_BUFFER'.
            m_SharedFile = OpenFileMapping(SECTION_MAP_READ, False, "DBWIN_BUFFER")
            If m_SharedFile = IntPtr.Zero Then
                m_SharedFile = CreateFileMapping(New IntPtr(-1), IntPtr.Zero, PageProtection.ReadWrite, 0, 4096, "DBWIN_BUFFER")
                If m_SharedFile = IntPtr.Zero Then
                    Throw CreateApplicationException("Failed to create a file mapping to slot 'DBWIN_BUFFER'")
                End If
            End If

            ' Create a view for this file mapping so we can access it
            m_SharedMem = MapViewOfFile(m_SharedFile, SECTION_MAP_READ, 0, 0, 512)
            If m_SharedMem = IntPtr.Zero Then
                Throw CreateApplicationException("Failed to create a mapping view for slot 'DBWIN_BUFFER'")
            End If

            ' Start a new thread where we can capture the output
            ' of OutputDebugString calls so we don't block here.
            m_Capturer = New Thread(New ThreadStart(AddressOf Capture))
            m_Capturer.Start()
        End SyncLock
    End Sub

    ''' <summary>
    ''' Captures 
    ''' </summary>
    Private Shared Sub Capture()
        Try
            ' Everything after the first DWORD is our debugging text
            Dim pString As New IntPtr(m_SharedMem.ToInt64() + Marshal.SizeOf(GetType(Integer)))

            While True
                m_AckEvent.Set()

                Dim ret As Boolean = m_ReadyEvent.WaitOne()

                ' if we have no capture set it means that someone
                ' called 'Stop()' and is now waiting for us to exit
                ' this endless loop.
                If m_Capturer Is Nothing Then
                    Exit While
                End If

                If ret Then
                    ' The first DWORD of the shared memory buffer contains
                    ' the process ID of the client that sent the debug string.
                    FireOnOutputDebugString(Marshal.ReadInt32(m_SharedMem), Marshal.PtrToStringAnsi(pString))
                End If
            End While

        Catch ex As Exception
            If Debugger.IsAttached Then
                MessageBox.Show(ex.ToString)
            End If
            Throw

            ' Cleanup
        Finally
            Dispose()
        End Try
    End Sub

    Private Shared Sub FireOnOutputDebugString(ByVal pid As Integer, ByVal text As String)
        ' Raise event if we have any listeners


#If Not Debug Then
        Try
#End If
            RaiseEvent OnOutputDebugString(pid, text)
#If Not Debug Then
        Catch ex As Exception
            Console.WriteLine("An 'OnOutputDebugString' handler failed to execute: " & ex.ToString())
        End Try
#End If
    End Sub

    ''' <summary>
    ''' Dispose all resources
    ''' </summary>
    Private Shared Sub Dispose()
        ' Close AckEvent
        If m_AckEvent IsNot Nothing Then
            m_AckEvent.Close()
            m_AckEvent = Nothing
        End If

        ' Close ReadyEvent
        If m_ReadyEvent IsNot Nothing Then
            m_ReadyEvent.Close()
            m_ReadyEvent = Nothing
        End If


        ' Unmap SharedMem
        If m_SharedMem <> IntPtr.Zero Then
            If Not UnmapViewOfFile(m_SharedMem) Then
                Throw CreateApplicationException("Failed to unmap view for slot 'DBWIN_BUFFER'")
            End If
            m_SharedMem = IntPtr.Zero
        End If
        ' Close SharedFile
        If m_SharedFile <> IntPtr.Zero Then
            If Not CloseHandle(m_SharedFile) Then
                Throw CreateApplicationException("Failed to close handle for 'SharedFile'")
            End If
            m_SharedFile = IntPtr.Zero
        End If
    End Sub

    ''' <summary>
    ''' Stops this debug monitor. This call we block the executing thread
    ''' until this debug monitor is stopped.
    ''' </summary>
    Public Shared Sub [Stop]()
        SyncLock m_SyncRoot
            If m_Capturer Is Nothing Then
                Throw New ObjectDisposedException("DebugMonitor", "This DebugMonitor is not running.")
            End If
            m_Capturer = Nothing
            While m_AckEvent IsNot Nothing
                m_ReadyEvent.Set()
                m_ReadyEvent.Reset()
                Thread.Sleep(10)
            End While
        End SyncLock
    End Sub

    ''' <summary>
    ''' Helper to create a new application exception, which has automaticly the 
    ''' last win 32 error code appended.
    ''' </summary>
    ''' <param name="text">text</param>
    Private Shared Function CreateApplicationException(ByVal text As String) As ApplicationException
        If text Is Nothing OrElse text.Length < 1 Then
            Throw New ArgumentNullException("text", "'text' may not be empty or null.")
        End If

        Return New ApplicationException(String.Format("{0}. Last Win32 Error was {1}", text, Marshal.GetLastWin32Error()))
    End Function
End Class
