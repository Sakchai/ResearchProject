﻿using System;
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
using Research.Services.Common;
using Research.Services.Events;
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

 
                //user tokens
                _allowedTokens.Add(TokenGroupNames.UserTokens, new[]
                {
                    "%User.Email%",
                    "%User.Username%",
                    "%User.FullName%",
                    "%User.FirstName%",
                    "%User.LastName%",
                    "%User.VatNumber%",
                    "%User.VatNumberStatus%",
                    "%User.CustomAttributes%",
                    "%User.PasswordRecoveryURL%",
                    "%User.AccountActivationURL%",
                    "%User.EmailRevalidationURL%",
                    "%Wishlist.URLForUser%"
                });

  
 
                //vendor tokens
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
        protected virtual string RouteUrl(int storeId = 0, string routeName = null, object routeValues = null)
        {
            //try to get a store by the passed identifier
            //var store = _storeService.GetStoreById(storeId) ?? _storeContext.CurrentStore
            //    ?? throw new Exception("No store could be loaded");

            //ensure that the store URL is specified
            //if (string.IsNullOrEmpty(store.Url))
            //    throw new Exception("URL cannot be null");

            //generate a URL with an absolute path
            var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);
            var url = new PathString(urlHelper.RouteUrl(routeName, routeValues));

            //remove the application path from the generated URL if exists
            var pathBase = _actionContextAccessor.ActionContext?.HttpContext?.Request?.PathBase ?? PathString.Empty;
            url.StartsWithSegments(pathBase, out url);

            //compose the result
            //return Uri.EscapeUriString(WebUtility.UrlDecode($"{store.Url.TrimEnd('/')}{url}"));
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
            var userAttribute = _genericAttributeService.GetAttributesForEntity(user.Id, "AccountActivationToken").FirstOrDefault();
            tokens.Add(new Token("User.Email", user.Email));
            tokens.Add(new Token("User.Username", user.UserName));
            tokens.Add(new Token("User.FullName", _userService.GetUserFullName(user)));
            tokens.Add(new Token("User.FirstName", user.FirstName));
            tokens.Add(new Token("User.LastName", user.LastName));
            //note: we do not use SEO friendly URLS for these links because we can get errors caused by having .(dot) in the URL (from the email address)
           // var passwordRecoveryUrl = RouteUrl(routeName: "PasswordRecoveryConfirm", routeValues: new { token = user.PasswordRecoveryToken, email = user.Email });
            var accountActivationUrl = RouteUrl(routeName: "AccountActivation", routeValues: new { token = userAttribute.Value, email = user.Email });
           // var emailRevalidationUrl = RouteUrl(routeName: "EmailRevalidation", routeValues: new { token = user.EmailRevalidationToken, email = user.Email });
            //var wishlistUrl = RouteUrl(routeName: "Wishlist", routeValues: new { userGuid = user.UserGuid });
            //tokens.Add(new Token("User.PasswordRecoveryURL", passwordRecoveryUrl, true));
            tokens.Add(new Token("User.AccountActivationURL", accountActivationUrl, true));
            //tokens.Add(new Token("User.EmailRevalidationURL", emailRevalidationUrl, true));
            //tokens.Add(new Token("Wishlist.URLForUser", wishlistUrl, true));

            //event notification
            _eventPublisher.EntityTokensAdded(user, tokens);
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
            tokens.Add(new Token("WebSite.Email", emailAccount.Email));
            tokens.Add(new Token("WebSite.URL", _siteInformationSettings.SiteURL));
            tokens.Add(new Token("WebSite.Name", _siteInformationSettings.SiteName));
            tokens.Add(new Token("WebSite.Address", _siteInformationSettings.SiteAddress));
            tokens.Add(new Token("WebSite.PhoneNumber", _siteInformationSettings.SitePhoneNumber));

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