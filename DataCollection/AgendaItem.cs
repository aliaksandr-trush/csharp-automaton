namespace RegOnline.RegressionTest.DataCollection
{
    using System;
    using System.Collections.Generic;

    public class AgendaItem : CustomField
    {
        public AgendaItem(string name) : base(name) { }

        public bool? IncludeOnEventWeb;
        public FormData.Gender? Gender;
        public int? AgeGreaterThan;
        public DateTime? AgeGreaterThanDate;
        public int? AgeLessThan;
        public DateTime? AgeLessThanDate;
    }

    public class AgendaItemCommon : AgendaItem
    {
        public AgendaItemCommon(string name) : base(name) { }

        public string NameOnBadge;
        public DateTime? StartDate;
        public DateTime? StartTime;
        public DateTime? EndDate;
        public DateTime? EndTime;
        public string Location;
        public double? Price;
        public EarlyPrice EarlyPrice;
        public LatePrice LatePrice;
        public List<DiscountCode> DiscountCode = new List<DiscountCode>();
        public string BulkCodes;
        public bool? RequireDC;
        public TaxRate TaxRateOne;
        public TaxRate TaxRateTwo;
        public int? SpacesAvailable;
        public bool? ShowCapacity;
        public FormData.AgendaLimitReachedOption? LimitReachedOption;
        public string LimitReachedMessage;
        public string WaitlistConfirmationText;
        public bool? AddToCalendar;
        public FormData.DateFormat? DateFormat;
        public string DetailsPopup;
        public string DetailsURL;
        public FormData.AgendaInitialStatus? InitialStatus;
        public string ConfirmationAddendum;
    }

    public class CharInputAgendaItem : AgendaItem
    {
        public CharInputAgendaItem(string name) : base(name) { }

        public string NameOnBadge;
        public int? CharLimit;
        public string DetailsPopup;
        public string DetailsURL;
        public string GroupName;
        public bool? ForceGroupToMatch;
    }

    public class AgendaItemMultipleChoice : AgendaItemCommon
    {
        public AgendaItemMultipleChoice(string name) : base(name) { }

        public List<ChoiceItem> ChoiceItems = new List<ChoiceItem>();
        public List<FormData.CommonlyUsedMultipleChoice> CommonlyUsedItems = new List<FormData.CommonlyUsedMultipleChoice>();
        public string GroupName;
        public bool? ForceGroupToMatch;
    }

    public class AgendaItemCheckBox : AgendaItemCommon
    {
        public AgendaItemCheckBox(string name)
            : base(name)
        {
            Type = FormData.CustomFieldType.CheckBox;
        }

        public string GroupName;
        public bool? ForceGroupToMatch;
    }

    public class AgendaItemRadioButton : AgendaItemMultipleChoice
    {
        public AgendaItemRadioButton(string name)
            : base(name)
        {
            Type = FormData.CustomFieldType.RadioButton;
        }
    }

    public class AgendaItemDropDown : AgendaItemMultipleChoice
    {
        public AgendaItemDropDown(string name)
            : base(name)
        {
            Type = FormData.CustomFieldType.Dropdown;
        }
    }

    public class AgendaItemTime : AgendaItemCommon
    {
        public AgendaItemTime(string name)
            : base(name)
        {
            Type = FormData.CustomFieldType.Time;
        }

        public string GroupName;
        public bool? ForceGroupToMatch;
    }

    public class AgendaItemHeader : AgendaItem
    {
        public AgendaItemHeader(string name)
            : base(name)
        {
            Type = FormData.CustomFieldType.SectionHeader;
        }

        public string DetailsPopup;
        public string DetailsURL;
    }

    public class AgendaItemAlways : AgendaItemCommon
    {
        public AgendaItemAlways(string name)
            : base(name)
        {
            Type = FormData.CustomFieldType.AlwaysSelected;
        }
    }

    public class AgendaItemContinue : AgendaItem
    {
        public AgendaItemContinue(string name)
            : base(name)
        {
            Type = FormData.CustomFieldType.ContinueButton;
        }
    }

    public class AgendaItemDate : AgendaItemCommon
    {
        public AgendaItemDate(string name)
            : base(name)
        {
            Type = FormData.CustomFieldType.Date;
        }

        public string GroupName;
        public bool? ForceGroupToMatch;
    }

    public class AgendaItemUpload : AgendaItemCommon
    {
        public AgendaItemUpload(string name)
            : base(name)
        {
            Type = FormData.CustomFieldType.FileUpload;
        }

        public string GroupName;
    }

    public class AgendaItemNumber : CharInputAgendaItem
    {
        public AgendaItemNumber(string name)
            : base(name)
        {
            Type = FormData.CustomFieldType.Number;
        }
    }

    public class AgendaItemOneLineText : CharInputAgendaItem
    {
        public AgendaItemOneLineText(string name)
            : base(name)
        {
            Type = FormData.CustomFieldType.OneLineText;
        }
    }

    public class AgendaItemParagraph : CharInputAgendaItem
    {
        public AgendaItemParagraph(string name)
            : base(name)
        {
            Type = FormData.CustomFieldType.Paragraph;
        }
    }

    public class AgendaItemContribution : AgendaItem
    {
        public AgendaItemContribution(string name)
            : base(name)
        {
            Type = FormData.CustomFieldType.Contribution;
        }

        public double MinAmount;
        public double MaxAmount;
    }

    public class AgendaItem_Duration : AgendaItem
    {
        public AgendaItem_Duration(string name)
            : base(name)
        {
            Type = FormData.CustomFieldType.Duration;
        }
    }
}
