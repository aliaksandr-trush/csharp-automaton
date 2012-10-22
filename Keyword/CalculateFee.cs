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

            foreach (LodgingResponse response in reg.Lodging_Responses)
            {
                foreach (RoomBlock block in response.Hotel.RoomBlocks)
                {
                    if ((block.Date >= response.CheckinDate) && (block.Date <= response.CheckoutDate))
                    {
                        reg.Fee_Summary.Total += response.RoomType.RoomRate;
                    }
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

            if (group.Primary.Event.MerchandisePage.ShippingFee.HasValue)
            {
                total += group.Primary.Event.MerchandisePage.ShippingFee.Value;
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

            Dictionary<RegType, List<EventFeeResponse>> responseDic = new Dictionary<RegType, List<EventFeeResponse>>();
            Dictionary<AgendaItem, List<AgendaResponse>> agendaDic = new Dictionary<AgendaItem, List<AgendaResponse>>();

            foreach (EventFeeResponse resp in regEventFeeResponses)
            {
                if (responseDic.ContainsKey(resp.RegType))
                {
                    responseDic[resp.RegType].Add(resp);
                }
                else if ((group.Primary.Event.StartPage.GroupDiscount != null)
                    && group.Primary.Event.StartPage.GroupDiscount.ApplyToRegTypes.Count != 0)
                {
                    if (group.Primary.Event.StartPage.GroupDiscount.ApplyToRegTypes.Exists(r => r == resp.RegType))
                    {
                        List<EventFeeResponse> regTypeResponse = new List<EventFeeResponse>();
                        regTypeResponse.Add(resp);
                        responseDic.Add(resp.RegType, regTypeResponse);
                    }
                }
                else
                {
                    List<EventFeeResponse> regTypeResponse = new List<EventFeeResponse>();
                    regTypeResponse.Add(resp);
                    responseDic.Add(resp.RegType, regTypeResponse);
                }
            }

            foreach (AgendaResponse resp in regAgendaResponses)
            {
                if (agendaDic.ContainsKey(resp.AgendaItem))
                {
                    agendaDic[resp.AgendaItem].Add(resp);
                }
                else if ((group.Primary.Event.StartPage.GroupDiscount != null)
                    && group.Primary.Event.StartPage.GroupDiscount.ApplyToAgendaItems.Count != 0)
                {
                    if (group.Primary.Event.StartPage.GroupDiscount.ApplyToAgendaItems.Exists(a => a == resp.AgendaItem))
                    {
                        List<AgendaResponse> agendaResponse = new List<AgendaResponse>();
                        agendaResponse.Add(resp);
                        agendaDic.Add(resp.AgendaItem, agendaResponse);
                    }
                }
                else
                {
                    List<AgendaResponse> agendaResponse = new List<AgendaResponse>();
                    agendaResponse.Add(resp);
                    agendaDic.Add(resp.AgendaItem, agendaResponse);
                }
                
            }

            foreach(KeyValuePair<RegType, List<EventFeeResponse>> dic in responseDic)
            {
                if ((group.Primary.Event.StartPage.GroupDiscount != null) && 
                    (dic.Value.Count >= group.Primary.Event.StartPage.GroupDiscount.GroupSize) &&
                    (group.Primary.Event.StartPage.GroupDiscount.ShowAndApply))
                {
                    if (group.Primary.Event.StartPage.GroupDiscount.GroupSizeOption == GroupDiscount_GroupSizeOption.SizeOrMore)
                    {
                        if (group.Primary.Event.StartPage.GroupDiscount.GroupDiscountType == GroupDiscount_DiscountType.Percent)
                        {
                            total -= dic.Value[0].Fee * group.Primary.Event.StartPage.GroupDiscount.DiscountAmount / 100.0 * dic.Value.Count;
                            for (int i = 0; i < dic.Value.Count; i++)
                            {
                                dic.Value[i].Fee -= dic.Value[i].Fee * group.Primary.Event.StartPage.GroupDiscount.DiscountAmount / 100.0;
                            }
                        }
                        else
                        {
                            total -= group.Primary.Event.StartPage.GroupDiscount.DiscountAmount * dic.Value.Count;
                            for (int i = 0; i < dic.Value.Count; i++)
                            {
                                dic.Value[i].Fee -= group.Primary.Event.StartPage.GroupDiscount.DiscountAmount;
                            }
                        }
                    }
                    else if (group.Primary.Event.StartPage.GroupDiscount.AddtionalRegOption == GroupDiscount_AdditionalRegOption.All)
                    {
                        for (int i = 1; i <= dic.Value.Count; i++)
                        {
                            if (i % group.Primary.Event.StartPage.GroupDiscount.GroupSize == 0)
                            {
                                if (group.Primary.Event.StartPage.GroupDiscount.GroupDiscountType == GroupDiscount_DiscountType.Percent)
                                {
                                    total -= dic.Value[0].Fee * group.Primary.Event.StartPage.GroupDiscount.DiscountAmount / 100.0 * group.Primary.Event.StartPage.GroupDiscount.GroupSize;
                                    for (int j = 0; j < dic.Value.Count; j++)
                                    {
                                        dic.Value[j].Fee -= dic.Value[j].Fee * group.Primary.Event.StartPage.GroupDiscount.DiscountAmount / 100.0;
                                    }
                                }
                                else
                                {
                                    total -= group.Primary.Event.StartPage.GroupDiscount.DiscountAmount * group.Primary.Event.StartPage.GroupDiscount.GroupSize;
                                    for (int j = 0; j < dic.Value.Count; j++)
                                    {
                                        dic.Value[j].Fee -= group.Primary.Event.StartPage.GroupDiscount.DiscountAmount;
                                    }
                                }
                            }
                        }
                    }
                    else if (group.Primary.Event.StartPage.GroupDiscount.AddtionalRegOption == GroupDiscount_AdditionalRegOption.AnyAdditional)
                    {
                        if (group.Primary.Event.StartPage.GroupDiscount.GroupDiscountType == GroupDiscount_DiscountType.Percent)
                        {
                            total -= dic.Value[0].Fee * group.Primary.Event.StartPage.GroupDiscount.DiscountAmount / 100.0 * (dic.Value.Count - group.Primary.Event.StartPage.GroupDiscount.GroupSize);
                            for (int i = 0; i < dic.Value.Count; i++)
                            {
                                dic.Value[i].Fee -= dic.Value[1].Fee * group.Primary.Event.StartPage.GroupDiscount.DiscountAmount / 100.0;
                            }
                        }
                        else
                        {
                            total -= group.Primary.Event.StartPage.GroupDiscount.DiscountAmount * (dic.Value.Count - group.Primary.Event.StartPage.GroupDiscount.GroupSize);
                            for (int i = 0; i < dic.Value.Count; i++)
                            {
                                dic.Value[i].Fee -= group.Primary.Event.StartPage.GroupDiscount.DiscountAmount;
                            }
                        } 
                    }
                    else if (group.Primary.Event.StartPage.GroupDiscount.AddtionalRegOption == GroupDiscount_AdditionalRegOption.Additional)
                    {
                        for (int i = 1; i <= dic.Value.Count; i++)
                        {
                            if ((i > group.Primary.Event.StartPage.GroupDiscount.GroupSize) && (i <= group.Primary.Event.StartPage.GroupDiscount.NumberOfAdditionalReg.Value))
                            {
                                if (group.Primary.Event.StartPage.GroupDiscount.GroupDiscountType == GroupDiscount_DiscountType.Percent)
                                {
                                    total -= dic.Value[i].Fee * group.Primary.Event.StartPage.GroupDiscount.DiscountAmount / 100.0;
                                    dic.Value[i].Fee -= dic.Value[i].Fee * group.Primary.Event.StartPage.GroupDiscount.DiscountAmount / 100.0;
                                }
                                else
                                { 
                                    total -= group.Primary.Event.StartPage.GroupDiscount.DiscountAmount;
                                    dic.Value[i].Fee -= group.Primary.Event.StartPage.GroupDiscount.DiscountAmount;
                                }
                            }
                        }
                    }
                }
            }

            foreach (KeyValuePair<AgendaItem, List<AgendaResponse>> dic in agendaDic)
            {
                if ((group.Primary.Event.StartPage.GroupDiscount != null)
                    && (dic.Value.Count >= group.Primary.Event.StartPage.GroupDiscount.GroupSize)
                    && (group.Primary.Event.StartPage.GroupDiscount.ShowAndApply))
                {
                    switch(dic.Value[0].AgendaItem.Type)
                    {
                        case FormData.CustomFieldType.AlwaysSelected:
                            AgendaResponse_AlwaysSelected respAlways = dic.Value[0] as AgendaResponse_AlwaysSelected;
                            total -= this.CalculateAgendaGroupDiscount(respAlways.Fee, group, dic.Value);
                            for (int i = 0; i < dic.Value.Count; i++)
                            {
                                AgendaResponse_AlwaysSelected resp = dic.Value[i] as AgendaResponse_AlwaysSelected;
                                resp.Fee -= this.CalculateAgendaGroupDiscount(resp.Fee, group, dic.Value) / dic.Value.Count;
                            }
                            break;
                        case FormData.CustomFieldType.CheckBox:
                            AgendaResponse_Checkbox respCheck = dic.Value[0] as AgendaResponse_Checkbox;
                            total -= this.CalculateAgendaGroupDiscount(respCheck.Fee, group, dic.Value);
                            for (int i = 0; i < dic.Value.Count; i++)
                            {
                                AgendaResponse_Checkbox resp = dic.Value[i] as AgendaResponse_Checkbox;
                                resp.Fee -= this.CalculateAgendaGroupDiscount(resp.Fee, group, dic.Value) / dic.Value.Count;
                            }
                            break;
                        case FormData.CustomFieldType.RadioButton:
                            AgendaResponse_MultipleChoice_RadioButton respRadio = dic.Value[0] as AgendaResponse_MultipleChoice_RadioButton;
                            total -= this.CalculateAgendaGroupDiscount(respRadio.Fee, group, dic.Value);
                            for (int i = 0; i < dic.Value.Count; i++)
                            {
                                AgendaResponse_MultipleChoice_RadioButton resp = dic.Value[i] as AgendaResponse_MultipleChoice_RadioButton;
                                resp.Fee -= this.CalculateAgendaGroupDiscount(resp.Fee, group, dic.Value) / dic.Value.Count;
                            }
                            break;
                        case FormData.CustomFieldType.Dropdown:
                            AgendaResponse_MultipleChoice_DropDown respDrop = dic.Value[0] as AgendaResponse_MultipleChoice_DropDown;
                            total -= this.CalculateAgendaGroupDiscount(respDrop.Fee, group, dic.Value);
                            for (int i = 0; i < dic.Value.Count; i++)
                            {
                                AgendaResponse_MultipleChoice_DropDown resp = dic.Value[i] as AgendaResponse_MultipleChoice_DropDown;
                                resp.Fee -= this.CalculateAgendaGroupDiscount(resp.Fee, group, dic.Value) / dic.Value.Count;
                            }
                            break;
                        case FormData.CustomFieldType.Contribution:
                            AgendaResponse_Contribution respContri = dic.Value[0] as AgendaResponse_Contribution;
                            total -= this.CalculateAgendaGroupDiscount(respContri.ContributionAmount, group, dic.Value);
                            for (int i = 0; i < dic.Value.Count; i++)
                            {
                                AgendaResponse_Contribution resp = dic.Value[i] as AgendaResponse_Contribution;
                                resp.ContributionAmount -= this.CalculateAgendaGroupDiscount(resp.ContributionAmount, group, dic.Value) / dic.Value.Count;
                            }
                            break;
                        case FormData.CustomFieldType.FileUpload:
                            AgendaResponse_FileUpload respFile = dic.Value[0] as AgendaResponse_FileUpload;
                            total -= this.CalculateAgendaGroupDiscount(respFile.Fee, group, dic.Value);
                            for (int i = 0; i < dic.Value.Count; i++)
                            {
                                AgendaResponse_FileUpload resp = dic.Value[i] as AgendaResponse_FileUpload;
                                resp.Fee -= this.CalculateAgendaGroupDiscount(resp.Fee, group, dic.Value) / dic.Value.Count;
                            }
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
                    total -= reg.EventFee_Response.Fee - reg.EventFee_Response.Code.CalculateDiscountedPrice(reg.EventFee_Response.Fee);
                    reg.EventFee_Response.Fee -= reg.EventFee_Response.Fee - reg.EventFee_Response.Code.CalculateDiscountedPrice(reg.EventFee_Response.Fee);
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
                                        total -= resp.Fee - resp.Code.CalculateDiscountedPrice(resp.Fee);
                                        resp.Fee -= resp.Fee - resp.Code.CalculateDiscountedPrice(resp.Fee);
                                    }
                                }
                                break;
                            case FormData.CustomFieldType.CheckBox:
                                {
                                    AgendaResponse_Checkbox resp = response as AgendaResponse_Checkbox;
                                    if (resp.Code != null)
                                    {
                                        total -= resp.Fee - resp.Code.CalculateDiscountedPrice(resp.Fee);
                                        resp.Fee -= resp.Fee - resp.Code.CalculateDiscountedPrice(resp.Fee);
                                    }
                                }
                                break;
                            case FormData.CustomFieldType.RadioButton:
                                {
                                    AgendaResponse_MultipleChoice_RadioButton resp = response as AgendaResponse_MultipleChoice_RadioButton;
                                    if (resp.Code != null)
                                    {
                                        total -= resp.Fee - resp.Code.CalculateDiscountedPrice(resp.Fee);
                                        resp.Fee -= resp.Fee - resp.Code.CalculateDiscountedPrice(resp.Fee);
                                    }
                                }
                                break;
                            case FormData.CustomFieldType.Dropdown:
                                {
                                    AgendaResponse_MultipleChoice_DropDown resp = response as AgendaResponse_MultipleChoice_DropDown;
                                    if (resp.Code != null)
                                    {
                                        total -= resp.Fee - resp.Code.CalculateDiscountedPrice(resp.Fee);
                                        resp.Fee -= resp.Fee - resp.Code.CalculateDiscountedPrice(resp.Fee);
                                    }
                                }
                                break;
                            case FormData.CustomFieldType.FileUpload:
                                {
                                    AgendaResponse_FileUpload resp = response as AgendaResponse_FileUpload;
                                    if (resp.Code != null)
                                    {
                                        total -= resp.Fee - resp.Code.CalculateDiscountedPrice(resp.Fee);
                                        resp.Fee -= resp.Fee - resp.Code.CalculateDiscountedPrice(resp.Fee);
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
                                    total -= (resp.Fee - resp.Discount_Code.CalculateDiscountedPrice(resp.Fee)) * resp.Quantity;
                                    resp.Fee -= resp.Fee - resp.Discount_Code.CalculateDiscountedPrice(resp.Fee);
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
            double amountApplyTax1 = 0;
            double amountApplyTax2 = 0;
            double tax1 = 0;
            double tax2 = 0;

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
                    amountApplyTax1 += reg.EventFee_Response.Fee;
                }
                if ((reg.EventFee_Response != null) && (reg.EventFee_Response.RegType.ApplyTaxTwo.HasValue)
                    && (reg.EventFee_Response.RegType.ApplyTaxTwo.Value))
                {
                    amountApplyTax2 += reg.EventFee_Response.Fee;
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
                                    AgendaItem_AlwaysSelected agenda = resp.AgendaItem as AgendaItem_AlwaysSelected;
                                    if ((agenda.ApplyTaxOne.HasValue) && (agenda.ApplyTaxOne.Value))
                                    {
                                        if ((group.Primary.Event.TaxRateOne.Country.HasValue) && (reg.Country.HasValue))
                                        {
                                            if (group.Primary.Event.TaxRateOne.Country.Value == reg.Country.Value)
                                            {
                                                amountApplyTax1 += resp.Fee;
                                            }
                                            if ((group.Primary.Event.TaxRateOne.Country.Value == DataCollection.FormData.Countries.EU)
                                                && (reg.Country.Value == FormData.Countries.Austria))
                                            {
                                                amountApplyTax1 += resp.Fee;
                                            }
                                        }
                                        else if (!group.Primary.Event.TaxRateOne.Country.HasValue)
                                        {
                                            amountApplyTax1 += resp.Fee;
                                        }
                                    }
                                    if ((agenda.ApplyTaxTwo.HasValue) && (agenda.ApplyTaxTwo.Value))
                                    {
                                        if ((group.Primary.Event.TaxRateOne.Country.HasValue) && (reg.Country.HasValue))
                                        {
                                            if (group.Primary.Event.TaxRateOne.Country.Value == reg.Country.Value)
                                            {
                                                amountApplyTax2 += resp.Fee;
                                            }
                                            if ((group.Primary.Event.TaxRateOne.Country.Value == DataCollection.FormData.Countries.EU)
                                                && (reg.Country.Value == FormData.Countries.Austria))
                                            {
                                                amountApplyTax2 += resp.Fee;
                                            }
                                        }
                                        else if (!group.Primary.Event.TaxRateOne.Country.HasValue)
                                        {
                                            amountApplyTax2 += resp.Fee;
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
                                                amountApplyTax1 += resp.Fee;
                                            }
                                            if ((group.Primary.Event.TaxRateOne.Country.Value == DataCollection.FormData.Countries.EU)
                                                && (reg.Country.Value == FormData.Countries.Austria))
                                            {
                                                amountApplyTax1 += resp.Fee;
                                            }
                                        }
                                        else if (!group.Primary.Event.TaxRateOne.Country.HasValue)
                                        {
                                            amountApplyTax1 += resp.Fee;
                                        }
                                    }
                                    if ((agenda.ApplyTaxTwo.HasValue) && (agenda.ApplyTaxTwo.Value))
                                    {
                                        if ((group.Primary.Event.TaxRateOne.Country.HasValue) && (reg.Country.HasValue))
                                        {
                                            if (group.Primary.Event.TaxRateOne.Country.Value == reg.Country.Value)
                                            {
                                                amountApplyTax2 += resp.Fee;
                                            }
                                            if ((group.Primary.Event.TaxRateOne.Country.Value == DataCollection.FormData.Countries.EU)
                                                && (reg.Country.Value == FormData.Countries.Austria))
                                            {
                                                amountApplyTax2 += resp.Fee;
                                            }
                                        }
                                        else if (!group.Primary.Event.TaxRateOne.Country.HasValue)
                                        {
                                            amountApplyTax2 += resp.Fee;
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
                                                amountApplyTax1 += resp.Fee;
                                            }
                                            if ((group.Primary.Event.TaxRateOne.Country.Value == DataCollection.FormData.Countries.EU)
                                                && (reg.Country.Value == FormData.Countries.Austria))
                                            {
                                                amountApplyTax1 += resp.Fee;
                                            }
                                        }
                                        else if (!group.Primary.Event.TaxRateOne.Country.HasValue)
                                        {
                                            amountApplyTax1 += resp.Fee;
                                        }
                                    }
                                    if ((agenda.ApplyTaxTwo.HasValue) && (agenda.ApplyTaxTwo.Value))
                                    {
                                        if ((group.Primary.Event.TaxRateOne.Country.HasValue) && (reg.Country.HasValue))
                                        {
                                            if (group.Primary.Event.TaxRateOne.Country.Value == reg.Country.Value)
                                            {
                                                amountApplyTax2 += resp.Fee;
                                            }
                                            if ((group.Primary.Event.TaxRateOne.Country.Value == DataCollection.FormData.Countries.EU)
                                                && (reg.Country.Value == FormData.Countries.Austria))
                                            {
                                                amountApplyTax2 += resp.Fee;
                                            }
                                        }
                                        else if (!group.Primary.Event.TaxRateOne.Country.HasValue)
                                        {
                                            amountApplyTax2 += resp.Fee;
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
                                                amountApplyTax1 += resp.Fee;
                                            }
                                            if ((group.Primary.Event.TaxRateOne.Country.Value == DataCollection.FormData.Countries.EU)
                                                && (reg.Country.Value == FormData.Countries.Austria))
                                            {
                                                amountApplyTax1 += resp.Fee;
                                            }
                                        }
                                        else if (!group.Primary.Event.TaxRateOne.Country.HasValue)
                                        {
                                            amountApplyTax1 += resp.Fee;
                                        }
                                    }
                                    if ((agenda.ApplyTaxTwo.HasValue) && (agenda.ApplyTaxTwo.Value))
                                    {
                                        if ((group.Primary.Event.TaxRateOne.Country.HasValue) && (reg.Country.HasValue))
                                        {
                                            if (group.Primary.Event.TaxRateOne.Country.Value == reg.Country.Value)
                                            {
                                                amountApplyTax2 += resp.Fee;
                                            }
                                            if ((group.Primary.Event.TaxRateOne.Country.Value == DataCollection.FormData.Countries.EU)
                                                && (reg.Country.Value == FormData.Countries.Austria))
                                            {
                                                amountApplyTax2 += resp.Fee;
                                            }
                                        }
                                        else if (!group.Primary.Event.TaxRateOne.Country.HasValue)
                                        {
                                            amountApplyTax2 += resp.Fee;
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
                                                amountApplyTax1 += resp.Fee;
                                            }
                                            if ((group.Primary.Event.TaxRateOne.Country.Value == DataCollection.FormData.Countries.EU)
                                                && (reg.Country.Value == FormData.Countries.Austria))
                                            {
                                                amountApplyTax1 += resp.Fee;
                                            }
                                        }
                                        else if (!group.Primary.Event.TaxRateOne.Country.HasValue)
                                        {
                                            amountApplyTax1 += resp.Fee;
                                        }
                                    }
                                    if ((agenda.ApplyTaxTwo.HasValue) && (agenda.ApplyTaxTwo.Value))
                                    {
                                        if ((group.Primary.Event.TaxRateOne.Country.HasValue) && (reg.Country.HasValue))
                                        {
                                            if (group.Primary.Event.TaxRateOne.Country.Value == reg.Country.Value)
                                            {
                                                amountApplyTax2 += resp.Fee;
                                            }
                                            if ((group.Primary.Event.TaxRateOne.Country.Value == DataCollection.FormData.Countries.EU)
                                                && (reg.Country.Value == FormData.Countries.Austria))
                                            {
                                                amountApplyTax2 += resp.Fee;
                                            }
                                        }
                                        else if (!group.Primary.Event.TaxRateOne.Country.HasValue)
                                        {
                                            amountApplyTax2 += resp.Fee;
                                        }
                                    }
                                }
                                break;
                            default:
                                break;
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
                                if ((resp.Merchandise_Item.ApplyTaxOne.HasValue)
                                    && (resp.Merchandise_Item.ApplyTaxOne.Value))
                                {
                                    if ((group.Primary.Event.TaxRateOne.Country.HasValue) && (reg.Country.HasValue))
                                    {
                                        if (group.Primary.Event.TaxRateOne.Country.Value == reg.Country.Value)
                                        {
                                            amountApplyTax1 += resp.Fee * resp.Quantity;
                                        }
                                        if ((group.Primary.Event.TaxRateOne.Country.Value == DataCollection.FormData.Countries.EU)
                                            && (reg.Country.Value == FormData.Countries.Austria))
                                        {
                                            amountApplyTax1 += resp.Fee * resp.Quantity;
                                        }
                                    }
                                    else if (!group.Primary.Event.TaxRateOne.Country.HasValue)
                                    {
                                        amountApplyTax1 += resp.Fee * resp.Quantity;
                                    }
                                }
                                if ((resp.Merchandise_Item.ApplyTaxTwo.HasValue)
                                    && (resp.Merchandise_Item.ApplyTaxTwo.Value))
                                {
                                    if ((group.Primary.Event.TaxRateOne.Country.HasValue) && (reg.Country.HasValue))
                                    {
                                        if (group.Primary.Event.TaxRateOne.Country.Value == reg.Country.Value)
                                        {
                                            amountApplyTax2 += resp.Fee * resp.Quantity;
                                        }
                                        if ((group.Primary.Event.TaxRateOne.Country.Value == DataCollection.FormData.Countries.EU)
                                            && (reg.Country.Value == FormData.Countries.Austria))
                                        {
                                            amountApplyTax2 += resp.Fee * resp.Quantity;
                                        }
                                    }
                                    else if (!group.Primary.Event.TaxRateOne.Country.HasValue)
                                    {
                                        amountApplyTax2 += resp.Fee * resp.Quantity;
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
                                            amountApplyTax1 += resp.Fee;
                                        }
                                        if ((group.Primary.Event.TaxRateOne.Country.Value == DataCollection.FormData.Countries.EU)
                                            && (reg.Country.Value == FormData.Countries.Austria))
                                        {
                                            amountApplyTax1 += resp.Fee;
                                        }
                                    }
                                    else if (!group.Primary.Event.TaxRateOne.Country.HasValue)
                                    {
                                        amountApplyTax1 += resp.Fee;
                                    }
                                }
                                if ((resp.Merchandise_Item.ApplyTaxTwo.HasValue)
                                    && (resp.Merchandise_Item.ApplyTaxTwo.Value))
                                {
                                    if ((group.Primary.Event.TaxRateOne.Country.HasValue) && (reg.Country.HasValue))
                                    {
                                        if (group.Primary.Event.TaxRateOne.Country.Value == reg.Country.Value)
                                        {
                                            amountApplyTax2 += resp.Fee;
                                        }
                                        if ((group.Primary.Event.TaxRateOne.Country.Value == DataCollection.FormData.Countries.EU)
                                            && (reg.Country.Value == FormData.Countries.Austria))
                                        {
                                            amountApplyTax2 += resp.Fee;
                                        }
                                    }
                                    else if (!group.Primary.Event.TaxRateOne.Country.HasValue)
                                    {
                                        amountApplyTax2 += resp.Fee;
                                    }
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }



            if (group.Primary.Event.TaxRateOne != null)
            {
                tax1 = Math.Round(amountApplyTax1 * group.Primary.Event.TaxRateOne.Rate / 100.0, 2, MidpointRounding.AwayFromZero);
            }

            if (group.Primary.Event.TaxRateTwo != null)
            {
                tax2 = Math.Round(amountApplyTax2 * group.Primary.Event.TaxRateTwo.Rate / 100.0, 2, MidpointRounding.AwayFromZero);
            }

            total = Math.Round(total, 2, MidpointRounding.AwayFromZero) + tax1 + tax2;

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

        private double CalculateAgendaGroupDiscount(double agendaFee, Group group, List<AgendaResponse> agendaresponses)
        {
            double total = 0;

            if (group.Primary.Event.StartPage.GroupDiscount.GroupSizeOption == GroupDiscount_GroupSizeOption.SizeOrMore)
            {
                if (group.Primary.Event.StartPage.GroupDiscount.GroupDiscountType == GroupDiscount_DiscountType.Percent)
                {
                    total += agendaFee * group.Primary.Event.StartPage.GroupDiscount.DiscountAmount / 100.0 * agendaresponses.Count;
                }
                else
                {
                    total += group.Primary.Event.StartPage.GroupDiscount.DiscountAmount * agendaresponses.Count;
                }
            }
            else if (group.Primary.Event.StartPage.GroupDiscount.AddtionalRegOption == GroupDiscount_AdditionalRegOption.All)
            {
                for (int i = 1; i <= agendaresponses.Count; i++)
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
                    total += agendaFee * group.Primary.Event.StartPage.GroupDiscount.DiscountAmount / 100.0 * (agendaresponses.Count - group.Primary.Event.StartPage.GroupDiscount.GroupSize);
                }
                else
                {
                    total += group.Primary.Event.StartPage.GroupDiscount.DiscountAmount * (agendaresponses.Count - group.Primary.Event.StartPage.GroupDiscount.GroupSize);
                }
            }
            else if (group.Primary.Event.StartPage.GroupDiscount.AddtionalRegOption == GroupDiscount_AdditionalRegOption.Additional)
            {
                for (int i = 1; i <= agendaresponses.Count; i++)
                {
                    if ((i > group.Primary.Event.StartPage.GroupDiscount.GroupSize) && (i <= group.Primary.Event.StartPage.GroupDiscount.NumberOfAdditionalReg.Value + agendaresponses.Count))
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
    }
}
