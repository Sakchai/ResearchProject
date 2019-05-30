using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Research.Core;
using Research.Core.Data;
using Research.Core.Domain.Media;
using Research.Data;
using Research.Enum;
using Research.Infrastructure;
using Research.Services.Configuration;
using Research.Services.Events;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using static SixLabors.ImageSharp.Configuration;

namespace Research.Services.Media
{
    /// <summary>
    /// Picture service
    /// </summary>
    public partial class PictureService : IPictureService
    {
        #region Fields

        private readonly IRepository<Picture> _pictureRepository;
        private readonly IRepository<Researcher> _researcherRepository;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;
        private readonly IDbContext _dbContext;
        private readonly IEventPublisher _eventPublisher;
        private readonly MediaSettings _mediaSettings;
        private readonly IDataProvider _dataProvider;
        private readonly IResearchFileProvider _fileProvider;
        private readonly IRepository<PictureBinary> _pictureBinaryRepository;
        private readonly IRepository<ProjectProgress> _projectProgressRepository;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="pictureRepository">Picture repository</param>
        /// <param name="productPictureRepository">Product picture repository</param>
        /// <param name="settingService">Setting service</param>
        /// <param name="webHelper">Web helper</param>
        /// <param name="dbContext">Database context</param>
        /// <param name="eventPublisher">Event publisher</param>
        /// <param name="mediaSettings">Media settings</param>
        /// <param name="dataProvider">Data provider</param>
        /// <param name="fileProvider">File provider</param>
        /// <param name="pictureBinaryRepository">PictureBinary repository</param>
        public PictureService(IRepository<Picture> pictureRepository,
            IRepository<Researcher> productPictureRepository,
            ISettingService settingService,
            IWebHelper webHelper,
            IDbContext dbContext,
            IEventPublisher eventPublisher,
            MediaSettings mediaSettings,
            IDataProvider dataProvider,
            IResearchFileProvider fileProvider,
            IRepository<PictureBinary> pictureBinaryRepository,
            IRepository<ProjectProgress> projectProgressRepository)
        {
            this._pictureRepository = pictureRepository;
            this._researcherRepository = productPictureRepository;
            this._settingService = settingService;
            this._webHelper = webHelper;
            this._dbContext = dbContext;
            this._eventPublisher = eventPublisher;
            this._mediaSettings = mediaSettings;
            this._dataProvider = dataProvider;
            this._fileProvider = fileProvider;
            this._pictureBinaryRepository = pictureBinaryRepository;
            this._projectProgressRepository = projectProgressRepository;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Calculates picture dimensions whilst maintaining aspect
        /// </summary>
        /// <param name="originalSize">The original picture size</param>
        /// <param name="targetSize">The target picture size (longest side)</param>
        /// <param name="resizeType">Resize type</param>
        /// <param name="ensureSizePositive">A value indicatingh whether we should ensure that size values are positive</param>
        /// <returns></returns>
        protected virtual Size CalculateDimensions(Size originalSize, int targetSize,
            ResizeType resizeType = ResizeType.LongestSide, bool ensureSizePositive = true)
        {
            float width, height;

            switch (resizeType)
            {
                case ResizeType.LongestSide:
                    if (originalSize.Height > originalSize.Width)
                    {
                        // portrait
                        width = originalSize.Width * (targetSize / (float)originalSize.Height);
                        height = targetSize;
                    }
                    else
                    {
                        // landscape or square
                        width = targetSize;
                        height = originalSize.Height * (targetSize / (float)originalSize.Width);
                    }
                    break;
                case ResizeType.Width:
                    width = targetSize;
                    height = originalSize.Height * (targetSize / (float)originalSize.Width);
                    break;
                case ResizeType.Height:
                    width = originalSize.Width * (targetSize / (float)originalSize.Height);
                    height = targetSize;
                    break;
                default:
                    throw new Exception("Not supported ResizeType");
            }

            if (ensureSizePositive)
            {
                if (width < 1)
                    width = 1;
                if (height < 1)
                    height = 1;
            }

            //we invoke Math.Round to ensure that no white background is rendered - https://www.nopcommerce.com/boards/t/40616/image-resizing-bug.aspx
            return new Size((int)Math.Round(width), (int)Math.Round(height));
        }

        /// <summary>
        /// Returns the file extension from mime type.
        /// </summary>
        /// <param name="mimeType">Mime type</param>
        /// <returns>File extension</returns>
        protected virtual string GetFileExtensionFromMimeType(string mimeType)
        {
            if (mimeType == null)
                return null;

            //TODO use FileExtensionContentTypeProvider to get file extension

            var parts = mimeType.Split('/');
            var lastPart = parts[parts.Length - 1];
            switch (lastPart)
            {
                case "pjpeg":
                    lastPart = "jpg";
                    break;
                case "x-png":
                    lastPart = "png";
                    break;
                case "x-icon":
                    lastPart = "ico";
                    break;
                case "x-pdf":
                    lastPart = "pdf";
                    break;
            }
            return lastPart;
        }

        /// <summary>
        /// Loads a picture from file
        /// </summary>
        /// <param name="pictureId">Picture identifier</param>
        /// <param name="mimeType">MIME type</param>
        /// <returns>Picture binary</returns>
        protected virtual byte[] LoadPictureFromFile(int pictureId, string mimeType)
        {
            var lastPart = GetFileExtensionFromMimeType(mimeType);
            var fileName = $"{pictureId:0000000}_0.{lastPart}";
            var filePath = lastPart.Equals("pdf") ? GetPdfLocalPath(fileName) : GetPictureLocalPath(fileName);

            return _fileProvider.ReadAllBytes(filePath);
        }


        /// <summary>
        /// Save picture on file system
        /// </summary>
        /// <param name="pictureId">Picture identifier</param>
        /// <param name="pictureBinary">Picture binary</param>
        /// <param name="mimeType">MIME type</param>
        protected virtual void SavePictureInFile(int pictureId, byte[] pictureBinary, string mimeType)
        {
            var lastPart = GetFileExtensionFromMimeType(mimeType);
            var fileName = $"{pictureId:0000000}_0.{lastPart}";
            _fileProvider.WriteAllBytes(GetPictureLocalPath(fileName), pictureBinary);
        }


        /// <summary>
        /// Get picture local path. Used when images stored on file system (not in the database)
        /// </summary>
        /// <param name="fileName">Filename</param>
        /// <returns>Local pdf path</returns>
        protected virtual string GetPdfLocalPath(string fileName)
        {
            return _fileProvider.GetAbsolutePath("files", fileName);
        }

        /// </summary>
        /// <param name="pictureId">Picture identifier</param>
        /// <param name="pictureBinary">Picture binary</param>
        /// <param name="mimeType">MIME type</param>
        protected virtual void SavePdfInFile(int pictureId, byte[] pictureBinary, string mimeType)
        {
            var lastPart = GetFileExtensionFromMimeType(mimeType);
            var fileName = $"{pictureId:0000000}_0.{lastPart}";
            _fileProvider.WriteAllBytes(GetPdfLocalPath(fileName), pictureBinary);
        }

        /// <summary>
        /// Delete a picture on file system
        /// </summary>
        /// <param name="picture">Picture</param>
        protected virtual void DeletePictureOnFileSystem(Picture picture)
        {
            if (picture == null)
                throw new ArgumentNullException(nameof(picture));

            var lastPart = GetFileExtensionFromMimeType(picture.MimeType);
            var fileName = $"{picture.Id:0000000}_0.{lastPart}";
            var filePath = GetPictureLocalPath(fileName);
            _fileProvider.DeleteFile(filePath);
        }

        /// <summary>
        /// Delete picture thumbs
        /// </summary>
        /// <param name="picture">Picture</param>
        protected virtual void DeletePictureThumbs(Picture picture)
        {
            var filter = $"{picture.Id:0000000}*.*";
            var currentFiles = _fileProvider.GetFiles(_fileProvider.GetAbsolutePath(ResearchMediaDefaults.ImageThumbsPath), filter, false);
            foreach (var currentFileName in currentFiles)
            {
                var thumbFilePath = GetThumbLocalPath(currentFileName);
                _fileProvider.DeleteFile(thumbFilePath);
            }
        }

        /// <summary>
        /// Get picture (thumb) local path
        /// </summary>
        /// <param name="thumbFileName">Filename</param>
        /// <returns>Local picture thumb path</returns>
        protected virtual string GetThumbLocalPath(string thumbFileName)
        {
            var thumbsDirectoryPath = _fileProvider.GetAbsolutePath(ResearchMediaDefaults.ImageThumbsPath);

            if (_mediaSettings.MultipleThumbDirectories)
            {
                //get the first two letters of the file name
                var fileNameWithoutExtension = _fileProvider.GetFileNameWithoutExtension(thumbFileName);
                if (fileNameWithoutExtension != null && fileNameWithoutExtension.Length > ResearchMediaDefaults.MultipleThumbDirectoriesLength)
                {
                    var subDirectoryName = fileNameWithoutExtension.Substring(0, ResearchMediaDefaults.MultipleThumbDirectoriesLength);
                    thumbsDirectoryPath = _fileProvider.GetAbsolutePath(ResearchMediaDefaults.ImageThumbsPath, subDirectoryName);
                    _fileProvider.CreateDirectory(thumbsDirectoryPath);
                }
            }

            var thumbFilePath = _fileProvider.Combine(thumbsDirectoryPath, thumbFileName);
            return thumbFilePath;
        }

        protected virtual string GetThumbLocalPdfPath(string thumbFileName)
        {
            var thumbsDirectoryPath = _fileProvider.GetAbsolutePath(ResearchMediaDefaults.PdfThumbsPath);

            if (_mediaSettings.MultipleThumbDirectories)
            {
                //get the first two letters of the file name
                var fileNameWithoutExtension = _fileProvider.GetFileNameWithoutExtension(thumbFileName);
                if (fileNameWithoutExtension != null && fileNameWithoutExtension.Length > ResearchMediaDefaults.MultipleThumbDirectoriesLength)
                {
                    var subDirectoryName = fileNameWithoutExtension.Substring(0, ResearchMediaDefaults.MultipleThumbDirectoriesLength);
                    thumbsDirectoryPath = _fileProvider.GetAbsolutePath(ResearchMediaDefaults.PdfThumbsPath, subDirectoryName);
                    _fileProvider.CreateDirectory(thumbsDirectoryPath);
                }
            }

            var thumbFilePath = _fileProvider.Combine(thumbsDirectoryPath, thumbFileName);
            return thumbFilePath;
        }
        /// <summary>
        /// Get picture (thumb) URL 
        /// </summary>
        /// <param name="thumbFileName">Filename</param>
        /// <param name="storeLocation">Store location URL; null to use determine the current store location automatically</param>
        /// <returns>Local picture thumb path</returns>
        protected virtual string GetThumbUrl(string thumbFileName, string storeLocation = null)
        {
            //storeLocation = !string.IsNullOrEmpty(storeLocation)
            //                        ? storeLocation
            //                        : _webHelper.GetStoreLocation();
            var url = storeLocation + "/images/thumbs/";

            if (_mediaSettings.MultipleThumbDirectories)
            {
                //get the first two letters of the file name
                var fileNameWithoutExtension = _fileProvider.GetFileNameWithoutExtension(thumbFileName);
                if (fileNameWithoutExtension != null && fileNameWithoutExtension.Length > ResearchMediaDefaults.MultipleThumbDirectoriesLength)
                {
                    var subDirectoryName = fileNameWithoutExtension.Substring(0, ResearchMediaDefaults.MultipleThumbDirectoriesLength);
                    url = url + subDirectoryName + "/";
                }
            }

            url = url + thumbFileName;
            return url;
        }

        protected virtual string GetThumbPdfUrl(string thumbFileName)
        {

            var url = "/files/thumbs/";

            if (_mediaSettings.MultipleThumbDirectories)
            {
                //get the first two letters of the file name
                var fileNameWithoutExtension = _fileProvider.GetFileNameWithoutExtension(thumbFileName);
                if (fileNameWithoutExtension != null && fileNameWithoutExtension.Length > ResearchMediaDefaults.MultipleThumbDirectoriesLength)
                {
                    var subDirectoryName = fileNameWithoutExtension.Substring(0, ResearchMediaDefaults.MultipleThumbDirectoriesLength);
                    url = url + subDirectoryName + "/";
                }
            }

            url = url + thumbFileName;
            return url;
        }
        /// <summary>
        /// Get picture local path. Used when images stored on file system (not in the database)
        /// </summary>
        /// <param name="fileName">Filename</param>
        /// <returns>Local picture path</returns>
        protected virtual string GetPictureLocalPath(string fileName)
        {
            return _fileProvider.GetAbsolutePath("images", fileName);
        }

        /// <summary>
        /// Gets the loaded picture binary depending on picture storage settings
        /// </summary>
        /// <param name="picture">Picture</param>
        /// <param name="fromDb">Load from database; otherwise, from file system</param>
        /// <returns>Picture binary</returns>
        protected virtual byte[] LoadPictureBinary(Picture picture, bool fromDb)
        {
            if (picture == null)
                throw new ArgumentNullException(nameof(picture));

            var result = fromDb
                ? picture.PictureBinary?.BinaryData ?? new byte[0]
                : LoadPictureFromFile(picture.Id, picture.MimeType);

            return result;
        }

        /// <summary>
        /// Get a value indicating whether some file (thumb) already exists
        /// </summary>
        /// <param name="thumbFilePath">Thumb file path</param>
        /// <param name="thumbFileName">Thumb file name</param>
        /// <returns>Result</returns>
        protected virtual bool GeneratedThumbExists(string thumbFilePath, string thumbFileName)
        {
            return _fileProvider.FileExists(thumbFilePath);
        }

        /// <summary>
        /// Save a value indicating whether some file (thumb) already exists
        /// </summary>
        /// <param name="thumbFilePath">Thumb file path</param>
        /// <param name="thumbFileName">Thumb file name</param>
        /// <param name="mimeType">MIME type</param>
        /// <param name="binary">Picture binary</param>
        protected virtual void SaveThumb(string thumbFilePath, string thumbFileName, string mimeType, byte[] binary)
        {
            //ensure \thumb directory exists
            var thumbsDirectoryPath = _fileProvider.GetAbsolutePath(ResearchMediaDefaults.ImageThumbsPath);
            _fileProvider.CreateDirectory(thumbsDirectoryPath);

            //save
            _fileProvider.WriteAllBytes(thumbFilePath, binary);
        }

        protected virtual void SavePdfThumb(string thumbFilePath, string thumbFileName, string mimeType, byte[] binary)
        {
            //ensure \thumb directory exists
            var thumbsDirectoryPath = _fileProvider.GetAbsolutePath(ResearchMediaDefaults.PdfThumbsPath);
            _fileProvider.CreateDirectory(thumbsDirectoryPath);

            //save
            _fileProvider.WriteAllBytes(thumbFilePath, binary);
        }

        /// <summary>
        /// Encode the image into a byte array in accordance with the specified image format
        /// </summary>
        /// <typeparam name="T">Pixel data type</typeparam>
        /// <param name="image">Image data</param>
        /// <param name="imageFormat">Image format</param>
        /// <param name="quality">Quality index that will be used to encode the image</param>
        /// <returns>Image binary data</returns>
        protected virtual byte[] EncodeImage<T>(Image<T> image, IImageFormat imageFormat, int? quality = null) where T : struct, IPixel<T>
        {
            using (var stream = new MemoryStream())
            {
                var imageEncoder = Default.ImageFormatsManager.FindEncoder(imageFormat);
                switch (imageEncoder)
                {
                    case JpegEncoder jpegEncoder:
                        jpegEncoder.IgnoreMetadata = true;
                        jpegEncoder.Quality = quality ?? _mediaSettings.DefaultImageQuality;
                        jpegEncoder.Encode(image, stream);
                        break;

                    case PngEncoder pngEncoder:
                        //pngEncoder.Gamma = (float) PngColorType.RgbWithAlpha;
                        pngEncoder.Encode(image, stream);
                        break;

                    case BmpEncoder bmpEncoder:
                        bmpEncoder.BitsPerPixel = BmpBitsPerPixel.Pixel32;
                        bmpEncoder.Encode(image, stream);
                        break;

                    case GifEncoder gifEncoder:
                        gifEncoder.IgnoreMetadata = true;
                        gifEncoder.Encode(image, stream);
                        break;

                    default:
                        imageEncoder.Encode(image, stream);
                        break;
                }
                return stream.ToArray();
            }
        }

        /// <summary>
        /// Updates the picture binary data
        /// </summary>
        /// <param name="picture">The picture object</param>
        /// <param name="binaryData">The picture binary data</param>
        /// <returns>Picture binary</returns>
        protected virtual PictureBinary UpdatePictureBinary(Picture picture, byte[] binaryData)
        {
            if (picture == null)
                throw new ArgumentNullException(nameof(picture));

            var pictureBinary = picture.PictureBinary;

            var isNew = pictureBinary == null;

            if (isNew)
                pictureBinary = new PictureBinary
                {
                    PictureId = picture.Id
                };

            pictureBinary.BinaryData = binaryData;

            if (isNew)
                _pictureBinaryRepository.Insert(pictureBinary);
            else
                _pictureBinaryRepository.Update(pictureBinary);

            return pictureBinary;
        }

        #endregion

        #region Getting picture local path/URL methods

        /// <summary>
        /// Gets the loaded picture binary depending on picture storage settings
        /// </summary>
        /// <param name="picture">Picture</param>
        /// <returns>Picture binary</returns>
        public virtual byte[] LoadPictureBinary(Picture picture)
        {
            return LoadPictureBinary(picture, StoreInDb);
        }



        /// <summary>
        /// Gets the default picture URL
        /// </summary>
        /// <param name="targetSize">The target picture size (longest side)</param>
        /// <param name="defaultPictureType">Default picture type</param>
        /// <param name="storeLocation">Store location URL; null to use determine the current store location automatically</param>
        /// <returns>Picture URL</returns>
        public virtual string GetDefaultPictureUrl(int targetSize = 0,
            PictureType defaultPictureType = PictureType.Entity,
            string storeLocation = null)
        {
            string defaultImageFileName;
            switch (defaultPictureType)
            {
                case PictureType.Avatar:
                    defaultImageFileName = _settingService.GetSettingByKey("Media.User.DefaultAvatarImageName", ResearchMediaDefaults.DefaultAvatarFileName);
                    break;
                case PictureType.Pdf:
                    defaultImageFileName = ResearchMediaDefaults.DefaultPdfFileName;
                    break;
                case PictureType.Entity:
                default:
                    defaultImageFileName = _settingService.GetSettingByKey("Media.DefaultImageName", ResearchMediaDefaults.DefaultImageFileName);
                    break;
            }
            var filePath = GetPictureLocalPath(defaultImageFileName);
            if (!_fileProvider.FileExists(filePath))
            {
                return "";
            }


            if (targetSize == 0)
            {
                //var url = (!string.IsNullOrEmpty(storeLocation)
                //                 ? storeLocation
                //                 : _webHelper.GetStoreLocation())
                //                 + 
                //                 "images/" + defaultImageFileName;
                var url = "images/" + defaultImageFileName;
                return url;
            }
            else
            {
                var fileExtension = _fileProvider.GetFileExtension(filePath);
                var thumbFileName = $"{_fileProvider.GetFileNameWithoutExtension(filePath)}_{targetSize}{fileExtension}";
                var thumbFilePath = GetThumbLocalPath(thumbFileName);
                if (!GeneratedThumbExists(thumbFilePath, thumbFileName))
                {
                    using (var image = Image.Load(filePath, out var imageFormat))
                    {
                        image.Mutate(imageProcess => imageProcess.Resize(new ResizeOptions
                        {
                            Mode = ResizeMode.Max,
                            Size = CalculateDimensions(image.Size(), targetSize)
                        }));
                        var pictureBinary = EncodeImage(image, imageFormat);
                        SaveThumb(thumbFilePath, thumbFileName, imageFormat.DefaultMimeType, pictureBinary);
                    }
                }
                var url = GetThumbUrl(thumbFileName, storeLocation);
                return url;
            }
        }

        /// <summary>
        /// Gets the default picture URL
        /// </summary>
        /// <param name="targetSize">The target picture size (longest side)</param>
        /// <param name="defaultPictureType">Default picture type</param>
        /// <param name="storeLocation">Store location URL; null to use determine the current store location automatically</param>
        /// <returns>Picture URL</returns>
        public virtual string GetDefaultPdfUrl()
        {
            return "files/" + ResearchMediaDefaults.DefaultPdfFileName;
        }
        /// <summary>
        /// Get a picture URL
        /// </summary>
        /// <param name="pictureId">Picture identifier</param>
        /// <param name="targetSize">The target picture size (longest side)</param>
        /// <param name="showDefaultPicture">A value indicating whether the default picture is shown</param>
        /// <param name="storeLocation">Store location URL; null to use determine the current store location automatically</param>
        /// <param name="defaultPictureType">Default picture type</param>
        /// <returns>Picture URL</returns>
        public virtual string GetPictureUrl(int pictureId,
            int targetSize = 0,
            bool showDefaultPicture = true,
            string storeLocation = null,
            PictureType defaultPictureType = PictureType.Entity)
        {
            var picture = GetPictureById(pictureId);
            return GetPictureUrl(picture, targetSize, showDefaultPicture, storeLocation, defaultPictureType);
        }

        /// <summary>
        /// Get a picture URL
        /// </summary>
        /// <param name="picture">Picture instance</param>
        /// <param name="targetSize">The target picture size (longest side)</param>
        /// <param name="showDefaultPicture">A value indicating whether the default picture is shown</param>
        /// <param name="storeLocation">Store location URL; null to use determine the current store location automatically</param>
        /// <param name="defaultPictureType">Default picture type</param>
        /// <returns>Picture URL</returns>
        public virtual string GetPictureUrl(Picture picture,
            int targetSize = 0,
            bool showDefaultPicture = true,
            string storeLocation = null,
            PictureType defaultPictureType = PictureType.Entity)
        {
            var url = string.Empty;
            byte[] pictureBinary = null;
            if (picture != null)
                pictureBinary = LoadPictureBinary(picture);
            if (picture == null || pictureBinary == null || pictureBinary.Length == 0)
            {
                if (showDefaultPicture)
                {
                    url = GetDefaultPictureUrl(targetSize, defaultPictureType, storeLocation);
                }
                return url;
            }

            if (picture.IsNew)
            {
                DeletePictureThumbs(picture);

                //we do not validate picture binary here to ensure that no exception ("Parameter is not valid") will be thrown
                picture = UpdatePicture(picture.Id,
                    pictureBinary,
                    picture.MimeType,
                    picture.SeoFilename,
                    picture.AltAttribute,
                    picture.TitleAttribute,
                    false,
                    false);
            }

            var seoFileName = picture.SeoFilename; // = GetPictureSeName(picture.SeoFilename); //just for sure

            var lastPart = GetFileExtensionFromMimeType(picture.MimeType);
            string thumbFileName;
            if (targetSize == 0)
            {
                thumbFileName = !string.IsNullOrEmpty(seoFileName)
                    ? $"{picture.Id:0000000}_{seoFileName}.{lastPart}"
                    : $"{picture.Id:0000000}.{lastPart}";
            }
            else
            {
                thumbFileName = !string.IsNullOrEmpty(seoFileName)
                    ? $"{picture.Id:0000000}_{seoFileName}_{targetSize}.{lastPart}"
                    : $"{picture.Id:0000000}_{targetSize}.{lastPart}";
            }
            var thumbFilePath = GetThumbLocalPath(thumbFileName);

            //the named mutex helps to avoid creating the same files in different threads,
            //and does not decrease performance significantly, because the code is blocked only for the specific file.
            using (var mutex = new Mutex(false, thumbFileName))
            {
                if (!GeneratedThumbExists(thumbFilePath, thumbFileName))
                {
                    mutex.WaitOne();

                    //check, if the file was created, while we were waiting for the release of the mutex.
                    if (!GeneratedThumbExists(thumbFilePath, thumbFileName))
                    {
                        byte[] pictureBinaryResized;
                        if (targetSize != 0)
                        {
                            //resizing required
                            using (var image = Image.Load(pictureBinary, out var imageFormat))
                            {
                                image.Mutate(imageProcess => imageProcess.Resize(new ResizeOptions
                                {
                                    Mode = ResizeMode.Max,
                                    Size = CalculateDimensions(image.Size(), targetSize)
                                }));

                                pictureBinaryResized = EncodeImage(image, imageFormat);
                            }
                        }
                        else
                        {
                            //create a copy of pictureBinary
                            pictureBinaryResized = pictureBinary.ToArray();
                        }

                        SaveThumb(thumbFilePath, thumbFileName, picture.MimeType, pictureBinaryResized);
                    }

                    mutex.ReleaseMutex();
                }

            }
            url = GetThumbUrl(thumbFileName, storeLocation);
            return url;
        }

        /// <summary>
        /// Get a picture URL
        /// </summary>
        /// <param name="picture">Picture instance</param>
        /// <param name="targetSize">The target picture size (longest side)</param>
        /// <param name="showDefaultPicture">A value indicating whether the default picture is shown</param>
        /// <param name="storeLocation">Store location URL; null to use determine the current store location automatically</param>
        /// <param name="defaultPictureType">Default picture type</param>
        /// <returns>Picture URL</returns>
        public virtual string GetPdfUrl(Picture picture,
            int targetSize = 0,
            PictureType defaultPictureType = PictureType.Entity)
        {
            var url = string.Empty;
            byte[] pictureBinary = null;
            if (picture != null)
                pictureBinary = LoadPictureBinary(picture);
            if (picture == null || pictureBinary == null || pictureBinary.Length == 0)
            {
                return url;
            }

            if (picture.IsNew)
            {
                DeletePictureThumbs(picture);

                //we do not validate picture binary here to ensure that no exception ("Parameter is not valid") will be thrown
                picture = UpdatePicture(picture.Id,
                    pictureBinary,
                    picture.MimeType,
                    picture.SeoFilename,
                    picture.AltAttribute,
                    picture.TitleAttribute,
                    false,
                    false);
            }

            var seoFileName = picture.SeoFilename; // = GetPictureSeName(picture.SeoFilename); //just for sure

            var lastPart = GetFileExtensionFromMimeType(picture.MimeType);
            string thumbFileName;
            if (targetSize == 0)
            {
                thumbFileName = !string.IsNullOrEmpty(seoFileName)
                    ? $"{picture.Id:0000000}_{seoFileName}.{lastPart}"
                    : $"{picture.Id:0000000}.{lastPart}";
            }
            else
            {
                thumbFileName = !string.IsNullOrEmpty(seoFileName)
                    ? $"{picture.Id:0000000}_{seoFileName}_{targetSize}.{lastPart}"
                    : $"{picture.Id:0000000}_{targetSize}.{lastPart}";
            }
            var thumbFilePath = GetThumbLocalPdfPath(thumbFileName);

            //the named mutex helps to avoid creating the same files in different threads,
            //and does not decrease performance significantly, because the code is blocked only for the specific file.
            using (var mutex = new Mutex(false, thumbFileName))
            {
                if (!GeneratedThumbExists(thumbFilePath, thumbFileName))
                {
                    mutex.WaitOne();

                    //check, if the file was created, while we were waiting for the release of the mutex.
                    if (!GeneratedThumbExists(thumbFilePath, thumbFileName))
                    {
                        byte[] pictureBinaryResized = pictureBinary.ToArray();

                        SavePdfThumb(thumbFilePath, thumbFileName, picture.MimeType, pictureBinaryResized);
                    }

                    mutex.ReleaseMutex();
                }

            }
            url = GetThumbPdfUrl(thumbFileName);
            return url;
        }

        /// <summary>
        /// Get a picture local path
        /// </summary>
        /// <param name="picture">Picture instance</param>
        /// <param name="targetSize">The target picture size (longest side)</param>
        /// <param name="showDefaultPicture">A value indicating whether the default picture is shown</param>
        /// <returns></returns>
        public virtual string GetThumbLocalPath(Picture picture, int targetSize = 0, bool showDefaultPicture = true)
        {
            var url = GetPictureUrl(picture, targetSize, showDefaultPicture);
            if (string.IsNullOrEmpty(url))
                return string.Empty;

            return GetThumbLocalPath(_fileProvider.GetFileName(url));
        }

        #endregion

        #region CRUD methods

        /// <summary>
        /// Gets a picture
        /// </summary>
        /// <param name="pictureId">Picture identifier</param>
        /// <returns>Picture</returns>
        public virtual Picture GetPictureById(int pictureId)
        {
            if (pictureId == 0)
                return null;

            return _pictureRepository.GetById(pictureId);
        }

        /// <summary>
        /// Deletes a picture
        /// </summary>
        /// <param name="picture">Picture</param>
        public virtual void DeletePicture(Picture picture)
        {
            if (picture == null)
                throw new ArgumentNullException(nameof(picture));

            //delete thumbs
            DeletePictureThumbs(picture);

            //delete from file system
            if (!StoreInDb)
                DeletePictureOnFileSystem(picture);

            //delete from database
            _pictureRepository.Delete(picture);

            //event notification
            _eventPublisher.EntityDeleted(picture);
        }

        /// <summary>
        /// Gets a collection of pictures
        /// </summary>
        /// <param name="pageIndex">Current page</param>
        /// <param name="pageSize">Items on each page</param>
        /// <returns>Paged list of pictures</returns>
        public virtual IPagedList<Picture> GetPictures(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = from p in _pictureRepository.Table
                        orderby p.Id descending
                        select p;
            var pics = new PagedList<Picture>(query, pageIndex, pageSize);
            return pics;
        }

        /// <summary>
        /// Inserts a picture
        /// </summary>
        /// <param name="pictureBinary">The picture binary</param>
        /// <param name="mimeType">The picture MIME type</param>
        /// <param name="seoFilename">The SEO filename</param>
        /// <param name="altAttribute">"alt" attribute for "img" HTML element</param>
        /// <param name="titleAttribute">"title" attribute for "img" HTML element</param>
        /// <param name="isNew">A value indicating whether the picture is new</param>
        /// <param name="validateBinary">A value indicating whether to validated provided picture binary</param>
        /// <returns>Picture</returns>
        public virtual Picture InsertPicture(byte[] pictureBinary, string mimeType, string seoFilename,
            string altAttribute = null, string titleAttribute = null,
            bool isNew = true, bool validateBinary = true)
        {
            mimeType = CommonHelper.EnsureNotNull(mimeType);
            mimeType = CommonHelper.EnsureMaximumLength(mimeType, 20);

            seoFilename = CommonHelper.EnsureMaximumLength(seoFilename, 100);

            if (validateBinary)
                pictureBinary = ValidatePicture(pictureBinary, mimeType);

            var picture = new Picture
            {
                MimeType = mimeType,
                SeoFilename = seoFilename,
                AltAttribute = altAttribute,
                TitleAttribute = titleAttribute,
                IsNew = isNew
            };
            _pictureRepository.Insert(picture);
            UpdatePictureBinary(picture, StoreInDb ? pictureBinary : new byte[0]);

            if (!StoreInDb)
                SavePictureInFile(picture.Id, pictureBinary, mimeType);

            //event notification
            _eventPublisher.EntityInserted(picture);

            return picture;
        }

        /// <summary>
        /// Updates the picture
        /// </summary>
        /// <param name="pictureId">The picture identifier</param>
        /// <param name="pictureBinary">The picture binary</param>
        /// <param name="mimeType">The picture MIME type</param>
        /// <param name="seoFilename">The SEO filename</param>
        /// <param name="altAttribute">"alt" attribute for "img" HTML element</param>
        /// <param name="titleAttribute">"title" attribute for "img" HTML element</param>
        /// <param name="isNew">A value indicating whether the picture is new</param>
        /// <param name="validateBinary">A value indicating whether to validated provided picture binary</param>
        /// <returns>Picture</returns>
        public virtual Picture UpdatePicture(int pictureId, byte[] pictureBinary, string mimeType,
            string seoFilename, string altAttribute = null, string titleAttribute = null,
            bool isNew = true, bool validateBinary = true)
        {
            mimeType = CommonHelper.EnsureNotNull(mimeType);
            mimeType = CommonHelper.EnsureMaximumLength(mimeType, 20);

            seoFilename = CommonHelper.EnsureMaximumLength(seoFilename, 100);

            if (validateBinary)
                pictureBinary = ValidatePicture(pictureBinary, mimeType);

            var picture = GetPictureById(pictureId);
            if (picture == null)
                return null;

            //delete old thumbs if a picture has been changed
            if (seoFilename != picture.SeoFilename)
                DeletePictureThumbs(picture);

            picture.MimeType = mimeType;
            picture.SeoFilename = seoFilename;
            picture.AltAttribute = altAttribute;
            picture.TitleAttribute = titleAttribute;
            picture.IsNew = isNew;

            _pictureRepository.Update(picture);
            UpdatePictureBinary(picture, StoreInDb ? pictureBinary : new byte[0]);

            if (!StoreInDb)
                SavePictureInFile(picture.Id, pictureBinary, mimeType);

            //event notification
            _eventPublisher.EntityUpdated(picture);

            return picture;
        }

        /// <summary>
        /// Updates a SEO filename of a picture
        /// </summary>
        /// <param name="pictureId">The picture identifier</param>
        /// <param name="seoFilename">The SEO filename</param>
        /// <returns>Picture</returns>
        public virtual Picture SetSeoFilename(int pictureId, string seoFilename)
        {
            var picture = GetPictureById(pictureId);
            if (picture == null)
                throw new ArgumentException("No picture found with the specified id");

            //update if it has been changed
            if (seoFilename != picture.SeoFilename)
            {
                //update picture
                picture = UpdatePicture(picture.Id,
                    LoadPictureBinary(picture),
                    picture.MimeType,
                    seoFilename,
                    picture.AltAttribute,
                    picture.TitleAttribute,
                    true,
                    false);
            }
            return picture;
        }

        /// <summary>
        /// Validates input picture dimensions
        /// </summary>
        /// <param name="pictureBinary">Picture binary</param>
        /// <param name="mimeType">MIME type</param>
        /// <returns>Picture binary or throws an exception</returns>
        public virtual byte[] ValidatePicture(byte[] pictureBinary, string mimeType)
        {
            using (var image = Image.Load(pictureBinary, out var imageFormat))
            {
                //resize the image in accordance with the maximum size
                if (Math.Max(image.Height, image.Width) > _mediaSettings.MaximumImageSize)
                {
                    image.Mutate(imageProcess => imageProcess.Resize(new ResizeOptions
                    {
                        Mode = ResizeMode.Max,
                        Size = new Size(_mediaSettings.MaximumImageSize)
                    }));
                }

                return EncodeImage(image, imageFormat);
            }
        }

        /// <summary>
        /// Get pictures hashes
        /// </summary>
        /// <param name="picturesIds">Pictures Ids</param>
        /// <returns></returns>
        public IDictionary<int, string> GetPicturesHash(int[] picturesIds)
        {
            var supportedLengthOfBinaryHash = _dataProvider.SupportedLengthOfBinaryHash;
            if (supportedLengthOfBinaryHash == 0 || !picturesIds.Any())
                return new Dictionary<int, string>();

            const string strCommand = "SELECT [Id] as [PictureId], HASHBYTES('sha1', substring([PictureBinary], 0, {0})) as [Hash] FROM [Picture] where [Id] in ({1})";
            return _dbContext
                .QueryFromSql<PictureHashItem>(string.Format(strCommand, supportedLengthOfBinaryHash, picturesIds.Select(p => p.ToString()).Aggregate((all, current) => all + ", " + current))).Distinct()
                .ToDictionary(p => p.PictureId, p => BitConverter.ToString(p.Hash).Replace("-", ""));
        }


        public Picture GetPicturesByResearcherId(int researcherId, int recordsToReturn = 0)
        {
            if (researcherId == 0)
                return new Picture();


            var query = from p in _pictureRepository.Table
                        join pp in _researcherRepository.Table on p.Id equals pp.PictureId
                        where pp.Id == researcherId
                        select p;

            if (recordsToReturn > 0)
                query = query.Take(recordsToReturn);

            var pic = query.FirstOrDefault();
            return pic;
        }

        public Picture GetPicturesByProjectProgressId(int projectProgressId, int recordsToReturn = 0)
        {
            if (projectProgressId == 0)
                return new Picture();


            var query = from p in _pictureRepository.Table
                        join pp in _projectProgressRepository.Table on p.Id equals pp.ProjectUploadId
                        where pp.Id == projectProgressId
                        select p;

            if (recordsToReturn > 0)
                query = query.Take(recordsToReturn);

            var pic = query.FirstOrDefault();
            return pic;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the images should be stored in data base.
        /// </summary>
        public virtual bool StoreInDb
        {
            get
            {
                return _settingService.GetSettingByKey("Media.Images.StoreInDB", true);
            }
            set
            {
                //check whether it's a new value
                if (this.StoreInDb == value)
                    return;

                //save the new setting value
                _settingService.SetSetting("Media.Images.StoreInDB", value);

                var pageIndex = 0;
                const int pageSize = 400;
                try
                {
                    while (true)
                    {
                        var pictures = GetPictures(pageIndex, pageSize);
                        pageIndex++;

                        //all pictures converted?
                        if (!pictures.Any())
                            break;

                        foreach (var picture in pictures)
                        {
                            var pictureBinary = LoadPictureBinary(picture, !value);

                            //we used the code below before. but it's too slow
                            //let's do it manually (uncommented code) - copy some logic from "UpdatePicture" method
                            /*just update a picture (all required logic is in "UpdatePicture" method)
                            we do not validate picture binary here to ensure that no exception ("Parameter is not valid") will be thrown when "moving" pictures
                            UpdatePicture(picture.Id,
                                          pictureBinary,
                                          picture.MimeType,
                                          picture.SeoFilename,
                                          true,
                                          false);*/
                            if (value)
                                //delete from file system. now it's in the database
                                DeletePictureOnFileSystem(picture);
                            else
                                //now on file system
                                SavePictureInFile(picture.Id, pictureBinary, picture.MimeType);
                            //update appropriate properties
                            UpdatePictureBinary(picture, value ? pictureBinary : new byte[0]);
                            picture.IsNew = true;
                            //raise event?
                            //_eventPublisher.EntityUpdated(picture);
                        }
                        //save all at once
                        _pictureRepository.Update(pictures);
                        //detach them in order to release memory
                        foreach (var picture in pictures)
                        {
                            _dbContext.Detach(picture);
                        }
                    }
                }
                finally
                {
                }
            }
        }

        #endregion
    }
}