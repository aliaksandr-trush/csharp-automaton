namespace RegOnline.RegressionTest.Managers.Manager.Dashboard
{
    using RegOnline.RegressionTest.Utilities;
    //using RegOnline.RegressionTest.Fixtures;
    using RegOnline.RegressionTest.UIUtility;
using System;
using System.Reflection;

    public class CustomReportCreator : ManagerBase
    {
        public enum Tab
        {
            [StringValue("General")]
            General = 0,

            [StringValue("Fields")]
            Fields,

            [StringValue("Filters")]
            Filters,

            [StringValue("Sorting and Grouping")]
            SortingAndGrouping,

            [StringValue("Smart Link")]
            SmartLink
        }

        public enum FieldsCategory
        {
            Standard = 0,
            Custom,
            EventFeeAndAgenda,
            Merchandise
        }

        public enum EventFeeAndAgendaFieldsColumn
        {
            Discount,
            Amount,
            Taxes,
            Credit,
            Waitlist
        }

        public enum MoveActions
        {
            Up,
            Down
        }

        public enum FilterLogics
        {
            And,
            Or
        }

        public enum StandardFields
        {
            [StringValue("Full Name (First Last)")]
            FullName,

            [StringValue("Event Title")]
            EventTitle,

            [StringValue("Email")]
            Email,

            [StringValue("Address Line 1")]
            AddressLine1,

            [StringValue("Total Charge")]
            TotalCharge,

            [StringValue("Balance Due")]
            Balance,

            [StringValue("Registration Status")]
            RegStatus,

            [StringValue("Registration Type")]
            RegType
        }

        public enum CustomFilter
        {
            [CustomFilterValue("")]
            Blank,

            [CustomFilterValue("s_RegTypeDescription")]
            RegType,

            [CustomFilterValue("s_BalanceDue")]
            BalanceDue
        }

        public enum FilterRegStatus
        {
            [CustomString("All")]
            All,

            [CustomString("Confirmed")]
            Confirmed
        }

        public enum FilterBalance
        {
            [CustomString("Balance > 0")]
            Positive,

            [CustomString("Balance = 0")]
            Zero,

            [CustomString("Balance < 0")]
            Negative,

            [CustomString("All")]
            All
        }

        public enum FilterOperators
        {
            [CustomString("<")]
            LessThan,

            [CustomString("<>")]
            NotEqualTo,

            [CustomString("=")]
            EqualTo,

            [CustomString(">")]
            GreaterThan,

            [CustomString("Field Is Empty")]
            FieldIsEmpty,

            [CustomString("Field Is Not Empty")]
            FieldIsNotEmpty
        }

        public enum SortingFields
        {
            [CustomString("")]
            Blank,

            [CustomString("s_FirstLastName")]
            FirstLastName,

            [CustomString("s_RegTypeDescription")]
            RegType
        }

        public sealed class CustomFilterValueAttribute : Attribute
        {
            public string FilterValue { get; set; }

            public CustomFilterValueAttribute(string filterValue)
            {
                this.FilterValue = filterValue;
            }

            public static string GetFilterValue(CustomFilter filter)
            {
                Type type = filter.GetType();
                FieldInfo fi = type.GetField(filter.ToString());
                CustomFilterValueAttribute[] attrs = fi.GetCustomAttributes(typeof(CustomFilterValueAttribute), false) as CustomFilterValueAttribute[];
                return attrs[0].FilterValue;
            }
        }

        public void Apply()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("btnSave", LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        public void Cancel()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("btnCancel", LocateBy.Id);
            Utility.ThreadSleep(2);
            SelectBuilderWindow();
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void OpenCustomReportCreator()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("Add Custom Report", LocateBy.LinkText);
            Utility.ThreadSleep(2);
            UIUtilityProvider.UIHelper.SelectPopUpFrameByName("dialog");
        }

        public void ClickEditCustomReport(string reportName)
        {
            string editReportLocator = string.Format("//*[text()='{0}']/../..//*[@title='Edit report']/..", reportName);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(editReportLocator, LocateBy.XPath);
            Utility.ThreadSleep(2);
            UIUtilityProvider.UIHelper.SelectPopUpFrameByName("dialog");
        }

        public void SetName(string name)
        {
            UIUtilityProvider.UIHelper.Type("crGeneral_tbReportName", name, LocateBy.Id);
        }

        public void ChooseTab(Tab tab)
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(string.Format("Tabimg_M{0}", (int)tab), LocateBy.Id);
        }

        public void ChooseFieldsCategory(FieldsCategory category)
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(string.Format("crFields_radioListFields_{0}", (int)category), LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        public void VerifyFieldsCategorySelected(FieldsCategory category)
        {
            string categoryLocator = string.Format("//input[@id='crFields_radioListFields_{0}'][@checked='checked']", (int)category);
            UIUtilityProvider.UIHelper.VerifyElementPresent(categoryLocator, true, LocateBy.XPath);
        }

        public void AddField(string field)
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(string.Format("//select[@id='crFields_bFly_Migrated_listBoxAvailable']/*[text()='{0}']", field), LocateBy.XPath);
            this.MoveSelectedItemToCurrentChoices();
        }

        public void DeleteField(string field)
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(string.Format("//select[@id='crFields_bFly_Migrated_listBoxSelected']/*[text()='{0}']", field), LocateBy.XPath);
            this.MoveSelectedItemToUnChoosed();
        }

        public void MoveSelectedItemToCurrentChoices()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("crFields_bFly_addButton", LocateBy.Id);
        }

        public void MoveSelectedItemToUnChoosed()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("crFields_bFly_removeButton", LocateBy.Id);
        }

        public void MoveAllItemsToCurrentChoices()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("crFields_bFly_addAllButton", LocateBy.Id);
        }

        public void MoveAllItemsToUnChoosed()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("crFields_bFly_removeAllButton", LocateBy.Id);
        }

        public void AdjustCurrentChoicesOrder(StandardFields field, MoveActions action)
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(string.Format("//*[text()='{0}']", StringEnum.GetStringValue(field)), LocateBy.XPath);
            
            switch(action)
            {
                case MoveActions.Up:
                    UIUtilityProvider.UIHelper.WaitForDisplayAndClick("moveItemUp", LocateBy.Id);
                    break;
                case MoveActions.Down:
                    UIUtilityProvider.UIHelper.WaitForDisplayAndClick("moveItemDown", LocateBy.Id);
                    break;
                default:
                    break;
            }
        }

        public void TypeFiltersDate(string from, string to)
        {
            string fromLocator = "crFilters_datePicker_Migrated_dpStart_dateInput_text";
            string toLocator = "crFilters_datePicker_Migrated_dpEnd_dateInput_text";
            UIUtilityProvider.UIHelper.Type(fromLocator, from, LocateBy.Id);
            UIUtilityProvider.UIHelper.Type(toLocator, to, LocateBy.Id);
        }

        public void SelectFiltersRegType(string regType)
        {
            UIUtilityProvider.UIHelper.SelectWithText("crFilters_ddlRegTypes", regType, LocateBy.Id);
        }

        public void SelectFiltersRegStatus(FilterRegStatus status)
        {
            UIUtilityProvider.UIHelper.SelectWithText("crFilters_ddlRegStatus", CustomStringAttribute.GetCustomString(status), LocateBy.Id);
        }

        public void SelectFiltersBalance(FilterBalance balance)
        {
            UIUtilityProvider.UIHelper.SelectWithText("crFilters_ddlBalance", CustomStringAttribute.GetCustomString(balance), LocateBy.Id);
        }

        public void SelectFilterLogic(FilterLogics filterLogic)
        {
            switch(filterLogic)
            {
                case FilterLogics.And:
                    UIUtilityProvider.UIHelper.WaitForDisplayAndClick("crFilters_customFilters_radioAND", LocateBy.Id);
                    break;
                case FilterLogics.Or:
                    UIUtilityProvider.UIHelper.WaitForDisplayAndClick("crFilters_customFilters_radioOR", LocateBy.Id);
                    break;
                default:
                    UIUtilityProvider.UIHelper.FailTest("No such logic!");
                    break;
            }
        }

        public void SetCustomFilterOne(CustomFilter field, FilterOperators operatorOne, string value)
        {
            UIUtilityProvider.UIHelper.SelectWithValue("crFilters_customFilters_ddlFieldFilter_0", CustomFilterValueAttribute.GetFilterValue(field), LocateBy.Id);
            UIUtilityProvider.UIHelper.SelectWithValue("crFilters_customFilters_ddlFilterOperator_0", CustomStringAttribute.GetCustomString(operatorOne), LocateBy.Id);
            UIUtilityProvider.UIHelper.Type("crFilters_customFilters_tbValue_0", value, LocateBy.Id);
        }

        public void SetCustomFilterTwo(CustomFilter field, FilterOperators operatorTwo, string value)
        {
            UIUtilityProvider.UIHelper.SelectWithValue("crFilters_customFilters_ddlFieldFilter_1", CustomFilterValueAttribute.GetFilterValue(field), LocateBy.Id);
            UIUtilityProvider.UIHelper.SelectWithValue("crFilters_customFilters_ddlFilterOperator_1", CustomStringAttribute.GetCustomString(operatorTwo), LocateBy.Id);
            UIUtilityProvider.UIHelper.Type("crFilters_customFilters_tbValue_1", value, LocateBy.Id);
        }

        public void ClickAddCustomFilter()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("AddFilter2", LocateBy.Id);
        }

        public void ClickClearCustomFilters()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("ClearFilters2", LocateBy.Id);
        }

        public void VerifyThirdFilterPresents(bool presents)
        {
            if (presents == true)
                UIUtilityProvider.UIHelper.VerifyElementPresent("crFilters_customFilters_ddlFieldFilter_2", presents, LocateBy.Id);
            else
                NUnit.Framework.Assert.True(UIUtilityProvider.UIHelper.IsElementHidden("//*[@id='crFilters_customFilters_ddlFieldFilter_2']/../..", LocateBy.XPath));
        }

        public void SelectGroupBy(string groupBy)
        {
            UIUtilityProvider.UIHelper.SelectWithValue("crSorts_ddlGrouping", groupBy, LocateBy.Id);
        }

        public void SelectFirstSortBy(SortingFields sortBy)
        {
            UIUtilityProvider.UIHelper.SelectWithValue("crSorts_ddlSort1", CustomStringAttribute.GetCustomString(sortBy), LocateBy.Id);
        }

        public void SetEnableSmartLink(bool check)
        {
            UIUtilityProvider.UIHelper.SetCheckbox("crAdvanced_cbShareReport", check, LocateBy.Id);
        }

        public void TypeSmartLinkPassword(string password)
        {
            UIUtilityProvider.UIHelper.Type("crAdvanced_tbPassword", password, LocateBy.Id);
        }

        public void ClickViewSmartLink()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("crAdvanced_hlShareLink", LocateBy.Id);
        }

        public void SelectSmartLinkPopupWindow()
        {
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
            UIUtilityProvider.UIHelper.SelectWindowByName("ReportHome");
        }

        public void CloseSmartLinkPopupWindow()
        {
            SelectSmartLinkPopupWindow();
            UIUtilityProvider.UIHelper.CloseWindow();
            UIUtilityProvider.UIHelper.SelectOriginalWindow();
            UIUtilityProvider.UIHelper.SelectPopUpFrameByName("dialog");
        }

        public void TypeSmartLinkPasswordAndSubmit(string password)
        {
            UIUtilityProvider.UIHelper.Type("txtPassword", password, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("submit", LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        public void VerifySmartLinkNeedsPassword(bool needs)
        {
            UIUtilityProvider.UIHelper.VerifyElementPresent("txtPassword", needs, LocateBy.Id);
        }

        public void SetShownColumn(EventFeeAndAgendaFieldsColumn column, bool isShow)
        {
            switch (column)
            {
                case EventFeeAndAgendaFieldsColumn.Discount:
                    break;
                case EventFeeAndAgendaFieldsColumn.Amount:
                    break;
                case EventFeeAndAgendaFieldsColumn.Taxes:
                    UIUtilityProvider.UIHelper.SetCheckbox("crFields_cbTax", isShow, LocateBy.Id);
                    break;
                case EventFeeAndAgendaFieldsColumn.Credit:
                    break;
                case EventFeeAndAgendaFieldsColumn.Waitlist:
                    break;
                default:
                    break;
            }
        }
    }
}
