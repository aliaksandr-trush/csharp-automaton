﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.261
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RegOnline.RegressionTest.Fixtures.API.RegOnlineGetEventRegistrationsService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="RegOnlineGetEventRegistrationsService.getEventRegistrationsSoap")]
    public interface getEventRegistrationsSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/RetrieveRegistrationInfo", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string RetrieveRegistrationInfo(string login, string password, int eventID);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface getEventRegistrationsSoapChannel : RegOnline.RegressionTest.Fixtures.API.RegOnlineGetEventRegistrationsService.getEventRegistrationsSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class getEventRegistrationsSoapClient : System.ServiceModel.ClientBase<RegOnline.RegressionTest.Fixtures.API.RegOnlineGetEventRegistrationsService.getEventRegistrationsSoap>, RegOnline.RegressionTest.Fixtures.API.RegOnlineGetEventRegistrationsService.getEventRegistrationsSoap {
        
        public getEventRegistrationsSoapClient() {
        }
        
        public getEventRegistrationsSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public getEventRegistrationsSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public getEventRegistrationsSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public getEventRegistrationsSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string RetrieveRegistrationInfo(string login, string password, int eventID) {
            return base.Channel.RetrieveRegistrationInfo(login, password, eventID);
        }
    }
}
