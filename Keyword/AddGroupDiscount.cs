namespace RegOnline.RegressionTest.Keyword
{
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.PageObject;
    using RegOnline.RegressionTest.Utilities;

    public class AddGroupDiscount
    {
        public void Add_GroupDiscount(Event evt, GroupDiscount groupDiscount)
        {
            KeywordProvider.Manager_Common.OpenFormDashboard(evt.Id);
            PageObjectProvider.Manager.Dashboard.EventDetails.EditForm_Click();
            PageObjectProvider.Builder.EventDetails.FormPages.StartPage.AddGroupDiscount_Click();
            PageObjectProvider.Builder.EventDetails.FormPages.StartPage.GroupDiscountDefine.SelectByName();

            PageObjectProvider.Builder.EventDetails.FormPages.StartPage.GroupDiscountDefine.GroupSize.Type(groupDiscount.GroupSize);
            PageObjectProvider.Builder.EventDetails.FormPages.StartPage.GroupDiscountDefine.GroupSizeOption.SelectWithText(
                CustomStringAttribute.GetCustomString(groupDiscount.GroupSizeOption));
            PageObjectProvider.Builder.EventDetails.FormPages.StartPage.GroupDiscountDefine.DiscountAmount.Type(groupDiscount.DiscountAmount);
            PageObjectProvider.Builder.EventDetails.FormPages.StartPage.GroupDiscountDefine.DiscountType.SelectWithText(
                CustomStringAttribute.GetCustomString(groupDiscount.GroupDiscountType));

            if ((groupDiscount.GroupSizeOption == GroupDiscount_GroupSizeOption.JustSize) && groupDiscount.AddtionalRegOption.HasValue)
            {
                PageObjectProvider.Builder.EventDetails.FormPages.StartPage.GroupDiscountDefine.AddtionalRegOption.SelectWithText(
                    CustomStringAttribute.GetCustomString(groupDiscount.AddtionalRegOption.Value));

                if ((groupDiscount.AddtionalRegOption.Value == GroupDiscount_AdditionalRegOption.Additional)
                    && (groupDiscount.NumberOfAdditionalReg.HasValue))
                {
                    PageObjectProvider.Builder.EventDetails.FormPages.StartPage.GroupDiscountDefine.AdditionalNumber.Type(
                        groupDiscount.NumberOfAdditionalReg.Value);
                }
            }

            PageObjectProvider.Builder.EventDetails.FormPages.StartPage.GroupDiscountDefine.ApplyToSelectedFees_Click();

            if ((groupDiscount.ApplyToAgendaItems.Count != 0) || (groupDiscount.ApplyToRegTypes.Count != 0))
            {
                PageObjectProvider.Builder.EventDetails.FormPages.StartPage.GroupDiscountDefine.All.Set(false);

                if (groupDiscount.ApplyToAgendaItems.Count != 0)
                {
                    foreach (AgendaItem agenda in groupDiscount.ApplyToAgendaItems)
                    {
                        PageObjectProvider.Builder.EventDetails.FormPages.StartPage.GroupDiscountDefine.ApplyToAgendaItem(agenda).Set(true);
                    }
                }

                if (groupDiscount.ApplyToRegTypes.Count != 0)
                {
                    foreach (RegType regType in groupDiscount.ApplyToRegTypes)
                    {
                        PageObjectProvider.Builder.EventDetails.FormPages.StartPage.GroupDiscountDefine.ApplyToRegType(regType).Set(true);
                    }
                }
            }

            PageObjectProvider.Builder.EventDetails.FormPages.StartPage.GroupDiscountDefine.SaveAndClose_Click();
            PageObjectProvider.Builder.EventDetails.SaveAndClose_Click();
            PageObjectProvider.Manager.Dashboard.ReturnToList_Click();
        }
    }
}
