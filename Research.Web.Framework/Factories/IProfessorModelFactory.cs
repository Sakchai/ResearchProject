
using Research.Data;
using Research.Web.Models.Professors;

namespace Research.Web.Factories
{
    /// <summary>
    /// Represents the researcher model factory
    /// </summary>
    public partial interface IProfessorModelFactory
    {
        /// <summary>
        /// Prepare researcher search model
        /// </summary>
        /// <param name="model">Professor search model</param>
        /// <returns>Professor search model</returns>
        ProfessorSearchModel PrepareProfessorSearchModel(ProfessorSearchModel searchModel);

        /// <summary>
        /// Prepare paged researcher list model
        /// </summary>
        /// <param name="searchModel">Professor search model</param>
        /// <returns>Professor list model</returns>
        ProfessorListModel PrepareProfessorListModel(ProfessorSearchModel searchModel);

        /// <summary>
        /// Prepare researcher model
        /// </summary>
        /// <param name="model">Professor model</param>
        /// <param name="researcher">Professor</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>Professor model</returns>
        ProfessorModel PrepareProfessorModel(ProfessorModel model, Professor researcher, bool excludeProperties = false);
    }
}