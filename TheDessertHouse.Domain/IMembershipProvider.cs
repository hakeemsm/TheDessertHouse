using System;
using System.Collections.Generic;

namespace TheDessertHouse.Domain
{
    public interface IMembershipProvider
    {
        MembershipCreateResult CreateUser(UserInformation userInformation, bool isApproved);
        bool ValidateUser(string userName, string password);
        MembershipUserWrapper GetUser(string userName, bool userIsOnline);
        string ResetPassword(string userName, string answer);
        bool ChangePassword(string userName, string password, string changePassword);
        IEnumerable<MembershipUserWrapper> GetAllUsers();
        int GetNumberOfUsersOnline();
        MembershipUserWrapper GetUserByUserName(string username);
        IEnumerable<MembershipUserWrapper> GetUsersByEmail(string email);
        bool DeleteUser(string userId);
        string[] GetAllRoles();
        void UpdateUser(MembershipUserWrapper membershipUserWrapper);
        void CreateRole(string newRole);
        bool DeleteRole(string role);
    }
}