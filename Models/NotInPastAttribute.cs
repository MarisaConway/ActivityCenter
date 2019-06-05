    
using System;
using System.ComponentModel.DataAnnotations;
public class NotInPastAttribute : ValidationAttribute{

    protected override ValidationResult IsValid(object value, ValidationContext validationContext){
        if(value != null && (DateTime)value < DateTime.Now){
            return new ValidationResult("Date Must Be in the Future");
        }
        return ValidationResult.Success;
    }
}