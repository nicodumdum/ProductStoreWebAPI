using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductStore.Filters
{
    public class NullableRequiredAttribute : ValidationAttribute, IClientValidatable
    {
        public bool AllowEmptyStrings { get; set; }

        public NullableRequiredAttribute()
            : base("The {0} field is required.")
        {
            AllowEmptyStrings = false;
        }

        public override bool IsValid(object value)
        {
            if (value == null)
                return false;

            if (value is string && !this.AllowEmptyStrings)
            {
                return !string.IsNullOrWhiteSpace(value as string);
            }

            return true;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var modelClientValidationRule = new ModelClientValidationRequiredRule(FormatErrorMessage(metadata.DisplayName));
            yield return modelClientValidationRule;
        }
    }
}