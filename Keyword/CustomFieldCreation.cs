namespace RegOnline.RegressionTest.Keyword
{
    public class CustomFieldCreation
    {
        private PageObject.Builder.CustomFieldDefine customFieldDefine = new PageObject.Builder.CustomFieldDefine("dialog");

        public void AddCustomField(DataCollection.CustomField field)
        {
            customFieldDefine.SelectByName();

            customFieldDefine.NameOnForm.Type(field.NameOnForm);
            customFieldDefine.FieldType_Click();
            customFieldDefine.CFType_Select(field.Type);

            customFieldDefine.SaveAndClose_Click();
        }
    }
}
