using System;

namespace TheDessertHouse.Domain
{
    public class Newsletter:IEntity
    {
        public virtual DateTime DateAdded { get; set; }

        public virtual string AddedBy { get; set; }

        public virtual string Subject { get; set; }

        public virtual string PlainTextBody { get; set; }

        public virtual string HtmlBody { get; set; }

        public virtual string Status { get; set; }

        public virtual DateTime? DateSent { get; set; }

        public virtual int Id { get; set; }
    }
}