using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.show.form
{
    public class SHW_FormQuestion
    {
        public int Id { get; set; }
        public int FormTypeId { get; set; }
        public int Sequence { get; set; }
        public string QuestionName { get; set; }
        public string InputType { get; set; }
        public string Category { get; set; }
        public string PlaceHolder { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public string CssStyle { get; set; }
        public int? ParentQuestionId { get; set; }
        public string FollowingUpQuestions { get; set; }
        public bool IsYesNoQuestion  { get; set; }
        public bool IsRequired  { get; set; }
        public bool IsVisible  { get; set; }
        public string ValidationRule { get; set; }
        public string ValidationMessage { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }

        virtual public SHW_FormType FormType { get; set; }
        virtual public List<SHW_FormQuestionOption> QuestionOptions { get;set;}
    }
}
