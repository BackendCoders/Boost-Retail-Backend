using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Boost.Retail.Data.Models
{
    public class Customer : BaseEntity
    {

        public Customer()
        {
            AccNo = string.Empty;
            Title = "";
            Initials = "";
            Surname = "";
            Firstname = "";
            HouseName = "";
            Address1 = "";
            Address2 = "";
            Address3 = "";
            Address4 = "";

            Postcode = "";
            Country = "UNITED KINGDOM";
            Telephone = "";
            Mobile = "";
            Email = "";
            LoyaltyNo = "";


            Category = "A";


            VATNumber = "";
            Country = "";


            DeliveryTitle = "";
            DeliveryInitials = "";
            DeliverySurname = "";
            DeliveryFirstname = "";
            DeliveryHousename = "";
            DeliveryAddress1 = "";
            DeliveryAddress2 = "";
            DeliveryAddress3 = "";
            DeliveryAddress4 = "";
            DeliveryPostcode = "";
            DeliveryCountry = "";

            WorkPhone = "";
            DOB = new DateTime(2000, 1, 1);
            Notes = "";

            CreditLimit = 99999;
            SendLetter = true;
            Current = true;
            Location = "";
        }


        [Key]
        public string AccNo { get; set; }
        public string Title { get; set; }
        public string Initials { get; set; }
        public string Surname { get; set; }
        public string Firstname { get; set; }
        public string HouseName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }

        public string Postcode { get; set; }
        public string Telephone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string LoyaltyNo { get; set; }
        public decimal Balance { get; set; }


       
        public bool Stop { get; set; }
        public bool SendLetter { get; set; }
        public decimal CreditLimit { get; set; }
        public bool Export { get; set; }
        public bool VATExempt { get; set; }
        public string Category { get; set; }

        public bool Autopay { get; set; }
        public string VATNumber { get; set; }
        public string Country { get; set; }

        public decimal Contract1 { get; set; }
        public decimal Contract2 { get; set; }
        public decimal Contract3 { get; set; }

        public string DeliveryTitle { get; set; }
        public string DeliveryInitials { get; set; }
        public string DeliverySurname { get; set; }
        public string DeliveryFirstname { get; set; }
        public string DeliveryHousename { get; set; }
        public string DeliveryAddress1 { get; set; }
        public string DeliveryAddress2 { get; set; }
        public string DeliveryAddress3 { get; set; }
        public string DeliveryAddress4 { get; set; }
        public string DeliveryPostcode { get; set; }
        public string DeliveryCountry { get; set; }

        public string WorkPhone { get; set; }
        public DateTime DOB { get; set; }
        public decimal DiscountMinor { get; set; }
        public decimal DiscountMajor { get; set; }
        public bool TradeCustomer { get; set; }
        public string Notes { get; set; }
        public string Location { get; set; }
        public bool Current { get; set; }



        // field removed from sql
        [NotMapped]
        public string Address
        {
            get
            {
                var address = string.Empty;
                if (!string.IsNullOrEmpty(Address1))
                {
                    address = Address1;
                }
                if (!string.IsNullOrEmpty(Address2))
                {
                    if (string.IsNullOrEmpty(address))
                    {
                        address = Address2;
                    }
                    else
                        address = address + ", " + Address2;
                }
                if (!string.IsNullOrEmpty(Address3))
                {
                    if (string.IsNullOrEmpty(address))
                    {
                        address = Address3;
                    }
                    else
                        address = address + ", " + Address3;
                }
                if (!string.IsNullOrEmpty(Address4))
                {
                    if (string.IsNullOrEmpty(address))
                    {
                        address = Address4;
                    }
                    else
                        address = address + ", " + Address4;
                }

                return address;
            }
        }


        [NotMapped]
        public string DeliveryAddress
        {
            get
            {
                var address = string.Empty;
                if (!string.IsNullOrEmpty(DeliveryAddress1))
                {
                    address = DeliveryAddress1;
                }
                if (!string.IsNullOrEmpty(DeliveryAddress2))
                {
                    if (string.IsNullOrEmpty(address))
                    {
                        address = DeliveryAddress2;
                    }
                    else
                        address = address + ", " + DeliveryAddress2;
                }
                if (!string.IsNullOrEmpty(DeliveryAddress3))
                {
                    if (string.IsNullOrEmpty(address))
                    {
                        address = DeliveryAddress3;
                    }
                    else
                        address = address + ", " + DeliveryAddress3;
                }
                if (!string.IsNullOrEmpty(DeliveryAddress4))
                {
                    if (string.IsNullOrEmpty(address))
                    {
                        address = DeliveryAddress4;
                    }
                    else
                        address = address + ", " + DeliveryAddress4;
                }

                return address;
            }
        }
        
    }
}
