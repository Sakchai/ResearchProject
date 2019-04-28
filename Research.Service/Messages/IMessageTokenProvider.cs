﻿using Research.Data;
using System.Collections.Generic;

namespace Research.Services.Messages
{
    /// <summary>
    /// Message token provider
    /// </summary>
    public partial interface IMessageTokenProvider
    {

        /// <summary>
        /// Add user tokens
        /// </summary>
        /// <param name="tokens">List of already added tokens</param>
        /// <param name="user">User</param>
        void AddUserTokens(IList<Token> tokens, User user);

        /// <summary>
        /// Add researcher tokens
        /// </summary>
        /// <param name="tokens">List of already added tokens</param>
        /// <param name="researcher">Researcher</param>
        void AddResearcherTokens(IList<Token> tokens, Researcher researcher);

        /// <summary>
        /// Add store tokens
        /// </summary>
        /// <param name="tokens">List of already added tokens</param>
        /// <param name="store">Store</param>
        /// <param name="emailAccount">Email account</param>
        void AddSiteTokens(IList<Token> tokens, EmailAccount emailAccount);
    }
}