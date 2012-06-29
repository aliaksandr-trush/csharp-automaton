﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.261
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RegOnline.RegressionTest.Fixtures.API.RegOnlineCFResponseService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="RegOnlineCFResponseService.CustomFieldResponseWSSoap")]
    public interface CustomFieldResponseWSSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Modify", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        void Modify(string customerUserName, string customerPassword, int cfId, int registrationId, int eventId, string dnaCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/AssignSeat", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        void AssignSeat(string customerUserName, string customerPassword, int cfId, int registrationId, int eventId, string levelId, string sectionId, string rowId, short seatId, string blockCode);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface CustomFieldResponseWSSoapChannel : RegOnline.RegressionTest.Fixtures.API.RegOnlineCFResponseService.CustomFieldResponseWSSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CustomFieldResponseWSSoapClient : System.ServiceModel.ClientBase<RegOnline.RegressionTest.Fixtures.API.RegOnlineCFResponseService.CustomFieldResponseWSSoap>, RegOnline.RegressionTest.Fixtures.API.RegOnlineCFResponseService.CustomFieldResponseWSSoap {
        
        public CustomFieldResponseWSSoapClient() {
        }
        
        public CustomFieldResponseWSSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public CustomFieldResponseWSSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CustomFieldResponseWSSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CustomFieldResponseWSSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void Modify(string customerUserName, string customerPassword, int cfId, int registrationId, int eventId, string dnaCode) {
            base.Channel.Modify(customerUserName, customerPassword, cfId, registrationId, eventId, dnaCode);
        }
        
        public void AssignSeat(string customerUserName, string customerPassword, int cfId, int registrationId, int eventId, string levelId, string sectionId, string rowId, short seatId, string blockCode) {
            base.Channel.AssignSeat(customerUserName, customerPassword, cfId, registrationId, eventId, levelId, sectionId, rowId, seatId, blockCode);
        }
    }
}
