
using Research.Data;
using Research.Web.Models.ResearchIssues;

namespace Research.Web.Factories
{
    /// <summary>
    /// Represents the researcher model factory
    /// </summary>
    public partial interface IResearchIssueModelFactory
    {
        /// <summary>
        /// Prepare researcher search model
        /// </summary>
        /// <param name="model">ResearchIssue search model</param>
        /// <returns>ResearchIssue search model</returns>
        ResearchIssueSearchModel PrepareResearchIssueSearchModel(ResearchIssueSearchModel searchModel);

        /// <summary>
        /// Prepare paged researcher list model
        /// </summary>
        /// <param name="searchModel">ResearchIssue search model</param>
        /// <returns>ResearchIssue list model</returns>
        ResearchIssueListModel PrepareResearchIssueListModel(ResearchIssueSearchModel searchModel);

        /// <summary>
        /// Prepare researcher model
        /// </summary>
        /// <param name="model">ResearchIssue model</param>
        /// <param name="researcher">ResearchIssue</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>ResearchIssue model</returns>
        ResearchIssueModel PrepareResearchIssueModel(ResearchIssueModel model, ResearchIssue researcher, bool excludeProperties = false);
    }
}