using System;

namespace RTSS_time_reader
{
    [Flags]
    public enum PipeReaderState
    {
        None = 0
        ,
        Starting            = 1 << 0
        ,
        Started             = 1 << 1
        ,
        FileOpening         = 1 << 2
        ,
        FileOpened          = 1 << 3
        ,
        PipeCreated         = 1 << 4
        ,
        ConnectionAccepted  = 1 << 5
        ,
        Error               = 1 << 6
        ,
        PipeIO              = 1 << 7
        ,
        EnabledWritingFile  = 1 << 8
    }
}