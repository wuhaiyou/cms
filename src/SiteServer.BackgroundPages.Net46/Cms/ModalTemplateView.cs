using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using SiteServer.Utils;
using SiteServer.CMS.Core;
using SiteServer.BackgroundPages.Core;

namespace SiteServer.BackgroundPages.Cms
{
    public class ModalTemplateView : BasePageCms
    {
        public TextBox TbContent;

        public static string GetOpenWindowString(int siteId, int templateLogId)
        {
            return LayerUtils.GetOpenScript("查看修订内容", PageUtilsEx.GetCmsUrl(siteId, nameof(ModalTemplateView), new NameValueCollection
            {
                {"templateLogID", templateLogId.ToString()}
            }));
        }

        public void Page_Load(object sender, EventArgs e)
        {
            if (IsForbidden) return;

            FxUtils.CheckRequestParameter("siteId");

            if (!IsPostBack)
            {
                var templateLogId = AuthRequest.GetQueryInt("templateLogID");
                TbContent.Text = DataProvider.TemplateLogDao.GetTemplateContent(templateLogId);
            }
        }
    }
}
