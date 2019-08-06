using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Research.Core;
using Research.Data;
using Research.Infrastructure;
using Research.Services.Media;

namespace Research.Web.Controllers
{
    public partial class DownloadController : BaseAdminController
    {
        #region Fields

        private readonly IDownloadService _downloadService;
        private readonly IResearchFileProvider _fileProvider;

        #endregion

        #region Ctor

        public DownloadController(IDownloadService downloadService,
            IResearchFileProvider fileProvider)
        {
            this._downloadService = downloadService;
            this._fileProvider = fileProvider;
        }

        #endregion

        #region Methods

        public virtual IActionResult DownloadFile(Guid downloadGuid)
        {
            var download = _downloadService.GetDownloadByGuid(downloadGuid);
            if (download == null)
                return Content("No download record found with the specified id");

            if (download.UseDownloadUrl)
                return new RedirectResult(download.DownloadUrl);

            //use stored data
            if (download.DownloadBinary == null)
                return Content($"Download data is not available any more. Download GD={download.Id}");

            var fileName = !string.IsNullOrWhiteSpace(download.Filename) ? download.Filename : download.Id.ToString();
            var contentType = !string.IsNullOrWhiteSpace(download.ContentType)
                ? download.ContentType
                : MimeTypes.ApplicationOctetStream;
            return new FileContentResult(download.DownloadBinary, contentType)
            {
                FileDownloadName = fileName + download.Extension
            };
        }

        [HttpPost]
        //do not validate request token (XSRF)
        //[AdminAntiForgery(true)] 
        public virtual IActionResult SaveDownloadUrl(string downloadUrl)
        {
            //insert
            var download = new Download
            {
                DownloadGuid = Guid.NewGuid(),
                UseDownloadUrl = true,
                DownloadUrl = downloadUrl,
                IsNew = true
              };
            _downloadService.InsertDownload(download);

            return Json(new { downloadId = download.Id });
        }

        [HttpPost]
        //do not validate request token (XSRF)
       // [AdminAntiForgery(true)]
        public virtual IActionResult AsyncUpload()
        {
            var httpPostedFile = Request.Form.Files.FirstOrDefault();
            if (httpPostedFile == null)
            {
                return Json(new
                {
                    success = false,
                    message = "No file uploaded",
                    downloadGuid = Guid.Empty
                });
            }

            var fileBinary = httpPostedFile.GetDownloadBits();

            var qqFileNameParameter = "qqfilename";
            var fileName = httpPostedFile.FileName;
            if (string.IsNullOrEmpty(fileName) && Request.Form.ContainsKey(qqFileNameParameter))
                fileName = Request.Form[qqFileNameParameter].ToString();
            //remove path (passed in IE)
            fileName = _fileProvider.GetFileName(fileName);

            var contentType = httpPostedFile.ContentType;

            var fileExtension = _fileProvider.GetFileExtension(fileName);
            if (!string.IsNullOrEmpty(fileExtension))
                fileExtension = fileExtension.ToLowerInvariant();

            var download = new Download
            {
                DownloadGuid = Guid.NewGuid(),
                UseDownloadUrl = false,
                DownloadUrl = string.Empty,
                DownloadBinary = fileBinary,
                ContentType = contentType,
                //we store filename without extension for downloads
                Filename = _fileProvider.GetFileNameWithoutExtension(fileName),
                Extension = fileExtension,
                IsNew = true
            };
            _downloadService.InsertDownload(download);

            //when returning JSON the mime-type must be set to text/plain
            //otherwise some browsers will pop-up a "Save As" dialog.
            return Json(new { success = true, 
                downloadId = download.Id, 
                downloadUrl = Url.Action("DownloadFile", new { downloadGuid = download.DownloadGuid }) });
        }

        #endregion
    }
}