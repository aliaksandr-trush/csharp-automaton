namespace RegOnline.RegressionTest.Managers.Builder
{
    using RegOnline.RegressionTest.Utilities;
    //using RegOnline.RegressionTest.Fixtures;
    using RegOnline.RegressionTest.UIUtility;

    public abstract class PredifinedMultiChoiceItemManagerBase : ManagerBase
    {
        public const string PredefinedTypeLocatorFormat = "//div[@id='divPredefinedTypes']/div[text()='{0}']";

        public enum PredefinedItemType
        {
            [StringValue("1-10")]
            OneToTen,

            [StringValue("Age")]
            Age,

            [StringValue("Agreement")]
            Agreement,

            [StringValue("Canadian provinces")]
            CanadianProvinces,

            [StringValue("Comparison")]
            Comparison,

            [StringValue("Continents")]
            Continents,

            [StringValue("Countries")]
            Countries,

            [StringValue("Days of the Week")]
            DaysOfTheWeek,

            [StringValue("Education")]
            Education,

            [StringValue("Employment")]
            Employment,

            [StringValue("Gender")]
            Gender,

            [StringValue("How Long")]
            HowLong,

            [StringValue("How Often")]
            HowOften,

            [StringValue("How should we contact you?")]
            HowShouldWeContactYou,

            [StringValue("Importance")]
            Importance,

            [StringValue("Income")]
            Income,

            [StringValue("Marital Status")]
            MaritalStatus,

            [StringValue("Months of the Year")]
            MonthsOfTheYear,

            [StringValue("Occupation")]
            Occupation,

            [StringValue("Referral Source")]
            ReferralSource,

            [StringValue("Satisfaction")]
            Satisfaction,

            [StringValue("Size")]
            Size,

            [StringValue("US States")]
            USStates,

            [StringValue("Would You")]
            WouldYou,

            [StringValue("Yes/No")]
            YesOrNo
        }

        public enum Agreement
        {
            [StringValue("Strongly Agree")]
            StronglyAgree,

            [StringValue("Agree")]
            Agree,

            [StringValue("Neutral")]
            Neutral,

            [StringValue("Disagree")]
            Disagree,

            [StringValue("Strongly Disagree")]
            StronglyDisagree,

            [StringValue("N/A")]
            NotApplicable
        }

        public enum Gender
        {
            [StringValue("Female")]
            Female,

            [StringValue("Male")]
            Male,

            [StringValue("Prefer Not to Answer")]
            PreferNotToAnswer
        }

        public enum WouldYou
        {
            [StringValue("Definitely")]
            Definitely,

            [StringValue("Probably")]
            Probably,

            [StringValue("Not Sure")]
            NotSure,

            [StringValue("Probably Not")]
            ProbablyNot,

            [StringValue("Definitely Not")]
            DefinitelyNot,
        }

        public enum YesOrNo
        {
            [StringValue("Yes")]
            Yes,

            [StringValue("No")]
            No
        }

        public abstract string FrameID { get; }

        public void SaveAndClose()
        {
            UIUtil.DefaultProvider.ClickSaveAndClose();
            Utility.ThreadSleep(1);
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }

        public void Cancel()
        {
            UIUtil.DefaultProvider.ClickCancel();
            Utility.ThreadSleep(1);
            SelectBuilderWindow();
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }

        public void ClickPredefinedItem(PredefinedItemType type)
        {
            string typeThing = StringEnum.GetStringValue(type);
            UIUtil.DefaultProvider.WaitForDisplayAndClick(string.Format(PredefinedTypeLocatorFormat, typeThing), LocateBy.XPath);
            Utility.ThreadSleep(1);
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }
    }
}
