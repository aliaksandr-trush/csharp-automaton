namespace RegOnline.RegressionTest.Keyword
{
    using System;
    using System.Collections.Generic;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.WebElements;

    public class BuilderDefault
    {
        private PageObject.Builder.Agenda Agenda = new PageObject.Builder.Agenda();

        public List<ChoiceItem> GetAgendaChoiceItem()
        {
            List<ChoiceItem> choiceItems = new List<ChoiceItem>();

            for (int i = 1; i <= Agenda.AgendaChoiceItemCount.Count; i++)
            {
                ButtonOrLink choiceItem = new ButtonOrLink(Agenda.AgendaChoiceItemCount.Locator + "[" +i+ "]//a", LocateBy.XPath);
                string tmp = choiceItem.GetAttribute("href").Trim();
                tmp = tmp.Replace("%20", "");
                tmp = tmp.Split(new string[] { "(" }, StringSplitOptions.RemoveEmptyEntries)[1].Trim();
                tmp = tmp.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)[2].Trim();
                ChoiceItem choice = new ChoiceItem(choiceItem.Text);
                
                choice.Id = Convert.ToInt32(tmp);
                choiceItems.Add(choice);
            }

            return choiceItems;
        }
    }
}
