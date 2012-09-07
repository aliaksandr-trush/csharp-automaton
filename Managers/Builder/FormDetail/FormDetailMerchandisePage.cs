namespace RegOnline.RegressionTest.Managers.Builder
{
    using System;
    using System.Linq;
    using NUnit.Framework;
    using RegOnline.RegressionTest.DataAccess;
    using RegOnline.RegressionTest.Managers;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.Attributes;

    public partial class FormDetailManager : ManagerBase
    {
        public MerchandiseManager MerchMgr { get; set; }

        public void ClickAddMerchandiseItem()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(GetAddGridItemLocator("ctl00_cph_grdFees_"), LocateBy.Id);
            Utility.ThreadSleep(1);
            WebDriverUtility.DefaultProvider.WaitForRADWindow();
            WebDriverUtility.DefaultProvider.SelectPopUpFrameByName(MerchandiseManager.MerchItemDetailDialogID);
        }

        [Step]
        public void AddMerchandiseItem(MerchandiseManager.MerchandiseType type, string name)
        {
            double fee = Math.Round(new Random((int)DateTime.Now.Ticks).NextDouble() * 1000 + 1, 2);
            double min = Math.Round(new Random((int)DateTime.Now.Ticks).NextDouble() * 50 + 1, 2);
            double max = Math.Round(new Random((int)DateTime.Now.Ticks).NextDouble() * 1000 + 50 + 1, 2);

            AddMerchandiseItemWithFeeAmount(type, name, fee, min, max);
        }

        [Step]
        public void AddMerchandiseItemWithFeeAmount(MerchandiseManager.MerchandiseType type, string name, double? fee, double? minFee, double? maxFee)
        {
            
            ClickAddMerchandiseItem ();
            this.MerchMgr.SetName(name);
            this.MerchMgr.SetType(type);
            this.MerchMgr.SetMerchItemPrice(type, fee, minFee, maxFee);
            this.MerchMgr.SaveAndClose();
        }

        [Step]
        public void AddMerchandiseItemWithMultipleChoiceItem(MerchandiseManager.MerchandiseType type, string name, double? fee, double? minFee, double? maxFee, string[] itemname, int? limit)
        {
            ClickAddMerchandiseItem();
            this.MerchMgr.SetName(name);
            this.MerchMgr.SetType(type);
            this.MerchMgr.SetMerchItemPrice(type, fee, minFee, maxFee);

            this.MerchMgr.ExpandAdvanced();
            for (int i = 0; i < itemname.Length; i++)
            {
                this.MerchMgr.AddMerchandiseMultipleChoiceItem(itemname[i], limit);
            }
            this.MerchMgr.SaveAndClose();
        }

        [Verify]
        public void VerifyMerchandiseItem(MerchandiseManager.MerchandiseType type, string name)
        {
            Fee fee = null;

            ClientDataContext db = new ClientDataContext();
            fee = (from f in db.Fees where f.Description == name && f.AmountTypeId == (int)type orderby f.Id ascending select f).ToList().Last();

            Assert.That(fee != null);

            switch (type)
            {
                case MerchandiseManager.MerchandiseType.Fixed:
                    Assert.That(fee.Amount > 0);
                    break;
                case MerchandiseManager.MerchandiseType.Variable:
                    Assert.That(fee.MinVarAmount > 0);
                    Assert.That(fee.MaxVarAmount > 0);
                    break;
            }
        }

        public void OpenMerchandiseItem(string merchName)
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(merchName, LocateBy.LinkText);
            Utility.ThreadSleep(1);
            WebDriverUtility.DefaultProvider.WaitForRADWindow();
            WebDriverUtility.DefaultProvider.SelectPopUpFrameByName(MerchandiseManager.MerchItemDetailDialogID);
        }

        public string GetMerchandiseIDByMerchName(string merchname)
        {
            string MerchHref = WebDriverUtility.DefaultProvider.GetAttribute(merchname, "href", LocateBy.LinkText);
            char[] sp1 = { '=' };
            char[] sp2 = { '&' };

            string MerchID = MerchHref.Split(sp1)[3].Split(sp2)[0];
            return MerchID;
        }
    }
}