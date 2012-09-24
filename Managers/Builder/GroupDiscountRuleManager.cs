namespace RegOnline.RegressionTest.Managers.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using RegOnline.RegressionTest.DataAccess;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;

    public class GroupDiscountRuleManager : ManagerBase
    {
        public const string FrameID = "dialog";
        public const string EnableGroupDiscountRule = "ctl00_cphDialog_groupDiscountEnabledCheckBox";
        private const string GroupSizeOrMoreLocator = "ctl00_cphDialog_ddlIsGroupSizeOrMore";
        private const string ApplyToAllLinkLocator = "ctl00_cphDialog_rbApplyAll";
        private const string ApplyToOnlySelectedLocator = "ctl00_cphDialog_rbApplySelected";
        public const string RegTypeFeeDefultName = "{0}_Event_Fee";
        private const string ItemListLocator = "ctl00_cphDialog_CustomFieldsTreeView";

        #region Enums
        public enum GroupSizeOption
        {
            JustSize,
            SizeOrMore
        }

        public enum DiscountType
        {
            [CustomString("US Dollar")]
            USDollar,

            [CustomString("Percent")]
            Percent
        }

        public enum AdditionalRegOption
        {
            Additional,
            All,
            AnyAdditional
        }
        #endregion

        public void SetGroupDiscountRule(
            int size, 
            GroupSizeOption sizeOrMore, 
            double discountPrice, 
            DiscountType type, 
            AdditionalRegOption additionalOrAll, 
            int? additionalNumber)
        {
            // Enable group discount rule
            this.SetEnableRule(true);

            // Set group size
            this.SetRuleGroupSize(size, sizeOrMore);

            // Set group discount
            this.SetDiscount(discountPrice, type);

            if (sizeOrMore == GroupSizeOption.JustSize)
            {
                this.SetAdditionalRegOption(additionalOrAll, additionalNumber);
            }

            this.SaveAndClose();
        }

        public void SetEnableRule(bool enable)
        {
            WebDriverUtility.DefaultProvider.SetCheckbox(EnableGroupDiscountRule, enable, LocateBy.Id);
        }

        public void SetRuleGroupSize(int groupSize, GroupSizeOption sizeOrMore)
        {
            // Enter group size
            WebDriverUtility.DefaultProvider.TypeRADNumericById("ctl00_cphDialog_GroupSizeTextBox", groupSize);

            // Select group size or more
            if (sizeOrMore == GroupSizeOption.SizeOrMore)
            {
                WebDriverUtility.DefaultProvider.SelectWithText(GroupSizeOrMoreLocator, "or more", LocateBy.Id);
            }
            else if (sizeOrMore == GroupSizeOption.JustSize)
            {
                WebDriverUtility.DefaultProvider.SelectWithText(GroupSizeOrMoreLocator, "", LocateBy.Id);
            }
        }

        public void SetDiscount(double price, DiscountType type)
        {
            // Enter discount amount
            WebDriverUtility.DefaultProvider.TypeRADNumericById("ctl00_cphDialog_AmountTextBox", price);

            // Select group rule type
            WebDriverUtility.DefaultProvider.SelectWithText("ctl00_cphDialog_ddlGroupRuleType", CustomStringAttribute.GetCustomString(type), LocateBy.Id);
        }

        public void SetAdditionalRegOption(AdditionalRegOption additionalOrAll, int? additionalNumber)
        {
            WebDriverUtility.DefaultProvider.SelectWithText("ctl00_cphDialog_AdditionalAllDropDownList", additionalOrAll.ToString(), LocateBy.Id);
            
            if (additionalOrAll == AdditionalRegOption.Additional)
            {
                WebDriverUtility.DefaultProvider.TypeRADNumericById("ctl00_cphDialog_EffectedSizeTextBox", additionalNumber);
            }
        }

        public void ExpandApplyRuleToAllItems()
        {
            //string linkText = UIUtilityProvider.UIHelper.GetText(ApplyToAllLinkLocator);
            
            //if (linkText == "all items")
            //{
            //    UIUtilityProvider.UIHelper.ClickAndWaitAJAX(ApplyToAllLinkLocator);
            //}
            if (!WebDriverUtility.DefaultProvider.IsChecked(ApplyToAllLinkLocator, LocateBy.Id))
            {
                WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(ApplyToAllLinkLocator, LocateBy.Id);
            }
        }

        public void CollapseApplyRuleToAllItems()
        {
            //string linkText = UIUtilityProvider.UIHelper.GetText(ApplyToAllLinkLocator);

            //if (linkText == "selected items")
            //{
            //    UIUtilityProvider.UIHelper.ClickAndWaitAJAX(ApplyToAllLinkLocator);
            //}

            if (!WebDriverUtility.DefaultProvider.IsChecked(ApplyToOnlySelectedLocator, LocateBy.Id))
            {
                WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(ApplyToOnlySelectedLocator, LocateBy.Id);
            }

        }

        public void SetApplyRuleToAll(bool applyToAll)
        {
            if (applyToAll)
            {
                WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(ApplyToAllLinkLocator, LocateBy.Id);
                WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
            }
            else
            {
                WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(ApplyToOnlySelectedLocator, LocateBy.Id);
                WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
                SetApplyRuleToItemByName(false, "All");
            }
            //string applyToAllCheckboxLocator = string.Format("//div[@id='{0}']/ul/li/div[@class='rtTop']/input[@class='rtChk']", ItemListLocator);
            //UIUtilityProvider.UIHelper.SetCheckbox(applyToAllCheckboxLocator, applyToAll);
            //UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void SetApplyRuleToItemByName(bool check, string itemName)
        {
            string itemLocator = string.Format("//span[text() = '{0}']/../input", itemName);

            if(WebDriverUtility.DefaultProvider.IsElementPresent("//span[@class='rtPlus']", LocateBy.XPath))
            {
                WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("//span[@class='rtPlus']", LocateBy.XPath);
                WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
            }

            WebDriverUtility.DefaultProvider.SetCheckbox(itemLocator, check, LocateBy.XPath);
        }
        
        public void SetRuleExpireTime(bool check, DateTime expireTime)
        {
            string ruleExpireCheckboxLocator = "ctl00_cphDialog_chkExpireRule";

            WebDriverUtility.DefaultProvider.SetCheckbox(ruleExpireCheckboxLocator, check, LocateBy.Id);
            WebDriverUtility.DefaultProvider.WaitForElementPresent("ctl00_cphDialog_dtpRuleExpireDate_dateInput_wrapper", LocateBy.Id);

            // Set expire time
            string date = string.Format("{0}/{1}/{2}", expireTime.Month, expireTime.Day, expireTime.Year);
            string datetime = expireTime.ToString("yyyy-MM-dd-00-00-00");
            
            string expireTimeboxLocator = "ctl00_cphDialog_dtpRuleExpireDate";
            WebDriverUtility.DefaultProvider.Type(expireTimeboxLocator + "_dateInput_text", date, LocateBy.Id);
            WebDriverUtility.DefaultProvider.Type(expireTimeboxLocator + "_dateInput", datetime, LocateBy.Id);
            WebDriverUtility.DefaultProvider.Type(expireTimeboxLocator, datetime, LocateBy.Id);
        }

        public void SetGroupDiscountMessage(string message)
        {
            string groupDiscountMessageLocator = "ctl00_cphDialog_RegIntroTextBox";
            WebDriverUtility.DefaultProvider.Type(groupDiscountMessageLocator, message, LocateBy.Id);
        }

        public void SetGroupDiscountPopUpMessage(int eventID, string message)
        {
            // If there is no custom pop up message use default message
            if (message == null)
            {
                WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("restore default", LocateBy.LinkText);
            }
            else
            {
                //S.DiscountRulesService discountRuleService = new S.DiscountRulesService();
                //E.TList<E.DiscountRules> discountRules = discountRuleService.GetByEventId(eventID);
                List<DiscountRule> discountRules = new List<DiscountRule>();

                ClientDataContext db = new ClientDataContext();
                discountRules = (from d in db.DiscountRules where d.EventId == eventID select d).ToList();

                if (discountRules.Count > 0)
                {
                    foreach (DiscountRule discountRule in discountRules)
                    {
                        if (discountRule.isCurrent)
                        {
                            discountRule.Reg_Description = Convert.ToString(message);
                            db.SubmitChanges();
                            //discountRuleService.Save(discountRule);
                            break;
                        }
                    }
                }
            }
        }

        private void SelectThisFrame()
        {
            try
            {
                WebDriverUtility.DefaultProvider.SelectPopUpFrameByName(FrameID);
            }
            catch
            {
                // Ignore
            }
        }

        public void SaveAndClose()
        {
            SelectThisFrame();
            WebDriverUtility.DefaultProvider.ClickSaveAndClose();
            Utility.ThreadSleep(1);
            SelectBuilderWindow();
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }

        public void Cancel()
        {
            SelectThisFrame();
            WebDriverUtility.DefaultProvider.ClickCancel();
            Utility.ThreadSleep(1);
            SelectBuilderWindow();
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }
    }
}
