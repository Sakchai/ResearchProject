using Research.Data;
using Research.Services.Messages;
using Research.Web.Extensions;
using Research.Web.Models.Factories;
using Research.Web.Models.Messages;
using System;
using System.Linq;

namespace Research.Web.Factories
{
    /// <summary>
    /// Represents the message template model factory implementation
    /// </summary>
    public partial class MessageTemplateModelFactory : IMessageTemplateModelFactory
    {
        #region Fields

        private readonly IBaseAdminModelFactory _baseAdminModelFactory;
        private readonly IMessageTemplateService _messageTemplateService;
        private readonly IMessageTokenProvider _messageTokenProvider;

        #endregion

        #region Ctor

        public MessageTemplateModelFactory(IBaseAdminModelFactory baseAdminModelFactory,
            IMessageTemplateService messageTemplateService,
            IMessageTokenProvider messageTokenProvider)
        {
            this._baseAdminModelFactory = baseAdminModelFactory;
            this._messageTemplateService = messageTemplateService;
            this._messageTokenProvider = messageTokenProvider;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare message template search model
        /// </summary>
        /// <param name="searchModel">Message template search model</param>
        /// <returns>Message template search model</returns>
        public virtual MessageTemplateSearchModel PrepareMessageTemplateSearchModel(MessageTemplateSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare available stores

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare paged message template list model
        /// </summary>
        /// <param name="searchModel">Message template search model</param>
        /// <returns>Message template list model</returns>
        public virtual MessageTemplateListModel PrepareMessageTemplateListModel(MessageTemplateSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get message templates
            var messageTemplates = _messageTemplateService.GetAllMessageTemplates(storeId: 0);
            
            //prepare list model
            var model = new MessageTemplateListModel
            {
                Data = messageTemplates.PaginationByRequestModel(searchModel).Select(messageTemplate =>
                {
                    //fill in model values from the entity
                    var messageTemplateModel = messageTemplate.ToModel<MessageTemplateModel>();

                   // messageTemplateModel.ListOfStores = string.Join(", ", storeNames);

                    return messageTemplateModel;
                }),
                Total = messageTemplates.Count
            };

            return model;
        }

        /// <summary>
        /// Prepare message template model
        /// </summary>
        /// <param name="model">Message template model</param>
        /// <param name="messageTemplate">Message template</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>Message template model</returns>
        public virtual MessageTemplateModel PrepareMessageTemplateModel(MessageTemplateModel model,
            MessageTemplate messageTemplate, bool excludeProperties = false)
        {
            //Action<MessageTemplateLocalizedModel, int> localizedModelConfiguration = null;

            if (messageTemplate != null)
            {
                //fill in model values from the entity
                model = model ?? messageTemplate.ToModel<MessageTemplateModel>();

                //define localized model configuration action
                model.BccEmailAddresses = messageTemplate.BccEmailAddresses;
                model.Subject = messageTemplate.Subject;
                model.Body = messageTemplate.Body;
                model.EmailAccountId = messageTemplate.EmailAccountId;

                //prepare available email accounts
                //_baseAdminModelFactory.PrepareEmailAccounts();

            }

            model.SendImmediately = !model.DelayBeforeSend.HasValue;
            model.HasAttachedDownload = model.AttachedDownloadId > 0;

            var allowedTokens = string.Join(", ", _messageTokenProvider.GetListOfAllowedTokens(messageTemplate.GetTokenGroups()));
            model.AllowedTokens = $"{allowedTokens}{Environment.NewLine}{Environment.NewLine}" +
                $"ConditionalStatement{Environment.NewLine}";

            //prepare available email accounts
            _baseAdminModelFactory.PrepareEmailAccounts(model.AvailableEmailAccounts);

            return model;
        }

        /// <summary>
        /// Prepare test message template model
        /// </summary>
        /// <param name="model">Test message template model</param>
        /// <param name="messageTemplate">Message template</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Test message template model</returns>
        public virtual TestMessageTemplateModel PrepareTestMessageTemplateModel(TestMessageTemplateModel model,
            MessageTemplate messageTemplate, int languageId)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (messageTemplate == null)
                throw new ArgumentNullException(nameof(messageTemplate));

            model.Id = messageTemplate.Id;
            model.LanguageId = languageId;

            //filter tokens to the current template
            var subject = messageTemplate.Subject;
            var body = messageTemplate.Body;
            model.Tokens = _messageTokenProvider.GetListOfAllowedTokens()
                .Where(token => subject.Contains(token) || body.Contains(token)).ToList();

            return model;
        }

        #endregion
    }
}