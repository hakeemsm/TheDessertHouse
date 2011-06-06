using System;
using System.Collections.Generic;

namespace TheDessertHouse.Domain
{
    public class Forum:IEntity,CommonEntity
    {
        public virtual DateTime DateAdded { get; set; }

        public virtual string AddedBy { get; set; }

        public virtual string Title { get; set; }

        public virtual string Path { get; set; }

        public virtual bool Moderated { get; set; }

        public virtual int Importance { get; set; }

        public virtual string Description { get; set; }

        public virtual int Id { get; set; }

        public virtual IList<ForumPost> Posts { get; set; }
    }
}