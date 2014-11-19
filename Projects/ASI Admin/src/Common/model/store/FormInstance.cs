using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class FormInstance
    {
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

		[Display(Description = "User Email", Prompt = "The user email")]
		[Required(ErrorMessage = "The user email is required")]
		[DataType(DataType.EmailAddress)]
        public string Email { get; set; }
		[Display(Description = "Salutation", Prompt = "How to address the user")]
		[Required(ErrorMessage = "You need a way to address the user")]
		public string Salutation { get; set; }

		[Display(Description = "Greetings", Prompt = "A small introduction to the order")]
		[Required(ErrorMessage = "You need a greeting message for the email")]
		[DataType(DataType.MultilineText)]
        public string Greetings { get; set; }
        public string ExternalReference { get; set; }

		[Display(Description = "Order Total", Prompt = "amount")]
		[Required(ErrorMessage = "You need to specify the order amount")]
		[DataType(DataType.Currency)]
		public decimal Total { get; set; }
		public virtual StoreOrderDetail OrderDetail { get; set; }
		public int? OrderDetailId { get; set; }

		[Display(Description = "Comments", Prompt = "Internal comments about the order")]
		[DataType(DataType.MultilineText)]
		public string Comments { get; set; }

		public string Sender { get; set; }
		public virtual IList<FormValue> Values { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
    }
}
