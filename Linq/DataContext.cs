namespace RegOnline.RegressionTest.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using RegOnline.RegressionTest.Configuration;

    public partial class ClientDataContext : System.Data.Linq.DataContext
    {
        public ClientDataContext() :
            base(ConfigReader.DefaultProvider.EnvironmentConfiguration.ClientDbConnection, mappingSource)
        {
            OnCreated();
        }
    }

    public partial class ROMasterDataContext : System.Data.Linq.DataContext
    {
        public ROMasterDataContext() :
            base(ConfigReader.DefaultProvider.EnvironmentConfiguration.ROMasterConnection, mappingSource)
        {
            OnCreated();
        }
    }
}
