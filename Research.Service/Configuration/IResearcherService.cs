using Research.Core;
using Research.Data;

namespace Research.Services
{
    public partial interface IResearcherService
    {

        /// <summary>
        /// Gets all researchers
        /// </summary>
        /// <param name="createdFromUtc">Created date from (UTC); null to load all records</param>
        /// <param name="createdToUtc">Created date to (UTC); null to load all records</param>
        /// <param name="customerRoleIds">A list of customer role identifiers to filter by (at least one match); pass null or empty list in order to load all researchers; </param>
        /// <param name="email">Email; null to load all researchers</param>
        /// <param name="username">Username; null to load all researchers</param>
        /// <param name="firstName">First name; null to load all researchers</param>
        /// <param name="lastName">Last name; null to load all researchers</param>
        /// <param name="dayOfBirth">Day of birth; 0 to load all researchers</param>
        /// <param name="monthOfBirth">Month of birth; 0 to load all researchers</param>
        /// <param name="phone">Phone; null to load all researchers</param>
        /// <param name="zipPostalCode">Phone; null to load all researchers</param>
        /// <param name="ipAddress">IP address; null to load all researchers</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="getOnlyTotalCount">A value in indicating whether you want to load only total number of records. Set to "true" if you don't want to load data from database</param>
        /// <returns>Researchers</returns>
        IPagedList<Researcher> GetAllResearchers(string email = null, string username = null, string firstName = null, string lastName = null, int dayOfBirth = 0, int monthOfBirth = 0, string phone = null, string zipPostalCode = null, string ipAddress = null, int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false);

        /// <summary>
        /// Delete a Researcher
        /// </summary>
        /// <param name="researcher">Researcher</param>
        void DeleteResearcher(Researcher researcher);

        /// <summary>
        /// Gets a Researcher
        /// </summary>
        /// <param name="researcherId">Researcher identifier</param>
        /// <returns>A Researcher</returns>
        Researcher GetResearcherById(int researcherId);

        /// <summary>
        /// Get researcher by email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>Researcher</returns>
        Researcher GetResearcherByEmail(string email);

        /// <summary>
        /// Updates the researcher
        /// </summary>
        /// <param name="researcher">Researcher</param>
        void UpdateResearcher(Researcher researcher);


        /// <summary>
        /// Remove researcher
        /// </summary>
        /// <param name="customer">Researcher</param>
        /// <param name="address">Address</param>
        void RemoveResearcherEducation(Researcher researcher, ResearcherEducation researcherEducation);
        Researcher GetResearcherById(int? researcherId);
    }
}
