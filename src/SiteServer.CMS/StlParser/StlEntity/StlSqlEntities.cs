using System.Collections.Generic;
using SiteServer.Utils;
using SiteServer.CMS.StlParser.Model;
using SiteServer.CMS.StlParser.Utility;
using System;

namespace SiteServer.CMS.StlParser.StlEntity
{
    [StlElement(Title = "数据库实体", Description = "通过 {sql.} 实体在模板中显示数据库值")]
    public class StlSqlEntities
    {
        private StlSqlEntities()
        {
        }

        public const string EntityName = "sql";

        public static SortedList<string, string> AttributeList => new SortedList<string, string>
        {
            {StlParserUtility.ItemIndex, "排序"}
        };

        internal static string Parse(string stlEntity, PageInfo pageInfo, ContextInfo contextInfo)
        {
            var parsedContent = string.Empty;

            if (contextInfo.Container?.SqlItem == null) return string.Empty;

            try
            {
                var attributeName = stlEntity.Substring(5, stlEntity.Length - 6);

                if (StringUtils.StartsWithIgnoreCase(attributeName, StlParserUtility.ItemIndex))
                {
                    parsedContent = StlParserUtility.ParseItemIndex(contextInfo.Container.SqlItem.ItemIndex, attributeName, contextInfo).ToString();
                }
                else
                {
                    if (contextInfo.Container.SqlItem.Dictionary.TryGetValue(attributeName, out var value))
                    {
                        parsedContent = Convert.ToString(value);
                    }
                }

                // parsedContent = StringUtils.StartsWithIgnoreCase(attributeName, StlParserUtility.ItemIndex) ? StlParserUtility.ParseItemIndex(contextInfo.ItemContainer.SqlItem.ItemIndex, attributeName, contextInfo).ToString() : DataBinder.Eval(contextInfo.ItemContainer.SqlItem.DataItem, attributeName, "{0}");
            }
            catch
            {
                // ignored
            }

            return parsedContent;
        }
    }
}
