using System;

namespace TheDessertHouse.Domain
{
    public class PollOptions:IEntity
    {
        public virtual DateTime DateAdded { get; set; }

        

        public virtual string OptionText { get; set; }

        public virtual int Votes { get; set; }

        public virtual Poll ForPoll { get; set; }

        public virtual int Id { get; set; }

        
    }
}