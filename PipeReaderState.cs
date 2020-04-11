using System;

namespace RTSS_time_reader
{
    [Flags]
    public enum PipeReaderState
    {
        None = 0
        ,
        Started = 1
        ,
        FileOpened = 2
        ,
        PipeCreated = 4
        ,
        ConnectionAccepted = 8
        ,
        Error = 16
        ,
        PipeIO = 32
    }
}