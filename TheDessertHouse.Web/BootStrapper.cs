using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using AutoMapper;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Context;
using StructureMap;
using TheDessertHouse.Domain;
using TheDessertHouse.Web.Models;

namespace TheDessertHouse.Web
{
    public class BootStrapper
    {
        public void Init()
        {
            log4net.Config.XmlConfigurator.Configure();
            HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();
            BuildNHibernateSessionFactory();
            AreaRegistration.RegisterAllAreas();

            RegisterRoutes(RouteTable.Routes);

            var container = new Container(x => x.AddRegistry(new DessertHouseRegistry()));
            RegisterGlobalFilters(GlobalFilters.Filters);
            DependencyResolver.SetResolver(new TheDessertHouseDependencyResolver(container));
            FilterProviders.Providers.Add(new DessertHouseFilterProvider(container));
            MapDomainAndViewTypes();

        }

        private static void BuildNHibernateSessionFactory()
        {

            var configuration = Fluently.Configure().Database(MsSqlConfiguration.MsSql2008.ConnectionString(
                s => s.FromConnectionStringWithKey("TheDessertHouseConnection")).CurrentSessionContext<WebSessionContext>().AdoNetBatchSize(100))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Article>())
                .BuildConfiguration();
            SessionFactory = configuration.BuildSessionFactory();
        }

        public static ISessionFactory SessionFactory { get; private set; }

        public static void MapDomainAndViewTypes()
        {
            Mapper.CreateMap<UserInformationView, UserInformation>();
            Mapper.CreateMap<UserInformation, UserInformationView>();
            Mapper.CreateMap<Article, ArticleView>();
            Mapper.CreateMap<ArticleView, Article>().ForMember("Id", e => e.UseDestinationValue());
            Mapper.CreateMap<Category, CategoryView>();
            Mapper.CreateMap<CategoryView, Category>().ForMember("Id", cv => cv.UseDestinationValue());
            Mapper.CreateMap<Comments, CommentsView>();
            Mapper.CreateMap<CommentsView, Comments>();
            Mapper.CreateMap<PollView, Poll>();
            Mapper.CreateMap<Poll, PollView>();
            Mapper.CreateMap<PollOptions, PollOptionView>();
            Mapper.CreateMap<PollOptionView, PollOptions>();
            Mapper.CreateMap<NewsletterView, Newsletter>();
            Mapper.CreateMap<Newsletter, NewsletterView>();
            Mapper.CreateMap<Forum, ForumView>();
            Mapper.CreateMap<ForumView, Forum>();
            Mapper.CreateMap<ForumPost, ForumPostView>().ForMember(m => m.Replies, f => f.MapFrom(p => p.Replies));
                //ForMember(p => p.Replies, x => x.Condition(f => f.ReplyCount > 0));
            Mapper.CreateMap<ForumPost, ForumPostReplyView>();
            Mapper.CreateMap<ForumPostView, ForumPost>();
            Mapper.CreateMap<ForumPostVote, ForumPostVoteView>();
            Mapper.CreateMap<ProductView, Product>().ForAllMembers(m => m.Condition(r => (!r.IsSourceValueNull)));
            Mapper.CreateMap<Product, ProductView>().ForMember(x=>x.CategoryId,p=>p.MapFrom(u=>u.Id));
            Mapper.CreateMap<Department, DepartmentView>();
            Mapper.CreateMap<DepartmentView, Department>();
            Mapper.CreateMap<ShippingMethod, ShippingMethodView>();


        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("ViewArticle", "Article/ViewArticle/{articleId}/{path}",
                            new
                                {
                                    controller = "Article",
                                    action = "ViewArticle",
                                    articleId = 1,
                                    path = (string)null
                                },
                            new { articleId = "[0-9]+", path = "[a-zA-Z0-9\\-]*" });

            
            routes.MapRoute("", "Forum/EditForum/{forumId}", new { controller = "Forum", action = "EditForum" });
            
            routes.MapRoute("", "Forum/ViewForum/{forumId}/{path}/{pageNum}", new { controller = "Forum", action = "ViewForum", forumId = 1, path = (string)null, pageNum = 1 });
            routes.MapRoute("", "Forum/ViewPost/{postId}/{path}/{pageNum}", new { controller = "Forum", action = "ViewPost", postId = 1, path = (string)null, pageNum = 1 });
            
            routes.MapRoute("", "Account/ManageUsers/{pageNum}",
                            new { controller = "Account", action = "ManageUsers", pageNum = 1 });

            routes.MapRoute("", "Article/DeleteArticle/{articleId}", new { controller = "Article", action = "DeleteArticle",articleId=UrlParameter.Optional });
            routes.MapRoute("", "Article/{action}/{pageNum}",
                            new { controller = "Article", action = "ManageArticles", pageNum = 1 });

            routes.MapRoute("", "StoreAdmin/EditDepartment/{departmentId}",
                            new {controller = "StoreAdmin", action = "EditDepartment", departmentId = 1});

            routes.MapRoute("", "Store/ViewDepartment/{departmentId}",
                            new { controller = "Store", action = "ViewDepartment", departmentId = 1 });
            
            routes.MapRoute(
                "",
                "{controller}/{action}/{pageNum}/{category}",
                new { controller = "Home", action = "Index", pageNum = 1, category = (string)null }
                );

            routes.MapRoute(
                "Default",
                "{controller}/{action}",
                new { controller = "Home", action = "Index" }
                );

        }
    }
}