using System;

namespace TheDessertHouse.Domain
{
    public class ForumPostVote:IEntity
    {
        public virtual int Id { get; set; }

        public virtual DateTime DateAdded { get; set; }

        public virtual string AddedByIP { get; set; }

        public virtual int Direction { get; set; }

        public virtual ForumPost Post { get; set; }

        public virtual string AddedBy { get; set; }
    }

    [Serializable]
    public class VoteId
    {
        public string AddedBy { get; set; }

        public int ForumPostId { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (VoteId)) return false;
            return Equals((VoteId) obj);
        }

        public bool Equals(VoteId other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.ForumPostId == ForumPostId && string.Equals(other.AddedBy, AddedBy);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (ForumPostId*666) ^ (AddedBy != null ? AddedBy.GetHashCode() : 0);
            }
        }
    }
}