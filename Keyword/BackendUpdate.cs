namespace RegOnline.RegressionTest.Keyword
{
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.WebElements;

    public class BackendUpdate
    {
        public void UpdateCustomField(Registrant reg)
        {
            if (reg.CustomField_Responses.Count != 0)
            {
                foreach (CustomFieldResponse resp in reg.CustomField_Responses)
                {
                    if (resp is AgendaResponse)
                    {
                        AgendaResponse respAgenda = resp as AgendaResponse;
                        PageObject.PageObjectProvider.Backend.AttendeeInfo.AgendaEdit_Click();
                        PageObject.PageObjectHelper.SelectTopWindow();

                        switch (respAgenda.AgendaItem.Type)
                        {
                            case DataCollection.EventData_Common.CustomFieldType.CheckBox:
                                {
                                    AgendaResponse_Checkbox ckresp = respAgenda as AgendaResponse_Checkbox;
                                    ((PageObject.PageObjectProvider.Backend.AgendaEdit.AgendaType(ckresp.AgendaItem)) as CheckBox).Set(ckresp.Checked.Value);
                                }
                                break;
                            default:
                                break;
                        }

                        PageObject.PageObjectProvider.Backend.AgendaEdit.SaveAndClose_Click();
                        PageObject.PageObjectHelper.SelectTopWindow();
                    }
                }
            }
        }
    }
}
