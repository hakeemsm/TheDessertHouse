using System;
using System.Collections.Generic;

namespace TheDessertHouse.Domain
{
    public class Article : IEntity,CommonEntity
    {
        public virtual int Id { get; set; }

        public virtual DateTime DateAdded { get; set; }

        public virtual string AddedBy { get; set; }

        public virtual Category ArticleCategory { get; set; }

        public virtual string Title { get; set; }

        public virtual string Path { get; set; }

        public virtual string Abstract { get; set; }

        public virtual string Body { get; set; }

        public virtual string Country { get; set; }

        public virtual string State { get; set; }

        public virtual string City { get; set; }

        public virtual DateTime ReleaseDate { get; set; }

        public virtual DateTime ExpireDate { get; set; }

        public virtual bool Approved { get; set; }

        public virtual bool Listed { get; set; }

        public virtual bool CommentsEnabled { get; set; }

        public virtual bool OnlyForMembers { get; set; }

        public virtual int ViewCount { get; set; }

        public virtual int Votes { get; set; }

        public virtual int TotalRating { get; set; }

        public virtual IList<Comments> Comments { get; set; }

        public virtual double AverageRating
        {
            get
            {
                if (Votes >= 1)
                    return (double) TotalRating/Votes;
                return 0;
            }
        }

        public virtual void Rate(int rating)
        {
            Votes++;
            TotalRating += rating;
        }
    }
}