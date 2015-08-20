using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CimscoPortal
{
    public static class ButtonExtension
    {
        public static HtmlString Add(this HtmlHelper htmlHelper, string url = "", bool right = false, bool isBtn = true, object htmlAttribute = null, string htmlText = "Add")
        {
            TagBuilder tag = new TagBuilder("a");
            if (isBtn)
            {
                tag.AddCssClass("btn btn-default");
                if (right)
                {
                    tag.MergeAttribute("href", url);
                    tag.MergeAttribute("data-original-title", "Add");
                    tag.MergeAttribute("title", "Add");
                }
                else
                {
                    tag.MergeAttribute("data-original-title", "No access");
                    tag.MergeAttribute("title", "No access");
                    tag.AddCssClass("not-allowed");
                }
                HtmlAttributeMerge(tag, htmlAttribute);
                tag.InnerHtml = htmlText;
            }
            else
            {
                tag.AddCssClass("custom-action-button tooltips");
                if (right)
                {
                    tag.MergeAttribute("href", url);
                    tag.MergeAttribute("data-original-title", "Add");
                    tag.MergeAttribute("title", "Edit");
                }
                else
                {
                    tag.MergeAttribute("data-original-title", "No access");
                    tag.MergeAttribute("title", "No access");
                    tag.AddCssClass("not-allowed");
                }
                HtmlAttributeMerge(tag, htmlAttribute);
                tag.InnerHtml = "<span class='fa fa-plus'></span>";
            }
            return MvcHtmlString.Create(tag.ToString());

        }
        public static HtmlString Edit(this HtmlHelper htmlHelper, string url = "", bool right = false, object htmlAttribute = null, string text = "Edit")
        {
            TagBuilder tag = new TagBuilder("a");
            tag.AddCssClass("btn btn-info btn-xs");
            if (right)
            {
                tag.MergeAttribute("href", url);
                tag.MergeAttribute("data-original-title", "Edit");
                tag.MergeAttribute("title", "Edit");
            }
            else
            {
                return MvcHtmlString.Create(string.Empty);
            }
            HtmlAttributeMerge(tag, htmlAttribute);
            tag.InnerHtml = text;
            return MvcHtmlString.Create(tag.ToString());

        }
        public static HtmlString Delete(this HtmlHelper htmlHelper, string url = "", bool right = false, object htmlAttribute = null, string text = "Delete", string message = "Are you sure want to Delete?")
        {
            TagBuilder tag = new TagBuilder("a");
            tag.AddCssClass("btn btn-danger btn-xs");
            if (right)
            {
                tag.MergeAttribute("href", url);
                tag.MergeAttribute("data-original-title", "Delete");
                tag.MergeAttribute("title", "Delete");
            }
            else
            {
                return MvcHtmlString.Create(string.Empty);
            }
            tag.MergeAttribute("data-message", message);
            HtmlAttributeMerge(tag, htmlAttribute);
            tag.InnerHtml = text;
            return MvcHtmlString.Create(tag.ToString());

        }
        public static void HtmlAttributeMerge(TagBuilder tag, object htmlAttribute)
        {
            if (htmlAttribute != null)
            {
                foreach (var prop in htmlAttribute.GetType().GetProperties())
                {
                    if (prop.Name != "class")
                        tag.MergeAttribute(prop.Name.Replace("_", "-"), Convert.ToString(prop.GetValue(htmlAttribute)));
                    else
                        tag.AddCssClass(Convert.ToString(prop.GetValue(htmlAttribute)));
                }
            }
        }
    }
}