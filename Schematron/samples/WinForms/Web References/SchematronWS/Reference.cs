﻿//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a tool.
//     Runtime Version: 1.1.4322.573
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 1.1.4322.573.
// 
namespace WinTest.SchematronWS {
    using System.Diagnostics;
    using System.Xml.Serialization;
    using System;
    using System.Web.Services.Protocols;
    using System.ComponentModel;
    using System.Web.Services;
    
    
    /// <remarks/>
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="ValidatedWSSoap", Namespace="http://aspnet2.com/kzu")]
    public class ValidatedWS : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        /// <remarks/>
        public ValidatedWS() {
            this.Url = "http://localhost/SchematronWS/ValidatedWS.asmx";
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://aspnet2.com/kzu/BatchInsert", RequestNamespace="http://aspnet2.com/kzu", ResponseNamespace="http://aspnet2.com/kzu", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void BatchInsert(System.Xml.XmlNode orders) {
            this.Invoke("BatchInsert", new object[] {
                        orders});
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginBatchInsert(System.Xml.XmlNode orders, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("BatchInsert", new object[] {
                        orders}, callback, asyncState);
        }
        
        /// <remarks/>
        public void EndBatchInsert(System.IAsyncResult asyncResult) {
            this.EndInvoke(asyncResult);
        }
    }
}
