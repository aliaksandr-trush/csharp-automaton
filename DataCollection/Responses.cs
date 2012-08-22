namespace RegOnline.RegressionTest.DataCollection
{
    using System;

    public class CustomFieldResponse
    {
    }

    public class CFResponse : CustomFieldResponse
    {
        public CustomField CustomField;
    }

    public class CFResponse_Checkbox : CFResponse
    {
        public bool? Checked;
    }

    public class CFResponse_MultipleChoice_RadioButton : CFResponse
    {
        public ChoiceItem ChoiceItem;
    }

    public class CFResponse_MultipleChoice_DropDown : CFResponse
    {
        public ChoiceItem ChoiceItem;
    }

    public class CFResponse_TextInput : CFResponse
    {
        public string CharToInput;
    }

    public class CFResponse_DateTime : CFResponse
    {
        public DateTime? DateTime;
    }

    public class CFResponse_FileUpload : CFResponse
    {
        public string FileSource;
    }

    public class AgendaResponse : CustomFieldResponse
    {
        public AgendaItem AgendaItem;
    }

    public class AgendaResponse_AlwaysSelected : AgendaResponse
    {
        public double? Fee;
        public DiscountCode Code { get; set; }
    }

    public class AgendaResponse_Checkbox : AgendaResponse
    {
        public bool? Checked;
        public double? Fee;
        public DiscountCode Code;
    }

    public class AgendaResponse_MultipleChoice_RadioButton : AgendaResponse
    {
        public ChoiceItem ChoiceItem;
        public double? Fee;
        public DiscountCode Code;
    }

    public class AgendaResponse_MultipleChoice_DropDown : AgendaResponse
    {
        public ChoiceItem ChoiceItem;
        public double? Fee;
        public DiscountCode Code;
    }

    public class AgendaResponse_TextInput : AgendaResponse
    {
        public string CharToInput;
    }

    public class AgendaResponse_Date : AgendaResponse
    {
        public DateTime? Date;
    }

    public class AgendaResponse_Time : AgendaResponse
    {
        public DateTime? Time;
    }

    public class AgendaResponse_FileUpload : AgendaResponse
    {
        public string FileSource;
        public double? Fee;
        public DiscountCode Code;
    }

    public class AgendaResponse_Contribution : AgendaResponse
    {
        public double? ContributionAmount;
    }

    public class AgendaResponse_Duration : AgendaResponse
    {
        public TimeSpan Duration;
    }

    public class MerchandiseResponse
    {
        public MerchandiseItem Merchandise_Item;
    }

    public class MerchResponse_FixedPrice : MerchandiseResponse
    {
        public int Quantity;
        public DiscountCode Discount_Code;
    }

    public class MerchResponse_VariableAmount : MerchandiseResponse
    {
        public double Amount;
    }

    public class EventFeeResponse
    {
        public RegType RegType;
        public DiscountCode Code;
        public double? Fee;

        public EventFeeResponse() { }

        public EventFeeResponse(RegType regType)
        {
            this.RegType = regType;
        }
    }
}
