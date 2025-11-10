using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coffee.Models
{
    public interface IAuthService
    {
        bool Register(RegisterViewModel model, HttpPostedFileBase Avatar, out string errorMessage);
        User Login(string username, string password, out string errorMessage);
        void Logout();
    }
}