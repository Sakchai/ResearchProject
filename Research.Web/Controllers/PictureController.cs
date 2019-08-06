﻿using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Research.Core;
using Research.Infrastructure;
using Research.Services.Media;

namespace Research.Web.Controllers
{
    public partial class PictureController : BaseAdminController
    {
        private readonly IPictureService _pictureService;
        private readonly IResearchFileProvider _fileProvider;

        public PictureController(IPictureService pictureService,
            IResearchFileProvider fileProvider)
        {
            this._pictureService = pictureService;
            this._fileProvider = fileProvider;
        }

        [HttpPost]
        //do not validate request token (XSRF)
       // [AdminAntiForgery(true)] 
        public virtual IActionResult AsyncUpload()
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.UploadPictures))
            //    return Json(new { success = false, error = "You do not have required permissions" }, "text/plain");

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

            const string qqFileNameParameter = "qqfilename";
            var fileName = httpPostedFile.FileName;
            if (string.IsNullOrEmpty(fileName) && Request.Form.ContainsKey(qqFileNameParameter))
                fileName = Request.Form[qqFileNameParameter].ToString();
            //remove path (passed in IE)
            fileName = _fileProvider.GetFileName(fileName);

            var contentType = httpPostedFile.ContentType;

            var fileExtension = _fileProvider.GetFileExtension(fileName);
            if (!string.IsNullOrEmpty(fileExtension))
                fileExtension = fileExtension.ToLowerInvariant();

            //contentType is not always available 
            //that's why we manually update it here
            //http://www.sfsu.edu/training/mimetype.htm
            if (string.IsNullOrEmpty(contentType))
            {
                switch (fileExtension)
                {
                    case ".bmp":
                        contentType = MimeTypes.ImageBmp;
                        break;
                    case ".gif":
                        contentType = MimeTypes.ImageGif;
                        break;
                    case ".jpeg":
                    case ".jpg":
                    case ".jpe":
                    case ".jfif":
                    case ".pjpeg":
                    case ".pjp":
                        contentType = MimeTypes.ImageJpeg;
                        break;
                    case ".png":
                        contentType = MimeTypes.ImagePng;
                        break;
                    case ".tiff":
                    case ".tif":
                        contentType = MimeTypes.ImageTiff;
                        break;
                    default:
                        break;
                }
            }

            var picture = _pictureService.InsertPicture(fileBinary, contentType, null);

            //when returning JSON the mime-type must be set to text/plain
            //otherwise some browsers will pop-up a "Save As" dialog.
            return Json(new { success = true, pictureId = picture.Id, imageUrl = _pictureService.GetPictureUrl(picture, 100) });
        }

        public virtual IActionResult AsyncPdfUpload()
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.UploadPictures))
            //    return Json(new { success = false, error = "You do not have required permissions" }, "text/plain");

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

            var contentType = httpPostedFile.ContentType;

            if (string.IsNullOrEmpty(contentType))
            {
                 contentType = MimeTypes.ApplicationPdf;
            }

            var pdf = _pictureService.InsertPicture(fileBinary, contentType, null);

            //when returning JSON the mime-type must be set to text/plain
            //otherwise some browsers will pop-up a "Save As" dialog.
            return Json(new { success = true, pictureId = pdf.Id, imageUrl = _pictureService.GetPdfUrl(pdf) });
        }
    }
}