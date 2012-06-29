namespace RegOnline.RegressionTest.Keyword
{
    using System;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.UIUtility;

    public class RegisterDefault
    {
        private PageObject.Register.Checkin Checkin = new PageObject.Register.Checkin();
        private PageObject.Register.Confirmation Confirmation = new PageObject.Register.Confirmation();
        private PageObject.Register.PageObjectHelper RegisterHelper = new PageObject.Register.PageObjectHelper();
        private PageObject.PageObjectHelper PageObjectHelper = new PageObject.PageObjectHelper();

        public void OpenRegisterPage(int eventId)
        {
            UIUtilityProvider.UIHelper.OpenUrl(ConfigurationProvider.XmlConfig.AccountConfiguration.BaseUrl + eventId);
            PageObjectHelper.Allow_Click();
        }

        public void OpenRegTypeDirectUrl(int eventId, int regTypeId)
        {
            UIUtilityProvider.UIHelper.OpenUrl(string.Format(ConfigurationProvider.XmlConfig.AccountConfiguration.BaseUrl + "?eventID={0}&rTypeID={1}", eventId, regTypeId));
            PageObjectHelper.Allow_Click();
        }

        public bool IsOnLoginPage()
        {
            return UIUtilityProvider.UIHelper.UrlContainsPath("register/login.aspx");
        }

        public bool IsOnCheckinPage()
        {
            return UIUtilityProvider.UIHelper.UrlContainsPath("register/checkin.aspx");
        }

        public bool IsOnPersonalInfoPage()
        {
            return UIUtilityProvider.UIHelper.UrlContainsPath("register/PersonalInfo.aspx");
        }

        public bool IsOnAgendaPage()
        {
            return UIUtilityProvider.UIHelper.UrlContainsPath("register/agenda.aspx");
        }

        public bool IsOnAttendeeCheckPage()
        {
            return UIUtilityProvider.UIHelper.UrlContainsPath("Register/AttendeeCheck.aspx");
        }

        public bool IsOnCheckoutPage()
        {
            return UIUtilityProvider.UIHelper.UrlContainsPath("register/checkout.aspx");
        }

        public bool IsOnConfirmationRedirectPage()
        {
            return (UIUtilityProvider.UIHelper.UrlContainsPath("regonline.com/register/ConfirmationRedirector.aspx")) && (UIUtilityProvider.UIHelper.IsTextPresent("Active Advantage"));
        }

        public bool IsOnConfirmationPage()
        {
            return UIUtilityProvider.UIHelper.UrlContainsPath("register/confirmation.aspx");
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
            string amount = Confirmation.Total.Text;
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

        public void SelectRegType(string regTypeName)
        {
            if (Checkin.RegTypeRadioButton.IsPresent)
            {
                Checkin.SelectRegTypeRadioButton(regTypeName);
            }
            else
            {
                Checkin.RegTypeDropDown.SelectWithText(regTypeName);
            }
        }

        public bool HasErrorMessage(string errorMessage)
        {
            bool found = false;

            int count = RegisterHelper.ErrorMessages.Count;
            string[] errorList = new string[count];

            for (int i = 1; i <= count; i++)
            {
                errorList[i - 1] = UIUtilityProvider.UIHelper.GetText(string.Format(
                    RegisterHelper.ErrorMessages.Locator + "[{0}]", i), LocateBy.XPath);
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
