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
            string pageSource = UIUtil.DefaultProvider.GetPageSource();
            string js = string.Empty;
            
            switch (option)
            {
                case MoreOption.Profile:
                    Regex regex = new Regex(@"LoadProfile\('[^)]+',\d+,'[^)]+'\);", RegexOptions.IgnoreCase);
                    MatchCollection matchCollection = regex.Matches(pageSource);
                    UIUtil.DefaultProvider.ExecuteJavaScript(matchCollection[0].Value);
                    break;

                case MoreOption.AddThisAttendeeToGroup:
                    regex = new Regex(@"LoadGrouping\('[^)]+',\d+,\d+\)");
                    matchCollection = regex.Matches(pageSource);
                    UIUtil.DefaultProvider.ExecuteJavaScript(matchCollection[0].Value);
                    break;

                case MoreOption.DeleteThisAttendee:
                    UIUtil.DefaultProvider.ExecuteJavaScript("DeleteRegistrant()");
                    break;

                case MoreOption.CreateNewGroupByAddingANewAttendee:
                    UIUtil.DefaultProvider.ExecuteJavaScript("AddNewAttendee('AddNewAttendee');");
                    break;

                case MoreOption.PrintThisPage:
                    UIUtil.DefaultProvider.ExecuteJavaScript("printTable('attendeeData','')");
                    break;

                case MoreOption.Transfer:
                    regex = new Regex(@"TransferRegistration\('\d+','[^)]+'\);");
                    matchCollection = regex.Matches(pageSource);
                    UIUtil.DefaultProvider.ExecuteJavaScript(matchCollection[0].Value);
                    break;

                default:
                    break;
            }
        }

        public void VerifyMenuOptionPresent(MoreOption option, bool present)
        {
            // Click 'More'
            UIUtil.DefaultProvider.WaitForDisplayAndClick(@"More>>", LocateBy.LinkText);

            Utilities.Utility.ThreadSleep(2);

            VerifyTool.VerifyValue(
                present,
                UIUtil.DefaultProvider.IsElementPresent(StringEnum.GetStringValue(option), LocateBy.LinkText),
                "More option '" + StringEnum.GetStringValue(option) + "' is present: {0}");
        }

        [Step]
        public void ResendConfirmation()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick("Resend Confirmation", LocateBy.LinkText);
            Utility.ThreadSleep(3);
            UIUtil.DefaultProvider.SelectWindowByName("Email");
            UIUtil.DefaultProvider.WaitForDisplayAndClick("submit", LocateBy.Name);
            Utility.ThreadSleep(3);
            this.SelectAttendeeInfoWindow();
            //UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        [Verify]
        public void GenerateInvoiceAndVerify(int registrationId)
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick("Generate Invoice", LocateBy.LinkText);
            Utility.ThreadSleep(3);
            UIUtil.DefaultProvider.SelectWindowByName("Invoice");

            // Verify we are on the correct page
            if (!UIUtil.DefaultProvider.UrlContainsAbsolutePath("register/invoice.aspx"))
            {
                UIUtil.DefaultProvider.FailTest("Not on invoice page!");
            }

            // Verify registration id
            string registrationIdLocator = "//div[@class='primaryContentArea']//*[text()='Registration ID:']/following-sibling::*";

            VerifyTool.VerifyValue(
                registrationId,
                Convert.ToInt32(UIUtil.DefaultProvider.GetText(registrationIdLocator, LocateBy.XPath)), 
                "Registration Id on invoice: {0}");

            UIUtil.DefaultProvider.CloseWindow();
            Utility.ThreadSleep(3);
            this.SelectAttendeeInfoWindow();
        }

        [Verify]
        public void CancelRegistrationAndVerify()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick("Cancel Registration", LocateBy.LinkText);
            Utility.ThreadSleep(3);
            UIUtil.DefaultProvider.SelectTopWindow();
            UIUtil.DefaultProvider.WaitForDisplayAndClick("Action", LocateBy.Name);
            Utility.ThreadSleep(3);
            DateTime expectedCancelledDateTime = DateTime.Now;
            Utility.ThreadSleep(1);
            this.SelectAttendeeInfoWindow();
            UIUtil.DefaultProvider.WaitForElementPresent(CancelledOnLocator, LocateBy.XPath);
            
            VerifyTool.VerifyValue(
                "This registration was cancelled on " + string.Format("{0:dd-MMM-yyyy hh:mm tt}.", expectedCancelledDateTime),
                UIUtil.DefaultProvider.GetText(CancelledOnLocator, LocateBy.XPath), 
                "Registration cancelled message: {0}");

        }

        [Verify]
        public void UndoCancelRegistrationAndVerify()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick("Undo Cancellation", LocateBy.LinkText);
            Utility.ThreadSleep(3);
            UIUtil.DefaultProvider.SelectTopWindow();
            UIUtil.DefaultProvider.WaitForDisplayAndClick("Action", LocateBy.Name);
            Utility.ThreadSleep(3);
            DateTime expectedCancelledDateTime = DateTime.Now;
            this.SelectAttendeeInfoWindow();
            UIUtil.DefaultProvider.WaitForPageToLoad();

            if (UIUtil.DefaultProvider.IsTextPresent("This registration was cancelled on "))
            {
                UIUtil.DefaultProvider.FailTest("The registration cancelled message is still there!");
            }
        }

        [Verify]
        public void GenerateRegDetailsAndVerify(int registrationId)
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick("Generate Reg Details", LocateBy.LinkText);
            Utility.ThreadSleep(3);
            UIUtil.DefaultProvider.SelectWindowByName("PrintMyReg");

            VerifyTool.VerifyValue(
                registrationId.ToString(),
                UIUtil.DefaultProvider.GetText("//th[text()='Registration ID:']/following-sibling::td", LocateBy.XPath), 
                "Registration Id on reg details: {0}");

            UIUtil.DefaultProvider.CloseWindow();
            Utility.ThreadSleep(1);
            this.SelectAttendeeInfoWindow();
        }

        public void PrintBadgeAndVerify()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick("Print Badge", LocateBy.LinkText);
            Utility.ThreadSleep(3);
            UIUtil.DefaultProvider.SelectWindowByName("PersonalBadges");

            // The page will be redirected to activereports/default.aspx first, loading, then redirected to badge
            // Wait for 2 minutes as the instruction on the page
            // If firefox has no pdf plugin, the browser will open a download window for the pdf file, 
            // rather than showing it directly in currect browser window
            UIUtil.DefaultProvider.WaitForPageToLoad(TimeSpan.FromMinutes(2));

            if (!UIUtil.DefaultProvider.UrlContainsAbsolutePath("RegOnlineBadges/BadgeReport.aspx"))
            {
                UIUtil.DefaultProvider.FailTest("Not on badge page!");
            }

            UIUtil.DefaultProvider.CloseWindow();
            Utility.ThreadSleep(1);
            this.SelectAttendeeInfoWindow();
        }

        [Verify]
        public void CheckInAndVerify()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick("Check-In", LocateBy.LinkText);

            // There will be a selenium exception if I call WaitForPageToLoad() after clicking Check-in! WHY?!
            Utility.ThreadSleep(3);
            
            VerifyFieldValue(
                PersonalInfoViewField.Status, 
                StringEnum.GetStringValue(Report.ReportManager.AttendeeStatus.Attended));
        }

        [Verify]
        public void UndoCheckinAndVerify()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick("Undo Check-in", LocateBy.LinkText);
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
            UIUtil.DefaultProvider.SelectWindowByName("Profile");
            VerifyTool.VerifyValue(name + " (" + emailAddress + ")", UIUtil.DefaultProvider.GetText("lblProfleTitle", LocateBy.Id), "Profile title: {0}");
            UIUtil.DefaultProvider.CloseWindow();
            Utility.ThreadSleep(1);
            this.SelectAttendeeInfoWindow();
        }

        [Verify]
        public void AddToGroupAndVerify()
        {
            this.ClickMenuOptionUnderMore(MoreOption.AddThisAttendeeToGroup);
            UIUtil.DefaultProvider.SelectWindowByName("Grouping");
            UIUtil.DefaultProvider.Type("GroupId", "11367058", LocateBy.Name);
            UIUtil.DefaultProvider.WaitForDisplayAndClick("submit", LocateBy.Name);
            Utility.ThreadSleep(1.5);
            this.SelectAttendeeInfoWindow();
            UIUtil.DefaultProvider.IsTextPresent("This Attendee is part of a group");
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
            UIUtil.DefaultProvider.SelectWindowByName("RegTransfer");
            UIUtil.DefaultProvider.WaitForPageToLoad();
            UIUtil.DefaultProvider.SelectWithText("ddEvent", nameForEventToTransferTo + " (" + eventIdToTransferTo + ")", LocateBy.Id);

            UIUtil.DefaultProvider.WaitForPageToLoad();

            Assert.AreEqual(UIUtil.DefaultProvider.IsOptionExistInSelect("ddRegType", regTypeName, LocateBy.Id), exist);
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
            UIUtil.DefaultProvider.SelectWindowByName("RegTransfer");
            UIUtil.DefaultProvider.WaitForPageToLoad();
            UIUtil.DefaultProvider.SelectWithText("ddEvent", nameForEventToTransferTo + " (" + eventIdToTransferTo + ")", LocateBy.Id);

            UIUtil.DefaultProvider.WaitForPageToLoad();
            
            // Click 'Next'
            UIUtil.DefaultProvider.WaitForDisplayAndClick("btnStart", LocateBy.Id);
            UIUtil.DefaultProvider.WaitForPageToLoad();

            // Get new registration id
            int newRegistrationId = Convert.ToInt32(UIUtil.DefaultProvider.GetText("lblNewRegisterId", LocateBy.Id));

            // Click 'Next'
            UIUtil.DefaultProvider.WaitForPageToLoad();
            UIUtil.DefaultProvider.WaitForDisplayAndClick("btnNextCompare", LocateBy.Id);

            // Click 'Next'
            UIUtil.DefaultProvider.WaitForPageToLoad();
            UIUtil.DefaultProvider.WaitForDisplayAndClick("btnNextPayment", LocateBy.Id);

            // Click 'Finish', then transfering window will be closed
            UIUtil.DefaultProvider.WaitForPageToLoad();
            UIUtil.DefaultProvider.WaitForDisplayAndClick("btnFinish", LocateBy.Id);

            return newRegistrationId;
        }
    }
}
