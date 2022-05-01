using System;
using System.CodeDom;
using System.Runtime.CompilerServices;

namespace RTSS_time_reader
{
    [Flags]
    public enum PipeReaderStateEnum
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

    public struct PipeReaderState
    {
        private volatile PipeReaderStateEnum m_stateValue;

        public PipeReaderState(PipeReaderStateEnum p_stateValue)
        {
            m_stateValue = p_stateValue;
        }

        public static PipeReaderState None
        {
            get { return new PipeReaderState(PipeReaderStateEnum.None); }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PipeReaderStateEnum GetDifference(PipeReaderState another)
        {
            return m_stateValue ^ another.m_stateValue;
        }

        public PipeReaderStateEnum GetClearedFlags(PipeReaderState p_oldState)
        {
            return p_oldState.m_stateValue & ~m_stateValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ReadFlag(PipeReaderStateEnum p_flag)
        {
            return (m_stateValue & p_flag) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetFlag(PipeReaderStateEnum p_flag, bool p_value)
        {
            if (p_value)
                m_stateValue = m_stateValue | p_flag;
            else
                m_stateValue = m_stateValue & ~p_flag;
        }


        public bool IsStarted
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return ReadFlag(PipeReaderStateEnum.Starting) || ReadFlag(PipeReaderStateEnum.Started);
            }
        }

        public bool EnabledWritingFile
        {
            get { return ReadFlag(PipeReaderStateEnum.EnabledWritingFile); }
        }

        public bool IsPipeCreated
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ReadFlag(PipeReaderStateEnum.PipeCreated); }
        }

        public bool IsFileOpened
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ReadFlag(PipeReaderStateEnum.FileOpened); }
        }

        public bool IsFileOpening
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ReadFlag(PipeReaderStateEnum.FileOpening); }
        }

        public bool IsConnectionAccepted
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ReadFlag(PipeReaderStateEnum.ConnectionAccepted); }
        }

        public bool IsError
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ReadFlag(PipeReaderStateEnum.Error); }
        }

        public static bool operator ==(PipeReaderState p_a, PipeReaderState p_b) => p_a.m_stateValue == p_b.m_stateValue;

        public static bool operator !=(PipeReaderState p_a, PipeReaderState p_b) => !(p_a == p_b);
    }
}