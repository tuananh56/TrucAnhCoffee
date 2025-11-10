using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Coffee.Models
{
    public class AuthServiceDecorator : IAuthService
    {
        private readonly IAuthService _authService;

        public AuthServiceDecorator(IAuthService authService)
        {
            _authService = authService;
        }

        public bool Register(RegisterViewModel model, HttpPostedFileBase Avatar, out string errorMessage)
        {
            bool result = _authService.Register(model, Avatar, out errorMessage);
            if (result)
            {
                LogAction($"Người dùng '{model.Username}' đã đăng ký.");
            }
            return result;
        }

        public User Login(string username, string password, out string errorMessage)
        {
            User user = _authService.Login(username, password, out errorMessage);
            if (user != null)
            {
                LogAction($"Người dùng '{username}' đã đăng nhập.");
            }
            return user;
        }

        public void Logout()
        {   
            _authService.Logout();
            LogAction("Người dùng đã đăng xuất.");
        }

        private void LogAction(string message)
        {
            string logFile = HttpContext.Current.Server.MapPath("~/App_Data/AuthLog.txt");
            File.AppendAllText(logFile, $"{DateTime.Now}: {message}\n");
        }
    }
}