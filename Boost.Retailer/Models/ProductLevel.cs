using System.ComponentModel.DataAnnotations;

namespace Boost.Retail.Data.Models
{
    public class ProductLevel : BaseEntity
    {
        public ProductLevel()
        {

        }

        [Key]
        public string PartNumber { get; set; }

        public int Min01 { get; set; }
        public int Max01 { get; set; }
        public bool Replenish01 { get; set; }

        public int Min02 { get; set; }
        public int Max02 { get; set; }
        public bool Replenish02 { get; set; }

        public int Min03 { get; set; }
        public int Max03 { get; set; }
        public bool Replenish03 { get; set; }

        public int Min04 { get; set; }
        public int Max04 { get; set; }
        public bool Replenish04 { get; set; }

        public int Min05 { get; set; }
        public int Max05 { get; set; }
        public bool Replenish05 { get; set; }

        public int Min06 { get; set; }
        public int Max06 { get; set; }
        public bool Replenish06 { get; set; }

        public int Min07 { get; set; }
        public int Max07 { get; set; }
        public bool Replenish07 { get; set; }

        public int Min08 { get; set; }
        public int Max08 { get; set; }
        public bool Replenish08 { get; set; }

        public int Min09 { get; set; }
        public int Max09 { get; set; }
        public bool Replenish09 { get; set; }

        public int Min10 { get; set; }
        public int Max10 { get; set; }
        public bool Replenish10 { get; set; }

        public int Min11 { get; set; }
        public int Max11 { get; set; }
        public bool Replenish11 { get; set; }

        public int Min12 { get; set; }
        public int Max12 { get; set; }
        public bool Replenish12 { get; set; }

        public int Min13 { get; set; }
        public int Max13 { get; set; }
        public bool Replenish13 { get; set; }

        public int Min14 { get; set; }
        public int Max14 { get; set; }
        public bool Replenish14 { get; set; }

        public int Min15 { get; set; }
        public int Max15 { get; set; }
        public bool Replenish15 { get; set; }

        public int Min16 { get; set; }
        public int Max16 { get; set; }
        public bool Replenish16 { get; set; }

        public int Min17 { get; set; }
        public int Max17 { get; set; }
        public bool Replenish17 { get; set; }

        public int Min18 { get; set; }
        public int Max18 { get; set; }
        public bool Replenish18 { get; set; }

        public int Min19 { get; set; }
        public int Max19 { get; set; }
        public bool Replenish19 { get; set; }

        public int Min20 { get; set; }
        public int Max20 { get; set; }
        public bool Replenish20 { get; set; }

        public int Min21 { get; set; }
        public int Max21 { get; set; }
        public bool Replenish21 { get; set; }

        public int Min22 { get; set; }
        public int Max22 { get; set; }
        public bool Replenish22 { get; set; }

        public int Min23 { get; set; }
        public int Max23 { get; set; }
        public bool Replenish23 { get; set; }

        public int Min24 { get; set; }
        public int Max24 { get; set; }
        public bool Replenish24 { get; set; }

        public int Min25 { get; set; }
        public int Max25 { get; set; }
        public bool Replenish25 { get; set; }

        public int Min26 { get; set; }
        public int Max26 { get; set; }
        public bool Replenish26 { get; set; }

        public int Min27 { get; set; }
        public int Max27 { get; set; }
        public bool Replenish27 { get; set; }

        public int Min28 { get; set; }
        public int Max28 { get; set; }
        public bool Replenish28 { get; set; }

        public int Min29 { get; set; }
        public int Max29 { get; set; }
        public bool Replenish29 { get; set; }

        public int Min30 { get; set; }
        public int Max30 { get; set; }
        public bool Replenish30 { get; set; }

        /// <summary>
        /// Returns the string that is needed to be appeneded to the product string to pass to CUPART.DLL
        /// </summary>
        /// <returns></returns>
        public string GetStringMinMax()
        {
            var tmp = string.Empty;
            tmp += $"|{Min01.ToString().PadLeft(4, '0')}|{Max01.ToString().PadLeft(4, '0')}";
            tmp += $"|{Min02.ToString().PadLeft(4, '0')}|{Max02.ToString().PadLeft(4, '0')}";
            tmp += $"|{Min03.ToString().PadLeft(4, '0')}|{Max03.ToString().PadLeft(4, '0')}";
            tmp += $"|{Min04.ToString().PadLeft(4, '0')}|{Max04.ToString().PadLeft(4, '0')}";
            tmp += $"|{Min05.ToString().PadLeft(4, '0')}|{Max05.ToString().PadLeft(4, '0')}";
            tmp += $"|{Min06.ToString().PadLeft(4, '0')}|{Max06.ToString().PadLeft(4, '0')}";
            tmp += $"|{Min07.ToString().PadLeft(4, '0')}|{Max07.ToString().PadLeft(4, '0')}";
            tmp += $"|{Min08.ToString().PadLeft(4, '0')}|{Max08.ToString().PadLeft(4, '0')}";
            tmp += $"|{Min09.ToString().PadLeft(4, '0')}|{Max09.ToString().PadLeft(4, '0')}";
            tmp += $"|{Min10.ToString().PadLeft(4, '0')}|{Max10.ToString().PadLeft(4, '0')}";
            tmp += $"|{Min11.ToString().PadLeft(4, '0')}|{Max11.ToString().PadLeft(4, '0')}";
            tmp += $"|{Min12.ToString().PadLeft(4, '0')}|{Max12.ToString().PadLeft(4, '0')}";
            tmp += $"|{Min13.ToString().PadLeft(4, '0')}|{Max13.ToString().PadLeft(4, '0')}";
            tmp += $"|{Min14.ToString().PadLeft(4, '0')}|{Max14.ToString().PadLeft(4, '0')}";
            tmp += $"|{Min15.ToString().PadLeft(4, '0')}|{Max15.ToString().PadLeft(4, '0')}";
            tmp += $"|{Min16.ToString().PadLeft(4, '0')}|{Max16.ToString().PadLeft(4, '0')}";
            tmp += $"|{Min17.ToString().PadLeft(4, '0')}|{Max17.ToString().PadLeft(4, '0')}";
            tmp += $"|{Min18.ToString().PadLeft(4, '0')}|{Max18.ToString().PadLeft(4, '0')}";
            tmp += $"|{Min19.ToString().PadLeft(4, '0')}|{Max19.ToString().PadLeft(4, '0')}";
            tmp += $"|{Min20.ToString().PadLeft(4, '0')}|{Max20.ToString().PadLeft(4, '0')}";
            tmp += $"|{Min21.ToString().PadLeft(4, '0')}|{Max21.ToString().PadLeft(4, '0')}";
            tmp += $"|{Min22.ToString().PadLeft(4, '0')}|{Max22.ToString().PadLeft(4, '0')}";
            tmp += $"|{Min23.ToString().PadLeft(4, '0')}|{Max23.ToString().PadLeft(4, '0')}";
            tmp += $"|{Min24.ToString().PadLeft(4, '0')}|{Max24.ToString().PadLeft(4, '0')}";
            tmp += $"|{Min25.ToString().PadLeft(4, '0')}|{Max25.ToString().PadLeft(4, '0')}";
            tmp += $"|{Min26.ToString().PadLeft(4, '0')}|{Max26.ToString().PadLeft(4, '0')}";
            tmp += $"|{Min27.ToString().PadLeft(4, '0')}|{Max27.ToString().PadLeft(4, '0')}";
            tmp += $"|{Min28.ToString().PadLeft(4, '0')}|{Max28.ToString().PadLeft(4, '0')}";
            tmp += $"|{Min29.ToString().PadLeft(4, '0')}|{Max29.ToString().PadLeft(4, '0')}";
            tmp += $"|{Min30.ToString().PadLeft(4, '0')}|{Max30.ToString().PadLeft(4, '0')}";

            return tmp;
        }

        public string GetStringReplenish()
        {
            var tmp2 = string.Empty;
            tmp2 += Replenish01 ? "|1" : "|0";
            tmp2 += Replenish02 ? "|1" : "|0";
            tmp2 += Replenish03 ? "|1" : "|0";
            tmp2 += Replenish04 ? "|1" : "|0";
            tmp2 += Replenish05 ? "|1" : "|0";
            tmp2 += Replenish06 ? "|1" : "|0";
            tmp2 += Replenish07 ? "|1" : "|0";
            tmp2 += Replenish08 ? "|1" : "|0";
            tmp2 += Replenish09 ? "|1" : "|0";
            tmp2 += Replenish10 ? "|1" : "|0";
            tmp2 += Replenish11 ? "|1" : "|0";
            tmp2 += Replenish12 ? "|1" : "|0";
            tmp2 += Replenish13 ? "|1" : "|0";
            tmp2 += Replenish14 ? "|1" : "|0";
            tmp2 += Replenish15 ? "|1" : "|0";
            tmp2 += Replenish16 ? "|1" : "|0";
            tmp2 += Replenish17 ? "|1" : "|0";
            tmp2 += Replenish18 ? "|1" : "|0";
            tmp2 += Replenish19 ? "|1" : "|0";
            tmp2 += Replenish20 ? "|1" : "|0";
            tmp2 += Replenish21 ? "|1" : "|0";
            tmp2 += Replenish22 ? "|1" : "|0";
            tmp2 += Replenish23 ? "|1" : "|0";
            tmp2 += Replenish24 ? "|1" : "|0";
            tmp2 += Replenish25 ? "|1" : "|0";
            tmp2 += Replenish26 ? "|1" : "|0";
            tmp2 += Replenish27 ? "|1" : "|0";
            tmp2 += Replenish28 ? "|1" : "|0";
            tmp2 += Replenish29 ? "|1" : "|0";
            tmp2 += Replenish30 ? "|1" : "|0";
            return tmp2;
        }

        public int TotalMin()
        {
            return Min01 + Min02 + Min03 + Min04 + Min05 + Min06 + Min07 + Min08 + Min09 + Min10 +
                Min11 + Min12 + Min13 + Min14 + Min15 + Min16 + Min17 + Min18 + Min19 + Min20 +
                Min21 + Min22 + Min23 + Min24 + Min25 + Min26 + Min27 + Min28 + Min29 + Min30;
        }
        public int TotalMax()
        {
            return Max01 + Max02 + Max03 + Max04 + Max05 + Max06 + Max07 + Max08 + Max09 + Max10 +
                Max11 + Max12 + Max13 + Max14 + Max15 + Max16 + Max17 + Max18 + Max19 + Max20 +
                Max21 + Max22 + Max23 + Max24 + Max25 + Max26 + Max27 + Max28 + Max29 + Max30;
        }


        public class MMX
        {
            public string Location { get; set; } = string.Empty;
            public int Min { get; set; }
            public int Max { get; set; }
            public bool Replenish { get; set; }
        }


    }
}
