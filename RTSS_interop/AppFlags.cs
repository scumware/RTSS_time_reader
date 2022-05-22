using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTSS_time_reader.RTSS_interop
{
    using DWORD = System.UInt32;

    [Flags]
    public enum AppFlags :DWORD
    {
        None        = 0,
        OpenGL      = APPFLAGS.APPFLAG_OGL,
        DirectDraw  = APPFLAGS.APPFLAG_DD,
        Direct3D8   = APPFLAGS.APPFLAG_D3D8,
        Direct3D9   = APPFLAGS.APPFLAG_D3D9,
        Direct3D9Ex = APPFLAGS.APPFLAG_D3D9EX,
        Direct3D10  = APPFLAGS.APPFLAG_D3D10,
        Direct3D11  = APPFLAGS.APPFLAG_D3D11,
        Direct3D12  = APPFLAGS.APPFLAG_D3D12,
        ArchitectureX64 = APPFLAGS.APPFLAG_ARCHITECTURE_X64,
        ArchitectureUWP = APPFLAGS.APPFLAG_ARCHITECTURE_UWP,

        ProfileUpdateRequested = APPFLAGS.APPFLAG_PROFILE_UPDATE_REQUESTED,
        MASK = (APPFLAGS.APPFLAG_DD | APPFLAGS.APPFLAG_D3D8 | APPFLAGS.APPFLAG_D3D9 | APPFLAGS.APPFLAG_D3D9EX | APPFLAGS.APPFLAG_OGL | APPFLAGS.APPFLAG_D3D10  | APPFLAGS.APPFLAG_D3D11),
    };

    [Flags]
    public enum StatFlags
    {
        None   = 0,
        STATFLAG_RECORD   = 0x00000001,
        Record = STATFLAG_RECORD,
    };
}
