namespace RegOnline.RegressionTest.Keyword
{
    using System;
    using System.Collections.Generic;
    using RegOnline.RegressionTest.DataCollection;

    public class CalculateFee
    {
        public double CalculateTotalFee(Group group)
        {
            return this.CalculateTaxRates(group);
        }

        public double CalculateTotalFee(Registrant reg)
        {
            Group group = new Group();
            group.Primary = reg;

            return this.CalculateTotalFee(group);
        }

        private double CalculateSingleReg(Registrant reg)
        {
            reg.Fee_Summary = new FeeSummary();
            reg.Fee_Summary.Total = 0;

            if (reg.EventFee_Response != null)
            {
                if (reg.EventFee_Response.RegType.EarlyPrice != null)
                {
                    if ((reg.EventFee_Response.RegType.EarlyPrice.EarlyPriceType == FormData.EarlyPriceType.DateAndTime)
                        && (reg.EventFee_Response.RegType.EarlyPrice.EarlyPriceDate.Value > DateTime.Now))
                    {
                        reg.Fee_Summary.Total += reg.EventFee_Response.RegType.EarlyPrice.earlyPrice;
                        reg.EventFee_Response.Fee = reg.EventFee_Response.RegType.EarlyPrice.earlyPrice;
                    }

                    if (reg.EventFee_Response.RegType.EarlyPrice.EarlyPriceType == FormData.EarlyPriceType.Registrants)
                    {
                        int previousRegsForThisRegType = 0;

                        foreach (Registrant previousReg in reg.Event.Registrants)
                        {
                            if (previousReg.EventFee_Response.RegType == reg.EventFee_Response.RegType)
                            {
                                previousRegsForThisRegType += 1;
                            }
                        }

                        if (previousRegsForThisRegType <= reg.EventFee_Response.RegType.EarlyPrice.FirstNRegistrants.Value)
                        {
                            reg.Fee_Summary.Total += reg.EventFee_Response.RegType.EarlyPrice.earlyPrice;
                            reg.EventFee_Response.Fee = reg.EventFee_Response.RegType.EarlyPrice.earlyPrice;
                        }
                        else
                        {
                            reg.Fee_Summary.Total += reg.EventFee_Response.RegType.Price.Value;
                            reg.EventFee_Response.Fee = reg.EventFee_Response.RegType.Price.Value;
                        }
                    }
                }
                else if ((reg.EventFee_Response.RegType.LatePrice != null)
                    && (reg.EventFee_Response.RegType.LatePrice.LatePriceDate < DateTime.Now))
                {
                    reg.Fee_Summary.Total += reg.EventFee_Response.RegType.LatePrice.latePrice;
                    reg.EventFee_Response.Fee = reg.EventFee_Response.RegType.LatePrice.latePrice;
                }
                else
                {
                    reg.Fee_Summary.Total += reg.EventFee_Response.Fee;
                }
            }

            foreach (CustomFieldResponse responses in reg.CustomField_Responses)
            {
                if (responses is AgendaResponse)
                {
                    AgendaResponse response = responses as AgendaResponse;
                    AgendaItem_Common agenda;

                    switch (response.AgendaItem.Type)
                    {
                        case FormData.CustomFieldType.AlwaysSelected:
                            {
                                AgendaResponse_AlwaysSelected resp = response as AgendaResponse_AlwaysSelected;
                                agenda = resp.AgendaItem as AgendaItem_Common;
                                reg.Fee_Summary.Total += this.CalculateAgendaEarlyLateFee(agenda, reg.Event);
                                resp.Fee = this.CalculateAgendaEarlyLateFee(agenda, reg.Event);
                            }
                            break;

                        case FormData.CustomFieldType.CheckBox:
                            {
                                AgendaResponse_Checkbox resp = response as AgendaResponse_Checkbox;
                                agenda = resp.AgendaItem as AgendaItem_Common;
                                reg.Fee_Summary.Total += this.CalculateAgendaEarlyLateFee(agenda, reg.Event);
                                resp.Fee = this.CalculateAgendaEarlyLateFee(agenda, reg.Event);
                            }
                            break;

                        case FormData.CustomFieldType.RadioButton:
                            {
                                AgendaResponse_MultipleChoice_RadioButton resp = response as AgendaResponse_MultipleChoice_RadioButton;
                                agenda = resp.AgendaItem as AgendaItem_Common;
                                reg.Fee_Summary.Total += this.CalculateAgendaEarlyLateFee(agenda, reg.Event);
                                resp.Fee = this.CalculateAgendaEarlyLateFee(agenda, reg.Event);
                            }
                            break;

                        case FormData.CustomFieldType.Dropdown:
                            {
                                AgendaResponse_MultipleChoice_DropDown resp = response as AgendaResponse_MultipleChoice_DropDown;
                                agenda = resp.AgendaItem as AgendaItem_Common;
                                reg.Fee_Summary.Total += this.CalculateAgendaEarlyLateFee(agenda, reg.Event);
                                resp.Fee = this.CalculateAgendaEarlyLateFee(agenda, reg.Event);
                            }
                            break;

                        case FormData.CustomFieldType.Contribution:
                            {
                                AgendaResponse_Contribution resp = response as AgendaResponse_Contribution;
                                agenda = resp.AgendaItem as AgendaItem_Common;
                                reg.Fee_Summary.Total += this.CalculateAgendaEarlyLateFee(agenda, reg.Event);
                                resp.ContributionAmount = this.CalculateAgendaEarlyLateFee(agenda, reg.Event);
                            }
                            break;

                        case FormData.CustomFieldType.FileUpload:
                            {
                                AgendaResponse_FileUpload resp = response as AgendaResponse_FileUpload;
                                agenda = resp.AgendaItem as AgendaItem_Common;
                                reg.Fee_Summary.Total += this.CalculateAgendaEarlyLateFee(agenda, reg.Event);
                                resp.Fee = this.CalculateAgendaEarlyLateFee(agenda, reg.Event);
                            }
                            break;

                        default:
                            throw new ArgumentException(string.Format("Agenda item of specified type has no price: {0}", response.AgendaItem.Type));
                    }
                }
            }

            foreach (MerchandiseResponse response in reg.Merchandise_Responses)
            {
                switch (response.Merchandise_Item.Type)
                {
                    case FormData.MerchandiseType.Fixed:
                        {
                            MerchResponse_FixedPrice resp = response as MerchResponse_FixedPrice;
                            double price = (resp.Merchandise_Item.Price.HasValue ? resp.Merchandise_Item.Price.Value : 0);
                            reg.Fee_Summary.Total += price * resp.Quantity;
                        }
                        break;

                    case FormData.MerchandiseType.Variable:
                        {
                            MerchResponse_VariableAmount resp = response as MerchResponse_VariableAmount;
                            reg.Fee_Summary.Total += resp.Amount;
                        }
                        break;

                    default:
                        throw new ArgumentException(string.Format("Merchandise item of specified type has no price: {0}", response.Merchandise_Item.Type));
                }
            }

            return reg.Fee_Summary.Total;
        }

        private double CalculateGroupDiscount(Group group)
        {
            double total = 0;

            total += this.CalculateSingleReg(group.Primary);

            foreach (Registrant reg in group.Secondaries)
            {
                total += this.CalculateSingleReg(reg);
            }

            List<Registrant> regs = new List<Registrant>();

            regs.Add(group.Primary);

            foreach (Registrant reg in group.Secondaries)
            {
                regs.Add(reg);
            }

            List<EventFeeResponse> regEventFeeResponses = new List<EventFeeResponse>();
            List<AgendaResponse> regAgendaResponses = new List<AgendaResponse>();

            foreach (Registrant reg in regs)
            {
                if (reg.EventFee_Response != null)
                {
                    regEventFeeResponses.Add(reg.EventFee_Response);
                }

                foreach (CustomFieldResponse resp in reg.CustomField_Responses)
                {
                    if (resp is AgendaResponse)
                    {
                        AgendaResponse response = resp as AgendaResponse;

                        switch (response.AgendaItem.Type)
                        {
                            case FormData.CustomFieldType.AlwaysSelected:
                            case FormData.CustomFieldType.CheckBox:
                            case FormData.CustomFieldType.RadioButton:
                            case FormData.CustomFieldType.Dropdown:
                            case FormData.CustomFieldType.Contribution:
                            case FormData.CustomFieldType.FileUpload:
                                regAgendaResponses.Add(response);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            List<EventFeeResponseCount> eventFeeResponseCount = new List<EventFeeResponseCount>();
            List<AgendaResponseCount> agendaResponseCount = new List<AgendaResponseCount>();

            foreach (EventFeeResponse resp in regEventFeeResponses)
            {
                if (eventFeeResponseCount.Exists(e => e.EventFee_Response.RegType == resp.RegType))
                {
                    eventFeeResponseCount.Find(e => e.EventFee_Response.RegType == resp.RegType).count += 1;
                }
                else if ((group.Primary.Event.StartPage.GroupDiscount != null) 
                    && group.Primary.Event.StartPage.GroupDiscount.ApplyToRegTypes.Count != 0)
                {
                    if (group.Primary.Event.StartPage.GroupDiscount.ApplyToRegTypes.Exists(r => r == resp.RegType))
                    {
                        EventFeeResponseCount responseCount = new EventFeeResponseCount();
                        responseCount.EventFee_Response = resp;
                        responseCount.count = 1;
                        eventFeeResponseCount.Add(responseCount);
                    }
                }
                else
                {
                    EventFeeResponseCount responseCount = new EventFeeResponseCount();
                    responseCount.EventFee_Response = resp;
                    responseCount.count = 1;
                    eventFeeResponseCount.Add(responseCount);
                }
            }

            foreach (AgendaResponse resp in regAgendaResponses)
            {
                if (agendaResponseCount.Exists(a => a.Agenda_Response.AgendaItem == resp.AgendaItem))
                {
                    agendaResponseCount.Find(a => a.Agenda_Response.AgendaItem == resp.AgendaItem).count += 1;
                }
                else if ((group.Primary.Event.StartPage.GroupDiscount != null) 
                    && group.Primary.Event.StartPage.GroupDiscount.ApplyToAgendaItems.Count != 0)
                {
                    if (group.Primary.Event.StartPage.GroupDiscount.ApplyToAgendaItems.Exists(a => a == resp.AgendaItem))
                    {
                        AgendaResponseCount responseCount = new AgendaResponseCount();
                        responseCount.Agenda_Response = resp;
                        responseCount.count = 1;
                        agendaResponseCount.Add(responseCount);
                    }
                }
                else
                {
                    AgendaResponseCount responseCount = new AgendaResponseCount();
                    responseCount.Agenda_Response = resp;
                    responseCount.count = 1;
                    agendaResponseCount.Add(responseCount);
                }
            }

            foreach (EventFeeResponseCount count in eventFeeResponseCount)
            {
                if ((group.Primary.Event.StartPage.GroupDiscount != null) && 
                    (count.count >= group.Primary.Event.StartPage.GroupDiscount.GroupSize) &&
                    (group.Primary.Event.StartPage.GroupDiscount.ShowAndApply))
                {
                    if (group.Primary.Event.StartPage.GroupDiscount.GroupSizeOption == GroupDiscount_GroupSizeOption.SizeOrMore)
                    {
                        if (group.Primary.Event.StartPage.GroupDiscount.GroupDiscountType == GroupDiscount_DiscountType.Percent)
                        {
                            total -= count.EventFee_Response.Fee * group.Primary.Event.StartPage.GroupDiscount.DiscountAmount / 100.0 * count.count;
                            count.EventFee_Response.Fee -= count.EventFee_Response.Fee * group.Primary.Event.StartPage.GroupDiscount.DiscountAmount / 100.0 * count.count;
                        }
                        else
                        {
                            total -= group.Primary.Event.StartPage.GroupDiscount.DiscountAmount * count.count;
                            count.EventFee_Response.Fee -= group.Primary.Event.StartPage.GroupDiscount.DiscountAmount * count.count;
                        }
                    }
                    else if (group.Primary.Event.StartPage.GroupDiscount.AddtionalRegOption == GroupDiscount_AdditionalRegOption.All)
                    {
                        for (int i = 1; i <= count.count; i++)
                        {
                            if (i % group.Primary.Event.StartPage.GroupDiscount.GroupSize == 0)
                            {
                                if (group.Primary.Event.StartPage.GroupDiscount.GroupDiscountType == GroupDiscount_DiscountType.Percent)
                                {
                                    total -= count.EventFee_Response.Fee * group.Primary.Event.StartPage.GroupDiscount.DiscountAmount / 100.0 * group.Primary.Event.StartPage.GroupDiscount.GroupSize;
                                    count.EventFee_Response.Fee -= count.EventFee_Response.Fee * group.Primary.Event.StartPage.GroupDiscount.DiscountAmount / 100.0 * group.Primary.Event.StartPage.GroupDiscount.GroupSize;
                                }
                                else
                                {
                                    total -= group.Primary.Event.StartPage.GroupDiscount.DiscountAmount * group.Primary.Event.StartPage.GroupDiscount.GroupSize;
                                    count.EventFee_Response.Fee -= group.Primary.Event.StartPage.GroupDiscount.DiscountAmount * group.Primary.Event.StartPage.GroupDiscount.GroupSize;
                                }
                            }
                        }
                    }
                    else if (group.Primary.Event.StartPage.GroupDiscount.AddtionalRegOption == GroupDiscount_AdditionalRegOption.AnyAdditional)
                    {
                        if (group.Primary.Event.StartPage.GroupDiscount.GroupDiscountType == GroupDiscount_DiscountType.Percent)
                        {
                            total -= count.EventFee_Response.Fee * group.Primary.Event.StartPage.GroupDiscount.DiscountAmount / 100.0 * (count.count - group.Primary.Event.StartPage.GroupDiscount.GroupSize);
                            count.EventFee_Response.Fee -= count.EventFee_Response.Fee * group.Primary.Event.StartPage.GroupDiscount.DiscountAmount / 100.0 * (count.count - group.Primary.Event.StartPage.GroupDiscount.GroupSize);
                        }
                        else
                        {
                            total -= group.Primary.Event.StartPage.GroupDiscount.DiscountAmount * (count.count - group.Primary.Event.StartPage.GroupDiscount.GroupSize);
                            count.EventFee_Response.Fee -= group.Primary.Event.StartPage.GroupDiscount.DiscountAmount * (count.count - group.Primary.Event.StartPage.GroupDiscount.GroupSize);
                        } 
                    }
                    else if (group.Primary.Event.StartPage.GroupDiscount.AddtionalRegOption == GroupDiscount_AdditionalRegOption.Additional)
                    {
                        for (int i = 1; i <= count.count; i++)
                        {
                            if ((i > group.Primary.Event.StartPage.GroupDiscount.GroupSize) && (i <= group.Primary.Event.StartPage.GroupDiscount.NumberOfAdditionalReg.Value))
                            {
                                if (group.Primary.Event.StartPage.GroupDiscount.GroupDiscountType == GroupDiscount_DiscountType.Percent)
                                {
                                    total -= count.EventFee_Response.Fee * group.Primary.Event.StartPage.GroupDiscount.DiscountAmount / 100.0;
                                    count.EventFee_Response.Fee -= count.EventFee_Response.Fee * group.Primary.Event.StartPage.GroupDiscount.DiscountAmount / 100.0;
                                }
                                else
                                { 
                                    total -= group.Primary.Event.StartPage.GroupDiscount.DiscountAmount;
                                    count.EventFee_Response.Fee -= group.Primary.Event.StartPage.GroupDiscount.DiscountAmount;
                                }
                            }
                        }
                    }
                }
            }

            foreach (AgendaResponseCount count in agendaResponseCount)
            {
                if ((group.Primary.Event.StartPage.GroupDiscount != null)
                    && (count.count >= group.Primary.Event.StartPage.GroupDiscount.GroupSize)
                    && (group.Primary.Event.StartPage.GroupDiscount.ShowAndApply))
                {
                    switch(count.Agenda_Response.AgendaItem.Type)
                    {
                        case FormData.CustomFieldType.AlwaysSelected:
                            AgendaResponse_AlwaysSelected respAlways = count.Agenda_Response as AgendaResponse_AlwaysSelected;
                            respAlways.Fee = total -= this.CalculateAgendaGroupDiscount(respAlways.Fee, group, count);
                            break;
                        case FormData.CustomFieldType.CheckBox:
                            AgendaResponse_Checkbox respCheck = count.Agenda_Response as AgendaResponse_Checkbox;
                            respCheck.Fee = total -= this.CalculateAgendaGroupDiscount(respCheck.Fee, group, count);
                            break;
                        case FormData.CustomFieldType.RadioButton:
                            AgendaResponse_MultipleChoice_RadioButton respRadio = count.Agenda_Response as AgendaResponse_MultipleChoice_RadioButton;
                            respRadio.Fee = total -= this.CalculateAgendaGroupDiscount(respRadio.Fee, group, count);
                            break;
                        case FormData.CustomFieldType.Dropdown:
                            AgendaResponse_MultipleChoice_DropDown respDrop = count.Agenda_Response as AgendaResponse_MultipleChoice_DropDown;
                            respDrop.Fee = total -= this.CalculateAgendaGroupDiscount(respDrop.Fee, group, count);
                            break;
                        case FormData.CustomFieldType.Contribution:
                            AgendaResponse_Contribution respContri = count.Agenda_Response as AgendaResponse_Contribution;
                            respContri.ContributionAmount = total -= this.CalculateAgendaGroupDiscount(respContri.ContributionAmount, group, count);
                            break;
                        case FormData.CustomFieldType.FileUpload:
                            AgendaResponse_FileUpload respFile = count.Agenda_Response as AgendaResponse_FileUpload;
                            respFile.Fee = total -= this.CalculateAgendaGroupDiscount(respFile.Fee, group, count);
                            break;
                        default:
                            break;
                    }
                }
            }

            return total;
        }

        private double CalculateDiscountCode(Group group)
        {
            double total = this.CalculateGroupDiscount(group);

            List<Registrant> regs = new List<Registrant>();
            regs.Add(group.Primary);

            foreach (Registrant reg in group.Secondaries)
            {
                regs.Add(reg);
            }

            foreach (Registrant reg in regs)
            {
                if ((reg.EventFee_Response != null) && (reg.EventFee_Response.Code != null))
                {
                    reg.EventFee_Response.Fee = total -= reg.EventFee_Response.Fee - reg.EventFee_Response.Code.CalculateDiscountedPrice(reg.EventFee_Response.Fee);
                }

                foreach (CustomFieldResponse responses in reg.CustomField_Responses)
                {
                    if (responses is AgendaResponse)
                    {
                        AgendaResponse response = responses as AgendaResponse;
                        
                        switch (response.AgendaItem.Type)
                        {
                            case FormData.CustomFieldType.AlwaysSelected:
                                {
                                    AgendaResponse_AlwaysSelected resp = response as AgendaResponse_AlwaysSelected;
                                    if (resp.Code != null)
                                    {
                                        resp.Fee = total -= resp.Fee - resp.Code.CalculateDiscountedPrice(resp.Fee);
                                    }
                                }
                                break;
                            case FormData.CustomFieldType.CheckBox:
                                {
                                    AgendaResponse_Checkbox resp = response as AgendaResponse_Checkbox;
                                    if (resp.Code != null)
                                    {
                                        resp.Fee = total -= resp.Fee - resp.Code.CalculateDiscountedPrice(resp.Fee);
                                    }
                                }
                                break;
                            case FormData.CustomFieldType.RadioButton:
                                {
                                    AgendaResponse_MultipleChoice_RadioButton resp = response as AgendaResponse_MultipleChoice_RadioButton;
                                    if (resp.Code != null)
                                    {
                                        resp.Fee = total -= resp.Fee - resp.Code.CalculateDiscountedPrice(resp.Fee);
                                    }
                                }
                                break;
                            case FormData.CustomFieldType.Dropdown:
                                {
                                    AgendaResponse_MultipleChoice_DropDown resp = response as AgendaResponse_MultipleChoice_DropDown;
                                    if (resp.Code != null)
                                    {
                                        resp.Fee = total -= resp.Fee - resp.Code.CalculateDiscountedPrice(resp.Fee);
                                    }
                                }
                                break;
                            case FormData.CustomFieldType.FileUpload:
                                {
                                    AgendaResponse_FileUpload resp = response as AgendaResponse_FileUpload;
                                    if (resp.Code != null)
                                    {
                                        resp.Fee = total -= resp.Fee - resp.Code.CalculateDiscountedPrice(resp.Fee);
                                    }
                                }
                                break;
                            default:
                                throw new ArgumentException(string.Format("Agenda item of specified type has no price: {0}", response.AgendaItem.Type));
                        }
                    }
                }

                foreach (MerchandiseResponse response in reg.Merchandise_Responses)
                {
                    switch (response.Merchandise_Item.Type)
                    {
                        case FormData.MerchandiseType.Fixed:
                            {
                                MerchResponse_FixedPrice resp = response as MerchResponse_FixedPrice;
                                if (resp.Discount_Code != null)
                                {
                                    resp.Fee = total -= (resp.Fee - resp.Discount_Code.CalculateDiscountedPrice(resp.Fee)) * resp.Quantity;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            return total;
        }

        private double CalculateTaxRates(Group group)
        {
            double total = this.CalculateDiscountCode(group);

            List<Registrant> regs = new List<Registrant>();
            regs.Add(group.Primary);

            foreach (Registrant reg in group.Secondaries)
            {
                regs.Add(reg);
            }

            foreach (Registrant reg in regs)
            {
                if ((reg.EventFee_Response != null) && (reg.EventFee_Response.RegType.ApplyTaxOne.HasValue)
                    && (reg.EventFee_Response.RegType.ApplyTaxOne.Value))
                {
                    if ((group.Primary.Event.TaxRateOne.Country.HasValue) && (reg.Country.HasValue))
                    {
                        if (group.Primary.Event.TaxRateOne.Country.Value == reg.Country.Value)
                        {
                            total += reg.EventFee_Response.Fee * group.Primary.Event.TaxRateOne.Rate / 100.00;
                        }
                        if ((group.Primary.Event.TaxRateOne.Country.Value == DataCollection.FormData.Countries.EU)
                            && (reg.Country.Value == FormData.Countries.Austria))
                        {
                            total += reg.EventFee_Response.Fee * group.Primary.Event.TaxRateOne.Rate / 100.00;
                        }
                    }
                    else if (!group.Primary.Event.TaxRateOne.Country.HasValue)
                    {
                        total += reg.EventFee_Response.Fee * group.Primary.Event.TaxRateOne.Rate / 100.00;
                    }
                }

                if ((reg.EventFee_Response != null) && (reg.EventFee_Response.RegType.ApplyTaxTwo.HasValue)
                    && (reg.EventFee_Response.RegType.ApplyTaxTwo.Value))
                {
                    if ((group.Primary.Event.TaxRateOne.Country.HasValue) && (reg.Country.HasValue))
                    {
                        if (group.Primary.Event.TaxRateOne.Country.Value == reg.Country.Value)
                        {
                            total += reg.EventFee_Response.Fee * group.Primary.Event.TaxRateTwo.Rate / 100.00;
                        }
                        if ((group.Primary.Event.TaxRateOne.Country.Value == DataCollection.FormData.Countries.EU)
                            && (reg.Country.Value == FormData.Countries.Austria))
                        {
                            total += reg.EventFee_Response.Fee * group.Primary.Event.TaxRateTwo.Rate / 100.00;
                        }
                    }
                    else if (!group.Primary.Event.TaxRateOne.Country.HasValue)
                    {
                        total += reg.EventFee_Response.Fee * group.Primary.Event.TaxRateTwo.Rate / 100.00;
                    }
                }

                foreach (CustomFieldResponse responses in reg.CustomField_Responses)
                {
                    if (responses is AgendaResponse)
                    {
                        AgendaResponse response = responses as AgendaResponse;

                        double taxAmount = 0;

                        switch (response.AgendaItem.Type)
                        {
                            case FormData.CustomFieldType.AlwaysSelected:
                                {
                                    AgendaResponse_AlwaysSelected resp = response as AgendaResponse_AlwaysSelected;
                                    AgendaItem_AlwaysSelected agenda = resp.AgendaItem as AgendaItem_AlwaysSelected;
                                    if ((agenda.ApplyTaxOne.HasValue) && (agenda.ApplyTaxOne.Value))
                                    {
                                        if ((group.Primary.Event.TaxRateOne.Country.HasValue) && (reg.Country.HasValue))
                                        {
                                            if (group.Primary.Event.TaxRateOne.Country.Value == reg.Country.Value)
                                            {
                                                taxAmount += resp.Fee * group.Primary.Event.TaxRateOne.Rate / 100.00;
                                            }
                                            if ((group.Primary.Event.TaxRateOne.Country.Value == DataCollection.FormData.Countries.EU)
                                                && (reg.Country.Value == FormData.Countries.Austria))
                                            {
                                                taxAmount += resp.Fee * group.Primary.Event.TaxRateOne.Rate / 100.00;
                                            }
                                        }
                                        else if (!group.Primary.Event.TaxRateOne.Country.HasValue)
                                        {
                                            taxAmount += resp.Fee * group.Primary.Event.TaxRateOne.Rate / 100.00;
                                        }
                                    }
                                    if ((agenda.ApplyTaxTwo.HasValue) && (agenda.ApplyTaxTwo.Value))
                                    {
                                        if ((group.Primary.Event.TaxRateOne.Country.HasValue) && (reg.Country.HasValue))
                                        {
                                            if (group.Primary.Event.TaxRateOne.Country.Value == reg.Country.Value)
                                            {
                                                taxAmount += resp.Fee * group.Primary.Event.TaxRateTwo.Rate / 100.00;
                                            }
                                            if ((group.Primary.Event.TaxRateOne.Country.Value == DataCollection.FormData.Countries.EU)
                                                && (reg.Country.Value == FormData.Countries.Austria))
                                            {
                                                taxAmount += resp.Fee * group.Primary.Event.TaxRateTwo.Rate / 100.00;
                                            }
                                        }
                                        else if (!group.Primary.Event.TaxRateOne.Country.HasValue)
                                        {
                                            taxAmount += resp.Fee * group.Primary.Event.TaxRateTwo.Rate / 100.00;
                                        }
                                    }
                                }
                                break;
                            case FormData.CustomFieldType.CheckBox:
                                {
                                    AgendaResponse_Checkbox resp = response as AgendaResponse_Checkbox;
                                    AgendaItem_CheckBox agenda = resp.AgendaItem as AgendaItem_CheckBox;
                                    if ((agenda.ApplyTaxOne.HasValue) && (agenda.ApplyTaxOne.Value))
                                    {
                                        if ((group.Primary.Event.TaxRateOne.Country.HasValue) && (reg.Country.HasValue))
                                        {
                                            if (group.Primary.Event.TaxRateOne.Country.Value == reg.Country.Value)
                                            {
                                                taxAmount += resp.Fee * group.Primary.Event.TaxRateOne.Rate / 100.00;
                                            }
                                            if ((group.Primary.Event.TaxRateOne.Country.Value == DataCollection.FormData.Countries.EU)
                                                && (reg.Country.Value == FormData.Countries.Austria))
                                            {
                                                taxAmount += resp.Fee * group.Primary.Event.TaxRateOne.Rate / 100.00;
                                            }
                                        }
                                        else if (!group.Primary.Event.TaxRateOne.Country.HasValue)
                                        {
                                            taxAmount += resp.Fee * group.Primary.Event.TaxRateOne.Rate / 100.00;
                                        }
                                    }
                                    if ((agenda.ApplyTaxTwo.HasValue) && (agenda.ApplyTaxTwo.Value))
                                    {
                                        if ((group.Primary.Event.TaxRateOne.Country.HasValue) && (reg.Country.HasValue))
                                        {
                                            if (group.Primary.Event.TaxRateOne.Country.Value == reg.Country.Value)
                                            {
                                                taxAmount += resp.Fee * group.Primary.Event.TaxRateTwo.Rate / 100.00;
                                            }
                                            if ((group.Primary.Event.TaxRateOne.Country.Value == DataCollection.FormData.Countries.EU)
                                                && (reg.Country.Value == FormData.Countries.Austria))
                                            {
                                                taxAmount += resp.Fee * group.Primary.Event.TaxRateTwo.Rate / 100.00;
                                            }
                                        }
                                        else if (!group.Primary.Event.TaxRateOne.Country.HasValue)
                                        {
                                            taxAmount += resp.Fee * group.Primary.Event.TaxRateTwo.Rate / 100.00;
                                        }
                                    }
                                }
                                break;
                            case FormData.CustomFieldType.RadioButton:
                                {
                                    AgendaResponse_MultipleChoice_RadioButton resp = response as AgendaResponse_MultipleChoice_RadioButton;
                                    AgendaItem_MultipleChoice_RadioButton agenda = resp.AgendaItem as AgendaItem_MultipleChoice_RadioButton;
                                    if ((agenda.ApplyTaxOne.HasValue) && (agenda.ApplyTaxOne.Value))
                                    {
                                        if ((group.Primary.Event.TaxRateOne.Country.HasValue) && (reg.Country.HasValue))
                                        {
                                            if (group.Primary.Event.TaxRateOne.Country.Value == reg.Country.Value)
                                            {
                                                taxAmount += resp.Fee * group.Primary.Event.TaxRateOne.Rate / 100.00;
                                            }
                                            if ((group.Primary.Event.TaxRateOne.Country.Value == DataCollection.FormData.Countries.EU)
                                                && (reg.Country.Value == FormData.Countries.Austria))
                                            {
                                                taxAmount += resp.Fee * group.Primary.Event.TaxRateOne.Rate / 100.00;
                                            }
                                        }
                                        else if (!group.Primary.Event.TaxRateOne.Country.HasValue)
                                        {
                                            taxAmount += resp.Fee * group.Primary.Event.TaxRateOne.Rate / 100.00;
                                        }
                                    }
                                    if ((agenda.ApplyTaxTwo.HasValue) && (agenda.ApplyTaxTwo.Value))
                                    {
                                        if ((group.Primary.Event.TaxRateOne.Country.HasValue) && (reg.Country.HasValue))
                                        {
                                            if (group.Primary.Event.TaxRateOne.Country.Value == reg.Country.Value)
                                            {
                                                taxAmount += resp.Fee * group.Primary.Event.TaxRateTwo.Rate / 100.00;
                                            }
                                            if ((group.Primary.Event.TaxRateOne.Country.Value == DataCollection.FormData.Countries.EU)
                                                && (reg.Country.Value == FormData.Countries.Austria))
                                            {
                                                taxAmount += resp.Fee * group.Primary.Event.TaxRateTwo.Rate / 100.00;
                                            }
                                        }
                                        else if (!group.Primary.Event.TaxRateOne.Country.HasValue)
                                        {
                                            taxAmount += resp.Fee * group.Primary.Event.TaxRateTwo.Rate / 100.00;
                                        }
                                    }
                                }
                                break;
                            case FormData.CustomFieldType.Dropdown:
                                {
                                    AgendaResponse_MultipleChoice_DropDown resp = response as AgendaResponse_MultipleChoice_DropDown;
                                    AgendaItem_MultipleChoice_DropDown agenda = resp.AgendaItem as AgendaItem_MultipleChoice_DropDown;
                                    if ((agenda.ApplyTaxOne.HasValue) && (agenda.ApplyTaxOne.Value))
                                    {
                                        if ((group.Primary.Event.TaxRateOne.Country.HasValue) && (reg.Country.HasValue))
                                        {
                                            if (group.Primary.Event.TaxRateOne.Country.Value == reg.Country.Value)
                                            {
                                                taxAmount += resp.Fee * group.Primary.Event.TaxRateOne.Rate / 100.00;
                                            }
                                            if ((group.Primary.Event.TaxRateOne.Country.Value == DataCollection.FormData.Countries.EU)
                                                && (reg.Country.Value == FormData.Countries.Austria))
                                            {
                                                taxAmount += resp.Fee * group.Primary.Event.TaxRateOne.Rate / 100.00;
                                            }
                                        }
                                        else if (!group.Primary.Event.TaxRateOne.Country.HasValue)
                                        {
                                            taxAmount += resp.Fee * group.Primary.Event.TaxRateOne.Rate / 100.00;
                                        }
                                    }
                                    if ((agenda.ApplyTaxTwo.HasValue) && (agenda.ApplyTaxTwo.Value))
                                    {
                                        if ((group.Primary.Event.TaxRateOne.Country.HasValue) && (reg.Country.HasValue))
                                        {
                                            if (group.Primary.Event.TaxRateOne.Country.Value == reg.Country.Value)
                                            {
                                                taxAmount += resp.Fee * group.Primary.Event.TaxRateTwo.Rate / 100.00;
                                            }
                                            if ((group.Primary.Event.TaxRateOne.Country.Value == DataCollection.FormData.Countries.EU)
                                                && (reg.Country.Value == FormData.Countries.Austria))
                                            {
                                                taxAmount += resp.Fee * group.Primary.Event.TaxRateTwo.Rate / 100.00;
                                            }
                                        }
                                        else if (!group.Primary.Event.TaxRateOne.Country.HasValue)
                                        {
                                            taxAmount += resp.Fee * group.Primary.Event.TaxRateTwo.Rate / 100.00;
                                        }
                                    }
                                }
                                break;
                            case FormData.CustomFieldType.FileUpload:
                                {
                                    AgendaResponse_FileUpload resp = response as AgendaResponse_FileUpload;
                                    AgendaItem_FileUpload agenda = resp.AgendaItem as AgendaItem_FileUpload;
                                    if ((agenda.ApplyTaxOne.HasValue) && (agenda.ApplyTaxOne.Value))
                                    {
                                        if ((group.Primary.Event.TaxRateOne.Country.HasValue) && (reg.Country.HasValue))
                                        {
                                            if (group.Primary.Event.TaxRateOne.Country.Value == reg.Country.Value)
                                            {
                                                taxAmount += resp.Fee * group.Primary.Event.TaxRateOne.Rate / 100.00;
                                            }
                                            if ((group.Primary.Event.TaxRateOne.Country.Value == DataCollection.FormData.Countries.EU)
                                                && (reg.Country.Value == FormData.Countries.Austria))
                                            {
                                                taxAmount += resp.Fee * group.Primary.Event.TaxRateOne.Rate / 100.00;
                                            }
                                        }
                                        else if (!group.Primary.Event.TaxRateOne.Country.HasValue)
                                        {
                                            taxAmount += resp.Fee * group.Primary.Event.TaxRateOne.Rate / 100.00;
                                        }
                                    }
                                    if ((agenda.ApplyTaxTwo.HasValue) && (agenda.ApplyTaxTwo.Value))
                                    {
                                        if ((group.Primary.Event.TaxRateOne.Country.HasValue) && (reg.Country.HasValue))
                                        {
                                            if (group.Primary.Event.TaxRateOne.Country.Value == reg.Country.Value)
                                            {
                                                taxAmount += resp.Fee * group.Primary.Event.TaxRateTwo.Rate / 100.00;
                                            }
                                            if ((group.Primary.Event.TaxRateOne.Country.Value == DataCollection.FormData.Countries.EU)
                                                && (reg.Country.Value == FormData.Countries.Austria))
                                            {
                                                taxAmount += resp.Fee * group.Primary.Event.TaxRateTwo.Rate / 100.00;
                                            }
                                        }
                                        else if (!group.Primary.Event.TaxRateOne.Country.HasValue)
                                        {
                                            taxAmount += resp.Fee * group.Primary.Event.TaxRateTwo.Rate / 100.00;
                                        }
                                    }
                                }
                                break;
                            default:
                                break;
                        }

                        total += taxAmount;
                    }
                }

                foreach (MerchandiseResponse response in reg.Merchandise_Responses)
                {
                    switch (response.Merchandise_Item.Type)
                    {
                        case FormData.MerchandiseType.Fixed:
                            {
                                MerchResponse_FixedPrice resp = response as MerchResponse_FixedPrice;
                                if ((resp.Merchandise_Item.ApplyTaxOne.HasValue)
                                    && (resp.Merchandise_Item.ApplyTaxOne.Value))
                                {
                                    if ((group.Primary.Event.TaxRateOne.Country.HasValue) && (reg.Country.HasValue))
                                    {
                                        if (group.Primary.Event.TaxRateOne.Country.Value == reg.Country.Value)
                                        {
                                            total += resp.Fee * group.Primary.Event.TaxRateOne.Rate / 100.00;
                                        }
                                        if ((group.Primary.Event.TaxRateOne.Country.Value == DataCollection.FormData.Countries.EU)
                                            && (reg.Country.Value == FormData.Countries.Austria))
                                        {
                                            total += resp.Fee * group.Primary.Event.TaxRateOne.Rate / 100.00;
                                        }
                                    }
                                    else if (!group.Primary.Event.TaxRateOne.Country.HasValue)
                                    {
                                        total += resp.Fee * group.Primary.Event.TaxRateOne.Rate / 100.00;
                                    }
                                }
                                if ((resp.Merchandise_Item.ApplyTaxTwo.HasValue)
                                    && (resp.Merchandise_Item.ApplyTaxTwo.Value))
                                {
                                    if ((group.Primary.Event.TaxRateOne.Country.HasValue) && (reg.Country.HasValue))
                                    {
                                        if (group.Primary.Event.TaxRateOne.Country.Value == reg.Country.Value)
                                        {
                                            total += resp.Fee * group.Primary.Event.TaxRateTwo.Rate / 100.00;
                                        }
                                        if ((group.Primary.Event.TaxRateOne.Country.Value == DataCollection.FormData.Countries.EU)
                                            && (reg.Country.Value == FormData.Countries.Austria))
                                        {
                                            total += resp.Fee * group.Primary.Event.TaxRateTwo.Rate / 100.00;
                                        }
                                    }
                                    else if (!group.Primary.Event.TaxRateOne.Country.HasValue)
                                    {
                                        total += resp.Fee * group.Primary.Event.TaxRateTwo.Rate / 100.00;
                                    }
                                }
                            }
                            break;
                        case FormData.MerchandiseType.Variable:
                            {
                                MerchResponse_VariableAmount resp = response as MerchResponse_VariableAmount;
                                if ((resp.Merchandise_Item.ApplyTaxOne.HasValue)
                                    && (resp.Merchandise_Item.ApplyTaxOne.Value))
                                {
                                    if ((group.Primary.Event.TaxRateOne.Country.HasValue) && (reg.Country.HasValue))
                                    {
                                        if (group.Primary.Event.TaxRateOne.Country.Value == reg.Country.Value)
                                        {
                                            total += resp.Fee * group.Primary.Event.TaxRateOne.Rate / 100.00;
                                        }
                                        if ((group.Primary.Event.TaxRateOne.Country.Value == DataCollection.FormData.Countries.EU)
                                            && (reg.Country.Value == FormData.Countries.Austria))
                                        {
                                            total += resp.Fee * group.Primary.Event.TaxRateOne.Rate / 100.00;
                                        }
                                    }
                                    else if (!group.Primary.Event.TaxRateOne.Country.HasValue)
                                    {
                                        total += resp.Fee * group.Primary.Event.TaxRateOne.Rate / 100.00;
                                    }
                                }
                                if ((resp.Merchandise_Item.ApplyTaxTwo.HasValue)
                                    && (resp.Merchandise_Item.ApplyTaxTwo.Value))
                                {
                                    if ((group.Primary.Event.TaxRateOne.Country.HasValue) && (reg.Country.HasValue))
                                    {
                                        if (group.Primary.Event.TaxRateOne.Country.Value == reg.Country.Value)
                                        {
                                            total += resp.Fee * group.Primary.Event.TaxRateTwo.Rate / 100.00;
                                        }
                                        if ((group.Primary.Event.TaxRateOne.Country.Value == DataCollection.FormData.Countries.EU)
                                            && (reg.Country.Value == FormData.Countries.Austria))
                                        {
                                            total += resp.Fee * group.Primary.Event.TaxRateTwo.Rate / 100.00;
                                        }
                                    }
                                    else if (!group.Primary.Event.TaxRateOne.Country.HasValue)
                                    {
                                        total += resp.Fee * group.Primary.Event.TaxRateTwo.Rate / 100.00;
                                    }
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            return total;
        }

        private double CalculateAgendaEarlyLateFee(AgendaItem_Common agenda, Event evt)
        {
            double agendaFee = 0;

            if (agenda.EarlyPrice != null)
            {
                if ((agenda.EarlyPrice.EarlyPriceType == FormData.EarlyPriceType.DateAndTime)
                    && (agenda.EarlyPrice.EarlyPriceDate.Value > DateTime.Now))
                {
                    agendaFee = agenda.EarlyPrice.earlyPrice;
                }
                else if (agenda.EarlyPrice.EarlyPriceType == FormData.EarlyPriceType.Registrants)
                {
                    int previousRegsForThisAgenda = 0;

                    foreach (Registrant previousReg in evt.Registrants)
                    {
                        foreach (CustomFieldResponse cfResponse in previousReg.CustomField_Responses)
                        {
                            if (cfResponse is AgendaResponse)
                            {
                                AgendaResponse agResponse = cfResponse as AgendaResponse;

                                if (agResponse.AgendaItem.NameOnForm == agenda.NameOnForm)
                                {
                                    previousRegsForThisAgenda += 1;
                                }
                            }
                        }
                    }

                    if (agenda.EarlyPrice.FirstNRegistrants.Value >= previousRegsForThisAgenda)
                    {
                        agendaFee = agenda.EarlyPrice.earlyPrice;
                    }
                    else
                    {
                        agendaFee = agenda.Price.Value;
                    }
                }
            }
            else if ((agenda.LatePrice != null)
                && (agenda.LatePrice.LatePriceDate < DateTime.Now))
            {
                agendaFee = agenda.LatePrice.latePrice;
            }
            else
            {
                agendaFee = agenda.Price.Value;
            }

            return agendaFee;
        }

        private double CalculateAgendaGroupDiscount(double agendaFee, Group group, AgendaResponseCount count)
        {
            double total = 0;

            if (group.Primary.Event.StartPage.GroupDiscount.GroupSizeOption == GroupDiscount_GroupSizeOption.SizeOrMore)
            {
                if (group.Primary.Event.StartPage.GroupDiscount.GroupDiscountType == GroupDiscount_DiscountType.Percent)
                {
                    total += agendaFee * group.Primary.Event.StartPage.GroupDiscount.DiscountAmount / 100.0 * count.count;
                }
                else
                {
                    total += group.Primary.Event.StartPage.GroupDiscount.DiscountAmount * count.count;
                }
            }
            else if (group.Primary.Event.StartPage.GroupDiscount.AddtionalRegOption == GroupDiscount_AdditionalRegOption.All)
            {
                for (int i = 1; i <= count.count; i++)
                {
                    if (i % group.Primary.Event.StartPage.GroupDiscount.GroupSize == 0)
                    {
                        if (group.Primary.Event.StartPage.GroupDiscount.GroupDiscountType == GroupDiscount_DiscountType.Percent)
                        {
                            total += agendaFee * group.Primary.Event.StartPage.GroupDiscount.DiscountAmount / 100.0 * group.Primary.Event.StartPage.GroupDiscount.GroupSize;
                        }
                        else
                        {
                            total += group.Primary.Event.StartPage.GroupDiscount.DiscountAmount * group.Primary.Event.StartPage.GroupDiscount.GroupSize;
                        }
                    }
                }
            }
            else if (group.Primary.Event.StartPage.GroupDiscount.AddtionalRegOption == GroupDiscount_AdditionalRegOption.AnyAdditional)
            {
                if (group.Primary.Event.StartPage.GroupDiscount.GroupDiscountType == GroupDiscount_DiscountType.Percent)
                {
                    total += agendaFee * group.Primary.Event.StartPage.GroupDiscount.DiscountAmount / 100.0 * (count.count - group.Primary.Event.StartPage.GroupDiscount.GroupSize);
                }
                else
                {
                    total += group.Primary.Event.StartPage.GroupDiscount.DiscountAmount * (count.count - group.Primary.Event.StartPage.GroupDiscount.GroupSize);
                }
            }
            else if (group.Primary.Event.StartPage.GroupDiscount.AddtionalRegOption == GroupDiscount_AdditionalRegOption.Additional)
            {
                for (int i = 1; i <= count.count; i++)
                {
                    if ((i > group.Primary.Event.StartPage.GroupDiscount.GroupSize) && (i <= group.Primary.Event.StartPage.GroupDiscount.NumberOfAdditionalReg.Value + count.count))
                    {
                        if (group.Primary.Event.StartPage.GroupDiscount.GroupDiscountType == GroupDiscount_DiscountType.Percent)
                        {
                            total += agendaFee * group.Primary.Event.StartPage.GroupDiscount.DiscountAmount / 100.0;
                        }
                        else
                        {
                            total += group.Primary.Event.StartPage.GroupDiscount.DiscountAmount;
                        }
                    }
                }
            }

            return total;
        }

        private class EventFeeResponseCount
        {
            public EventFeeResponse EventFee_Response;
            public int count;
        }

        private class AgendaResponseCount
        {
            public AgendaResponse Agenda_Response;
            public int count;
        }

    }
}
