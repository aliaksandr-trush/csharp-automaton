namespace RegOnline.RegressionTest.Managers.Builder
{
    using RegOnline.RegressionTest.Utilities;
    //using RegOnline.RegressionTest.Fixtures;
    using RegOnline.RegressionTest.UIUtility;

    public class MembershipIntegrationManager : ManagerBase
    {
        #region Constants
        private const string DialogID = "dialog2";
        private const string MembershipIntegrationLinkLocator = "ctl00_cphDialog_btnIntegrateWithMembership";
        private const string MembershipNameDropDownLocator = "ctl00_cphDialog_ddlMemberships";
        private const string MembershipTypeTableLocator = "ctl00_cphDialog_tblTypes";
        private const string MembershipStatusTableLocator = "ctl00_cphDialog_tblStatuses";

        private const string MembershipTypeCheckboxLocatorFormat =
                    "//table[@id='" + MembershipTypeTableLocator + "']//span[text()='{0}']/../../td[2]/input";

        private const string SelectAllMembershipTypeCheckboxLocator = "ctl00_cphDialog_selectAllTypes";

        private const string MembershipStatusCheckboxLocatorFormat =
                        "//table[@id='" + MembershipStatusTableLocator + "']//td[contains(text(), '{0}')]/../td[2]/input";

        private const string SelectAllMembershipStatusCheckboxLocator = "ctl00_cphDialog_selectAllStatuses";
        #endregion

        #region Enum
        public enum MembershipStatus
        {
            Confirmed,
            Pending,
            Approved,
            Waitlisted,
            Declined,
            Incomplete,
            Cancelled,
            Attended
        }
        #endregion

        #region Public methods
        public void ClickIntegrationLink()
        {
            //Click 'Integrate with membership'
            UIUtil.DefaultProvider.WaitForDisplayAndClick(MembershipIntegrationLinkLocator, LocateBy.Id);
            Utility.ThreadSleep(1);
            UIUtil.DefaultProvider.WaitForAJAXRequest();
            UIUtil.DefaultProvider.SelectUpperFrame();

            try
            {
                UIUtil.DefaultProvider.SelectPopUpFrameByName(DialogID);
            }
            catch
            {
                //Ignore any error
            }
        }

        public void Delete()
        {
            //Delete all membership types and membership statuses, then the integration is deleted
            SetMembershipTypeSelection(false);
            SetMembershipStatusSelection(false);
        }

        private void SelectThisFrame()
        {
            try
            {
                UIUtil.DefaultProvider.SelectPopUpFrameByName(DialogID);
            }
            catch
            {
                // Ignore
            }
        }

        public void SaveAndStay()
        {
            SelectThisFrame();
            UIUtil.DefaultProvider.ClickSaveAndStay();
            Utility.ThreadSleep(1);
        }

        public void SaveAndClose()
        {
            SelectThisFrame();
            UIUtil.DefaultProvider.ClickSaveAndClose();
            Utility.ThreadSleep(1);
            UIUtil.DefaultProvider.SelectOriginalWindow();
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }

        public void Cancel()
        {
            SelectThisFrame();
            UIUtil.DefaultProvider.ClickCancel();
            Utility.ThreadSleep(1);
            UIUtil.DefaultProvider.SelectOriginalWindow();
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }

        public void SelectMembership(int membershipEventID, string membershipEventName)
        {
            // Select a membership
            UIUtil.DefaultProvider.SelectWithText(MembershipNameDropDownLocator, membershipEventID.ToString() + " - " + membershipEventName, LocateBy.Id);
        }

        // Use a boolean variable to decide whether to select all
        public void SetMembershipTypeSelection(bool checkEach, params string[] membershipTypeNames)
        {
            UIUtil.DefaultProvider.WaitForElementPresent(MembershipTypeTableLocator, LocateBy.Id);

            foreach (string membershipTypeName in membershipTypeNames)
            {
                // Get the checkbox locator according to the membership type table locator and membership type name
                string membershipTypeCheckboxLocator =
                    string.Format(MembershipTypeCheckboxLocatorFormat, membershipTypeName);

                UIUtil.DefaultProvider.SetCheckbox(membershipTypeCheckboxLocator, checkEach, LocateBy.XPath);
            }
        }

        // Check or uncheck the 'CheckAll' checkbox according to the parameter isCheckAll
        public void SetMembershipTypeSelection(bool checkAll)
        {
            UIUtil.DefaultProvider.SetCheckbox(SelectAllMembershipTypeCheckboxLocator, true, LocateBy.Id);

            // Check the checkbox to select all first, uncheck it, then all membership types are unchecked
            if (checkAll == false)
            {
                UIUtil.DefaultProvider.SetCheckbox(SelectAllMembershipTypeCheckboxLocator, false, LocateBy.Id);
            }
        }

        public void SetMembershipStatusSelection(bool checkEach, params MembershipStatus[] membershipStatuses)
        {
            UIUtil.DefaultProvider.WaitForElementPresent(MembershipStatusTableLocator, LocateBy.Id);

            foreach (MembershipStatus membershipStatus in membershipStatuses)
            {
                // Get the checkbox locator according to the membership status table locator and status
                string membershipStatusCheckboxLocator =
                        string.Format(MembershipStatusCheckboxLocatorFormat, membershipStatus.ToString());

                UIUtil.DefaultProvider.SetCheckbox(membershipStatusCheckboxLocator, checkEach, LocateBy.XPath);
            }
        }

        public void SetMembershipStatusSelection(bool checkAll)
        {
            UIUtil.DefaultProvider.SetCheckbox(SelectAllMembershipStatusCheckboxLocator, true, LocateBy.Id);

            if (checkAll == false)
            {
                UIUtil.DefaultProvider.SetCheckbox(SelectAllMembershipStatusCheckboxLocator, false, LocateBy.Id);
            }
        }

        #region Verify membership integration
        public void VerifyIntegrationImage(bool isIntegrated)
        {
            // Verify whether the image is present or not
            UIUtil.DefaultProvider.WaitForElementPresent(MembershipIntegrationLinkLocator, LocateBy.Id);
            
            VerifyTool.VerifyValue(
                isIntegrated,
                UIUtil.DefaultProvider.IsElementPresent("//img[@id='ctl00_cphDialog_integratedImage']", LocateBy.XPath),
                "The membership integration image is present : {0}");
        }

        public void VerifyMembershipEventIdAndName(int membershipEventID, string membershipEventName)
        {
            UIUtil.DefaultProvider.WaitForElementPresent(MembershipNameDropDownLocator, LocateBy.Id);

            VerifyTool.VerifyValue(
                membershipEventID.ToString() + " - " + membershipEventName,
                UIUtil.DefaultProvider.GetSelectedLabel(MembershipNameDropDownLocator, LocateBy.Id),
                "The selected membership event to integrate with : {0}");
        }

        public void VerifyMembershipTypeSelection(bool isCheckAll, params string[] membershipTypeNames)
        {
            foreach (string membershipTypeName in membershipTypeNames)
            {
                string membershipTypeCheckboxLocator =
                    string.Format(MembershipTypeCheckboxLocatorFormat, membershipTypeName);

                UIUtil.DefaultProvider.WaitForElementPresent(membershipTypeCheckboxLocator, LocateBy.XPath);

                VerifyTool.VerifyValue(
                    isCheckAll,
                    UIUtil.DefaultProvider.GetValue(membershipTypeCheckboxLocator, LocateBy.XPath) == "on",
                    "The checkbox for membership type '" + membershipTypeName + "' is checked : {0}");
            }
        }

        public void VerifyMembershipTypeSelection(bool isCheckAll)
        {
            UIUtil.DefaultProvider.WaitForElementPresent(SelectAllMembershipTypeCheckboxLocator, LocateBy.Id);

            VerifyTool.VerifyValue(
                isCheckAll,
                UIUtil.DefaultProvider.GetValue(SelectAllMembershipTypeCheckboxLocator, LocateBy.Id) == "on",
                "All membership types are checked : {0}");
        }

        public void VerifyMembershipStatusSelection(bool isCheckAll, params MembershipStatus[] membershipStatuses)
        {
            foreach (MembershipStatus membershipStatus in membershipStatuses)
            {
                string membershipStatusCheckboxLocator =
                    string.Format(MembershipStatusCheckboxLocatorFormat, membershipStatus);

                UIUtil.DefaultProvider.WaitForElementPresent(membershipStatusCheckboxLocator, LocateBy.XPath);

                VerifyTool.VerifyValue(
                    isCheckAll,
                    UIUtil.DefaultProvider.GetValue(membershipStatusCheckboxLocator, LocateBy.XPath) == "on",
                    "The checkbox for membership status '" + membershipStatus + "' is checked : {0}");
            }
        }

        public void VerifyMembershipStatusSelection(bool isCheckAll)
        {
            UIUtil.DefaultProvider.WaitForElementPresent(SelectAllMembershipStatusCheckboxLocator, LocateBy.Id);

            VerifyTool.VerifyValue(
                isCheckAll,
                UIUtil.DefaultProvider.GetValue(SelectAllMembershipStatusCheckboxLocator, LocateBy.Id) == "on",
                "All membership statuses are checked : {0}");
        }
        #endregion

        #endregion
    }
}
