using System.Globalization;

namespace SIM.Suppliers
{
    public class ColorConverter
    {
        public static string GetStandardMultiColor(string[] colors)
        {
            var ti = CultureInfo.CurrentCulture.TextInfo;
            var tmp = new List<string>();
            
            foreach (var color in colors)
            {
                tmp.Add(ti.ToTitleCase(color));
            }

            return string.Join("/", tmp);
        }

        public static string GetStandardColor(string color)
        {
            var ti = CultureInfo.CurrentCulture.TextInfo;

            color = color.Replace(" and ", "/").Replace(" with ", "/");

            return ti.ToTitleCase(color);
        }
    }
}
