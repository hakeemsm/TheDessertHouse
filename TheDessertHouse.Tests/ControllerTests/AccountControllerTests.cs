using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Moq;
using NUnit.Framework;
using TheDessertHouse.Domain;
using TheDessertHouse.Web;
using TheDessertHouse.Web.Controllers;
using TheDessertHouse.Web.Models;

namespace TheDessertHouse.Tests.ControllerTests
{
    [TestFixture]
    public class AccountControllerTests:BaseTestController
    {
        private Mock<IMembershipProvider> _memberShipMock;
        //private UserInformation _userInformation;
        private UserInformationView _userInformationView;

        [SetUp]
        public void Setup()
        {
            _memberShipMock = new Mock<IMembershipProvider>();
           
            _userInformationView = new UserInformationView
            {
                UserName = "spiderman#1",
                Password = "abcd",
                ConfirmPassword = "abcd",
                Email = "",
                SecretQuestion = "",
                HintAnswer = "",
                //ReturnUrl = "www.asp.net/mvc"
            };
            BootStrapper.MapDomainAndViewTypes();
        }

        [Test]
        public void Create_Membership_Fails_For_Duplicate_User()
        {


            MembershipCreateResult memberShipCreateResult = new MembershipCreateResult();
            memberShipCreateResult.CreateStatus = MembershipCreateStatus.DuplicateUserName;
            _memberShipMock.Setup(x => x.CreateUser(It.IsAny<UserInformation>(), true)).Returns(memberShipCreateResult);

            var accountController = new AccountController(_memberShipMock.Object);
            var actionResult = accountController.Register(_userInformationView);
            
            Assert.That(((ViewResult)actionResult).ViewBag.ErrorMessage, Is.EqualTo("Username already exists. Please enter a different user name."));
            
            Assert.That(((ViewResult)actionResult).Model, Is.EqualTo(_userInformationView));
            Assert.That(((ViewResult)actionResult).ViewData["PageTitle"], Is.EqualTo("Register New User"));
        }

        [Test]
        public void Create_Membership_Fails_For_Duplicate_Email()
        {
            MembershipCreateResult memberShipCreateResult = new MembershipCreateResult { MembershipUserObject = null, CreateStatus = MembershipCreateStatus.DuplicateEmail };

            _memberShipMock.Setup(x => x.CreateUser(It.IsAny<UserInformation>(), true)).Returns(memberShipCreateResult);

            var accountController = new AccountController(_memberShipMock.Object);
            var actionResult = accountController.Register(_userInformationView);

            Assert.That(((ViewResult)actionResult).ViewBag.ErrorMessage, Is.EqualTo("A username for that e-mail address already exists. Please enter a different e-mail address."));
            Assert.That(((ViewResult)actionResult).ViewData.Model, Is.EqualTo(_userInformationView));
            Assert.That(((ViewResult)actionResult).ViewData["PageTitle"], Is.EqualTo("Register New User"));
        }

        [Test]
        public void Create_Membership_Fails_For_Invalid_Password()
        {
            MembershipCreateResult memberShipCreateResult = new MembershipCreateResult { MembershipUserObject = null, CreateStatus = MembershipCreateStatus.InvalidPassword };
            _memberShipMock.Setup(x => x.CreateUser(It.IsAny<UserInformation>(), true)).Returns(memberShipCreateResult);

            var accountController = new AccountController(_memberShipMock.Object);
            var actionResult = accountController.Register(_userInformationView);

            Assert.That(((ViewResult)actionResult).ViewBag.ErrorMessage, Is.EqualTo("The password provided is invalid. Please enter a valid password value."));
            Assert.That(((ViewResult)actionResult).ViewData.Model, Is.EqualTo(_userInformationView));
            Assert.That(((ViewResult)actionResult).ViewData["PageTitle"], Is.EqualTo("Register New User"));
        }

        [Test]
        public void Create_Membership_Fails_For_Invalid_Email()
        {
            MembershipCreateResult memberShipCreateResult = new MembershipCreateResult { MembershipUserObject = null, CreateStatus = MembershipCreateStatus.InvalidEmail };
            _memberShipMock.Setup(x => x.CreateUser(It.IsAny<UserInformation>(), true)).Returns(memberShipCreateResult);

            var accountController = new AccountController(_memberShipMock.Object);
            var actionResult = accountController.Register(_userInformationView);

            Assert.That(((ViewResult)actionResult).ViewBag.ErrorMessage, Is.EqualTo("The e-mail address provided is invalid. Please check the value and try again."));
            Assert.That(((ViewResult)actionResult).ViewData.Model, Is.EqualTo(_userInformationView));
            Assert.That(((ViewResult)actionResult).ViewData["PageTitle"], Is.EqualTo("Register New User"));
        }

        [Test]
        public void Create_Membership_Fails_For_Invalid_UserName()
        {
            MembershipCreateResult memberShipCreateResult = new MembershipCreateResult { MembershipUserObject = null, CreateStatus = MembershipCreateStatus.InvalidUserName };
            _memberShipMock.Setup(x => x.CreateUser(It.IsAny<UserInformation>(), true)).Returns(memberShipCreateResult);

            var accountController = new AccountController(_memberShipMock.Object);
            var actionResult = accountController.Register(_userInformationView);

            Assert.That(((ViewResult)actionResult).ViewBag.ErrorMessage, Is.EqualTo("The user name provided is invalid. Please check the value and try again."));
            Assert.That(((ViewResult)actionResult).ViewData.Model, Is.EqualTo(_userInformationView));
            Assert.That(((ViewResult)actionResult).ViewData["PageTitle"], Is.EqualTo("Register New User"));
        }

        [Test]
        public void Create_Membership_Fails_For_Invalid_Question()
        {
            MembershipCreateResult memberShipCreateResult = new MembershipCreateResult { MembershipUserObject = null, CreateStatus = MembershipCreateStatus.InvalidQuestion };
            _memberShipMock.Setup(x => x.CreateUser(It.IsAny<UserInformation>(), true)).Returns(memberShipCreateResult);

            var accountController = new AccountController(_memberShipMock.Object);
            var actionResult = accountController.Register(_userInformationView);

            Assert.That(((ViewResult)actionResult).ViewBag.ErrorMessage, Is.EqualTo("The password retrieval question provided is invalid. Please check the value and try again."));
            Assert.That(((ViewResult)actionResult).ViewData.Model, Is.EqualTo(_userInformationView));
            Assert.That(((ViewResult)actionResult).ViewData["PageTitle"], Is.EqualTo("Register New User"));
        }

        [Test]
        public void Create_Membership_Fails_For_Invalid_Answer()
        {
            MembershipCreateResult memberShipCreateResult = new MembershipCreateResult { MembershipUserObject = null, CreateStatus = MembershipCreateStatus.InvalidAnswer };
            _memberShipMock.Setup(x => x.CreateUser(It.IsAny<UserInformation>(), true)).Returns(memberShipCreateResult);

            var accountController = new AccountController(_memberShipMock.Object);
            var actionResult = accountController.Register(_userInformationView);

            Assert.That(((ViewResult)actionResult).ViewBag.ErrorMessage, Is.EqualTo("The password retrieval answer provided is invalid. Please check the value and try again."));
            Assert.That(((ViewResult)actionResult).ViewData.Model, Is.EqualTo(_userInformationView));
            Assert.That(((ViewResult)actionResult).ViewData["PageTitle"], Is.EqualTo("Register New User"));
        }

        [Test]
        public void Create_Membership_Failed_Because_User_Rejected()
        {
            MembershipCreateResult memberShipCreateResult = new MembershipCreateResult { MembershipUserObject = null, CreateStatus = MembershipCreateStatus.UserRejected };
            _memberShipMock.Setup(x => x.CreateUser(It.IsAny<UserInformation>(), true)).Returns(memberShipCreateResult);

            var accountController = new AccountController(_memberShipMock.Object);
            var actionResult = accountController.Register(_userInformationView);

            Assert.That(((ViewResult)actionResult).ViewBag.ErrorMessage, Is.EqualTo("The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator."));
            Assert.That(((ViewResult)actionResult).ViewData.Model, Is.EqualTo(_userInformationView));
            Assert.That(((ViewResult)actionResult).ViewData["PageTitle"], Is.EqualTo("Register New User"));
        }

        [Test]
        public void Invalid_Credentials_Causes_Login_To_Fail_After_User_Creation()
        {
            MembershipCreateResult memberShipCreateResult = new MembershipCreateResult { MembershipUserObject = null, CreateStatus = MembershipCreateStatus.Success };
            _memberShipMock.Setup(x => x.CreateUser(It.IsAny<UserInformation>(), true)).Returns(memberShipCreateResult);
            _memberShipMock.Setup(x => x.ValidateUser(_userInformationView.UserName, _userInformationView.Password)).Returns(false);

            var accountController = new AccountController(_memberShipMock.Object);
            var actionResult = accountController.Register(_userInformationView);
            Assert.That(((ViewResult)actionResult).ViewBag.ErrorMessage,
                        Is.EqualTo("Login failed! Please make sure you are using the correct user name and password."));
            Assert.That(((ViewResult)actionResult).ViewData.Model, Is.EqualTo(_userInformationView));
            Assert.That(((ViewResult)actionResult).ViewData["PageTitle"], Is.EqualTo("Register New User"));

        }

        [Test,Ignore]
        public void Successful_Login_After_User_Creation_Redirects_To_Url_If_Specified()
        {
            MembershipCreateResult memberShipCreateResult = new MembershipCreateResult { MembershipUserObject = null, CreateStatus = MembershipCreateStatus.Success };
            //_userInformationView.ReturnUrl = "/";
            _memberShipMock.Setup(x => x.CreateUser(It.IsAny<UserInformation>(), true)).Returns(memberShipCreateResult);
            _memberShipMock.Setup(x => x.ValidateUser(_userInformationView.UserName, _userInformationView.Password)).Returns(true);

            var accountController = new AccountController(_memberShipMock.Object);
            var actionResult = accountController.Register(_userInformationView);

            Assert.That(actionResult, Is.TypeOf<RedirectResult>());
            Assert.That(((RedirectResult)actionResult).Url, Is.EqualTo("/"));

        }

        [Test]
        public void Forgot_Password_Presents_UserInformation_View()
        {
            var accountController = new AccountController(_memberShipMock.Object);
            ViewResult result = accountController.ForgotPassword();

            Assert.That(result.ViewData.Model, Is.TypeOf<UserInformationView>());
            Assert.That(accountController.ViewBag.PageTitle, Is.EqualTo("Forgot Password"));
            
        }

        [Test]
        public void Forgot_Password_Fetches_Hint_Question_For_A_Valid_User()
        {
            var membershipUser = new MembershipUserWrapper { SecretQuestion = "Q1" };
            _memberShipMock.Setup(x => x.GetUser(_userInformationView.UserName, false)).Returns(membershipUser);

            var accountController = new AccountController(_memberShipMock.Object);
            ActionResult result = accountController.ForgotPassword(_userInformationView.UserName, "");

            Assert.That(((ViewResult)result).ViewData.Model, Is.TypeOf<UserInformationView>());
            Assert.That(accountController.ViewBag.PageTitle, Is.EqualTo("Forgot Password"));
            Assert.That(((UserInformationView)((ViewResult)result).ViewData.Model).SecretQuestion, Is.EqualTo("Q1"));

        }

        [Test]
        public void Forgot_Password_Returns_Error_For_Invalid_User()
        {
            MembershipUserWrapper membershipUser = null;
            _memberShipMock.Setup(x => x.GetUser(_userInformationView.UserName, false)).Returns(membershipUser);

            var accountController = new AccountController(_memberShipMock.Object);
            ActionResult result = accountController.ForgotPassword(_userInformationView.UserName, "");

            Assert.That(((ViewResult)result).ViewData.Model, Is.TypeOf<UserInformationView>());
            Assert.That(accountController.ViewBag.PageTitle, Is.EqualTo("Forgot Password"));
            Assert.That(accountController.ViewBag.ErrorMessage, Is.EqualTo("The user you have specified is invalid, please recheck your username and try again"));
            Assert.That(((UserInformationView)((ViewResult)result).ViewData.Model).SecretQuestion, Is.EqualTo(string.Empty));

        }

        [Test]
        public void Forgot_Password_Redirects_To_ChangePassword_For_Correct_Answer()
        {
            _memberShipMock.Setup(x => x.ResetPassword(_userInformationView.UserName, "secretAnswer")).Returns("gennedpwd");
            _memberShipMock.Setup(x => x.ValidateUser(_userInformationView.UserName, "gennedpwd")).Returns(true);

            var accountController = new AccountController(_memberShipMock.Object);

            ActionResult result = accountController.ForgotPassword(_userInformationView.UserName, "secretAnswer");
            Assert.That(result, Is.TypeOf<RedirectToRouteResult>());
        }

        [Test]
        public void Change_Password_Displays_View_With_Temp_Password()
        {
            var accountController = new AccountController(_memberShipMock.Object);
            ActionResult result = accountController.ChangePassword("tempPassword");

            Assert.That(((ViewResult)result).ViewData.Model, Is.TypeOf<UserInformationView>());
            Assert.That(((UserInformationView)((ViewResult)result).ViewData.Model).Password, Is.EqualTo("tempPassword"));
            Assert.That(accountController.ViewBag.PageTitle, Is.EqualTo("Change Password"));
        }

        

        [Test]
        public void Change_Password_Displays_Success_When_Password_Changed()
        {
            var userInformation = new UserInformationView { UserName = "userName", Password = "tempPassword", ChangePassword = "c2d2", ConfirmPassword = "c2d2" };
            _memberShipMock.Setup(x => x.ChangePassword(userInformation.UserName, userInformation.Password, userInformation.ChangePassword)).Returns(true);
            var accountController = new AccountController(_memberShipMock.Object);
            ActionResult result = accountController.ChangePassword(userInformation);

            Assert.That(result, Is.TypeOf<RedirectToRouteResult>());
            Assert.That(((RedirectToRouteResult)result).RouteValues.ContainsValue("Login"));
            Assert.That(((RedirectToRouteResult)result).RouteValues.ContainsKey("action"));
        }

        [Test]
        public void Manage_Users_Gets_No_Of_Registered_Users_And_Users_Online()
        {
            IEnumerable<MembershipUserWrapper> allusers = new List<MembershipUserWrapper>
                                                              {new MembershipUserWrapper{UserName = "user1"}, new MembershipUserWrapper{UserName="user2"}};
            _memberShipMock.Setup(x => x.GetAllUsers()).Returns(allusers);
            //IEnumerable<MembershipUserWrapper> onlineUsers = new List<MembershipUserWrapper>();
            
            _memberShipMock.Setup(x => x.GetNumberOfUsersOnline()).Returns(1);
            var accountController = new AccountController(_memberShipMock.Object);

            //ActionResult result = accountController.ManageUsers("searchType", "searchInput",0);
            ActionResult result = accountController.ManageUsers(1);
            ViewResult viewResult = (ViewResult)result;

            Assert.That(viewResult.ViewData.Model, Is.TypeOf<ManageUsersView>());
            Assert.That(accountController.ViewBag.PageTitle, Is.EqualTo("Account Management"));

            ManageUsersView model = (ManageUsersView)viewResult.ViewData.Model;
            Assert.That(model.RegisteredUsers, Is.EqualTo(2));
            Assert.That(model.UsersOnline, Is.EqualTo(1));
            Assert.That(model.SearchOptionList, Is.TypeOf<SelectList>());
        }

        [Test]
        public void Manage_User_Gets_List_Of_Users_Based_On_User_Name()
        {
            MembershipUserWrapper user = new MembershipUserWrapper { UserName = "userName" };
            _memberShipMock.Setup(x => x.GetUserByUserName("userName")).Returns(user);
            var accountController = new AccountController(_memberShipMock.Object);
            ActionResult result = accountController.ManageUsers("userName", "UserName",0);

            ManageUsersView model = (ManageUsersView)((ViewResult)result).ViewData.Model;
            Assert.That(model.UserList, Is.TypeOf<Pagination<MembershipUserWrapper>>());
            Assert.That(model.UserList.FirstOrDefault().UserName, Is.EqualTo("userName"));

        }

        [Test]
        public void Manage_User_Gets_List_Of_Users_Based_On_Email()
        {
            List<MembershipUserWrapper> users = new List<MembershipUserWrapper> { new MembershipUserWrapper { Email = "me@myself.com" } };
            _memberShipMock.Setup(x => x.GetUsersByEmail("me@myself.com")).Returns(users);
            var accountController = new AccountController(_memberShipMock.Object);
            ActionResult result = accountController.ManageUsers("me@myself.com","Email",1);
            ViewResult viewResult = (ViewResult)result;

            ManageUsersView model = (ManageUsersView)viewResult.Model;
            Assert.That(model.UserList, Is.TypeOf<Pagination<MembershipUserWrapper>>());
            Assert.That(model.UserList.FirstOrDefault().Email, Is.EqualTo("me@myself.com"));
        }

        [Test]
        public void Edit_User_Sets_User_And_Roles_In_View()
        {
            string[] roles = { "User", "Admin" };
            _memberShipMock.Setup(x => x.GetAllRoles()).Returns(roles);
            _memberShipMock.Setup(x => x.GetUser("userId", false)).Returns(new MembershipUserWrapper { UserName = "userId" });
            var accountController = new AccountController(_memberShipMock.Object);
            ActionResult result = accountController.EditUser("userId");
            Assert.That(((ViewResult)result).Model, Is.TypeOf<EditUserView>());
            EditUserView model = (EditUserView)((ViewResult)result).ViewData.Model;
            Assert.That(model.Roles.Length, Is.EqualTo(2));
            Assert.That(model.Roles[0], Is.EqualTo("User"));
            Assert.That(model.MembershipUser, Is.TypeOf<MembershipUserWrapper>());
            Assert.That(model.MembershipUser.UserName, Is.EqualTo("userId"));
        }

        
    }
}
