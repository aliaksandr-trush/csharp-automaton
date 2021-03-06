﻿namespace RegOnline.RegressionTest.Managers.Builder
{
    using System;
    using RegOnline.RegressionTest.Managers;
    //using RegOnline.RegressionTest.Fixtures;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;

    public partial class FormDetailManager : ManagerBase
    {
        public OtherEventTypeAgendaAndCFManager OldAGAndCFMgr { get; set; }

        public void ClickAddAgendaItemOld()
        {
            UIUtil.DefaultProvider.SwitchToMainContent();
            UIUtil.DefaultProvider.WaitForDisplayAndClick(GetAddGridItemLocator(OtherEventTypeAgendaAndCFManager.Locator.AddOptionLinkLocatorPrefix), LocateBy.Id);
            Utility.ThreadSleep(1);
            UIUtil.DefaultProvider.SelectPopUpFrameByName(OtherEventTypeAgendaAndCFManager.FrameId);
        }

        // Donation custom field is very much similiar to donation option
        public void ClickAddCustomFieldOld()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(GetAddGridItemLocator(PIPageAddCFLinkLocatorPrefix), LocateBy.Id);
            Utility.ThreadSleep(1);
            UIUtil.DefaultProvider.WaitForAJAXRequest();
            UIUtil.DefaultProvider.SelectPopUpFrameByName(OtherEventTypeAgendaAndCFManager.FrameId);
            this.ResizeCustomFieldRADWindowAndAdjustPosition(1000, 800, 20, 20);
        }

        public void AddCustomFieldOldStyle(OtherEventTypeAgendaAndCFManager.FieldType type, string name)
        {
            this.ClickAddCustomFieldOld();
            this.OldAGAndCFMgr.SetQuestionDescription(name);
            this.OldAGAndCFMgr.SetTypeWithDefaultsOld(type);
            this.OldAGAndCFMgr.SaveAndCloseAgendaOrCFItem();
            UIUtil.DefaultProvider.SwitchToMainContent();
            UIUtil.DefaultProvider.WaitForPageToLoad();
        }

        public void AddAgendaItemOldStyle(OtherEventTypeAgendaAndCFManager.FieldType type, string name)
        {
            this.ClickAddAgendaItemOld();
            this.OldAGAndCFMgr.SetQuestionDescription(name);

            if (UIUtil.DefaultProvider.IsElementPresent(OtherEventTypeAgendaAndCFManager.Locator.FeeTypeTableLocator, LocateBy.Id))
            {
                this.OldAGAndCFMgr.SetMembershipFeeType(OtherEventTypeAgendaAndCFManager.FeeType.OneTimeFee);
            }

            this.OldAGAndCFMgr.SetTypeWithDefaultsOld(type);

            // Enter amount
            if (type == OtherEventTypeAgendaAndCFManager.FieldType.CheckBox ||
                type == OtherEventTypeAgendaAndCFManager.FieldType.RadioButton ||
                type == OtherEventTypeAgendaAndCFManager.FieldType.Dropdown ||
                type == OtherEventTypeAgendaAndCFManager.FieldType.FileUpload ||
                type == OtherEventTypeAgendaAndCFManager.FieldType.AlwaysSelected)
            {
                this.OldAGAndCFMgr.SetRegularPrice(Math.Round(new Random((int)DateTime.Now.Ticks).NextDouble() * 1000 + 1, 2));
            }

            UIUtil.DefaultProvider.WaitForAJAXRequest();

            this.OldAGAndCFMgr.SaveAndCloseAgendaOrCFItem();
            UIUtil.DefaultProvider.SwitchToMainContent();
            UIUtil.DefaultProvider.WaitForPageToLoad();
        }

        public void AddAgendaItemOldStyleWithPrice(
            OtherEventTypeAgendaAndCFManager.FieldType type,
            string name,
            double standardPrice)
        {
            this.ClickAddAgendaItemOld();
            this.OldAGAndCFMgr.SetQuestionDescription(name);

            if (UIUtil.DefaultProvider.IsElementPresent(OtherEventTypeAgendaAndCFManager.Locator.FeeTypeTableLocator, LocateBy.Id))
            {
                OldAGAndCFMgr.SetMembershipFeeType(OtherEventTypeAgendaAndCFManager.FeeType.OneTimeFee);
            }

            this.OldAGAndCFMgr.SetTypeWithDefaultsOld(type);

            if (type == OtherEventTypeAgendaAndCFManager.FieldType.CheckBox ||
                type == OtherEventTypeAgendaAndCFManager.FieldType.RadioButton ||
                type == OtherEventTypeAgendaAndCFManager.FieldType.Dropdown ||
                type == OtherEventTypeAgendaAndCFManager.FieldType.FileUpload ||
                type == OtherEventTypeAgendaAndCFManager.FieldType.AlwaysSelected)
            {
                OldAGAndCFMgr.SetRegularPrice(standardPrice);
            }

            UIUtil.DefaultProvider.WaitForAJAXRequest();

            OldAGAndCFMgr.SaveAndCloseAgendaOrCFItem();
            UIUtil.DefaultProvider.SwitchToMainContent();
            UIUtil.DefaultProvider.WaitForPageToLoad();
        }
    }
}
