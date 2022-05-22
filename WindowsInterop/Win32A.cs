using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;
// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

namespace RTSS_time_reader.WindowsInterop
{
    using HANDLE = System.IntPtr;
    using DWORD = System.UInt32;
    using SIZE_T = System.UInt64;

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class Win32A
    {
        public const int MAX_PATH = 260;

        public const int ERROR_SUCCESS = 0;
        public const uint INVALID_HANDLE_VALUE = unchecked((uint) -1);
        public static unsafe IntPtr INVALID_HANDLE_PTR;

        static Win32A()
        {
            unsafe
            {
                INVALID_HANDLE_PTR = new IntPtr((void*) INVALID_HANDLE_VALUE);
            }
        }

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern IntPtr GetCurrentThread();


        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint GetCurrentThreadId();

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenThread(uint desiredAccess, bool inheritHandle, uint threadId);

        [DllImport("kernel32.dll", SetLastError = true, EntryPoint = "CloseHandle")]
        public static extern bool CloseHandleInternal(HANDLE handle);

        public static void CloseHandle(HANDLE handle)
        {
            var closed = CloseHandleInternal(handle);
            if (!closed)
                ThrowLastWin32Error();
        }


        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern bool CancelSynchronousIo(IntPtr hThread);

        public static IntPtr CreateNamedPipe(string lpName, uint dwOpenMode,
            uint dwPipeMode, uint nMaxInstances, uint nOutBufferSize, uint nInBufferSize,
            uint nDefaultTimeOut, IntPtr lpSecurityAttributes)
        {
            return CreateNamedPipeA(lpName, dwOpenMode,
                dwPipeMode, nMaxInstances, nOutBufferSize, nInBufferSize,
                nDefaultTimeOut, lpSecurityAttributes);
        }

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern IntPtr CreateNamedPipeA(string lpName,
            DWORD dwOpenMode,
            DWORD dwPipeMode,
            uint nMaxInstances,
            uint nOutBufferSize,
            uint nInBufferSize,
            uint nDefaultTimeOut,
            IntPtr lpSecurityAttributes);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool GetNamedPipeClientProcessId(
            IntPtr hPipe,
            out uint ClientProcessId
        );

        [Flags]
        public enum PipeOpenModeFlags : uint
        {
            PIPE_ACCESS_DUPLEX = 0x00000003,
            PIPE_ACCESS_INBOUND = 0x00000001,
            PIPE_ACCESS_OUTBOUND = 0x00000002,
            FILE_FLAG_FIRST_PIPE_INSTANCE = 0x00080000,
            FILE_FLAG_WRITE_THROUGH = 0x80000000,
            FILE_FLAG_OVERLAPPED = 0x40000000,
            WRITE_DAC = 0x00040000,
            WRITE_OWNER = 0x00080000,
            ACCESS_SYSTEM_SECURITY = 0x01000000
        }

        [Flags]
        public enum PipeModeFlags : uint
        {
            //One of the following type modes can be specified. The same type mode must be specified for each instance of the pipe.
            PIPE_TYPE_BYTE = 0x00000000,
            PIPE_TYPE_MESSAGE = 0x00000004,

            //One of the following read modes can be specified. Different instances of the same pipe can specify different read modes
            PIPE_READMODE_BYTE = 0x00000000,
            PIPE_READMODE_MESSAGE = 0x00000002,

            //One of the following wait modes can be specified. Different instances of the same pipe can specify different wait modes.
            PIPE_WAIT = 0x00000000,
            PIPE_NOWAIT = 0x00000001,

            //One of the following remote-client modes can be specified. Different instances of the same pipe can specify different remote-client modes.
            PIPE_ACCEPT_REMOTE_CLIENTS = 0x00000000,
            PIPE_REJECT_REMOTE_CLIENTS = 0x00000008
        }



        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, KeyModifiers fsModifiers, int vk);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [Flags]
        public enum KeyModifiers : int
        {
            None = 0,
            Alt = 1,
            Ctrl = 2,
            Shift = 4,
            Win = 8,
            MOD_NOREPEAT = 0x4000
        }

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern ushort GlobalAddAtom(string lpString);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern ushort GlobalDeleteAtom(ushort nAtom);

        public enum WindowsMessages : int
        {
            WM_HOTKEY = 0x0312
            ,
            WM_KEYDOWN = 0x0100
            ,
            WM_KEYUP = 0x0101
            ,
            WM_SYSKEYDOWN = 0x0104
            ,
            WM_SYSKEYUP = 0x0105
            ,
            WM_CHAR  = 0x0102
        }

        [DllImport("user32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern int GetKeyNameText(int lParam, [Out] StringBuilder lpString,
            int nSize);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr OpenFileMapping(DWORD dwDesiredAccess, bool bInheritHandle,
            string lpName);


        [DllImport("kernel32.dll")]
        public static extern IntPtr MapViewOfFileEx(
            [In] IntPtr hFileMappingObject,
            [In] FileMapAccessType dwDesiredAccess,
            [In] DWORD dwFileOffsetHigh,
            [In] DWORD dwFileOffsetLow,
            [In] DWORD dwNumberOfBytesToMap,
            [In, Optional] IntPtr lpBaseAddress);

        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr MapViewOfFile(
            [In] HANDLE hFileMappingObject,
            [In] DWORD dwDesiredAccess,
            [In] DWORD dwFileOffsetHigh,
            [In] DWORD dwFileOffsetLow,
            [In] DWORD dwNumberOfBytesToMap
        );

        [DllImport("kernel32.dll")]
        public static extern bool UnmapViewOfFile([In] IntPtr lpBaseAddress);

        [DllImport("Kernel32.dll", EntryPoint = "RtlZeroMemory", SetLastError = false)]
        public static extern void ZeroMemory(IntPtr dest, DWORD size);

        public static void ThrowLastWin32Error()
        {
            var win32Error = Marshal.GetLastWin32Error();
            if (win32Error != Win32A.ERROR_SUCCESS)
            {
                throw new Win32Exception(win32Error);
            }
        }
    }


    [Flags]
    public enum FileMapAccessType : uint
    {
        Copy = 0x01,
        Write = 0x02,
        Read = 0x04,
        AllAccess = 0x08,
        Execute = 0x20,
    }

    [Flags]
    public enum SectionFlags :DWORD
    {
        STANDARD_RIGHTS_REQUIRED    = 0x000F0000,
        SECTION_QUERY               = 0x0001,
        SECTION_MAP_WRITE           = 0x0002,
        SECTION_MAP_READ            = 0x0004,
        SECTION_MAP_EXECUTE         = 0x0008,
        SECTION_EXTEND_SIZE         = 0x0010,
        SECTION_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED | SECTION_QUERY |
                SECTION_MAP_WRITE |
                SECTION_MAP_READ |
                SECTION_MAP_EXECUTE |
                SECTION_EXTEND_SIZE)
    }

    [Flags]
    public enum FileMapFlags : DWORD
    {
        FILE_MAP_ALL_ACCESS = SectionFlags.SECTION_ALL_ACCESS
    }
}
