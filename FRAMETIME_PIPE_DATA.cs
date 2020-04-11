// ReSharper disable InconsistentNaming

using System.Runtime.InteropServices;

namespace RTSS_time_reader
{
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct FRAMETIME_PIPE_DATA
    {
        [FieldOffset(0)] public fixed byte Data[8];

        [FieldOffset(0)] public uint dwApp;
        [FieldOffset(4)] public uint dwFrametime;
    };
}