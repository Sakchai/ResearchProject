using System.Collections.Generic;
using FluentValidation.Attributes;

namespace Research.Web.Models.Messages
{
  //  [Validator(typeof(TestMessageTemplateValidator))]
    public partial class TestMessageTemplateModel : BaseEntityModel
    {
        public TestMessageTemplateModel()
        {
            Tokens = new List<string>();
        }

        public int LanguageId { get; set; }

        public List<string> Tokens { get; set; }

        public string SendTo { get; set; }
    }
}