namespace RegOnline.RegressionTest.Managers.Register
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Attributes;

    public partial class RegisterManager : ManagerBase
    {
        #region Update already registered
        [Step]
        public void ClickEditPersonalInformationLink(int index)
        {
            string regTypeHeaderLocator = "//table//th[text()='Type']";
            string personalInformationLinkLocator_Old = string.Format(regTypeHeaderLocator + "/../../tr[{0}]/td[3]/a", index + 1);

            string personalInformationLinkLocatorFormat = "ctl00_cph_grdMembers_ctl{0}_lnkPersInfo";

            string personalInformationLinkLocator = string.Format(
                personalInformationLinkLocatorFormat,
                ((index + 1) < 10 ? "0" : string.Empty) + (index + 1).ToString());

            UIUtil.DefaultProvider.WaitForDisplayAndClick(personalInformationLinkLocator, LocateBy.Id);
            UIUtil.DefaultProvider.WaitForPageToLoad();
        }

        public void ClickEditAgendaLink(int index)
        {
            string agendaLinkLocatorFormat = "ctl00_cph_grdMembers_ctl{0}_lnkAgenda";

            string agendaLinkLocator = string.Format(
                agendaLinkLocatorFormat,
                ((index + 1) < 10 ? "0" : string.Empty) + (index + 1).ToString());

            UIUtil.DefaultProvider.WaitForDisplayAndClick(agendaLinkLocator, LocateBy.Id);
            UIUtil.DefaultProvider.WaitForPageToLoad();
        }

        public void ClickEditLodgingAndTravelLink(int index)
        {
            string agendaLinkLocatorFormat = "ctl00_cph_grdMembers_ctl{0}_lnkTrvLdg";

            string agendaLinkLocator = string.Format(
                agendaLinkLocatorFormat,
                ((index + 1) < 10 ? "0" : string.Empty) + (index + 1).ToString());

            UIUtil.DefaultProvider.WaitForDisplayAndClick(agendaLinkLocator, LocateBy.Id);
            UIUtil.DefaultProvider.WaitForPageToLoad();
        }

        public void ClickSubstituteLink(int index)
        {
            string substituteLinkLocatorFormat = "ctl00_cph_grdMembers_ctl{0}_lnkSubst";
            string substituteLinkLocator = string.Empty;

            substituteLinkLocator = string.Format(
                substituteLinkLocatorFormat,
                ((index + 1) < 10 ? "0" : string.Empty) + (index + 1).ToString());

            UIUtil.DefaultProvider.WaitForDisplayAndClick(substituteLinkLocator, LocateBy.Id);
            UIUtil.DefaultProvider.WaitForPageToLoad();
        }

        public void VerifyHasSubstituteLink(int index, bool isHas)
        {
            Assert.AreEqual(HasSubstituteLink(index), isHas);
        }

        public bool HasSubstituteLink(int index)
        {
            string substituteLinkLocatorFormat = "ctl00_cph_grdMembers_ctl{0}_lnkSubst";
            string substituteLinkLocator = string.Empty;

            substituteLinkLocator = string.Format(
                substituteLinkLocatorFormat,
                ((index + 1) < 10 ? "0" : string.Empty) + (index + 1).ToString());

            return UIUtil.DefaultProvider.IsElementDisplay(substituteLinkLocator, LocateBy.Id);
        }

        public void ClickCancelLink(int index,bool isCancel)
        {
            string cancelLinkLocatorFormat = "ctl00_cph_grdMembers_ctl{0}_lnkCancel";
            string cancelLinkLocator = string.Empty;

            // Note: the primary attendee cannot be cancelled!
            cancelLinkLocator = string.Format(
                cancelLinkLocatorFormat,
                ((index + 1) < 10 ? "0" : string.Empty) + (index + 1).ToString());

            UIUtil.DefaultProvider.WaitForDisplayAndClick(cancelLinkLocator, LocateBy.Id);
            UIUtil.DefaultProvider.WaitForPageToLoad();

            if (isCancel)
            {
                UIUtil.DefaultProvider.WaitForDisplayAndClick("//div[@class='confirmDialog ui-dialog-content ui-widget-content']/div[@class='buttonGroup']/a[text()='OK']", LocateBy.XPath);
            }
            else
            {
                UIUtil.DefaultProvider.WaitForDisplayAndClick("//div[@class='confirmDialog ui-dialog-content ui-widget-content']/div[@class='buttonGroup']/a[text()='Cancel']", LocateBy.XPath);
            }
        }

        public void ClickFinalizeButton()
        {
            string finalizeButtonLocator = "//button[text()='Continue']";
            UIUtil.DefaultProvider.WaitForDisplayAndClick(finalizeButtonLocator, LocateBy.XPath);
            UIUtil.DefaultProvider.WaitForPageToLoad();
        }

        public string GetRegTypeText(int index)
        {
            string regTypeTextLocator = string.Format("//div[@id='pageContent']/table/tbody/tr[{0}]/td[2]", index.ToString());
            return UIUtil.DefaultProvider.GetText(regTypeTextLocator, LocateBy.XPath);
        }
        #endregion
    }
}