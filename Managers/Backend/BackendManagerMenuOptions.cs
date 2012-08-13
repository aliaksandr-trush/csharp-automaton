namespace RegOnline.RegressionTest.Managers.Backend
{
    using System;
    using System.Linq;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Attributes;
    using RegOnline.RegressionTest.DataAccess;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using System.Text.RegularExpressions;

    public partial class BackendManager : ManagerBase
    {
        public const string CancelledOnLocator = "//table[@id='attendeeData']/preceding-sibling::table//font";

        public enum MoreOption
        {
            [StringValue("Profile")]
            Profile,

            [StringValue("Add this attendee to Group")]
            AddThisAttendeeToGroup,

            [StringValue("Delete this attendee")]
            DeleteThisAttendee,

            [StringValue("Create new group by adding a new attendee")]
            CreateNewGroupByAddingANewAttendee,

            [StringValue("Print this page")]
            PrintThisPage,

            [StringValue("Transfer")]
            Transfer
        }

        public void ClickMenuOptionUnderMore(MoreOption option)
        {
            string pageSource = UIUtilityProvider.UIHelper.GetPageSource();
            string js = string.Empty;
            
            switch (option)
            {
                case MoreOption.Profile:
                    Regex regex = new Regex(@"LoadProfile\('[^)]+',\d+,'[^)]+'\);", RegexOptions.IgnoreCase);
                    MatchCollection matchCollection = regex.Matches(pageSource);
                    UIUtilityProvider.UIHelper.ExecuteJavaScript(matchCollection[0].Value);
                    break;

                case MoreOption.AddThisAttendeeToGroup:
                    regex = new Regex(@"LoadGrouping\('[^)]+',\d+,\d+\)");
                    matchCollection = regex.Matches(pageSource);
                    UIUtilityProvider.UIHelper.ExecuteJavaScript(matchCollection[0].Value);
                    break;

                case MoreOption.DeleteThisAttendee:
                    UIUtilityProvider.UIHelper.ExecuteJavaScript("DeleteRegistrant()");
                    break;

                case MoreOption.CreateNewGroupByAddingANewAttendee:
                    UIUtilityProvider.UIHelper.ExecuteJavaScript("AddNewAttendee('AddNewAttendee');");
                    break;

                case MoreOption.PrintThisPage:
                    UIUtilityProvider.UIHelper.ExecuteJavaScript("printTable('attendeeData','')");
                    break;

                case MoreOption.Transfer:
                    regex = new Regex(@"TransferRegistration\('\d+','[^)]+'\);");
                    matchCollection = regex.Matches(pageSource);
                    UIUtilityProvider.UIHelper.ExecuteJavaScript(matchCollection[0].Value);
                    break;

                default:
                    break;
            }
        }

        public void VerifyMenuOptionPresent(MoreOption option, bool present)
        {
            // Click 'More'
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(@"More>>", LocateBy.LinkText);

            Utilities.Utility.ThreadSleep(2);

            VerifyTool.VerifyValue(
                present,
                UIUtilityProvider.UIHelper.IsElementPresent(StringEnum.GetStringValue(option), LocateBy.LinkText),
                "More option '" + StringEnum.GetStringValue(option) + "' is present: {0}");
        }

        [Step]
        public void ResendConfirmation()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("Resend Confirmation", LocateBy.LinkText);
            UIUtilityProvider.UIHelper.SelectWindowByName("Email");
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("submit", LocateBy.Name);
            Utility.ThreadSleep(3);
            this.SelectAttendeeInfoWindow();
            //UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        [Verify]
        public void GenerateInvoiceAndVerify(int registrationId)
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("Generate Invoice", LocateBy.LinkText);
            UIUtilityProvider.UIHelper.SelectWindowByName("Invoice");

            // Verify we are on the correct page
            if (!UIUtilityProvider.UIHelper.UrlContainsAbsolutePath("register/invoice.aspx"))
            {
                UIUtilityProvider.UIHelper.FailTest("Not on invoice page!");
            }

            // Verify registration id
            string registrationIdLocator = "//div[@class='primaryContentArea']//*[text()='Registration ID:']/following-sibling::*";

            VerifyTool.VerifyValue(
                registrationId,
                Convert.ToInt32(UIUtilityProvider.UIHelper.GetText(registrationIdLocator, LocateBy.XPath)), 
                "Registration Id on invoice: {0}");

            UIUtilityProvider.UIHelper.CloseWindow();
            Utility.ThreadSleep(3);
            this.SelectAttendeeInfoWindow();
        }

        [Verify]
        public void CancelRegistrationAndVerify()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("Cancel Registration", LocateBy.LinkText);
            UIUtilityProvider.UIHelper.SelectTopWindow();
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("Action", LocateBy.Name);
            DateTime expectedCancelledDateTime = DateTime.Now;
            Utility.ThreadSleep(1);
            this.SelectAttendeeInfoWindow();
            UIUtilityProvider.UIHelper.WaitForElementPresent(CancelledOnLocator, LocateBy.XPath);
            
            VerifyTool.VerifyValue(
                "This registration was cancelled on " + string.Format("{0:dd-MMM-yyyy hh:mm tt}.", expectedCancelledDateTime),
                UIUtilityProvider.UIHelper.GetText(CancelledOnLocator, LocateBy.XPath), 
                "Registration cancelled message: {0}");

        }

        [Verify]
        public void UndoCancelRegistrationAndVerify()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("Undo Cancellation", LocateBy.LinkText);
            UIUtilityProvider.UIHelper.SelectTopWindow();
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("Action", LocateBy.Name);
            DateTime expectedCancelledDateTime = DateTime.Now;
            this.SelectAttendeeInfoWindow();
            UIUtilityProvider.UIHelper.WaitForPageToLoad();

            if (UIUtilityProvider.UIHelper.IsTextPresent("This registration was cancelled on "))
            {
                UIUtilityProvider.UIHelper.FailTest("The registration cancelled message is still there!");
            }
        }

        [Verify]
        public void GenerateRegDetailsAndVerify(int registrationId)
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("Generate Reg Details", LocateBy.LinkText);
            UIUtilityProvider.UIHelper.SelectWindowByName("PrintMyReg");

            VerifyTool.VerifyValue(
                registrationId.ToString(),
                UIUtilityProvider.UIHelper.GetText("//th[text()='Registration ID:']/following-sibling::td", LocateBy.XPath), 
                "Registration Id on reg details: {0}");

            UIUtilityProvider.UIHelper.CloseWindow();
            Utility.ThreadSleep(1);
            this.SelectAttendeeInfoWindow();
        }

        public void PrintBadgeAndVerify()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("Print Badge", LocateBy.LinkText);
            UIUtilityProvider.UIHelper.SelectWindowByName("PersonalBadges");

            // The page will be redirected to activereports/default.aspx first, loading, then redirected to badge
            // Wait for 2 minutes as the instruction on the page
            // If firefox has no pdf plugin, the browser will open a download window for the pdf file, 
            // rather than showing it directly in currect browser window
            UIUtilityProvider.UIHelper.WaitForPageToLoad(TimeSpan.FromMinutes(2));

            if (!UIUtilityProvider.UIHelper.UrlContainsAbsolutePath("RegOnlineBadges/BadgeReport.aspx"))
            {
                UIUtilityProvider.UIHelper.FailTest("Not on badge page!");
            }

            UIUtilityProvider.UIHelper.CloseWindow();
            Utility.ThreadSleep(1);
            this.SelectAttendeeInfoWindow();
        }

        [Verify]
        public void CheckInAndVerify()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("Check-In", LocateBy.LinkText);

            // There will be a selenium exception if I call WaitForPageToLoad() after clicking Check-in! WHY?!
            Utility.ThreadSleep(3);
            
            VerifyFieldValue(
                PersonalInfoViewField.Status, 
                StringEnum.GetStringValue(Report.ReportManager.AttendeeStatus.Attended));
        }

        [Verify]
        public void UndoCheckinAndVerify()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("Undo Check-in", LocateBy.LinkText);
            Utility.ThreadSleep(3);

            VerifyFieldValue(
                PersonalInfoViewField.Status,
                StringEnum.GetStringValue(Report.ReportManager.AttendeeStatus.Confirmed));
        }

        [Verify]
        public void OpenProfileAndVerify(string emailAddress, string name)
        {
            this.ClickMenuOptionUnderMore(MoreOption.Profile);
            Utility.ThreadSleep(3);
            UIUtilityProvider.UIHelper.SelectWindowByName("Profile");
            VerifyTool.VerifyValue(name + " (" + emailAddress + ")", UIUtilityProvider.UIHelper.GetText("lblProfleTitle", LocateBy.Id), "Profile title: {0}");
            UIUtilityProvider.UIHelper.CloseWindow();
            Utility.ThreadSleep(1);
            this.SelectAttendeeInfoWindow();
        }

        [Verify]
        public void AddToGroupAndVerify()
        {
            this.ClickMenuOptionUnderMore(MoreOption.AddThisAttendeeToGroup);
            UIUtilityProvider.UIHelper.SelectWindowByName("Grouping");
            UIUtilityProvider.UIHelper.Type("GroupId", "11367058", LocateBy.Name);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("submit", LocateBy.Name);
            Utility.ThreadSleep(1.5);
            this.SelectAttendeeInfoWindow();
            UIUtilityProvider.UIHelper.IsTextPresent("This Attendee is part of a group");
        }

        /// <summary>
        /// Verify RegType Exist in event In TransferAttendee page
        /// </summary>
        /// <param name="eventIdToTransferTo"></param>
        /// <param name="nameForEventToTransferTo"></param>
        /// <param name="regTypeName"></param>
        /// <param name="exist"></param>
        public void VerifyRegTypeExistInTransferAttendee(int eventIdToTransferTo, string nameForEventToTransferTo, string regTypeName, bool exist)
        {
            this.ClickMenuOptionUnderMore(MoreOption.Transfer);
            UIUtilityProvider.UIHelper.SelectWindowByName("RegTransfer");
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
            UIUtilityProvider.UIHelper.SelectWithText("ddEvent", nameForEventToTransferTo + " (" + eventIdToTransferTo + ")", LocateBy.Id);

            UIUtilityProvider.UIHelper.WaitForPageToLoad();

            Assert.AreEqual(UIUtilityProvider.UIHelper.IsOptionExistInSelect("ddRegType", regTypeName, LocateBy.Id), exist);
        }

        /// <summary>
        /// Transfer attendee to another event
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="nameForEventToTransferTo"></param>
        /// <returns>New registration id</returns>
        public int TransferAttendee(int eventIdToTransferTo, string nameForEventToTransferTo)
        {
            this.ClickMenuOptionUnderMore(MoreOption.Transfer);
            UIUtilityProvider.UIHelper.SelectWindowByName("RegTransfer");
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
            UIUtilityProvider.UIHelper.SelectWithText("ddEvent", nameForEventToTransferTo + " (" + eventIdToTransferTo + ")", LocateBy.Id);

            UIUtilityProvider.UIHelper.WaitForPageToLoad();
            
            // Click 'Next'
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("btnStart", LocateBy.Id);

            // Get new registration id
            int newRegistrationId = Convert.ToInt32(UIUtilityProvider.UIHelper.GetText("lblNewRegisterId", LocateBy.Id));

            // Click 'Next'
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("btnNextCompare", LocateBy.Id);

            // Click 'Next'
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("btnNextPayment", LocateBy.Id);

            // Click 'Finish', then transfering window will be closed
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("btnFinish", LocateBy.Id);

            return newRegistrationId;
        }
    }
}
