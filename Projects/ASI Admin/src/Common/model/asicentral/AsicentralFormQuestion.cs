using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.asicentral
{
    public class AsicentralFormQuestion
    {
        public int Id { get; set; }
        public int FormTypeId { get; set; }
        public int Sequence { get; set; }
        public string Name { get; set; }
        public string InputType { get; set; }
        public string PlaceHolder { get; set; }
        public string Description { get; set; }
        public int? ParentQuestionId { get; set; }
        public string FollowingUpQuestions { get; set; }
        public bool IsRequired { get; set; }
        public bool IsVisible { get; set; }
        public string ValidationRule { get; set; }
        public string ValidationMessage { get; set; }
        public string CssStyle { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
        virtual public AsicentralFormType FormType { get; set; }
        virtual public List<AsicentralFormQuestionOption> QuestionOptions { get; set; }
    }
}
