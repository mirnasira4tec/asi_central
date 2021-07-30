using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace asi.asicentral.web.Models.forms
{
    public class FormQuestions
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [AllowHtml]
        public string Description { get; set; }
        public string ShortDescription { get; set; } = string.Empty;
        public bool IsRequired { get; set; } = true;
        public bool IsFollowUpQuestion { get; set; } = false;
        public string FollowingUpQuestions { get; set; }
        public int Sequence { get; set; }
        public string Answer { get; set; } = string.Empty;
        public bool IsYesNoQuestion { get; set; } = false;
        public List<Option> OptionValues { get; set; }
        public InputControl InputType { get; set; } = InputControl.Text;
        public ValidationType ValidationType { get; set; } = ValidationType.None;
        public FollowUpType FollowUpType { get; set; } = FollowUpType.None;
        public List<SelectListItem> Dropdown { get; set; }
        public string PlaceHolder { get; set; } = string.Empty;
        public string CssClass { get; set; }
        public bool IsSelected { get; set; } = false;
    }
    public class Option
    {
        public string Value { get; set; }
        public bool IsSelected { get; set; }
        public string Text { get; set; }
    }
    public enum InputControl
    {
        None,
        Text,
        Radio,
        DropDown,
        CheckBoxlist,
        Header,
        TextArea,
        CheckBox
    }
    public enum ValidationType
    {
        None,
        Email,
        Phone
    }
    public enum FollowUpType
    {
        None,
        FollowUpOnYes,
        FollowUpOnNo
    }
}