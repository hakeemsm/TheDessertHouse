using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.TestHelper;
using MvcContrib.TestHelper.Fakes;
using NUnit.Framework;
using TheDessertHouse.Web;
using TheDessertHouse.Web.Controllers;
using TheDessertHouse.Web.Models;

namespace TheDessertHouse.Tests
{
    [TestFixture]
    public class NavigationTests
    {
        [SetUp]
        public void Setup()
        {
            RouteTable.Routes.Clear();
            BootStrapper.RegisterRoutes(RouteTable.Routes);
        }

        [Test]
        public void Root_Navigates_To_Home_Index()
        {
            RouteTable.Routes.GetRouteData(new FakeHttpContext("~/")).ShouldMapTo<HomeController>(hc => hc.Index(1,null));
            
        }

        [Test]
        public void ViewArticle_Returns_Article_Id_And_Path_In_Url()
        {
            //OutBoundUrl.OfRouteNamed("ViewArticle").ShouldMapToUrl("/Article/ViewArticle/1");
            "~/Article/ViewArticle/6/peaches-and-cream".Route().ShouldMapTo<ArticleController>(
                a => a.ViewArticle(6, "peaches-and-cream"));
        }

        [Test]
        public void Category_Link_Maps_To_Specified_Category()
        {
            "~/Home/Index/6/Pies".Route().ShouldMapTo<HomeController>(a => a.Index(6, "Pies"));
            OutBoundUrl.Of<HomeController>(c => c.Index(4, "Pies")).ShouldMapToUrl("/Home/Index/4/Pies");
        }

        [Test]
        public void Create_Category_Posts_To_CreateCategory_Action()
        {
            "~/Category/CreateCategory".WithMethod(HttpVerbs.Post).ShouldMapTo<CategoryController>(
                a => a.CreateCategory(new CategoryView{CategoryExists=false,Description = "",ImageUrl = "",Importance=1,Title = ""}));
        }

        [Test]
        public void Manage_Articles_Gets_Routed_WIth_The_Correct_Page_Number()
        {
            "~/Article/ManageArticles/2".Route().ShouldMapTo<ArticleController>(a => a.ManageArticles(2));
        }

        [Test]
        public void Manage_Comments_Gets_Routed_WIth_The_Correct_Page_Number()
        {
            "~/Article/ManageComments/10".Route().ShouldMapTo<ArticleController>(a => a.ManageComments(10));
        }

        [Test]
        public void Manage_Users_Gets_Routed_WIth_The_Correct_Page_Number()
        {
            "~/Account/ManageUsers/20".Route().ShouldMapTo<AccountController>(a => a.ManageUsers(20));
        }

        [Test]
        public void Next_And_Previous_On_Home_Page_Are_Routed_Properly()
        {
            OutBoundUrl.Of<HomeController>(h=>h.Index(2,null)).ShouldMapToUrl("/Home/Index/2");
        }

        [Test]
        public void Edit_Forum_Link_Has_ForumId_In_The_Path()
        {
            "~/Forum/EditForum/1".ShouldMapTo<ForumController>(f => f.EditForum(1));
        }

        [Test]
        public void View_Forum_Link_Has_ForumId_And_ForumPath()
        {
            "~/Forum/ViewForum/2/I_Hate_sugar/2".ShouldMapTo<ForumController>(c => c.ViewForum(2, "I_Hate_sugar",2));
        }

        [Test]
        public void Link_With_PostId_And_Title_Maps_To_View_Post()
        {
            "~/Forum/ViewPost/2/I_Love_sugar".ShouldMapTo<ForumController>(c => c.ViewPost(2, "I_Love_sugar",1));
        }

        [Test]
        public void Edit_Department_With_Dept_Id_Maps_To_StoreAdmin_EditDepartment()
        {
            "~/StoreAdmin/EditDepartment/2".ShouldMapTo<StoreAdminController>(s => s.EditDepartment(2));
        }

        [Test]
        public void View_Department_With_Dept_Id_Maps_To_StoreAdmin_ViewDepartment()
        {
            "~/Store/ViewDepartment/11".ShouldMapTo<StoreController>(s => s.ViewDepartment(11));
        }

        [Test]
        public void Delete_Article_Gets_Routed_With_ArticleId()
        {
            "~/Article/DeleteArticle/1".ShouldMapTo<ArticleController>(a => a.DeleteArticle(1));
        }

    }
}
