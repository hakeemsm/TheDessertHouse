using System;
using System.Collections.Generic;

namespace TheDessertHouse.Domain
{
    public class NewsletterMessage
    {
        public string FromEmail { get; set; }

        public string FromEmailDisplayName { get; set; }

        public List<string> EmailList { get; set; }

        public Newsletter Newsletter { get; set; }
    }
}