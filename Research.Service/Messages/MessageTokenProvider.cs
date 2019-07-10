using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Research.Core;
using Research.Core.Domain;
using Research.Core.Domain.Users;
using Research.Data;
using Research.Infrastructure;
using Research.Services.Common;
using Research.Services.Events;
using Research.Services.Logging;
using Research.Services.Media;

namespace Research.Services.Messages
{
    /// <summary>
    /// Message token provider
    /// </summary>
    public partial class MessageTokenProvider : IMessageTokenProvider
    {
        #region Fields

        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly IUserService _userService;
       // private readonly IDownloadService _downloadService;
        private readonly IEventPublisher _eventPublisher;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IWorkContext _workContext;
        private readonly SiteInformationSettings _siteInformationSettings;
        private Dictionary<string, IEnumerable<string>> _allowedTokens;
        private readonly IGenericAttributeService _genericAttributeService;

        #endregion

        #region Ctor

        public MessageTokenProvider(IActionContextAccessor actionContextAccessor,
            IUserService userService,
        //    IDownloadService downloadService,
            IEventPublisher eventPublisher,
            IUrlHelperFactory urlHelperFactory,
            IWorkContext workContext,
            SiteInformationSettings siteInformationSettings,
            IGenericAttributeService genericAttributeService)
        {
            this._actionContextAccessor = actionContextAccessor;
            this._userService = userService;
       //     this._downloadService = downloadService;
            this._eventPublisher = eventPublisher;
            this._urlHelperFactory = urlHelperFactory;
            this._workContext = workContext;
            this._siteInformationSettings = siteInformationSettings;
            this._genericAttributeService = genericAttributeService;
        }

        #endregion

        #region Allowed tokens

        /// <summary>
        /// Get all available tokens by token groups
        /// </summary>
        protected Dictionary<string, IEnumerable<string>> AllowedTokens
        {
            get
            {
                if (_allowedTokens != null)
                    return _allowedTokens;

                _allowedTokens = new Dictionary<string, IEnumerable<string>>();

                //web site tokens
                _allowedTokens.Add(TokenGroupNames.WebSiteTokens, new[]
                {
                    "%WebSite.Email%",
                    "%WebSite.URL%",
                    "%WebSite.Name%",
                    "%WebSite.Address%",
                    "%WebSite.PhoneNumber%",
                    "%Facebook.URL%",
                    "%Twitter.URL%",
                    "%YouTube.URL%",
                    "%GooglePlus.URL%"
                });

                //user tokens
                _allowedTokens.Add(TokenGroupNames.UserTokens, new[]
                {
                    "%User.Email%",
                    "%User.Username%",
                    "%User.FullName%",
                    "%User.FirstName%",
                    "%User.LastName%",
                    "%User.PasswordRecoveryURL%",
                    "%User.AccountActivationURL%",
                    "%User.EmailRevalidationURL%",
                });

  
 
                //researcher tokens
                _allowedTokens.Add(TokenGroupNames.ResearcherTokens, new[]
                {
                    "%Researcher.FirstName%",
                    "%Researcher.LastName%",
                    "%Researcher.Email%",
                });

  
                return _allowedTokens;
            }
        }

        #endregion

        /// <summary>
        /// Generates an absolute URL for the specified store, routeName and route values
        /// </summary>
        /// <param name="storeId">Store identifier; Pass 0 to load URL of the current store</param>
        /// <param name="routeName">The name of the route that is used to generate URL</param>
        /// <param name="routeValues">An object that contains route values</param>
        /// <returns>Generated URL</returns>
        protected virtual string RouteUrl(string routeName = null, string controlName = "",object routeValues = null )
        {
            //try to get a store by the passed identifier
            //var store = _storeService.GetStoreById(storeId) ?? _storeContext.CurrentStore
            //    ?? throw new Exception("No store could be loaded");

            //ensure that the store URL is specified
            //if (string.IsNullOrEmpty(store.Url))
            //    throw new Exception("URL cannot be null");

            //generate a URL with an absolute path
            var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);
            //var url = new PathString(urlHelper.RouteUrl(routeName, routeValues));
            var url = new PathString(urlHelper.Action(routeName,controlName, routeValues));

            //remove the application path from the generated URL if exists
            var pathBase = _actionContextAccessor.ActionContext?.HttpContext?.Request?.PathBase ?? PathString.Empty;
            url.StartsWithSegments(pathBase, out url);

            //string urlValue = string.Format("{0}{1}", _siteInformationSettings.SiteURL, url.ToUriComponent());
            //compose the result
            //return Uri.EscapeUriString(WebUtility.UrlDecode($"{store.Url.TrimEnd('/')}{url}"));
            //return Uri.EscapeUriString(WebUtility.UrlDecode($"{_siteInformationSettings.SiteURL.TrimEnd('/')}{url}"));
            return Uri.EscapeUriString(WebUtility.UrlDecode($"{url}"));
        }


        #region Methods


        /// <summary>
        /// Add user tokens
        /// </summary>
        /// <param name="tokens">List of already added tokens</param>
        /// <param name="user">User</param>
        public virtual void AddUserTokens(IList<Token> tokens, User user)
        {

            tokens.Add(new Token("WebSite.URL", _siteInformationSettings.SiteURL));
            tokens.Add(new Token("WebSite.Name", _siteInformationSettings.SiteName));
            tokens.Add(new Token("WebSite.Address", _siteInformationSettings.SiteAddress));
            tokens.Add(new Token("WebSite.PhoneNumber", _siteInformationSettings.SitePhoneNumber));

            tokens.Add(new Token("User.Email", user.Email));
            tokens.Add(new Token("User.Username", user.UserName));
            tokens.Add(new Token("User.FullName", _userService.GetUserFullName(user)));
            tokens.Add(new Token("User.FirstName", user.FirstName));
            tokens.Add(new Token("User.LastName", user.LastName));

            //string emailRevalidationUrl = GetURL(user, "EmailRevalidationToken", "EmailRevalidation");
            string accountActivationUrl = GetURL(user, "AccountActivationToken", "User", "AccountActivation");
            string passwordRecoveryUrl = GetURL(user, "PasswordRecoveryToken", "User", "PasswordRecoveryConfirm");
           
            tokens.Add(new Token("User.AccountActivationURL", accountActivationUrl, true));
            tokens.Add(new Token("User.PasswordRecoveryURL", passwordRecoveryUrl, true));
            // tokens.Add(new Token("User.EmailRevalidationURL", emailRevalidationUrl, true));

            //event notification
            _eventPublisher.EntityTokensAdded(user, tokens);
        }

        private string GetURL(User user,string tokenName,string controlName, string routeNameValue)
        {
            string url = string.Empty;
            string websiteURL = _siteInformationSettings.SiteURL;
            try
            {
                var userAttribute = _genericAttributeService.GetAttributesForEntityByToken(user.Id, nameof(user), tokenName).OrderByDescending(x => x.Id).FirstOrDefault();
                string attributeValue = userAttribute != null ? userAttribute.Value : string.Empty;
                url = RouteUrl(routeName: routeNameValue, controlName: controlName, routeValues: new { token = attributeValue, email = user.Email });
                
            }
            catch (Exception exc)
            {
                //log error
                var logger = EngineContext.Current.Resolve<ILogger>();
                //we put in to nested try-catch to prevent possible cyclic (if some error occurs)
                try
                {
                    logger.Error(exc.Message, exc);
                }
                catch (Exception)
                {
                    //do nothing
                }
            }
            return $"{websiteURL.TrimEnd('/')}{url}";
            //return url;
        }

        /// <summary>
        /// Add researcher tokens
        /// </summary>
        /// <param name="tokens">List of already added tokens</param>
        /// <param name="researcher">Researcher</param>
        public virtual void AddResearcherTokens(IList<Token> tokens, Researcher researcher)
        {
            tokens.Add(new Token("Researcher.Name", researcher.FirstName));
            tokens.Add(new Token("Researcher.Email", researcher.Email));

            //var researcherAttributesXml = _genericAttributeService.GetAttribute<string>(researcher, ResearchResearcherDefaults.ResearcherAttributes);
            //tokens.Add(new Token("Researcher.ResearcherAttributes", _researcherAttributeFormatter.FormatAttributes(researcherAttributesXml), true));

            //event notification
            _eventPublisher.EntityTokensAdded(researcher, tokens);
        }

        public void AddSiteTokens(IList<Token> tokens, EmailAccount emailAccount)
        {
            if (emailAccount == null)
                throw new ArgumentNullException(nameof(emailAccount));

            var user =_userService.GetUserByEmail(emailAccount.Email);



            tokens.Add(new Token("WebSite.URL", _siteInformationSettings.SiteURL));
            tokens.Add(new Token("WebSite.Name", _siteInformationSettings.SiteName));
            tokens.Add(new Token("WebSite.Address", _siteInformationSettings.SiteAddress));
            tokens.Add(new Token("WebSite.PhoneNumber", _siteInformationSettings.SitePhoneNumber));

            tokens.Add(new Token("User.Email", user.Email));
            tokens.Add(new Token("User.Username", user.UserName));
            tokens.Add(new Token("User.FullName", _userService.GetUserFullName(user)));
            tokens.Add(new Token("User.FirstName", user.FirstName));
            tokens.Add(new Token("User.LastName", user.LastName));

            //string emailRevalidationUrl = GetURL(user, "EmailRevalidationToken", "EmailRevalidation");
            string accountActivationUrl = GetURL(user, "AccountActivationToken", "User", "AccountActivation");
            //string passwordRecoveryUrl = GetURL(user, "PasswordRecoveryToken", "PasswordRecoveryConfirm");

            //tokens.Add(new Token("User.PasswordRecoveryURL", passwordRecoveryUrl, true));
            tokens.Add(new Token("User.AccountActivationURL", accountActivationUrl, true));
            //tokens.Add(new Token("User.EmailRevalidationURL", emailRevalidationUrl, true));

            tokens.Add(new Token("Facebook.URL", _siteInformationSettings.FacebookLink));
            tokens.Add(new Token("Twitter.URL", _siteInformationSettings.TwitterLink));
            tokens.Add(new Token("YouTube.URL", _siteInformationSettings.YoutubeLink));
            tokens.Add(new Token("GooglePlus.URL", _siteInformationSettings.GooglePlusLink));

            //event notification
            _eventPublisher.EntityTokensAdded(emailAccount, tokens);
        }

        public IEnumerable<string> GetListOfAllowedTokens(IEnumerable<string> tokenGroups = null)
        {
            var additionTokens = new AdditionTokensAddedEvent();
            _eventPublisher.Publish(additionTokens);

            var allowedTokens = AllowedTokens.Where(x => tokenGroups == null || tokenGroups.Contains(x.Key))
                .SelectMany(x => x.Value).ToList();

            allowedTokens.AddRange(additionTokens.AdditionTokens);

            return allowedTokens.Distinct();
        }


        #endregion
    }
}