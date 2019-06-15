using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Research.Core;
using Research.Core.Domain.Common;
using Research.Core.Domain.Messages;
using Research.Data;
using Research.Enum;
using Research.Services.Events;

namespace Research.Services.Messages
{
    /// <summary>
    /// Workflow message service
    /// </summary>
    public partial class WorkflowMessageService : IWorkflowMessageService
    {
        #region Fields

        private readonly CommonSettings _commonSettings;
        private readonly EmailAccountSettings _emailAccountSettings;
        private readonly IUserService _userService;
        private readonly IEmailAccountService _emailAccountService;
        private readonly IEventPublisher _eventPublisher;
        private readonly IMessageTemplateService _messageTemplateService;
        private readonly IMessageTokenProvider _messageTokenProvider;
        private readonly IQueuedEmailService _queuedEmailService;
        private readonly ITokenizer _tokenizer;

        #endregion

        #region Ctor

        public WorkflowMessageService(CommonSettings commonSettings,
            EmailAccountSettings emailAccountSettings,
            IUserService userService,
            IEmailAccountService emailAccountService,
            IEventPublisher eventPublisher,
            IMessageTemplateService messageTemplateService,
            IMessageTokenProvider messageTokenProvider,
            IQueuedEmailService queuedEmailService,
            ITokenizer tokenizer)
        {
            this._commonSettings = commonSettings;
            this._emailAccountSettings = emailAccountSettings;
            this._userService = userService;
            this._emailAccountService = emailAccountService;
            this._eventPublisher = eventPublisher;
            this._messageTemplateService = messageTemplateService;
            this._messageTokenProvider = messageTokenProvider;
            this._queuedEmailService = queuedEmailService;
            this._tokenizer = tokenizer;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Get active message templates by the name
        /// </summary>
        /// <param name="messageTemplateName">Message template name</param>
        /// <param name="storeId">Store identifier</param>
        /// <returns>List of message templates</returns>
        protected virtual IList<MessageTemplate> GetActiveMessageTemplates(string messageTemplateName, int storeId)
        {
            //get message templates by the name
            var messageTemplates = _messageTemplateService.GetMessageTemplatesByName(messageTemplateName, storeId);

            //no template found
            if (!messageTemplates?.Any() ?? true)
                return new List<MessageTemplate>();

            //filter active templates
            messageTemplates = messageTemplates.Where(messageTemplate => messageTemplate.IsActive).ToList();

            return messageTemplates;
        }

        /// <summary>
        /// Get EmailAccount to use with a message templates
        /// </summary>
        /// <param name="messageTemplate">Message template</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>EmailAccount</returns>
        protected virtual EmailAccount GetEmailAccountOfMessageTemplate(MessageTemplate messageTemplate, int languageId)
        {
            //var emailAccountId = _localizationService.GetLocalized(messageTemplate, mt => mt.EmailAccountId, languageId);
            //some 0 validation (for localizable "Email account" dropdownlist which saves 0 if "Standard" value is chosen)
            //if (emailAccountId == 0)
            //    emailAccountId = messageTemplate.EmailAccountId;
            var emailAccountId = messageTemplate.EmailAccountId;

            var emailAccount = (_emailAccountService.GetEmailAccountById(emailAccountId) ?? _emailAccountService.GetEmailAccountById(_emailAccountSettings.DefaultEmailAccountId)) ??
                               _emailAccountService.GetAllEmailAccounts().FirstOrDefault();
            return emailAccount;
        }

        ///// <summary>
        ///// Ensure language is active
        ///// </summary>
        ///// <param name="languageId">Language identifier</param>
        ///// <param name="storeId">Store identifier</param>
        ///// <returns>Return a value language identifier</returns>
        //protected virtual int EnsureLanguageIsActive(int languageId, int storeId)
        //{
        //    //load language by specified ID
        //    var language = _languageService.GetLanguageById(languageId);

        //    if (language == null || !language.Published)
        //    {
        //        //load any language from the specified store
        //        language = _languageService.GetAllLanguages(storeId: storeId).FirstOrDefault();
        //    }

        //    if (language == null || !language.Published)
        //    {
        //        //load any language
        //        language = _languageService.GetAllLanguages().FirstOrDefault();
        //    }

        //    if (language == null)
        //        throw new Exception("No active language could be loaded");

        //    return language.Id;
        //}

        #endregion

        #region Methods

        #region User workflow

        /// <summary>
        /// Sends 'New user' notification message to a store owner
        /// </summary>
        /// <param name="user">User instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendUserRegisteredNotificationMessage(User user, int languageId)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            // var store = _storeContext.CurrentStore;
            // languageId = EnsureLanguageIsActive(languageId, store.Id);
            //var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.UserRegisteredNotification, store.Id);
            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.UserRegisteredNotification, 0);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddUserTokens(commonTokens, user);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                //_messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);
                _messageTokenProvider.AddUserTokens(tokens, user);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = emailAccount.Email;
                var toName = emailAccount.DisplayName;

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
            }).ToList();
        }

        /// <summary>
        /// Sends a welcome message to a user
        /// </summary>
        /// <param name="user">User instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendUserWelcomeMessage(User user, int languageId)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            //var store = _storeContext.CurrentStore;
            //languageId = EnsureLanguageIsActive(languageId, store.Id);
            //var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.UserWelcomeMessage, store.Id);
            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.UserWelcomeMessage, 0);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddUserTokens(commonTokens, user);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                //_messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);
                _messageTokenProvider.AddUserTokens(tokens, user);
                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = user.Email;
                var toName = _userService.GetUserFullName(user);

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
            }).ToList();
        }

        /// <summary>
        /// Sends an email validation message to a user
        /// </summary>
        /// <param name="user">User instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendUserEmailValidationMessage(User user, int languageId)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            //var store = _storeContext.CurrentStore;
            //languageId = EnsureLanguageIsActive(languageId, store.Id);
            //var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.UserEmailValidationMessage, store.Id);
            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.UserEmailValidationMessage, 0);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            //var commonTokens = new List<Token>();
            //_messageTokenProvider.AddUserTokens(commonTokens, user);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                //var tokens = new List<Token>(commonTokens);
                var tokens = new List<Token>();
                //_messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);
                _messageTokenProvider.AddUserTokens(tokens, user);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = user.Email;
                var toName = _userService.GetUserFullName(user);

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
            }).ToList();
        }

        /// <summary>
        /// Sends an email re-validation message to a user
        /// </summary>
        /// <param name="user">User instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendUserEmailRevalidationMessage(User user, int languageId)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

           // var store = _storeContext.CurrentStore;
           // languageId = EnsureLanguageIsActive(languageId, store.Id);

            //var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.UserEmailRevalidationMessage, store.Id);
            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.UserEmailRevalidationMessage, 0);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddUserTokens(commonTokens, user);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                //_messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);
                _messageTokenProvider.AddUserTokens(tokens, user);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                //email to re-validate
                var toEmail = user.EmailToRevalidate;
                var toName = _userService.GetUserFullName(user);

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
            }).ToList();
        }

        /// <summary>
        /// Sends password recovery message to a user
        /// </summary>
        /// <param name="user">User instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendUserPasswordRecoveryMessage(User user, int languageId)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            //var store = _storeContext.CurrentStore;
            //languageId = EnsureLanguageIsActive(languageId, store.Id);

            //var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.UserPasswordRecoveryMessage, store.Id);
            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.UserPasswordRecoveryMessage, 0);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddUserTokens(commonTokens, user);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                //_messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);
                _messageTokenProvider.AddUserTokens(tokens, user);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = user.Email;
                var toName = _userService.GetUserFullName(user);

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
            }).ToList();
        }

        #endregion


        #region Misc

        /// <summary>
        /// Sends 'New researcher account submitted' message to a store owner
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="researcher">Researcher</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendNewResearcherAccountApplyStoreOwnerNotification(User user, Researcher researcher, int languageId)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (researcher == null)
                throw new ArgumentNullException(nameof(researcher));

            //var store = _storeContext.CurrentStore;
            //languageId = EnsureLanguageIsActive(languageId, store.Id);

            //var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.NewResearcherAccountApplyStoreOwnerNotification, store.Id);
            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.NewResearcherAccountApplyStoreOwnerNotification, 0);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddUserTokens(commonTokens, user);
            _messageTokenProvider.AddResearcherTokens(commonTokens, researcher);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                //_messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);
                _messageTokenProvider.AddUserTokens(tokens, user);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = emailAccount.Email;
                var toName = emailAccount.DisplayName;

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
            }).ToList();
        }

        /// <summary>
        /// Sends 'Researcher information changed' message to a store owner
        /// </summary>
        /// <param name="researcher">Researcher</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendResearcherInformationChangeNotification(Researcher researcher, int languageId)
        {
            if (researcher == null)
                throw new ArgumentNullException(nameof(researcher));

            //var store = _storeContext.CurrentStore;
            //languageId = EnsureLanguageIsActive(languageId, store.Id);

            //var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.ResearcherInformationChangeNotification, store.Id);
            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.ResearcherInformationChangeNotification, 0);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            _messageTokenProvider.AddResearcherTokens(commonTokens, researcher);

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                //_messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);
                _messageTokenProvider.AddResearcherTokens(tokens, researcher);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = emailAccount.Email;
                var toName = emailAccount.DisplayName;

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
            }).ToList();
        }

 
        /// <summary>
        /// Sends "contact us" message
        /// </summary>
        /// <param name="languageId">Message language identifier</param>
        /// <param name="senderEmail">Sender email</param>
        /// <param name="senderName">Sender name</param>
        /// <param name="subject">Email subject. Pass null if you want a message template subject to be used.</param>
        /// <param name="body">Email body</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendContactUsMessage(int languageId, string senderEmail,
            string senderName, string subject, string body)
        {
            //var store = _storeContext.CurrentStore;
            //languageId = EnsureLanguageIsActive(languageId, store.Id);
            //var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.ContactUsMessage, store.Id);
            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.ContactUsMessage, 0);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>
            {
                new Token("ContactUs.SenderEmail", senderEmail),
                new Token("ContactUs.SenderName", senderName),
                new Token("ContactUs.Body", body, true)
            };

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                //chai
                // _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);
                _messageTokenProvider.AddSiteTokens(tokens,  emailAccount);
                string fromEmail;
                string fromName;
                //required for some SMTP servers
                if (_commonSettings.UseSystemEmailForContactUsForm)
                {
                    fromEmail = emailAccount.Email;
                    fromName = emailAccount.DisplayName;
                    body = $"<strong>From</strong>: {WebUtility.HtmlEncode(senderName)} - {WebUtility.HtmlEncode(senderEmail)}<br /><br />{body}";
                }
                else
                {
                    fromEmail = senderEmail;
                    fromName = senderName;
                }

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = emailAccount.Email;
                var toName = emailAccount.DisplayName;

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName,
                    fromEmail: fromEmail,
                    fromName: fromName,
                    subject: subject,
                    replyToEmailAddress: senderEmail,
                    replyToName: senderName);
            }).ToList();
        }

        /// <summary>
        /// Sends "contact researcher" message
        /// </summary>
        /// <param name="researcher">Researcher</param>
        /// <param name="languageId">Message language identifier</param>
        /// <param name="senderEmail">Sender email</param>
        /// <param name="senderName">Sender name</param>
        /// <param name="subject">Email subject. Pass null if you want a message template subject to be used.</param>
        /// <param name="body">Email body</param>
        /// <returns>Queued email identifier</returns>
        public virtual IList<int> SendContactResearcherMessage(Researcher researcher, int languageId, string senderEmail,
            string senderName, string subject, string body)
        {
            if (researcher == null)
                throw new ArgumentNullException(nameof(researcher));

            //var store = _storeContext.CurrentStore;
            //languageId = EnsureLanguageIsActive(languageId, store.Id);

            //var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.ContactResearcherMessage, store.Id);
            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.ContactResearcherMessage, 0);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>
            {
                new Token("ContactUs.SenderEmail", senderEmail),
                new Token("ContactUs.SenderName", senderName),
                new Token("ContactUs.Body", body, true)
            };

            return messageTemplates.Select(messageTemplate =>
            {
                //email account
                var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

                string fromEmail;
                string fromName;
                //required for some SMTP servers
                if (_commonSettings.UseSystemEmailForContactUsForm)
                {
                    fromEmail = emailAccount.Email;
                    fromName = emailAccount.DisplayName;
                    body = $"<strong>From</strong>: {WebUtility.HtmlEncode(senderName)} - {WebUtility.HtmlEncode(senderEmail)}<br /><br />{body}";
                }
                else
                {
                    fromEmail = senderEmail;
                    fromName = senderName;
                }

                var tokens = new List<Token>(commonTokens);
                //_messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);
                _messageTokenProvider.AddResearcherTokens(tokens, researcher);

                //event notification
                _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

                var toEmail = researcher.Email;
                var toName = $"{researcher.FirstName} {researcher.LastName}";

                return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName,
                    fromEmail: fromEmail,
                    fromName: fromName,
                    subject: subject,
                    replyToEmailAddress: senderEmail,
                    replyToName: senderName);
            }).ToList();
        }

        /// <summary>
        /// Sends a test email
        /// </summary>
        /// <param name="messageTemplateId">Message template identifier</param>
        /// <param name="sendToEmail">Send to email</param>
        /// <param name="tokens">Tokens</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual int SendTestEmail(int messageTemplateId, string sendToEmail, List<Token> tokens, int languageId)
        {
            var messageTemplate = _messageTemplateService.GetMessageTemplateById(messageTemplateId);
            if (messageTemplate == null)
                throw new ArgumentException("Template cannot be loaded");

            //email account
            var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

            //event notification
            _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

            return SendNotification(messageTemplate, emailAccount, languageId, tokens, sendToEmail, null);
        }

        /// <summary>
        /// Send notification
        /// </summary>
        /// <param name="messageTemplate">Message template</param>
        /// <param name="emailAccount">Email account</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="tokens">Tokens</param>
        /// <param name="toEmailAddress">Recipient email address</param>
        /// <param name="toName">Recipient name</param>
        /// <param name="attachmentFilePath">Attachment file path</param>
        /// <param name="attachmentFileName">Attachment file name</param>
        /// <param name="replyToEmailAddress">"Reply to" email</param>
        /// <param name="replyToName">"Reply to" name</param>
        /// <param name="fromEmail">Sender email. If specified, then it overrides passed "emailAccount" details</param>
        /// <param name="fromName">Sender name. If specified, then it overrides passed "emailAccount" details</param>
        /// <param name="subject">Subject. If specified, then it overrides subject of a message template</param>
        /// <returns>Queued email identifier</returns>
        public virtual int SendNotification(MessageTemplate messageTemplate,
            EmailAccount emailAccount, int languageId, IEnumerable<Token> tokens,
            string toEmailAddress, string toName,
            string attachmentFilePath = null, string attachmentFileName = null,
            string replyToEmailAddress = null, string replyToName = null,
            string fromEmail = null, string fromName = null, string subject = null)
        {
            if (messageTemplate == null)
                throw new ArgumentNullException(nameof(messageTemplate));

            if (emailAccount == null)
                throw new ArgumentNullException(nameof(emailAccount));

            //retrieve localized message template data
            //var bcc = _localizationService.GetLocalized(messageTemplate, mt => mt.BccEmailAddresses, languageId);
            //if (string.IsNullOrEmpty(subject))
            //    subject = _localizationService.GetLocalized(messageTemplate, mt => mt.Subject, languageId);
            //var body = _localizationService.GetLocalized(messageTemplate, mt => mt.Body, languageId);
            var bcc = messageTemplate.BccEmailAddresses;
            subject = messageTemplate.Subject;
            var body = messageTemplate.Body;
            //Replace subject and body tokens 
            var subjectReplaced = _tokenizer.Replace(subject, tokens, false);
            var bodyReplaced = _tokenizer.Replace(body, tokens, true);

            //limit name length
            toName = CommonHelper.EnsureMaximumLength(toName, 300);

            var email = new QueuedEmail
            {
                Priority = QueuedEmailPriority.High,
                From = !string.IsNullOrEmpty(fromEmail) ? fromEmail : emailAccount.Email,
                FromName = !string.IsNullOrEmpty(fromName) ? fromName : emailAccount.DisplayName,
                To = toEmailAddress,
                ToName = toName,
                ReplyTo = replyToEmailAddress,
                ReplyToName = replyToName,
                CC = string.Empty,
                Bcc = bcc,
                Subject = subjectReplaced,
                Body = bodyReplaced,
                AttachmentFilePath = attachmentFilePath,
                AttachmentFileName = attachmentFileName,
                AttachedDownloadId = messageTemplate.AttachedDownloadId,
                CreatedOnUtc = DateTime.UtcNow,
                EmailAccountId = emailAccount.Id,
                DontSendBeforeDateUtc = !messageTemplate.DelayBeforeSend.HasValue ? null
                    : (DateTime?)(DateTime.UtcNow + TimeSpan.FromHours(messageTemplate.DelayPeriod.ToHours(messageTemplate.DelayBeforeSend.Value)))
            };

            _queuedEmailService.InsertQueuedEmail(email);
            return email.Id;
        }

        #endregion

        #endregion
    }
}