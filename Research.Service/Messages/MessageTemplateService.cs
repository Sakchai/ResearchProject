﻿using System;
using System.Collections.Generic;
using System.Linq;
using Research.Core.Caching;
using Research.Data;
using Research.Services.Events;

namespace Research.Services.Messages
{
    /// <summary>
    /// Message template service
    /// </summary>
    public partial class MessageTemplateService : IMessageTemplateService
    {
        #region Fields

        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<MessageTemplate> _messageTemplateRepository;
        private readonly string _entityName;

        #endregion

        #region Ctor

        public MessageTemplateService(ICacheManager cacheManager,
            IEventPublisher eventPublisher,
            IRepository<MessageTemplate> messageTemplateRepository)
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._messageTemplateRepository = messageTemplateRepository;
            this._entityName = typeof(MessageTemplate).Name;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete a message template
        /// </summary>
        /// <param name="messageTemplate">Message template</param>
        public virtual void DeleteMessageTemplate(MessageTemplate messageTemplate)
        {
            if (messageTemplate == null)
                throw new ArgumentNullException(nameof(messageTemplate));

            _messageTemplateRepository.Delete(messageTemplate);

            _cacheManager.RemoveByPattern(ResearchMessageDefaults.MessageTemplatesPatternCacheKey);

            //event notification
            _eventPublisher.EntityDeleted(messageTemplate);
        }

        /// <summary>
        /// Inserts a message template
        /// </summary>
        /// <param name="messageTemplate">Message template</param>
        public virtual void InsertMessageTemplate(MessageTemplate messageTemplate)
        {
            if (messageTemplate == null)
                throw new ArgumentNullException(nameof(messageTemplate));

            _messageTemplateRepository.Insert(messageTemplate);

            _cacheManager.RemoveByPattern(ResearchMessageDefaults.MessageTemplatesPatternCacheKey);

            //event notification
            _eventPublisher.EntityInserted(messageTemplate);
        }

        /// <summary>
        /// Updates a message template
        /// </summary>
        /// <param name="messageTemplate">Message template</param>
        public virtual void UpdateMessageTemplate(MessageTemplate messageTemplate)
        {
            if (messageTemplate == null)
                throw new ArgumentNullException(nameof(messageTemplate));

            _messageTemplateRepository.Update(messageTemplate);

            _cacheManager.RemoveByPattern(ResearchMessageDefaults.MessageTemplatesPatternCacheKey);

            //event notification
            _eventPublisher.EntityUpdated(messageTemplate);
        }

        /// <summary>
        /// Gets a message template
        /// </summary>
        /// <param name="messageTemplateId">Message template identifier</param>
        /// <returns>Message template</returns>
        public virtual MessageTemplate GetMessageTemplateById(int messageTemplateId)
        {
            if (messageTemplateId == 0)
                return null;

            return _messageTemplateRepository.GetById(messageTemplateId);
        }

        /// <summary>
        /// Gets message templates by the name
        /// </summary>
        /// <param name="messageTemplateName">Message template name</param>
        /// <param name="storeId">Store identifier; pass null to load all records</param>
        /// <returns>List of message templates</returns>
        public virtual IList<MessageTemplate> GetMessageTemplatesByName(string messageTemplateName, int? storeId = null)
        {
            if (string.IsNullOrWhiteSpace(messageTemplateName))
                throw new ArgumentException(nameof(messageTemplateName));

            var key = string.Format(ResearchMessageDefaults.MessageTemplatesByNameCacheKey, messageTemplateName, storeId ?? 0);
            return _cacheManager.Get(key, () =>
            {
                //get message templates with the passed name
                var templates = _messageTemplateRepository.Table
                    .Where(messageTemplate => messageTemplate.Name.Equals(messageTemplateName))
                    .OrderBy(messageTemplate => messageTemplate.Id).ToList();

                //filter by the store
                //if (storeId.HasValue && storeId.Value > 0)
                //    templates = templates.Where(messageTemplate => _storeMappingService.Authorize(messageTemplate, storeId.Value)).ToList();

                return templates;
            });
        }

        /// <summary>
        /// Gets all message templates
        /// </summary>
        /// <param name="storeId">Store identifier; pass 0 to load all records</param>
        /// <returns>Message template list</returns>
        public virtual IList<MessageTemplate> GetAllMessageTemplates(string subject)
        {
            var key = string.Format(ResearchMessageDefaults.MessageTemplatesAllCacheKey, subject);
            return _cacheManager.Get(key, () =>
            {
                var query = _messageTemplateRepository.Table;
                if (!string.IsNullOrEmpty(subject))
                    query = query.Where(t => t.Subject.Contains(subject));
                
                ////store mapping
                //query = from t in query
                //    join sm in _storeMappingRepository.Table
                //        on new { c1 = t.Id, c2 = _entityName } equals new { c1 = sm.EntityId, c2 = sm.EntityName } into tSm
                //    from sm in tSm.DefaultIfEmpty()
                //    where !t.LimitedToStores || storeId == sm.StoreId
                //    select t;

                query = query.OrderBy(t => t.Name);

                return query.ToList();
            });
        }

        /// <summary>
        /// Create a copy of message template with all depended data
        /// </summary>
        /// <param name="messageTemplate">Message template</param>
        /// <returns>Message template copy</returns>
        public virtual MessageTemplate CopyMessageTemplate(MessageTemplate messageTemplate)
        {
            if (messageTemplate == null)
                throw new ArgumentNullException(nameof(messageTemplate));

            var mtCopy = new MessageTemplate
            {
                Name = messageTemplate.Name,
                BccEmailAddresses = messageTemplate.BccEmailAddresses,
                Subject = messageTemplate.Subject,
                Body = messageTemplate.Body,
                IsActive = messageTemplate.IsActive,
                AttachedDownloadId = messageTemplate.AttachedDownloadId,
                EmailAccountId = messageTemplate.EmailAccountId,
                LimitedToStores = messageTemplate.LimitedToStores,
                DelayBeforeSend = messageTemplate.DelayBeforeSend,
                DelayPeriod = messageTemplate.DelayPeriod
            };

            InsertMessageTemplate(mtCopy);

            //var languages = _languageService.GetAllLanguages(true);

            ////localization
            //foreach (var lang in languages)
            //{
            //    var bccEmailAddresses = _localizationService.GetLocalized(messageTemplate, x => x.BccEmailAddresses, lang.Id, false, false);
            //    if (!string.IsNullOrEmpty(bccEmailAddresses))
            //        _localizedEntityService.SaveLocalizedValue(mtCopy, x => x.BccEmailAddresses, bccEmailAddresses, lang.Id);

            //    var subject = _localizationService.GetLocalized(messageTemplate, x => x.Subject, lang.Id, false, false);
            //    if (!string.IsNullOrEmpty(subject))
            //        _localizedEntityService.SaveLocalizedValue(mtCopy, x => x.Subject, subject, lang.Id);

            //    var body = _localizationService.GetLocalized(messageTemplate, x => x.Body, lang.Id, false, false);
            //    if (!string.IsNullOrEmpty(body))
            //        _localizedEntityService.SaveLocalizedValue(mtCopy, x => x.Body, body, lang.Id);

            //    var emailAccountId = _localizationService.GetLocalized(messageTemplate, x => x.EmailAccountId, lang.Id, false, false);
            //    if (emailAccountId > 0)
            //        _localizedEntityService.SaveLocalizedValue(mtCopy, x => x.EmailAccountId, emailAccountId, lang.Id);
            //}

            ////store mapping
            //var selectedStoreIds = _storeMappingService.GetStoresIdsWithAccess(messageTemplate);
            //foreach (var id in selectedStoreIds)
            //{
            //    _storeMappingService.InsertStoreMapping(mtCopy, id);
            //}

            return mtCopy;
        }

        #endregion
    }
}