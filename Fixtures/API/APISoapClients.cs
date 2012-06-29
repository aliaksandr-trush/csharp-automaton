namespace RegOnline.RegressionTest.Fixtures.API.RegOnlineGetEventsService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.ServiceModel;

    public partial class getEventsSoapClient : IDisposable
    {
        /// <summary>
        /// Properly disposes of the WCF client, taking exception handling 
        /// based on State into consideration.
        /// </summary>
        public void Dispose()
        {
            if (this.State == CommunicationState.Faulted)
            {
                this.Abort();
            }
            else
            {
                this.Close();
            }
        }
    }
}

//TODO: add other implementations of IDisposable for service clients here as needed