using System;
using System.Reflection;

namespace DotNetHelper_Contracts.Models.OAuth
{
    public class AuthGoogle
    {

   

		    public  string Type { get; set; } 
			// OAuth for Google login
			public string ClientId { get; set; }  // { get { return ""; } }
	    	public  string ClientSecret { get; set; } //{ get { return ""; } }
		    // These values do not need changing
		    public string[] Scopes { get; set; } = {"https://www.googleapis.com/auth/userinfo.email", "https://www.googleapis.com/auth/userinfo.profile"};
		    public  string Scope { get; set; } = "https://www.googleapis.com/auth/userinfo.email+" +  "https://www.googleapis.com/auth/userinfo.profile";
		    public  string AuthorizeUrl { get; set; } = "https://accounts.google.com/o/oauth2/auth";
		    public  string AccessTokenUrl { get; set; } = "https://accounts.google.com/o/oauth2/token";
		    public  string UserInfoUrl { get; set; } = "https://www.googleapis.com/oauth2/v2/userinfo"; 
		    // Unique name for the device keystore where the credentials are stored
		    public  string KeystoreService { get; set; } =  $"{GetApplicationName()}_GoogleOAuth";//{ get { return App.AppName + "_GoogleOAuth"; } }


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
