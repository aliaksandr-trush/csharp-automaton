namespace RegOnline.RegressionTest.DataCollection
{
    using System;

    public class CustomFieldResponse
    {
        public bool IsUpdate = true;
    }

    public class CFResponse : CustomFieldResponse
    {
        public CustomField CustomField;
    }

    public class CFCheckboxResponse : CFResponse
    {
        public bool? Checked;
    }

    public class CFRadioButtonResponse : CFResponse
    {
        public ChoiceItem ChoiceItem;
    }

    public class CFDropDownResponse : CFResponse
    {
        public ChoiceItem ChoiceItem;
    }

    public class CFCharInputResponse : CFResponse
    {
        public string CharToInput;
    }

    public class CFDateTimeResponse : CFResponse
    {
        public DateTime? DateTime;
    }

    public class CFFileUploadResponse : CFResponse
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
        public DiscountCode Code;
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

    public class AgendaResponse_Duration : AgendaResponse
    {
        public TimeSpan Duration;
    }
}
