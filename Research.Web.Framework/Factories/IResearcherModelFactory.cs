
using Research.Data;
using Research.Web.Models.Researchers;

namespace Research.Web.Factories
{
    /// <summary>
    /// Represents the researcher model factory
    /// </summary>
    public partial interface IResearcherModelFactory
    {
        /// <summary>
        /// Prepare researcher search model
        /// </summary>
        /// <param name="model">Researcher search model</param>
        /// <returns>Researcher search model</returns>
        ResearcherSearchModel PrepareResearcherSearchModel(ResearcherSearchModel searchModel);

        /// <summary>
        /// Prepare paged researcher list model
        /// </summary>
        /// <param name="searchModel">Researcher search model</param>
        /// <returns>Researcher list model</returns>
        ResearcherListModel PrepareResearcherListModel(ResearcherSearchModel searchModel);

        /// <summary>
        /// Prepare researcher model
        /// </summary>
        /// <param name="model">Researcher model</param>
        /// <param name="researcher">Researcher</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>Researcher model</returns>
        ResearcherModel PrepareResearcherModel(ResearcherModel model, Researcher researcher, bool excludeProperties = false);
        ResearcherEducationListModel PrepareResearcherEducationListModel(ResearcherEducationSearchModel searchModel, Researcher researcher);
    }
}