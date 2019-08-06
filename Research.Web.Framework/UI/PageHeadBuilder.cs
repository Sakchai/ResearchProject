﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using BundlerMinifier;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Research.Core;
using Research.Core.Caching;
using Research.Infrastructure;

namespace Research.Web.Framework.UI
{
    /// <summary>
    /// Page head builder
    /// </summary>
    public partial class PageHeadBuilder : IPageHeadBuilder
    {
        #region Fields

        private static readonly object s_lock = new object();

        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IStaticCacheManager _cacheManager;
        private readonly IResearchFileProvider _fileProvider;
        private BundleFileProcessor _processor;

        private readonly List<string> _titleParts;
        private readonly List<string> _metaDescriptionParts;
        private readonly List<string> _metaKeywordParts;
        private readonly Dictionary<ResourceLocation, List<ScriptReferenceMeta>> _scriptParts;
        private readonly Dictionary<ResourceLocation, List<string>> _inlineScriptParts;
        private readonly Dictionary<ResourceLocation, List<CssReferenceMeta>> _cssParts;
        private readonly List<string> _canonicalUrlParts;
        private readonly List<string> _headCustomParts;
        private readonly List<string> _pageCssClassParts;
        private string _editPageUrl;
        private string _activeAdminMenuSystemName;

        //in minutes
        private const int RecheckBundledFilesPeriod = 120;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="seoSettings">SEO settings</param>
        /// <param name="hostingEnvironment">Hosting environment</param>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="fileProvider">File provider</param>
        public PageHeadBuilder(IHostingEnvironment hostingEnvironment,
            IStaticCacheManager cacheManager,
            IResearchFileProvider fileProvider)
        {
            this._hostingEnvironment = hostingEnvironment;
            this._cacheManager = cacheManager;
            this._fileProvider = fileProvider;
            this._processor = new BundleFileProcessor();

            this._titleParts = new List<string>();
            this._metaDescriptionParts = new List<string>();
            this._metaKeywordParts = new List<string>();
            this._scriptParts = new Dictionary<ResourceLocation, List<ScriptReferenceMeta>>();
            this._inlineScriptParts = new Dictionary<ResourceLocation, List<string>>();
            this._cssParts = new Dictionary<ResourceLocation, List<CssReferenceMeta>>();
            this._canonicalUrlParts = new List<string>();
            this._headCustomParts = new List<string>();
            this._pageCssClassParts = new List<string>();
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Get bundled file name
        /// </summary>
        /// <param name="parts">Parts to bundle</param>
        /// <returns>File name</returns>
        protected virtual string GetBundleFileName(string[] parts)
        {
            if (parts == null || parts.Length == 0)
                throw new ArgumentException("parts");

            //calculate hash
            var hash = "";
            using (SHA256 sha = new SHA256Managed())
            {
                // string concatenation
                var hashInput = "";
                foreach (var part in parts)
                {
                    hashInput += part;
                    hashInput += ",";
                }

                var input = sha.ComputeHash(Encoding.Unicode.GetBytes(hashInput));
                hash = WebEncoders.Base64UrlEncode(input);
            }
            //ensure only valid chars
            //hash = SeoExtensions.GetSeName(hash);
            
            return hash;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add title element to the <![CDATA[<head>]]>
        /// </summary>
        /// <param name="part">Title part</param>
        public virtual void AddTitleParts(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;

            _titleParts.Add(part);
        }
        /// <summary>
        /// Append title element to the <![CDATA[<head>]]>
        /// </summary>
        /// <param name="part">Title part</param>
        public virtual void AppendTitleParts(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;
            
            _titleParts.Insert(0, part);
        }
        /// <summary>
        /// Generate all title parts
        /// </summary>
        /// <param name="addDefaultTitle">A value indicating whether to insert a default title</param>
        /// <returns>Generated string</returns>
        public virtual string GenerateTitle(bool addDefaultTitle)
        {
            var result = "";
            //var specificTitle = string.Join(_seoSettings.PageTitleSeparator, _titleParts.AsEnumerable().Reverse().ToArray());
            //if (!string.IsNullOrEmpty(specificTitle))
            //{
            //    if (addDefaultTitle)
            //    {
            //        //store name + page title
            //        switch (_seoSettings.PageTitleSeoAdjustment)
            //        {
            //            case PageTitleSeoAdjustment.PagenameAfterStorename:
            //                {
            //                    result = string.Join(_seoSettings.PageTitleSeparator, _seoSettings.DefaultTitle, specificTitle);
            //                }
            //                break;
            //            case PageTitleSeoAdjustment.StorenameAfterPagename:
            //            default:
            //                {
            //                    result = string.Join(_seoSettings.PageTitleSeparator, specificTitle, _seoSettings.DefaultTitle);
            //                }
            //                break;
                            
            //        }
            //    }
            //    else
            //    {
            //        //page title only
            //        result = specificTitle;
            //    }
            //}
            //else
            //{
            //    //store name only
            //    result = _seoSettings.DefaultTitle;
            //}
            //return result;
            return result;
        }

        /// <summary>
        /// Add meta description element to the <![CDATA[<head>]]>
        /// </summary>
        /// <param name="part">Meta description part</param>
        public virtual void AddMetaDescriptionParts(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;
            
            _metaDescriptionParts.Add(part);
        }
        /// <summary>
        /// Append meta description element to the <![CDATA[<head>]]>
        /// </summary>
        /// <param name="part">Meta description part</param>
        public virtual void AppendMetaDescriptionParts(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;
            
            _metaDescriptionParts.Insert(0, part);
        }
        /// <summary>
        /// Generate all description parts
        /// </summary>
        /// <returns>Generated string</returns>
        public virtual string GenerateMetaDescription()
        {
            //var metaDescription = string.Join(", ", _metaDescriptionParts.AsEnumerable().Reverse().ToArray());
            //var result = !string.IsNullOrEmpty(metaDescription) ? metaDescription : _seoSettings.DefaultMetaDescription;
            //return result;

            return string.Join(", ", _metaDescriptionParts.AsEnumerable().Reverse().ToArray());
        }

        /// <summary>
        /// Add meta keyword element to the <![CDATA[<head>]]>
        /// </summary>
        /// <param name="part">Meta keyword part</param>
        public virtual void AddMetaKeywordParts(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;
            
            _metaKeywordParts.Add(part);
        }
        /// <summary>
        /// Append meta keyword element to the <![CDATA[<head>]]>
        /// </summary>
        /// <param name="part">Meta keyword part</param>
        public virtual void AppendMetaKeywordParts(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;

            _metaKeywordParts.Insert(0, part);
        }
        /// <summary>
        /// Generate all keyword parts
        /// </summary>
        /// <returns>Generated string</returns>
        public virtual string GenerateMetaKeywords()
        {
            //var metaKeyword = string.Join(", ", _metaKeywordParts.AsEnumerable().Reverse().ToArray());
            //var result = !string.IsNullOrEmpty(metaKeyword) ? metaKeyword : _seoSettings.DefaultMetaKeywords;
            //return result;
            return string.Join(", ", _metaKeywordParts.AsEnumerable().Reverse().ToArray());
        }

        /// <summary>
        /// Add script element
        /// </summary>
        /// <param name="location">A location of the script element</param>
        /// <param name="src">Script path (minified version)</param>
        /// <param name="debugSrc">Script path (full debug version). If empty, then minified version will be used</param>
        /// <param name="excludeFromBundle">A value indicating whether to exclude this script from bundling</param>
        /// <param name="isAsync">A value indicating whether to add an attribute "async" or not for js files</param>
        public virtual void AddScriptParts(ResourceLocation location, string src, string debugSrc, bool excludeFromBundle, bool isAsync)
        {
            if (!_scriptParts.ContainsKey(location))
                _scriptParts.Add(location, new List<ScriptReferenceMeta>());

            if (string.IsNullOrEmpty(src))
                return;

            if (string.IsNullOrEmpty(debugSrc))
                debugSrc = src;

            _scriptParts[location].Add(new ScriptReferenceMeta
            {
                ExcludeFromBundle = excludeFromBundle,
                IsAsync = isAsync,
                Src = src,
                DebugSrc = debugSrc
            });
        }
        /// <summary>
        /// Append script element
        /// </summary>
        /// <param name="location">A location of the script element</param>
        /// <param name="src">Script path (minified version)</param>
        /// <param name="debugSrc">Script path (full debug version). If empty, then minified version will be used</param>
        /// <param name="excludeFromBundle">A value indicating whether to exclude this script from bundling</param>
        /// <param name="isAsync">A value indicating whether to add an attribute "async" or not for js files</param>
        public virtual void AppendScriptParts(ResourceLocation location, string src, string debugSrc, bool excludeFromBundle, bool isAsync)
        {
            if (!_scriptParts.ContainsKey(location))
                _scriptParts.Add(location, new List<ScriptReferenceMeta>());

            if (string.IsNullOrEmpty(src))
                return;

            if (string.IsNullOrEmpty(debugSrc))
                debugSrc = src;

            _scriptParts[location].Insert(0, new ScriptReferenceMeta
            {
                ExcludeFromBundle = excludeFromBundle,
                IsAsync = isAsync,
                Src = src,
                DebugSrc = debugSrc
            });
        }
        /// <summary>
        /// Generate all script parts
        /// </summary>
        /// <param name="urlHelper">URL Helper</param>
        /// <param name="location">A location of the script element</param>
        /// <param name="bundleFiles">A value indicating whether to bundle script elements</param>
        /// <returns>Generated string</returns>
        public virtual string GenerateScripts(IUrlHelper urlHelper, ResourceLocation location, bool? bundleFiles = null)
        {
            if (!_scriptParts.ContainsKey(location) || _scriptParts[location] == null)
                return "";

            if (!_scriptParts.Any())
                return "";

            var debugModel = _hostingEnvironment.IsDevelopment();
            
            //chai
            //if (!bundleFiles.HasValue)
            //{
            //    //use setting if no value is specified
            //    bundleFiles = _seoSettings.EnableJsBundling;
            //}

            if (bundleFiles.Value)
            {
                var partsToBundle = _scriptParts[location]
                    .Where(x => !x.ExcludeFromBundle)
                    .Distinct()
                    .ToArray();
                var partsToDontBundle = _scriptParts[location]
                    .Where(x => x.ExcludeFromBundle)
                    .Distinct()
                    .ToArray();

                var result = new StringBuilder();

                //parts to  bundle
                if (partsToBundle.Any())
                {
                    //ensure \bundles directory exists
                    _fileProvider.CreateDirectory(_fileProvider.GetAbsolutePath("bundles"));

                    var bundle = new Bundle();
                    foreach (var item in partsToBundle)
                    {
                        new PathString(urlHelper.Content(debugModel ? item.DebugSrc : item.Src))
                            .StartsWithSegments(urlHelper.ActionContext.HttpContext.Request.PathBase, out PathString path);
                        var src = path.Value.TrimStart('/');

                        //check whether this file exists, if not it should be stored into /wwwroot directory
                        if (!_fileProvider.FileExists(_fileProvider.MapPath(path)))
                            src = $"wwwroot/{src}";

                        bundle.InputFiles.Add(src);
                    }
                    //output file
                    var outputFileName = GetBundleFileName(partsToBundle.Select(x => debugModel ? x.DebugSrc : x.Src).ToArray());
                    bundle.OutputFileName = "wwwroot/bundles/" + outputFileName + ".js";
                    //save
                    var configFilePath = _hostingEnvironment.ContentRootPath + "\\" + outputFileName + ".json";
                    bundle.FileName = configFilePath;
                    lock (s_lock)
                    {
                        //performance optimization. do not bundle and minify for each HTTP request
                        //we periodically re-check already bundles file
                        //so if we have minification enabled, it could take up to several minutes to see changes in updated resource files (or just reset the cache or restart the site)
                        var cacheKey = $"Research.minification.shouldrebuild.js-{outputFileName}";
                        var shouldRebuild = _cacheManager.Get(cacheKey, RecheckBundledFilesPeriod, () => true);
                        if (shouldRebuild)
                        {
                            //store json file to see a generated config file (for debugging purposes)
                            //BundleHandler.AddBundle(configFilePath, bundle);

                            //process
                            _processor.Process(configFilePath, new List<Bundle> { bundle });
                            _cacheManager.Set(cacheKey, false, RecheckBundledFilesPeriod);
                        }
                    }
                    //render
                    result.AppendFormat("<script src=\"{0}\" type=\"{1}\"></script>", urlHelper.Content("~/bundles/" + outputFileName + ".min.js"), MimeTypes.TextJavascript);
                    result.Append(Environment.NewLine);
                }


                //parts to not bundle
                foreach (var item in partsToDontBundle)
                {
                    var src = debugModel ? item.DebugSrc : item.Src;
                    result.AppendFormat("<script {2}src=\"{0}\" type=\"{1}\"></script>", urlHelper.Content(src), MimeTypes.TextJavascript, item.IsAsync ? "async " : "");
                    result.Append(Environment.NewLine);
                }
                return result.ToString();
            }
            else
            {
                //bundling is disabled
                var result = new StringBuilder();
                foreach (var item in _scriptParts[location].Distinct())
                {
                    var src = debugModel ? item.DebugSrc : item.Src;
                    result.AppendFormat("<script {2}src=\"{0}\" type=\"{1}\"></script>", urlHelper.Content(src), MimeTypes.TextJavascript, item.IsAsync ? "async ":"");
                    result.Append(Environment.NewLine);
                }
                return result.ToString();
            }
        }

        /// <summary>
        /// Add inline script element
        /// </summary>
        /// <param name="location">A location of the script element</param>
        /// <param name="script">Script</param>
        public virtual void AddInlineScriptParts(ResourceLocation location, string script)
        {
            if (!_inlineScriptParts.ContainsKey(location))
                _inlineScriptParts.Add(location, new List<string>());

            if (string.IsNullOrEmpty(script))
                return;

            _inlineScriptParts[location].Add(script);
        }
        /// <summary>
        /// Append inline script element
        /// </summary>
        /// <param name="location">A location of the script element</param>
        /// <param name="script">Script</param>
        public virtual void AppendInlineScriptParts(ResourceLocation location, string script)
        {
            if (!_inlineScriptParts.ContainsKey(location))
                _inlineScriptParts.Add(location, new List<string>());

            if (string.IsNullOrEmpty(script))
                return;

            _inlineScriptParts[location].Insert(0, script);
        }
        /// <summary>
        /// Generate all inline script parts
        /// </summary>
        /// <param name="urlHelper">URL Helper</param>
        /// <param name="location">A location of the script element</param>
        /// <returns>Generated string</returns>
        public virtual string GenerateInlineScripts(IUrlHelper urlHelper, ResourceLocation location)
        {
            if (!_inlineScriptParts.ContainsKey(location) || _inlineScriptParts[location] == null)
                return "";

            if (!_inlineScriptParts.Any())
                return "";

            var result = new StringBuilder();
            foreach (var item in _inlineScriptParts[location])
            {
                result.Append(item);
                result.Append(Environment.NewLine);
            }
            return result.ToString();
        }

        /// <summary>
        /// Add CSS element
        /// </summary>
        /// <param name="location">A location of the script element</param>
        /// <param name="src">Script path (minified version)</param>
        /// <param name="debugSrc">Script path (full debug version). If empty, then minified version will be used</param>
        /// <param name="excludeFromBundle">A value indicating whether to exclude this script from bundling</param>
        public virtual void AddCssFileParts(ResourceLocation location, string src, string debugSrc, bool excludeFromBundle = false)
        {
            if (!_cssParts.ContainsKey(location))
                _cssParts.Add(location, new List<CssReferenceMeta>());

            if (string.IsNullOrEmpty(src))
                return;

            if (string.IsNullOrEmpty(debugSrc))
                debugSrc = src;

            _cssParts[location].Add(new CssReferenceMeta
            {
                ExcludeFromBundle = excludeFromBundle,
                Src = src,
                DebugSrc = debugSrc
            });
        }
        /// <summary>
        /// Append CSS element
        /// </summary>
        /// <param name="location">A location of the script element</param>
        /// <param name="src">Script path (minified version)</param>
        /// <param name="debugSrc">Script path (full debug version). If empty, then minified version will be used</param>
        /// <param name="excludeFromBundle">A value indicating whether to exclude this script from bundling</param>
        public virtual void AppendCssFileParts(ResourceLocation location, string src, string debugSrc, bool excludeFromBundle = false)
        {
            if (!_cssParts.ContainsKey(location))
                _cssParts.Add(location, new List<CssReferenceMeta>());

            if (string.IsNullOrEmpty(src))
                return;

            if (string.IsNullOrEmpty(debugSrc))
                debugSrc = src;

            _cssParts[location].Insert(0, new CssReferenceMeta
            {
                ExcludeFromBundle = excludeFromBundle,
                Src = src,
                DebugSrc = debugSrc
            });
        }
        /// <summary>
        /// Generate all CSS parts
        /// </summary>
        /// <param name="urlHelper">URL Helper</param>
        /// <param name="location">A location of the script element</param>
        /// <param name="bundleFiles">A value indicating whether to bundle script elements</param>
        /// <returns>Generated string</returns>
        public virtual string GenerateCssFiles(IUrlHelper urlHelper, ResourceLocation location, bool? bundleFiles = null)
        {
            if (!_cssParts.ContainsKey(location) || _cssParts[location] == null)
                return "";

            if (!_cssParts.Any())
                return "";


            var debugModel = _hostingEnvironment.IsDevelopment();
            //chai
            //if (!bundleFiles.HasValue)
            //{
            //    //use setting if no value is specified
            //    bundleFiles = _seoSettings.EnableCssBundling;
            //}

            //CSS bundling is not allowed in virtual directories
            if (urlHelper.ActionContext.HttpContext.Request.PathBase.HasValue)
                bundleFiles = false;

            if (bundleFiles.Value)
            {
                var partsToBundle = _cssParts[location]
                    .Where(x => !x.ExcludeFromBundle)
                    .Distinct()
                    .ToArray();
                var partsToDontBundle = _cssParts[location]
                    .Where(x => x.ExcludeFromBundle)
                    .Distinct()
                    .ToArray();

                var result = new StringBuilder();


                //parts to  bundle
                if (partsToBundle.Any())
                {
                    //ensure \bundles directory exists
                    _fileProvider.CreateDirectory(_fileProvider.GetAbsolutePath("bundles"));

                    var bundle = new Bundle();
                    foreach (var item in partsToBundle)
                    {
                        var src = debugModel ? item.DebugSrc : item.Src;
                        src = urlHelper.Content(src);
                        //check whether this file exists 
                        var srcPath = _fileProvider.Combine(_hostingEnvironment.ContentRootPath, src.Remove(0, 1).Replace("/", "\\"));
                        if (_fileProvider.FileExists(srcPath))
                        {
                            //remove starting /
                            src = src.Remove(0, 1);
                        }
                        else
                        {
                            //if not, it should be stored into /wwwroot directory
                            src = "wwwroot/" + src;
                        }
                        bundle.InputFiles.Add(src);
                    }
                    //output file
                    var outputFileName = GetBundleFileName(partsToBundle.Select(x => { return debugModel ? x.DebugSrc : x.Src; }).ToArray());
                    bundle.OutputFileName = "wwwroot/bundles/" + outputFileName + ".css";
                    //save
                    var configFilePath = _hostingEnvironment.ContentRootPath + "\\" + outputFileName + ".json";
                    bundle.FileName = configFilePath;
                    lock (s_lock)
                    {
                        //performance optimization. do not bundle and minify for each HTTP request
                        //we periodically re-check already bundles file
                        //so if we have minification enabled, it could take up to several minutes to see changes in updated resource files (or just reset the cache or restart the site)
                        var cacheKey = $"Research.minification.shouldrebuild.css-{outputFileName}";
                        var shouldRebuild = _cacheManager.Get<bool>(cacheKey, RecheckBundledFilesPeriod, () => true);
                        if (shouldRebuild)
                        {
                            //store json file to see a generated config file (for debugging purposes)
                            //BundleHandler.AddBundle(configFilePath, bundle);

                            //process
                            _processor.Process(configFilePath, new List<Bundle> {bundle});
                            _cacheManager.Set(cacheKey, false, RecheckBundledFilesPeriod);
                        }
                    }
                    //render
                    result.AppendFormat("<link href=\"{0}\" rel=\"stylesheet\" type=\"{1}\" />", urlHelper.Content("~/bundles/" + outputFileName + ".min.css"), MimeTypes.TextCss);
                    result.Append(Environment.NewLine);
                }

                //parts not to bundle
                foreach (var item in partsToDontBundle)
                {
                    var src = debugModel ? item.DebugSrc : item.Src;
                    result.AppendFormat("<link href=\"{0}\" rel=\"stylesheet\" type=\"{1}\" />", urlHelper.Content(src), MimeTypes.TextCss);
                    result.Append(Environment.NewLine);
                }

                return result.ToString();
            }
            else
            {
                //bundling is disabled
                var result = new StringBuilder();
                foreach (var item in _cssParts[location].Distinct())
                {
                    var src = debugModel ? item.DebugSrc : item.Src;
                    result.AppendFormat("<link href=\"{0}\" rel=\"stylesheet\" type=\"{1}\" />", urlHelper.Content(src), MimeTypes.TextCss);
                    result.AppendLine();
                }
                return result.ToString();
            }
        }

        /// <summary>
        /// Add canonical URL element to the <![CDATA[<head>]]>
        /// </summary>
        /// <param name="part">Canonical URL part</param>
        public virtual void AddCanonicalUrlParts(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;
                       
            _canonicalUrlParts.Add(part);
        }
        /// <summary>
        /// Append canonical URL element to the <![CDATA[<head>]]>
        /// </summary>
        /// <param name="part">Canonical URL part</param>
        public virtual void AppendCanonicalUrlParts(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;
                       
            _canonicalUrlParts.Insert(0, part);
        }
        /// <summary>
        /// Generate all canonical URL parts
        /// </summary>
        /// <returns>Generated string</returns>
        public virtual string GenerateCanonicalUrls()
        {
            var result = new StringBuilder();
            foreach (var canonicalUrl in _canonicalUrlParts)
            {
                result.AppendFormat("<link rel=\"canonical\" href=\"{0}\" />", canonicalUrl);
                result.Append(Environment.NewLine);
            }
            return result.ToString();
        }

        /// <summary>
        /// Add any custom element to the <![CDATA[<head>]]> element
        /// </summary>
        /// <param name="part">The entire element. For example, <![CDATA[<meta name="msvalidate.01" content="123121231231313123123" />]]></param>
        public virtual void AddHeadCustomParts(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;

            _headCustomParts.Add(part);
        }
        /// <summary>
        /// Append any custom element to the <![CDATA[<head>]]> element
        /// </summary>
        /// <param name="part">The entire element. For example, <![CDATA[<meta name="msvalidate.01" content="123121231231313123123" />]]></param>
        public virtual void AppendHeadCustomParts(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;

            _headCustomParts.Insert(0, part);
        }
        /// <summary>
        /// Generate all custom elements
        /// </summary>
        /// <returns>Generated string</returns>
        public virtual string GenerateHeadCustom()
        {
            //use only distinct rows
            var distinctParts = _headCustomParts.Distinct().ToList();
            if (!distinctParts.Any())
                return "";

            var result = new StringBuilder();
            foreach (var path in distinctParts)
            {
                result.Append(path);
                result.Append(Environment.NewLine);
            }
            return result.ToString();
        }

        /// <summary>
        /// Add CSS class to the <![CDATA[<head>]]> element
        /// </summary>
        /// <param name="part">CSS class</param>
        public virtual void AddPageCssClassParts(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;

            _pageCssClassParts.Add(part);
        }
        /// <summary>
        /// Append CSS class to the <![CDATA[<head>]]> element
        /// </summary>
        /// <param name="part">CSS class</param>
        public virtual void AppendPageCssClassParts(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;

            _pageCssClassParts.Insert(0, part);
        }
        /// <summary>
        /// Generate all title parts
        /// </summary>
        /// <returns>Generated string</returns>
        public virtual string GeneratePageCssClasses()
        {
            var result = string.Join(" ", _pageCssClassParts.AsEnumerable().Reverse().ToArray());
            return result;
        }
        
        /// <summary>
        /// Specify "edit page" URL
        /// </summary>
        /// <param name="url">URL</param>
        public virtual void AddEditPageUrl(string url)
        {
            _editPageUrl = url;
        }
        /// <summary>
        /// Get "edit page" URL
        /// </summary>
        /// <returns>URL</returns>
        public virtual string GetEditPageUrl()
        {
            return _editPageUrl;
        }
        
        /// <summary>
        /// Specify system name of admin menu item that should be selected (expanded)
        /// </summary>
        /// <param name="systemName">System name</param>
        public virtual void SetActiveMenuItemSystemName(string systemName)
        {
            _activeAdminMenuSystemName = systemName;
        }
        /// <summary>
        /// Get system name of admin menu item that should be selected (expanded)
        /// </summary>
        /// <returns>System name</returns>
        public virtual string GetActiveMenuItemSystemName()
        {
            return _activeAdminMenuSystemName;
        }

        #endregion
        
        #region Nested classes

        /// <summary>
        /// JS file meta data
        /// </summary>
        private class ScriptReferenceMeta : IEquatable<ScriptReferenceMeta>
        {
            /// <summary>
            /// A value indicating whether to exclude the script from bundling
            /// </summary>
            public bool ExcludeFromBundle { get; set; }

            /// <summary>
            /// A value indicating whether to load the script asynchronously 
            /// </summary>
            public bool IsAsync { get; set; }

            /// <summary>
            /// Src for production
            /// </summary>
            public string Src { get; set; }

            /// <summary>
            /// Src for debugging
            /// </summary>
            public string DebugSrc { get; set; }

            /// <summary>
            /// Equals
            /// </summary>
            /// <param name="item">Other item</param>
            /// <returns>Result</returns>
            public bool Equals(ScriptReferenceMeta item)
            {
                if (item == null)
                    return false;
                return this.Src.Equals(item.Src) && this.DebugSrc.Equals(item.DebugSrc);
            }
            /// <summary>
            /// Get hash code
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return Src == null ? 0 : Src.GetHashCode();
            }
        }

        /// <summary>
        /// CSS file meta data
        /// </summary>
        private class CssReferenceMeta : IEquatable<CssReferenceMeta>
        {
            public bool ExcludeFromBundle { get; set; }

            /// <summary>
            /// Src for production
            /// </summary>
            public string Src { get; set; }

            /// <summary>
            /// Src for debugging
            /// </summary>
            public string DebugSrc { get; set; }

            /// <summary>
            /// Equals
            /// </summary>
            /// <param name="item">Other item</param>
            /// <returns>Result</returns>
            public bool Equals(CssReferenceMeta item)
            {
                if (item == null)
                    return false;
                return this.Src.Equals(item.Src) && this.DebugSrc.Equals(item.DebugSrc);
            }
            /// <summary>
            /// Get hash code
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return Src == null ? 0 : Src.GetHashCode();
            }
        }

        #endregion
    }
}
