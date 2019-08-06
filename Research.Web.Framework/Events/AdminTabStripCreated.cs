﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Research.Web.Framework.Events
{
    /// <summary>
    /// Admin tabstrip created event
    /// </summary>
    public class AdminTabStripCreated
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="helper">HTML Helper</param>
        /// <param name="tabStripName">Tabstrip name</param>
        public AdminTabStripCreated(IHtmlHelper helper, string tabStripName)
        {
            this.Helper = helper;
            this.TabStripName = tabStripName;
            this.BlocksToRender = new List<IHtmlContent>();
        }

        /// <summary>
        /// HTML Helper
        /// </summary>
        public IHtmlHelper Helper { get; private set; }
        /// <summary>
        /// TabStripName
        /// </summary>
        public string TabStripName { get; private set; }
        /// <summary>
        /// Blocks to render
        /// </summary>
        public IList<IHtmlContent> BlocksToRender { get; set; }
    }
}
