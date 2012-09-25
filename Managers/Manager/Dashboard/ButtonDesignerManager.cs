namespace RegOnline.RegressionTest.Managers.Manager.Dashboard
{
    using System;
    //using RegOnline.RegressionTest.Fixtures;
    using RegOnline.RegressionTest.UIUtility;

    public class ButtonDesignerManager : ManagerBase
    {
        #region Enums

        public enum ButtonType
        {
            Button,
            FlashWidget,
            Text,
            InlineForm
        }

        public enum ButtonDestination
        {
            Form,
            Website,
            Calendar,
            Renewal
        }

        public enum ButtonWindowOption
        {
            New
        }

        public enum ButtonStyle
        {
            Style0,
            Style1,
            Style2,
            Style3,
            Style4
        }

        public enum ButtonLinkText
        {
            Text
        }
        #endregion

        #region Locators

        protected override string GetLocator<TEnum>(TEnum fieldEnum, LocatorType locatorType)
        {
            string locator = string.Empty;
            string enumString = fieldEnum.ToString();
            Type enumType = fieldEnum.GetType();

            if (enumType == typeof(ButtonType))
            {
                int enumInt = (int)Enum.Parse(typeof(ButtonType), enumString);
                locator = GetLocator((ButtonType)enumInt, locatorType);
            }
            else if (enumType == typeof(ButtonDestination))
            {
                int enumInt = (int)Enum.Parse(typeof(ButtonDestination), enumString);
                locator = GetLocator((ButtonDestination)enumInt, locatorType);
            }
            else if (enumType == typeof(ButtonWindowOption))
            {
                int enumInt = (int)Enum.Parse(typeof(ButtonWindowOption), enumString);
                locator = GetLocator((ButtonWindowOption)enumInt, locatorType);
            }
            else if (enumType == typeof(ButtonStyle))
            {
                int enumInt = (int)Enum.Parse(typeof(ButtonStyle), enumString);
                locator = GetLocator((ButtonStyle)enumInt, locatorType);
            }
            else if (enumType == typeof(ButtonLinkText))
            {
                int enumInt = (int)Enum.Parse(typeof(ButtonLinkText), enumString);
                locator = GetLocator((ButtonLinkText)enumInt, locatorType);
            }

            return locator;
        }

        private string GetLocator(ButtonType buttonType, LocatorType locatorType)
        {
            string baseLocator = "//img[contains(@src,'example-{0}.gif')]/../..{1}";
            string forStep = string.Empty;
            string forType = string.Empty;
            string locator = string.Empty;

            switch (buttonType)
            {
                case ButtonType.Button:
                    forStep = "button";
                    break;
                case ButtonType.FlashWidget:
                    forStep = "flash-widget";
                    break;
                case ButtonType.Text:
                    forStep = "text-link";
                    break;
                case ButtonType.InlineForm:
                    forStep = "iframe";
                    break;
            }

            switch (locatorType)
            {
                case LocatorType.Edit:
                    forType = "/input";
                    break;
                case LocatorType.Label:
                    forType = "/label";
                    break;
            }

            locator = string.Format(baseLocator, forStep, forType);
            return locator;
        }

        private string GetLocator(ButtonDestination buttonDest, LocatorType locatorType)
        {
            string baseLocator = "//td[contains(label,'{0}')]{1}";
            string forStep = buttonDest.ToString();
            string forType = string.Empty;
            string locator = string.Empty;

            switch (locatorType)
            {
                case LocatorType.Edit:
                    forType = "/input";
                    break;
                case LocatorType.Label:
                    forType = "/label";
                    break;
            }

            locator = string.Format(baseLocator, forStep, forType);
            return locator;
        }

        private string GetLocator(ButtonWindowOption windowOption, LocatorType locatorType)
        {
            string locator = string.Empty;

            switch (locatorType)
            {
                case LocatorType.Edit:
                    locator = "//input[@id='ctl00_cphDialog_Wizard1_chkNewWindow']";
                    break;
                case LocatorType.Label:
                    locator = "//label[@for='ctl00_cphDialog_Wizard1_chkNewWindow']";
                    break;
            }

            return locator;
        }

        private string GetLocator(ButtonStyle buttonStyle, LocatorType locatorType)
        {
            string baseForId = "ctl00_cphDialog_Wizard1_rblButtonStyles_{0}";
            string baseForEdit = "//input[@id='{0}']";
            string baseForLabel = "//label[@for='{0}']";
            string locator = string.Empty;
            string forStyle = string.Format(baseForId,(int)buttonStyle);

            switch (locatorType)
            {
                case LocatorType.Edit:
                    locator = string.Format(baseForEdit,forStyle);
                    break;
                case LocatorType.Label:
                    locator = string.Format(baseForLabel, forStyle);
                    break;
            }

            return locator;
        }

        private string GetLocator(ButtonLinkText buttonLinkText, LocatorType locatorType)
        {
            string baseLocator = "//div[@id='ctl00_cphDialog_Wizard1_pnlLinkText']{0}";
            string locator = string.Empty;

            switch (locatorType)
            {
                case LocatorType.Edit:
                    locator = string.Format(baseLocator,"/input");
                    break;
                case LocatorType.Label:
                    locator = string.Format(baseLocator, "/b");
                    break;
            }

            return locator;
        }

        #endregion

        #region Input types

        protected override InputType GetInputType<T>(T fieldEnum)
        {
            InputType inputType = new InputType();
            string enumString = fieldEnum.ToString();
            Type enumType = fieldEnum.GetType();

            if (enumType == typeof(ButtonType))
            {
                int enumInt = (int)Enum.Parse(typeof(ButtonType), enumString);
                inputType = GetInputType((ButtonType)enumInt);
            }
            else if (enumType == typeof(ButtonDestination))
            {
                int enumInt = (int)Enum.Parse(typeof(ButtonDestination), enumString);
                inputType = GetInputType((ButtonDestination)enumInt);
            }
            else if (enumType == typeof(ButtonWindowOption))
            {
                int enumInt = (int)Enum.Parse(typeof(ButtonWindowOption), enumString);
                inputType = GetInputType((ButtonWindowOption)enumInt);
            }
            else if (enumType == typeof(ButtonStyle))
            {
                int enumInt = (int)Enum.Parse(typeof(ButtonStyle), enumString);
                inputType = GetInputType((ButtonStyle)enumInt);
            }
            else if (enumType == typeof(ButtonLinkText))
            {
                int enumInt = (int)Enum.Parse(typeof(ButtonLinkText), enumString);
                inputType = GetInputType((ButtonLinkText)enumInt);
            }

            return inputType;
        }

        private InputType GetInputType(ButtonType fieldType)
        {
            InputType inputType = InputType.Checkbox;

            return inputType;
        }

        private InputType GetInputType(ButtonDestination fieldType)
        {
            InputType inputType = InputType.Checkbox;

            return inputType;
        }

        private InputType GetInputType(ButtonWindowOption fieldType)
        {
            InputType inputType = InputType.Checkbox;

            return inputType;
        }

        private InputType GetInputType(ButtonStyle fieldType)
        {
            InputType inputType = InputType.Checkbox;

            return inputType;
        }

        private InputType GetInputType(ButtonLinkText fieldType)
        {
            InputType inputType = InputType.Textbox;

            return inputType;
        }

        #endregion

        public string GetButtonCode()
        {
            return UIUtil.DefaultProvider.GetText("ctl00_cphDialog_Wizard1_txtBtnHtml", LocateBy.Id);
        }

        public void ClickButtonDesignerNext()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick("//input[contains(@id,'NextButton')]", LocateBy.XPath);
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }

        public void ClickButtonDesignerPrevious()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick("//input[contains(@id,'PreviousButton')]", LocateBy.XPath);
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }

        public void ClickButtonDesignerClose()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick("ctl00_cphDialog_Wizard1_FinishNavigationTemplateContainerID_FinishButton", LocateBy.Id);
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }
    }
}
