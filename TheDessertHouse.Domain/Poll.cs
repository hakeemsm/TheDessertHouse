using System;
using System.Collections.Generic;

namespace TheDessertHouse.Domain
{
    public class Poll:IEntity
    {
        public virtual DateTime DateAdded { get; set; }

        public virtual string AddedBy { get; set; }

        public virtual string PollQuestion { get; set; }

        public virtual string Path { get; set; }

        public virtual bool IsCurrent { get; set; }

        public virtual bool IsArchived { get; set; }

        public virtual DateTime? DateArchived { get; set; }

        public virtual int Id { get; set; }

        public virtual IList<PollOptions> Options { get; set; }
    }
}