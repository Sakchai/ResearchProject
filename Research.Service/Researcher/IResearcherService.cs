using Research.Core;
using Research.Data;
using Research.Enum;

namespace Research.Services.Researchers
{
    public partial interface IResearcherService
    {

        /// <summary>
        /// Gets all researchers
        /// </summary>
        /// <param name="agency">agency; null to load all researchers</param>
        /// <param name="personalType">personalType; null to load all researchers</param>
        /// <param name="firstName">First name; null to load all researchers</param>
        /// <param name="lastName">Last name; null to load all researchers</param>
        /// <param name="isActive">isActive; true to load all researchers</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="getOnlyTotalCount">A value in indicating whether you want to load only total number of records. Set to "true" if you don't want to load data from database</param>
        /// <returns>Researchers</returns>
        IPagedList<Researcher> GetAllResearchers(int agency = 0, int personalType = 0, string firstName = null, string lastName = null, int isCompleted = 0, int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false);

        /// <summary>
        /// Gets a researcher
        /// </summary>
        /// <param name="researcherId">Researcher identifier</param>
        /// <returns>Researcher</returns>
        Researcher GetResearcherById(int researcherId);

        /// <summary>
        /// Inserts researcher
        /// </summary>
        /// <param name="researcher">Researcher</param>
        void InsertResearcher(Researcher researcher);

        /// <summary>
        /// Updates the researcher
        /// </summary>
        /// <param name="researcher">Researcher</param>
        void UpdateResearcher(Researcher researcher);
        /// <summary>
        /// Delete a Researcher
        /// </summary>
        /// <param name="researcher">Researcher</param>
        void DeleteResearcher(Researcher researcher);


        /// <summary>
        /// Get researcher by email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>Researcher</returns>
        Researcher GetResearcherByEmail(string email);



        /// <summary>
        /// Remove researcher
        /// </summary>
        /// <param name="customer">Researcher</param>
        /// <param name="address">Address</param>
        void RemoveResearcherEducation(Researcher researcher, ResearcherEducation researcherEducation);

        /// <summary>
        /// Gets all researcherEducations
        /// </summary>
        /// <param name="researcherId">researcherId; null to load all researcherEducations</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="getOnlyTotalCount">A value in indicating whether you want to load only total number of records. Set to "true" if you don't want to load data from database</param>
        /// <returns>ResearcherEducations</returns>
        IPagedList<ResearcherEducation> GetAllResearcherEducations(int researcherId = 0, int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false);

        /// <summary>
        /// Gets a researcherEducation
        /// </summary>
        /// <param name="researcherEducationId">ResearcherEducation identifier</param>
        /// <returns>ResearcherEducation</returns>
        ResearcherEducation GetResearcherEducationById(int researcherEducationId);

        /// <summary>
        /// Inserts researcherEducation
        /// </summary>
        /// <param name="researcherEducation">ResearcherEducation</param>
        void InsertResearcherEducation(ResearcherEducation researcherEducation);

        /// <summary>
        /// Updates the researcherEducation
        /// </summary>
        /// <param name="researcherEducation">ResearcherEducation</param>
        void UpdateResearcherEducation(ResearcherEducation researcherEducation);
        /// <summary>
        /// Delete a ResearcherEducation
        /// </summary>
        /// <param name="researcherEducation">ResearcherEducation</param>
        void DeleteResearcherEducation(ResearcherEducation researcherEducation);

        string GetNextNumber();
    }
}
