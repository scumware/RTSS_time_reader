using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using RTSS_time_reader.WindowsInterop;

using HANDLE = System.IntPtr;
using DWORD = System.UInt32;
using SIZE_T = System.UInt64;
using LONG = System.Int64;
using LARGE_INTEGER = System.Int64;
using BYTE = System.Byte;
using FLOAT= System.Single;
using CHAR= System.SByte;
// ReSharper disable BuiltInTypeReferenceStyle
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable CommentTypo
// ReSharper disable UnusedType.Global
// ReSharper disable LocalizableElement

// ReSharper disable IdentifierTypo
#pragma warning disable 169

// ReSharper disable InconsistentNaming
namespace RTSS_time_reader.RTSS_interop
{
	/////////////////////////////////////////////////////////////////////////////
	//# include "RTSSHooksTypes.h"
	/////////////////////////////////////////////////////////////////////////////

	// v1.0 memory structure
    [StructLayout(LayoutKind.Sequential)]
	public struct RTSS_SHARED_MEMORY_V_1_0
	{
        public DWORD dwSignature;
		//signature allows applications to verify status of shared memory

		//The signature can be set to:
		//'RTSS'	- statistics server's memory is initialized and contains 
		//			valid data 
		//0xDEAD	- statistics server's memory is marked for deallocation and
		//			no longer contain valid data
		//otherwise	the memory is not initialized
        public DWORD dwVersion;
		//structure version ((major<<16) + minor)
		//must be set to 0x00010000 for v1.0 structure
        public DWORD dwTime0;
		//start time of framerate measurement period (in milliseconds)

		//Take a note that this field must contain non-zero value to calculate 
		//framerate properly!
        public DWORD dwTime1;
		//end time of framerate measurement period (in milliseconds)
        public DWORD dwFrames;
		//amount of frames rendered during (dwTime1 - dwTime0) period 

		//to calculate framerate use the following formula:
		//1000.0f * dwFrames / (dwTime1 - dwTime0)
	}

	/////////////////////////////////////////////////////////////////////////////
	//use this flag to force the server to update OSD
	/////////////////////////////////////////////////////////////////////////////
	// v1.1 memory structure
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct RTSS_SHARED_MEMORY_V_1_1
	{
        public DWORD dwSignature;
		//signature allows applications to verify status of shared memory

		//The signature can be set to:
		//'RTSS'	- statistics server's memory is initialized and contains 
		//			valid data 
		//0xDEAD	- statistics server's memory is marked for deallocation and
		//			no longer contain valid data
		//otherwise	the memory is not initialized
        public DWORD dwVersion;
		//structure version ((major<<16) + minor)
		//must be set to 0x00010001 for v1.1 structure
        public DWORD dwTime0;
		//start time of framerate measurement period (in milliseconds)

		//Take a note that this field must contain non-zero value to calculate 
		//framerate properly!
        public DWORD dwTime1;
		//end time of framerate measurement period (in milliseconds)
        public DWORD dwFrames;
		//amount of frames rendered during (dwTime1 - dwTime0) period

		//to calculate framerate use the following formula:
		//1000.0f * dwFrames / (dwTime1 - dwTime0)

        public DWORD dwOSDFlags;
		//bitmask, containing combination of OSDFLAG_... flags

		//Note: set OSDFLAG_UPDATED flag as soon as you change any OSD related
		//field
        public DWORD dwOSDX;
		//OSD X-coordinate (coordinate wrapping is allowed, i.e. -5 defines 5
		//pixel offset from the right side of the screen)
        public DWORD dwOSDY;
		//OSD Y-coordinate (coordinate wrapping is allowed, i.e. -5 defines 5
		//pixel offset from the bottom side of the screen)
        public DWORD dwOSDPixel;
		//OSD pixel zooming ratio
        public DWORD dwOSDColor;
		//OSD color in RGB format
        public fixed CHAR szOSD[256];
		//OSD text
        public fixed CHAR szOSDOwner[32];
		//OSD owner ID

		//Use this field to capture OSD and prevent other applications from
		//using OSD when it is already in use by your application.
		//You should change this field only if it is empty (i.e. when OSD is
		//not owned by any application) or if it is set to your own application's
		//ID (i.e. when you own OSD)
		//You shouldn't change any OSD related feilds until you own OSD
	}

	// v1.2 memory structure
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct RTSS_SHARED_MEMORY_V_1_2
	{
        public DWORD dwSignature;
		//signature allows applications to verify status of shared memory

		//The signature can be set to:
		//'RTSS'	- statistics server's memory is initialized and contains 
		//			valid data 
		//0xDEAD	- statistics server's memory is marked for deallocation and
		//			no longer contain valid data
		//otherwise	the memory is not initialized
        public DWORD dwVersion;
		//structure version ((major<<16) + minor)
		//must be set to 0x00010002 for v1.2 structure
        public DWORD dwTime0;
		//start time of framerate measurement period (in milliseconds)

		//Take a note that this field must contain non-zero value to calculate 
		//framerate properly!
        public DWORD dwTime1;
		//end time of framerate measurement period (in milliseconds)
        public DWORD dwFrames;
		//amount of frames rendered during (dwTime1 - dwTime0) period

		//to calculate framerate use the following formula:
		//1000.0f * dwFrames / (dwTime1 - dwTime0)

        public DWORD dwOSDFlags;
		//bitmask, containing combination of OSDFLAG_... flags

		//Note: set OSDFLAG_UPDATED flag as soon as you change any OSD related
		//field
        public DWORD dwOSDX;
		//OSD X-coordinate (coordinate wrapping is allowed, i.e. -5 defines 5
		//pixel offset from the right side of the screen)
        public DWORD dwOSDY;
		//OSD Y-coordinate (coordinate wrapping is allowed, i.e. -5 defines 5
		//pixel offset from the bottom side of the screen)
        public DWORD dwOSDPixel;
		//OSD pixel zooming ratio
        public DWORD dwOSDColor;
		//OSD color in RGB format
        public fixed CHAR szOSD[256];
		//primary OSD slot text
        public fixed CHAR szOSDOwner[32];
		//primary OSD slot owner ID

		//Use this field to capture OSD slot and prevent other applications from
		//using OSD when it is already in use by your application.
		//You should change this field only if it is empty (i.e. when OSD slot is
		//not owned by any application) or if it is set to your own application's
		//ID (i.e. when you own OSD slot)
		//You shouldn't change any OSD related feilds until you own OSD slot

        public fixed CHAR szOSD1[256];
		//OSD slot 1 text
        public fixed CHAR szOSD1Owner[32];
		//OSD slot 1 owner ID
        public fixed CHAR szOSD2[256];
		//OSD slot 2 text
        public fixed CHAR szOSD2Owner[32];
		//OSD slot 2 owner ID
        public fixed CHAR szOSD3[256];
		//OSD slot 3 text
        public fixed CHAR szOSD3Owner[32];
		//OSD slot 3 owner ID
	}


	// v1.3 memory structure
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct RTSS_SHARED_MEMORY_V_1_3
	{
        public DWORD dwSignature;
		//signature allows applications to verify status of shared memory

		//The signature can be set to:
		//'RTSS'	- statistics server's memory is initialized and contains 
		//			valid data 
		//0xDEAD	- statistics server's memory is marked for deallocation and
		//			no longer contain valid data
		//otherwise	the memory is not initialized
        public DWORD dwVersion;
		//structure version ((major<<16) + minor)
		//must be set to 0x00010003 for v1.3 structure
        public DWORD dwTime0;
		//start time of framerate measurement period (in milliseconds)

		//Take a note that this field must contain non-zero value to calculate 
		//framerate properly!
        public DWORD dwTime1;
		//end time of framerate measurement period (in milliseconds)
        public DWORD dwFrames;
		//amount of frames rendered during (dwTime1 - dwTime0) period

		//to calculate framerate use the following formula:
		//1000.0f * dwFrames / (dwTime1 - dwTime0)

        public DWORD dwOSDFlags;
		//bitmask, containing combination of OSDFLAG_... flags

		//Note: set OSDFLAG_UPDATED flag as soon as you change any OSD related
		//field
        public DWORD dwOSDX;
		//OSD X-coordinate (coordinate wrapping is allowed, i.e. -5 defines 5
		//pixel offset from the right side of the screen)

        public DWORD dwOSDY;
		//OSD Y-coordinate (coordinate wrapping is allowed, i.e. -5 defines 5
		//pixel offset from the bottom side of the screen)

        public DWORD dwOSDPixel;
		//OSD pixel zooming ratio

        public DWORD dwOSDColor;
		//OSD color in RGB format

        public fixed CHAR szOSD[256];
		//primary OSD slot text

        public fixed CHAR szOSDOwner[32];
		//primary OSD slot owner ID

		//Use this field to capture OSD slot and prevent other applications from
		//using OSD when it is already in use by your application.
		//You should change this field only if it is empty (i.e. when OSD slot is
		//not owned by any application) or if it is set to your own application's
		//ID (i.e. when you own OSD slot)
		//You shouldn't change any OSD related feilds until you own OSD slot

        public fixed CHAR szOSD1[256];
		//OSD slot 1 text

        public fixed CHAR szOSD1Owner[32];
		//OSD slot 1 owner ID

        public fixed CHAR szOSD2[256];
		//OSD slot 2 text

        public fixed CHAR szOSD2Owner[32];
		//OSD slot 2 owner ID

        public fixed CHAR szOSD3[256];
		//OSD slot 3 text

        public fixed CHAR szOSD3Owner[32];
		//OSD slot 3 owner ID

        public DWORD dwStatFlags;
		//bitmask containing combination of STATFLAG_... flags

        public DWORD dwStatTime0;
		//statistics record period start time

        public DWORD dwStatTime1;
		//statistics record period end time

        public DWORD dwStatFrames;
		//total amount of frames rendered during statistics record period

        public DWORD dwStatCount;
		//amount of min/avg/max measurements during statistics record period 

        public DWORD dwStatFramerateMin;
		//minimum instantaneous framerate measured during statistics record period 

        public DWORD dwStatFramerateAvg;
		//average instantaneous framerate measured during statistics record period 

        public DWORD dwStatFramerateMax;
		//maximum instantaneous framerate measured during statistics record period 
	}

    enum APPFLAGS:DWORD
	{
        // WARNING! The following API usage flags are deprecated and valid in 2.9 
        // and older shared memory layout only

        APPFLAG_DEPRECATED_DD = 0x00000010,
		APPFLAG_DEPRECATED_D3D8 = 0x00000100,
		APPFLAG_DEPRECATED_D3D9 = 0x00001000,
		APPFLAG_DEPRECATED_D3D9EX = 0x00002000,
		APPFLAG_DEPRECATED_OGL = 0x00010000,
		APPFLAG_DEPRECATED_D3D10 = 0x00100000,
		APPFLAG_DEPRECATED_D3D11 = 0x01000000,
		APPFLAG_DEPRECATED_API_USAGE_MASK						=(APPFLAG_DD | APPFLAG_D3D8 | APPFLAG_D3D9 | APPFLAG_D3D9EX | APPFLAG_OGL | APPFLAG_D3D10  | APPFLAG_D3D11),

	// The following API usage flags are valid in 2.10 and newer shared memory 
	// layout only

        APPFLAG_OGL												=0x00000001, 
        APPFLAG_DD												=0x00000002,
        APPFLAG_D3D8											=0x00000003,
        APPFLAG_D3D9											=0x00000004,
        APPFLAG_D3D9EX											=0x00000005,
        APPFLAG_D3D10											=0x00000006,
        APPFLAG_D3D11											=0x00000007,
        APPFLAG_D3D12											=0x00000008,
        APPFLAG_D3D12AFR										=0x00000009,
        APPFLAG_VULKAN											=0x0000000A,

        APPFLAG_API_USAGE_MASK									=0x0000FFFF,

        APPFLAG_ARCHITECTURE_X64								=0x00010000,
        APPFLAG_ARCHITECTURE_UWP								=0x00020000,

        APPFLAG_PROFILE_UPDATE_REQUESTED						=0x10000000,
    }

    [Flags]
    public enum SCREENCAPTUREFLAGS : DWORD
    {
        /////////////////////////////////////////////////////////////////////////////
        SCREENCAPTUREFLAG_REQUEST_CAPTURE = 0x00000001,
        SCREENCAPTUREFLAG_REQUEST_CAPTURE_OSD = 0x00000010,
    }

    [Flags]
    public enum VIDEOCAPTUREFLAGS : DWORD
    {
    /////////////////////////////////////////////////////////////////////////////
        VIDEOCAPTUREFLAG_REQUEST_CAPTURE_START = 0x00000001,
        VIDEOCAPTUREFLAG_REQUEST_CAPTURE_PROGRESS				=0x00000002,
        VIDEOCAPTUREFLAG_REQUEST_CAPTURE_STOP					=0x00000004,
        VIDEOCAPTUREFLAG_REQUEST_CAPTURE_MASK					=0x00000007,
        VIDEOCAPTUREFLAG_REQUEST_CAPTURE_OSD					=0x00000010,

        VIDEOCAPTUREFLAG_INTERNAL_RESIZE						=0x00010000,
    }

    [Flags]
    public enum PROCESS_PERF_COUNTERLAGS : DWORD
    {
        /////////////////////////////////////////////////////////////////////////////
        PROCESS_PERF_COUNTER_ID_RAM_USAGE = 0x00000001,
        PROCESS_PERF_COUNTER_ID_D3DKMT_VRAM_USAGE_LOCAL = 0x00000100,

        PROCESS_PERF_COUNTER_ID_D3DKMT_VRAM_USAGE_SHARED = 0x00000101
        /////////////////////////////////////////////////////////////////////////////
    }

    // v2.0 memory structure
    [StructLayout(LayoutKind.Sequential)]
	public unsafe struct RTSS_SHARED_MEMORY
    {
        public const DWORD dwSignatureRTSS = 0x52545353;
        public const DWORD dwSignatureDEAD = 0xDEAD0000;

		public DWORD dwSignature;
		//signature allows applications to verify status of shared memory

		//The signature can be set to:
		//'RTSS'	- statistics server's memory is initialized and contains 
		//			valid data 
		//0xDEAD	- statistics server's memory is marked for deallocation and
		//			no longer contain valid data
		//otherwise	the memory is not initialized
        public DWORD dwVersion;
		//structure version ((major<<16) + minor)
		//must be set to 0x0002xxxx for v2.x structure 

        public DWORD dwAppEntrySize;
		//size of RTSS_SHARED_MEMORY_OSD_ENTRY for compatibility with future versions

        public DWORD dwAppArrOffset;
		//offset of arrOSD array for compatibility with future versions

        public DWORD dwAppArrSize;
		//size of arrOSD array for compatibility with future versions

        public DWORD dwOSDEntrySize;
		//size of RTSS_SHARED_MEMORY_APP_ENTRY for compatibility with future versions

        public DWORD dwOSDArrOffset;
		//offset of arrApp array for compatibility with future versions

        public DWORD dwOSDArrSize;
		//size of arrOSD array for compatibility with future versions

        public DWORD dwOSDFrame;
		//Global OSD frame ID. Increment it to force the server to update OSD for all currently active 3D
		//applications.

		//next fields are valid for v2.14 and newer shared memory format only

        public LONG dwBusy;
		//set bit 0 when you're writing to shared memory and reset it when done

		//WARNING: do not forget to reset it, otherwise you'll completely lock OSD updates for all clients


		//next fields are valid for v2.15 and newer shared memory format only

        public DWORD dwDesktopVideoCaptureFlags;
        public fixed DWORD dwDesktopVideoCaptureStat[5];
		//shared copy of desktop video capture flags and performance stats for 64-bit applications

		//next fields are valid for v2.16 and newer shared memory format only

        public DWORD dwLastForegroundApp;
		//last foreground application entry index

        public DWORD dwLastForegroundAppProcessID;
		//last foreground application process ID

		//next fields are valid for v2.18 and newer shared memory format only

        public DWORD dwProcessPerfCountersEntrySize;
		//size of RTSS_SHARED_MEMORY_PROCESS_PERF_COUNTER_ENTRY for compatibility with future versions

        public DWORD dwProcessPerfCountersArrOffset;
		//offset of arrPerfCounters array for compatibility with future versions (relative to application entry)

		//next fields are valid for v2.19 and newer shared memory format only

        public LARGE_INTEGER qwLatencyMarkerSetTimestamp;
        public LARGE_INTEGER qwLatencyMarkerResetTimestamp;

		//OSD slot descriptor structure

        public struct RTSS_SHARED_MEMORY_OSD_ENTRY
        {
            public const int szOSDCharsCount = 256;
            public fixed CHAR szOSD[256];
            //OSD slot text

            public fixed CHAR szOSDOwner[256];
            //OSD slot owner ID

            //next fields are valid for v2.7 and newer shared memory format only

            public const int szOSDExCharsCount = 4096;
            public fixed CHAR szOSDEx[szOSDExCharsCount];
            //extended OSD slot text

            //next fields are valid for v2.12 and newer shared memory format only

            public fixed BYTE buffer[262144];
            //OSD slot data buffer


            //next fields are valid for v2.20 and newer shared memory format only

            public fixed CHAR szOSDEx2[32768];
            //additional 32KB extended OSD slot text
        };

        //process performance counter structure

        [StructLayout(LayoutKind.Sequential)]
	public struct RTSS_SHARED_MEMORY_PROCESS_PERF_COUNTER_ENTRY
		{
            public DWORD dwID;
			//performance counter ID, PROCESS_PERF_COUNTER_ID_XXX

            public DWORD dwParam;
			//performance counter parameters specific to performance counter ID
			//PROCESS_PERF_COUNTER_ID_D3DKMT_VRAM_USAGE_LOCAL	: contains GPU location (PCI bus, device and function)
			//PROCESS_PERF_COUNTER_ID_D3DKMT_VRAM_USAGE_SHARED	: contains GPU location (PCI bus, device and function)

            public DWORD dwData;
			//performance counter data
		};

	//application descriptor structure

    [StructLayout(LayoutKind.Sequential)]
	public struct RTSS_SHARED_MEMORY_APP_ENTRY
		{
			//application identification related fields

            public DWORD dwProcessID;
			//process ID

            public fixed CHAR szName[Win32A.MAX_PATH];
			//process executable name

            public DWORD dwFlags;
			//application specific flags

			//instantaneous framerate related fields

            public DWORD dwTime0;
			//start time of framerate measurement period (in milliseconds)

			//Take a note that this field must contain non-zero value to calculate 
			//framerate properly!
            public DWORD dwTime1;
			//end time of framerate measurement period (in milliseconds)

            public DWORD dwFrames;
			//amount of frames rendered during (dwTime1 - dwTime0) period

            public DWORD dwFrameTime;
			//frame time (in microseconds)


			//to calculate framerate use the following formulas:

			//1000.0f * dwFrames / (dwTime1 - dwTime0) for framerate calculated once per second
			//or
			//1000000.0f / dwFrameTime for framerate calculated once per frame 

			//framerate statistics related fields

            public DWORD dwStatFlags;
			//bitmask containing combination of STATFLAG_... flags

            public DWORD dwStatTime0;
			//statistics record period start time

            public DWORD dwStatTime1;
			//statistics record period end time

            public DWORD dwStatFrames;
			//total amount of frames rendered during statistics record period

            public DWORD dwStatCount;
			//amount of min/avg/max measurements during statistics record period 

            public DWORD dwStatFramerateMin;
			//minimum instantaneous framerate measured during statistics record period 

            public DWORD dwStatFramerateAvg;
			//average instantaneous framerate measured during statistics record period 

            public DWORD dwStatFramerateMax;
			//maximum instantaneous framerate measured during statistics record period 

			//OSD related fields

            public DWORD dwOSDX;
			//OSD X-coordinate (coordinate wrapping is allowed, i.e. -5 defines 5
			//pixel offset from the right side of the screen)

            public DWORD dwOSDY;
			//OSD Y-coordinate (coordinate wrapping is allowed, i.e. -5 defines 5
			//pixel offset from the bottom side of the screen)

            public DWORD dwOSDPixel;
			//OSD pixel zooming ratio

            public DWORD dwOSDColor;
			//OSD color in RGB format

            public DWORD dwOSDFrame;
			//application specific OSD frame ID. Don't change it directly!

            public DWORD dwScreenCaptureFlags;
            public fixed CHAR szScreenCapturePath[Win32A.MAX_PATH];

			//next fields are valid for v2.1 and newer shared memory format only

            public DWORD dwOSDBgndColor;
			//OSD background color in RGB format

			//next fields are valid for v2.2 and newer shared memory format only

            public DWORD dwVideoCaptureFlags;
            public fixed CHAR szVideoCapturePath[Win32A.MAX_PATH];
            public DWORD dwVideoFramerate;
            public DWORD dwVideoFramesize;
            public DWORD dwVideoFormat;
            public DWORD dwVideoQuality;
            public DWORD dwVideoCaptureThreads;

            public DWORD dwScreenCaptureQuality;
            public DWORD dwScreenCaptureThreads;

			//next fields are valid for v2.3 and newer shared memory format only

            public DWORD dwAudioCaptureFlags;

			//next fields are valid for v2.4 and newer shared memory format only

            public DWORD dwVideoCaptureFlagsEx;

			//next fields are valid for v2.5 and newer shared memory format only

            public DWORD dwAudioCaptureFlags2;

            public DWORD dwStatFrameTimeMin;
            public DWORD dwStatFrameTimeAvg;
            public DWORD dwStatFrameTimeMax;
            public DWORD dwStatFrameTimeCount;

            public fixed DWORD dwStatFrameTimeBuf[1024];
            public DWORD dwStatFrameTimeBufPos;
            public DWORD dwStatFrameTimeBufFramerate;

			//next fields are valid for v2.6 and newer shared memory format only

            public LARGE_INTEGER qwAudioCapturePTTEventPush;
            public LARGE_INTEGER qwAudioCapturePTTEventRelease;

            public LARGE_INTEGER qwAudioCapturePTTEventPush2;
            public LARGE_INTEGER qwAudioCapturePTTEventRelease2;

			//next fields are valid for v2.8 and newer shared memory format only

            public DWORD dwPrerecordSizeLimit;
            public DWORD dwPrerecordTimeLimit;

			//next fields are valid for v2.13 and newer shared memory format only

            public LARGE_INTEGER qwStatTotalTime;
            public fixed DWORD dwStatFrameTimeLowBuf[1024];
            public DWORD dwStatFramerate1Dot0PercentLow;
            public DWORD dwStatFramerate0Dot1PercentLow;

			//next fields are valid for v2.17 and newer shared memory format only

            public DWORD dw1Dot0PercentLowBufPos;
            public DWORD dw0Dot1PercentLowBufPos;

			//next fields are valid for v2.18 and newer shared memory format only

            public DWORD dwProcessPerfCountersFlags;
            public DWORD dwProcessPerfCountersCount;
            public DWORD dwProcessPerfCountersSamplingPeriod;
            public DWORD dwProcessPerfCountersSamplingTime;
            public DWORD dwProcessPerfCountersTimestamp;

			//next fields are valid for v2.19 and newer shared memory format only

            public LARGE_INTEGER qwLatencyMarkerPresentTimestamp;

			//next fields are valid for v2.20 and newer shared memory format only

            public DWORD dwResolutionX;
            public DWORD dwResolutionY;

			//WARNING: next fields should never (!!!) be accessed directly, use the offsets to access them in order to provide 
			//compatibility with future versions

            public RTSS_SHARED_MEMORY_PROCESS_PERF_COUNTER_ITEMS256 arrPerfCounters;

			[StructLayout(LayoutKind.Sequential)]
            public struct RTSS_SHARED_MEMORY_PROCESS_PERF_COUNTER_ITEMS256
			{
				private readonly struct RTSS_SHARED_MEMORY_PROCESS_PERF_COUNTER_ITEMS32
				{
                    private readonly RTSS_SHARED_MEMORY_PROCESS_PERF_COUNTER_ENTRY
					i000, i001, i002, i003, i004, i005, i006, i007, i008, i009, i010, i011, i012, i013, i014, i015,
                    i016, i017, i018, i019, i020, i021, i022, i023, i024, i025, i026, i027, i028, i029, i030, i031;
                }

                private readonly RTSS_SHARED_MEMORY_PROCESS_PERF_COUNTER_ITEMS32
                    i000, i001, i002, i003, i004, i005, i006, i007;


                public RTSS_SHARED_MEMORY_PROCESS_PERF_COUNTER_ENTRY this[int i]
                {
                    get
                    {
                        fixed (void* pdata = &i000)
                        {
                            var p0Entry = (RTSS_SHARED_MEMORY_PROCESS_PERF_COUNTER_ENTRY*) pdata;
							RTSS_SHARED_MEMORY_PROCESS_PERF_COUNTER_ENTRY result = *(p0Entry + i * sizeof(RTSS_SHARED_MEMORY_PROCESS_PERF_COUNTER_ENTRY));
                            return result;
                        }
                    }
                    set
                    {
                        fixed (void* pdata = &i000)
                        {
                            var p0Entry = (RTSS_SHARED_MEMORY_PROCESS_PERF_COUNTER_ENTRY*)pdata;
                            *(p0Entry + i * sizeof(RTSS_SHARED_MEMORY_PROCESS_PERF_COUNTER_ENTRY)) = value;
                        }
                    }
                }
			}
		};

		//WARNING: next fields should never (!!!) be accessed directly, use the offsets to access them in order to provide 
		//compatibility with future versions

        public RTSS_SHARED_MEMORY_OSD_ENTRY_ITEMS arrOSD;
		//array of OSD slots

        public RTSS_SHARED_MEMORY_APP_ENTRY_ITEMS arrApp;
		//array of application descriptors

		//next fields are valid for v2.9 and newer shared memory format only

		//WARNING: due to design flaw there is no offset available for this field, so it must be calculated manually as
		//dwAppArrOffset + dwAppArrSize * dwAppEntrySize

        public VIDEO_CAPTURE_PARAM autoVideoCaptureParam;

        public RTSS_SHARED_MEMORY_OSD_ENTRY_ITEMS* OSD_ENTRY_ITEMS
        {
            get
            {
                fixed (void* p = &dwSignature)
                {
                    var bp = (byte*) p;
                    var pItems = (RTSS_SHARED_MEMORY.RTSS_SHARED_MEMORY_OSD_ENTRY_ITEMS*)
                        (bp + dwOSDArrOffset);
                    return pItems;
                }
            }
        }

        public RTSS_SHARED_MEMORY_APP_ENTRY_ITEMS* APP_ENTRY_ITEMS
        {
            get
            {
                fixed (void* p = &dwSignature)
                {
                    var bp = (byte*)p;
                    var pItems = (RTSS_SHARED_MEMORY.RTSS_SHARED_MEMORY_APP_ENTRY_ITEMS*)
                        (bp + dwAppArrOffset);
                    return pItems;
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
		public struct RTSS_SHARED_MEMORY_OSD_ENTRY_ITEMS
        {
            private RTSS_SHARED_MEMORY_OSD_ENTRY i0, i1, i2, i3, i4, i5, i6, i7;

            public RTSS_SHARED_MEMORY_OSD_ENTRY* this[int i]
            {
                get
                {
                    void* pointer;

					switch (i)
                    {
                        case 0: fixed (void* p = &i0) {  pointer = p;}; break;
                        case 1: fixed (void* p = &i1) {  pointer = p;}; break;
                        case 2: fixed (void* p = &i2) {  pointer = p;}; break;
                        case 3: fixed (void* p = &i3) {  pointer = p;}; break;
                        case 4: fixed (void* p = &i4) {  pointer = p;}; break;
                        case 5: fixed (void* p = &i5) {  pointer = p;}; break;
						case 6: fixed (void* p = &i6) { pointer = p; }; break;
						case 7: fixed (void* p = &i7) { pointer = p; }; break;
						default: throw new InvalidEnumArgumentException("i");
                    }

                    return (RTSS_SHARED_MEMORY_OSD_ENTRY*) pointer;
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
		public struct RTSS_SHARED_MEMORY_APP_ENTRY_ITEMS
        {
            private struct RTSS_SHARED_MEMORY_APP_ENTRY_ITEMS32
            {
                private RTSS_SHARED_MEMORY_APP_ENTRY
                    i000, i001, i002, i003, i004, i005, i006, i007, i008, i009, i010, i011, i012, i013, i014, i015,
                    i016, i017, i018, i019, i020, i021, i022, i023, i024, i025, i026, i027, i028, i029, i030, i031;
            }

            private RTSS_SHARED_MEMORY_APP_ENTRY_ITEMS32
                i000, i001, i002, i003, i004, i005, i006, i007;

            public RTSS_SHARED_MEMORY_APP_ENTRY* this[int i]
            {
                get
                {
                    fixed (void* pdata = &i000)
                    {
                        var penties = (byte*) pdata;
                        var result = (penties + i * sizeof(RTSS_SHARED_MEMORY_APP_ENTRY));
                        return (RTSS_SHARED_MEMORY_APP_ENTRY*) result;
                    }
                }
            }
		}
	}

    
    public struct RTSS_EMBEDDED_OBJECT
	{
        public const string RTSS_EMBEDDED_OBJECT_GRAPH_SIGNATURE						="GR00";
        public DWORD dwSignature;
		//embedded object signature

        public DWORD dwSize;
		//embedded object size in bytes

        public LONG dwWidth;
		//embedded object width in pixels (if positive) or in chars (if negative)

        public LONG dwHeight;
		//embedded object height in pixels (if positive) or in chars (if negative)

        public LONG dwMargin;
		//embedded object margin in pixels
	}




	[Flags]
    public enum RTSS_EMBEDDED_OBJECT_GRAPH_FLAGS
	{
	/////////////////////////////////////////////////////////////////////////////
        FILLED						=1,
        FRAMERATE					=2,
        FRAMETIME					=4,
        BAR							=8,
        BGND						=16,
        VERTICAL					=32,
        MIRRORED					=64,
        AUTOSCALE					=128,

        FRAMERATE_MIN				=256,
        FRAMERATE_AVG				=512,
        FRAMERATE_MAX				=1024,
        FRAMERATE_1DOT0_PERCENT_LOW	=2048,
        FRAMERATE_0DOT1_PERCENT_LOW	=4096
	/////////////////////////////////////////////////////////////////////////////
    }


    [StructLayout(LayoutKind.Sequential)]
	unsafe struct RTSS_EMBEDDED_OBJECT_GRAPH
	{
        public RTSS_EMBEDDED_OBJECT header;
		//embedded object header

        public DWORD dwFlags;
		//bitmask containing RTSS_EMBEDDED_OBJECT_GRAPH_FLAG_XXX flags

        public FLOAT fltMin;
		//graph mininum value

        public FLOAT fltMax;
		//graph maximum value

        public DWORD dwDataCount;
		//count of data samples in fltData array


        public fltDataItems fltData;
        //graph data samples array

		[StructLayout(LayoutKind.Sequential, Pack = 0)]
		public struct fltDataItems
        {
            private FLOAT fltData;
            //graph data samples zero item

			public FLOAT this[int i]
            {
                get
                {
					fixed(FLOAT* pfltData = &fltData)
                    {
                        FLOAT result = *(pfltData + i * sizeof(FLOAT));
                        return result;
					}
				}
                set
                {
                    fixed (FLOAT* pfltData = &fltData)
                    {
                        *(pfltData + i * sizeof(FLOAT)) = value;
                    }
                }
            }
		}
	}
}
