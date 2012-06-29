namespace RegOnline.RegressionTest.Keyword
{
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.PageObject.Builder;

    public class AddEarlyLatePrice
    {
        private EventDetails EventDetails = new EventDetails();
        private Agenda Agenda = new Agenda();

        public void AddEarlyPrice(EarlyPrice earlyPrice, FormData.Location location)
        {
            switch (location)
            {
                case FormData.Location.RegType:
                    EventDetails.RegTypeDefine.EventFeeDefine.AddEarlyPrice_Click();
                    EventDetails.RegTypeDefine.EventFeeDefine.EarlyPrice.Type(earlyPrice.earlyPrice);

                    switch (earlyPrice.EarlyPriceType)
                    {
                        case FormData.EarlyPriceType.DateAndTime:
                            EventDetails.RegTypeDefine.EventFeeDefine.EarlyPriceDateTime.Click();
                            EventDetails.RegTypeDefine.EventFeeDefine.EarlyPriceDate_Type(earlyPrice.EarlyPriceDate.Value);
                            EventDetails.RegTypeDefine.EventFeeDefine.EarlyPriceTime_Type(earlyPrice.EarlyPriceTime.Value);
                            break;
                        case FormData.EarlyPriceType.Registrants:
                            EventDetails.RegTypeDefine.EventFeeDefine.EarlyPriceRegLimit.Click();
                            EventDetails.RegTypeDefine.EventFeeDefine.EarlyPriceRegistrations.Type(earlyPrice.FirstNRegistrants.Value);
                            break;
                        default:
                            break;
                    }
                    break;
                case FormData.Location.Agenda:
                    Agenda.AddEarlyPrice_Click();
                    Agenda.EarlyPrice.Type(earlyPrice.earlyPrice);

                    switch (earlyPrice.EarlyPriceType)
                    {
                        case FormData.EarlyPriceType.DateAndTime:
                            Agenda.EarlyPriceDateTime.Click();
                            Agenda.EarlyPriceDate_Type(earlyPrice.EarlyPriceDate.Value);
                            Agenda.EarlyPriceTime_Type(earlyPrice.EarlyPriceTime.Value);
                            break;
                        case FormData.EarlyPriceType.Registrants:
                            Agenda.EarlyPriceRegLimit.Click();
                            Agenda.EarlyPriceRegistrations.Type(earlyPrice.FirstNRegistrants.Value);
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
                    EventDetails.RegTypeDefine.EventFeeDefine.AddLatePrice_Click();
                    EventDetails.RegTypeDefine.EventFeeDefine.LatePriceTime_Type(latePrice.LatePriceTime);
                    EventDetails.RegTypeDefine.EventFeeDefine.LatePriceDate_Type(latePrice.LatePriceDate);
                    EventDetails.RegTypeDefine.EventFeeDefine.LatePrice.Type(latePrice.latePrice);
                    break;
                case FormData.Location.Agenda:
                    Agenda.AddLatePrice_Click();
                    Agenda.LatePriceTime_Type(latePrice.LatePriceTime);
                    Agenda.LatePriceDate_Type(latePrice.LatePriceDate);
                    Agenda.LatePrice.Type(latePrice.latePrice);
                    break;
                default:
                    break;
            }
        }
    }
}
