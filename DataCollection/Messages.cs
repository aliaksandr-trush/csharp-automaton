namespace RegOnline.RegressionTest.DataCollection
{
    public class Messages
    {
        public static class RegisterError
        {
            public const string CodeLimitHasReached = "No more \"{0}\" codes are being accepted for this registrant type. Enter another code or contact your event administrator.";
            public const string InvalidCode = "You have entered an invalid code. ";
            public const string YouMustEnterValidEmailAddress = "You must enter a valid email address.";
            public const string InvalidPassword = "Your login or password is incorrect. Please try again.";
            public const string EmailAddressesDoNotMatch = "The email addresses that you entered do not match. Re-enter your email address.";
            public const string EmailAlreadyUsed = "Our records show that this email address has already been used to register for this event. Each registrant must use a unique email address.";
            public const string ContributionNotInMinAndMax = "Enter an amount between {0} and {1} for \"AGConribution\".";
            public const string DiscountCodeNotFilled = "You must enter a discount code.";
            public const string AgendaCodeLimitReached = "The limit for the discount code you entered has been reached and is no longer being accepted.";
            public const string RequiredCheckBoxNotChecked = "You must select all required check boxes.";
        }

        public static class BuilderError
        {
            public const string AgendaNoMultipleChoice = "You must have at least one item in the multiple choice list when the \"Field Type\" is set to Multiple Choice";
            public const string ServiceEndPointFormatError = "\"Service Endpoint URL\" must start with \"https://\"";
        }
    }
}
