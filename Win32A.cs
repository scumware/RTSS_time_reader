using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

namespace RTSS_time_reader
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class Win32A
    {
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

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr handle);

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
            uint dwOpenMode,
            uint dwPipeMode,
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

        [DllImport("user32.dll")]
        public static extern int GetKeyNameText(int lParam, [Out] StringBuilder lpString,
            int nSize);
    }
}
