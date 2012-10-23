namespace RegOnline.RegressionTest.PageObject.Manager.Account
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using RegOnline.RegressionTest.WebElements;
    using RegOnline.RegressionTest.Utilities;

    public class Default : Window
    {
        public enum Tab
        {
            [CustomString("Account Infomation")]
            AccountInfo,

            [CustomString("Payment Gateways")]
            PaymentGateways
        }

        public PaymentGateways Tab_PaymentGateways = new PaymentGateways();

        public void SwitchTab(Tab tab)
        {
            Clickable linkToSwitchTab = new Clickable(
                string.Format("//div[@id='ctl00_ctl00_cphDialog_cpMgrMain_radTAccountTabs']//ul/li/a//span[text()='{0}']", CustomStringAttribute.GetCustomString(tab)), 
                UIUtility.LocateBy.XPath);

            linkToSwitchTab.WaitForDisplayAndClick();
            WaitForAJAX();
            WaitForLoad();
        }
    }
}
