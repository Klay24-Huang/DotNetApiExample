using Shared.Attributes;

namespace Shared.Helpers
{
    public static class TimeHelper
    {
        // 靜態設置時區
        private static string _timeZone = string.Empty;

        // 靜態初始化，設定時區
        public static void Initialize(string timeZone)
        {
            _timeZone = timeZone;
        }

        public static DateTimeOffset GetTimeByTimeZone()
        {
            if (string.IsNullOrEmpty(_timeZone))
            {
                throw new InvalidOperationException("TimeHelper not initialized. Please call Initialize with a valid time zone.");
            }

            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(_timeZone);
            var utcNow = DateTime.UtcNow;
            var timeWithOffset = TimeZoneInfo.ConvertTimeFromUtc(utcNow, timeZoneInfo);
            return new DateTimeOffset(timeWithOffset, timeZoneInfo.GetUtcOffset(utcNow));
        }
    }
}
