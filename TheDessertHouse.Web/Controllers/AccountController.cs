using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using FluentNHibernate.Utils;
using TheDessertHouse.Domain;
using TheDessertHouse.Web.Configuration;
using TheDessertHouse.Web.Models;


namespace TheDessertHouse.Web.Controllers
{
    public class AccountController : Controller
    {
        private IMembershipProvider _memberShipProvider;
        //
        // GET: /Account/

        public AccountController(IMembershipProvider membershipProvider)
        {
            _memberShipProvider = membershipProvider;
        }

        public ActionResult Index()
        {
            return RedirectToAction("Register");
        }

        public ActionResult Register()
        {

            ViewBag.PageTitle = "Register new user";
            return View();
        }


        [HttpPost]
        public ActionResult Register(UserInformationView userInformationView)
        {
            try
            {
                var userInformation = Mapper.Map<UserInformationView, UserInformation>(userInformationView);
                MembershipCreateResult membershipCreateResult = _memberShipProvider.CreateUser(userInformation, true);
                if (membershipCreateResult.CreateStatus == MembershipCreateStatus.Success)
                {
                    if (_memberShipProvider.ValidateUser(userInformationView.UserName, userInformationView.Password))
                    {
                        ViewBag.SuccessMessage = "Your account has been successfully created. Please login with your credentials";
                        return RedirectToAction("Login");
                    }
                    ViewBag.ErrorMessage =
                        "Login failed! Please make sure you are using the correct user name and password.";
                }
                else
                    ViewBag.ErrorMessage = GetErrorMessage(membershipCreateResult.CreateStatus);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = "An exception occurred. Please try again " + e.Message;
                throw;
            }

            ViewBag.PageTitle = "Register New User";
            return View(userInformationView);
        }


        [NonAction]
        private string GetErrorMessage(MembershipCreateStatus membershipCreateStatus)
        {
            switch (membershipCreateStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Username already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return @"A username for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return @"The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return @"The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return @"The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return @"The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return @"The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return @"The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return @"An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

            }
        }

        [HttpGet]
        public ViewResult ForgotPassword()
        {
            ViewBag.PageTitle = "Forgot Password";
            return View(new UserInformationView());
        }

        public ActionResult GetHintAnswer(string userName)
        {
            var membershipUserWrapper = _memberShipProvider.GetUser(userName, false);
            return Json(new{userName, membershipUserWrapper.SecretQuestion},JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ForgotPassword(string userName, string hintAnswer)
        {
            if (!string.IsNullOrEmpty(hintAnswer))
            {
                string resetPassword = _memberShipProvider.ResetPassword(userName, hintAnswer);
                if (_memberShipProvider.ValidateUser(userName, resetPassword))
                    return RedirectToAction("ChangePassword", new { resetPassword });
            }
            var membershipUserWrapper = _memberShipProvider.GetUser(userName, false);
            var userInformation = new UserInformationView();
            if (null != membershipUserWrapper)
            {
                userInformation.SecretQuestion = membershipUserWrapper.SecretQuestion;
                userInformation.UserName = userName;
            }
            else
            {
                ViewBag.ErrorMessage = "The user you have specified is invalid, please recheck your username and try again";
                userInformation.SecretQuestion = string.Empty;
            }
            ViewBag.PageTitle = "Forgot Password";
            return View(userInformation);
        }


        [HttpPost]
        public ActionResult ChangePassword(UserInformationView userInformation)
        {
            ViewBag.PageTitle = "Change Password";
            try
            {
                string userName = string.IsNullOrEmpty(userInformation.UserName)
                               ? HttpContext.User.Identity.Name
                               : userInformation.UserName;
                if (_memberShipProvider.ChangePassword(userName, userInformation.Password,
                                                       userInformation.ChangePassword))
                {
                    ViewBag.SuccessMessage = "Your password has been sucessfully changed. Please login with your new password";
                    return RedirectToAction("Login");
                }
                ViewBag.ErrorMessage = "Password could not be changed at this time. Please try again later";
            }
            catch (Exception exception)
            {
                ViewBag.ErrorMessage = "An error occurred trying to change your password. Please try again";
                throw;
            }

            return View(userInformation);
        }

        public ActionResult ChangePassword(string resetPassword)
        {
            ViewBag.PageTitle = "Change Password";
            var model = new UserInformationView();
            if (!string.IsNullOrEmpty(resetPassword))
            {
                ViewBag.InformationalMessage = "Your password has been reset to a temp. password. Please change it";
                model.Password = resetPassword;
            }
            return View(model);
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public ActionResult ManageUsers(string searchInput, string searchType, int pageNum)
        {

            var searchOptions = new List<SelectListItem>
                                      {
                                          new SelectListItem {Text = "User Name", Value = "UserName"},
                                          new SelectListItem {Text = "Email",Value = "Email"}
                                      };
            
            var manageUsersView = new ManageUsersView
                                      {
                                          SearchOptionList = new SelectList(searchOptions, "Value", "Text", searchType ?? "UserName"),
                                          SearchInput = searchInput ?? string.Empty,
                                          UsersOnline = _memberShipProvider.GetNumberOfUsersOnline(),
                                          RegisteredUsers = _memberShipProvider.GetAllUsers().Count()
                                      };

            int usersPerPage = DessertHouseConfigurationSection.Current.Users.PageSize;
            List<MembershipUserWrapper> userList;
            if (searchType == "Email") 
                userList = _memberShipProvider.GetUsersByEmail(searchInput).
                            Where(m=>m.Email.Equals(searchInput,StringComparison.OrdinalIgnoreCase)).ToList();  //had to filter out email again here as the Membership Provider has a bug in it
            else
            {
                var user = _memberShipProvider.GetUserByUserName(searchInput);
                userList = null == user ? new List<MembershipUserWrapper>() : new List<MembershipUserWrapper> { user };
            }
            if (!userList.Any())
                ViewBag.InformationalMessage = "No user found with the specified inputs";
            
            manageUsersView.UserList = new Pagination<MembershipUserWrapper>(userList.Skip((pageNum - 1) * usersPerPage).Take(usersPerPage), pageNum, usersPerPage, userList.Count);

            ViewBag.PageTitle = "Account Management";
            return View(manageUsersView);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ManageUsers(int pageNum)
        {
            var searchOptions = new List<SelectListItem>
                                      {
                                          new SelectListItem {Text = "User Name", Value = "UserName"},
                                          new SelectListItem {Text = "Email",Value = "Email"}
                                      };
            var currUserName = null != HttpContext ? HttpContext.User.Identity.Name : string.Empty;
            var allUsers = _memberShipProvider.GetAllUsers().Where(m=>!m.UserName.Equals(currUserName,StringComparison.OrdinalIgnoreCase));
            
            var userCount = allUsers.Count();
            var manageUsersView = new ManageUsersView
            {
                SearchOptionList = new SelectList(searchOptions, "Value", "Text", "UserName"),
                SearchInput = string.Empty,
                UsersOnline = _memberShipProvider.GetNumberOfUsersOnline(),
                RegisteredUsers = userCount
            };
            int usersPerPage = DessertHouseConfigurationSection.Current.Users.PageSize;
            var users = allUsers.Skip((pageNum-1)*usersPerPage).Take(usersPerPage).ToList();
            manageUsersView.UserList = new Pagination<MembershipUserWrapper>(users, pageNum, usersPerPage,userCount);
            ViewBag.PageTitle = "Account Management";
            
            return View(manageUsersView);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult DeleteUser(string userId)
        {
            bool deleted = _memberShipProvider.DeleteUser(userId);
            return Json(new { deleted, userId });

        }

        [Authorize(Roles = "Admin")]
        public ActionResult EditUser(string userid)
        {
            var user = _memberShipProvider.GetUser(userid, false);
            if (null == user)
                throw new Exception("Trying to edit non existent user " + userid);
            var editUserView = new EditUserView { Roles = _memberShipProvider.GetAllRoles(), MembershipUser = user };
            ViewBag.PageTitle = "Edit " + userid;
            return View(editUserView);
        }

        [Authorize(Roles = "Admin"), HttpPost]
        public ActionResult EditUser(string userId, bool approved)
        {
            string[] allRoles = _memberShipProvider.GetAllRoles();

            var reqKeys = Request.Form.AllKeys;
            reqKeys.Where(r => allRoles.Contains(r)).Each(r =>
            {
                var selRoles = Request.Form.GetValues(r);
                if (selRoles.Length == 2)
                {
                    if (!Roles.IsUserInRole(userId, r))
                        Roles.AddUserToRole(userId, r);
                }
                else
                {
                    if (Roles.IsUserInRole(userId, r)) Roles.RemoveUserFromRole(userId, r);
                }

            });


            MembershipUserWrapper membershipUser = _memberShipProvider.GetUser(userId, false);
            membershipUser.IsApproved = approved;
            _memberShipProvider.UpdateUser(membershipUser);

            ViewBag.SuccessMessage = "User Information has been updated";


            ViewBag.PageTitle = "Edit " + userId;
            return View(new EditUserView { Roles = allRoles, MembershipUser = membershipUser });

        }

        [Authorize(Roles = "Admin")]
        public ActionResult ManageRoles()
        {
            ViewBag.TotalRoles = _memberShipProvider.GetAllRoles().Count();
            ViewBag.PageTitle = "Role Management";
            return View();
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public ActionResult CreateRole(string newRole)
        {
            _memberShipProvider.CreateRole(newRole);
            return Json(new { roleCreated = true, roleName = newRole });
        }

        [Authorize(Roles = "Admin"), HttpPost]
        public ActionResult DeleteRole(string role)
        {
            var roleDeleted = _memberShipProvider.DeleteRole(role);
            return Json(new { roleDeleted, roleId = role,roleCount=_memberShipProvider.GetAllRoles().Count() });
        }

        public ActionResult Login(string returnUrl)
        {
            ViewBag.PageTitle = "Login";
            return !string.IsNullOrEmpty(returnUrl) ? View(new LoginViewModel {ReturnUrl = returnUrl}) : View();
        }


        [HttpPost]
        public ActionResult Login(LoginViewModel login)
        {
            string returnUrl = string.IsNullOrEmpty(login.ReturnUrl)? "/":login.ReturnUrl;
            if (returnUrl == "/" && Request.ApplicationPath.Contains("TheDessertHouse"))
                returnUrl = "/TheDessertHouse";

            if (_memberShipProvider.ValidateUser(login.UserName, login.Password))
            {
                FormsAuthentication.SetAuthCookie(login.UserName, login.Persistent);
                return Redirect(returnUrl);
            }
            ViewBag.ErrorMessage = "No records found with those credentials. Please try again";
            ViewBag.PageTitle = "Login";
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult CheckAuthentication()
        {
            bool isAuthenticated=false;
            if (User != null && User.Identity.IsAuthenticated)
                isAuthenticated = true;
            return Json(isAuthenticated,JsonRequestBehavior.AllowGet);
        }
    }
}
