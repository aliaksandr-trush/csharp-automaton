﻿namespace RegOnline.RegressionTest.Keyword
{
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.PageObject.Builder;

    public class AddDiscountCode
    {
        public void AddDiscountCodes(CustomFieldCode code, DataCollection.EventData_Common.Location location)
        {
            switch(location)
            {
                case DataCollection.EventData_Common.Location.EventFee:
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.EventFeeDefine.AddDiscountCode_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.EventFeeDefine.Code_Define.SelectByName();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.EventFeeDefine.Code_Define.Code.Type(code.CodeString);

                    switch (code.CodeType)
                    {
                        case DataCollection.EventData_Common.CustomFieldCodeType.DiscountCode:
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.EventFeeDefine.Code_Define.Discount_Click();
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.EventFeeDefine.Code_Define.CodeDirection.SelectWithText(code.CodeDirection.Value.ToString());

                            if (code.CodeKind == DataCollection.EventData_Common.ChangeType.Percent)
                            {
                                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.EventFeeDefine.Code_Define.Percentage_Click();
                            }

                            if (code.CodeKind == DataCollection.EventData_Common.ChangeType.FixedAmount)
                            {
                                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.EventFeeDefine.Code_Define.FixAmount_Click();
                            }

                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.EventFeeDefine.Code_Define.Amount.Type(code.Amount);
                            break;

                        case DataCollection.EventData_Common.CustomFieldCodeType.AccessCode:
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.EventFeeDefine.Code_Define.Access_Click();
                            break;

                        default:
                            break;
                    }

                    if (code.Limit.HasValue)
                    {
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.EventFeeDefine.Code_Define.UseLimit.Type(code.Limit.Value);
                    }

                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.EventFeeDefine.Code_Define.SaveAndClose_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.EventFeeDefine.SelectByName();
                    break;

                case DataCollection.EventData_Common.Location.RegType:
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.AddDiscountCode_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.Code_Define.SelectByName();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.Code_Define.Code.Type(code.CodeString);

                    switch (code.CodeType)
                    {
                        case DataCollection.EventData_Common.CustomFieldCodeType.DiscountCode:
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.Code_Define.Discount_Click();
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.Code_Define.CodeDirection.SelectWithText(code.CodeDirection.Value.ToString());

                            if (code.CodeKind == DataCollection.EventData_Common.ChangeType.Percent)
                            {
                                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.Code_Define.Percentage_Click();
                            }

                            if (code.CodeKind == DataCollection.EventData_Common.ChangeType.FixedAmount)
                            {
                                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.Code_Define.FixAmount_Click();
                            }

                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.Code_Define.Amount.Type(code.Amount);
                            break;

                        case DataCollection.EventData_Common.CustomFieldCodeType.AccessCode:
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.Code_Define.Access_Click();
                            break;

                        default:
                            break;
                    }

                    if (code.Limit.HasValue)
                    {
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.Code_Define.UseLimit.Type(code.Limit.Value);
                    }

                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.Code_Define.SaveAndClose_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.SelectByName();
                    break;

                case DataCollection.EventData_Common.Location.Agenda:
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AddDiscountCode_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.CodeDefine.SelectByName();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.CodeDefine.Code.Type(code.CodeString);

                    switch (code.CodeType)
                    {
                        case DataCollection.EventData_Common.CustomFieldCodeType.DiscountCode:
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.CodeDefine.Discount_Click();
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.CodeDefine.CodeDirection.SelectWithText(code.CodeDirection.Value.ToString());

                            if (code.CodeKind == DataCollection.EventData_Common.ChangeType.Percent)
                            {
                                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.CodeDefine.Percentage_Click();
                            }

                            if (code.CodeKind == DataCollection.EventData_Common.ChangeType.FixedAmount)
                            {
                                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.CodeDefine.FixAmount_Click();
                            }

                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.CodeDefine.Amount.Type(code.Amount);
                            break;
                        case DataCollection.EventData_Common.CustomFieldCodeType.AccessCode:
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.CodeDefine.Access_Click();
                            break;
                        default:
                            break;
                    }

                    if (code.Limit.HasValue)
                    {
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.CodeDefine.UseLimit.Type(code.Limit.Value);
                    }

                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.CodeDefine.SaveAndClose_Click();
                    break;

                default:
                    break;
            }
        }
    }
}
