namespace SIM.Suppliers.Whyte
{
    public class WhyteFeedDto
    {
        public string ModelName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public string EAN { get; set; } = string.Empty;
        public string ImageLocation1 { get; set; } = string.Empty;
        public string ImageLocation2 { get; set; } = string.Empty;
        public string ImageLocation3 { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public string SRP { get; set; } = string.Empty;
        public string ElectricBikeYESNO { get; set; } = string.Empty;
        public string Colour { get; set; } = string.Empty;
        public string Weight { get; set; } = string.Empty;

        #region Specifications
        public string Frame { get; set; } = string.Empty;
        public string Fork { get; set; } = string.Empty;
        public string RearShock { get; set; } = string.Empty;
        public string Headset { get; set; } = string.Empty;
        public string RearHub { get; set; } = string.Empty;
        public string FrontHub { get; set; } = string.Empty;
        public string Spokes { get; set; } = string.Empty;
        public string Rims { get; set; } = string.Empty;
        public string Tyre { get; set; } = string.Empty;
        public string ShiftLevers { get; set; } = string.Empty;
        public string FrontMech { get; set; } = string.Empty;
        public string RearMech { get; set; } = string.Empty;
        public string Cassette { get; set; } = string.Empty;
        public string Chain { get; set; } = string.Empty;
        public string Crankset { get; set; } = string.Empty;
        public string BottomBracket { get; set; } = string.Empty;
        public string Seatpost { get; set; } = string.Empty;
        public string Saddle { get; set; } = string.Empty;
        public string Handlebar { get; set; } = string.Empty;
        public string Stem { get; set; } = string.Empty;
        public string Grips { get; set; } = string.Empty;
        public string BrakesFront { get; set; } = string.Empty;
        public string BrakesRear { get; set; } = string.Empty;
        public string BrakesLevers { get; set; } = string.Empty;
        public string Pedals { get; set; } = string.Empty;
        public string eBikeMotor { get; set; } = string.Empty;
        public string eBikeDisplay { get; set; } = string.Empty;
        public string eBikeBattery { get; set; } = string.Empty;
        public string eBikeBatteryCharger { get; set; } = string.Empty;
        public string ShockStrokeAndSag { get; set; } = string.Empty;
        public string ReducerBushWidths { get; set; } = string.Empty;
        public string EyetoEyeLength { get; set; } = string.Empty;
        #endregion

        #region Geometry
        public string Reach { get; set; } = string.Empty;
        public string Stack { get; set; } = string.Empty;
        public string HeadAngle { get; set; } = string.Empty;
        public string SeatAngle { get; set; } = string.Empty;
        public string BottomBracketHeight { get; set; } = string.Empty;
        public string Wheelbase { get; set; } = string.Empty;
        public string RearCentre { get; set; } = string.Empty;
        public string StandoverHeight { get; set; } = string.Empty;
        public string SeatubeLength { get; set; } = string.Empty;
        public string HeadtubeLength { get; set; } = string.Empty;
        public string SizeGuide1 { get; set; } = string.Empty;
        public string SizeGuide2 { get; set; } = string.Empty;

        #endregion

        public List<Geometry> Geometries { get; set; }
        public List<Specification> Specifications { get; set; }
    }

    public class Geometry
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Unit { get; set; }
    }

    public class Specification
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
