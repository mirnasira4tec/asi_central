﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace asi.asicentral.web.CreditCardService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CreditCard", Namespace="http://schemas.datacontract.org/2004/07/asi.asicentral.CreditCardVolt.model")]
    [System.SerializableAttribute()]
    public partial class CreditCard : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AddressField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CardHolderNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CityField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CountryField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CountryCodeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime ExpirationDateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MaskedPANField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NumberField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PostalCodeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<long> TokenField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TypeField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Address {
            get {
                return this.AddressField;
            }
            set {
                if ((object.ReferenceEquals(this.AddressField, value) != true)) {
                    this.AddressField = value;
                    this.RaisePropertyChanged("Address");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CardHolderName {
            get {
                return this.CardHolderNameField;
            }
            set {
                if ((object.ReferenceEquals(this.CardHolderNameField, value) != true)) {
                    this.CardHolderNameField = value;
                    this.RaisePropertyChanged("CardHolderName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string City {
            get {
                return this.CityField;
            }
            set {
                if ((object.ReferenceEquals(this.CityField, value) != true)) {
                    this.CityField = value;
                    this.RaisePropertyChanged("City");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Country {
            get {
                return this.CountryField;
            }
            set {
                if ((object.ReferenceEquals(this.CountryField, value) != true)) {
                    this.CountryField = value;
                    this.RaisePropertyChanged("Country");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CountryCode {
            get {
                return this.CountryCodeField;
            }
            set {
                if ((object.ReferenceEquals(this.CountryCodeField, value) != true)) {
                    this.CountryCodeField = value;
                    this.RaisePropertyChanged("CountryCode");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime ExpirationDate {
            get {
                return this.ExpirationDateField;
            }
            set {
                if ((this.ExpirationDateField.Equals(value) != true)) {
                    this.ExpirationDateField = value;
                    this.RaisePropertyChanged("ExpirationDate");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid Id {
            get {
                return this.IdField;
            }
            set {
                if ((this.IdField.Equals(value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string MaskedPAN {
            get {
                return this.MaskedPANField;
            }
            set {
                if ((object.ReferenceEquals(this.MaskedPANField, value) != true)) {
                    this.MaskedPANField = value;
                    this.RaisePropertyChanged("MaskedPAN");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Number {
            get {
                return this.NumberField;
            }
            set {
                if ((object.ReferenceEquals(this.NumberField, value) != true)) {
                    this.NumberField = value;
                    this.RaisePropertyChanged("Number");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PostalCode {
            get {
                return this.PostalCodeField;
            }
            set {
                if ((object.ReferenceEquals(this.PostalCodeField, value) != true)) {
                    this.PostalCodeField = value;
                    this.RaisePropertyChanged("PostalCode");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string State {
            get {
                return this.StateField;
            }
            set {
                if ((object.ReferenceEquals(this.StateField, value) != true)) {
                    this.StateField = value;
                    this.RaisePropertyChanged("State");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<long> Token {
            get {
                return this.TokenField;
            }
            set {
                if ((this.TokenField.Equals(value) != true)) {
                    this.TokenField = value;
                    this.RaisePropertyChanged("Token");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Type {
            get {
                return this.TypeField;
            }
            set {
                if ((object.ReferenceEquals(this.TypeField, value) != true)) {
                    this.TypeField = value;
                    this.RaisePropertyChanged("Type");
                }
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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="web.CreditCardService.ICreditCardService")]
    public interface ICreditCardService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICreditCardService/Ping", ReplyAction="http://tempuri.org/ICreditCardService/PingResponse")]
        string Ping();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICreditCardService/Ping", ReplyAction="http://tempuri.org/ICreditCardService/PingResponse")]
        System.Threading.Tasks.Task<string> PingAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICreditCardService/Store", ReplyAction="http://tempuri.org/ICreditCardService/StoreResponse")]
        System.Guid Store(asi.asicentral.web.CreditCardService.CreditCard creditCard);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICreditCardService/Store", ReplyAction="http://tempuri.org/ICreditCardService/StoreResponse")]
        System.Threading.Tasks.Task<System.Guid> StoreAsync(asi.asicentral.web.CreditCardService.CreditCard creditCard);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICreditCardService/StoreCreditCard", ReplyAction="http://tempuri.org/ICreditCardService/StoreCreditCardResponse")]
        System.Guid StoreCreditCard(string asiCompany, asi.asicentral.web.CreditCardService.CreditCard creditCard);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICreditCardService/StoreCreditCard", ReplyAction="http://tempuri.org/ICreditCardService/StoreCreditCardResponse")]
        System.Threading.Tasks.Task<System.Guid> StoreCreditCardAsync(string asiCompany, asi.asicentral.web.CreditCardService.CreditCard creditCard);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICreditCardService/Delete", ReplyAction="http://tempuri.org/ICreditCardService/DeleteResponse")]
        void Delete(System.Guid id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICreditCardService/Delete", ReplyAction="http://tempuri.org/ICreditCardService/DeleteResponse")]
        System.Threading.Tasks.Task DeleteAsync(System.Guid id);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ICreditCardServiceChannel : asi.asicentral.web.CreditCardService.ICreditCardService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CreditCardServiceClient : System.ServiceModel.ClientBase<asi.asicentral.web.CreditCardService.ICreditCardService>, asi.asicentral.web.CreditCardService.ICreditCardService {
        
        public CreditCardServiceClient() {
        }
        
        public CreditCardServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public CreditCardServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CreditCardServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CreditCardServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string Ping() {
            return base.Channel.Ping();
        }
        
        public System.Threading.Tasks.Task<string> PingAsync() {
            return base.Channel.PingAsync();
        }
        
        public System.Guid Store(asi.asicentral.web.CreditCardService.CreditCard creditCard) {
            return base.Channel.Store(creditCard);
        }
        
        public System.Threading.Tasks.Task<System.Guid> StoreAsync(asi.asicentral.web.CreditCardService.CreditCard creditCard) {
            return base.Channel.StoreAsync(creditCard);
        }
        
        public System.Guid StoreCreditCard(string asiCompany, asi.asicentral.web.CreditCardService.CreditCard creditCard) {
            return base.Channel.StoreCreditCard(asiCompany, creditCard);
        }
        
        public System.Threading.Tasks.Task<System.Guid> StoreCreditCardAsync(string asiCompany, asi.asicentral.web.CreditCardService.CreditCard creditCard) {
            return base.Channel.StoreCreditCardAsync(asiCompany, creditCard);
        }
        
        public void Delete(System.Guid id) {
            base.Channel.Delete(id);
        }
        
        public System.Threading.Tasks.Task DeleteAsync(System.Guid id) {
            return base.Channel.DeleteAsync(id);
        }
    }
}
