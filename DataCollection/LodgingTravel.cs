namespace RegOnline.RegressionTest.DataCollection
{
    using System.Collections.Generic;

    public class Lodging
    {
        public List<Hotel> Hotels = new List<Hotel>();
        public List<LodgingStandardFields> StandardFields = new List<LodgingStandardFields>();
    }

    public class Hotel
    {
    }

    public class LodgingStandardFields
    {
        public FormData.LodgingStandardFields Field;
        public bool? Visible;
        public bool? Required;
    }
}
