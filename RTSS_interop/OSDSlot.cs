using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTSS_time_reader.RTSS_interop
{
    public abstract class OSDSlot
    {
        protected OSDSlot() { }
        public abstract void UpdateOSDslotText(string p_newText);
        public abstract void Clean();
    }
}
