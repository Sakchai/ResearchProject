using System;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Research.Web.Framework.TagHelpers.Shared
{
    /// <summary>
    /// nop-required tag helper
    /// </summary>
    [HtmlTargetElement("nop-required", TagStructure = TagStructure.WithoutEndTag)]
    public class ResearchRequiredTagHelper : TagHelper
    {
        /// <summary>
        /// Process
        /// </summary>
        /// <param name="context">Context</param>
        /// <param name="output">Output</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            //clear the output
            output.SuppressOutput();

            output.TagName = "span";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("class", "required");
            output.Content.SetContent("*");
        }
    }
}