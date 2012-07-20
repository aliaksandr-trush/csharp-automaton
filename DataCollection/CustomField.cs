namespace RegOnline.RegressionTest.DataCollection
{
    using System;
    using System.Collections.Generic;

    public class CustomField
    {
        public CustomField(string name)
        {
            this.NameOnForm = name;
        }

        public int Id;
        public string NameOnForm;
        public string NameOnReceipt;
        public string NameOnReports;
        public FormData.CustomFieldType Type;
        public List<CustomFieldVisibleOption> CustomFieldVisibleOption = new List<CustomFieldVisibleOption>();
        public List<string> ConditionalLogic = new List<string>();
        public DateTime? ShowStarting;
        public DateTime? HideStarting;
    }

    public class CommonCustomField : CustomField
    {
        public CommonCustomField(string name) : base(name) { }

        public string NameOnBadge;
        public int? SpacesAvailable;
        public bool? ShowCapacity;
        public FormData.AgendaLimitReachedOption? LimitReachedOption;
        public string LimitReachedMessage;
        public string DetailsPopup;
        public string DetailsURL;
    }

    public class CharInputCustomField : CustomField
    {
        public CharInputCustomField(string name) : base(name) { }

        public string NameOnBadge;
        public int CharLimit;
        public string DetailsPopup;
        public string DetailsURL;
        public string GroupName;
        public bool? ForceGroupToMatch;
    }

    public class CFMultipleChoice : CommonCustomField
    {
        public CFMultipleChoice(string name) : base(name) { }

        public List<ChoiceItem> ChoiceItems = new List<ChoiceItem>();
        public List<FormData.CommonlyUsedMultipleChoice> CommonlyUsedItems = new List<FormData.CommonlyUsedMultipleChoice>();
        public string GroupName;
        public bool? ForceGroupToMatch;
    }

    public class CFCheckBox : CommonCustomField
    {
        public CFCheckBox(string name)
            : base(name)
        {
            Type = FormData.CustomFieldType.CheckBox;
        }

        public string GroupName;
        public bool? ForceGroupToMatch;
    }

    public class CFRadioButton : CFMultipleChoice
    {
        public CFRadioButton(string name)
            : base(name)
        {
            Type = FormData.CustomFieldType.RadioButton;
        }
    }

    public class CFDropDown : CFMultipleChoice
    {
        public CFDropDown(string name)
            : base(name)
        {
            Type = FormData.CustomFieldType.Dropdown;
        }
    }

    public class CFTime : CommonCustomField
    {
        public CFTime(string name)
            : base(name)
        {
            Type = FormData.CustomFieldType.Time;
        }

        public string GroupName;
        public bool? ForceGroupToMatch;
    }

    public class CFHeader : CustomField
    {
        public CFHeader(string name)
            : base(name)
        {
            Type = FormData.CustomFieldType.SectionHeader;
        }

        public string DetailsPopup;
        public string DetailsURL;
    }

    public class CFAlways : CommonCustomField
    {
        public CFAlways(string name)
            : base(name)
        {
            Type = FormData.CustomFieldType.AlwaysSelected;
        }
    }

    public class CFContinue : CustomField
    {
        public CFContinue(string name)
            : base(name)
        {
            Type = FormData.CustomFieldType.ContinueButton;
        }
    }

    public class CFDate : CommonCustomField
    {
        public CFDate(string name)
            : base(name)
        {
            Type = FormData.CustomFieldType.Time;
        }

        public string GroupName;
        public bool? ForceGroupToMatch;
    }

    public class CFUpload : CommonCustomField
    {
        public CFUpload(string name)
            : base(name)
        {
            Type = FormData.CustomFieldType.FileUpload;
        }

        public string GroupName;
    }

    public class CFNumber : CharInputCustomField
    {
        public CFNumber(string name)
            : base(name)
        {
            Type = FormData.CustomFieldType.Number;
        }
    }

    public class CFOneLineText : CharInputCustomField
    {
        public CFOneLineText(string name)
            : base(name)
        {
            Type = FormData.CustomFieldType.Number;
        }
    }

    public class CFParagraph : CharInputCustomField
    {
        public CFParagraph(string name)
            : base(name)
        {
            Type = FormData.CustomFieldType.Paragraph;
        }
    }

    public class CustomFieldVisibleOption
    {
        public RegType RegType = new RegType();
        public bool? Visible;
        public bool? Required;
        public bool? AdminOnly;

        public CustomFieldVisibleOption()
        {
            this.RegType = null;
        }

        public CustomFieldVisibleOption(RegType regType)
        {
            this.RegType = regType;
        }
    }

    public class ChoiceItem
    {
        public int Id;
        public string Name;
        public double? Price;
        public int? SingleLimit;
        public int? GroupLimit;
        public bool? Visible;

        public ChoiceItem(string name)
        {
            this.Name = name;
        }
    }

    public class MultipleChoice_CommonlyUsed
    {
        public static CommonlyUsed_Agreement Agreement = new CommonlyUsed_Agreement();
        public static CommonlyUsed_YesOrNo YesOrNo = new CommonlyUsed_YesOrNo();
    }

    public class CommonlyUsed_Agreement
    {
        public readonly string Agree = "Agree";
        public readonly string StronglyAgree = "Strongly Agree";
        public readonly string Neutral = "Neutral";
        public readonly string Disagree = "Disagree";
        public readonly string NA = "N/A";
    }

    public class CommonlyUsed_YesOrNo
    {
        public readonly string Yes = "Yes";
        public readonly string No = "No";
    }
}
