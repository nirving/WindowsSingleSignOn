using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.DirectoryServices.ActiveDirectory;
using System.Web.Security;
using System.DirectoryServices;

namespace FormsAuthAd
{
    public partial class Logon : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public List<ADDomain> GetDomains()
        {
            return GetNetBIOSDomains();
        }

        private List<ADDomain> GetNetBIOSDomains()
        {
            List<ADDomain> ret = new List<ADDomain>();
            DirectoryEntry RootDSE = new DirectoryEntry("LDAP://rootDSE");

            // Retrieve the Configuration Naming Context from RootDSE
            string configNC = RootDSE.Properties["configurationNamingContext"].Value.ToString();

            // Connect to the Configuration Naming Context
            DirectoryEntry configSearchRoot = new DirectoryEntry("LDAP://" + configNC);

            // Search for all partitions where the NetBIOSName is set.
            DirectorySearcher configSearch = new DirectorySearcher(configSearchRoot);
            configSearch.Filter = ("(NETBIOSName=*)");

            // Configure search to return dnsroot and ncname attributes
            configSearch.PropertiesToLoad.Add("dnsroot");
            configSearch.PropertiesToLoad.Add("NETBIOSName");
            SearchResultCollection forestPartitionList = configSearch.FindAll();

            // Loop through each returned domain in the result collection
            foreach (SearchResult domainPartition in forestPartitionList)
            {
                ADDomain ad = new ADDomain();
                ad.Name = domainPartition.Properties["NETBIOSName"][0].ToString();
                ad.Path = domainPartition.Properties["NETBIOSName"][0].ToString();
                ret.Add(ad);
            }
            return ret;
        }

        
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string adPath = txtDomain.Text;

            LdapAuthentication adAuth = new LdapAuthentication(adPath);
            try
            {
                if (true == adAuth.IsAuthenticated(txtDomain.Text, txtUsername.Text, txtPassword.Text))
                {
                    string groups = adAuth.GetGroups();


                    //Create the ticket, and add the groups.
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, txtDomain.Text+@"\"+txtUsername.Text, DateTime.Now, DateTime.Now.AddMinutes(60), false, groups);

                    //Encrypt the ticket.
                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                    //Create a cookie, and then add the encrypted ticket to the cookie as data.
                    HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

                    //Add the cookie to the outgoing cookies collection.
                    Response.Cookies.Add(authCookie);

                    //You can redirect now.
                    Response.Redirect(FormsAuthentication.GetRedirectUrl(txtUsername.Text, false));
                }
                else
                {
                    errorLabel.Text = "Authentication did not succeed. Check user name and password.";
                }
            }
            catch (Exception ex)
            {
                errorLabel.Text = "Error authenticating. " + ex.Message;
            }
        }

    }
}