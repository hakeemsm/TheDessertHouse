using System;
using System.Configuration;
using System.Web.Configuration;

namespace TheDessertHouse.Web.Configuration
{
    public class DessertHouseConfigurationSection : ConfigurationSection
    {
        public static DessertHouseConfigurationSection Current = WebConfigurationManager.GetSection("theDessertHouse") as DessertHouseConfigurationSection;

        [ConfigurationProperty("articles", IsRequired = true)]
        public ArticlesElement Articles { get { return base["articles"] as ArticlesElement; } }

        [ConfigurationProperty("users", IsRequired = true)]
        public UsersElement Users
        {
            get { return base["users"] as UsersElement; }
        }

        [ConfigurationProperty("polls", IsRequired = true)]
        public PollsElement Polls
        {
            get { return base["polls"] as PollsElement; }
        }

        [ConfigurationProperty("newsletters",IsRequired = true)]
        public NewslettersElement Newsletters
        {
            get { return base["newsletters"] as NewslettersElement; }
        }

        [ConfigurationProperty("forums",IsRequired = true)]
        public ForumsElement Forums
        {
            get { return base["forums"] as ForumsElement; }
        }

        [ConfigurationProperty("store",IsRequired = true)]
        public StoreElement Store
        {
            get { return base["store"] as StoreElement; }
        }
    }

    public class StoreElement:ConfigurationElement
    {
        [ConfigurationProperty("payPalServer",IsRequired = true)]
        public string PayPalServer { 
            get { return base["payPalServer"] as string; }
            set { base["payPalServer"] = value; }
        }

        [ConfigurationProperty("payPalAccount", IsRequired = true)]
        public string PayPalAccount
        {
            get { return base["payPalAccount"] as string; }
            set { base["payPalAccount"] = value; }
        }

        [ConfigurationProperty("payPalToken", IsRequired = true)]
        public string PayPalToken
        {
            get { return base["payPalToken"] as string; }
            set { base["payPalToken"] = value; }
        }
    }

    public class NewslettersElement:ConfigurationElement
    {
        [ConfigurationProperty("fromEmailDisplayName",IsRequired = true,DefaultValue = "News Mailer")]
        public string FromEmailDisplayName
        {
            get { return base["fromEmailDisplayName"] as string; }
            set { base["fromEmailDisplayName"] = value; }
        }

        [ConfigurationProperty("fromEmail", DefaultValue = "mailbot@tdh.com",IsRequired = true)]
        public string FromEmail
        {
            get { return base["fromEmail"] as string; }
            set { base["fromEmail"] = value; }
        }
    }

    public class UsersElement : ConfigurationElement
    {
        [ConfigurationProperty("pageSize", DefaultValue = 10)]
        public int PageSize
        {
            get { return (int)base["pageSize"]; }
            set { base["pageSize"] = value; }
        }
    }

    public class ArticlesElement : ConfigurationElement
    {
        [ConfigurationProperty("pageSize", DefaultValue = 10)]
        public int PageSize
        {
            get { return (int)base["pageSize"]; }
            set { base["pageSize"] = value; }
        }
    }

    public class PollsElement:ConfigurationElement
    {
        [ConfigurationProperty("pageSize", DefaultValue = 10)]
        public int PageSize
        {
            get { return (int)base["pageSize"]; }
            set { base["pageSize"] = value; }
        }

        [ConfigurationProperty("archiveIsPublic", DefaultValue = false)]
        public bool ArchiveIsPublic
        {
            get { return (bool)base["archiveIsPublic"]; }
            set { base["archiveIsPublic"] = value; }
        }
    }

    public class ForumsElement:ConfigurationElement
    {
        [ConfigurationProperty("postReplyPageSize",DefaultValue = 20)]
        public int PostReplyPageSize
        {
            get { return (int)base["postReplyPageSize"]; }
            set { base["postReplyPageSize"] = value; }
        }

        [ConfigurationProperty("forumPageSize", DefaultValue = 15)]
        public int ForumPageSize
        {
            get { return (int)base["forumPageSize"]; }
            set { base["forumPageSize"] = value; }
        }
    }
}