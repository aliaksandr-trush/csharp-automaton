namespace RegOnline.RegressionTest.Keyword
{
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.WebElements;

    public class BackendUpdate
    {
        PageObject.Backend.AttendeeInfo attendeeInfo = new PageObject.Backend.AttendeeInfo();
        PageObject.Backend.AgendaEdit agendaEdit = new PageObject.Backend.AgendaEdit();

        public void UpdateCustomField(Registrant reg)
        {
            if (reg.CustomField_Responses.Count != 0)
            {
                foreach (CustomFieldResponse resp in reg.CustomField_Responses)
                {
                    if (resp is AgendaResponse)
                    {
                        AgendaResponse respAgenda = resp as AgendaResponse;
                        attendeeInfo.AgendaEdit_Click();
                        PageObject.PageObjectHelper.SelectTopWindow();

                        switch (respAgenda.AgendaItem.Type)
                        {
                            case FormData.CustomFieldType.CheckBox:
                                {
                                    AgendaResponse_Checkbox ckresp = respAgenda as AgendaResponse_Checkbox;
                                    ((agendaEdit.AgendaType(ckresp.AgendaItem)) as CheckBox).Set(ckresp.Checked.Value);
                                }
                                break;
                            default:
                                break;
                        }

                        agendaEdit.SaveAndClose_Click();
                        PageObject.PageObjectHelper.SelectTopWindow();
                    }
                }
            }
        }
    }
}
