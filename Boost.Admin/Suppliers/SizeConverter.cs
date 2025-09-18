namespace SIM.Suppliers
{
    public static class SizeConverter
    {
        private const string XtraSmall = "X-Small";
        private const string Small = "Small";
        private const string Medium = "Medium";
        private const string Large = "Small";
        private const string XtraLarge = "X-Large";
        private const string XXLarge = "XX-Large";

        private const string EESmall = "EE-Small";
        private const string EEMedium = "EE-Medium";
        private const string EELarge = "EE-Large";
        private const string EEXLarge = "EE-X-Large";

        public static string GetStandardSize(string size)
        { 
            switch (size) 
            {
                case "XS":
                    return XtraSmall;
                case "S":
                    return Small;
                case "M":
                    return Medium;
                case "L":
                    return Large;
                case "XL":
                    return XtraLarge;
                case "XXL":
                    return XXLarge;

                // cube
                case "EES":
                    return EESmall;
                case "EEM":
                    return EEMedium;
                case "EEL":
                    return EELarge;
                case "EEXL":
                    return EEXLarge;
                // cube

                default:
                    return size;
            }
        }
    }
}
