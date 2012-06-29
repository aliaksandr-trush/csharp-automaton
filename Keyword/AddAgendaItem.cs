namespace RegOnline.RegressionTest.Keyword
{
    using System.Collections.Generic;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.PageObject.Builder;

    public class AddAgendaItem
    {
        private Agenda Agenda = new Agenda();
        private PageObjectHelper PageObjectHelper = new PageObjectHelper();

        public void AddAgendaItems(AgendaItem agenda)
        {
            if (Agenda.CreateAgendaItem.IsPresent)
            {
                Agenda.CreateAgendaItem_Click();
            }
            else
            {
                Agenda.AddAgendaItem_Click();
            }

            #region Common Settings
            Agenda.NameOnForm.Type(agenda.NameOnFrom);
            Agenda.FieldType_Click();
            Agenda.AgendaType_Select(agenda.Type);

            if (agenda.CustomFieldVisibleOption.Count != 0)
            {
                Agenda.ShowAllRegTypes_Click();

                foreach (CustomFieldVisibleOption visbleOption in agenda.CustomFieldVisibleOption)
                {
                    if (visbleOption.RegType == null)
                    {
                        if (visbleOption.Visible.HasValue)
                        {
                            Agenda.VisibleToAll.Set(visbleOption.Visible.Value);
                        }
                        if (visbleOption.Required.HasValue)
                        {
                            Agenda.RequiredByAll.Set(visbleOption.Required.Value);
                        }
                        if (visbleOption.AdminOnly.HasValue)
                        {
                            Agenda.AdminOnlyToAll.Set(visbleOption.AdminOnly.Value);
                        }

                        Agenda.ShowAllRegTypes_Click();
                    }
                    else
                    {
                        if (visbleOption.Visible.HasValue)
                        {
                            Agenda.VisibleToRegType(visbleOption.RegType.RegTypeId).Set(visbleOption.Visible.Value);
                        }
                        if (visbleOption.Required.HasValue)
                        {
                            Agenda.RequiredByRegType(visbleOption.RegType.RegTypeId).Set(visbleOption.Required.Value);
                        }
                        if (visbleOption.AdminOnly.HasValue)
                        {
                            Agenda.AdminOnlyToRegType(visbleOption.RegType.RegTypeId).Set(visbleOption.AdminOnly.Value);
                        }
                    }
                }
            }

            if (agenda.ConditionalLogic.Count != 0)
            {
                foreach (string logic in agenda.ConditionalLogic)
                {
                    while (!Agenda.ConditionalLogicParent(logic).IsPresent)
                    {
                        Agenda.ExpandConditionalLogic_Click();
                    }

                    Agenda.ConditionalLogicParent(logic).Set(true);
                }
            }

            PageObjectHelper.Advanced_Click();

            if (agenda.IncludeOnEventWeb.HasValue)
            {
                Agenda.IncludeOnEventWeb.Set(agenda.IncludeOnEventWeb.Value);
            }

            if (agenda.ShowStarting.HasValue)
            {
                Agenda.ShowStarting_Type(agenda.ShowStarting.Value);
            }

            if (agenda.HideStarting.HasValue)
            {
                Agenda.HideStarting_Type(agenda.HideStarting.Value);
            }

            if (agenda.Gender.HasValue)
            {
                Agenda.Gender.SelectWithText(agenda.Gender.Value.ToString());
            }

            if (agenda.AgeGreaterThan.HasValue)
            {
                Agenda.AgeGreaterThan.Type(agenda.AgeGreaterThan.Value);
                Agenda.AgeGreaterThanDate_Type(agenda.AgeGreaterThanDate.Value);
            }

            if (agenda.AgeLessThan.HasValue)
            {
                Agenda.AgeLessThan.Type(agenda.AgeLessThan.Value);
                Agenda.AgeLessThanDate_Type(agenda.AgeLessThanDate.Value);
            }

            if ((agenda.NameOnReceipt != null) || (agenda.NameOnReports != null))
            {
                Agenda.NameOptions_Click();

                if (agenda.NameOnReceipt != null)
                {
                    Agenda.NameOnReceipt.Type(agenda.NameOnReceipt);
                }

                if (agenda.NameOnReports != null)
                {
                    Agenda.NameOnReports.Type(agenda.NameOnReports);
                }
            }
            #endregion

            #region Common AgendaItem
            if ((agenda is AgendaItemCheckBox) || (agenda is AgendaItemRadioButton) || (agenda is AgendaItemDropDown) ||
                (agenda is AgendaItemTime) || (agenda is AgendaItemDate) || (agenda is AgendaItemUpload) || (agenda is AgendaItemAlways))
            {
                AgendaItemCommon ag = agenda as AgendaItemCommon;

                if (ag.NameOnBadge != null)
                {
                    if (!Agenda.NameOnBadge.IsPresent)
                    {
                        Agenda.NameOptions_Click();
                    }

                    Agenda.NameOnBadge.Type(ag.NameOnBadge);
                }

                if (ag.StartDate.HasValue)
                {
                    Agenda.StartDate_Type(ag.StartDate.Value);
                }
                if (ag.StartTime.HasValue)
                {
                    Agenda.StartTime_Type(ag.StartTime.Value);
                }
                if (ag.EndDate.HasValue)
                {
                    Agenda.EndDate_Type(ag.EndDate.Value);
                }
                if (ag.EndTime.HasValue)
                {
                    Agenda.EndTime_Type(ag.EndTime.Value);
                }
                if (ag.Location != null)
                {
                    Agenda.Location.Type(ag.Location);
                }
                if (ag.Price.HasValue)
                {
                    Agenda.StandardPrice.Type(ag.Price.Value);
                }

                if ((ag.EarlyPrice != null) || (ag.LatePrice != null) ||
                    (ag.DiscountCode.Count != 0) || (ag.BulkCodes != null) ||
                    (ag.TaxRateOne != null) || (ag.TaxRateTwo != null))
                {
                    Agenda.PriceOptionsLink_Click();

                    if (ag.EarlyPrice != null)
                    {
                        KeywordProvider.AddEarlyLatePrice.AddEarlyPrice(ag.EarlyPrice, FormData.Location.Agenda);
                    }

                    if (ag.LatePrice != null)
                    {
                        KeywordProvider.AddEarlyLatePrice.AddLatePrice(ag.LatePrice, FormData.Location.Agenda);
                    }

                    if (ag.DiscountCode.Count != 0)
                    {
                        foreach (DiscountCode dc in ag.DiscountCode)
                        {
                            KeywordProvider.AddDiscountCode.AddDiscountCodes(dc, FormData.Location.Agenda);
                        }
                    }

                    if (ag.BulkCodes != null)
                    {
                        Agenda.AddBulkCodes_Click();
                        Agenda.BulkLoadCodesDefine.SelectByName();
                        Agenda.BulkLoadCodesDefine.CodesDefine.Type(ag.BulkCodes);
                        Agenda.BulkLoadCodesDefine.SaveAndClose_Click();
                    }

                    if ((ag.TaxRateOne != null) || (ag.TaxRateTwo != null))
                    {
                        KeywordProvider.AddTaxRate.AddTaxRates(ag.TaxRateOne, ag.TaxRateTwo, FormData.Location.Agenda);
                    }

                    if (ag.TaxRateOne.Apply.HasValue)
                    {
                        Agenda.ApplyTaxOne.Set(ag.TaxRateOne.Apply.Value);
                    }

                    if (ag.TaxRateTwo.Apply.HasValue)
                    {
                        Agenda.ApplyTaxTwo.Set(ag.TaxRateTwo.Apply.Value);
                    }
                }

                if (ag.SpacesAvailable.HasValue)
                {
                    Agenda.Capacity.Type(ag.SpacesAvailable.Value);

                    if ((ag.ShowCapacity.HasValue) || (ag.LimitReachedOption.HasValue))
                    {
                        Agenda.CapacityOptionsLink_Click();

                        if (ag.ShowCapacity.HasValue)
                        {
                            Agenda.ShowRemainingCapacity.Set(ag.ShowCapacity.Value);
                        }

                        if (ag.LimitReachedOption.HasValue)
                        {
                            switch (ag.LimitReachedOption.Value)
                            {
                                case FormData.AgendaLimitReachedOption.HideItem:
                                    Agenda.HideWhenLimitReached.Click();
                                    break;
                                case FormData.AgendaLimitReachedOption.ShowMessage:
                                    Agenda.ShowMessageWhenLimitReached.Click();
                                    Agenda.AddLimitReachedMessage_Click();
                                    Agenda.LimitReachedMessageEditor.SelectByName();
                                    Agenda.LimitReachedMessageEditor.HtmlMode_Click();
                                    Agenda.LimitReachedMessageEditor.Content_Type(ag.LimitReachedMessage);
                                    Agenda.LimitReachedMessageEditor.SaveAndClose_Click();
                                    break;
                                case FormData.AgendaLimitReachedOption.Waitlist:
                                    Agenda.WaitlistWhenLimitReached.Click();

                                    if (ag.WaitlistConfirmationText != null)
                                    {
                                        Agenda.AddTextToConfirmation_Click();
                                        Agenda.TextToConfirmationEditor.SelectByName();
                                        Agenda.TextToConfirmationEditor.HtmlMode_Click();
                                        Agenda.TextToConfirmationEditor.Content_Type(ag.WaitlistConfirmationText);
                                        Agenda.TextToConfirmationEditor.SaveAndClose_Click();
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }

                if (ag.AddToCalendar.HasValue)
                {
                    Agenda.AddToCalendar.Set(ag.AddToCalendar.Value);
                }

                if (ag.DateFormat.HasValue)
                {
                    Agenda.DateFormat.SelectWithValue(((int)ag.DateFormat.Value).ToString());
                }

                if (ag.DetailsPopup != null)
                {
                    Agenda.AddDetails_Click();
                    Agenda.DetailsEditor.SelectByName();
                    Agenda.DetailsEditor.HtmlMode_Click();
                    Agenda.DetailsEditor.Content_Type(ag.DetailsPopup);
                    Agenda.DetailsEditor.SaveAndClose_Click();
                }

                if (ag.DetailsURL != null)
                {
                    Agenda.DetailsURL.Type(ag.DetailsURL);
                }

                if (ag.InitialStatus.HasValue)
                {
                    Agenda.InitialStatus.SelectWithValue(((int)ag.InitialStatus.Value).ToString());
                }

                if (ag.ConfirmationAddendum != null)
                {
                    Agenda.AddConfirmationAddendum_Click();
                    Agenda.ConfirmationAddendumEditor.SelectByName();
                    Agenda.ConfirmationAddendumEditor.HtmlMode_Click();
                    Agenda.ConfirmationAddendumEditor.Content_Type(ag.ConfirmationAddendum);
                    Agenda.ConfirmationAddendumEditor.SaveAndClose_Click();
                }
            }
            #endregion

            #region MultipleChoice AgendaItem
            if ((agenda is AgendaItemRadioButton) || (agenda is AgendaItemDropDown))
            {
                AgendaItemMultipleChoice ag = agenda as AgendaItemMultipleChoice;

                if (ag.ChoiceItems.Count != 0)
                {
                    Agenda.AddMultipleChoiceItem_Click();
                    Agenda.MultipleChoiceDefine.SelectByName();

                    for (int i = 0; i < ag.ChoiceItems.Count; i++)
                    {
                        Agenda.MultipleChoiceDefine.NameOnForm.Type(ag.ChoiceItems[i].Name);
                        Agenda.MultipleChoiceDefine.NameOnReports.Type(ag.ChoiceItems[i].Name);

                        if (ag.ChoiceItems[i].Price.HasValue)
                        {
                            Agenda.MultipleChoiceDefine.Price.Type(ag.ChoiceItems[i].Price.Value);
                        }
                        if (ag.ChoiceItems[i].SingleLimit.HasValue)
                        {
                            Agenda.MultipleChoiceDefine.Limit.Type(ag.ChoiceItems[i].SingleLimit.Value);
                        }
                        if (ag.ChoiceItems[i].GroupLimit.HasValue)
                        {
                            Agenda.MultipleChoiceDefine.GroupLimit.Type(ag.ChoiceItems[i].GroupLimit.Value);
                        }
                        if (ag.ChoiceItems[i].Visible.HasValue)
                        {
                            Agenda.MultipleChoiceDefine.Visible.Set(ag.ChoiceItems[i].Visible.Value);
                        }

                        if (i == ag.ChoiceItems.Count - 1)
                        {
                            Agenda.MultipleChoiceDefine.SaveAndClose_Click();
                        }
                        else
                        {
                            Agenda.MultipleChoiceDefine.SaveAndNew_Click();
                        }
                    }

                    List<ChoiceItem> choiceItems = KeywordProvider.BuilderDefault.GetAgendaChoiceItem();

                    for (int i = 0; i < ag.ChoiceItems.Count; i++)
                    {
                        ag.ChoiceItems[i].Id = choiceItems[i].Id;
                    }
                }

                if (ag.CommonlyUsedItems.Count != 0)
                {
                    foreach (FormData.CommonlyUsedMultipleChoice item in ag.CommonlyUsedItems)
                    {
                        Agenda.AddCommonlyUsedItem_Click();
                        Agenda.CommonlyUsedItemsDefine.SelectByName();
                        Agenda.CommonlyUsedItemsDefine.CommonlyUsedItem_Click(item);
                        Agenda.CommonlyUsedItemsDefine.SaveAndClose_Click();
                    }

                    List<ChoiceItem> choiceItems = KeywordProvider.BuilderDefault.GetAgendaChoiceItem();

                    for (int i = 0; i < choiceItems.Count; i++)
                    {
                        ag.ChoiceItems.Add(choiceItems[i]);
                    }
                }

                if (ag.GroupName != null)
                {
                    Agenda.GroupName.Type(ag.GroupName);
                }

                if (ag.ForceGroupToMatch.HasValue)
                {
                    Agenda.ForceGroupToMatch.Set(ag.ForceGroupToMatch.Value);
                }
            }
            #endregion

            #region CharInput AgendaItem
            if ((agenda is AgendaItemNumber) || (agenda is AgendaItemOneLineText) || (agenda is AgendaItemParagraph))
            {
                CharInputAgendaItem ag = agenda as CharInputAgendaItem;

                if (ag.NameOnBadge != null)
                {
                    if (!Agenda.NameOnBadge.IsPresent)
                    {
                        Agenda.NameOptions_Click();
                    }

                    Agenda.NameOnBadge.Type(ag.NameOnBadge);
                }

                if (ag.CharLimit.HasValue)
                {
                    if (agenda is AgendaItemParagraph)
                    {
                        Agenda.ParagraphLimit.Type(ag.CharLimit.Value);
                    }
                    else
                    {
                        Agenda.CharacterLimit.Type(ag.CharLimit.Value);
                    }
                }

                if (ag.DetailsPopup != null)
                {
                    Agenda.AddDetails_Click();
                    Agenda.DetailsEditor.SelectByName();
                    Agenda.DetailsEditor.HtmlMode_Click();
                    Agenda.DetailsEditor.Content_Type(ag.DetailsPopup);
                    Agenda.DetailsEditor.SaveAndClose_Click();
                }

                if (ag.DetailsURL != null)
                {
                    Agenda.DetailsURL.Type(ag.DetailsURL);
                }

                if (ag.GroupName != null)
                {
                    Agenda.GroupName.Type(ag.GroupName);
                }

                if (ag.ForceGroupToMatch.HasValue)
                {
                    Agenda.ForceGroupToMatch.Set(ag.ForceGroupToMatch.Value);
                }
            }
            #endregion

            #region Other Related Settings
            if (agenda is AgendaItemHeader)
            {
                AgendaItemHeader ag = agenda as AgendaItemHeader;

                if (ag.DetailsPopup != null)
                {
                    Agenda.AddDetails_Click();
                    Agenda.DetailsEditor.SelectByName();
                    Agenda.DetailsEditor.HtmlMode_Click();
                    Agenda.DetailsEditor.Content_Type(ag.DetailsPopup);
                    Agenda.DetailsEditor.SaveAndClose_Click();
                }

                if (ag.DetailsURL != null)
                {
                    Agenda.DetailsURL.Type(ag.DetailsURL);
                }
            }

            if (agenda is AgendaItemUpload)
            {
                AgendaItemUpload ag = agenda as AgendaItemUpload;

                if (ag.GroupName != null)
                {
                    Agenda.GroupName.Type(ag.GroupName);
                }
            }

            if (agenda is AgendaItemDate)
            {
                AgendaItemDate ag = agenda as AgendaItemDate;

                if (ag.GroupName != null)
                {
                    Agenda.GroupName.Type(ag.GroupName);
                }

                if (ag.ForceGroupToMatch.HasValue)
                {
                    Agenda.ForceGroupToMatch.Set(ag.ForceGroupToMatch.Value);
                }
            }

            if (agenda is AgendaItemTime)
            {
                AgendaItemTime ag = agenda as AgendaItemTime;

                if (ag.GroupName != null)
                {
                    Agenda.GroupName.Type(ag.GroupName);
                }

                if (ag.ForceGroupToMatch.HasValue)
                {
                    Agenda.ForceGroupToMatch.Set(ag.ForceGroupToMatch.Value);
                }
            }

            if (agenda is AgendaItemCheckBox)
            {
                AgendaItemCheckBox ag = agenda as AgendaItemCheckBox;

                if (ag.GroupName != null)
                {
                    Agenda.GroupName.Type(ag.GroupName);
                }

                if (ag.ForceGroupToMatch.HasValue)
                {
                    Agenda.ForceGroupToMatch.Set(ag.ForceGroupToMatch.Value);
                }
            }
            #endregion

            Agenda.SaveItem_Click();
        }
    }
}
