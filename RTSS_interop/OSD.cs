using RTSS_time_reader.WindowsInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading;

// ReSharper disable LocalizableElement

namespace RTSS_time_reader.RTSS_interop
{
    using DWORD = System.UInt32;
    using HANDLE = System.IntPtr;
    using OSD_EntryT = RTSS_SHARED_MEMORY.RTSS_SHARED_MEMORY_OSD_ENTRY;

    public unsafe class OSD: IDisposable
    {
        // ReSharper disable once InconsistentNaming
        const int TICKS_PER_MICROSECOND = 10;

        private string m_entryName;
        private bool m_disposed;
        private readonly IntPtr m_hMapFile;
        private readonly RTSS_SHARED_MEMORY* m_rtssMemory;
        private static readonly ulong rtss2_7_ver = RTSS_VERSION(2, 7);
        private readonly List<OSDSlotInternal> m_osdSlots;
        private volatile bool m_ownLock;

        /// <param name="p_entryName">
        ///     The name of the OSD entry. Should be unique and not more than 255 chars once converted to ANSI.
        /// </param>
        /// <param name="p_osd"></param>
        public OSD(string p_entryName)
        {
            if(string.IsNullOrWhiteSpace(p_entryName) )
                throw new ArgumentException("Entry name cannot be null, empty, or whitespace", "p_entryName");

            if(p_entryName.Length > 255 )
                throw new ArgumentException("Entry name exceeds max length of 255 when converted to ANSI", "p_entryName");

            m_entryName = p_entryName;

            //just open/close to make sure RTSS is working
            OpenSharedMemory(out m_hMapFile, out m_rtssMemory);

            m_disposed = false;
            m_osdSlots = new List<OSDSlotInternal>();
        }


        public OSDEntry[] GetExitingOSDEntries()
        {
            var list = new List<OSDEntry>();

            LockMemory();
            var size = m_rtssMemory->dwOSDArrSize;
            try
            {
                var osdEntryItems = m_rtssMemory->OSD_ENTRY_ITEMS;
                for (int i = 0; i < size; i++)
                {
                    var pEntry = (*osdEntryItems)[i];

                    if (ByteUtils.Strlen(pEntry->szOSDOwner) > 0)
                    {
                        var entry = new OSDEntry
                        {
                            Owner = new string(pEntry->szOSDOwner),
                            Text = m_rtssMemory->dwVersion >= rtss2_7_ver ? new string(pEntry->szOSDEx) : new string(pEntry->szOSD)
                        };

                        list.Add(entry);
                    }
                }

            }
            finally
            {
                UnlockMemory();
            }

            return list.ToArray();
        }


        public OSDSlot GrabOSDSlot()
        {
            return GrabOSDSlots(1)[0];
        }


        public IList<OSDSlot> GrabOSDSlots(int p_count)
        {
            var osdSlots = new List<OSDSlot>();
            LockMemory();
            try
            {
                for (int osdSlotNumber = 1; osdSlotNumber < m_rtssMemory->dwOSDArrSize; osdSlotNumber++)
                {
                    if (p_count == 0)
                        break;

                    var pEntry = (*m_rtssMemory->OSD_ENTRY_ITEMS)[osdSlotNumber];

                    //if we need a new slot and this one is unused, claim it
                    if (0 == ByteUtils.Strlen(pEntry->szOSDOwner))
                    {
                        ByteUtils.strcpy_s(pEntry->szOSDOwner, m_entryName);
                        var grabbedSlot = new OSDSlotInternal(pEntry, this);
                        osdSlots.Add(grabbedSlot);
                        m_osdSlots.Add(grabbedSlot);
                        --p_count;
                    }
                }
            }
            finally
            {
                UnlockMemory();
            }

            return osdSlots;
        }

        public List<OSDSlot> FindOsdSlots()
        {
            var osdSlots = new List<OSDSlot>();
            LockMemory();
            try
            {
                for (int osdSlotNumber = 1; osdSlotNumber < m_rtssMemory->dwOSDArrSize; osdSlotNumber++)
                {

                    var pEntry = (*m_rtssMemory->OSD_ENTRY_ITEMS)[osdSlotNumber];

                    //if we need a new slot and this one is unused, claim it
                    if (0 == ByteUtils.Strlen(pEntry->szOSDOwner))
                    {
                        continue;
                    }

                    //if this is our slot
                    if (ByteUtils.StrCmp(pEntry->szOSDOwner, m_entryName))
                    {
                        foreach (var osdSlot in m_osdSlots)
                        {
                            if (osdSlot.ContainsEntry(pEntry))
                            {
                                osdSlots.Add(osdSlot);
                            }
                            else
                            {
                                osdSlots.Add(new OSDSlotInternal(pEntry, this));
                            }
                        }
                    }
                }
            }
            finally
            {
                UnlockMemory();
            }

            return osdSlots;
        }

        ///<summary>
        ///Text should be no longer than 4095 chars once converted to ANSI. Lower case looks awful.
        ///</summary>
        public void Update(OSDSlot p_osdSlot, string p_newText)
        {
            if (p_osdSlot == null)
                throw new ArgumentNullException(nameof(p_osdSlot));
            if (p_newText == null)
                throw new ArgumentNullException("p_newText");
            if (m_disposed)
                throw new ObjectDisposedException("OSD");

            try
            {
                LockMemory();
                p_osdSlot.UpdateOSDslotText(p_newText);
            }
            finally
            {
                UnlockMemory();
            }
        }

        public AppEntry[] GetAppEntries()
        {
            var list = new List<AppEntry>();

            LockMemory();
            try
            {
                var appEntryItems = m_rtssMemory->APP_ENTRY_ITEMS;

                //include all slots
                for (int i = 0; i < m_rtssMemory->dwAppArrSize; i++)
                {
                    RTSS_SHARED_MEMORY.RTSS_SHARED_MEMORY_APP_ENTRY* pEntry = (*appEntryItems)[i];

                    if (0 != pEntry->dwProcessID)
                    {
                        var entry = new AppEntry();

                        //basic fields
                        entry.ProcessId = pEntry->dwProcessID;
                        entry.Name = new string(pEntry->szName);
                        entry.Flags = (AppFlags) pEntry->dwFlags;

                        //instantaneous framerate fields
                        entry.InstantaneousTimeStart = TimeFromTickCount(pEntry->dwTime0);
                        entry.InstantaneousTimeEnd = TimeFromTickCount(pEntry->dwTime1);
                        entry.InstantaneousFrames = pEntry->dwFrames;
                        entry.InstantaneousFrameTime = TimeSpan.FromTicks(pEntry->dwFrameTime * TICKS_PER_MICROSECOND);

                        //framerate stats fields
                        entry.StatFlags = (StatFlags) pEntry->dwStatFlags;
                        entry.StatTimeStart = TimeFromTickCount(pEntry->dwStatTime0);
                        entry.StatTimeEnd = TimeFromTickCount(pEntry->dwStatTime1);
                        entry.StatFrames = pEntry->dwStatFrames;
                        entry.StatCount = pEntry->dwStatCount;
                        entry.StatFramerateMin = pEntry->dwStatFramerateMin;
                        entry.StatFramerateAvg = pEntry->dwStatFramerateAvg;
                        entry.StatFramerateMax = pEntry->dwStatFramerateMax;
                        if (m_rtssMemory->dwVersion >= RTSS_VERSION(2, 5))
                        {
                            entry.StatFrameTimeMin = pEntry->dwStatFrameTimeMin;
                            entry.StatFrameTimeAvg = pEntry->dwStatFrameTimeAvg;
                            entry.StatFrameTimeMax = pEntry->dwStatFrameTimeMax;
                            entry.StatFrameTimeCount = pEntry->dwStatFrameTimeCount;
                        }

                        //OSD fields
                        entry.OSDCoordinateX = pEntry->dwOSDX;
                        entry.OSDCoordinateY = pEntry->dwOSDY;
                        entry.OSDZoom = pEntry->dwOSDPixel;
                        entry.OSDFrameId = pEntry->dwOSDFrame;
                        entry.OSDColor = Color.FromArgb((int) pEntry->dwOSDColor);
                        if (m_rtssMemory->dwVersion >= RTSS_VERSION(2, 1))
                            entry.OSDBackgroundColor = Color.FromArgb((int) pEntry->dwOSDBgndColor);

                        //screenshot fields
                        entry.ScreenshotFlags = (SCREENCAPTUREFLAGS) pEntry->dwScreenCaptureFlags;
                        entry.ScreenshotPath = new string(pEntry->szScreenCapturePath);
                        if (m_rtssMemory->dwVersion >= RTSS_VERSION(2, 2))
                        {
                            entry.ScreenshotQuality = pEntry->dwScreenCaptureQuality;
                            entry.ScreenshotThreads = pEntry->dwScreenCaptureThreads;
                        }

//video capture fields
                        if (m_rtssMemory->dwVersion >= RTSS_VERSION(2, 2))
                        {
                            entry.VideoCaptureFlags = (VideoCaptureFlags) pEntry->dwVideoCaptureFlags;
                            entry.VideoCapturePath = new string(pEntry->szVideoCapturePath);
                            entry.VideoFramerate = pEntry->dwVideoFramerate;
                            entry.VideoFramesize = pEntry->dwVideoFramesize;
                            entry.VideoFormat = pEntry->dwVideoFormat;
                            entry.VideoQuality = pEntry->dwVideoQuality;
                            entry.VideoCaptureThreads = pEntry->dwVideoCaptureThreads;
                        }

                        if (m_rtssMemory->dwVersion >= RTSS_VERSION(2, 4))
                            entry.VideoCaptureFlagsEx = pEntry->dwVideoCaptureFlagsEx;

//audio capture fields
                        if (m_rtssMemory->dwVersion >= RTSS_VERSION(2, 3))
                            entry.AudioCaptureFlags = pEntry->dwAudioCaptureFlags;
                        if (m_rtssMemory->dwVersion >= RTSS_VERSION(2, 5))
                            entry.AudioCaptureFlags2 = pEntry->dwAudioCaptureFlags2;
                        if (m_rtssMemory->dwVersion >= RTSS_VERSION(2, 6))
                        {
                            entry.AudioCapturePTTEventPush = pEntry->qwAudioCapturePTTEventPush;
                            entry.AudioCapturePTTEventRelease = pEntry->qwAudioCapturePTTEventRelease;
                            entry.AudioCapturePTTEventPush2 = pEntry->qwAudioCapturePTTEventPush2;
                            entry.AudioCapturePTTEventRelease2 = pEntry->qwAudioCapturePTTEventRelease2;
                        }

                        list.Add(entry);
                    }
                }
            }
            finally
            {
                UnlockMemory();
            }

            return list.ToArray();
        }




        protected static void OpenSharedMemory(out HANDLE hMapFile, out RTSS_SHARED_MEMORY* p_rtssMemory)
        {
            hMapFile = IntPtr.Zero;
            p_rtssMemory = null;

            IntPtr mapFileHandle = IntPtr.Zero;
            RTSS_SHARED_MEMORY* rtss_memory = null;
            try
            {
                mapFileHandle = Win32A.OpenFileMapping((DWORD)FileMapFlags.FILE_MAP_ALL_ACCESS, false, "RTSSSharedMemoryV2");
                if (mapFileHandle == IntPtr.Zero)
                    Win32A.ThrowLastWin32Error();

                rtss_memory = (RTSS_SHARED_MEMORY*)Win32A.MapViewOfFile(mapFileHandle, (DWORD)FileMapFlags.FILE_MAP_ALL_ACCESS, 0, 0, 0);
                if (rtss_memory == null)
                    Win32A.ThrowLastWin32Error();

                if (!(rtss_memory->dwSignature == RTSS_SHARED_MEMORY.dwSignatureRTSS && rtss_memory->dwVersion >= RTSS_VERSION(2, 0)))
                    throw new System.IO.InvalidDataException("Failed to validate RTSS Shared Memory structure");

                hMapFile = mapFileHandle;
                p_rtssMemory = rtss_memory;
            }
            catch
            {
                CloseSharedMemory(mapFileHandle, rtss_memory);
                throw;
            }
        }

        protected static ulong RTSS_VERSION(uint p_hi, uint p_low)
        {
            return ((p_hi << 16) + p_low);
        }

        protected static void CloseSharedMemory(HANDLE hMapFile, RTSS_SHARED_MEMORY *pMem)
        {
            if (pMem != null)
                Win32A.UnmapViewOfFile((IntPtr)pMem);

            if (hMapFile != IntPtr.Zero)
                Win32A.CloseHandle(hMapFile);
        }

        ~OSD()
        {
            if (m_disposed)
                return;

            Dispose();
        }

        public void Dispose()
        {
            if (m_disposed)
                return;

            foreach (var osdSlot in m_osdSlots)
            {
                osdSlot.Clean();
                ForceOSDUpdateInternal();
            }

            CloseSharedMemory(m_hMapFile, m_rtssMemory);
            m_disposed = true;
        }


        public void ForceOSDUpdate()
        {
            if (m_disposed)
                throw new ObjectDisposedException("OSD");

            ForceOSDUpdateInternal();
        }

        private static DateTime TimeFromTickCount(DWORD ticks)
        {
            return DateTime.Now - TimeSpan.FromMilliseconds(ticks);
        }


        protected uint OSDEntrySize()
        {
            return m_rtssMemory->dwOSDEntrySize;
        }

        protected uint OsdVersion()
        {
            return m_rtssMemory->dwVersion;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void ForceOSDUpdateInternal()
        {
            LockMemory();
            try
            {
                m_rtssMemory->dwOSDFrame++; //forces OSD update
            }
            finally
            {
                UnlockMemory();
            }
        }


        private void UnlockMemory()
        {
            m_ownLock = false;
            m_rtssMemory->dwBusy = 0;
        }

        private void LockMemory()
        {
            if (m_ownLock)
                return;

            while (1 == Interlocked.CompareExchange(ref m_rtssMemory->dwBusy, 1, 0))
            {
                Thread.SpinWait(100);
            }

            m_ownLock = true;
        }


        protected class OSDSlotInternal : OSDSlot, IDisposable
        {
            private readonly OSD_EntryT* m_osdEntry;
            private readonly OSD m_osd;

            public OSDSlotInternal(OSD_EntryT* p_osdEntry, OSD p_osd)
            {
                this.m_osdEntry = p_osdEntry;
                m_osd = p_osd;
            }

            public override void UpdateOSDslotText(string p_newText)
            {
                if (p_newText == null)
                    throw new ArgumentNullException("p_newText");

                var lpNewText = (sbyte*)Marshal.StringToHGlobalAnsi(p_newText).ToPointer();
                try
                {
                    if (ByteUtils.Strlen(lpNewText) > 4095)
                        throw new ArgumentException("Text exceeds max length of 4095 when converted to ANSI", "p_newText");

                    UpdateOSDslotText(m_osdEntry, lpNewText);
                }
                finally
                {
                    Marshal.FreeHGlobal((IntPtr)lpNewText);
                }
            }

            private void UpdateOSDslotText(OSD_EntryT* pEntry, sbyte* lpNewText)
            {
                //use extended text slot for v2.7 and higher shared memory, it allows displaying 4096 symbols instead of 256 for regular text slot
                if (m_osd.OsdVersion() >= rtss2_7_ver)
                {
                    ByteUtils.strncpy_s(pEntry->szOSDEx, lpNewText, OSD_EntryT.szOSDExCharsCount - 1);
                }
                else
                {
                    ByteUtils.strncpy_s(pEntry->szOSD, lpNewText, OSD_EntryT.szOSDCharsCount - 1);
                }
                m_osd.ForceOSDUpdateInternal();
            }


            public unsafe bool ContainsEntry(RTSS_SHARED_MEMORY.RTSS_SHARED_MEMORY_OSD_ENTRY* p_pEntry)
            {
                return m_osdEntry == p_pEntry;
            }


            public void Dispose()
            {
                Clean();
            }

            public override void Clean()
            {
                Win32A.ZeroMemory((IntPtr)(m_osdEntry), m_osd.OSDEntrySize()); //won't get optimized away
            }
        }
    }
}
