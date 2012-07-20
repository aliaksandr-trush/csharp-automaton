namespace RegOnline.RegressionTest.Keyword
{
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.PageObject.Builder;

    public class AddEarlyLatePrice
    {
        public void AddEarlyPrice(EarlyPrice earlyPrice, FormData.Location location)
        {
            switch (location)
            {
                case FormData.Location.RegType:
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.EventFeeDefine.AddEarlyPrice_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.EventFeeDefine.EarlyPrice.Type(earlyPrice.earlyPrice);

                    switch (earlyPrice.EarlyPriceType)
                    {
                        case FormData.EarlyPriceType.DateAndTime:
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.EventFeeDefine.EarlyPriceDateTime.Click();
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.EventFeeDefine.EarlyPriceDate_Type(earlyPrice.EarlyPriceDate.Value);
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.EventFeeDefine.EarlyPriceTime_Type(earlyPrice.EarlyPriceTime.Value);
                            break;
                        case FormData.EarlyPriceType.Registrants:
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.EventFeeDefine.EarlyPriceRegLimit.Click();
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.EventFeeDefine.EarlyPriceRegistrations.Type(earlyPrice.FirstNRegistrants.Value);
                            break;
                        default:
                            break;
                    }
                    break;
                case FormData.Location.Agenda:
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AddEarlyPrice_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.EarlyPrice.Type(earlyPrice.earlyPrice);

                    switch (earlyPrice.EarlyPriceType)
                    {
                        case FormData.EarlyPriceType.DateAndTime:
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.EarlyPriceDateTime.Click();
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.EarlyPriceDate_Type(earlyPrice.EarlyPriceDate.Value);
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.EarlyPriceTime_Type(earlyPrice.EarlyPriceTime.Value);
                            break;
                        case FormData.EarlyPriceType.Registrants:
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.EarlyPriceRegistrations.Type(earlyPrice.FirstNRegistrants.Value);
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.EarlyPriceRegLimit.Click();
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        public void AddLatePrice(LatePrice latePrice, FormData.Location location)
        {
            switch (location)
            {
                case FormData.Location.RegType:
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.EventFeeDefine.AddLatePrice_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.EventFeeDefine.LatePriceTime_Type(latePrice.LatePriceTime);
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.EventFeeDefine.LatePriceDate_Type(latePrice.LatePriceDate);
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.EventFeeDefine.LatePrice.Type(latePrice.latePrice);
                    break;
                case FormData.Location.Agenda:
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AddLatePrice_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.LatePriceTime_Type(latePrice.LatePriceTime);
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.LatePriceDate_Type(latePrice.LatePriceDate);
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.LatePrice.Type(latePrice.latePrice);
                    break;
                default:
                    break;
            }
        }
    }
}
