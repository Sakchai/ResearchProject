using Research.Data;
using System;
namespace Research.Services.Media
{
    /// <summary>
    /// Download service interface
    /// </summary>
    public partial interface IDownloadService
    {
        /// <summary>
        /// Gets a download
        /// </summary>
        /// <param name="downloadId">Download identifier</param>
        /// <returns>Download</returns>
        Download GetDownloadById(int downloadId);

        /// <summary>
        /// Gets a download by GUID
        /// </summary>
        /// <param name="downloadGuid">Download GUID</param>
        /// <returns>Download</returns>
        Download GetDownloadByGuid(Guid downloadGuid);

        /// <summary>
        /// Deletes a download
        /// </summary>
        /// <param name="download">Download</param>
        void DeleteDownload(Download download);

        /// <summary>
        /// Inserts a download
        /// </summary>
        /// <param name="download">Download</param>
        void InsertDownload(Download download);

        /// <summary>
        /// Updates the download
        /// </summary>
        /// <param name="download">Download</param>
        void UpdateDownload(Download download);


    }
}
