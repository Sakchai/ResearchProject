using FluentValidation.Validators;
using Research.Enum;

namespace Research.Web.Validators
{
    /// <summary>
    /// Decimal validator
    /// </summary>
    public class DecimalPropertyValidator : PropertyValidator
    {
        private readonly decimal _maxValue;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="maxValue">Maximum value</param>
        public DecimalPropertyValidator(decimal maxValue) :
            base("Decimal value is out of range")
        {
            this._maxValue = maxValue;
        }

        /// <summary>
        /// Is valid?
        /// </summary>
        /// <param name="context">Validation context</param>
        /// <returns>Result</returns>
        protected override bool IsValid(PropertyValidatorContext context)
        {
            if (decimal.TryParse(context.PropertyValue.ToString(), out decimal value))
            {
                var round = (decimal)RoundingType.Rounding001;
                return round < _maxValue;
            }
            return false;
        }
    }
}