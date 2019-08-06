using Research.Data.Infrastructure;

namespace Research.Core.Domain
{
    /// <summary>
    /// Store information settings
    /// </summary>
    public class SiteInformationSettings : ISettings
    {
        public string SiteName { get; set; }
        public string SiteURL { get; set; }
        public string SiteAddress { get; set; }
        public string SitePhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether web site is closed
        /// </summary>
        public bool SiteClosed { get; set; }

        /// <summary>
        /// Gets or sets a picture identifier of the logo. If 0, then the default one will be used
        /// </summary>
        public int LogoPictureId { get; set; }

        /// <summary>
        /// Gets or sets a default store theme
        /// </summary>
        public string DefaultStoreTheme { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether customers are allowed to select a theme
        /// </summary>
        public bool AllowCustomerToSelectTheme { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether mini profiler should be displayed in public store (used for debugging)
        /// </summary>
        public bool DisplayMiniProfilerInPublicStore { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether mini profiler should be displayed only for users with access to the admin area
        /// </summary>
        public bool DisplayMiniProfilerForAdminOnly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether we should display warnings about the new EU cookie law
        /// </summary>
        public bool DisplayEuCookieLawWarning { get; set; }

        /// <summary>
        /// Gets or sets a value of Facebook page URL of the site
        /// </summary>
        public string FacebookLink { get; set; }

        /// <summary>
        /// Gets or sets a value of Twitter page URL of the site
        /// </summary>
        public string TwitterLink { get; set; }

        /// <summary>
        /// Gets or sets a value of YouTube channel URL of the site
        /// </summary>
        public string YoutubeLink { get; set; }

        /// <summary>
        /// Gets or sets a value of Google+ page URL of the site
        /// </summary>
        public string GooglePlusLink { get; set; }
    }
}
