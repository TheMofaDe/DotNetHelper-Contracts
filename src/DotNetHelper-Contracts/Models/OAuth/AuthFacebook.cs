using System;
using System.Reflection;

namespace DotNetHelper_Contracts.Models.OAuth
{
	public class AuthFacebook
    {
	 
	    public string Type { get; set; } = "facebook";
		// OAuth for Facebook login
		//public override string ClientId { get { return ""; } } 
		public string ClientId { get; set; } 
		public string ClientSecret { get; set; } 
		// The scopes for the particular API accessed, delimited by "+" symbols
		public string Scope { get; set; } = "email+public_profile";
		public string AuthorizeUrl { get; set; }   = "https://m.facebook.com/dialog/oauth";
		public string AccessTokenUrl { get; set; }  = "https://m.facebook.com/dialog/oauth/token";
		public string UserInfoUrl { get; set; }   = "https://graph.facebook.com/v2.8/me";
		public string RedirectUrl { get; set; } = "https://www.facebook.com/connect/login_success.html";
		// Unique name for the device keystore where the credentials are stored
		public string KeystoreService => $"{GetApplicationName()}_FacebookOAuth";





        /// <summary>
        /// Gets the name of the application.
        /// </summary>
        /// <returns>System.String.</returns>
        private static string GetApplicationName()
        {
            try
            {
                var assdata = Assembly.GetEntryAssembly();
                return assdata?.FullName.Split(',')[0];
                // ALTERNATIVES 
                //  System.IO.Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                //  typeof(Application).Namespace.Split('.')[0]; (".");
            }
            catch (Exception)
            {
                // ignored
            }
            return "";
        }
    }
	
}
