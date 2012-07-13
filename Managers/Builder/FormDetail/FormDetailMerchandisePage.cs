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
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(GetAddGridItemLocator("ctl00_cph_grdFees_"), LocateBy.Id);
            Utility.ThreadSleep(1);
            UIUtilityProvider.UIHelper.WaitForRADWindow();
            UIUtilityProvider.UIHelper.SelectPopUpFrameByName(MerchandiseManager.MerchItemDetailDialogID);
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
            ClickAddMerchandiseItem();
            this.MerchMgr.SetName(name);
            this.MerchMgr.SetType(type);
            this.MerchMgr.SetMerchItemPrice(type, fee, minFee, maxFee);
            this.MerchMgr.SaveAndClose();
        }

        [Verify]
        public void VerifyMerchandiseItem(MerchandiseManager.MerchandiseType type, string name)
        {
            ////ReloadEvent();

            Fee fee = null;

            ClientDataContext db = new ClientDataContext();
            fee = (from f in db.Fees where f.Description == name && f.AmountTypeId == (int)type orderby f.Id ascending select f).ToList().Last();

            //E.Fees fee = Event.FeesCollection.Find(
            //    delegate(E.Fees feeInner)
            //    {
            //        return feeInner.Description == name &&
            //            feeInner.ReportDescription == feeInner.Description &&
            //            feeInner.Fieldname == feeInner.Description &&
            //            feeInner.AmountTypeId == (int)type;
            //    }
            //);
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
                ////case MerchandiseManager.MerchandiseType.Percentage:
                ////    Assert.That(fee.Pct > 0);
                ////    break;
            }
        }

        public void OpenMerchandiseItem(string merchName)
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(merchName, LocateBy.LinkText);
            Utility.ThreadSleep(1);
            UIUtilityProvider.UIHelper.WaitForRADWindow();
            UIUtilityProvider.UIHelper.SelectPopUpFrameByName(MerchandiseManager.MerchItemDetailDialogID);
        }

        public string GetMerchandiseIDByMerchName(string merchname)
        {
            string MerchHref = UIUtilityProvider.UIHelper.GetAttribute(merchname, "href", LocateBy.LinkText);
            char[] sp1 = { '=' };
            char[] sp2 = { '&' };

            string MerchID = MerchHref.Split(sp1)[3].Split(sp2)[0];
            return MerchID;
        }
    }
}