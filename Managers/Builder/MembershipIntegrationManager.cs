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
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(MembershipIntegrationLinkLocator, LocateBy.Id);
            Utility.ThreadSleep(1);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            UIUtilityProvider.UIHelper.SelectUpperFrame();

            try
            {
                UIUtilityProvider.UIHelper.SelectPopUpFrameByName(DialogID);
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
                UIUtilityProvider.UIHelper.SelectPopUpFrameByName(DialogID);
            }
            catch
            {
                // Ignore
            }
        }

        public void SaveAndStay()
        {
            SelectThisFrame();
            UIUtilityProvider.UIHelper.ClickSaveAndStay();
            Utility.ThreadSleep(1);
        }

        public void SaveAndClose()
        {
            SelectThisFrame();
            UIUtilityProvider.UIHelper.ClickSaveAndClose();
            Utility.ThreadSleep(1);
            UIUtilityProvider.UIHelper.SelectOriginalWindow();
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void Cancel()
        {
            SelectThisFrame();
            UIUtilityProvider.UIHelper.ClickCancel();
            Utility.ThreadSleep(1);
            UIUtilityProvider.UIHelper.SelectOriginalWindow();
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void SelectMembership(int membershipEventID, string membershipEventName)
        {
            // Select a membership
            UIUtilityProvider.UIHelper.SelectWithText(MembershipNameDropDownLocator, membershipEventID.ToString() + " - " + membershipEventName, LocateBy.Id);
        }

        // Use a boolean variable to decide whether to select all
        public void SetMembershipTypeSelection(bool checkEach, params string[] membershipTypeNames)
        {
            UIUtilityProvider.UIHelper.WaitForElementPresent(MembershipTypeTableLocator, LocateBy.Id);

            foreach (string membershipTypeName in membershipTypeNames)
            {
                // Get the checkbox locator according to the membership type table locator and membership type name
                string membershipTypeCheckboxLocator =
                    string.Format(MembershipTypeCheckboxLocatorFormat, membershipTypeName);

                UIUtilityProvider.UIHelper.SetCheckbox(membershipTypeCheckboxLocator, checkEach, LocateBy.XPath);
            }
        }

        // Check or uncheck the 'CheckAll' checkbox according to the parameter isCheckAll
        public void SetMembershipTypeSelection(bool checkAll)
        {
            UIUtilityProvider.UIHelper.SetCheckbox(SelectAllMembershipTypeCheckboxLocator, true, LocateBy.Id);

            // Check the checkbox to select all first, uncheck it, then all membership types are unchecked
            if (checkAll == false)
            {
                UIUtilityProvider.UIHelper.SetCheckbox(SelectAllMembershipTypeCheckboxLocator, false, LocateBy.Id);
            }
        }

        public void SetMembershipStatusSelection(bool checkEach, params MembershipStatus[] membershipStatuses)
        {
            UIUtilityProvider.UIHelper.WaitForElementPresent(MembershipStatusTableLocator, LocateBy.Id);

            foreach (MembershipStatus membershipStatus in membershipStatuses)
            {
                // Get the checkbox locator according to the membership status table locator and status
                string membershipStatusCheckboxLocator =
                        string.Format(MembershipStatusCheckboxLocatorFormat, membershipStatus.ToString());

                UIUtilityProvider.UIHelper.SetCheckbox(membershipStatusCheckboxLocator, checkEach, LocateBy.XPath);
            }
        }

        public void SetMembershipStatusSelection(bool checkAll)
        {
            UIUtilityProvider.UIHelper.SetCheckbox(SelectAllMembershipStatusCheckboxLocator, true, LocateBy.Id);

            if (checkAll == false)
            {
                UIUtilityProvider.UIHelper.SetCheckbox(SelectAllMembershipStatusCheckboxLocator, false, LocateBy.Id);
            }
        }

        #region Verify membership integration
        public void VerifyIntegrationImage(bool isIntegrated)
        {
            // Verify whether the image is present or not
            UIUtilityProvider.UIHelper.WaitForElementPresent(MembershipIntegrationLinkLocator, LocateBy.Id);
            
            VerifyTool.VerifyValue(
                isIntegrated,
                UIUtilityProvider.UIHelper.IsElementPresent("//img[@id='ctl00_cphDialog_integratedImage']", LocateBy.XPath),
                "The membership integration image is present : {0}");
        }

        public void VerifyMembershipEventIdAndName(int membershipEventID, string membershipEventName)
        {
            UIUtilityProvider.UIHelper.WaitForElementPresent(MembershipNameDropDownLocator, LocateBy.Id);

            VerifyTool.VerifyValue(
                membershipEventID.ToString() + " - " + membershipEventName,
                UIUtilityProvider.UIHelper.GetSelectedLabel(MembershipNameDropDownLocator, LocateBy.Id),
                "The selected membership event to integrate with : {0}");
        }

        public void VerifyMembershipTypeSelection(bool isCheckAll, params string[] membershipTypeNames)
        {
            foreach (string membershipTypeName in membershipTypeNames)
            {
                string membershipTypeCheckboxLocator =
                    string.Format(MembershipTypeCheckboxLocatorFormat, membershipTypeName);

                UIUtilityProvider.UIHelper.WaitForElementPresent(membershipTypeCheckboxLocator, LocateBy.XPath);

                VerifyTool.VerifyValue(
                    isCheckAll,
                    UIUtilityProvider.UIHelper.GetValue(membershipTypeCheckboxLocator, LocateBy.XPath) == "on",
                    "The checkbox for membership type '" + membershipTypeName + "' is checked : {0}");
            }
        }

        public void VerifyMembershipTypeSelection(bool isCheckAll)
        {
            UIUtilityProvider.UIHelper.WaitForElementPresent(SelectAllMembershipTypeCheckboxLocator, LocateBy.Id);

            VerifyTool.VerifyValue(
                isCheckAll,
                UIUtilityProvider.UIHelper.GetValue(SelectAllMembershipTypeCheckboxLocator, LocateBy.Id) == "on",
                "All membership types are checked : {0}");
        }

        public void VerifyMembershipStatusSelection(bool isCheckAll, params MembershipStatus[] membershipStatuses)
        {
            foreach (MembershipStatus membershipStatus in membershipStatuses)
            {
                string membershipStatusCheckboxLocator =
                    string.Format(MembershipStatusCheckboxLocatorFormat, membershipStatus);

                UIUtilityProvider.UIHelper.WaitForElementPresent(membershipStatusCheckboxLocator, LocateBy.XPath);

                VerifyTool.VerifyValue(
                    isCheckAll,
                    UIUtilityProvider.UIHelper.GetValue(membershipStatusCheckboxLocator, LocateBy.XPath) == "on",
                    "The checkbox for membership status '" + membershipStatus + "' is checked : {0}");
            }
        }

        public void VerifyMembershipStatusSelection(bool isCheckAll)
        {
            UIUtilityProvider.UIHelper.WaitForElementPresent(SelectAllMembershipStatusCheckboxLocator, LocateBy.Id);

            VerifyTool.VerifyValue(
                isCheckAll,
                UIUtilityProvider.UIHelper.GetValue(SelectAllMembershipStatusCheckboxLocator, LocateBy.Id) == "on",
                "All membership statuses are checked : {0}");
        }
        #endregion

        #endregion
    }
}
