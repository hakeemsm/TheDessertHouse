using System;
using System.Collections;
using System.Collections.Generic;
using FluentNHibernate.Cfg;
using FluentNHibernate.Testing;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using NHibernate.Tool.hbm2ddl;
using TheDessertHouse.Domain;
using FluentNHibernate.Cfg.Db;
using NUnit.Framework;

namespace TheDessertHouse.Tests
{
    [TestFixture,Explicit]
    public class OrmPersistenceTests
    {
        private ISession _session;

        [SetUp]
        public void Setup()
        {
            Configuration config = GenerateMapping();
            
            _session = config.BuildSessionFactory().OpenSession();
        }

        private Configuration GenerateMapping()
        {
            return Fluently.Configure().Database(
                SQLiteConfiguration.Standard.ConnectionString(cs => cs.FromConnectionStringWithKey("DessertHouseConnection"))
                .CurrentSessionContext<WebSessionContext>().AdoNetBatchSize(100).ShowSql())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Article>())
                .BuildConfiguration();
        }

        [Test]
        public void Articles_Can_Be_Added_And_Removed()
        {
            object catId;
            var category = new Category
                                    {
                                        DateAdded = DateTime.Now.AddDays(-3),
                                        AddedBy = "admin",
                                        Title = "Ice Creams",
                                        Path = "/Ice",
                                        Importance = 2,
                                        Description = "yada yada yada",
                                        ImageUrl = ""

                                    };
            using (var trx = _session.BeginTransaction())
            {
                _session.Save(category);
                trx.Commit();
                _session.Clear();
            }

            using (var trx = _session.BeginTransaction())
            {

                new PersistenceSpecification<Article>(_session, new PocoComparer())
                    .CheckProperty(p => p.ArticleCategory, category)
                    .CheckProperty(p => p.Path, "")
                    .CheckProperty(p => p.Body, "")
                    .CheckProperty(p => p.DateAdded, DateTime.Now)
                    .CheckProperty(p => p.AddedBy, "Admin")
                    .CheckProperty(p => p.ReleaseDate, DateTime.Now.AddDays(2))
                    .CheckProperty(p => p.ExpireDate, DateTime.Now.AddDays(7))
                    .CheckProperty(p => p.Abstract, "Bake cake")
                    .CheckProperty(p => p.Title, "Yummy cakes")
                    .CheckProperty(p => p.CommentsEnabled, true)
                    .CheckProperty(p => p.Approved, true)
                    .CheckProperty(p => p.Listed, false)
                    .CheckProperty(p => p.OnlyForMembers, false)
                    .CheckProperty(p => p.TotalRating, 0)
                    .CheckProperty(p => p.ViewCount, 0)
                    .CheckProperty(p => p.Votes, 0)
                    .VerifyTheMappings();

                trx.Rollback();
            }
        }

        [Test]
        public void Categories_Can_Be_Added_And_Removed()
        {
            using (var trx = _session.BeginTransaction())
            {
                try
                {
                    new PersistenceSpecification<Category>(_session, new PocoComparer())
                        .CheckProperty(p => p.DateAdded, DateTime.Now)
                        .CheckProperty(p => p.AddedBy, "Admin")
                        .CheckProperty(p => p.Title, "Ice Creams")
                        .CheckProperty(p => p.Path, "/Ice Creams")
                        .CheckProperty(p => p.Importance, 1)
                        .CheckProperty(p => p.Description, "blah..blah..blah")
                        .CheckProperty(p => p.ImageUrl, "~/img1.png")
                        .VerifyTheMappings();
                }
                finally
                {
                    trx.Rollback();
                }

            }
        }

        [Test]
        public void Comments_Can_Be_Added_And_Removed()
        {
            using (var trx = _session.BeginTransaction())
            {
                new PersistenceSpecification<Comments>(_session, new PocoComparer())
                    .CheckProperty(p => p.Id, 1)
                    .CheckProperty(p => p.DateAdded, DateTime.Now)
                    .CheckProperty(p => p.AddedBy, "user1")
                    .CheckProperty(p => p.AddedByEmail, "user1@somedomain.com")
                    .CheckProperty(p => p.AddedByIP, "0.0.0.0")
                    //.CheckProperty(p => p.ForArticle, article)
                    .CheckProperty(p => p.Body, "blah..blah..blah")
                    .VerifyTheMappings();
                trx.Rollback();
            }
        }

        [Test]
        public void Polls_Can_Be_Added_And_Retrieved()
        {
            using (var trx = _session.BeginTransaction())
            {
                new PersistenceSpecification<Poll>(_session, new PocoComparer())
                    .CheckProperty(p => p.DateAdded, DateTime.Now)
                    .CheckProperty(p => p.AddedBy, "admin1")
                    .CheckProperty(p => p.PollQuestion, "Do you like desserts")
                    .CheckProperty(p => p.Path, "")
                    .CheckProperty(p => p.IsCurrent, true)
                    .CheckProperty(p => p.IsArchived, false)
                    .VerifyTheMappings();
                trx.Rollback();

            }
        }

        [Test]
        public void Poll_Options_Are_Linked_To_Poll_Question_In_Poll_Table()
        {
            var poll = new Poll
                           {
                               AddedBy = "admin",
                               DateAdded = DateTime.Now.AddHours(-2),
                               IsArchived = false,
                               IsCurrent = true,
                               PollQuestion = "What is best type of dessert",
                               Path = ""
                           };

            using (var trx = _session.BeginTransaction())
            {
                _session.Save(poll);
           
                new PersistenceSpecification<PollOptions>(_session, new PocoComparer())
                        .CheckProperty(p => p.DateAdded, DateTime.Now.AddHours(-2))
                        .CheckProperty(p => p.OptionText, "Chocolate")
                        .CheckProperty(p => p.Votes, 10)
                        .CheckProperty(p => p.ForPoll, poll)
                        .VerifyTheMappings();
                trx.Rollback();
                
            }

        }

        [Test]
        public void Can_Write_To_And_Read_From_Newsletters_Table()
        {
            using (var trx=_session.BeginTransaction())
            {
                new PersistenceSpecification<Newsletter>(_session, new PocoComparer())
                    .CheckProperty(p => p.DateAdded, DateTime.Now.AddDays(-2))
                    .CheckProperty(p => p.AddedBy, "Admin")
                    .CheckProperty(p => p.Subject, "Chocchip Recipe")
                    .CheckProperty(p => p.PlainTextBody, "blah..blah..blah")
                    .CheckProperty(p => p.HtmlBody, "<html><body>blah..blah..blah</body></body>")
                    .CheckProperty(p => p.Status, "Sent")
                    .CheckProperty(p => p.DateSent, DateTime.Now.AddDays(-1))
                    .VerifyTheMappings();
                trx.Rollback();
            }

        }

        [Test]
        public void Forums_Can_Be_Inserted_And_Removed()
        {
            using (var trx=_session.BeginTransaction())
            {
                new PersistenceSpecification<Forum>(_session, new PocoComparer())
                    .CheckProperty(p => p.DateAdded, DateTime.Now.AddDays(-2))
                    .CheckProperty(p => p.AddedBy, "Admin")
                    .CheckProperty(p => p.Title, "Sugar")
                    .CheckProperty(p => p.Path, "Sugar_Is_Sweet")
                    .CheckProperty(p => p.Moderated, true)
                    .CheckProperty(p => p.Importance, 2)
                    .CheckProperty(p => p.Description, "Sweetness")
                    .VerifyTheMappings();
                trx.Rollback();


            }
        }

        [Test]
        public void Forum_Posts_Can_Be_Added_AndRemoved()
        {
            using (var trx = _session.BeginTransaction())
            {
                var forum = new Forum
                            {
                                AddedBy = "Admin",
                                DateAdded = DateTime.Now.AddDays(-1),
                                Description = "Test Forum",
                                Importance = 0,
                                Moderated = false,
                                Path = "",
                                Title = ""
                            };
                _session.Save(forum);
                var forumPost = new ForumPost
                                    {
                                        AddedBy = "Admin",
                                        DateAdded = DateTime.Now.AddDays(-2),
                                        AddedByIP = "127.0.0.1",
                                        Approved = true,
                                        Closed = false,
                                        Body = "",
                                        Forum = forum,
                                        Title = "Sugar",
                                        Path = "",
                                        ParentPostId = null
                                    };
                new PersistenceSpecification<ForumPost>(_session, new PocoComparer())
                    .VerifyTheMappings(forumPost);
                trx.Rollback();


            }
        }

        [Test]
        public void Forum_Post_Replies_Are_Persisted_And_Retrieved()
        {
            using (var trx = _session.BeginTransaction())
            {
                var forum = new Forum
                {
                    AddedBy = "Admin",
                    DateAdded = DateTime.Now.AddDays(-1),
                    Description = "Test Forum",
                    Importance = 0,
                    Moderated = false,
                    Path = "",
                    Title = ""
                };
               
                var forumPost = new ForumPost
                {
                    AddedBy = "Admin",
                    DateAdded = DateTime.Now.AddDays(-2),
                    AddedByIP = "127.0.0.1",
                    Approved = true,
                    Closed = false,
                    Body = "",
                    Forum = forum,
                    Title = "Sugar",
                    Path = "",
                    ParentPostId = null
                };
                _session.Save(forum);
                var forumPostReply = new ForumPost
                {
                    AddedBy = "User",
                    DateAdded = DateTime.Now.AddDays(-1),
                    AddedByIP = "1.0.0.1",
                    Approved = true,
                    Closed = false,
                    Body = "This is the reply",
                    Forum = forum,
                    Title = "Sugar",
                    Path = "",
                    ParentPostId = forumPost.Id
                };
                forumPost.Replies = new List<ForumPost>{forumPostReply};
                _session.Save(forumPost);
                new PersistenceSpecification<ForumPost>(_session, new PocoComparer())
                    .VerifyTheMappings(forumPost);
                trx.Rollback();


            }
        }

        [Test]
        public void Votes_Can_Be_Cast_For_A_Given_Post()
        {
            using (var trx=_session.BeginTransaction())
            {
                var forum = new Forum
                {
                    AddedBy = "Admin",
                    DateAdded = DateTime.Now.AddDays(-1),
                    Description = "Test Forum",
                    Importance = 0,
                    Moderated = false,
                    Path = "",
                    Title = ""
                };

                var forumPost = new ForumPost
                {
                    AddedBy = "Admin",
                    DateAdded = DateTime.Now.AddDays(-2),
                    AddedByIP = "127.0.0.1",
                    Approved = true,
                    Closed = false,
                    Body = "",
                    Forum = forum,
                    Title = "Sugar",
                    Path = "",
                    ParentPostId = null
                };
                forum.Posts = new List<ForumPost> {forumPost};
                _session.Save(forum);
                var voteId = new VoteId {ForumPostId = forumPost.Id, AddedBy = "user"};
                new PersistenceSpecification<ForumPostVote>(_session, new PocoComparer())
                    //.CheckProperty(x => x.Id, voteId)
                    .CheckProperty(x => x.DateAdded, DateTime.Now.AddHours(-1))
                    .CheckProperty(x => x.AddedByIP, "0.0.0.0")
                    .CheckProperty(x => x.Direction, 1)
                    .CheckProperty(x=>x.AddedBy,"user99")
                    .VerifyTheMappings();
                trx.Rollback();

            }
        }

        [Test]
        public void Voting_On_A_Post_Persists_Updated_Vote_Count()
        {
            using (var trx = _session.BeginTransaction())
            {
                var forum = new Forum
                                {
                                    AddedBy = "Admin",
                                    DateAdded = DateTime.Now.AddDays(-1),
                                    Description = "Test Forum",
                                    Importance = 0,
                                    Moderated = false,
                                    Path = "",
                                    Title = ""
                                };

                var forumPost = new ForumPost
                                    {
                                        AddedBy = "Admin",
                                        DateAdded = DateTime.Now.AddDays(-2),
                                        AddedByIP = "127.0.0.1",
                                        Approved = true,
                                        Closed = false,
                                        Body = "",
                                        Forum = forum,
                                        Title = "Sugar",
                                        Path = "",
                                        ParentPostId = null
                                    };
                _session.Save(forum);
                var voteId = new VoteId {AddedBy = "user1", ForumPostId = forumPost.Id};
                var vote = new ForumPostVote
                                        {
                                            Post = forumPost,
                                            AddedByIP = "0.1.1.1",
                                            DateAdded = DateTime.Now.AddHours(-1),
                                            Direction = -1,
                                            AddedBy = "user99"
                                            
                                        };
                forumPost.Votes=new List<ForumPostVote>{vote};
                forumPost.VoteCount = vote.Direction;
                _session.Save(forumPost);
                var persistedPost = _session.Get<ForumPost>(forumPost.Id);
                var savedVote = _session.Get<ForumPostVote>(vote.Id);
                Assert.That(persistedPost.VoteCount,Is.EqualTo(-1));
                Assert.That(savedVote.Id,Is.EqualTo(persistedPost.Id));
                
                trx.Rollback();
            }
        }

        [Test]
        public void Every_Product_Belongs_To_A_Department()
        {
            var department = new Department
                                 {
                                     DateAdded = DateTime.Now.AddDays(-10),
                                     AddedBy = "StoreAdmin",
                                     Title = "Books",
                                     Importance = 1,
                                     Description = "The Book store",
                                     ImageUrl = "book.png"
                                 };
            var product = new Product
                              {
                                  DateAdded = DateTime.Now.AddDays(-2),
                                  AddedBy = "StoreAdmin",
                                  Title = "Ice Dream",
                                  Description = "All about Ice creams",
                                  SKU = "A1b2",
                                  UnitPrice = 24.34M,
                                  DiscountPercentage = 0.33M,
                                  UnitsInStock = 20,
                                  SmallImageUrl = "small.jpg",
                                  FullImageUrl = "big.png"
                              };
            using (var trx=_session.BeginTransaction())
            {
                new PersistenceSpecification<Department>(_session, new PocoComparer()).VerifyTheMappings(department);
                product.Category = department;
                department.Products = new List<Product> {product};
                _session.Save(department);
                var savedDept = _session.Get<Department>(department.Id);
                Assert.That(savedDept.Products.Count,Is.EqualTo(1));
                Assert.That(savedDept.Products[0].AddedBy,Is.EqualTo("StoreAdmin"));
                Assert.That(savedDept.Products[0].SKU,Is.EqualTo("A1b2"));
                trx.Rollback();

            }
        }

        [Test]
        public void Orders_Are_Composed_Of_Order_Items()
        {
            var orderItem1 = new OrderItem
                                {
                                    DateAdded = DateTime.Now.AddMinutes(-30),
                                    AddedBy = "shopper1",
                                    ProductId = 4,
                                    Title = "Cookbook",
                                    SKU = "sku1",
                                    UnitPrice = 10.0M,
                                    Quantity = 10
                                };
            var orderItem2 = new OrderItem
                                {
                                    DateAdded = DateTime.Now.AddMinutes(-30),
                                    AddedBy = "shopper1",
                                    ProductId = 5,
                                    Title = "Apple Pie",
                                    SKU = "sku11",
                                    UnitPrice = 11.11M,
                                    Quantity = 10
                                };
            var order = new Order
                            {
                                DateAdded = DateTime.Now.AddHours(-1),
                                AddedBy = "shopper1",
                                Status = "pending",
                                ShippingMethod = "Overnight",
                                SubTotal = 33.23M,
                                Shipping = 11.33M,
                                ShippingFirstName = "John",
                                ShippingLastName = "Doe",
                                ShippingStreet = "123 Main",
                                ShippingZipCode = "11234",
                                ShippingCity = "Springfield",
                                ShippingState = "OH",
                                CustomerEmail = "me@whatever.com",
                                ShippedDate = null,
                                TransactionId = "trxn007",
                                TrackingId = "ax1234",
                                Items = new List<OrderItem> {orderItem1,orderItem2 }
                            };
            orderItem1.Order = order;
            orderItem2.Order = order;
            using (var trx=_session.BeginTransaction())
            {
                new PersistenceSpecification<Order>(_session,new PocoComparer()).TransactionalSave(order);
                var savedOrder = _session.Get<Order>(order.Id);
                Assert.That(savedOrder.Items.Count,Is.EqualTo(2));

                savedOrder.Items.Remove(orderItem2);
                _session.Save(savedOrder);
                savedOrder = _session.Get<Order>(order.Id);
                Assert.That(savedOrder.Items.Count, Is.EqualTo(1));
            }

        }

        [Test]
        public void ShippingMethods_Are_Persisted_And_Retrievable()
        {
            var shippingMethod = new ShippingMethod
                                     {
                                         DateAdded = DateTime.Now.AddMinutes(-30),
                                         AddedBy = "shopper1",
                                         Title = "Cookbook",
                                         Price = 10.0M
                                     };
            using (var trx=_session.BeginTransaction())
            {
                new PersistenceSpecification<ShippingMethod>(_session,new PocoComparer()).TransactionalSave(shippingMethod);
            }
        }

        [TearDown]
        public void TearDown()
        {
            _session.Dispose();
        }
    }

    public class PocoComparer : IEqualityComparer
    {
        public new bool Equals(object x, object y)
        {
            if (null == x || null == y)
                return false;
            if (x.GetType().Equals(y.GetType()))
            {
                if (x is DateTime)
                    return ((DateTime)x).ToString("yyyyMMdd").Equals(((DateTime)y).ToString("yyyyMMdd"));
                if (x is IEntity)
                    return ((IEntity)x).Id == ((IEntity)y).Id;
            }
            return x.Equals(y);
        }

        public int GetHashCode(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
