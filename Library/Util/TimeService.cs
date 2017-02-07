using System;

namespace Library.Util
{
    public static class TimeService
    {
        private static readonly DateTime DummyDateTime = DateTime.MinValue;
        private static DateTime nextDateTime = DummyDateTime;
        public static DateTime Now
        {
            get
            {
                var dateTime = (nextDateTime == DummyDateTime) ? DateTime.Now : nextDateTime;
                nextDateTime = DummyDateTime;
                return dateTime;
            }
        }
        public static DateTime NextTime
        {
            set {
                nextDateTime = value;
            }
        }
    }
}
