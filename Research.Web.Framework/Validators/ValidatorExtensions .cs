using FluentValidation;
using Research.Core.Domain.Users;

namespace Research.Web.Validators
{
    /// <summary>
    /// Validator extensions
    /// </summary>
    public static class ValidatorExtensions
    {

        /// <summary>
        /// Set decimal validator
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="ruleBuilder">RuleBuilder</param>
        /// <param name="maxValue">Maximum value</param>
        /// <returns>Result</returns>
        public static IRuleBuilderOptions<T, decimal> IsDecimal<T>(this IRuleBuilder<T, decimal> ruleBuilder, decimal maxValue)
        {
            return ruleBuilder.SetValidator(new DecimalPropertyValidator(maxValue));
        }

        /// <summary>
        /// Set username validator
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="ruleBuilder">RuleBuilder</param>
        /// <param name="userSettings">User settings</param>
        /// <returns>Result</returns>
        public static IRuleBuilderOptions<T, string> IsUsername<T>(this IRuleBuilder<T, string> ruleBuilder, UserSettings userSettings)
        {
            return ruleBuilder.SetValidator(new UsernamePropertyValidator(userSettings));
        }
    }
}
