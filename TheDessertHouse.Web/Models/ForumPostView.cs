using System;
using System.Collections.Generic;
using TheDessertHouse.Domain;

namespace TheDessertHouse.Web.Models
{
    public class ForumPostViewBase
    {
        public int Id { get; set; }

        public DateTime DateAdded { get; set; }

        public string AddedBy { get; set; }

        public string AddedByIP { get; set; }

        public string Title { get; set; }

        public string Path { get; set; }

        public string Body { get; set; }

        public bool Approved { get; set; }

        public bool Closed { get; set; }

        public int VoteCount { get; set; }

        public long ViewCount { get; set; }

        public int ReplyCount { get; set; }

        public string LastPostBy { get; set; }

        public DateTime LastPostDate { get; set; }

        public int? ParentPostId { get; set; }

        //public ForumView Forum { get; set; }

        public IList<ForumPostVoteView> Votes { get; set; }

        public string Avatar { get; set; }

        public string LastPostDateInUTC { get { return LastPostDate != default(DateTime) ? LastPostDate.SinceTime() : ""; } }
    }

    public class ForumPostView:ForumPostViewBase
    {
        public IEnumerable<ForumPostReplyView> Replies { get; set; }

        public Pagination<ForumPostReplyView> PagedReplies { get; set; }

        public int UserVote { get; set; }
    }

    public class ForumPostReplyView:ForumPostViewBase
    {

    }
}