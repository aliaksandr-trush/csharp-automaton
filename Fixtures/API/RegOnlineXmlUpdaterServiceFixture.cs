namespace RegOnline.RegressionTest.Fixtures.API
{
    using System;
    using System.Xml;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.Fixtures.API.RegOnlineXmlUpdaterService;
    using RegOnline.RegressionTest.Attributes;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class RegOnlineXmlUpdaterServiceFixture : APIFixtureBase
    {
        private const string EventName = "WebService - RegOnlineXmlUpdaterServiceFixture";
        private const string CFName = "CFCheckbox";
        private const string AgendaItemName = "AgendaCheckbox";
        private const double AgendaFee = 10;

        private XmlUpdaterSoapClient service;
        private int eventId;
        private int registerId;

        protected override Uri RemoteAddressUri { get; set; }

        public RegOnlineXmlUpdaterServiceFixture()
        {
            RequiresBrowser = true;

            this.RemoteAddressUri = new Uri(
                BaseUri,
                ConfigReader.DefaultProvider.WebServiceConfiguration[ConfigReader.WebServiceEnum.XmlUpdaterService].Url);

            this.service = new XmlUpdaterSoapClient(
                ConfigReader.DefaultProvider.WebServiceConfiguration[ConfigReader.WebServiceEnum.XmlUpdaterService].EndpointConfigName,
                RemoteAddressUri.ToString());
        }

        [Test]
        [Category(Priority.Three)]
        [Description("863")]
        public void UpdateRegistrations()
        {
            this.PrepareEvent();
            this.CreateRegistration();

            string response = this.service.UpdateRegistrations(
                ConfigReader.DefaultProvider.AccountConfiguration.Login,
                ConfigReader.DefaultProvider.AccountConfiguration.Password, 
                this.eventId, 
                this.GetXmlData());

            Assert.IsTrue(response.Contains(this.registerId.ToString()));
        }

        private string GetXmlData()
        {
            string xmlFormat = @"
                            <updateData>
                                <registration id='{0}'>
                                    <agendaitems>
                                        <{1} value='True' quantity='1' />
                                    </agendaitems>
                                </registration>
                            </updateData>";

            string xml = string.Format(xmlFormat, this.registerId, AgendaItemName);

            return xml;
        }

        private string GetXml()
        {
            XmlDocument doc = new XmlDocument();

            XmlElement fees = doc.CreateElement("fees");

            XmlElement customfields = doc.CreateElement("customfields");

            XmlElement agendaitem = doc.CreateElement(AgendaItemName);
            agendaitem.SetAttribute("value", "a");
            agendaitem.SetAttribute("quantity", "1");

            XmlElement agendaitems = doc.CreateElement("agendaitems");
            agendaitems.AppendChild(agendaitem);

            XmlElement registration = doc.CreateElement("registration");
            registration.SetAttribute("id", this.registerId.ToString());
            registration.AppendChild(fees);
            registration.AppendChild(customfields);
            registration.AppendChild(agendaitems);

            XmlElement updateData = doc.CreateElement("updateData");
            updateData.AppendChild(registration);
            doc.AppendChild(updateData);

            return doc.InnerXml;
        }

        [Step]
        private void PrepareEvent()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.SelectFolder();

            ManagerSiteMgr.DeleteExpiredDuplicateEvents(EventName);

            if (!ManagerSiteMgr.EventExists(EventName))
            {
                this.AddEvent();
            }
            else
            {
                this.eventId = ManagerSiteMgr.GetFirstEventId(EventName);
            }
        }

        private void AddEvent()
        {
            ManagerSiteMgr.ClickAddEvent(Managers.Manager.ManagerSiteManager.EventType.ProEvent);
            BuilderMgr.SetEventNameAndShortcut(EventName);
            BuilderMgr.GotoPage(Managers.Builder.FormDetailManager.Page.PI);
            BuilderMgr.AddPICustomField(Managers.Builder.CustomFieldManager.CustomFieldType.CheckBox, CFName);
            BuilderMgr.GotoPage(Managers.Builder.FormDetailManager.Page.Agenda);
            BuilderMgr.ClickYesOnSplashPage();
            BuilderMgr.AddAgendaItemWithPriceAndNoDate(Managers.Builder.AgendaItemManager.AgendaItemType.CheckBox, AgendaItemName, null);
            BuilderMgr.GotoPage(Managers.Builder.FormDetailManager.Page.Checkout);
            BuilderMgr.EnterEventCheckoutPage();
            this.eventId = BuilderMgr.GetEventId();
            BuilderMgr.SaveAndClose();
        }

        [Step]
        private void CreateRegistration()
        {
            RegisterMgr.OpenRegisterPage(this.eventId);
            RegisterMgr.Checkin();
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.SetCustomFieldCheckBox(CFName, true);
            RegisterMgr.Continue();
            RegisterMgr.SetCustomFieldCheckBox(AgendaItemName, true);
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
            this.registerId = RegisterMgr.GetRegistrationIdOnConfirmationPage();
        }
    }
}