﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RegOnline.RegressionTest.Fixtures.API.RegOnlineRetrieveSingleRegService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://www.regonline.com/webservices/", ConfigurationName="RegOnlineRetrieveSingleRegService.RetrieveSingleRegistrationManagerSoap")]
    public interface RetrieveSingleRegistrationManagerSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.regonline.com/webservices/RetrieveSingleRegistration", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string RetrieveSingleRegistration(string customerUserName, string customerPassword, int eventID, int registrationID);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface RetrieveSingleRegistrationManagerSoapChannel : RegOnline.RegressionTest.Fixtures.API.RegOnlineRetrieveSingleRegService.RetrieveSingleRegistrationManagerSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class RetrieveSingleRegistrationManagerSoapClient : System.ServiceModel.ClientBase<RegOnline.RegressionTest.Fixtures.API.RegOnlineRetrieveSingleRegService.RetrieveSingleRegistrationManagerSoap>, RegOnline.RegressionTest.Fixtures.API.RegOnlineRetrieveSingleRegService.RetrieveSingleRegistrationManagerSoap {
        
        public RetrieveSingleRegistrationManagerSoapClient() {
        }
        
        public RetrieveSingleRegistrationManagerSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public RetrieveSingleRegistrationManagerSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public RetrieveSingleRegistrationManagerSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public RetrieveSingleRegistrationManagerSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string RetrieveSingleRegistration(string customerUserName, string customerPassword, int eventID, int registrationID) {
            return base.Channel.RetrieveSingleRegistration(customerUserName, customerPassword, eventID, registrationID);
        }
    }
}
