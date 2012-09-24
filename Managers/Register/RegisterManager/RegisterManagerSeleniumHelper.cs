namespace RegOnline.RegressionTest.Managers.Register
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using RegOnline.RegressionTest.DataAccess;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.UIUtility;

    public partial class RegisterManager : ManagerBase
    {
        #region Selenium helpers

        private void DoSeleniumActionForMerchandise(Fee fee, int quantity)
        {
            int merchandiseId = Convert.ToInt32(fee.Id);
            int merchandiseTypeId = Convert.ToInt32(fee.TypeId);
            double amount = Convert.ToDouble(fee.Amount);
            string descReport = Convert.ToString(fee.ReportDescription);
            string descForm = Convert.ToString(fee.Description);
            descReport = (String.IsNullOrEmpty(descReport) ? Convert.ToString(fee.Description) : descReport);
            string response = String.Empty;

            switch (merchandiseTypeId)
            {
                //normal
                case 1:
                    response = quantity.ToString();
                    SelectMerchandiseQuantityByName(descForm, quantity);
                    break;
                default://multiple choice
                    throw new Exception("Not yet implemetned");
            }

            this.FillMerchandiseResponseList(
                this.CurrentRegistrationId,
                merchandiseResponses,
                merchandiseId,
                descReport,
                response,
                amount.ToString());
        }

        private void DoSeleniumActionForCustomField(DataRow row)
        {
            int customFieldId = Convert.ToInt32(row["Id"]);
            int customFieldTypeId = Convert.ToInt32(row["TypeId"]);
            string description = Convert.ToString(row["ReportDescription"]);
            description = (String.IsNullOrEmpty(description) ? Convert.ToString(row["Description"]) : description);
            decimal amount = Convert.ToDecimal(row["Amount"]);

            int customFieldCategory = Convert.ToInt32(row["CategoryID"]);

            string response = String.Empty;
            bool requiresTracking = true;

            switch (customFieldTypeId)
            {
                case (int)CustomFieldManager.CustomFieldType.CheckBox:
                    //Click(customFieldId.ToString());
                    UIUtil.DefaultProvider.SetCheckbox(customFieldId.ToString(), true, LocateBy.Id);
                    this.WaitForConditionalLogic();
                    break;

                case (int)CustomFieldManager.CustomFieldType.Dropdown:
                    List<Custom_Field_List_Item> dropDownListItems = Fetch_CustomFieldListItems(CurrentEventId, customFieldId);

                    if (dropDownListItems.Count > 0)
                    {
                        response = Convert.ToString(dropDownListItems[0].Description);
                        amount = Convert.ToDecimal(dropDownListItems[0].Amount);
                        UIUtil.DefaultProvider.SelectWithText(customFieldId.ToString(), response, LocateBy.Id);
                        this.WaitForConditionalLogic();
                    }

                    break;

                case (int)CustomFieldManager.CustomFieldType.RadioButton:
                    List<Custom_Field_List_Item> radioListItems = Fetch_CustomFieldListItems(CurrentEventId, customFieldId);

                    if (radioListItems.Count > 0)
                    {
                        int listItemId = Convert.ToInt32(radioListItems[0].Id);
                        response = Convert.ToString(radioListItems[0].Description);
                        amount = Convert.ToDecimal(radioListItems[0].Amount);
                        UIUtil.DefaultProvider.WaitForDisplayAndClick(listItemId.ToString(), LocateBy.Id);
                        this.WaitForConditionalLogic();
                    }

                    break;

                case 1:
                    throw new Exception("Not yet implemented");

                case (int)CustomFieldManager.CustomFieldType.OneLineText:
                case (int)CustomFieldManager.CustomFieldType.Paragraph:
                    response = "some text " + CurrentTicks;
                    UIUtil.DefaultProvider.Type(customFieldId.ToString(), response, LocateBy.Id);
                    break;

                case (int)CustomFieldManager.CustomFieldType.ContinueButton:
                    throw new Exception("Not yet implemented");

                case (int)CustomFieldManager.CustomFieldType.Number:
                case 12:
                    throw new Exception("Not yet implemented");

                case (int)CustomFieldManager.CustomFieldType.Date:
                case 15:
                    throw new Exception("Not yet implemented");

                case (int)CustomFieldManager.CustomFieldType.Time:
                    throw new Exception("Not yet implemented");

                case (int)CustomFieldManager.CustomFieldType.AlwaysSelected:
                    //throw new Exception("Not yet implemented");
                    break;

                default://section header - do not need any selenium action.  TODO: check if these elements exist.
                    requiresTracking = false;
                    break;
            }

            if (requiresTracking)
            {
                this.FillCustomFieldResponseList(
                    this.CurrentRegistrationId,
                    this.customFieldResponses,
                    customFieldId,
                    description,
                    customFieldCategory,
                    response,
                    amount.ToString(),
                    null);
            }
        }

        private void DoSeleniumActionForCustomField(Custom_Field customField)
        {
            int customFieldId = customField.Id;
            ClientDataContext db = new ClientDataContext();
            Custom_Field_Type customFieldType = (from cft in db.Custom_Field_Types where cft.Id == customField.TypeId select cft).Single();
            string description = customField.ReportDescription;
            description = (String.IsNullOrEmpty(description) ? customField.Description : description);
            decimal amount = 0.0M;

            if (customField.Amount.HasValue)
            {
                amount = customField.Amount.Value;
            }

            List<Custom_Fields_Pricing> pricing = (from p in db.Custom_Fields_Pricings where p.cfId == customField.Id select p).ToList();

            if (pricing.Count != 0)
            {
                foreach (Custom_Fields_Pricing cfp in pricing)
                {
                    if (cfp.startDate != null)
                    {
                        if (cfp.startDate < DateTime.Now)
                        {
                            amount = cfp.amount.Value;
                        }
                    }

                    if (cfp.endDate != null)
                    {
                        if (cfp.endDate > DateTime.Now)
                        {
                            amount = cfp.amount.Value;
                        }
                    }
                }
            }

            int customFieldCategory = Convert.ToInt32(customField.CategoryID);

            string response = String.Empty;
            bool requiresTracking = true;

            switch (customFieldType.Id)
            {
                case (int)CustomFieldManager.CustomFieldType.CheckBox:
                    UIUtil.DefaultProvider.SetCheckbox(customFieldId.ToString(), true, LocateBy.Id);
                    this.WaitForConditionalLogic();
                    break;

                case (int)CustomFieldManager.CustomFieldType.Dropdown:
                    List<Custom_Field_List_Item> dropDownListItems = this.DataTool.GetCustomFieldListItems(this.CurrentEventId, customFieldId);

                    if (dropDownListItems.Count > 0)
                    {
                        response = dropDownListItems[0].Description;
                        amount = Convert.ToDecimal(dropDownListItems[0].Amount);
                        UIUtil.DefaultProvider.SelectWithText(customFieldId.ToString(), response, LocateBy.Id);
                        this.WaitForConditionalLogic();
                    }

                    break;

                case (int)CustomFieldManager.CustomFieldType.RadioButton:
                    List<Custom_Field_List_Item> radioListItems = this.DataTool.GetCustomFieldListItems(this.CurrentEventId, customFieldId);

                    if (radioListItems.Count > 0)
                    {
                        int listItemId = radioListItems[0].Id;
                        response = radioListItems[0].Description;
                        amount = Convert.ToDecimal(radioListItems[0].Amount);
                        UIUtil.DefaultProvider.WaitForDisplayAndClick(listItemId.ToString(), LocateBy.Id);
                        this.WaitForConditionalLogic();
                    }

                    break;

                case 1:
                    throw new Exception("Not yet implemented");

                case (int)CustomFieldManager.CustomFieldType.OneLineText:
                case (int)CustomFieldManager.CustomFieldType.Paragraph:
                    response = "some text " + this.CurrentTicks;
                    UIUtil.DefaultProvider.Type(customFieldId.ToString(), response, LocateBy.Id);
                    break;

                case (int)CustomFieldManager.CustomFieldType.ContinueButton:
                    throw new Exception("Not yet implemented");

                case (int)CustomFieldManager.CustomFieldType.Number:
                case 12:
                    throw new Exception("Not yet implemented");

                case (int)CustomFieldManager.CustomFieldType.Date:
                case 15:
                    throw new Exception("Not yet implemented");

                case (int)CustomFieldManager.CustomFieldType.Time:
                    throw new Exception("Not yet implemented");

                case (int)CustomFieldManager.CustomFieldType.AlwaysSelected:
                    //throw new Exception("Not yet implemented");
                    break;

                default://section header - do not need any selenium action.  TODO: check if these elements exist.
                    requiresTracking = false;
                    break;
            }

            if (requiresTracking)
            {
                this.FillCustomFieldResponseList(
                    this.CurrentRegistrationId,
                    this.customFieldResponses,
                    customFieldId,
                    description,
                    customFieldCategory,
                    response,
                    amount.ToString(),
                    null);
            }
        }

        public void EnterAgendaItemCode(int customFieldId, string code)
        {
            string customFieldPasswordFieldId = "dc" + customFieldId.ToString();

            if (UIUtil.DefaultProvider.IsElementPresent(customFieldPasswordFieldId, LocateBy.Id))
            {
                UIUtil.DefaultProvider.Type(customFieldPasswordFieldId, code, LocateBy.Id);
            }
        }

        public void WaitForConditionalLogic()
        {
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }
        #endregion
    }
}
