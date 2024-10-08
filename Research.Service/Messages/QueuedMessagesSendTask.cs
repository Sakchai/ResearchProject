﻿using System;
using Research.Services.Logging;
using Research.Services.Messages;
using Research.Services.Tasks;

namespace Research.Services.Messages
{
    /// <summary>
    /// Represents a task for sending queued message 
    /// </summary>
    public partial class QueuedMessagesSendTask : IScheduleTask
    {
        private readonly IQueuedEmailService _queuedEmailService;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="queuedEmailService">Queued email service</param>
        /// <param name="emailSender">Email sender</param>
        /// <param name="logger">Logger</param>
        public QueuedMessagesSendTask(IQueuedEmailService queuedEmailService,
            IEmailSender emailSender, 
            ILogger logger)
        {
            this._queuedEmailService = queuedEmailService;
            this._emailSender = emailSender;
            this._logger = logger;
        }

        /// <summary>
        /// Executes a task
        /// </summary>
        public virtual void Execute()
        {
            var maxTries = 3;
            var queuedEmails = _queuedEmailService.SearchEmails(null, null, null, null,
                true, true, maxTries, false, 0, 500);
            foreach (var queuedEmail in queuedEmails)
            {
                var bcc = string.IsNullOrWhiteSpace(queuedEmail.Bcc) 
                            ? null 
                            : queuedEmail.Bcc.Split(new [] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                var cc = string.IsNullOrWhiteSpace(queuedEmail.CC) 
                            ? null 
                            : queuedEmail.CC.Split(new [] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                try
                {
                    _emailSender.SendEmail(queuedEmail.EmailAccount, 
                        queuedEmail.Subject, 
                        queuedEmail.Body,
                       queuedEmail.From, 
                       queuedEmail.FromName, 
                       queuedEmail.To, 
                       queuedEmail.ToName,
                       queuedEmail.ReplyTo,
                       queuedEmail.ReplyToName,
                       bcc, 
                       cc, 
                       queuedEmail.AttachmentFilePath,
                       queuedEmail.AttachmentFileName,
                       queuedEmail.AttachedDownloadId);

                    queuedEmail.SentOnUtc = DateTime.UtcNow;
                }
                catch (Exception exc)
                {
                    _logger.Error($"Error sending e-mail. {exc.Message}", exc);
                }
                finally
                {
                    queuedEmail.SentTries = queuedEmail.SentTries + 1;
                    _queuedEmailService.UpdateQueuedEmail(queuedEmail);
                }
            }
        }
    }
}
