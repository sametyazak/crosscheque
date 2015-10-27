using LinkedListLoop.src.server.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMatrix.WebData;
using Microsoft.Web.WebPages.OAuth;
using System.Web.Security;

namespace LinkedListLoop.src.server
{
    public class UserManager
    {
        public static void Login(LoginModel model)
        {
            if (WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                //HttpContext.Current.Response.Redirect(returnUrl, true);
                return;
            }

            throw new Exception("Yalış kullanıcı adı veya şifre yanlış!");
        }

        public static void LogOut()
        {
            WebSecurity.Logout();
        }

        public static void Register(RegisterModel model)
        {
            WebSecurity.CreateUserAndAccount(model.UserName, model.Password);

            AddUserRole(model.UserName, AccessLevel.Demo.ToString());
            if (WebSecurity.Login(model.UserName, model.Password))
            {
                return;
            }

            throw new Exception("Kullanıcı kayıt sırasında bir hata oluştu!");
        }

        public static void Manage(LocalPasswordModel model, string userName)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(userName));

            if (hasLocalAccount)
            {
                bool changePasswordSucceeded;
                try
                {
                    changePasswordSucceeded = WebSecurity.ChangePassword(userName, model.OldPassword, model.NewPassword);
                }
                catch (Exception ex)
                {
                    LogManager.InsertExceptionLog(ex);
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    //HttpContext.Current.Response.Redirect("./ManagePassword.aspx");
                }
                else
                {
                    throw new Exception("The current password is incorrect or the new password is invalid.");
                }
            }
            else
            {
                try
                {
                    WebSecurity.CreateAccount(userName, model.NewPassword);
                    //HttpContext.Current.Response.Redirect("./ManagePassword.aspx");
                }
                catch (Exception e)
                {
                    LogManager.InsertExceptionLog(e);
                }
            }

        }

        public static void CreateAccessRolesIfNeccessary()
        {
            var roles = (SimpleRoleProvider)Roles.Provider;
            var membership = (SimpleMembershipProvider)Membership.Provider;

            var values = Enum.GetValues(typeof(AccessLevel)).Cast<AccessLevel>();

            foreach (AccessLevel level in values)
            {
                if (!roles.RoleExists(level.ToString()))
                {
                    roles.CreateRole(level.ToString());
                }
            }

        }

        public static string[] GetAllRoles()
        {
            var roles = (SimpleRoleProvider)Roles.Provider;
            return roles.GetAllRoles();
        }

        public static List<string> GetAllUsers()
        {
            dynamic users;
            List<string> userList = new List<string>();

            using (var db = WebMatrix.Data.Database.Open("DefaultConnection"))
            {
                users = db.Query("SELECT * FROM UserProfile");

                foreach (var user in users)
                {
                    userList.Add(user.UserName);
                }
            }

            return userList;
        }

        public static string[] GetUserRoles(string userName)
        {
            var roles = (SimpleRoleProvider)Roles.Provider;
            return roles.GetRolesForUser(userName);
        }

        public static void AddUserRole(string userName, string roleName)
        {
            var roles = (SimpleRoleProvider)Roles.Provider;

            if (!roles.IsUserInRole(userName, roleName))
            {
                string[] users = new string[1];
                string[] roleNames = new string[1];

                users[0] = userName;
                roleNames[0] = roleName;

                roles.AddUsersToRoles(users, roleNames);
            }
        }

        public static void DeleteUserRole(string userName, string roleName)
        {
            var roles = (SimpleRoleProvider)Roles.Provider;

            if (roles.IsUserInRole(userName, roleName))
            {
                string[] users = new string[1];
                string[] roleNames = new string[1];

                users[0] = userName;
                roleNames[0] = roleName;

                roles.RemoveUsersFromRoles(users, roleNames);
            }
        }
    }
}