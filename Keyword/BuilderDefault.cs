namespace RegOnline.RegressionTest.Keyword
{
    using System;
    using System.Collections.Generic;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.WebElements;

    public class BuilderDefault
    {
        public List<ChoiceItem> GetAgendaChoiceItem()
        {
            List<ChoiceItem> choiceItems = new List<ChoiceItem>();

            for (int i = 1; i <= PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AgendaChoiceItemCount.Count; i++)
            {
                ButtonOrLink choiceItem = new ButtonOrLink(PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AgendaChoiceItemCount.Locator + "[" + i + "]//a", LocateBy.XPath);
                ChoiceItem choice = new ChoiceItem(choiceItem.Text);
                
                choiceItems.Add(choice);
            }

            return choiceItems;
        }
    }
}
