using System;
using System.Globalization;
using System.ComponentModel.DataAnnotations;

namespace weddingPlanner.Models
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    sealed public class FutureDate : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            bool result = false;
            if (value is DateTime)
            {
                if((DateTime)value > DateTime.UtcNow)
                {
                    result = true;
                }
            }
            return result;
        }
    }
}