using System;
using Datory;

namespace SiteServer.CMS.Model
{
    [Table("siteserver_ContentCheck")]
    public class ContentCheckInfo : Entity
    {
        [TableColumn]
        public string TableName { get; set; }

        [TableColumn]
        public int SiteId { get; set; }

        [TableColumn]
        public int ChannelId { get; set; }

        [TableColumn]
        public int ContentId { get; set; }

        [TableColumn]
        public string UserName { get; set; }

        [TableColumn]
        private string IsChecked { get; set; }

        public bool Checked
        {
            get => IsChecked == "True";
            set => IsChecked = value.ToString();
        }

        [TableColumn]
        public int CheckedLevel { get; set; }

        [TableColumn]
        public DateTime? CheckDate { get; set; }

        [TableColumn]
        public string Reasons { get; set; }
    }
}
