namespace RegOnline.RegressionTest.Managers.Builder
{
    using System;
    using RegOnline.RegressionTest.Managers.Builder;
    //using RegOnline.RegressionTest.Fixtures;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.Attributes;
    using System.Collections.Generic;

    public class LodgingStandardFieldsManager
    {
        internal static readonly DateTime DefaultCheckInOutDateFrom = ManagerBase.DefaultEventStartDate.AddDays(-1);
        internal static readonly DateTime DefaultCheckInOutDateTo = ManagerBase.DefaultEventEndDate.AddDays(1);

        public enum LodgingStandardFields
        {
            [StringValue("Check-In Date")]
            CheckInDate,

            [StringValue("Check-Out Date")]
            CheckOutDate,

            [StringValue("Room Preference")]
            RoomPreference,

            [StringValue("Bed Preference")]
            BedPreference,

            [StringValue("Smoking Preference")]
            SmokingPreference,

            [StringValue("Sharing With")]
            SharingWith,

            [StringValue("Adjoining With")]
            AdjoiningWith,

            [StringValue("Additional Info")]
            AdditionalInfo,

            [StringValue("Credit Card Holder")]
            CreditCardHoder,

            [StringValue("Credit Card Number")]
            CreditCardNumber,

            [StringValue("Expiration Date")]
            ExpirationDate
        }

        [Step]
        public void SetRoomType(bool? visible, bool? required)
        {
            if (visible.HasValue)
            {
                WebDriverUtility.DefaultProvider.SetCheckbox("ctl00_cph_chkRoomTypeV", visible.Value, LocateBy.Id);
            }
            if (required.HasValue)
            {
                WebDriverUtility.DefaultProvider.SetCheckbox("ctl00_cph_chkRoomTypeR", required.Value, LocateBy.Id);
            }
        }

        public void SetBedType(bool? visible, bool? required)
        {
            if (visible.HasValue)
            {
                WebDriverUtility.DefaultProvider.SetCheckbox("ctl00_cph_chkBedTypeV", visible.Value, LocateBy.Id);
            }
            if (required.HasValue)
            {
                WebDriverUtility.DefaultProvider.SetCheckbox("ctl00_cph_chkBedTypeR", required.Value, LocateBy.Id);
            }
        }

        public void SetSmokingPreference(bool? visible, bool? required)
        {
            if (visible.HasValue)
            {
                WebDriverUtility.DefaultProvider.SetCheckbox("ctl00_cph_chkSmokingPreferenceV", visible.Value, LocateBy.Id);
            }
            if (required.HasValue)
            {
                WebDriverUtility.DefaultProvider.SetCheckbox("ctl00_cph_chkSmokingPreferenceR", required.Value, LocateBy.Id);
            }
        }

        public void SetSharingWith(bool? visible, bool? required)
        {
            if (visible.HasValue)
            {
                WebDriverUtility.DefaultProvider.SetCheckbox("ctl00_cph_chkSharingWithV", visible.Value, LocateBy.Id);
            }
            if (required.HasValue)
            {
                WebDriverUtility.DefaultProvider.SetCheckbox("ctl00_cph_chkSharingWithR", required.Value, LocateBy.Id);
            }
        }

        public void SetAdjoiningWith(bool? visible, bool? required)
        {
            if (visible.HasValue)
            {
                WebDriverUtility.DefaultProvider.SetCheckbox("ctl00_cph_chkAdjoiningWithV", visible.Value, LocateBy.Id);
            }
            if (required.HasValue)
            {
                WebDriverUtility.DefaultProvider.SetCheckbox("ctl00_cph_chkAdjoiningWithR", required.Value, LocateBy.Id);
            }
        }

        public void SetCheckInOutDate(bool visible, bool required)
        {
            WebDriverUtility.DefaultProvider.SetCheckbox("ctl00_cph_chkCheckInOutDateV", visible, LocateBy.Id);
            WebDriverUtility.DefaultProvider.SetCheckbox("ctl00_cph_chkCheckInOutDateR", required, LocateBy.Id);
        }

        public void SetValidDateRangeForCheckInOut(DateTime from, DateTime to)
        {
            WebDriverUtility.DefaultProvider.SetDateTimeById("ctl00_cph_dtpLodgingDateFrom", from);
            WebDriverUtility.DefaultProvider.SetDateTimeById("ctl00_cph_dtpLodgingDateTo", to);
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("tblLodgingStandardFields", LocateBy.Id);
        }

        public void SetValidDateRangeForCheckInOutDefault()
        {
            this.SetValidDateRangeForCheckInOut(DefaultCheckInOutDateFrom, DefaultCheckInOutDateTo);
        }

        public void SetAdditionalInfo(bool? visible, bool? required)
        {
            if (visible.HasValue)
            {
                WebDriverUtility.DefaultProvider.SetCheckbox("ctl00_cph_chkAdditionalInfoV", visible.Value, LocateBy.Id);
            }
            if (required.HasValue)
            {
                WebDriverUtility.DefaultProvider.SetCheckbox("ctl00_cph_chkAdditionalInfoR", required.Value, LocateBy.Id);
            }
        }
    }
}