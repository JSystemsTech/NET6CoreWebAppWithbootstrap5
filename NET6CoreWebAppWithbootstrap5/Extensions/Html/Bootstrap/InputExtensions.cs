using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace NET6CoreWebAppWithbootstrap5.Extensions.Html.Bootstrap
{
    public static class InputExtensions
    {

        private static IDictionary<string, object?> AppendFormControlClass(this IDictionary<string, object?> htmlAttributes)
            => htmlAttributes.AppendClass("form-control");


        public static IHtmlContent FormControlTextBox(this IHtmlHelper htmlHelper, string name, object value, string format, IDictionary<string, object?> htmlAttributes)
            => htmlHelper.TextBox(name, value, format, htmlAttributes.AppendFormControlClass());
        public static IHtmlContent FormControlTextBox(this IHtmlHelper htmlHelper, string name, object value, IDictionary<string, object?> htmlAttributes)
            => htmlHelper.TextBox(name, value, htmlAttributes.AppendFormControlClass());
        public static IHtmlContent FormControlTextBox(this IHtmlHelper htmlHelper, string name, object value, string format, object htmlAttributes)
            => htmlHelper.FormControlTextBox(name, value, format, htmlAttributes.ToHtmlAttributesDictionary());
        public static IHtmlContent FormControlTextBox(this IHtmlHelper htmlHelper, string name, object value)
            => htmlHelper.FormControlTextBox(name, value, new { });
        public static IHtmlContent FormControlTextBox(this IHtmlHelper htmlHelper, string name, object value, string format)
            => htmlHelper.FormControlTextBox(name, value, format, new { });
        public static IHtmlContent FormControlTextBox(this IHtmlHelper htmlHelper, string name)
            => htmlHelper.FormControlTextBox(name, "");

        public static IHtmlContent FormControlTextBox(this IHtmlHelper htmlHelper, string name, object value, object htmlAttributes)
            => htmlHelper.FormControlTextBox(name, value, htmlAttributes.ToHtmlAttributesDictionary());
        public static IHtmlContent FormControlTextBoxFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format, object htmlAttributes)
            => htmlHelper.FormControlTextBoxFor(expression, format, htmlAttributes.ToHtmlAttributesDictionary());
        public static IHtmlContent FormControlTextBoxFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
            => htmlHelper.FormControlTextBoxFor(expression, htmlAttributes.ToHtmlAttributesDictionary());
        public static IHtmlContent FormControlTextBoxFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object?> htmlAttributes)
            => htmlHelper.TextBoxFor(expression, htmlHelper.HasValidationError(expression) ? htmlAttributes.AppendFormControlClass().AppendClass("is-invalid") : htmlAttributes.AppendFormControlClass());
        public static IHtmlContent FormControlTextBoxFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
            => htmlHelper.FormControlTextBoxFor(expression, new { });
        public static IHtmlContent FormControlTextBoxFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format)
            => htmlHelper.FormControlTextBoxFor(expression, format, new { });
        public static IHtmlContent FormControlTextBoxFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format, IDictionary<string, object?> htmlAttributes)
            => htmlHelper.TextBoxFor(expression, format, htmlHelper.HasValidationError(expression) ? htmlAttributes.AppendFormControlClass().AppendClass("is-invalid") : htmlAttributes.AppendFormControlClass());


        public static IHtmlContent FormControlTextAreaFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
            => htmlHelper.FormControlTextAreaFor(expression, new { });
        public static IHtmlContent FormControlTextAreaFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
            => htmlHelper.FormControlTextAreaFor(expression, htmlAttributes.ToHtmlAttributesDictionary());
        public static IHtmlContent FormControlTextAreaFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object?> htmlAttributes)
        => htmlHelper.TextAreaFor(expression, htmlHelper.HasValidationError(expression) ? htmlAttributes.AppendFormControlClass().AppendClass("is-invalid") : htmlAttributes.AppendFormControlClass());
        public static IHtmlContent FormControlTextAreaFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, int rows, int columns)
            => htmlHelper.FormControlTextAreaFor(expression, rows, columns, new { });
        public static IHtmlContent FormControlTextAreaFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, int rows, int columns, object htmlAttributes)
            => htmlHelper.FormControlTextAreaFor(expression, rows,columns, htmlAttributes.ToHtmlAttributesDictionary());
        public static IHtmlContent FormControlTextAreaFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, int rows, int columns, IDictionary<string, object?> htmlAttributes)
            => htmlHelper.TextAreaFor(expression, rows, columns, htmlHelper.HasValidationError(expression) ? htmlAttributes.AppendFormControlClass().AppendClass("is-invalid") : htmlAttributes.AppendFormControlClass());


        public static IHtmlContent FormControlDateFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format, object htmlAttributes)
            => htmlHelper.FormControlDateFor(expression, format, htmlAttributes.ToHtmlAttributesDictionary());
        public static IHtmlContent FormControlDateFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
            => htmlHelper.FormControlDateFor(expression, htmlAttributes.ToHtmlAttributesDictionary());
        public static IHtmlContent FormControlDateFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object?> htmlAttributes)
            => htmlHelper.TextBoxFor(expression, "{0:yyyy-MM-dd}", htmlHelper.HasValidationError(expression) ? htmlAttributes.AppendFormControlClass().AppendClass("is-invalid").AppendHtmlAttribute("type", "date") : htmlAttributes.AppendFormControlClass().AppendHtmlAttribute("type", "date"));
        public static IHtmlContent FormControlDateFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
            => htmlHelper.FormControlTextBoxFor(expression, new { });
        public static IHtmlContent FormControlDateFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format)
            => htmlHelper.FormControlTextBoxFor(expression, format, new { });
        public static IHtmlContent FormControlDateFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format, IDictionary<string, object?> htmlAttributes)
            => htmlHelper.TextBoxFor(expression, format, htmlHelper.HasValidationError(expression) ? htmlAttributes.AppendFormControlClass().AppendClass("is-invalid").AppendHtmlAttribute("type","date") : htmlAttributes.AppendFormControlClass().AppendHtmlAttribute("type", "date"));




        public static IHtmlContent FormControlValidationMessageFor<TModel, TResult>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TResult>> expression, string? message, object htmlAttributes)
        => htmlHelper.ValidationMessageFor(expression, message, htmlAttributes.ToHtmlAttributesDictionary().AppendClass("invalid-feedback"));
        public static IHtmlContent FormControlValidationMessageFor<TModel, TResult>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TResult>> expression, string? message)
        => htmlHelper.FormControlValidationMessageFor(expression, message, new { });
        public static IHtmlContent FormControlValidationMessageFor<TModel, TResult>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TResult>> expression)
        => htmlHelper.FormControlValidationMessageFor(expression, htmlHelper.ErrorMessageFor(expression));

        private static IHtmlContent FormControlTextBoxForBase<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object?> htmlAttributes)
        {
            var metaData = htmlHelper.MetaDataFor(expression);

            return htmlHelper.TextBoxFor(expression, htmlAttributes.AppendFormControlClass());
        }
        private static IHtmlContent FormControlTextBoxForBase<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format, IDictionary<string, object?> htmlAttributes)
        {
            var metadata = htmlHelper.MetaDataFor(expression);

            return htmlHelper.TextBoxFor(expression, format, htmlAttributes.AppendFormControlClass());
        }



        public static IHtmlContent FormControlLabelFor<TModel, TResult>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TResult>> expression)
        => htmlHelper.FormControlLabelFor(expression, new { });
        public static IHtmlContent FormControlLabelFor<TModel, TResult>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TResult>> expression, object htmlAttributes)
        {
            var metadata = htmlHelper.MetaDataFor(expression);
                
            string extraClasses = metadata.IsRequired ? "form-label required" : "form-label";
            return htmlHelper.LabelFor(expression, htmlAttributes.ToHtmlAttributesDictionary().AppendClass(extraClasses));
        }


        public static IHtmlContent FormControlPasswordFor(this IHtmlHelper htmlHelper, string name, object value, object htmlAttributes)
            => htmlHelper.FormControlPasswordFor(name, value, htmlAttributes.ToHtmlAttributesDictionary());
        public static IHtmlContent FormControlPasswordFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format, object htmlAttributes)
            => htmlHelper.FormControlPasswordFor(expression, format, htmlAttributes.ToHtmlAttributesDictionary());
        public static IHtmlContent FormControlPasswordFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
            => htmlHelper.FormControlPasswordFor(expression, htmlAttributes.ToHtmlAttributesDictionary());
        public static IHtmlContent FormControlPasswordFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object?> htmlAttributes)
        {
            htmlAttributes.Add("type", "password");
            return htmlHelper.FormControlTextBoxFor(expression, htmlAttributes);
        }
        public static IHtmlContent FormControlPasswordFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
            => htmlHelper.FormControlPasswordFor(expression, new { });
        public static IHtmlContent FormControlPasswordFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format)
            => htmlHelper.FormControlPasswordFor(expression, format, new { });
        public static IHtmlContent FormControlPasswordFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format, IDictionary<string, object?> htmlAttributes)
        {
            htmlAttributes.Add("type", "password");
            return htmlHelper.FormControlTextBoxFor(expression, format, htmlAttributes);
        }


         


        //public static IHtmlContent FormControlApplicationPasswordFor(this IHtmlHelper htmlHelper, string name, object value, object htmlAttributes)
        //    => htmlHelper.FormControlApplicationPasswordFor(name, value, htmlAttributes.ToHtmlAttributesDictionary());
        //public static IHtmlContent FormControlApplicationPasswordFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format, object htmlAttributes)
        //    => htmlHelper.FormControlApplicationPasswordFor(expression, format, htmlAttributes.ToHtmlAttributesDictionary());
        //public static IHtmlContent FormControlApplicationPasswordFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        //    => htmlHelper.FormControlApplicationPasswordFor(expression, htmlAttributes.ToHtmlAttributesDictionary());
        //public static IHtmlContent FormControlApplicationPasswordFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object?> htmlAttributes)
        //{
        //    IServiceProvider provider = htmlHelper.ViewContext.HttpContext.RequestServices;
        //    PasswordOptions passwordOptions = provider.GetService<IOptions<PasswordOptions>>().Value;
        //    htmlAttributes.Add("minlength", passwordOptions.MinLength);
        //    htmlAttributes.Add("maxlength", passwordOptions.MaxLength);
        //    return htmlHelper.FormControlPasswordFor(expression, htmlAttributes);
        //}
        //public static IHtmlContent FormControlApplicationPasswordFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        //    => htmlHelper.FormControlApplicationPasswordFor(expression, new { });
        //public static IHtmlContent FormControlApplicationPasswordFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format)
        //    => htmlHelper.FormControlApplicationPasswordFor(expression, format, new { });
        //public static IHtmlContent FormControlApplicationPasswordFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format, IDictionary<string, object?> htmlAttributes)
        //{
        //    IServiceProvider provider = htmlHelper.ViewContext.HttpContext.RequestServices;
        //    PasswordOptions passwordOptions = provider.GetService<IOptions<PasswordOptions>>().Value;
        //    htmlAttributes.Add("minlength", passwordOptions.MinLength);
        //    htmlAttributes.Add("maxlength", passwordOptions.MaxLength);
        //    return htmlHelper.FormControlPasswordFor(expression, format, htmlAttributes);
        //}


        public static IHtmlContent FormControlFileFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
            => htmlHelper.FormControlFileFor(expression, new { });
        public static IHtmlContent FormControlFileFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
            => htmlHelper.FormControlFileFor(expression, htmlAttributes.ToHtmlAttributesDictionary());
        public static IHtmlContent FormControlFileFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object?> htmlAttributes)
        {
            htmlAttributes.Add("type", "file");
            return htmlHelper.FormControlTextBoxFor(expression, htmlAttributes);
        }

        public static IHtmlContent FormControlImageFileFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
            => htmlHelper.FormControlImageFileFor(expression, new { });
        public static IHtmlContent FormControlImageFileFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
            => htmlHelper.FormControlImageFileFor(expression, htmlAttributes.ToHtmlAttributesDictionary());
        public static IHtmlContent FormControlImageFileFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object?> htmlAttributes)
        {
            htmlAttributes.Add("type", "file");
            htmlAttributes.Add("accept", "image/*");
            return htmlHelper.FormControlTextBoxFor(expression, htmlAttributes);
        }

        public static IHtmlContent FormControlVideoFileFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
            => htmlHelper.FormControlVideoFileFor(expression, new { });
        public static IHtmlContent FormControlVideoFileFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
            => htmlHelper.FormControlVideoFileFor(expression, htmlAttributes.ToHtmlAttributesDictionary());
        public static IHtmlContent FormControFormControlVideoFileForlImageFileFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object?> htmlAttributes)
        {
            htmlAttributes.Add("type", "file");
            htmlAttributes.Add("accept", "video/*");
            return htmlHelper.FormControlTextBoxFor(expression, htmlAttributes);
        }
        public static IHtmlContent FormControlAudioFileFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
            => htmlHelper.FormControlAudioFileFor(expression, new { });
        public static IHtmlContent FormControlAudioFileFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
            => htmlHelper.FormControlAudioFileFor(expression, htmlAttributes.ToHtmlAttributesDictionary());
        public static IHtmlContent FormControlAudioFileFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object?> htmlAttributes)
        {
            htmlAttributes.Add("type", "file");
            htmlAttributes.Add("accept", "audio/*");
            return htmlHelper.FormControlTextBoxFor(expression, htmlAttributes);
        }

        private static string AcceptPDF = "application/pdf";
        
        public static IHtmlContent FormControlPDFFileFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
            => htmlHelper.FormControlPDFFileFor(expression, new { });
        public static IHtmlContent FormControlPDFFileFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
            => htmlHelper.FormControlPDFFileFor(expression, htmlAttributes.ToHtmlAttributesDictionary());
        public static IHtmlContent FormControlPDFFileFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object?> htmlAttributes)
        {
            htmlAttributes.Add("type", "file");
            htmlAttributes.Add("accept", AcceptPDF);
            return htmlHelper.FormControlTextBoxFor(expression, htmlAttributes);
        }

        private static string AcceptPowerPoint = ".ppt,.pptx,application/vnd.ms-powerpoint,application/vnd.openxmlformats-officedocument.presentationml.slideshow,application/vnd.openxmlformats-officedocument.presentationml.presentation";
        public static IHtmlContent FormControlPowerPointFileFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
            => htmlHelper.FormControlPowerPointFileFor(expression, new { });
        public static IHtmlContent FormControlPowerPointFileFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
            => htmlHelper.FormControlPowerPointFileFor(expression, htmlAttributes.ToHtmlAttributesDictionary());
        public static IHtmlContent FormControlPowerPointFileFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object?> htmlAttributes)
        {
            htmlAttributes.Add("type", "file");
            htmlAttributes.Add("accept", AcceptPowerPoint);
            return htmlHelper.FormControlTextBoxFor(expression, htmlAttributes);
        }
        private static string AcceptWord = ".doc,.docx,application/msword,application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        public static IHtmlContent FormControlWordFileFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
            => htmlHelper.FormControlWordFileFor(expression, new { });
        public static IHtmlContent FormControlWordFileFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
            => htmlHelper.FormControlWordFileFor(expression, htmlAttributes.ToHtmlAttributesDictionary());
        public static IHtmlContent FormControlWordFileFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object?> htmlAttributes)
        {
            htmlAttributes.Add("type", "file");
            htmlAttributes.Add("accept", AcceptWord);
            return htmlHelper.FormControlTextBoxFor(expression, htmlAttributes);
        }

        private static string AcceptExcel = ".csv,.xls,.xlsx,application/vnd.openxmlformats-officedocument.spreadsheetml.sheet,application/vnd.ms-excel";
        public static IHtmlContent FormControlExcelFileFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
            => htmlHelper.FormControlExcelFileFor(expression, new { });
        public static IHtmlContent FormControlExcelFileFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
            => htmlHelper.FormControlExcelFileFor(expression, htmlAttributes.ToHtmlAttributesDictionary());
        public static IHtmlContent FormControlExcelFileFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object?> htmlAttributes)
        {
            htmlAttributes.Add("type", "file");
            htmlAttributes.Add("accept", AcceptExcel);
            return htmlHelper.FormControlTextBoxFor(expression, htmlAttributes);
        }

        public static IHtmlContent FormControlDocumentFileFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
            => htmlHelper.FormControlDocumentFileFor(expression, new { });
        public static IHtmlContent FormControlDocumentFileFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
            => htmlHelper.FormControlDocumentFileFor(expression, htmlAttributes.ToHtmlAttributesDictionary());
        public static IHtmlContent FormControlDocumentFileFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object?> htmlAttributes)
        {
            htmlAttributes.Add("type", "file");
            htmlAttributes.Add("accept", $"{AcceptPDF},{AcceptPowerPoint},{AcceptWord},{AcceptExcel}");
            return htmlHelper.FormControlTextBoxFor(expression, htmlAttributes);
        }


        public static MvcForm BeginUploadForm(this IHtmlHelper htmlHelper, string actionName, string controllerName)
            => htmlHelper.BeginUploadForm(actionName, controllerName, new { });
        public static MvcForm BeginUploadForm(this IHtmlHelper htmlHelper, string actionName, string controllerName, object routeValues)
            => htmlHelper.BeginUploadForm(actionName, controllerName, routeValues, new { });
        public static MvcForm BeginUploadForm(this IHtmlHelper htmlHelper, string actionName, string controllerName, object routeValues, object htmlAttributes)
            => htmlHelper.BeginUploadForm(actionName, controllerName, routeValues, htmlAttributes.ToHtmlAttributesDictionary());
        public static MvcForm BeginUploadForm(this IHtmlHelper htmlHelper, string actionName, string controllerName, object routeValues, IDictionary<string, object?> htmlAttributes)
        {
            htmlAttributes.Add("enctype", "multipart/form-data");
            return htmlHelper.BeginForm(actionName, controllerName, routeValues, FormMethod.Post,true, htmlAttributes);
        }


    }
}