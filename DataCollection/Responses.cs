namespace RegOnline.RegressionTest.DataCollection
{
    using System;

    public class CustomFieldResponse
    {
        public CustomField CustomField;
    }

    public class CFCheckboxResponse : CustomFieldResponse
    {
        public bool? Checked;
    }

    public class CFRadioButtonResponse : CustomFieldResponse
    {
        public ChoiceItem ChoiceItem;
    }

    public class CFDropDownResponse : CustomFieldResponse
    {
        public ChoiceItem ChoiceItem;
    }

    public class CFCharInputResponse : CustomFieldResponse
    {
        public string CharToInput;
    }

    public class CFDateTimeResponse : CustomFieldResponse
    {
        public DateTime? DateTime;
    }

    public class CFFileUploadResponse : CustomFieldResponse
    {
        public string FileSource;
    }

    public class AgendaResponse : CustomFieldResponse
    {
        public AgendaItem AgendaItem;
    }

    public class AgendaCheckboxResponse : AgendaResponse
    {
        public bool? Checked;
    }

    public class AgendaRadioButtonResponse : AgendaResponse
    {
        public ChoiceItem ChoiceItem;
    }

    public class AgendaDropDownResponse : AgendaResponse
    {
        public ChoiceItem ChoiceItem;
    }

    public class AgendaCharInputResponse : AgendaResponse
    {
        public string CharToInput;
    }

    public class AgendaDateResponse : AgendaResponse
    {
        public DateTime? Date;
    }

    public class AgendaTimeResponse : AgendaResponse
    {
        public DateTime? Time;
    }

    public class AgendaFileUploadResponse : AgendaResponse
    {
        public string FileSource;
    }

    public class AgendaContributionResponse : AgendaResponse
    {
        public double? Contribution;
    }
}
