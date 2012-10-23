namespace RegOnline.RegressionTest.PageObject.Manager.Account
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using RegOnline.RegressionTest.WebElements;

    public class PaymentGateways : Window
    {
        private Clickable link_AddPaymentGateway = new Clickable("ctl00_ctl00_cphDialog_cpMgrMain_addPmtGateway", UIUtility.LocateBy.Id);
    }

    public class GatewayRow : Window
    {
        public Clickable Description;
        public Label Gateway;
        public Label Currency;
        public Label ModifiedBy;
        public Label LastModified;
        public Clickable VerifyCCProcessing;

        public GatewayRow(DataCollection.FormData.Gateway gateway, DataCollection.FormData.GatewayCategory category)
        {

        }
    }
}
