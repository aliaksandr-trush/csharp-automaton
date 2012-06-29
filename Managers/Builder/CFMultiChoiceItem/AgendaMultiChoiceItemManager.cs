namespace RegOnline.RegressionTest.Managers.Builder
{
    //using RegOnline.RegressionTest.Fixtures;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.UIUtility;

    public class AgendaMultiChoiceItemManager : MultiChoiceItemManagerBase
    {
        private const string PriceTxtboxLocator = "ctl00_cphDialog_rntAmount";

        public override string FrameID
        {
            get
            {
                return "dialog";
            }
        }

        public override void SaveAndClose()
        {
            SelectThisFrame();
            UIUtilityProvider.UIHelper.ClickSaveAndClose();
            Utility.ThreadSleep(1);
            SelectBuilderWindow();
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public override void Cancel()
        {
            SelectThisFrame();
            UIUtilityProvider.UIHelper.ClickCancel();
            Utility.ThreadSleep(1);
            SelectBuilderWindow();
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void SetMultiChoiceItem(string name, double? price)
        {
            SetName(name);
            if (price != null)
            {
                this.SetPrice(price);
            }
        }

        public void SetPrice(double? price)
        {
            UIUtilityProvider.UIHelper.TypeRADNumericById(PriceTxtboxLocator, price);
        }
    }
}
