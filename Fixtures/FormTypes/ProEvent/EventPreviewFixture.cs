namespace RegOnline.RegressionTest.Fixtures.FormTypes.ProEvent
{
    using System.Threading;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class EventPreviewFixture : FixtureBase
    {
        private const string EventName = "EventPreviewFixture";
        private const string EventConfirmationMessage = "The event is completed. You can close the window now.";

        private enum RegTypes
        {
            [StringValue("RegTypeAdminOnly")]
            RegTypeAdminOnly,

            [StringValue("RegTypeVisibleToAll")]
            RegTypeVisibleToAll,

            [StringValue("RegTypeInvisible")]
            RegTypeInvisible
        }

        private enum CFs
        {
            [StringValue("CFVisibleToAll")]
            CFVisibleToAll,

            [StringValue("CFAdminOnly")]
            CFAdminOnly
        }

        private enum Agendas
        {
            [StringValue("AgendaAdminOnly")]
            AgendaAdminOnly,

            [StringValue("AgendaVisibleToAll")]
            AgendaVisibleToAll
        }

        private enum Merchandises
        {
            [StringValue("MerchandiseVisible")]
            MerchandiseVisible,

            [StringValue("MerchandiseInvisible")]
            MerchandiseInvisible
        }

        private int eventId;
        private string eventSessionId;

        [Test]
        [Category(Priority.Three)]
        [Description("683")]
        public void PreviewEvent()
        {
            this.CreateEvent();

            ManagerSiteMgr.OpenEventDashboardUrl(this.eventId, this.eventSessionId);

            this.PreviewWithRegType();
            this.PreviewWithOutRegType();
        }

        private void CreateEvent()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder();
            if (ManagerSiteMgr.EventExists(EventName))
            {
                ManagerSiteMgr.DeleteEventByName(EventName);
            }            
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);
            this.eventId = BuilderMgr.GetEventId();

            //Set start page and add reg types
            BuilderMgr.SetStartPage(ManagerSiteManager.EventType.ProEvent, EventName);
            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName(StringEnum.GetStringValue(RegTypes.RegTypeAdminOnly));
            BuilderMgr.RegTypeMgr.SetVisibilities(false, true, null);
            BuilderMgr.RegTypeMgr.SaveAndClose();

            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName(StringEnum.GetStringValue(RegTypes.RegTypeVisibleToAll));
            BuilderMgr.RegTypeMgr.SetVisibilities(true, true, null);
            BuilderMgr.RegTypeMgr.SaveAndClose();

            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName(StringEnum.GetStringValue(RegTypes.RegTypeInvisible));
            BuilderMgr.RegTypeMgr.SetVisibilities(false, false, null);
            BuilderMgr.RegTypeMgr.SaveAndClose();

            BuilderMgr.Next();

            //Set personal infomation page
            BuilderMgr.ClickAddPICustomField();
            BuilderMgr.CFMgr.SetName(StringEnum.GetStringValue(CFs.CFVisibleToAll));
            BuilderMgr.CFMgr.SetVisibilityOption(true, CFManagerBase.VisibilityOption.Visible);
            BuilderMgr.CFMgr.SaveAndClose();

            BuilderMgr.ClickAddPICustomField();
            BuilderMgr.CFMgr.SetName(StringEnum.GetStringValue(CFs.CFAdminOnly));
            BuilderMgr.CFMgr.SetVisibilityOption(true, CFManagerBase.VisibilityOption.Admin);
            BuilderMgr.CFMgr.SaveAndClose();

            BuilderMgr.Next();

            //Set agenda
            BuilderMgr.ClickYesOnSplashPage();
            BuilderMgr.ClickAddAgendaItem();
            BuilderMgr.AGMgr.SetName(StringEnum.GetStringValue(Agendas.AgendaVisibleToAll));
            BuilderMgr.AGMgr.SetVisibilityOption(true, CFManagerBase.VisibilityOption.Visible);
            BuilderMgr.AGMgr.ClickSaveAndNew();
            Thread.Sleep(2000);

            BuilderMgr.AGMgr.SetName(StringEnum.GetStringValue(Agendas.AgendaAdminOnly));
            BuilderMgr.AGMgr.SetVisibilityOption(true, CFManagerBase.VisibilityOption.Admin);
            BuilderMgr.AGMgr.ClickSaveItem();
            WebDriverUtility.DefaultProvider.SelectTopWindow();

            BuilderMgr.Next();

            //Set lodging and travel
            BuilderMgr.ClickYesOnSplashPage();
            BuilderMgr.ClickAddHotel();
            BuilderMgr.HotelMgr.SelectHotelTemplate("different name");
            BuilderMgr.HotelMgr.SaveAndClose();
            BuilderMgr.SetHotelStandardFieldsVisibilityAndRequired(FormDetailManager.HotelStandardFields.RoomType, true, null);
            BuilderMgr.SetHotelStandardFieldsVisibilityAndRequired(FormDetailManager.HotelStandardFields.BedType, true, null);

            BuilderMgr.Next();

            //Set merchandise tiems
            BuilderMgr.ClickAddMerchandiseItem();
            BuilderMgr.MerchMgr.SetName(StringEnum.GetStringValue(Merchandises.MerchandiseVisible));
            BuilderMgr.MerchMgr.SetFixedPrice(10);
            BuilderMgr.MerchMgr.SetMerchVisibility(true);
            BuilderMgr.MerchMgr.SaveAndClose();

            BuilderMgr.ClickAddMerchandiseItem();
            BuilderMgr.MerchMgr.SetName(StringEnum.GetStringValue(Merchandises.MerchandiseInvisible));
            BuilderMgr.MerchMgr.SetFixedPrice(10);
            BuilderMgr.MerchMgr.SetMerchVisibility(false);
            BuilderMgr.MerchMgr.SaveAndClose();

            BuilderMgr.Next();

            //Set Payment method
            BuilderMgr.PaymentMethodMgr.AddPaymentMethod(ManagerBase.PaymentMethod.Check);
            BuilderMgr.PaymentMethodMgr.SetPaymentMethodVisibility(ManagerBase.PaymentMethod.Check, false, true, null);

            BuilderMgr.Next();

            //Set confrimation message
            BuilderMgr.AddConfirmationMessage(EventConfirmationMessage);

            BuilderMgr.SaveAndClose();
        }

        private void PreviewWithRegType()
        {
            ManagerSiteMgr.DashboardMgr.ClickPreviewForm();

            BuilderMgr.SetMobileViewMode(false);
            BuilderMgr.VerifyHasRegTypeWhenPreview(StringEnum.GetStringValue(RegTypes.RegTypeVisibleToAll), true);
            BuilderMgr.SelectRegTypeWhenPreview(StringEnum.GetStringValue(RegTypes.RegTypeVisibleToAll));
            BuilderMgr.Next();

            BuilderMgr.SetMobileViewMode(false);
            BuilderMgr.VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(CFs.CFVisibleToAll), true);
            BuilderMgr.VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(CFs.CFAdminOnly), false);
            BuilderMgr.Next();

            BuilderMgr.SetMobileViewMode(false);
            BuilderMgr.VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(Agendas.AgendaVisibleToAll), true);
            BuilderMgr.VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(Agendas.AgendaAdminOnly), false);
            BuilderMgr.Next();

            BuilderMgr.SetMobileViewMode(false);
            BuilderMgr.VerifyLodgingOptionsDisplayWhenPreview();
            BuilderMgr.SelectLodgingOpionWhenPreview(FormDetailManager.PreferLodging.Yes);
            BuilderMgr.Next();

            BuilderMgr.SetMobileViewMode(false);
            BuilderMgr.VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(Merchandises.MerchandiseVisible), true);
            BuilderMgr.VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(Merchandises.MerchandiseInvisible), false);
            BuilderMgr.Next();
            BuilderMgr.Next();

            BuilderMgr.SetMobileViewMode(false);
            BuilderMgr.VerifyCustomFieldPresentWhenPreview(EventConfirmationMessage, true);
            BuilderMgr.SaveAndClose();
        }

        private void PreviewWithOutRegType()
        {
            ManagerSiteMgr.DashboardMgr.ClickOption(Managers.Manager.Dashboard.DashboardManager.EventSetupFunction.EditRegForm);
            BuilderMgr.DeleteRegType(StringEnum.GetStringValue(RegTypes.RegTypeAdminOnly));
            BuilderMgr.DeleteRegType(StringEnum.GetStringValue(RegTypes.RegTypeVisibleToAll));
            BuilderMgr.DeleteRegType(StringEnum.GetStringValue(RegTypes.RegTypeInvisible));
            BuilderMgr.TogglePreviewAndEditMode();
            BuilderMgr.SetMobileViewMode(false);
            BuilderMgr.VerifyHasRegTypeWhenPreview(StringEnum.GetStringValue(RegTypes.RegTypeAdminOnly), false);
            BuilderMgr.VerifyHasRegTypeWhenPreview(StringEnum.GetStringValue(RegTypes.RegTypeVisibleToAll), false);
            BuilderMgr.VerifyHasRegTypeWhenPreview(StringEnum.GetStringValue(RegTypes.RegTypeInvisible), false);
            BuilderMgr.SelectBuilderWindow();
        }
    }
}
