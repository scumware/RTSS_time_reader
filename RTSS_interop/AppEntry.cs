using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RTSS_time_reader.RTSS_interop
{
    using DWORD = System.UInt32;

    public struct AppEntry
    {

        public uint ProcessId;
        public string Name;
        public AppFlags Flags;

        //instantaneous framerate fields
        public DateTime InstantaneousTimeStart;
        public DateTime InstantaneousTimeEnd;
        public DWORD InstantaneousFrames;
        public TimeSpan InstantaneousFrameTime;

        //framerate stats fields
        public StatFlags StatFlags;
        public DateTime StatTimeStart;
        public DateTime StatTimeEnd;
        public DWORD StatFrames;
        public DWORD StatCount;
        public DWORD StatFramerateMin;
        public DWORD StatFramerateAvg;
        public DWORD StatFramerateMax;

        //framerate stats 2.5+
        public DWORD StatFrameTimeMin;
        public DWORD StatFrameTimeAvg;
        public DWORD StatFrameTimeMax;
        public DWORD StatFrameTimeCount;
/*
        public unsafe fixed DWORD StatFrameTimeBuf[1024];
        public DWORD StatFrameTimeBufPos;
        public DWORD StatFrameTimeBufFramerate;
*/
        //OSD fields
        public uint OSDCoordinateX;
        public uint OSDCoordinateY;
        public DWORD OSDZoom;
        public Color OSDColor;
        public DWORD OSDFrameId;
        public Color OSDBackgroundColor; //2.1+

        //screenshot fields
        public SCREENCAPTUREFLAGS ScreenshotFlags;
        public string ScreenshotPath;
        public DWORD ScreenshotQuality; //2.2+
        public DWORD ScreenshotThreads; //2.2+

        //video capture fields - 2.2+
        public VideoCaptureFlags VideoCaptureFlags;
        public string VideoCapturePath;
        public DWORD VideoFramerate;
        public DWORD VideoFramesize;
        public DWORD VideoFormat;
        public DWORD VideoQuality;
        public DWORD VideoCaptureThreads;
        public DWORD VideoCaptureFlagsEx; //2.4+

        //audio capture fields
        public DWORD AudioCaptureFlags; //2.3+
        public DWORD AudioCaptureFlags2; //2.5+
        public Int64 AudioCapturePTTEventPush; //2.6+
        public Int64 AudioCapturePTTEventRelease; //2.6+
        public Int64 AudioCapturePTTEventPush2; //2.6+
        public Int64 AudioCapturePTTEventRelease2; //2.6+
    };
}
