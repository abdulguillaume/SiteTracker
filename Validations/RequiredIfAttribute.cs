using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Nigeria_Reg.Validation
{
    public class RequiredIfAttribute : ValidationAttribute, IClientValidatable
    {
        private String PropertyName { get; set; }
        private String ErrorMessage { get; set; }
        private Object DesiredValue { get; set; }

        public RequiredIfAttribute(String propertyName, Object desiredvalue, String errormessage)
        {
            this.PropertyName = propertyName;
            this.DesiredValue = desiredvalue;
            this.ErrorMessage = errormessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            Object instance = context.ObjectInstance;
            Type type = instance.GetType();
            Object proprtyvalue = type.GetProperty(PropertyName).GetValue(instance, null);
            if (proprtyvalue!=null && proprtyvalue.ToString() == DesiredValue.ToString() && value == null)
            {
                return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success;
        }




        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            //throw new NotImplementedException();
            var rule = new ModelClientValidationRule()
            {
                ErrorMessage = this.ErrorMessage,//"**specify",//FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "otherspecifyvalidation"
            };


            rule.ValidationParameters.Add("propertyname", PropertyName);
            rule.ValidationParameters.Add("desiredvalue", DesiredValue);

            yield return rule;
        }
    }
}