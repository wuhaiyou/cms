﻿//using System;
//using SiteServer.Utils;
//using Word.Plugin;
//using System.Collections.Specialized;
//using SiteServer.CMS.DataCache;
//using SiteServer.CMS.Model.Attributes;

//namespace SiteServer.CMS.Core.Office
//{
//    public static class WordUtils
//    {
//        public static string Parse(int siteId, string filePath, bool isClearFormat, bool isFirstLineIndent, bool isClearFontSize, bool isClearFontFamily, bool isClearImages)
//        {
//            if (string.IsNullOrEmpty(filePath)) return string.Empty;

//            var filename = PathUtils.GetFileNameWithoutExtension(filePath);

//            //被转换的html文档保存的位置
//            try
//            {
//                var saveFilePath = PathUtils.GetTemporaryFilesPath(filename + ".html");
//                FileUtils.DeleteFileIfExists(saveFilePath);
//                WordDntb.buildWord(filePath, saveFilePath);

//                var parsedContent = FileUtils.ReadText(saveFilePath, System.Text.Encoding.Default);
//                parsedContent = RegexUtils.GetInnerContent("body", parsedContent);

//                //try
//                //{
//                //    parsedContent = HtmlClearUtils.ClearElementAttributes(parsedContent, "p");
//                //}
//                //catch { }

//                if (isClearFormat)
//                {
//                    parsedContent = HtmlClearUtils.ClearFormat(parsedContent);
//                }

//                if (isFirstLineIndent)
//                {
//                    parsedContent = HtmlClearUtils.FirstLineIndent(parsedContent);
//                }

//                if (isClearFontSize)
//                {
//                    parsedContent = HtmlClearUtils.ClearFontSize(parsedContent);
//                }

//                if (isClearFontFamily)
//                {
//                    parsedContent = HtmlClearUtils.ClearFontFamily(parsedContent);
//                }

//                if (isClearImages)
//                {
//                    parsedContent = StringUtils.StripTags(parsedContent, "img");
//                }
//                else
//                {
//                    var siteInfo = SiteManager.GetSiteInfo(siteId);
//                    var imageFileNameArrayList = RegexUtils.GetOriginalImageSrcs(parsedContent);
//                    if (imageFileNameArrayList != null && imageFileNameArrayList.Count > 0)
//                    {
//                        foreach (var imageFileName in imageFileNameArrayList)
//                        {
//                            var imageFilePath = PathUtils.GetTemporaryFilesPath(imageFileName);
//                            var fileExtension = PathUtils.GetExtension(imageFilePath);
//                            var uploadDirectoryPath = PathUtility.GetUploadDirectoryPath(siteInfo, fileExtension);
//                            var uploadDirectoryUrl = PageUtility.GetSiteUrlByPhysicalPath(siteInfo, uploadDirectoryPath, true);
//                            if (!FileUtils.IsFileExists(imageFilePath)) continue;

//                            var uploadFileName = PathUtility.GetUploadFileName(siteInfo, imageFilePath);
//                            var destFilePath = PathUtils.Combine(uploadDirectoryPath, uploadFileName);
//                            FileUtils.MoveFile(imageFilePath, destFilePath, false);
//                            parsedContent = parsedContent.Replace(imageFileName, PageUtils.Combine(uploadDirectoryUrl, uploadFileName));

//                            FileUtils.DeleteFileIfExists(imageFilePath);
//                        }
//                    }
//                }

//                FileUtils.DeleteFileIfExists(filePath);
//                FileUtils.DeleteFileIfExists(saveFilePath);
//                return parsedContent.Trim();
//            }
//            catch (Exception ex)
//            {
//                LogUtils.AddErrorLog(ex);
//                return string.Empty;
//            }
//        }

//        public static NameValueCollection GetWordNameValueCollection(int siteId, bool isFirstLineTitle, bool isFirstLineRemove, bool isClearFormat, bool isFirstLineIndent, bool isClearFontSize, bool isClearFontFamily, bool isClearImages, string fileName)
//        {
//            var formCollection = new NameValueCollection();
//            var wordContent = Parse(siteId, PathUtils.GetTemporaryFilesPath(fileName), isClearFormat, isFirstLineIndent, isClearFontSize, isClearFontFamily, isClearImages);
//            if (!string.IsNullOrEmpty(wordContent))
//            {
//                var title = string.Empty;
//                if (isFirstLineTitle)
//                {
//                    title = RegexUtils.GetInnerContent("p", wordContent);
//                    title = StringUtils.StripTags(title);
//                    if (!string.IsNullOrEmpty(title) && isFirstLineRemove)
//                    {
//                        wordContent = StringUtils.ReplaceFirst(title, wordContent, string.Empty);
//                    }
//                    if (!string.IsNullOrEmpty(title))
//                    {
//                        title = title.Trim();
//                        title = title.Trim('　', ' ');
//                        title = StringUtils.StripEntities(title);
//                    }
//                }
//                if (string.IsNullOrEmpty(title))
//                {
//                    title = PathUtils.GetFileNameWithoutExtension(fileName);
//                }
//                if (!string.IsNullOrEmpty(title))
//                {
//                    title = StringUtils.MaxLengthText(title, 200, string.Empty);
//                }
//                formCollection[ContentAttribute.Title] = title;

//                wordContent = StringUtils.ReplaceFirst("<p></p>", wordContent, string.Empty);

//                formCollection[BackgroundContentAttribute.Content] = wordContent;
//            }
//            return formCollection;
//        }
//    }
//}