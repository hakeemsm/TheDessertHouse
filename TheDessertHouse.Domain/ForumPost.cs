using System;
using System.Collections.Generic;

namespace TheDessertHouse.Domain
{
    public class ForumPost:IEntity, CommonEntity
    {
        public virtual int Id { get; set; }

        public virtual DateTime DateAdded { get; set; }

        public virtual string AddedBy { get; set; }

        public virtual string AddedByIP { get; set; }

        public virtual string Title { get; set; }

        public virtual string Path { get; set; }

        public virtual string Body { get; set; }

        public virtual bool Approved { get; set; }

        public virtual bool Closed { get; set; }

        public virtual int VoteCount { get; set; }

        public virtual long ViewCount { get; set; }

        public virtual int ReplyCount { get; set; }

        public virtual string LastPostBy { get; set; }

        public virtual DateTime LastPostDate { get; set; }

        public virtual int? ParentPostId { get; set; }

        public virtual Forum Forum { get; set; }

        public virtual IList<ForumPost> Replies { get; set; }

        public virtual IList<ForumPostVote> Votes { get; set; }
    }
}