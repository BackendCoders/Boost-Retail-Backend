namespace Boost.Retail.Domain.Enums
{
 
    public class AppConstants
    {
        #region Default Values
        public const string ALL_LOCATIONS = "00";
        public const string ALL_SUPPLIERS = "000000";
        public const string UNRAISED_ORDERNO = "0000000000";
        public const string DEFAULT_STOCKNO = "0000000000";
        public const string SYSTEM_STAFF_CODE = "00";
        #endregion
    }
       

    #region PRODUCTS

    public enum Genders
    {
        Unisex,
        Male,
        Female
    }
    public enum ClickCollect
    {
        InStoreAndHomeDelivery,
        InStoreOnly,
        HomeDeliveryOnly
    }
    public enum LeadTimes
    {
        Unknown = 0,
        NextDay = 1,
        SameDay = 2,
        Days3To5 = 3,
        Days7To10 = 4,
        Weeks2 = 5,
        Weeks3To4 = 6,
        OneMonth = 7,
        CallForAvailability = 8
    }

    public enum Seasons
    {
        All,
        Winter,
        Summer,
    }
    public enum AgeRange
    {
        Any,
        Adult,
        Child
    }
    public enum PrintLabels
    {
        Yes,
        No,
        One
    }

    public enum DeliveryOptions
    {
        Standard = 0,
        Free = 1,
        Premium = 2
    }
    #endregion

    public enum LabelPrintOption
    {
        Yes = 0,
        No = 1,
        One = 2
    }
    public enum StockItemStatus
    {
        InStock = 0,
        Layaway = 1,
        Sold = 2,
        Journal = 3
    }

    public enum PurchaseOrderReason
    {
        Default = 0,
        UNKNOWN_IMPORTED = 999,
        STOCK_ORDER = 1,
        CUSTOMER_ORDER = 2,
        AUTO_ORDER = 3,
        CANCELLED = 4
    }

    public enum PurchaseOrderStatus
    {
        Default = 0,
        Raised = 1,
        Recieved = 2,
        Cancelled = 3
    }

    public enum B2bFileType
    {
        PSV = 0,
        CSV = 1,
        TSV = 2
    }

    public enum OutstandingOrderType
    {
        All = 0,
        Normal = 1,
        Xmas = 2
    }

    public enum Current
    {
        All = 0,
        CurrentOnly = 1,
        NonCurrent = 2,
    }

    public enum TransactionStatus
    {
        PendingPayment = 0,
        Completed = 1,
        Returned = 2,
        PartiallyReturned = 3
    }

    public enum PaymentType
    {
        Pending = 0,
        Cash = 1,
        Card = 2,
        Cheque = 3,
        Credit = 4,
        Bank = 5,
        Other = 6,
    }
    public enum LayawayType
    {
        NONE = 0,
        WORKSHOP = 1,
        STORE = 2,
        WEB = 3,
        PO = 4
    }


}
