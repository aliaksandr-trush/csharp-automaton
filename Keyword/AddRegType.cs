namespace RegOnline.RegressionTest.Keyword
{
    using System;
    using RegOnline.RegressionTest.DataCollection;

    public class AddRegType
    {
        public void Add_RegType(RegType regType, Event evt)
        {
            if (regType.AdditionalDetails != null)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.Title.Type(DateTime.Now.Ticks.ToString());
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.Shortcut.Type("char" + DateTime.Now.Ticks.ToString());
                PageObject.PageObjectProvider.Builder.EventDetails.SaveAndStay_Click();
            }

            if (PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.EmptyAddRegType.IsPresent)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.EmptyAddRegType_Click();
            }
            else
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.AddRegType_Click();
            }

            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.AdjustRADWindowPositionAndResize();

            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.SelectByName();

            #region RegTypeBasics
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.NameOnForm.Type(regType.Name);
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.NameOnReports.Type(regType.Name);

            if (regType.RegTypeLimit != null)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeLimit.Set(true);

                switch (regType.RegTypeLimit.LimitType)
                {
                    case DataCollection.EventData_Common.RegLimitType.Individual:
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.LimitToIndividual_Click();
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.IndividualLimit.Type(regType.RegTypeLimit.LimitTo);
                        break;
                    case DataCollection.EventData_Common.RegLimitType.Group:
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.LimitToGroup_Click();
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.GroupLimit.Type(regType.RegTypeLimit.LimitTo);
                        break;
                    default:
                        break;
                }

                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.SoldOutMessage.Type(regType.RegTypeLimit.SoldOutMessage);
            }
            if (regType.AdditionalDetails != null)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.AdditionalDetails_Click();
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.AdditionalDetailsEditor.SelectByName();
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.AdditionalDetailsEditor.HtmlMode_Click();
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.AdditionalDetailsEditor.Content_Type(regType.AdditionalDetails);
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.AdditionalDetailsEditor.SaveAndClose_Click();
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.SelectByName();
            }
            if (regType.IsPublic.HasValue)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.Public.Set(regType.IsPublic.Value);
            }
            if (regType.IsAdmin.HasValue)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.Admin.Set(regType.IsAdmin.Value);
            }
            if (regType.IsOnSite.HasValue)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.OnSite.Set(regType.IsOnSite.Value);
            }
            if (regType.Price.HasValue)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.EventFee.Type(regType.Price.Value);
            }
            #endregion

            if ((regType.AllCustomCodes.Count != 0) || (regType.EarlyPrice != null) ||
                (regType.LatePrice != null) || (evt.TaxRateOne != null) ||
                (evt.TaxRateTwo != null))
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.EventFeeAdvanced_Click();
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.AdjustRADWindowPositionAndResize();
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.SelectByName();

                if (regType.FeeName != null)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.NameOnReceipt.Type(regType.FeeName);
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.NameOnReports.Type(regType.FeeName);
                }
                else
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.NameOnReceipt.Type(regType.Name + "_" + RegType.Default.FeeName);
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.NameOnReports.Type(regType.Name + "_" + RegType.Default.FeeName);
                }

                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.StandardPrice.Type(regType.Price);
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.Options_Click();

                #region AddEarlyLatePrice
                if (regType.EarlyPrice != null)
                {
                    KeywordProvider.Add_EarlyLatePrice.AddEarlyPrice(regType.EarlyPrice, DataCollection.EventData_Common.Location.RegType);
                }

                if (regType.LatePrice != null)
                {
                    KeywordProvider.Add_EarlyLatePrice.AddLatePrice(regType.LatePrice, DataCollection.EventData_Common.Location.RegType);
                }
                #endregion

                #region AddDiscountCode
                if (regType.AllCustomCodes.Count != 0)
                {
                    if (!PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.AddDiscountCode.IsPresent)
                    {
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.Options_Click();
                    }

                    foreach (CustomFieldCode dc in regType.AllCustomCodes)
                    {
                        KeywordProvider.Add_DiscountCode.AddDiscountCodes(dc, DataCollection.EventData_Common.Location.RegType);
                    }

                    if (regType.RequireDC.HasValue)
                    {
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.RequireCode.Set(regType.RequireDC.Value);
                    }
                }
                #endregion

                #region AddTaxRate
                if ((evt.TaxRateOne != null) || (evt.TaxRateTwo != null))
                {
                    if (!PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.AddTaxRate.IsPresent)
                    {
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.Options_Click();
                    }

                    KeywordProvider.Add_TaxRate.AddTaxRates(evt.TaxRateOne, evt.TaxRateTwo, DataCollection.EventData_Common.Location.RegType);
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.SaveAndStay_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.Options_Click();

                    if (evt.TaxRateOne != null && regType.ApplyTaxOne.HasValue)
                    {
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.ApplyTaxOne.Set(regType.ApplyTaxOne.Value);
                    }

                    if (evt.TaxRateTwo != null && regType.ApplyTaxTwo.HasValue)
                    {
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.ApplyTaxTwo.Set(regType.ApplyTaxTwo.Value);
                    }
                }
                #endregion

                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.SaveAndClose_Click();
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.SelectByName();
            }

            #region RegTypeAdvanced
            if (regType.ShowStarting != null)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.ShowDate.Type(regType.ShowStarting.Value);
            }
            if (regType.HideStarting != null)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.HideDate_Type(regType.HideStarting.Value);
            }
            if (regType.MinGroupSize.HasValue)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.MinGroupSize.Type(regType.MinGroupSize.Value);
            }
            if (regType.MaxGroupSize.HasValue)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.MaxGroupSize.Type(regType.MaxGroupSize.Value);
            }
            if (regType.MinRegistrantMessage != null)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.AddMinRegMessage_Click();
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.MinRegMessageEditor.SelectByName();
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.MinRegMessageEditor.HtmlMode_Click();
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.MinRegMessageEditor.Content_Type(regType.MinRegistrantMessage);
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.MinRegMessageEditor.SaveAndClose_Click();
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.SelectByName();
            }
            if (regType.IsSSO)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.ExternalAuthentication_Click();
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.ExternalAuthenticationSetup.SelectByName();
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.ExternalAuthenticationSetup.SSORadio.Click();

                if (!PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.ExternalAuthenticationSetup.EndpointURL.HasAttribute("value"))
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.ExternalAuthenticationSetup.EndpointURL.Type(SSOData.SSOEndpointURL);
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.ExternalAuthenticationSetup.LoginURL.Type(SSOData.SSOLoginURL);
                }

                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.ExternalAuthenticationSetup.SaveAndClose_Click();
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.SelectByName();
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.EnableExternalAuthentication.Set(true);
            }
            #endregion

            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.SaveAndClose_Click();
        }
    }
}
