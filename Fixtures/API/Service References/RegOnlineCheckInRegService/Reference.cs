﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RegOnline.RegressionTest.Fixtures.API.RegOnlineCheckInRegService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="RegOnlineCheckInRegService.CheckInRegSoap")]
    public interface CheckInRegSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/CheckIn", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string CheckIn(int RegisterID, int Event_ID);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface CheckInRegSoapChannel : RegOnline.RegressionTest.Fixtures.API.RegOnlineCheckInRegService.CheckInRegSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CheckInRegSoapClient : System.ServiceModel.ClientBase<RegOnline.RegressionTest.Fixtures.API.RegOnlineCheckInRegService.CheckInRegSoap>, RegOnline.RegressionTest.Fixtures.API.RegOnlineCheckInRegService.CheckInRegSoap {
        
        public CheckInRegSoapClient() {
        }
        
        public CheckInRegSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public CheckInRegSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CheckInRegSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CheckInRegSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string CheckIn(int RegisterID, int Event_ID) {
            return base.Channel.CheckIn(RegisterID, Event_ID);
        }
    }
}
