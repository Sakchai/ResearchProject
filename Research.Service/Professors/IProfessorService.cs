using System.Collections.Generic;
using Research.Core;
using Research.Data;

namespace Research.Services.Professors
{
    /// <summary>
    /// Professor service interface
    /// </summary>
    public partial interface IProfessorService
    {
        /// <summary>
        /// Delete professor
        /// </summary>
        /// <param name="professor">Professor</param>
        void DeleteProfessor(Professor professor);

        /// <summary>
        /// Gets all faculties
        /// </summary>
        /// <param name="professorName">Professor name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Professors</returns>
        IPagedList<Professor> GetAllProfessors(int titleId = 0, string firstName = null, string lastName = null, string professorType = null,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

        /// <summary>
        /// Gets a professor
        /// </summary>
        /// <param name="professorId">Professor identifier</param>
        /// <returns>Professor</returns>
        Professor GetProfessorById(int professorId);

        string GetNextProfessorNumber();

        /// <summary>
        /// Inserts professor
        /// </summary>
        /// <param name="professor">Professor</param>
        void InsertProfessor(Professor professor);

        /// <summary>
        /// Updates the professor
        /// </summary>
        /// <param name="professor">Professor</param>
        void UpdateProfessor(Professor professor);

        /// <summary>
        /// Returns a list of names of not existing faculties
        /// </summary>
        /// <param name="professorIdsNames">The names and/or IDs of the faculties to check</param>
        /// <returns>List of names and/or IDs not existing faculties</returns>
        string[] GetNotExistingProfessors(string[] professorIdsNames);


        /// <summary>
        /// Gets faculties by identifier
        /// </summary>
        /// <param name="professorIds">Professor identifiers</param>
        /// <returns>Professors</returns>
        List<Professor> GetProfessorsByIds(int[] professorIds);
        List<Professor> GetAllProfessors();
    }
}
