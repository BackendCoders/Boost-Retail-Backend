namespace BoostRetail.Integrations.SConnect.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Formats a DateTimeOffset to ISO 8601 format with timezone offset: "yyyy-MM-ddTHH:mm:sszzz"
        /// Example: 2025-08-15T14:10:23+00:00
        /// </summary>
        public static string ToIso8601String(this DateTimeOffset dateTime)
        {
            return dateTime.ToString("yyyy-MM-ddTHH:mm:sszzz");
        }

        /// <summary>
        /// Converts a DateTime to DateTimeOffset (assumes UTC if unspecified) and formats to ISO 8601 with offset.
        /// </summary>
        public static string ToIso8601String(this DateTime dateTime)
        {
            var offset = dateTime.Kind switch
            {
                DateTimeKind.Utc => new DateTimeOffset(dateTime, TimeSpan.Zero),
                DateTimeKind.Local => new DateTimeOffset(dateTime),
                _ => new DateTimeOffset(DateTime.SpecifyKind(dateTime, DateTimeKind.Utc), TimeSpan.Zero)
            };

            return offset.ToIso8601String();
        }
    }
}
