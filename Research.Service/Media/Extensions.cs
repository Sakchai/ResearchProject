using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Research.Data;

namespace Research.Services.Media
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Gets the download binary array
        /// </summary>
        /// <param name="file">File</param>
        /// <returns>Download binary array</returns>
        public static byte[] GetDownloadBits(this IFormFile file)
        {
            using (var fileStream = file.OpenReadStream())
            using (var ms = new MemoryStream())
            {
                fileStream.CopyTo(ms);
                var fileBytes = ms.ToArray();
                return fileBytes;
            }
        }

        /// <summary>
        /// Gets the picture binary array
        /// </summary>
        /// <param name="file">File</param>
        /// <returns>Picture binary array</returns>
        public static byte[] GetPictureBits(this IFormFile file)
        {
            return GetDownloadBits(file);
        }

        /// <summary>
        /// Get researcher picture 
        /// </summary>
        /// <param name="researcher">Researcher</param>
        /// <param name="pictureService">Picture service</param>
        /// <returns>Picture</returns>
        public static Picture GetResearcherPicture(this Researcher researcher,
            IPictureService pictureService)
        {
            if (researcher == null)
                throw new ArgumentNullException(nameof(researcher));

            if (pictureService == null)
                throw new ArgumentNullException(nameof(pictureService));

            //now let's load the default product picture
            var productPicture = pictureService.GetPicturesByResearcherId(researcher.Id, 1);
            if (productPicture != null)
                return productPicture;

            return null;
        }
    }
}
