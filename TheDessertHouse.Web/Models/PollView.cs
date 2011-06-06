using System;
using System.Collections.Generic;

namespace TheDessertHouse.Web.Models
{
    public class PollView
    {
        public int Id { get; set; }

        public bool IsArchived { get; set; }

        public string PollQuestion { get; set; }

        public IEnumerable<PollOptionView> Options { get; set; }

        public bool ExistingPoll { get; set; }

        public bool IsCurrent { get; set; }

        public int TotalVotes { get; set; }
    }

    public class PollOptionView
    {
        public int Id { get; set; }

        public string OptionText { get; set; }

        public virtual int Votes { get; set; }
        
    }
}