using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TheDessertHouse.Web.Models
{
    public class ArticleView
    {
        public virtual int Id { get; set; }

        private DateTime _dateAdded;
        public virtual DateTime DateAdded
        {
            get
            {
                if (_dateAdded == default(DateTime))
                    _dateAdded = DateTime.Now;
                return _dateAdded;
            }
            set { _dateAdded = value; }
        }

        public virtual string AddedBy { get; set; }

        public virtual CategoryView ArticleCategory { get; set; }


        public virtual string Title { get; set; }

        private string _path;
        public virtual string Path
        {
            get
            {
                if (!string.IsNullOrEmpty(Title))
                    _path = Title.ToUrlFormat();
                return _path;
            }
            set { _path = value; }
        }

        public virtual string Abstract { get; set; }

        public virtual string Body { get; set; }

        public virtual string Country { get; set; }

        public virtual string State { get; set; }

        public virtual string City { get; set; }

        public virtual DateTime ReleaseDate
        {
            get
            {
                if (!string.IsNullOrEmpty(ReleaseDateField))
                    return Convert.ToDateTime(ReleaseDateField);
                return DateTime.Now;
            }
            set { ReleaseDateField = value.ToString("yyyy/MM/dd"); }
        }

        public virtual DateTime ExpireDate
        {
            get
            {
                if (!string.IsNullOrEmpty(ExpireDateField))
                    return Convert.ToDateTime(ExpireDateField);
                return DateTime.Now;
            }
            set { ExpireDateField = value.ToString("yyyy/MM/dd"); }
        }

        public virtual bool Approved { get; set; }

        public virtual bool Listed { get; set; }

        public virtual bool CommentsEnabled { get; set; }

        public virtual bool OnlyForMembers { get; set; }

        public virtual int ViewCount { get; set; }

        public virtual int Votes { get; set; }

        public virtual int TotalRating { get; set; }

        public virtual IList<CommentsView> Comments { get; set; }

        public SelectList Categories { get; set; }

        public bool HasData
        {
            get { return !string.IsNullOrEmpty(Title) && !string.IsNullOrEmpty(Path) && !string.IsNullOrEmpty(Body); }

        }

        [Display(Name = "Category")]
        public string CategoryTitle { get; set; }

        [Display(Name = "Expires on")]
        public string ExpireDateField { get; set; }

        [Display(Name = "Release date")]
        public string ReleaseDateField { get; set; }

        public bool IsDirty { get; set; }

        public double AverageRating { get; set; }

        public string ImageRatingUrl
        {
            get
            {
                string img = "~/Content/images/rating/{0}dessert.jpg";
                if (AverageRating <= 1.0)
                    img = String.Format(img, "1");
                else if (AverageRating <= 2.0)
                    img = String.Format(img, "2");
                else if (AverageRating <= 3.0)
                    img = String.Format(img, "3");
                else if (AverageRating <= 4.0)
                    img = String.Format(img, "4");
                else
                    img = String.Format(img, "5");

                return img;

            }
        }

        public string UserLiteral { get { return Votes > 1 ? "Users" : "User"; }}
    }
}