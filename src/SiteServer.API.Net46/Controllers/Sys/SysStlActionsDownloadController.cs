﻿using System.Web;
using System.Web.Http;
using SiteServer.API.Common;
using SiteServer.BackgroundPages.Core;
using SiteServer.CMS.Api.Sys.Stl;
using SiteServer.CMS.Core;
using SiteServer.CMS.DataCache;
using SiteServer.CMS.DataCache.Content;
using SiteServer.CMS.Model.Attributes;
using SiteServer.Utils;
using SiteServer.Utils.Enumerations;

namespace SiteServer.API.Controllers.Sys
{
    public class SysStlActionsDownloadController : ControllerBase
    {
        [HttpGet]
        [Route(ApiRouteActionsDownload.Route)]
        public void Main()
        {
            try
            {
                var request = GetRequest();

                if (!string.IsNullOrEmpty(request.GetQueryString("siteId")) && !string.IsNullOrEmpty(request.GetQueryString("fileUrl")) && string.IsNullOrEmpty(request.GetQueryString("contentId")))
                {
                    var siteId = request.GetQueryInt("siteId");
                    var fileUrl = TranslateUtils.DecryptStringBySecretKey(request.GetQueryString("fileUrl"));

                    if (PageUtils.IsProtocolUrl(fileUrl))
                    {
                        FxUtils.Page.Redirect(fileUrl);
                        return;
                    }

                    var siteInfo = SiteManager.GetSiteInfo(siteId);
                    var filePath = PathUtility.MapPath(siteInfo, fileUrl);
                    var fileType = EFileSystemTypeUtils.GetEnumType(PathUtils.GetExtension(filePath));
                    if (EFileSystemTypeUtils.IsDownload(fileType))
                    {
                        if (FileUtils.IsFileExists(filePath))
                        {
                            FxUtils.Page.Download(HttpContext.Current.Response, filePath);
                            return;
                        }
                    }
                    else
                    {
                        FxUtils.Page.Redirect(PageUtility.ParseNavigationUrl(siteInfo, fileUrl, false));
                        return;
                    }
                }
                else if (!string.IsNullOrEmpty(request.GetQueryString("filePath")))
                {
                    var filePath = TranslateUtils.DecryptStringBySecretKey(request.GetQueryString("filePath"));
                    var fileType = EFileSystemTypeUtils.GetEnumType(PathUtils.GetExtension(filePath));
                    if (EFileSystemTypeUtils.IsDownload(fileType))
                    {
                        if (FileUtils.IsFileExists(filePath))
                        {
                            FxUtils.Page.Download(HttpContext.Current.Response, filePath);
                            return;
                        }
                    }
                    else
                    {
                        var fileUrl = PageUtilsEx.GetRootUrlByPhysicalPath(filePath);
                        FxUtils.Page.Redirect(PageUtilsEx.ParseNavigationUrl(fileUrl));
                        return;
                    }
                }
                else if (!string.IsNullOrEmpty(request.GetQueryString("siteId")) && !string.IsNullOrEmpty(request.GetQueryString("channelId")) && !string.IsNullOrEmpty(request.GetQueryString("contentId")) && !string.IsNullOrEmpty(request.GetQueryString("fileUrl")))
                {
                    var siteId = request.GetQueryInt("siteId");
                    var channelId = request.GetQueryInt("channelId");
                    var contentId = request.GetQueryInt("contentId");
                    var fileUrl = TranslateUtils.DecryptStringBySecretKey(request.GetQueryString("fileUrl"));
                    var siteInfo = SiteManager.GetSiteInfo(siteId);
                    var channelInfo = ChannelManager.GetChannelInfo(siteId, channelId);
                    var contentInfo = ContentManager.GetContentInfo(siteInfo, channelInfo, contentId);

                    channelInfo.ContentDao.AddDownloads(channelId, contentId);

                    if (!string.IsNullOrEmpty(contentInfo?.Get<string>(ContentAttribute.FileUrl)))
                    {
                        if (PageUtils.IsProtocolUrl(fileUrl))
                        {
                            FxUtils.Page.Redirect(fileUrl);
                            return;
                        }

                        var filePath = PathUtility.MapPath(siteInfo, fileUrl, true);
                        var fileType = EFileSystemTypeUtils.GetEnumType(PathUtils.GetExtension(filePath));
                        if (EFileSystemTypeUtils.IsDownload(fileType))
                        {
                            if (FileUtils.IsFileExists(filePath))
                            {
                                FxUtils.Page.Download(HttpContext.Current.Response, filePath);
                                return;
                            }
                        }
                        else
                        {
                            FxUtils.Page.Redirect(PageUtility.ParseNavigationUrl(siteInfo, fileUrl, false));
                            return;
                        }
                    }
                }
            }
            catch
            {
                // ignored
            }

            HttpContext.Current.Response.Write("下载失败，不存在此文件！");
        }
    }
}
