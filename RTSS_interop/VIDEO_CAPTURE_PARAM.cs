using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using RTSS_time_reader.WindowsInterop;
// ReSharper disable InconsistentNaming

using HANDLE = System.IntPtr;
using DWORD = System.UInt32;
using SIZE_T = System.UInt64;
using LONG = System.Int64;
using LARGE_INTEGER = System.Int64;
using BYTE = System.Byte;
using BOOL = System.Int32;
// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace RTSS_time_reader.RTSS_interop
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct VIDEO_CAPTURE_PARAM
    {
        DWORD dwVersion;
        fixed char szFilename[Win32A.MAX_PATH];
        DWORD dwFramerate;
        DWORD dwFramesize;
        DWORD dwFormat;
        DWORD dwQuality;
        DWORD dwThreads;
        BOOL bCaptureOSD;
        DWORD dwAudioCaptureFlags;
        DWORD dwVideoCaptureFlagsEx;
        DWORD dwAudioCaptureFlags2;
        DWORD dwPrerecordSizeLimit;
        DWORD dwPrerecordTimeLimit;
    }
}
