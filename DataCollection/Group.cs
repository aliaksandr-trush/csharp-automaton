namespace RegOnline.RegressionTest.DataCollection
{
    using System.Collections.Generic;

    public class Group
    {
        public Registrant Primary;
        public List<Registrant> Secondaries = new List<Registrant>();
    }
}
