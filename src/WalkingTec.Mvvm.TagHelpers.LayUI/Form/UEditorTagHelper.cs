using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WalkingTec.Mvvm.TagHelpers.LayUI.Form
{
    [HtmlTargetElement("wt:ueditor", Attributes = REQUIRED_ATTR_NAME, TagStructure = TagStructure.WithoutEndTag)]
    public class UEditorTagHelper : BaseFieldTag
    {
        //文本框为空显示的PlaceHolder
        public string EmptyText { get; set; }

        //定义高度
        public new int? Height { get; set; }

        //定义宽度
        public new int? Width { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string placeHolder = EmptyText ?? "";
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            string StrWidth = Width == null ? "100%" : (Width + "px");
            string StrHeight = Height == null ? "200px" : (Height + "px");
            output.Attributes.Add("style", $"width:{StrWidth};height:{StrHeight};");
            output.Attributes.Add("isrich", "1");
            string Content = @"<script>UE.loadEditor('" + Id + "').ready(function(){this.setContent('" + DefaultValue != null ? DefaultValue.ToString() : Field?.Model?.ToString() + "')});</script>";
            output.PostElement.AppendHtml(Content);
            base.Process(context, output);
        }
    }
}
