namespace RegOnline.RegressionTest.Keyword
{
    using System.Collections.Generic;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.PageObject.Builder;

    public class AddAgendaItem
    {
        public void AddAgendaItems(AgendaItem agenda)
        {
            if (PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.CreateAgendaItem.IsPresent)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.CreateAgendaItem_Click();
            }
            else
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AddAgendaItem_Click();
            }

            #region Common Settings
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.NameOnForm.Type(agenda.NameOnFrom);
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.NameOptions_Click();
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.NameOnReceipt.Type(agenda.NameOnFrom);
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.NameOnReports.Type(agenda.NameOnFrom);
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.FieldType_Click();
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AgendaType_Select(agenda.Type);

            if (agenda.CustomFieldVisibleOption.Count != 0)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.ShowAllRegTypes_Click();

                foreach (CustomFieldVisibleOption visbleOption in agenda.CustomFieldVisibleOption)
                {
                    if (visbleOption.RegType == null)
                    {
                        if (visbleOption.Visible.HasValue)
                        {
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.VisibleToAll.Set(visbleOption.Visible.Value);
                        }
                        if (visbleOption.Required.HasValue)
                        {
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.RequiredByAll.Set(visbleOption.Required.Value);
                        }
                        if (visbleOption.AdminOnly.HasValue)
                        {
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AdminOnlyToAll.Set(visbleOption.AdminOnly.Value);
                        }

                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.ShowAllRegTypes_Click();
                    }
                    else
                    {
                        if (visbleOption.Visible.HasValue)
                        {
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.VisibleToRegType(visbleOption.RegType.RegTypeId).Set(visbleOption.Visible.Value);
                        }
                        if (visbleOption.Required.HasValue)
                        {
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.RequiredByRegType(visbleOption.RegType.RegTypeId).Set(visbleOption.Required.Value);
                        }
                        if (visbleOption.AdminOnly.HasValue)
                        {
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AdminOnlyToRegType(visbleOption.RegType.RegTypeId).Set(visbleOption.AdminOnly.Value);
                        }
                    }
                }
            }

            if (agenda.ConditionalLogic.Count != 0)
            {
                foreach (string logic in agenda.ConditionalLogic)
                {
                    while (!PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.ConditionalLogicParent(logic).IsPresent)
                    {
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.ExpandConditionalLogic_Click();
                    }

                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.ConditionalLogicParent(logic).Set(true);
                }
            }

            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.Advanced_Click();

            if (agenda.IncludeOnEventWeb.HasValue)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.IncludeOnEventWeb.Set(agenda.IncludeOnEventWeb.Value);
            }

            if (agenda.ShowStarting.HasValue)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.ShowStarting_Type(agenda.ShowStarting.Value);
            }

            if (agenda.HideStarting.HasValue)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.HideStarting_Type(agenda.HideStarting.Value);
            }

            if (agenda.Gender.HasValue)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.Gender.SelectWithText(agenda.Gender.Value.ToString());
            }

            if (agenda.AgeGreaterThan.HasValue)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AgeGreaterThan.Type(agenda.AgeGreaterThan.Value);
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AgeGreaterThanDate_Type(agenda.AgeGreaterThanDate.Value);
            }

            if (agenda.AgeLessThan.HasValue)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AgeLessThan.Type(agenda.AgeLessThan.Value);
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AgeLessThanDate_Type(agenda.AgeLessThanDate.Value);
            }

            if ((agenda.NameOnReceipt != null) || (agenda.NameOnReports != null))
            {
                if (!PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.NameOnReports.IsDisplay)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.NameOptions_Click();
                }

                if (agenda.NameOnReceipt != null)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.NameOnReceipt.Type(agenda.NameOnReceipt);
                }

                if (agenda.NameOnReports != null)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.NameOnReports.Type(agenda.NameOnReports);
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
                    if (!PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.NameOnBadge.IsPresent)
                    {
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.NameOptions_Click();
                    }

                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.NameOnBadge.Type(ag.NameOnBadge);
                }

                if (ag.StartDate.HasValue)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.StartDate_Type(ag.StartDate.Value);
                }
                if (ag.StartTime.HasValue)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.StartTime_Type(ag.StartTime.Value);
                }
                if (ag.EndDate.HasValue)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.EndDate_Type(ag.EndDate.Value);
                }
                if (ag.EndTime.HasValue)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.EndTime_Type(ag.EndTime.Value);
                }
                if (ag.Location != null)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.Location.Type(ag.Location);
                }
                if (ag.Price.HasValue)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.StandardPrice.Type(ag.Price.Value);
                }

                if ((ag.EarlyPrice != null) || (ag.LatePrice != null) ||
                    (ag.DiscountCode.Count != 0) || (ag.BulkCodes != null) ||
                    (ag.TaxRateOne != null) || (ag.TaxRateTwo != null))
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.PriceOptionsLink_Click();

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
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AddBulkCodes_Click();
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.BulkLoadCodesDefine.SelectByName();
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.BulkLoadCodesDefine.CodesDefine.Type(ag.BulkCodes);
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.BulkLoadCodesDefine.SaveAndClose_Click();
                    }

                    if ((ag.TaxRateOne != null) || (ag.TaxRateTwo != null))
                    {
                        KeywordProvider.AddTaxRate.AddTaxRates(ag.TaxRateOne, ag.TaxRateTwo, FormData.Location.Agenda);
                    }

                    if (ag.TaxRateOne.Apply.HasValue)
                    {
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.ApplyTaxOne.Set(ag.TaxRateOne.Apply.Value);
                    }

                    if (ag.TaxRateTwo.Apply.HasValue)
                    {
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.ApplyTaxTwo.Set(ag.TaxRateTwo.Apply.Value);
                    }
                }

                if (ag.SpacesAvailable.HasValue)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.Capacity.Type(ag.SpacesAvailable.Value);

                    if ((ag.ShowCapacity.HasValue) || (ag.LimitReachedOption.HasValue))
                    {
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.CapacityOptionsLink_Click();

                        if (ag.ShowCapacity.HasValue)
                        {
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.ShowRemainingCapacity.Set(ag.ShowCapacity.Value);
                        }

                        if (ag.LimitReachedOption.HasValue)
                        {
                            switch (ag.LimitReachedOption.Value)
                            {
                                case FormData.AgendaLimitReachedOption.HideItem:
                                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.HideWhenLimitReached.Click();
                                    break;
                                case FormData.AgendaLimitReachedOption.ShowMessage:
                                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.ShowMessageWhenLimitReached.Click();
                                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AddLimitReachedMessage_Click();
                                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.LimitReachedMessageEditor.SelectByName();
                                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.LimitReachedMessageEditor.HtmlMode_Click();
                                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.LimitReachedMessageEditor.Content_Type(ag.LimitReachedMessage);
                                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.LimitReachedMessageEditor.SaveAndClose_Click();
                                    break;
                                case FormData.AgendaLimitReachedOption.Waitlist:
                                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.WaitlistWhenLimitReached.Click();

                                    if (ag.WaitlistConfirmationText != null)
                                    {
                                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AddTextToConfirmation_Click();
                                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.TextToConfirmationEditor.SelectByName();
                                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.TextToConfirmationEditor.HtmlMode_Click();
                                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.TextToConfirmationEditor.Content_Type(ag.WaitlistConfirmationText);
                                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.TextToConfirmationEditor.SaveAndClose_Click();
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
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AddToCalendar.Set(ag.AddToCalendar.Value);
                }

                if (ag.DateFormat.HasValue)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.DateFormat.SelectWithValue(((int)ag.DateFormat.Value).ToString());
                }

                if (ag.DetailsPopup != null)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AddDetails_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.DetailsEditor.SelectByName();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.DetailsEditor.HtmlMode_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.DetailsEditor.Content_Type(ag.DetailsPopup);
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.DetailsEditor.SaveAndClose_Click();
                }

                if (ag.DetailsURL != null)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.DetailsURL.Type(ag.DetailsURL);
                }

                if (ag.InitialStatus.HasValue)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.InitialStatus.SelectWithValue(((int)ag.InitialStatus.Value).ToString());
                }

                if (ag.ConfirmationAddendum != null)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AddConfirmationAddendum_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.ConfirmationAddendumEditor.SelectByName();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.ConfirmationAddendumEditor.HtmlMode_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.ConfirmationAddendumEditor.Content_Type(ag.ConfirmationAddendum);
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.ConfirmationAddendumEditor.SaveAndClose_Click();
                }
            }
            #endregion

            #region MultipleChoice AgendaItem
            if ((agenda is AgendaItemRadioButton) || (agenda is AgendaItemDropDown))
            {
                AgendaItemMultipleChoice ag = agenda as AgendaItemMultipleChoice;

                if (ag.ChoiceItems.Count != 0)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AddMultipleChoiceItem_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.MultipleChoiceDefine.SelectByName();

                    for (int i = 0; i < ag.ChoiceItems.Count; i++)
                    {
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.MultipleChoiceDefine.NameOnForm.Type(ag.ChoiceItems[i].Name);
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.MultipleChoiceDefine.NameOnReports.Type(ag.ChoiceItems[i].Name);

                        if (ag.ChoiceItems[i].Price.HasValue)
                        {
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.MultipleChoiceDefine.Price.Type(ag.ChoiceItems[i].Price.Value);
                        }
                        if (ag.ChoiceItems[i].SingleLimit.HasValue)
                        {
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.MultipleChoiceDefine.Limit.Type(ag.ChoiceItems[i].SingleLimit.Value);
                        }
                        if (ag.ChoiceItems[i].GroupLimit.HasValue)
                        {
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.MultipleChoiceDefine.GroupLimit.Type(ag.ChoiceItems[i].GroupLimit.Value);
                        }
                        if (ag.ChoiceItems[i].Visible.HasValue)
                        {
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.MultipleChoiceDefine.Visible.Set(ag.ChoiceItems[i].Visible.Value);
                        }

                        if (i == ag.ChoiceItems.Count - 1)
                        {
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.MultipleChoiceDefine.SaveAndClose_Click();
                        }
                        else
                        {
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.MultipleChoiceDefine.SaveAndNew_Click();
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
                    int count = ag.ChoiceItems.Count;

                    foreach (FormData.CommonlyUsedMultipleChoice item in ag.CommonlyUsedItems)
                    {
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AddCommonlyUsedItem_Click();
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.CommonlyUsedItemsDefine.SelectByName();
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.CommonlyUsedItemsDefine.CommonlyUsedItem_Click(item);
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.CommonlyUsedItemsDefine.SaveAndClose_Click();
                    }

                    List<ChoiceItem> choiceItems = KeywordProvider.BuilderDefault.GetAgendaChoiceItem();

                    for (int i = count; i < choiceItems.Count; i++)
                    {
                        ag.ChoiceItems.Add(choiceItems[i]);
                    }
                }

                if (ag.GroupName != null)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.GroupName.Type(ag.GroupName);
                }

                if (ag.ForceGroupToMatch.HasValue)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.ForceGroupToMatch.Set(ag.ForceGroupToMatch.Value);
                }
            }
            #endregion

            #region CharInput AgendaItem
            if ((agenda is AgendaItemNumber) || (agenda is AgendaItemOneLineText) || (agenda is AgendaItemParagraph))
            {
                CharInputAgendaItem ag = agenda as CharInputAgendaItem;

                if (ag.NameOnBadge != null)
                {
                    if (!PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.NameOnBadge.IsPresent)
                    {
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.NameOptions_Click();
                    }

                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.NameOnBadge.Type(ag.NameOnBadge);
                }

                if (ag.CharLimit.HasValue)
                {
                    if (agenda is AgendaItemParagraph)
                    {
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.ParagraphLimit.Type(ag.CharLimit.Value);
                    }
                    else
                    {
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.CharacterLimit.Type(ag.CharLimit.Value);
                    }
                }

                if (ag.DetailsPopup != null)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AddDetails_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.DetailsEditor.SelectByName();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.DetailsEditor.HtmlMode_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.DetailsEditor.Content_Type(ag.DetailsPopup);
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.DetailsEditor.SaveAndClose_Click();
                }

                if (ag.DetailsURL != null)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.DetailsURL.Type(ag.DetailsURL);
                }

                if (ag.GroupName != null)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.GroupName.Type(ag.GroupName);
                }

                if (ag.ForceGroupToMatch.HasValue)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.ForceGroupToMatch.Set(ag.ForceGroupToMatch.Value);
                }
            }
            #endregion

            #region Other Related Settings
            if (agenda is AgendaItemHeader)
            {
                AgendaItemHeader ag = agenda as AgendaItemHeader;

                if (ag.DetailsPopup != null)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AddDetails_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.DetailsEditor.SelectByName();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.DetailsEditor.HtmlMode_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.DetailsEditor.Content_Type(ag.DetailsPopup);
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.DetailsEditor.SaveAndClose_Click();
                }

                if (ag.DetailsURL != null)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.DetailsURL.Type(ag.DetailsURL);
                }
            }

            if (agenda is AgendaItemUpload)
            {
                AgendaItemUpload ag = agenda as AgendaItemUpload;

                if (ag.GroupName != null)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.GroupName.Type(ag.GroupName);
                }
            }

            if (agenda is AgendaItemDate)
            {
                AgendaItemDate ag = agenda as AgendaItemDate;

                if (ag.GroupName != null)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.GroupName.Type(ag.GroupName);
                }

                if (ag.ForceGroupToMatch.HasValue)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.ForceGroupToMatch.Set(ag.ForceGroupToMatch.Value);
                }
            }

            if (agenda is AgendaItemTime)
            {
                AgendaItemTime ag = agenda as AgendaItemTime;

                if (ag.GroupName != null)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.GroupName.Type(ag.GroupName);
                }

                if (ag.ForceGroupToMatch.HasValue)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.ForceGroupToMatch.Set(ag.ForceGroupToMatch.Value);
                }
            }

            if (agenda is AgendaItemCheckBox)
            {
                AgendaItemCheckBox ag = agenda as AgendaItemCheckBox;

                if (ag.GroupName != null)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.GroupName.Type(ag.GroupName);
                }

                if (ag.ForceGroupToMatch.HasValue)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.ForceGroupToMatch.Set(ag.ForceGroupToMatch.Value);
                }
            }

            if (agenda is AgendaItemContribution)
            {
                AgendaItemContribution ag = agenda as AgendaItemContribution;
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.MinAmount.Type(ag.MinAmount);
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.MaxAmount.Type(ag.MaxAmount);
            }
            #endregion

            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.SaveItem_Click();
        }
    }
}
