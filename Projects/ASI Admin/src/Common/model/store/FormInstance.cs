﻿using asi.asicentral.interfaces;
using asi.asicentral.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace asi.asicentral.model.store
{
    public class FormInstance
    {
        private string _status = null;

        public FormInstance()
        {
            if (this.GetType() == typeof(FormInstance))
            {
                Values = new List<FormValue>();
            }
        }

        public int Id { get; set; }
        public virtual FormType FormType { get; set; }
        public int FormTypeId { get; set; }

        [Display(Name = "Customer Email", Prompt = "Customer email the form is sent to")]
        [Required(ErrorMessage = "The customer email is required")]
        [DataType(DataType.EmailAddress)]
        [StringLength(500, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldLength")]
        public string Email { get; set; }

        [Display(Name = "Internal Notification Emails", Prompt = "The people to notify when order is through")]
        [StringLength(500, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldLength")]
        public string NotificationEmails { get; set; }

        [Display(Name = "Salutation", Prompt = "How to address the user")]
        [Required(ErrorMessage = "You need a way to address the user")]
        [StringLength(200, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldLength")]
        public string Salutation { get; set; }

        [Display(Name = "Greetings", Prompt = "A small introduction to the order")]
        [Required(ErrorMessage = "You need a greeting message for the email")]
        [DataType(DataType.MultilineText)]
        [StringLength(10000, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldLength")]
        public string Greetings { get; set; }

        [StringLength(150, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldLength")]
        public string ExternalReference { get; set; }

        [Display(Name = "Payment Due Now", Prompt = "amount")]
        [DataType(DataType.Currency)]
        public decimal? InitialPayment { get; set; }

        [Display(Name = "Order Total", Prompt = "amount")]
        [Required(ErrorMessage = "You need to specify the order amount")]
        [DataType(DataType.Currency)]
        public decimal Total { get; set; }

        public virtual StoreOrderDetail OrderDetail { get; set; }
        public int? OrderDetailId { get; set; }

        [Display(Name = "Comments", Prompt = "Internal comments about the order")]
        [DataType(DataType.MultilineText)]
        [StringLength(10000, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldLength")]
        public string Comments { get; set; }

        [StringLength(200, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldLength")]
        public string Sender { get; set; }

        public virtual IList<FormValue> Values { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
        public string Status
        {
            get
            {
                if (_status == null)
                {
                    _status = Id > 0 ? "Not Started" : "New";
                    if (OrderDetail != null && OrderDetail.Order != null)
                    {
                        if (OrderDetail.Order.ProcessStatus == asi.asicentral.model.store.OrderStatus.Approved)
                        {
                            _status = "Approved";
                        }
                        else if (OrderDetail.Order.ProcessStatus == asi.asicentral.model.store.OrderStatus.Rejected)
                        {
                            _status = "Rejected";
                        }
                        else if (OrderDetail.Order.IsCompleted)
                        {
                            _status = "Purchased";
                        }
                        else
                        {
                            _status = "In Progress";
                        }
                    }
                    else
                    {
                        if (this.FormType != null && string.IsNullOrWhiteSpace(this.FormType.Implementation))
                        {
                            _status = "Approved";
                        }
                    }
                }
                return _status;
            }

            set
            {
                _status = value;
            }
        }

        public void Copy(FormInstance instance, string updateSource)
        {
            //copying the fields values
            Comments = instance.Comments;
            Email = instance.Email;
            Greetings = instance.Greetings;
            NotificationEmails = instance.NotificationEmails;
            Salutation = instance.Salutation;
            InitialPayment = instance.InitialPayment;
            Total = instance.Total;
            UpdateDate = DateTime.UtcNow;
            UpdateSource = updateSource;
            if (OrderDetail != null)
            {
                OrderDetail.Cost = instance.Total;
                if (OrderDetail.Order != null)
                {
                    InitialPayment = instance.InitialPayment;
                    OrderDetail.Order.Total = instance.Total;
                    OrderDetail.Order.AnnualizedTotal = instance.Total;
                }
            }
            //copying the form values
            Values = Values.OrderBy(value => value.Sequence).ToList();
            for (int i = 0; i < Values.Count; i++)
            {
                Values[i].Value = instance.Values[i].Value;
                Values[i].UpdateDate = DateTime.UtcNow;
                Values[i].UpdateSource = updateSource;
            }
        }

        public StoreOrder CreateOrder(IStoreService storeService)
        {
            StoreOrder value = null;

            if (OrderDetail == null)
            {
                var order = new StoreOrder
                {
                    IsCompleted = false,
                    ContextId = FormType.ContextId,
                    OrderRequestType = FormType.RequestType,
                    ProcessStatus = OrderStatus.Pending,
                    IsStoreRequest = false,
                    LoggedUserEmail = Email.ToLower(),
                    UserReference = Guid.NewGuid().ToString(),
                    Campaign = FormType.Name,
                    InitialPayment = InitialPayment.HasValue && InitialPayment.Value > 0 ? InitialPayment.Value : (decimal?)null,
                    Total = Total,
                    AnnualizedTotal = Total,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "FormInstance-CreateOrder",
                };
                //add order detail record
                var product = storeService.GetAll<ContextProduct>(false).SingleOrDefault(prod => prod.Id == FormType.ProductIdentifier);
                if (product == null) throw new Exception("The Form product cannot be found in the database");
                var orderDetail = new StoreOrderDetail
                {
                    Product = product,
                    Quantity = 1,
                    Cost = Total,
                    ApplicationCost = 0,
                    IsSubscription = false,
                    TaxCost = 0,
                    ShippingCost = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "FormInstance-CreateOrder",
                };
                order.OrderDetails.Add(orderDetail);
                OrderDetail = orderDetail;
                value = order;
            }
            else
            {
                value = OrderDetail.Order;
            }
            return value;
        }
    }
}
