namespace SIM.Suppliers.Cube
{
    public class CubeBikeFeedDto
    {
        public string EANCode { get; set; }
        public string Scancode { get; set; } // Used in 2023
        public string ItemCode { get; set; }
        public string ModelCode { get; set; }
        public string MotherCode { get; set; }
        public string Brand { get; set; }
        public string ProductDescription { get; set; }
        public string Modelname { get; set; }
        public string SizeCode { get; set; }
        public string SizeDescription { get; set; }
        public string Wheelsize { get; set; }
        public string Color { get; set; }
        public string Type { get; set; }
        public string BasicDealerPriceUK { get; set; }
        public string BasicConsumerPriceUK { get; set; }
        public string Description { get; set; }
        public string DescriptionFrame { get; set; }
        public string Frame { get; set; }
        public string Fork { get; set; }
        public string Headset { get; set; }
        public string Stem { get; set; }
        public string Handlebar { get; set; }
        public string Grips { get; set; }
        public string RearDerailleur { get; set; }
        public string FrontDerailleur { get; set; }
        public string Shifters { get; set; }
        public string BrakeSystem { get; set; }
        public string BottomBracket { get; set; }
        public string Crankset { get; set; }
        public string CranksetChainringSizes { get; set; }
        public string Cassette { get; set; }
        public string NumberOfGears { get; set; }
        public string Chain { get; set; }
        public string Rims { get; set; }
        public string FrontHub { get; set; }
        public string RearHub { get; set; }
        public string Tires { get; set; }
        public string Pedals { get; set; }
        public string Saddle { get; set; }
        public string SeatPost { get; set; }
        public string SeatPostDropperYesNo { get; set; }
        public string SeatPostClamp { get; set; }
        public string NetWeightKg { get; set; }
        public string GrossWeightKg { get; set; }
        public string BoxLabelCode { get; set; }
        public string BoxLength { get; set; }
        public string BoxWidth { get; set; }
        public string BoxHeight { get; set; }
        public string COO { get; set; }




        // Image URL fields
        public string ImageURL { get; set; } // For 2025
        public string ImageLink { get; set; } // For 2023




        // Additional fields specific to 2023 and 2025
        public string ArticleCodeWithoutSize { get; set; } // Used in 2023
        public string ItemNumber { get; set; } // Used in 2025
        public string MaxRiderWeightKg { get; set; } // Used in 2025
        public string MaxBikeWeightKg { get; set; } // Used in 2025
        public string StepLength { get; set; } // Used in 2025
        public string RelatedItems { get; set; } // Used in 2025




        // Light-related fields
        public string FrontLight { get; set; }
        public string RearLight { get; set; }
        public string Kickstand { get; set; }
        public string Mudguard { get; set; }
        public string Bell { get; set; }
        public string Carrier { get; set; }



        // Wheels
        public string WheelsetSystemWheels { get; set; }
        public string FrontTire { get; set; }
        public string RearTire { get; set; }



        // Suspension and Hardware
        public string Shok { get; set; }
        public string ShokHardware { get; set; }
        public string IntegratedBarStem { get; set; }
        public string BarExtensions { get; set; }
        public string HandlebarTape { get; set; }



        // Drive and protection
        public string ChainGuide { get; set; }
        public string IntegratedShiftersBrakelevers { get; set; }
        public string BrakeLevers { get; set; }
        public string GearTeeth { get; set; }
        public string ChainProtection { get; set; }




        // Electric components
        public string Engine { get; set; }
        public string Battery { get; set; }
        public string Charger { get; set; }
        public string Display { get; set; }
    }


}
