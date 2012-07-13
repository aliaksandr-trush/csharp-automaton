namespace RegOnline.RegressionTest.Keyword
{
    using System;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.UIUtility;

    public class RegisterDefault
    {
        public void OpenRegisterPageUrl(int eventId)
        {
            UIUtilityProvider.UIHelper.OpenUrl(ConfigurationProvider.XmlConfig.AccountConfiguration.BaseUrl + eventId);
            PageObject.PageObjectHelper.AllowCookie_Click();
        }

        public void OpenRegTypeDirectUrl(int eventId, int regTypeId)
        {
            UIUtilityProvider.UIHelper.OpenUrl(string.Format(ConfigurationProvider.XmlConfig.AccountConfiguration.BaseUrl + "?eventID={0}&rTypeID={1}", eventId, regTypeId));
            PageObject.PageObjectHelper.AllowCookie_Click();
        }

        public void OpenRegisterPageUrl(string shortcut)
        {
            UIUtilityProvider.UIHelper.OpenUrl(string.Format(
                "{0}{1}", 
                ConfigurationProvider.XmlConfig.AccountConfiguration.BaseUrl,
                shortcut));

            PageObject.PageObjectHelper.AllowCookie_Click();
        }

        public string GenerateCurrentRegistrantLastName()
        {
            string lastName = string.Empty;

            int check = Convert.ToInt32(System.DateTime.Now.Ticks.ToString().Substring(0, 5)) % 9;

            lastName =
                "Rgrssn-"
                + check.ToString()
                + System.DateTime.Now.Ticks.ToString().Substring(6);

            return lastName;
        }

        public double GetConfirmationTotal()
        {
            string amount = PageObject.PageObjectProvider.Register.RegistationSite.Confirmation.Total.Text;
            string a = "";

            for (int i = 0; i < amount.Length; i++)
            {
                if (Char.IsNumber(amount, i) || (amount.Substring(i, 1) == "."))
                {
                    a += amount.Substring(i, 1);
                }
            }

            return Convert.ToDouble(a);
        }

        public void SelectRegType(RegType regType)
        {
            if (PageObject.PageObjectProvider.Register.RegistationSite.Checkin.RegTypeRadioButton.IsPresent)
            {
                PageObject.PageObjectProvider.Register.RegistationSite.Checkin.SelectRegTypeRadioButton(regType);
            }
            else
            {
                PageObject.PageObjectProvider.Register.RegistationSite.Checkin.RegTypeDropDown.SelectWithText(regType.RegTypeName);
            }
        }

        public bool HasErrorMessage(string errorMessage)
        {
            bool found = false;

            int count = PageObject.PageObjectProvider.Register.RegistationSite.ErrorMessages.Count;
            string[] errorList = new string[count];

            for (int i = 1; i <= count; i++)
            {
                errorList[i - 1] = UIUtilityProvider.UIHelper.GetText(string.Format(
                    PageObject.PageObjectProvider.Register.RegistationSite.ErrorMessages.Locator + "[{0}]", i), LocateBy.XPath);
            }

            foreach (string error in errorList)
            {
                if (error.Contains(errorMessage))
                    found = true;
            }

            return found;
        }
    }
}
