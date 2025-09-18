namespace Boost.Retail.Domain
{
    public static class StringExtensions
    {
        /// <summary>  
        /// Truncates the string to the specified maximum length.  
        /// </summary>  
        /// <param name="value">The string to truncate.</param>  
        /// <param name="maxLength">The maximum length of the string.</param>  
        /// <returns>The truncated string if it exceeds the maximum length; otherwise, the original string.</returns>  
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
    }
}
