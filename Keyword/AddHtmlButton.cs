namespace RegOnline.RegressionTest.Keyword
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class AddHtmlButton
    {
        public void CreateButton(DataCollection.HtmlButton button)
        {
            PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.Link_ButtonDesigner_Click();
            PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.Frame_ButtonDesigner.SelectByName();

            if (button.Market_Selection != DataCollection.HtmlButton.MarketSelection.Events)
            {
                PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.Frame_ButtonDesigner.ProgressBarStep_Click(PageObject.Manager.Dashboard.ButtonDesigner.ProgressBarStep.MarketSelection);
                PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.Frame_ButtonDesigner.MarketSelection_Select(button.Market_Selection);
                PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.Frame_ButtonDesigner.NextStep_Click();
            }

            PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.Frame_ButtonDesigner.BuildIt_Click(button);

            Enum.TryParse<DataCollection.HtmlButton.Keyword>(PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.Frame_ButtonDesigner.Link_ButtonKeyword.Text, out button.Button_Keyword);
        }
    }
}
