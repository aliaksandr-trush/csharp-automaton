namespace RegOnline.RegressionTest.Keyword
{
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.PageObject.Builder;

    public class AddDiscountCode
    {
        private EventDetails EventDetails = new EventDetails();
        private Agenda Agenda = new Agenda();

        public void AddDiscountCodes(DiscountCode code, FormData.Location location)
        {
            switch(location)
            {
                case FormData.Location.RegType:
                    EventDetails.RegTypeDefine.EventFeeDefine.AddDiscountCode_Click();
                    EventDetails.RegTypeDefine.EventFeeDefine.CodeDefine.SelectByName();
                    EventDetails.RegTypeDefine.EventFeeDefine.CodeDefine.Code.Type(code.Code);

                    switch (code.CodeType)
                    {
                        case FormData.DiscountCodeType.DiscountCode:
                            EventDetails.RegTypeDefine.EventFeeDefine.CodeDefine.Discount_Click();
                            EventDetails.RegTypeDefine.EventFeeDefine.CodeDefine.CodeDirection.SelectWithText(code.CodeDirection.ToString());

                            if (code.CodeKind == FormData.ChangeType.Percent)
                            {
                                EventDetails.RegTypeDefine.EventFeeDefine.CodeDefine.Percentage_Click();
                            }

                            if (code.CodeKind == FormData.ChangeType.FixedAmount)
                            {
                                EventDetails.RegTypeDefine.EventFeeDefine.CodeDefine.FixAmount_Click();
                            }

                            EventDetails.RegTypeDefine.EventFeeDefine.CodeDefine.Amount.Type(code.Amount);
                            break;
                        case FormData.DiscountCodeType.AccessCode:
                            EventDetails.RegTypeDefine.EventFeeDefine.CodeDefine.Access_Click();
                            break;
                        default:
                            break;
                    }

                    if (code.Limit.HasValue)
                    {
                        EventDetails.RegTypeDefine.EventFeeDefine.CodeDefine.UseLimit.Type(code.Limit.Value);
                    }

                    EventDetails.RegTypeDefine.EventFeeDefine.CodeDefine.SaveAndClose_Click();
                    break;
                case FormData.Location.Agenda:
                    Agenda.AddDiscountCode_Click();
                    Agenda.CodeDefine.SelectByName();
                    Agenda.CodeDefine.Code.Type(code.Code);

                    switch (code.CodeType)
                    {
                        case FormData.DiscountCodeType.DiscountCode:
                            Agenda.CodeDefine.Discount_Click();
                            Agenda.CodeDefine.CodeDirection.SelectWithText(code.CodeDirection.ToString());

                            if (code.CodeKind == FormData.ChangeType.Percent)
                            {
                                Agenda.CodeDefine.Percentage_Click();
                            }

                            if (code.CodeKind == FormData.ChangeType.FixedAmount)
                            {
                                Agenda.CodeDefine.FixAmount_Click();
                            }

                            Agenda.CodeDefine.Amount.Type(code.Amount);
                            break;
                        case FormData.DiscountCodeType.AccessCode:
                            Agenda.CodeDefine.Access_Click();
                            break;
                        default:
                            break;
                    }

                    if (code.Limit.HasValue)
                    {
                        Agenda.CodeDefine.UseLimit.Type(code.Limit.Value);
                    }

                    Agenda.CodeDefine.SaveAndClose_Click();
                    break;
                default:
                    break;
            }
        }
    }
}
