namespace RegOnline.RegressionTest.Keyword
{
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.PageObject.Builder;

    public class AddDiscountCode
    {
        public void AddDiscountCodes(DiscountCode code, FormData.Location location)
        {
            switch(location)
            {
                case FormData.Location.RegType:
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.EventFeeDefine.AddDiscountCode_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.EventFeeDefine.CodeDefine.SelectByName();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.EventFeeDefine.CodeDefine.Code.Type(code.Code);

                    switch (code.CodeType)
                    {
                        case FormData.DiscountCodeType.DiscountCode:
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.EventFeeDefine.CodeDefine.Discount_Click();
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.EventFeeDefine.CodeDefine.CodeDirection.SelectWithText(code.CodeDirection.ToString());

                            if (code.CodeKind == FormData.ChangeType.Percent)
                            {
                                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.EventFeeDefine.CodeDefine.Percentage_Click();
                            }

                            if (code.CodeKind == FormData.ChangeType.FixedAmount)
                            {
                                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.EventFeeDefine.CodeDefine.FixAmount_Click();
                            }

                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.EventFeeDefine.CodeDefine.Amount.Type(code.Amount);
                            break;
                        case FormData.DiscountCodeType.AccessCode:
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.EventFeeDefine.CodeDefine.Access_Click();
                            break;
                        default:
                            break;
                    }

                    if (code.Limit.HasValue)
                    {
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.EventFeeDefine.CodeDefine.UseLimit.Type(code.Limit.Value);
                    }

                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.EventFeeDefine.CodeDefine.SaveAndClose_Click();
                    break;
                case FormData.Location.Agenda:
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AddDiscountCode_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.CodeDefine.SelectByName();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.CodeDefine.Code.Type(code.Code);

                    switch (code.CodeType)
                    {
                        case FormData.DiscountCodeType.DiscountCode:
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.CodeDefine.Discount_Click();
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.CodeDefine.CodeDirection.SelectWithText(code.CodeDirection.ToString());

                            if (code.CodeKind == FormData.ChangeType.Percent)
                            {
                                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.CodeDefine.Percentage_Click();
                            }

                            if (code.CodeKind == FormData.ChangeType.FixedAmount)
                            {
                                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.CodeDefine.FixAmount_Click();
                            }

                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.CodeDefine.Amount.Type(code.Amount);
                            break;
                        case FormData.DiscountCodeType.AccessCode:
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
