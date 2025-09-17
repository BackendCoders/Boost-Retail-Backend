using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Boost.Retail.Data.Models
{
    public class Inventory : BaseEntity
    {
        [Key]
        [Required]
        [DisplayName("Part Number")]
        [MinLength(5)]
        [StringLength(5, ErrorMessage = "Part Number can only be 5 characters.")]
        public string PartNumber { set; get; } = string.Empty;

        [DisplayName("Stock at Location 01")]
        [DefaultValue(0)]
        public int L01 { set; get; } = 0;

        [DisplayName("Stock at Location 02")]
        [DefaultValue(0)]
        public int L02 { set; get; } = 0;

        [DisplayName("Stock at Location 03")]
        [DefaultValue(0)]
        public int L03 { set; get; } = 0;

        [DisplayName("Stock at Location 04")]
        [DefaultValue(0)]
        public int L04 { set; get; } = 0;

        [DisplayName("Stock at Location 05")]
        [DefaultValue(0)]
        public int L05 { set; get; } = 0;

        [DisplayName("Stock at Location 06")]
        [DefaultValue(0)]
        public int L06 { set; get; } = 0;

        [DisplayName("Stock at Location 07")]
        [DefaultValue(0)]
        public int L07 { set; get; } = 0;

        [DisplayName("Stock at Location 08")]
        [DefaultValue(0)]
        public int L08 { set; get; } = 0;

        [DisplayName("Stock at Location 09")]
        [DefaultValue(0)]
        public int L09 { set; get; } = 0;

        [DisplayName("Stock at Location 10")]
        [DefaultValue(0)]
        public int L10 { set; get; } = 0;

        [DisplayName("Stock at Location 11")]
        [DefaultValue(0)]
        public int L11 { set; get; } = 0;

        [DisplayName("Stock at Location 12")]
        [DefaultValue(0)]
        public int L12 { set; get; } = 0;

        [DisplayName("Stock at Location 13")]
        [DefaultValue(0)]
        public int L13 { set; get; } = 0;

        [DisplayName("Stock at Location 14")]
        [DefaultValue(0)]
        public int L14 { set; get; } = 0;

        [DisplayName("Stock at Location 15")]
        [DefaultValue(0)]
        public int L15 { set; get; } = 0;

        [DisplayName("Stock at Location 16")]
        [DefaultValue(0)]
        public int L16 { set; get; } = 0;

        [DisplayName("Stock at Location 16")]
        [DefaultValue(0)]
        public int L17 { set; get; } = 0;

        [DisplayName("Stock at Location 18")]
        [DefaultValue(0)]
        public int L18 { set; get; } = 0;

        [DisplayName("Stock at Location 19")]
        [DefaultValue(0)]
        public int L19 { set; get; } = 0;

        [DisplayName("Stock at Location 20")]
        [DefaultValue(0)]
        public int L20 { set; get; } = 0;

        [DisplayName("Stock at Location 21")]
        [DefaultValue(0)]
        public int L21 { set; get; } = 0;

        [DisplayName("Stock at Location 22")]
        [DefaultValue(0)]
        public int L22 { set; get; } = 0;

        [DisplayName("Stock at Location 23")]
        [DefaultValue(0)]
        public int L23 { set; get; } = 0;

        [DisplayName("Stock at Location 24")]
        [DefaultValue(0)]
        public int L24 { set; get; } = 0;

        [DisplayName("Stock at Location 25")]
        [DefaultValue(0)]
        public int L25 { set; get; } = 0;

        [DisplayName("Stock at Location 26")]
        [DefaultValue(0)]
        public int L26 { set; get; } = 0;

        [DisplayName("Stock at Location 27")]
        [DefaultValue(0)]
        public int L27 { set; get; } = 0;

        [DisplayName("Stock at Location 28")]
        [DefaultValue(0)]
        public int L28 { set; get; } = 0;

        [DisplayName("Stock at Location 29")]
        [DefaultValue(0)]
        public int L29 { set; get; } = 0;

        [DisplayName("Stock at Location 30")]
        [DefaultValue(0)]
        public int L30 { set; get; } = 0;

        [NotMapped]
        [DisplayName("Total Stock (All Locations)")]
        public int TotalStock
        {
            get
            {
                return L01 + L02 + L03 + L04 + L05 + L06 + L07 + L08 + L09 + L10 + L11 + L12 + L13 + L14 + L15 +
                    L16 + L17 + L18 + L19 + L20 + L21 + L22 + L23 + L24 + L25 + L26 + L27 + L28 + L29 + L30;

            }
        }

        /// <summary>
        /// Gets the stock level for the given location
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public int GetStockLevel(string location)
        {
            if (location == "00")
                return TotalStock;
            else
                return (int)GetType().GetProperty($"L{location}").GetValue(this, null);
        }

        /// <summary>
        /// Sets the stock value to the given quantity
        /// </summary>
        /// <param name="location"></param>
        /// <param name="qty"></param>
        public void SetStockLevel(string location, int qty)
        {
            // get original value
            var val = GetStockLevel(location);
            val = val + qty;

            GetType().GetProperty($"L{location}")
                .SetValue(this, Convert.ChangeType(val, typeof(int)), null);
        }
    }
}
