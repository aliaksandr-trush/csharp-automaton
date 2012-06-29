namespace RegOnline.RegressionTest.Managers.Register
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Attributes;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.Linq;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;

    public partial class RegisterManager : ManagerBase
    {
        #region Constants
        public const string FindCheckBox = "//label[text()='{0}']";
        public const string FindDropDown = "//label[text()='{0}']";
        public const string FindRadioButtonsOld = "//td[text()='{0}']/../following-sibling::tr[1]//label[text()='{1}']/preceding-sibling::input[1]/@id";
        public const string FindRadioButtons = "//*[text()='{0}']/../ol/li//label[text()='{1}']";
        public const string FindOneLineTextContributionNumber = "//label[text()='{0}']/../..//input";
        public const string FindParagraph = "//label[text()='{0}']/following-sibling::textarea[1]";
        public const string FindDate = "//label[(text()='{0}')]/../../div/input";
        public const string FindTime = "//label[(text()='{0}')]/../../div/input";
        public const string FindUpload = "//p[text()='{0}']/../..//a[@class='button']";
        public const string CustomFieldsListLocator = "//legend[text()='Other Info']/following-sibling::ol";
        public const string CustomFieldsListItemLocator = CustomFieldsListLocator + "/li";
        #endregion

        #region Custom Fields

        [Step]
        public void SelectPersonalInfoCustomFields()
        {
            List<Custom_Field> customFieldsList = this.DataTool.GetPersonalInfoCustomFields(this.CurrentEventId);

            foreach (Custom_Field customField in customFieldsList)
            {
                if (UIUtilityProvider.UIHelper.IsElementPresent(customField.Id.ToString(), LocateBy.Id)
                    || UIUtilityProvider.UIHelper.IsElementPresent(customField.Id.ToString(), LocateBy.Name))
                {
                    this.DoSeleniumActionForCustomField(customField);
                }
            }
        }

        [Step]
        public void SelectTravelCustomFields()
        {
            List<Custom_Field> travelCustomFields = this.Fetch_CustomFieldsFor(
                CurrentEventId, 
                CustomFieldManager.CustomFieldCategory.CustomField, 
                CustomFieldManager.CustomFieldLocation.LT_Travel, 
                false);

            foreach (Custom_Field customField in travelCustomFields)
            {
                if (UIUtilityProvider.UIHelper.IsElementPresent(customField.Id.ToString(), LocateBy.Id)
                    || UIUtilityProvider.UIHelper.IsElementPresent(customField.Id.ToString(), LocateBy.Name))
                {
                    this.DoSeleniumActionForCustomField(customField);
                }
            }
        }

        [Step]
        public void SelectLodgingCustomFields()
        {
            List<Custom_Field> lodgingCustomFields = this.Fetch_CustomFieldsFor(
                CurrentEventId, 
                CustomFieldManager.CustomFieldCategory.CustomField, 
                CustomFieldManager.CustomFieldLocation.LT_Lodging, 
                false);

            foreach (Custom_Field customField in lodgingCustomFields)
            {
                if (UIUtilityProvider.UIHelper.IsElementPresent(customField.Id.ToString(), LocateBy.Id)
                    || UIUtilityProvider.UIHelper.IsElementPresent(customField.Id.ToString(), LocateBy.Name))
                {
                    this.DoSeleniumActionForCustomField(customField);
                }
            }
        }

        [Step]
        public void SelectPreferencesCustomFields()
        {
            List<Custom_Field> preferencesCustomFields = this.Fetch_CustomFieldsFor(
                CurrentEventId, 
                CustomFieldManager.CustomFieldCategory.CustomField, 
                CustomFieldManager.CustomFieldLocation.LT_Preferences, 
                false);

            foreach (Custom_Field customField in preferencesCustomFields)
            {
                if (UIUtilityProvider.UIHelper.IsElementPresent(customField.Id.ToString(), LocateBy.Id)
                    || UIUtilityProvider.UIHelper.IsElementPresent(customField.Id.ToString(), LocateBy.Name))
                {
                    this.DoSeleniumActionForCustomField(customField);
                }
            }
        }

        public void VerifyCustomFieldNames(List<string> expectNames)
        {
            List<string> actualNames = this.GetCustomFieldNames();

            if (expectNames.Count != actualNames.Count)
            {
                VerifyTool.VerifyValue(expectNames.Count, actualNames.Count, "There are {0} custom field items in total.");
            }
            else
            {
                for (int i = 0; i < expectNames.Count; i++)
                {
                    if (expectNames[i] != actualNames[i])
                    {
                        VerifyTool.VerifyValue(expectNames[i], actualNames[i], "The name of custom field " + (i + 1).ToString() + " is {0}.");
                    }
                }
            }
        }

        public List<string> GetCustomFieldNames()
        {
            int count = UIUtilityProvider.UIHelper.GetXPathCountByXPath(CustomFieldsListItemLocator);

            List<string> names = new List<string>();

            for (int i = 0; i < count; i++)
            {
                names.Add(UIUtilityProvider.UIHelper.GetText(string.Format(CustomFieldsListItemLocator + "[{0}]", i + 1), LocateBy.XPath));
            }

            return names;
        }

        [Step]
        public void SetCustomFieldCheckBox(string customFieldName, bool isChecked)
        {
            string xPath = string.Format(FindCheckBox, customFieldName);
            UIUtilityProvider.UIHelper.SetCheckbox(UIUtilityProvider.UIHelper.GetAttribute(xPath, "for", LocateBy.XPath), isChecked, LocateBy.Id);
            this.WaitForConditionalLogic();
        }

        public void VerifyCustomFieldCheckBox(string customFieldName, bool isChecked, bool editable)
        {
            string xPath = string.Format(FindCheckBox, customFieldName);

            VerifyTool.VerifyValue(
                isChecked,
                UIUtilityProvider.UIHelper.IsChecked(UIUtilityProvider.UIHelper.GetAttribute(xPath, "for", LocateBy.XPath), LocateBy.Id), 
                "Checkbox " + customFieldName + " is checked: {0}");

            VerifyTool.VerifyValue(
                editable,
                UIUtilityProvider.UIHelper.IsEditable(UIUtilityProvider.UIHelper.GetAttribute(xPath, "for", LocateBy.XPath), LocateBy.Id),
                "Checkbox " + customFieldName + "is editable: {0}");
        }

        public void SelectCustomFieldDropDown(string customFieldName, string choice)
        {
            string xPath = string.Format(FindDropDown, customFieldName);
            UIUtilityProvider.UIHelper.SelectWithText(UIUtilityProvider.UIHelper.GetAttribute(xPath, "for", LocateBy.XPath), choice, LocateBy.Id);
            this.WaitForConditionalLogic();
        }
        
        public void SelectCustomFieldDropDown(int id, string choice)
        {
            UIUtilityProvider.UIHelper.SelectWithText(UIUtilityProvider.UIHelper.GetAttribute(id.ToString(), "for", LocateBy.Id), choice, LocateBy.Id);
            this.WaitForConditionalLogic();
        }

        public void VerifyCustomFieldDropDown(string customFieldName, string choice, bool editable)
        {
            string xPath = string.Format(FindDropDown, customFieldName);

            if (!String.IsNullOrEmpty(choice))
            {
                VerifyTool.VerifyValue(
                    choice,
                    UIUtilityProvider.UIHelper.GetSelectedOptionFromDropdownByXPath("//*[@id='" + UIUtilityProvider.UIHelper.GetAttribute(xPath, "for", LocateBy.XPath) + "']"),
                    "Dropdown " + customFieldName + " selection: {0}");
            }
            else
            {
                Utilities.VerifyTool.VerifyValue(
                    false,
                    UIUtilityProvider.UIHelper.IsAnySelectionMadeOnDropDownByXPath("//*[@id='" + UIUtilityProvider.UIHelper.GetAttribute(xPath, "for", LocateBy.XPath) + "']"),
                    "Dropdown " + customFieldName + " has a selection");
            }

            VerifyTool.VerifyValue(
                editable,
               UIUtilityProvider.UIHelper.IsEditable(UIUtilityProvider.UIHelper.GetAttribute(xPath, "for", LocateBy.XPath), LocateBy.Id),
               "Dropdown " + customFieldName + " is editable: {0}");
        }

        public void SelectCustomFieldRadioButtons(string customFieldName, string choice)
        {
            string xPath = string.Format(FindRadioButtons, customFieldName, choice);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(UIUtilityProvider.UIHelper.GetAttribute(xPath, "for", LocateBy.XPath), LocateBy.Id);
            this.WaitForConditionalLogic();
        }

        public void VerifyCustomFieldRadioButtons(string customFieldName, string choice, bool isChecked, bool editable)
        {
            string xPath = string.Format(FindRadioButtons, customFieldName, choice);

            VerifyTool.VerifyValue(
                isChecked,
                UIUtilityProvider.UIHelper.IsChecked(UIUtilityProvider.UIHelper.GetAttribute(xPath, "for", LocateBy.XPath), LocateBy.Id),
                "Radio Buttons " + customFieldName + " selection {0}");

            VerifyTool.VerifyValue(
                editable,
                UIUtilityProvider.UIHelper.IsEditable(UIUtilityProvider.UIHelper.GetAttribute(xPath, "for", LocateBy.XPath), LocateBy.Id),
                "Radio Buttons " + customFieldName + " is editable: {0}");
        }

        [Step]
        public void FillOutCustomOneLineTextOrNumberOrContribution(string customFieldName, string input)
        {
            string xPath = string.Format(FindOneLineTextContributionNumber, customFieldName);
            UIUtilityProvider.UIHelper.Type(xPath, input, LocateBy.XPath);
        }

        public void FillOutCustomOneLineTextOrNumberOrContribution(int id, string input)
        {
            UIUtilityProvider.UIHelper.Type(id.ToString(), input, LocateBy.Id);
        }

        public void VerifyCustomOneLineTextOrNumberOrContribution(string customFieldName, string text, bool editable)
        {
            string xPath = string.Format(FindOneLineTextContributionNumber, customFieldName);

            VerifyTool.VerifyValue(
                text,
                UIUtilityProvider.UIHelper.GetAttribute(xPath, "value", LocateBy.XPath), 
                "Field " + customFieldName + "'s text : {0}");

            VerifyTool.VerifyValue(
                editable,
                UIUtilityProvider.UIHelper.IsEditable(UIUtilityProvider.UIHelper.GetAttribute(xPath, "id", LocateBy.XPath), LocateBy.Id),
                "Field " + customFieldName + "is editable: {0}");
        }

        public void FillOutCustomParagraphField(string customFieldName, string textToEnter)
        {
            string xPath = string.Format(FindParagraph, customFieldName);
            string id = UIUtilityProvider.UIHelper.GetAttribute(xPath, "id", LocateBy.XPath);
            UIUtilityProvider.UIHelper.Type(id, textToEnter, LocateBy.Id);
        }

        public void FillOutCustomParagraphField(int id, string textToEnter)
        {
            UIUtilityProvider.UIHelper.Type(id.ToString(), textToEnter, LocateBy.Id);
        }

        public void VerifyCustomParagraphField(string customFieldName, string text, bool editable)
        {
            string xPath = string.Format(FindParagraph, customFieldName);
            string id = UIUtilityProvider.UIHelper.GetAttribute(xPath, "id", LocateBy.XPath);
            VerifyTool.VerifyValue(text, UIUtilityProvider.UIHelper.GetText(id, LocateBy.Id), "Paragraph " + customFieldName + " text: {0}");
            VerifyTool.VerifyValue(editable, UIUtilityProvider.UIHelper.IsEditable(id, LocateBy.Id), "Paragraph " + customFieldName + " is editable: {0}");
        }

        public void FillOutCustomDateField(string customFieldName, string date)
        {
            string xPath = string.Format(FindDate, customFieldName);
            UIUtilityProvider.UIHelper.WaitForElementPresent(xPath, LocateBy.XPath);
            UIUtilityProvider.UIHelper.Type(xPath, date, LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(xPath + "/following-sibling::img", LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void FillOutCustomDateField(int id, string date)
        {
            UIUtilityProvider.UIHelper.WaitForElementPresent(id.ToString(), LocateBy.Id);
            UIUtilityProvider.UIHelper.Type(id.ToString(), date, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(string.Format("//input[@id='{0}']/following-sibling::img", id), LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void Uploadfile(string customFieldName, string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                UIUtilityProvider.UIHelper.WaitForDisplayAndClick(string.Format(FindUpload, customFieldName), LocateBy.XPath);
                UIUtilityProvider.UIHelper.SelectWindowByTitle("File Upload Page");
                UIUtilityProvider.UIHelper.WaitForPageToLoad();

                //WebDriverBase.WebDriverManager.driver.FindElement(By.Id("ctl00_cphBody_ruFilefile0")).SendKeys(
                //    Fixtures.ConfigurationProvider.XmlConfig.AllSettings.DataPath + fileName);

                UIUtilityProvider.UIHelper.SendKeys("ctl00_cphBody_ruFilefile0", ConfigurationProvider.XmlConfig.EnvironmentConfiguration.DataPath + fileName, LocateBy.Id);
                UIUtilityProvider.UIHelper.WaitForDisplayAndClick("ctl00_cphBody_btnSubmit", LocateBy.Id);
                UIUtilityProvider.UIHelper.WaitForPageToLoad();
                UIUtilityProvider.UIHelper.WaitForDisplayAndClick("ctl00_cphBody_btnCancel", LocateBy.Id);
                UIUtilityProvider.UIHelper.SelectOriginalWindow();
            }
        }


        public void VerifyCustomDateField(string customFieldName, string date, bool editable)
        {
            string xPath = string.Format(FindDate, customFieldName);
            UIUtilityProvider.UIHelper.WaitForElementPresent(xPath, LocateBy.XPath);
            VerifyTool.VerifyValue(date, UIUtilityProvider.UIHelper.GetAttribute(xPath, "value", LocateBy.XPath), "Date " + customFieldName + ":{0}");
            VerifyTool.VerifyValue(editable, UIUtilityProvider.UIHelper.IsEditable(xPath, LocateBy.XPath), "Date " + customFieldName + " is editable: {0}");
            //UIUtilityProvider.UIHelper.Click(UIUtilityProvider.UIHelper.GetLocatorPrefix(UIBase.UIManager.FindLocatorBy.Id) + "outsideWrapper");
        }

        public void FillOutCustomTimeField(string customFieldName, string time)
        {
            string xPath = string.Format(FindTime, customFieldName);
            UIUtilityProvider.UIHelper.WaitForElementPresent(xPath, LocateBy.XPath);
            UIUtilityProvider.UIHelper.Type(xPath, time, LocateBy.XPath);
            UIUtilityProvider.UIHelper.SwitchToMainContent();
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(xPath + "/..", LocateBy.XPath);
        }

        public void FillOutCustomTimeField(int id, string time)
        {
            string locator_Id = id.ToString();
            UIUtilityProvider.UIHelper.Type(locator_Id, time, LocateBy.Id);
            UIUtilityProvider.UIHelper.SwitchToMainContent();
            //clears the time drop down picker that can interfere with webdriver. 
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("//ol[@class='fieldList custom']", LocateBy.XPath);/*//label[@for='" + id + "']"  + "/../.."*/
        }

        public void VerifyCustomTimeField(string customFieldName, string time, bool editable)
        {
            string xPath = string.Format(FindTime, customFieldName);
            VerifyTool.VerifyValue(time, UIUtilityProvider.UIHelper.GetAttribute(xPath, "value", LocateBy.XPath), "Time " + customFieldName + ":{0}");
            VerifyTool.VerifyValue(editable, UIUtilityProvider.UIHelper.IsEditable(xPath, LocateBy.XPath), "Time " + customFieldName + " is editable: {0}");
        }

        public void SetCustomFieldCheckbox(int id, bool check)
        {
            UIUtilityProvider.UIHelper.SetCheckbox(id.ToString(), check, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        [Step]
        public void SetCustomFieldCheckbox(string name, bool check)
        {
            this.SetCustomFieldCheckbox(this.GetCustomFieldIDForCheckboxItem(name), check);
        }

        public void TypeCustomField(int id, string text)
        {
            UIUtilityProvider.UIHelper.Type(id.ToString(), text, LocateBy.Id);
        }

        [Step]
        public void TypeCustomField(string name, string text)
        {
            this.TypeCustomField(this.GetCustomFieldIDForCheckboxItem(name), text);
        }

        /// <summary>
        /// Get cf id for checkbox item
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [Step]
        public int GetCustomFieldIDForCheckboxItem(string name)
        {
            return Convert.ToInt32(UIUtilityProvider.UIHelper.GetAttribute("//label[text()='" + name + "']", "for", LocateBy.XPath));
        }

        /// <summary>
        /// Get id for custom field on register PI page by index(starting from 1)
        /// </summary>
        /// <param name="cfIndexInRow">Index for cf item(starting from 1)</param>
        /// <returns></returns>
        public int GetCustomFieldIdOnPIPage(int cfIndexInRow)
        {
            return Convert.ToInt32(UIUtilityProvider.UIHelper.GetAttribute(string.Format(
                "//legend[text()='Other Personal Information']/following-sibling::ol/li[@data-z='{0}']",
                cfIndexInRow), "data - id", LocateBy.XPath));
        }

        /// <summary>
        /// Get id for custom field on register agenda page by index(starting from 1)
        /// </summary>
        /// <param name="cfIndexInRow">Index for cf item(starting from 1)</param>
        /// <returns></returns>
        public int GetCustomFieldIdOnAgendaPage(int cfIndexInRow)
        {
            return Convert.ToInt32(UIUtilityProvider.UIHelper.GetAttribute(string.Format(
                "//div[@id='pageContent']/fieldset/ol/li[@data-z='{0}']",
                cfIndexInRow), "data-id", LocateBy.XPath));
        }
        #endregion
    }
}
