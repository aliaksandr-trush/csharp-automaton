namespace RegOnline.RegressionTest.Keyword
{
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.PageObject.Builder;
    using RegOnline.RegressionTest.Utilities;

    public class AddEarlyLatePrice
    {
        public void AddEarlyPrice(EarlyPrice earlyPrice, FormData.Location location)
        {
            switch (location)
            {
                case FormData.Location.RegType:
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.AddEarlyPrice_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.EarlyPrice.Type(earlyPrice.earlyPrice);

                    switch (earlyPrice.EarlyPriceType)
                    {
                        case FormData.EarlyPriceType.DateAndTime:
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.EarlyPriceDateTime.Click();
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.EarlyPriceDate_Type(earlyPrice.EarlyPriceDate.Value);
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.EarlyPriceTime_Type(earlyPrice.EarlyPriceTime.Value);
                            break;
                        case FormData.EarlyPriceType.Registrants:
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.EarlyPriceRegLimit.Click();
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.EarlyPriceRegistrations.Type(earlyPrice.FirstNRegistrants.Value);
                            break;
                        default:
                            break;
                    }

                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.StandardPrice.Click();
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

                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.StandardPrice.Click();
                    break;

                default:
                    break;
            }

            
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.WaitForAJAX();
            Utility.ThreadSleep(2);
        }

        public void AddLatePrice(LatePrice latePrice, FormData.Location location)
        {
            switch (location)
            {
                case FormData.Location.RegType:
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.AddLatePrice_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.LatePriceTime_Type(latePrice.LatePriceTime);
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.LatePriceDate_Type(latePrice.LatePriceDate);
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.LatePrice.Type(latePrice.latePrice);
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.StandardPrice.Click();
                    break;
                case FormData.Location.Agenda:
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AddLatePrice_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.LatePriceTime_Type(latePrice.LatePriceTime);
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.LatePriceDate_Type(latePrice.LatePriceDate);
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.LatePrice.Type(latePrice.latePrice);
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.StandardPrice.Click();
                    break;
                default:
                    break;
            }

            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.WaitForAJAX();
            Utility.ThreadSleep(2);
        }
    }
}
