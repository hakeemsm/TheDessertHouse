using System;

namespace TheDessertHouse.Web.Models
{
    public class NewsletterView
    {
        public  DateTime DateAdded { get; set; }

        public  string AddedBy { get; set; }

        public  string Subject { get; set; }

        public  string PlainTextBody
        {
            get { return HtmlBody; }
            set { /*HtmlBody = value;*/ }
        }

        public  string HtmlBody { get; set; }

        public  string Status { get; set; }

        public  DateTime DateSent { get; set; }

        public  int Id { get; set; }
    }
}