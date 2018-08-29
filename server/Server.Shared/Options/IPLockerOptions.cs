using System;

namespace Server.Shared.Options
{
    // ReSharper disable once InconsistentNaming
    public class IPLockerOptions
    {
        public TimeSpan LockedTime { get; set; }
        public TimeSpan LimitTime { get; set; }
        public int MaxVisitsTimes { get; set; }
    }
}
