namespace Boost.Admin.Data
{
   

    public enum BrandType
    { 
        Cannondale = 0,
        Cube = 1,
        Frog = 2,
        Giant = 3,
        Haibike = 4,
        Lapierre = 5,
        Liv = 6,
        Marin = 7,
        Merida = 8,
        Orbea = 9,
        Raleigh = 10,
        Specialized = 11,
        Tern = 12,
        Trek = 13,
        Whyte = 14
    }

    public enum DataSupplier
    {
        Giant = 1,
        Trek = 2,
        Specialized = 3,
        Raleigh = 4,
        Whyte = 5,
        Madison = 6,
        Cube = 7,
        Sportline = 8
    }

    public enum GenderOrAgeGroup
    {
        None,
        Boys,
        Girls,
        Men,
        Women,
        Unisex,
    }

    public enum ImportType
    {
        All,
        Bikes,
        Accessories,
        Apparell
    }

    public enum ProductType
    {
        Unknown,
        Bike,
        Frame,
        Accessory,
        Clothing,
        AfterMarket,
        EBike,
        ServicePart
    }

    public enum GiantProductType
    {
        Both,
        Bike,
        Gear
    }

    public enum GiantDivisionCode
    {
        Giant,
        Liv,
        Cadex
    }

    public enum SupplierProductType
    {
        Both,
        Bike,
        Accessories
    }

    public enum ProductStatus
    {
        Active,
        Deferred,
        Hidden
    }
}
