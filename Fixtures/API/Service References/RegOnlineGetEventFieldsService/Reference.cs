﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RegOnline.RegressionTest.Fixtures.API.RegOnlineGetEventFieldsService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="RegOnlineGetEventFieldsService.getEventFieldsSoap")]
    public interface getEventFieldsSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/RetrieveEventFields", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string RetrieveEventFields(string login, string password, int eventID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/RetrieveEventFields2", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string RetrieveEventFields2(string login, string password, int eventID, bool excludeAmounts);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface getEventFieldsSoapChannel : RegOnline.RegressionTest.Fixtures.API.RegOnlineGetEventFieldsService.getEventFieldsSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class getEventFieldsSoapClient : System.ServiceModel.ClientBase<RegOnline.RegressionTest.Fixtures.API.RegOnlineGetEventFieldsService.getEventFieldsSoap>, RegOnline.RegressionTest.Fixtures.API.RegOnlineGetEventFieldsService.getEventFieldsSoap {
        
        public getEventFieldsSoapClient() {
        }
        
        public getEventFieldsSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public getEventFieldsSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public getEventFieldsSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public getEventFieldsSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string RetrieveEventFields(string login, string password, int eventID) {
            return base.Channel.RetrieveEventFields(login, password, eventID);
        }
        
        public string RetrieveEventFields2(string login, string password, int eventID, bool excludeAmounts) {
            return base.Channel.RetrieveEventFields2(login, password, eventID, excludeAmounts);
        }
    }
}
