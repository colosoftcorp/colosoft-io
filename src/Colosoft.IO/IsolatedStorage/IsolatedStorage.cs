using System;

namespace Colosoft.IO.IsolatedStorage
{
    public static class IsolatedStorage
    {
        public static string ApplicationName { get; set; } = "Colosoft";

        public static string AuthenticationServiceName { get; set; }

        public static string AuthenticationContextDirectory
        {
            get
            {
                return System.IO.Path.Combine(
                    SystemDirectory,
                    "AuthData",
                    string.IsNullOrEmpty(AuthenticationServiceName) ? "Default" : AuthenticationServiceName);
            }
        }

        public static string SystemDirectory
        {
            get
            {
                return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ApplicationName);
            }
        }

        public static string CommonSystemDirectory
        {
            get
            {
                return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), ApplicationName);
            }
        }

        public static string UserProfileDirectory
        {
            get
            {
                return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ApplicationName);
            }
        }
    }
}
