using System.Collections.Generic;
using System.Linq;
using System.Web.Security;

namespace TheDessertHouse.Domain
{
    /// <summary>
    /// This class wraps the ASP.NET Membership Provider so that the Controllers and Unit tests don't have direct dependency on it
    /// All the method names reflect the method names in the class that it wraps
    /// </summary>
    public class MembershipProviderWrapper : IMembershipProvider
    {
        public MembershipCreateResult CreateUser(UserInformation userInformation, bool isApproved)
        {
            MembershipCreateStatus membershipStatus;
            var membershipUser = Membership.CreateUser(userInformation.UserName, userInformation.Password, userInformation.Email,
                                                                  userInformation.SecretQuestion, userInformation.HintAnswer, isApproved, out membershipStatus);
            return new MembershipCreateResult { MembershipUserObject = membershipUser, CreateStatus = membershipStatus };
        }

        public bool ValidateUser(string userName, string password)
        {
            return Membership.ValidateUser(userName, password);
        }

        public MembershipUserWrapper GetUser(string userName, bool userIsOnline)
        {
            var membershipUser = Membership.Provider.GetUser(userName, userIsOnline);
            if (membershipUser != null)
                return membershipUser.ToMembershipWrapper();
            return null;
        }

        public string ResetPassword(string userName, string answer)
        {
            return Membership.Provider.ResetPassword(userName, answer);
        }

        public bool ChangePassword(string userName, string password, string changePassword)
        {
            return Membership.Provider.ChangePassword(userName, password, changePassword);
        }

        public IEnumerable<MembershipUserWrapper> GetAllUsers()
        {
            return (from MembershipUser user in Membership.GetAllUsers() select user.ToMembershipWrapper()).ToList();
        }

        public int GetNumberOfUsersOnline()
        {
            return Membership.GetNumberOfUsersOnline();
        }

        public MembershipUserWrapper GetUserByUserName(string username)
        {
            var membershipUser = Membership.GetUser(username);
            if (null == membershipUser)
                return null;
            return membershipUser.ToMembershipWrapper();
        }

        public IEnumerable<MembershipUserWrapper> GetUsersByEmail(string email)
        {
            var users = Membership.FindUsersByEmail(email); //this doesnt work as advertised, returns users even for unmatching emails!
            
            return (from MembershipUser user in users select Membership.GetUser(user.UserName).ToMembershipWrapper()).ToList();
        }

        public bool DeleteUser(string userId)
        {
            return Membership.DeleteUser(userId);

        }

        public string[] GetAllRoles()
        {
            return Roles.GetAllRoles();
        }

        public void UpdateUser(MembershipUserWrapper membershipUserWrapper)
        {
            var membershipUser = Membership.GetUser(membershipUserWrapper.UserName);
            if (null!=membershipUser)
            {
                membershipUser.IsApproved = membershipUser.IsApproved;
                Membership.UpdateUser(membershipUser);
            }
        }

        public void CreateRole(string newRole)
        {
            Roles.CreateRole(newRole);

        }

        public bool DeleteRole(string role)
        {
            return Roles.DeleteRole(role);
        }
    }

    public static class MembershipExtension
    {
        public static MembershipUserWrapper ToMembershipWrapper(this MembershipUser membershipUser)
        {
            return new MembershipUserWrapper
            {
                UserName = membershipUser.UserName,
                Email = membershipUser.Email,
                IsApproved = membershipUser.IsApproved,
                LastActivityDate = membershipUser.LastActivityDate,
                CreationDate = membershipUser.CreationDate,
                LastLoginDate = membershipUser.LastLoginDate,
                IsOnline = membershipUser.IsOnline,
                IsLockedOut = membershipUser.IsLockedOut,
                SecretQuestion = membershipUser.PasswordQuestion
                
            };

        }
    }
}