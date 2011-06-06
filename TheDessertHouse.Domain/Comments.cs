using System;

namespace TheDessertHouse.Domain
{
    public class Comments:IEntity
    {
        public virtual int Id { get; set; }

        public virtual DateTime DateAdded { get; set; }

        public virtual string AddedBy { get; set; }

        public virtual string AddedByEmail { get; set; }

        public virtual string AddedByIP { get; set; }

        public virtual Article ForArticle { get; set; }

        public virtual string Body { get; set; }
    }
}