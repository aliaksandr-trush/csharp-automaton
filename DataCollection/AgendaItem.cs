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

    public class AgendaItem_Common : AgendaItem
    {
        public AgendaItem_Common(string name) : base(name) { }

        public DateTime? StartDate;
        public DateTime? StartTime;
        public DateTime? EndDate;
        public DateTime? EndTime;
        public string Location;
        public double? Price;
        public EarlyPrice EarlyPrice;
        public LatePrice LatePrice;
        public List<DiscountCode> DiscountCodes = new List<DiscountCode>();
        public string BulkCodes;
        public bool? RequireDC;
        public bool? ApplyTaxOne;
        public bool? ApplyTaxTwo;
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

    public class AgendaItem_TextInput : AgendaItem
    {
        public AgendaItem_TextInput(string name) : base(name) { }

        public int? CharLimit;
        public string DetailsPopup;
        public string DetailsURL;
        public string GroupName;
        public bool? ForceGroupToMatch;
    }

    public class AgendaItem_MultipleChoice : AgendaItem_Common
    {
        public AgendaItem_MultipleChoice(string name) : base(name) { }

        public List<ChoiceItem> ChoiceItems = new List<ChoiceItem>();
        public List<FormData.CommonlyUsedMultipleChoice> CommonlyUsedItems = new List<FormData.CommonlyUsedMultipleChoice>();
        public string GroupName;
        public bool? ForceGroupToMatch;
    }

    public class AgendaItem_CheckBox : AgendaItem_Common
    {
        public AgendaItem_CheckBox(string name)
            : base(name)
        {
            Type = FormData.CustomFieldType.CheckBox;
        }

        public string GroupName;
        public bool? ForceGroupToMatch;
    }

    public class AgendaItem_MultipleChoice_RadioButton : AgendaItem_MultipleChoice
    {
        public AgendaItem_MultipleChoice_RadioButton(string name)
            : base(name)
        {
            Type = FormData.CustomFieldType.RadioButton;
        }
    }

    public class AgendaItem_MultipleChoice_DropDown : AgendaItem_MultipleChoice
    {
        public AgendaItem_MultipleChoice_DropDown(string name)
            : base(name)
        {
            Type = FormData.CustomFieldType.Dropdown;
        }
    }

    public class AgendaItem_Time : AgendaItem_Common
    {
        public AgendaItem_Time(string name)
            : base(name)
        {
            Type = FormData.CustomFieldType.Time;
        }

        public string GroupName;
        public bool? ForceGroupToMatch;
    }

    public class AgendaItem_Header : AgendaItem
    {
        public AgendaItem_Header(string name)
            : base(name)
        {
            Type = FormData.CustomFieldType.SectionHeader;
        }

        public string DetailsPopup;
        public string DetailsURL;
    }

    public class AgendaItem_AlwaysSelected : AgendaItem_Common
    {
        public AgendaItem_AlwaysSelected(string name)
            : base(name)
        {
            Type = FormData.CustomFieldType.AlwaysSelected;
        }
    }

    public class AgendaItem_ContinueButton : AgendaItem
    {
        public AgendaItem_ContinueButton(string name)
            : base(name)
        {
            Type = FormData.CustomFieldType.ContinueButton;
        }
    }

    public class AgendaItem_Date : AgendaItem_Common
    {
        public AgendaItem_Date(string name)
            : base(name)
        {
            Type = FormData.CustomFieldType.Date;
        }

        public string GroupName;
        public bool? ForceGroupToMatch;
    }

    public class AgendaItem_FileUpload : AgendaItem_Common
    {
        public AgendaItem_FileUpload(string name)
            : base(name)
        {
            Type = FormData.CustomFieldType.FileUpload;
        }

        public string GroupName;
    }

    public class AgendaItem_Number : AgendaItem_TextInput
    {
        public AgendaItem_Number(string name)
            : base(name)
        {
            Type = FormData.CustomFieldType.Number;
        }
    }

    public class AgendaItem_OneLineText : AgendaItem_TextInput
    {
        public AgendaItem_OneLineText(string name)
            : base(name)
        {
            Type = FormData.CustomFieldType.OneLineText;
        }
    }

    public class AgendaItem_Paragraph : AgendaItem_TextInput
    {
        public AgendaItem_Paragraph(string name)
            : base(name)
        {
            Type = FormData.CustomFieldType.Paragraph;
        }
    }

    public class AgendaItem_Contribution : AgendaItem
    {
        public AgendaItem_Contribution(string name)
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
