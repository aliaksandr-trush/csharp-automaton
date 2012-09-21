namespace RegOnline.RegressionTest.PageObject.Backend
{
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.WebElements;

    public class AgendaEdit : Window
    {
        public Clickable SaveAndClose = new Clickable("btnSaveAndClose", UIUtility.LocateBy.Id);

        public ElementBase AgendaType(AgendaItem agenda)
        {
            switch (agenda.Type)
            {
                case FormData.CustomFieldType.CheckBox:
                    return new CheckBox(string.Format("CF{0}", agenda.Id), UIUtility.LocateBy.Id);
                default:
                    return null;
            }
        }

        public void SaveAndClose_Click()
        {
            this.SaveAndClose.WaitForDisplay();
            this.SaveAndClose.Click();
            Utilities.Utility.ThreadSleep(3);
        }
    }
}
