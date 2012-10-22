namespace RegOnline.RegressionTest.DataCollection
{
    using System;
    using System.Collections.Generic;

    public class CustomFieldResponseList : List<CustomFieldResponse>
    {
        public new void Add(CustomFieldResponse response)
        {
            base.Add(response.Clone());
        }

        public bool HasAgendaResponse()
        {
            bool hasAgendaResponse = false;
            
            foreach (CustomFieldResponse re in this)
            {
                if (re is AgendaResponse)
                {
                    hasAgendaResponse = true;
                }
            }

            return hasAgendaResponse;
        }
    }

    public class CustomFieldResponse
    {
        public virtual CustomFieldResponse Clone()
        {
            return this;
        }
    }

    public class CFResponse : CustomFieldResponse
    {
        public CustomField CustomField;

        public override CustomFieldResponse Clone()
        {
            CFResponse copy = new CFResponse();
            copy.CustomField = this.CustomField;
            return copy;
        }
    }

    public class CFResponse_Checkbox : CFResponse
    {
        public bool? Checked;

        public override CustomFieldResponse Clone()
        {
            CFResponse_Checkbox copy = new CFResponse_Checkbox();
            copy.CustomField = this.CustomField;
            copy.Checked = this.Checked;
            return copy;
        }
    }

    public class CFResponse_MultipleChoice_RadioButton : CFResponse
    {
        public ChoiceItem ChoiceItem;

        public override CustomFieldResponse Clone()
        {
            CFResponse_MultipleChoice_RadioButton copy = new CFResponse_MultipleChoice_RadioButton();
            copy.CustomField = this.CustomField;
            copy.ChoiceItem = this.ChoiceItem;
            return copy;
        }
    }

    public class CFResponse_MultipleChoice_DropDown : CFResponse
    {
        public ChoiceItem ChoiceItem;

        public override CustomFieldResponse Clone()
        {
            CFResponse_MultipleChoice_DropDown copy = new CFResponse_MultipleChoice_DropDown();
            copy.CustomField = this.CustomField;
            copy.ChoiceItem = this.ChoiceItem;
            return copy;
        }
    }

    public class CFResponse_TextInput : CFResponse
    {
        public string CharToInput;

        public override CustomFieldResponse Clone()
        {
            CFResponse_TextInput copy = new CFResponse_TextInput();
            copy.CustomField = this.CustomField;
            copy.CharToInput = string.Copy(this.CharToInput);
            return copy;
        }
    }

    public class CFResponse_DateTime : CFResponse
    {
        public DateTime? DateTime;

        public override CustomFieldResponse Clone()
        {
            CFResponse_DateTime copy = new CFResponse_DateTime();
            copy.CustomField = this.CustomField;

            if (this.DateTime.HasValue)
            {
                copy.DateTime = this.DateTime;
            }

            return copy;
        }
    }

    public class CFResponse_FileUpload : CFResponse
    {
        public string FileSource;

        public override CustomFieldResponse Clone()
        {
            CFResponse_FileUpload copy = new CFResponse_FileUpload();
            copy.CustomField = this.CustomField;
            copy.FileSource = string.Copy(this.FileSource);
            return copy;
        }
    }

    public class AgendaResponse : CustomFieldResponse
    {
        public AgendaItem AgendaItem;

        public override CustomFieldResponse Clone()
        {
            AgendaResponse copy = new AgendaResponse();
            copy.AgendaItem = this.AgendaItem;
            return copy;
        }
    }

    public class AgendaResponse_AlwaysSelected : AgendaResponse
    {
        public double Fee { get; set; }
        public CustomFieldCode Code { get; set; }

        public override CustomFieldResponse Clone()
        {
            AgendaResponse_AlwaysSelected copy = new AgendaResponse_AlwaysSelected();
            copy.AgendaItem = this.AgendaItem;
            copy.Fee = this.Fee;
            copy.Code = this.Code;
            return copy;
        }
    }

    public class AgendaResponse_Checkbox : AgendaResponse
    {
        public bool? Checked;
        public double Fee { get; set; }
        public CustomFieldCode Code;

        public override CustomFieldResponse Clone()
        {
            AgendaResponse_Checkbox copy = new AgendaResponse_Checkbox();
            copy.AgendaItem = this.AgendaItem;
            copy.Checked = this.Checked;
            copy.Fee = this.Fee;
            copy.Code = this.Code;
            return copy;
        }
    }

    public class AgendaResponse_MultipleChoice_RadioButton : AgendaResponse
    {
        public ChoiceItem ChoiceItem;
        public double Fee { get; set; }
        public CustomFieldCode Code;

        public override CustomFieldResponse Clone()
        {
            AgendaResponse_MultipleChoice_RadioButton copy = new AgendaResponse_MultipleChoice_RadioButton();
            copy.AgendaItem = this.AgendaItem;
            copy.ChoiceItem = this.ChoiceItem;
            copy.Fee = this.Fee;
            copy.Code = this.Code;
            return copy;
        }
    }

    public class AgendaResponse_MultipleChoice_DropDown : AgendaResponse
    {
        public ChoiceItem ChoiceItem;
        public double Fee { get; set; }
        public CustomFieldCode Code;

        public override CustomFieldResponse Clone()
        {
            AgendaResponse_MultipleChoice_DropDown copy = new AgendaResponse_MultipleChoice_DropDown();
            copy.AgendaItem = this.AgendaItem;
            copy.ChoiceItem = this.ChoiceItem;
            copy.Fee = this.Fee;
            copy.Code = this.Code;
            return copy;
        }
    }

    public class AgendaResponse_TextInput : AgendaResponse
    {
        public string CharToInput;

        public override CustomFieldResponse Clone()
        {
            AgendaResponse_TextInput copy = new AgendaResponse_TextInput();
            copy.AgendaItem = this.AgendaItem;
            copy.CharToInput = string.Copy(this.CharToInput);
            return copy;
        }
    }

    public class AgendaResponse_Date : AgendaResponse
    {
        public DateTime? Date;

        public override CustomFieldResponse Clone()
        {
            AgendaResponse_Date copy = new AgendaResponse_Date();
            copy.AgendaItem = this.AgendaItem;

            if (this.Date.HasValue)
            {
                copy.Date = this.Date;
            }

            return copy;
        }
    }

    public class AgendaResponse_Time : AgendaResponse
    {
        public DateTime? Time;

        public override CustomFieldResponse Clone()
        {
            AgendaResponse_Time copy = new AgendaResponse_Time();
            copy.AgendaItem = this.AgendaItem;

            if (this.Time.HasValue)
            {
                copy.Time = this.Time;
            }

            return copy;
        }
    }

    public class AgendaResponse_FileUpload : AgendaResponse
    {
        public string FileSource;
        public double Fee { get; set; }
        public CustomFieldCode Code;

        public override CustomFieldResponse Clone()
        {
            AgendaResponse_FileUpload copy = new AgendaResponse_FileUpload();
            copy.AgendaItem = this.AgendaItem;
            copy.FileSource = string.Copy(this.FileSource);
            copy.Fee = this.Fee;
            copy.Code = this.Code;
            return copy;
        }
    }

    public class AgendaResponse_Contribution : AgendaResponse
    {
        public double ContributionAmount { get; set; }

        public override CustomFieldResponse Clone()
        {
            AgendaResponse_Contribution copy = new AgendaResponse_Contribution();
            copy.AgendaItem = this.AgendaItem;
            copy.ContributionAmount = this.ContributionAmount;
            return copy;
        }
    }

    public class AgendaResponse_Duration : AgendaResponse
    {
        public TimeSpan Duration;

        public override CustomFieldResponse Clone()
        {
            AgendaResponse_Duration copy = new AgendaResponse_Duration();
            copy.AgendaItem = this.AgendaItem;
            copy.Duration = new TimeSpan(this.Duration.Ticks);
            return copy;
        }
    }

    public class MerchandiseResponseList : List<MerchandiseResponse>
    {
        public new void Add(MerchandiseResponse response)
        {
            base.Add(response.Clone());
        }
    }

    public class MerchandiseResponse
    {
        public MerchandiseItem Merchandise_Item;
        public double Fee { get; set; }

        public virtual MerchandiseResponse Clone()
        {
            MerchandiseResponse copy = new MerchandiseResponse();
            copy.Merchandise_Item = this.Merchandise_Item;
            copy.Fee = this.Fee;
            return copy;
        }
    }

    public class MerchResponse_FixedPrice : MerchandiseResponse
    {
        public int Quantity;
        public CustomFieldCode Discount_Code;

        public override MerchandiseResponse Clone()
        {
            MerchResponse_FixedPrice copy = new MerchResponse_FixedPrice();
            copy.Merchandise_Item = this.Merchandise_Item;
            copy.Quantity = this.Quantity;
            copy.Discount_Code = this.Discount_Code;
            copy.Fee = this.Fee;
            return copy;
        }
    }

    public class MerchResponse_VariableAmount : MerchandiseResponse
    {
        public double Amount;

        public override MerchandiseResponse Clone()
        {
            MerchResponse_VariableAmount copy = new MerchResponse_VariableAmount();
            copy.Merchandise_Item = this.Merchandise_Item;
            copy.Amount = this.Amount;
            copy.Fee = this.Fee;
            return copy;
        }
    }

    public class EventFeeResponse
    {
        public RegType RegType;
        public CustomFieldCode Code;
        public double Fee { get; set; }

        public EventFeeResponse() { }

        public EventFeeResponse(RegType regType)
        {
            this.RegType = regType;
        }

        public EventFeeResponse Clone()
        {
            EventFeeResponse copy = new EventFeeResponse();
            copy.RegType = this.RegType;
            copy.Code = this.Code;
            copy.Fee = this.Fee;
            return copy;
        }
    }

    public class LodgingResponse
    {
        public Hotel Hotel;
        public RoomType RoomType;
        public DateTime? CheckinDate;
        public DateTime? CheckoutDate;
        public double Fee { get; set; }

        public LodgingResponse Clone()
        {
            LodgingResponse copy = new LodgingResponse();
            copy.Hotel = this.Hotel;
            copy.RoomType = this.RoomType;
            copy.CheckinDate = this.CheckinDate;
            copy.CheckoutDate = this.CheckoutDate;
            copy.Fee = this.Fee;
            return copy;
        }
    }

    public class LodgingResponseList : List<LodgingResponse>
    {
        public new void Add(LodgingResponse response)
        {
            base.Add(response.Clone());
        }
    }
}
