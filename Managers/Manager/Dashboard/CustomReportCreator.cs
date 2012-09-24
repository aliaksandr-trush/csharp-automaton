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
            UIUtil.DefaultProvider.WaitForDisplayAndClick("btnSave", LocateBy.Id);
            UIUtil.DefaultProvider.WaitForPageToLoad();
        }

        public void Cancel()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick("btnCancel", LocateBy.Id);
            Utility.ThreadSleep(2);
            SelectBuilderWindow();
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }

        public void OpenCustomReportCreator()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick("Add Custom Report", LocateBy.LinkText);
            Utility.ThreadSleep(2);
            UIUtil.DefaultProvider.SelectPopUpFrameByName("dialog");
        }

        public void ClickEditCustomReport(string reportName)
        {
            string editReportLocator = string.Format("//*[text()='{0}']/../..//*[@title='Edit report']/..", reportName);
            UIUtil.DefaultProvider.WaitForDisplayAndClick(editReportLocator, LocateBy.XPath);
            Utility.ThreadSleep(2);
            UIUtil.DefaultProvider.SelectPopUpFrameByName("dialog");
        }

        public void SetName(string name)
        {
            UIUtil.DefaultProvider.Type("crGeneral_tbReportName", name, LocateBy.Id);
        }

        public void ChooseTab(Tab tab)
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(string.Format("Tabimg_M{0}", (int)tab), LocateBy.Id);
        }

        public void ChooseFieldsCategory(FieldsCategory category)
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(string.Format("crFields_radioListFields_{0}", (int)category), LocateBy.Id);
            UIUtil.DefaultProvider.WaitForPageToLoad();
        }

        public void VerifyFieldsCategorySelected(FieldsCategory category)
        {
            string categoryLocator = string.Format("//input[@id='crFields_radioListFields_{0}'][@checked='checked']", (int)category);
            UIUtil.DefaultProvider.VerifyElementPresent(categoryLocator, true, LocateBy.XPath);
        }

        public void AddField(string field)
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(string.Format("//select[@id='crFields_bFly_Migrated_listBoxAvailable']/*[text()='{0}']", field), LocateBy.XPath);
            this.MoveSelectedItemToCurrentChoices();
        }

        public void DeleteField(string field)
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(string.Format("//select[@id='crFields_bFly_Migrated_listBoxSelected']/*[text()='{0}']", field), LocateBy.XPath);
            this.MoveSelectedItemToUnChoosed();
        }

        public void MoveSelectedItemToCurrentChoices()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick("crFields_bFly_addButton", LocateBy.Id);
        }

        public void MoveSelectedItemToUnChoosed()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick("crFields_bFly_removeButton", LocateBy.Id);
        }

        public void MoveAllItemsToCurrentChoices()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick("crFields_bFly_addAllButton", LocateBy.Id);
        }

        public void MoveAllItemsToUnChoosed()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick("crFields_bFly_removeAllButton", LocateBy.Id);
        }

        public void AdjustCurrentChoicesOrder(StandardFields field, MoveActions action)
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(string.Format("//*[text()='{0}']", StringEnum.GetStringValue(field)), LocateBy.XPath);
            
            switch(action)
            {
                case MoveActions.Up:
                    UIUtil.DefaultProvider.WaitForDisplayAndClick("moveItemUp", LocateBy.Id);
                    break;
                case MoveActions.Down:
                    UIUtil.DefaultProvider.WaitForDisplayAndClick("moveItemDown", LocateBy.Id);
                    break;
                default:
                    break;
            }
        }

        public void TypeFiltersDate(string from, string to)
        {
            string fromLocator = "crFilters_datePicker_Migrated_dpStart_dateInput_text";
            string toLocator = "crFilters_datePicker_Migrated_dpEnd_dateInput_text";
            UIUtil.DefaultProvider.Type(fromLocator, from, LocateBy.Id);
            UIUtil.DefaultProvider.Type(toLocator, to, LocateBy.Id);
        }

        public void SelectFiltersRegType(string regType)
        {
            UIUtil.DefaultProvider.SelectWithText("crFilters_ddlRegTypes", regType, LocateBy.Id);
        }

        public void SelectFiltersRegStatus(FilterRegStatus status)
        {
            UIUtil.DefaultProvider.SelectWithText("crFilters_ddlRegStatus", CustomStringAttribute.GetCustomString(status), LocateBy.Id);
        }

        public void SelectFiltersBalance(FilterBalance balance)
        {
            UIUtil.DefaultProvider.SelectWithText("crFilters_ddlBalance", CustomStringAttribute.GetCustomString(balance), LocateBy.Id);
        }

        public void SelectFilterLogic(FilterLogics filterLogic)
        {
            switch(filterLogic)
            {
                case FilterLogics.And:
                    UIUtil.DefaultProvider.WaitForDisplayAndClick("crFilters_customFilters_radioAND", LocateBy.Id);
                    break;
                case FilterLogics.Or:
                    UIUtil.DefaultProvider.WaitForDisplayAndClick("crFilters_customFilters_radioOR", LocateBy.Id);
                    break;
                default:
                    UIUtil.DefaultProvider.FailTest("No such logic!");
                    break;
            }
        }

        public void SetCustomFilterOne(CustomFilter field, FilterOperators operatorOne, string value)
        {
            UIUtil.DefaultProvider.SelectWithValue("crFilters_customFilters_ddlFieldFilter_0", CustomFilterValueAttribute.GetFilterValue(field), LocateBy.Id);
            UIUtil.DefaultProvider.SelectWithValue("crFilters_customFilters_ddlFilterOperator_0", CustomStringAttribute.GetCustomString(operatorOne), LocateBy.Id);
            UIUtil.DefaultProvider.Type("crFilters_customFilters_tbValue_0", value, LocateBy.Id);
        }

        public void SetCustomFilterTwo(CustomFilter field, FilterOperators operatorTwo, string value)
        {
            UIUtil.DefaultProvider.SelectWithValue("crFilters_customFilters_ddlFieldFilter_1", CustomFilterValueAttribute.GetFilterValue(field), LocateBy.Id);
            UIUtil.DefaultProvider.SelectWithValue("crFilters_customFilters_ddlFilterOperator_1", CustomStringAttribute.GetCustomString(operatorTwo), LocateBy.Id);
            UIUtil.DefaultProvider.Type("crFilters_customFilters_tbValue_1", value, LocateBy.Id);
        }

        public void ClickAddCustomFilter()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick("AddFilter2", LocateBy.Id);
        }

        public void ClickClearCustomFilters()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick("ClearFilters2", LocateBy.Id);
        }

        public void VerifyThirdFilterPresents(bool presents)
        {
            if (presents == true)
                UIUtil.DefaultProvider.VerifyElementPresent("crFilters_customFilters_ddlFieldFilter_2", presents, LocateBy.Id);
            else
                NUnit.Framework.Assert.True(UIUtil.DefaultProvider.IsElementHidden("//*[@id='crFilters_customFilters_ddlFieldFilter_2']/../..", LocateBy.XPath));
        }

        public void SelectGroupBy(string groupBy)
        {
            UIUtil.DefaultProvider.SelectWithValue("crSorts_ddlGrouping", groupBy, LocateBy.Id);
        }

        public void SelectFirstSortBy(SortingFields sortBy)
        {
            UIUtil.DefaultProvider.SelectWithValue("crSorts_ddlSort1", CustomStringAttribute.GetCustomString(sortBy), LocateBy.Id);
        }

        public void SetEnableSmartLink(bool check)
        {
            UIUtil.DefaultProvider.SetCheckbox("crAdvanced_cbShareReport", check, LocateBy.Id);
        }

        public void TypeSmartLinkPassword(string password)
        {
            UIUtil.DefaultProvider.Type("crAdvanced_tbPassword", password, LocateBy.Id);
        }

        public void ClickViewSmartLink()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick("crAdvanced_hlShareLink", LocateBy.Id);
        }

        public void SelectSmartLinkPopupWindow()
        {
            UIUtil.DefaultProvider.WaitForPageToLoad();
            UIUtil.DefaultProvider.SelectWindowByName("ReportHome");
        }

        public void CloseSmartLinkPopupWindow()
        {
            SelectSmartLinkPopupWindow();
            UIUtil.DefaultProvider.CloseWindow();
            UIUtil.DefaultProvider.SelectOriginalWindow();
            UIUtil.DefaultProvider.SelectPopUpFrameByName("dialog");
        }

        public void TypeSmartLinkPasswordAndSubmit(string password)
        {
            UIUtil.DefaultProvider.Type("txtPassword", password, LocateBy.Id);
            UIUtil.DefaultProvider.WaitForDisplayAndClick("submit", LocateBy.Id);
            UIUtil.DefaultProvider.WaitForPageToLoad();
        }

        public void VerifySmartLinkNeedsPassword(bool needs)
        {
            UIUtil.DefaultProvider.VerifyElementPresent("txtPassword", needs, LocateBy.Id);
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
                    UIUtil.DefaultProvider.SetCheckbox("crFields_cbTax", isShow, LocateBy.Id);
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
