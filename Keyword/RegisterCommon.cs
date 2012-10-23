namespace RegOnline.RegressionTest.Keyword
{
    using System;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.UIUtility;
    using System.Text;

    public class RegisterCommon
    {
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

        public double GetTotal(DataCollection.EventData_Common.RegisterPage page)
        {
            string amount = string.Empty;

            switch (page)
            {
                case DataCollection.EventData_Common.RegisterPage.Agenda:
                    amount = PageObject.PageObjectProvider.Register.RegistationSite.Agenda.Total.Text;
                    break;
                case DataCollection.EventData_Common.RegisterPage.Confirmation:
                    amount = PageObject.PageObjectProvider.Register.RegistationSite.Confirmation.Total.Text;
                    break;
                default:
                    break;
            }

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

        public void SelectRegType(Registrant reg)
        {
            if (reg.EventFee_Response.RegType != null)
            {
                if (reg.Event.StartPage.Customize_RegType_DisplayOptions.DisplayOption == DataCollection.EventData_Common.RegTypeDisplayOption.DropDownList)
                {
                    PageObject.PageObjectProvider.Register.RegistationSite.Checkin.RegTypeDropDown.SelectWithText(reg.EventFee_Response.RegType.Name);
                }
                else
                {
                    PageObject.PageObjectProvider.Register.RegistationSite.Checkin.SelectRegTypeRadioButton(reg.EventFee_Response.RegType);
                }
            }
        }

        public void TypeEventFeeDiscountCode(Registrant reg)
        {
            if (reg.EventFee_Response.Code != null)
            {
                PageObject.PageObjectProvider.Register.RegistationSite.Checkin.EventFeeDiscountCode.Type(reg.EventFee_Response.Code.CodeString);
            }
        }

        public bool HasErrorMessage(string errorMessage)
        {
            bool found = false;

            foreach (string error in PageObject.PageObjectProvider.Register.RegistationSite.GetErrorMessages())
            {
                if (error.Contains(errorMessage))
                {
                    found = true;
                    break;
                }
            }

            return found;
        }

        public void VerifyErrorMessages(string[] expectedErrorMessages)
        {
            string[] actualErrorMessages = PageObject.PageObjectProvider.Register.RegistationSite.GetErrorMessages();

            if (expectedErrorMessages.Length != actualErrorMessages.Length)
            {
                this.FailTestWithExpectedAndActualErrorMessages("Expected and actual error COUNT don't match!", expectedErrorMessages, actualErrorMessages);
            }

            for (int cnt = 0; cnt < actualErrorMessages.Length; cnt++ )
            {
                if (!expectedErrorMessages[cnt].Equals(actualErrorMessages[cnt]))
                {
                    this.FailTestWithExpectedAndActualErrorMessages("Expected and actual error messages don't match!", expectedErrorMessages, actualErrorMessages);
                }
            }
        }

        internal void FailTestWithErrorMessages()
        {
            string[] errorMessages = PageObject.PageObjectProvider.Register.RegistationSite.GetErrorMessages();
            UIUtility.UIUtil.DefaultProvider.FailTest(this.ComposeFailTestErrorMessage(errorMessages));
        }

        private void FailTestWithExpectedAndActualErrorMessages(string precedingMessage, string[] expectedErrorMessages, string[] actualErrorMessages)
        {
            string expectedErrorMessage_CombinedString = this.ComposeFailTestErrorMessage(expectedErrorMessages);
            string actualErrorMessage_CombinedString = this.ComposeFailTestErrorMessage(actualErrorMessages);

            UIUtility.UIUtil.DefaultProvider.FailTest(string.Format(
                "{0} Expected:{1} Actual:{2}", 
                precedingMessage, 
                expectedErrorMessage_CombinedString, 
                actualErrorMessage_CombinedString));
        }

        private string ComposeFailTestErrorMessage(string[] errorMessages)
        {
            StringBuilder errorMessage = new StringBuilder();

            for (int cnt = 1; cnt <= errorMessages.Length; cnt++)
            {
                errorMessage.Append(string.Format("Error{0}:'", cnt));
                errorMessage.Append(errorMessages[cnt - 1]);
                errorMessage.Append("';");
            }

            return errorMessage.Replace(';', '.', errorMessage.Length - 1, 1).ToString();
        }
    }
}
