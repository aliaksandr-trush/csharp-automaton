﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RegOnline.RegressionTest.Fixtures.API.EventService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="https://www.regonline.com/webservices/", ConfigurationName="EventService.EventServiceSoap")]
    public interface EventServiceSoap {
        
        // CODEGEN: Generating message contract since message CreateEventFromTemplateRequest has headers
        [System.ServiceModel.OperationContractAttribute(Action="https://www.regonline.com/webservices/CreateEventFromTemplate", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        RegOnline.RegressionTest.Fixtures.API.EventService.CreateEventFromTemplateResponse CreateEventFromTemplate(RegOnline.RegressionTest.Fixtures.API.EventService.CreateEventFromTemplateRequest request);
        
        // CODEGEN: Generating message contract since message UpdateEventRequest has headers
        [System.ServiceModel.OperationContractAttribute(Action="https://www.regonline.com/webservices/UpdateEvent", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        RegOnline.RegressionTest.Fixtures.API.EventService.UpdateEventResponse UpdateEvent(RegOnline.RegressionTest.Fixtures.API.EventService.UpdateEventRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.regonline.com/webservices/")]
    public partial class AuthenticationHeader : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string userNameField;
        
        private string passwordField;
        
        private System.Xml.XmlAttribute[] anyAttrField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string UserName {
            get {
                return this.userNameField;
            }
            set {
                this.userNameField = value;
                this.RaisePropertyChanged("UserName");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string Password {
            get {
                return this.passwordField;
            }
            set {
                this.passwordField = value;
                this.RaisePropertyChanged("Password");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr {
            get {
                return this.anyAttrField;
            }
            set {
                this.anyAttrField = value;
                this.RaisePropertyChanged("AnyAttr");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="https://www.regonline.com/webservices/")]
    public partial class RegOnlineResponseOfBoolean : object, System.ComponentModel.INotifyPropertyChanged {
        
        private bool valueField;
        
        private ResponseStatus statusField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public bool Value {
            get {
                return this.valueField;
            }
            set {
                this.valueField = value;
                this.RaisePropertyChanged("Value");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public ResponseStatus Status {
            get {
                return this.statusField;
            }
            set {
                this.statusField = value;
                this.RaisePropertyChanged("Status");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.regonline.com/webservices/")]
    public partial class ResponseStatus : object, System.ComponentModel.INotifyPropertyChanged {
        
        private bool successField;
        
        private string errorMessageField;
        
        private int errorCodeField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public bool Success {
            get {
                return this.successField;
            }
            set {
                this.successField = value;
                this.RaisePropertyChanged("Success");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string ErrorMessage {
            get {
                return this.errorMessageField;
            }
            set {
                this.errorMessageField = value;
                this.RaisePropertyChanged("ErrorMessage");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public int ErrorCode {
            get {
                return this.errorCodeField;
            }
            set {
                this.errorCodeField = value;
                this.RaisePropertyChanged("ErrorCode");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="https://www.regonline.com/webservices/")]
    public partial class RegOnlineResponseOfInt32 : object, System.ComponentModel.INotifyPropertyChanged {
        
        private int valueField;
        
        private ResponseStatus statusField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public int Value {
            get {
                return this.valueField;
            }
            set {
                this.valueField = value;
                this.RaisePropertyChanged("Value");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public ResponseStatus Status {
            get {
                return this.statusField;
            }
            set {
                this.statusField = value;
                this.RaisePropertyChanged("Status");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="https://www.regonline.com/webservices/")]
    public partial class EventBasics : object, System.ComponentModel.INotifyPropertyChanged {
        
        private int idField;
        
        private string clientEventIDField;
        
        private string eventTitleField;
        
        private System.Nullable<System.DateTime> startDateField;
        
        private System.Nullable<System.DateTime> endDateField;
        
        private string locationNameField;
        
        private string address1Field;
        
        private string address2Field;
        
        private string cityField;
        
        private string stateField;
        
        private string postalCodeField;
        
        private string countryField;
        
        private string contactEmailAddressField;
        
        private string redirectConfirmationURLField;
        
        private string creditCardDescriptorField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public int ID {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
                this.RaisePropertyChanged("ID");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string ClientEventID {
            get {
                return this.clientEventIDField;
            }
            set {
                this.clientEventIDField = value;
                this.RaisePropertyChanged("ClientEventID");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string EventTitle {
            get {
                return this.eventTitleField;
            }
            set {
                this.eventTitleField = value;
                this.RaisePropertyChanged("EventTitle");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=3)]
        public System.Nullable<System.DateTime> StartDate {
            get {
                return this.startDateField;
            }
            set {
                this.startDateField = value;
                this.RaisePropertyChanged("StartDate");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=4)]
        public System.Nullable<System.DateTime> EndDate {
            get {
                return this.endDateField;
            }
            set {
                this.endDateField = value;
                this.RaisePropertyChanged("EndDate");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=5)]
        public string LocationName {
            get {
                return this.locationNameField;
            }
            set {
                this.locationNameField = value;
                this.RaisePropertyChanged("LocationName");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=6)]
        public string Address1 {
            get {
                return this.address1Field;
            }
            set {
                this.address1Field = value;
                this.RaisePropertyChanged("Address1");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=7)]
        public string Address2 {
            get {
                return this.address2Field;
            }
            set {
                this.address2Field = value;
                this.RaisePropertyChanged("Address2");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=8)]
        public string City {
            get {
                return this.cityField;
            }
            set {
                this.cityField = value;
                this.RaisePropertyChanged("City");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=9)]
        public string State {
            get {
                return this.stateField;
            }
            set {
                this.stateField = value;
                this.RaisePropertyChanged("State");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=10)]
        public string PostalCode {
            get {
                return this.postalCodeField;
            }
            set {
                this.postalCodeField = value;
                this.RaisePropertyChanged("PostalCode");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=11)]
        public string Country {
            get {
                return this.countryField;
            }
            set {
                this.countryField = value;
                this.RaisePropertyChanged("Country");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=12)]
        public string ContactEmailAddress {
            get {
                return this.contactEmailAddressField;
            }
            set {
                this.contactEmailAddressField = value;
                this.RaisePropertyChanged("ContactEmailAddress");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=13)]
        public string RedirectConfirmationURL {
            get {
                return this.redirectConfirmationURLField;
            }
            set {
                this.redirectConfirmationURLField = value;
                this.RaisePropertyChanged("RedirectConfirmationURL");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=14)]
        public string CreditCardDescriptor {
            get {
                return this.creditCardDescriptorField;
            }
            set {
                this.creditCardDescriptorField = value;
                this.RaisePropertyChanged("CreditCardDescriptor");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="CreateEventFromTemplate", WrapperNamespace="https://www.regonline.com/webservices/", IsWrapped=true)]
    public partial class CreateEventFromTemplateRequest {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://www.regonline.com/webservices/")]
        public RegOnline.RegressionTest.Fixtures.API.EventService.AuthenticationHeader AuthenticationHeader;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="https://www.regonline.com/webservices/", Order=0)]
        public int sourceEventID;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="https://www.regonline.com/webservices/", Order=1)]
        public int destinationCustomerID;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="https://www.regonline.com/webservices/", Order=2)]
        public RegOnline.RegressionTest.Fixtures.API.EventService.EventBasics eventBasics;
        
        public CreateEventFromTemplateRequest() {
        }
        
        public CreateEventFromTemplateRequest(RegOnline.RegressionTest.Fixtures.API.EventService.AuthenticationHeader AuthenticationHeader, int sourceEventID, int destinationCustomerID, RegOnline.RegressionTest.Fixtures.API.EventService.EventBasics eventBasics) {
            this.AuthenticationHeader = AuthenticationHeader;
            this.sourceEventID = sourceEventID;
            this.destinationCustomerID = destinationCustomerID;
            this.eventBasics = eventBasics;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="CreateEventFromTemplateResponse", WrapperNamespace="https://www.regonline.com/webservices/", IsWrapped=true)]
    public partial class CreateEventFromTemplateResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="https://www.regonline.com/webservices/", Order=0)]
        public RegOnline.RegressionTest.Fixtures.API.EventService.RegOnlineResponseOfInt32 CreateEventFromTemplateResult;
        
        public CreateEventFromTemplateResponse() {
        }
        
        public CreateEventFromTemplateResponse(RegOnline.RegressionTest.Fixtures.API.EventService.RegOnlineResponseOfInt32 CreateEventFromTemplateResult) {
            this.CreateEventFromTemplateResult = CreateEventFromTemplateResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="UpdateEvent", WrapperNamespace="https://www.regonline.com/webservices/", IsWrapped=true)]
    public partial class UpdateEventRequest {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://www.regonline.com/webservices/")]
        public RegOnline.RegressionTest.Fixtures.API.EventService.AuthenticationHeader AuthenticationHeader;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="https://www.regonline.com/webservices/", Order=0)]
        public RegOnline.RegressionTest.Fixtures.API.EventService.EventBasics eventBasics;
        
        public UpdateEventRequest() {
        }
        
        public UpdateEventRequest(RegOnline.RegressionTest.Fixtures.API.EventService.AuthenticationHeader AuthenticationHeader, RegOnline.RegressionTest.Fixtures.API.EventService.EventBasics eventBasics) {
            this.AuthenticationHeader = AuthenticationHeader;
            this.eventBasics = eventBasics;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="UpdateEventResponse", WrapperNamespace="https://www.regonline.com/webservices/", IsWrapped=true)]
    public partial class UpdateEventResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="https://www.regonline.com/webservices/", Order=0)]
        public RegOnline.RegressionTest.Fixtures.API.EventService.RegOnlineResponseOfBoolean UpdateEventResult;
        
        public UpdateEventResponse() {
        }
        
        public UpdateEventResponse(RegOnline.RegressionTest.Fixtures.API.EventService.RegOnlineResponseOfBoolean UpdateEventResult) {
            this.UpdateEventResult = UpdateEventResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface EventServiceSoapChannel : RegOnline.RegressionTest.Fixtures.API.EventService.EventServiceSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class EventServiceSoapClient : System.ServiceModel.ClientBase<RegOnline.RegressionTest.Fixtures.API.EventService.EventServiceSoap>, RegOnline.RegressionTest.Fixtures.API.EventService.EventServiceSoap {
        
        public EventServiceSoapClient() {
        }
        
        public EventServiceSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public EventServiceSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public EventServiceSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public EventServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        RegOnline.RegressionTest.Fixtures.API.EventService.CreateEventFromTemplateResponse RegOnline.RegressionTest.Fixtures.API.EventService.EventServiceSoap.CreateEventFromTemplate(RegOnline.RegressionTest.Fixtures.API.EventService.CreateEventFromTemplateRequest request) {
            return base.Channel.CreateEventFromTemplate(request);
        }
        
        public RegOnline.RegressionTest.Fixtures.API.EventService.RegOnlineResponseOfInt32 CreateEventFromTemplate(RegOnline.RegressionTest.Fixtures.API.EventService.AuthenticationHeader AuthenticationHeader, int sourceEventID, int destinationCustomerID, RegOnline.RegressionTest.Fixtures.API.EventService.EventBasics eventBasics) {
            RegOnline.RegressionTest.Fixtures.API.EventService.CreateEventFromTemplateRequest inValue = new RegOnline.RegressionTest.Fixtures.API.EventService.CreateEventFromTemplateRequest();
            inValue.AuthenticationHeader = AuthenticationHeader;
            inValue.sourceEventID = sourceEventID;
            inValue.destinationCustomerID = destinationCustomerID;
            inValue.eventBasics = eventBasics;
            RegOnline.RegressionTest.Fixtures.API.EventService.CreateEventFromTemplateResponse retVal = ((RegOnline.RegressionTest.Fixtures.API.EventService.EventServiceSoap)(this)).CreateEventFromTemplate(inValue);
            return retVal.CreateEventFromTemplateResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        RegOnline.RegressionTest.Fixtures.API.EventService.UpdateEventResponse RegOnline.RegressionTest.Fixtures.API.EventService.EventServiceSoap.UpdateEvent(RegOnline.RegressionTest.Fixtures.API.EventService.UpdateEventRequest request) {
            return base.Channel.UpdateEvent(request);
        }
        
        public RegOnline.RegressionTest.Fixtures.API.EventService.RegOnlineResponseOfBoolean UpdateEvent(RegOnline.RegressionTest.Fixtures.API.EventService.AuthenticationHeader AuthenticationHeader, RegOnline.RegressionTest.Fixtures.API.EventService.EventBasics eventBasics) {
            RegOnline.RegressionTest.Fixtures.API.EventService.UpdateEventRequest inValue = new RegOnline.RegressionTest.Fixtures.API.EventService.UpdateEventRequest();
            inValue.AuthenticationHeader = AuthenticationHeader;
            inValue.eventBasics = eventBasics;
            RegOnline.RegressionTest.Fixtures.API.EventService.UpdateEventResponse retVal = ((RegOnline.RegressionTest.Fixtures.API.EventService.EventServiceSoap)(this)).UpdateEvent(inValue);
            return retVal.UpdateEventResult;
        }
    }
}
