namespace RegOnline.RegressionTest.Managers.Register
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Attributes;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;

    public partial class RegisterManager : ManagerBase
    {
        #region Error Messages

        public static class Error
        {
            public const string EmailAddressIsBlank = "Email address is blank.";
            
            public const string EmailAddressesDoNotMatch = 
                "The email addresses that you entered do not match. Re-enter your email address.";

            public const string CheckinEventSoldOut =
                "We're sorry that the event you are trying to register for has been sold out.";

            public const string CheckinEventSoldOutForRegTypeDirectLink =
                "Sorry, the event you are registering for is currently full.";

            public const string CheckinRequiredFieldsError = "You must fill out all required fields.";

            public const string EventLimitReachedAndContinue =
                "This event is full. You cannot add another person.\rTo complete this registration, click Continue.";

            public const string RegTypeLimitReachedAndContinueFormat =
                "You have registered the maximum number of people for the {0} registration type.\rTo continue and complete this registration, click Continue.";

            public const string RegTypeLimitReachedFormat = "{0} limit reached!";

            public const string CheckinDisallowDuplicateEmail =
                "Our records show that this email address has already been used to register for this event. Each registrant must use a unique email address.";

            public const string SelectAllRequired_CF_Personal =
                "You must select all required check boxes. (Other Personal Information)";

            public const string SelectAllRequired_CF_Travel =
                "You must select all required check boxes. (Other Travel Information)";

            public const string SelectAllRequired_CF_Lodging =
                "You must select all required check boxes. (Other Lodging Information)";

            public const string SelectAllRequired_CF_Preferences =
                "You must select all required check boxes. (Other Preference Information)";

            public const string SelectAllRequired_AG =
                "You must fill out all required fields.";

            public const string SelectAllRequiredFields = "You must fill out all required fields.";

            public const string MerchandiseMinMaxFormat_NameMinMax =
                "The quantity entered for \"{0}\" is outside the acceptable range. The acceptable range is {1} thru {2}";

            //public const string MerchandiseDiscountCode =
            //    "No code was entered for \"{0}\".";

            public const string MerchandiseDiscountCode =
                "You must enter a discount code.";

            public const string MerchandiseDiscountCode_old =
                "No code was entered";

            public const string MerchandiseMinOnlyFormat_NameMin =
                "The minimum quantity accepted for \"{0}\" is {1}.";

            public const int MerchandiseMinLimitDefault = 0;

            public const string RegistrationNoLongerAccepted = 
                "Registrations are no longer being accepted for {0}.\r\nIf you have any further questions, contact the event organizer.";

            public const string OverLappingAgendaItems =
                "Some of the items you selected have overlapping times.";

            public const string IncorrectLogin =
                "Your email address is incorrect. Please try again.";

            public const string YouMustEnterValidEmailAddress = "You must enter a valid email address.";

            public const string CodeLimitHasReached = "No more \"{0}\" codes are being accepted for this registrant type. Enter another code or contact your event administrator.";

            public const string InvalidCode = "You have entered an invalid code. ";

            public const string InvalidPassword = "Your login or password is incorrect. Please try again.";
        }

        [Step]
        public void VerifyIncorrectURL(string eventName)
        {
            this.VerifyErrorMessage(string.Format(Error.RegistrationNoLongerAccepted, eventName));
        }

        public void FullRegTypeDirectLinkErrorMessage(string expectedMessage)
        {
            string actualMessage = WebDriverUtility.DefaultProvider.GetText("//div[@id='ctl00_valSummary']/ul/li", LocateBy.XPath);
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        // Verifies the expected error message at the top of the page
        [Verify]
        public void VerifyErrorMessage(string errorMessage)
        {
            CheckErrorMessage(errorMessage, true);
        }

        public void VerifyErrorMessage(string[] errorMessages)
        {
            CheckErrorMessage(errorMessages, true);
        }

        [Verify]
        public void VerifyErrorMessagesCount(int count)
        {
            int actualQuantity = this.GetErrorMessagesList().Count;

            if (actualQuantity != count)
            {
                VerifyTool.VerifyValue(count, actualQuantity, "Error messages' count: {0}");
            }
        }

        [Verify]
        public void VerifyErrorMessages(List<string> expErrorList)
        {
            List<string> actErrorList = this.GetErrorMessagesList();

            for (int i = 0; i < expErrorList.Count; i++)
            {
                if (expErrorList[i] != actErrorList[i])
                {
                    string forExpected = "For error " + i + ": " + expErrorList[i];
                    string forActual = "For error " + i + ": " + actErrorList[i];
                    Assert.AreEqual(forExpected, forActual);
                }
            }
        }

        public bool CheckErrorMessage(string errorMessage)
        {
            return CheckErrorMessage(errorMessage, false);
        }

        public bool CheckErrorMessage(string[] errorMessages)
        {
            return CheckErrorMessage(errorMessages, false);
        }

        public bool CheckErrorMessage(string errorMessage, bool doVerify)
        {
            bool found = false;

            string[] errorList = GetErrorMessages();

            foreach (string error in errorList)
            {
                if (error.Contains(errorMessage))
                    found = true;
            }

            if ((!found) && (doVerify))
            {
                string failMessage;

                if (errorList.Length < 1)
                    failMessage = "No errors!";
                else if (errorList.Length > 1)
                    failMessage = "Not found in " + errorList.Length + " error message(s).";
                else
                    failMessage = errorList[0];

                Assert.AreEqual(errorMessage, failMessage);
            }

            return found;
        }

        public bool CheckErrorMessage(string[] expErrorList, bool doVerify)
        {
            bool checkPass = true;
            string[] actErrorList = GetErrorMessages();

            if (expErrorList.Length != actErrorList.Length)
            {
                checkPass = false;
                string forExpected = expErrorList.Length + " error(s)";
                string forActual = actErrorList.Length + " error(s)";
                Assert.AreEqual(forExpected, forActual);
            }
            else
            {
                for (int i = 0; i < expErrorList.Length; i++)
                {
                    if (expErrorList[i] != actErrorList[i])
                    {
                        checkPass = false;
                        string forExpected = "For error " + i + ": " + expErrorList[i];
                        string forActual = "For error " + i + ": " + actErrorList[i];
                        Assert.AreEqual(forExpected, forActual);
                    }
                }
            }

            return checkPass;
        }

        public string[] GetErrorMessages()
        {
            string[] errorList = new string[0];

            if (HasErrors())
            {
                int count = Convert.ToInt32(WebDriverUtility.DefaultProvider.GetXPathCountByXPath(ErrorLocator + "/li"));
                string oneErrorFormat = ErrorLocator + "/li[{0}]";
                errorList = new string[count];

                for (int i = 1; i <= count; i++)
                {
                    errorList[i - 1] = WebDriverUtility.DefaultProvider.GetText(string.Format(oneErrorFormat, i), LocateBy.XPath);
                }
            }

            return errorList;
        }

        public List<string> GetErrorMessagesList()
        {
            List<string> errorMessagesList = new List<string>();

            if (this.HasErrors())
            {
                int count = Convert.ToInt32(WebDriverUtility.DefaultProvider.GetXPathCountByXPath(ErrorLocator + "/li"));
                string oneErrorFormat = ErrorLocator + "/li[{0}]";
                errorMessagesList = new List<string>(count);

                for (int i = 1; i <= count; i++)
                {
                    errorMessagesList.Add(WebDriverUtility.DefaultProvider.GetText(string.Format(oneErrorFormat, i), LocateBy.XPath));
                }
            }

            return errorMessagesList;
        }

        /// <summary>
        /// Convert the error messages List into a string
        /// </summary>
        /// <returns></returns>
        private string GetErrorMessageString(List<string> errorMessageList)
        {
            StringBuilder errorMessage = new StringBuilder();

            foreach (string message in errorMessageList)
            {
                errorMessage.Append("'");
                errorMessage.Append(message);
                errorMessage.Append("'");
                errorMessage.Append("\n");
            }

            return errorMessage.ToString();
        }

        public bool HasErrors()
        {
            return WebDriverUtility.DefaultProvider.GetAttribute(ErrorDIVLocator, "@style", LocateBy.XPath) != "display:none;";
        }

        public void VerifyHasErrors(bool hasErrors)
        {
            VerifyTool.VerifyValue(hasErrors, HasErrors(), "Has errors: {0}");
        }

        #endregion
    }
}
