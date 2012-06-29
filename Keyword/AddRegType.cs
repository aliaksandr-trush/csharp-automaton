namespace RegOnline.RegressionTest.Keyword
{
    using System;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.PageObject.Builder;

    public class AddRegType
    {
        private EventDetails EventDetails = new EventDetails();
        private PageObjectHelper BuilderHelper = new PageObjectHelper();

        public void AddRegTypes(RegType regType)
        {
            if (regType.AdditionalDetails != null)
            {
                EventDetails.Title.Type(DateTime.Now.Ticks.ToString());
                EventDetails.Shortcut.Type("char" + DateTime.Now.Ticks.ToString());
                BuilderHelper.SaveAndStay_Click();
            }

            if (EventDetails.EmptyAddRegType.IsPresent)
            {
                EventDetails.EmptyAddRegType_Click();
            }
            else
            {
                EventDetails.AddRegType_Click();
            }

            EventDetails.RegTypeDefine.SelectByName();

            #region RegTypeBasics
            EventDetails.RegTypeDefine.NameOnForm.Type(regType.RegTypeName);
            EventDetails.RegTypeDefine.NameOnReports.Type(regType.RegTypeName);

            if (regType.RegTypeLimit != null)
            {
                EventDetails.RegTypeDefine.RegTypeLimit.Set(true);

                switch (regType.RegTypeLimit.LimitType)
                {
                    case FormData.RegLimitType.Individual:
                        EventDetails.RegTypeDefine.LimitToIndividual_Click();
                        EventDetails.RegTypeDefine.IndividualLimit.Type(regType.RegTypeLimit.LimitTo);
                        break;
                    case FormData.RegLimitType.Group:
                        EventDetails.RegTypeDefine.LimitToGroup_Click();
                        EventDetails.RegTypeDefine.GroupLimit.Type(regType.RegTypeLimit.LimitTo);
                        break;
                    default:
                        break;
                }

                EventDetails.RegTypeDefine.SoldOutMessage.Type(regType.RegTypeLimit.SoldOutMessage);
            }
            if (regType.AdditionalDetails != null)
            {
                EventDetails.RegTypeDefine.AdditionalDetails_Click();
                EventDetails.RegTypeDefine.AdditionalDetailsEditor.SelectByName();
                EventDetails.RegTypeDefine.AdditionalDetailsEditor.HtmlMode_Click();
                EventDetails.RegTypeDefine.AdditionalDetailsEditor.Content_Type(regType.AdditionalDetails);
                EventDetails.RegTypeDefine.AdditionalDetailsEditor.SaveAndClose_Click();
            }
            if (regType.IsPublic.HasValue)
            {
                EventDetails.RegTypeDefine.Public.Set(regType.IsPublic.Value);
            }
            if (regType.IsAdmin.HasValue)
            {
                EventDetails.RegTypeDefine.Admin.Set(regType.IsAdmin.Value);
            }
            if (regType.IsOnSite.HasValue)
            {
                EventDetails.RegTypeDefine.OnSite.Set(regType.IsOnSite.Value);
            }
            if (regType.Price.HasValue)
            {
                EventDetails.RegTypeDefine.EventFee.Type(regType.Price.Value);
            }
            #endregion

            if ((regType.DiscountCode.Count != 0) || (regType.EarlyPrice != null) ||
                (regType.LatePrice != null) || (regType.TaxRateOne != null) ||
                (regType.TaxRateTwo != null))
            {
                EventDetails.RegTypeDefine.EventFeeAdvanced_Click();
                EventDetails.RegTypeDefine.EventFeeDefine.SelectByName();

                if (regType.FeeName != null)
                {
                    EventDetails.RegTypeDefine.EventFeeDefine.NameOnReceipt.Type(regType.FeeName);
                    EventDetails.RegTypeDefine.EventFeeDefine.NameOnReports.Type(regType.FeeName);
                }
                else
                {
                    EventDetails.EventFeeDefine.NameOnReceipt.Type(regType.RegTypeName + "_" + RegType.Default.FeeName);
                    EventDetails.EventFeeDefine.NameOnReports.Type(regType.RegTypeName + "_" + RegType.Default.FeeName);
                }

                EventDetails.EventFeeDefine.StandardPrice.Type(regType.Price);
                EventDetails.EventFeeDefine.Options_Click();

                #region AddEarlyLatePrice
                if (regType.EarlyPrice != null)
                {
                    KeywordProvider.AddEarlyLatePrice.AddEarlyPrice(regType.EarlyPrice, FormData.Location.RegType);
                }

                if (regType.LatePrice != null)
                {
                    KeywordProvider.AddEarlyLatePrice.AddLatePrice(regType.LatePrice, FormData.Location.RegType);
                    EventDetails.RegTypeDefine.EventFeeDefine.SaveAndStay_Click();
                }
                #endregion

                #region AddDiscountCode
                if (regType.DiscountCode.Count != 0)
                {
                    if (!EventDetails.RegTypeDefine.EventFeeDefine.AddDiscountCode.IsPresent)
                    {
                        EventDetails.RegTypeDefine.EventFeeDefine.Options_Click();
                    }

                    foreach (DiscountCode dc in regType.DiscountCode)
                    {
                        KeywordProvider.AddDiscountCode.AddDiscountCodes(dc, FormData.Location.RegType);
                    }

                    if (regType.RequireDC.HasValue)
                    {
                        EventDetails.RegTypeDefine.EventFeeDefine.RequireCode.Set(regType.RequireDC.Value);
                    }
                }
                #endregion

                #region AddTaxRate
                if ((regType.TaxRateOne != null) || (regType.TaxRateTwo != null))
                {
                    if (!EventDetails.RegTypeDefine.EventFeeDefine.AddTaxRate.IsPresent)
                    {
                        EventDetails.RegTypeDefine.EventFeeDefine.Options_Click();
                    }

                    KeywordProvider.AddTaxRate.AddTaxRates(regType.TaxRateOne, regType.TaxRateTwo, FormData.Location.RegType);
                    EventDetails.RegTypeDefine.EventFeeDefine.SaveAndStay_Click();
                    EventDetails.RegTypeDefine.EventFeeDefine.Options_Click();

                    if ((regType.TaxRateOne.Apply.HasValue) || (regType.TaxRateTwo.Apply.HasValue))
                    {
                        if (regType.TaxRateOne.Apply.HasValue)
                        {
                            EventDetails.RegTypeDefine.EventFeeDefine.ApplyTaxOne.Set(regType.TaxRateOne.Apply.Value);
                        }
                        if (regType.TaxRateTwo.Apply.HasValue)
                        {
                            EventDetails.RegTypeDefine.EventFeeDefine.ApplyTaxTwo.Set(regType.TaxRateTwo.Apply.Value);
                        }
                    }
                }
                #endregion

                EventDetails.RegTypeDefine.EventFeeDefine.SaveAndClose_Click();
            }

            #region RegTypeAdvanced
            if (regType.ShowStarting != null)
            {
                EventDetails.RegTypeDefine.ShowDate.Type(regType.ShowStarting.Value);
            }
            if (regType.HideStarting != null)
            {
                EventDetails.RegTypeDefine.HideDate_Type(regType.HideStarting.Value);
            }
            if (regType.MinGroupSize.HasValue)
            {
                EventDetails.RegTypeDefine.MinGroupSize.Type(regType.MinGroupSize.Value);
            }
            if (regType.MaxGroupSize.HasValue)
            {
                EventDetails.RegTypeDefine.MaxGroupSize.Type(regType.MaxGroupSize.Value);
            }
            if (regType.MinRegistrantMessage != null)
            {
                EventDetails.RegTypeDefine.AddMinRegMessage_Click();
                EventDetails.RegTypeDefine.MinRegMessageEditor.SelectByName();
                EventDetails.RegTypeDefine.MinRegMessageEditor.HtmlMode_Click();
                EventDetails.RegTypeDefine.MinRegMessageEditor.Content_Type(regType.MinRegistrantMessage);
                EventDetails.RegTypeDefine.MinRegMessageEditor.SaveAndClose_Click();
            }
            #endregion

            EventDetails.RegTypeDefine.SaveAndClose_Click();
        }
    }
}
