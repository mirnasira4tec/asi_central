using System;
using System.Data;
using System.Data.SqlServerCe;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using System.Configuration;
using Umbraco.Core;
using Umbraco.Web;
using Umbraco.Core.Services;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence;
using System.Collections;
using System.Collections.Generic;
using umbraco.NodeFactory;
using System.Text.RegularExpressions;

namespace DBConversions
{
    public partial class Menu : Form
    {

        public static readonly int NEWSLETTER_PARENT_NODE_ID = 3226;
        public static readonly string FOLDER_DOC_TYPE = "issue";
        public static readonly string VOLUME_DOC_TYPE = "NewsItem";
        public static readonly int TEMPLATE_ID=1116;
        public static readonly  Regex regex = new Regex(@"(?<info>.+)\s*(?<date>\d\d/\d+/\d+)");
        public IContentService service = null;
        public Dictionary<int, int> nodeFolderIds = new Dictionary<int, int>()
            {
                {3227,372},
                {3228,548},
                {3229,368},
                {3230,546},
                {3231,374},
                {3232,370},
                {3233,551},
                {3234,552}
            };
        public Menu()
        {
            InitializeComponent();
            lbl_confirmation.Text = "";
        }
        private void BtnConvert_Click(object sender, EventArgs e)
        {
            lbl_confirmation.Text = "Nodes creation started.......";
            
            List<int> parentNodeID = new List<int>();
           
            if (service == null)
            {
                service = getContentservice();
            }
            foreach (KeyValuePair<int, int> node in nodeFolderIds)
            {
                List<NewsLetter> Newsletters = new List<NewsLetter>();
                string sql = String.Format("SELECT  content_title,content_html,date_created from asicentral..[content] where folder_ID in ('{0}')  order by date_created desc ", node.Value);
               
                Newsletters = RetriveDatabaseValues(sql);
                CreateNodes(node, Newsletters);
            }
            lbl_confirmation.Text = "Nodes creation completed!";
        }


        internal void CreateNodes(KeyValuePair<int, int> node, List<NewsLetter> Newsletternodes)
        {
           
            string currentnodeYear = string.Empty;
            int parentContentID = -1;
             

            #region code to insert in to umbraco DB
            foreach (NewsLetter newsletterobj in Newsletternodes)
            {
                string Issuedyear = newsletterobj.date.ToString("Y").Replace(',', ' ');
                DateTime monthissued = DateTime.Parse(string.Concat("01 ", Issuedyear));
                
                IContent yearContent = null;
                

                if (currentnodeYear != Issuedyear)
                {
                    parentContentID = -1;
                    yearContent = service.CreateContent(Issuedyear.ToString(), node.Key, FOLDER_DOC_TYPE);
                    var rootnode = service.GetChildren(node.Key);
                 
                    
                        int parentitartionID = -1;
                        foreach (var childNode in rootnode)
                        {
                            parentitartionID = -1;
                            if (childNode.Name == yearContent.Name)
                            {
                                parentContentID = childNode.Id;
                                parentitartionID = childNode.Id;
                                break;
                            }


                        }
                        if (parentitartionID == -1)
                        {
                            try
                            {
                                yearContent.SetValue("issueDate", monthissued.ToShortDateString());
                                service.Save(yearContent);
                            }
                            catch { }
                            
                            parentContentID = yearContent.Id;
                        }
                    
                    currentnodeYear = Issuedyear;
                }
                var volumecontent = service.CreateContent(newsletterobj.ChildNodeName, parentContentID, VOLUME_DOC_TYPE);               
                var contentnode = service.GetChildren(parentContentID);
                int innercontentID = -1;

                
                    int childitrationId = -1;
                    foreach (var childNode in contentnode)
                    {
                        childitrationId = -1;
                        if (childNode.Name == volumecontent.Name)
                        {
                            innercontentID = childNode.Id;
                            childitrationId = childNode.Id;
                            break;
                        }
                    }
                    if (childitrationId == -1)
                    {
                        try
                        {
                            volumecontent.SetValue("postedDate", newsletterobj.date.ToShortDateString());
                            volumecontent.SetValue("content", newsletterobj.Content);
                            service.Save(volumecontent);
                        }
                        catch { }
                       
                    }

                
            }
            #endregion code to insert in to umbraco DB

        }

        internal List<NewsLetter> RetriveDatabaseValues(string sql)
        {
            using (SqlConnection ektronConn = new SqlConnection(ConfigurationManager.ConnectionStrings["Ektron.DbConnection"].ToString()))
            {
               
                SqlCommand cmd = new SqlCommand(sql, ektronConn);

                ektronConn.Open();
                SqlDataReader reader = null;
                reader = cmd.ExecuteReader();

                List<NewsLetter> Newsletternodes = new List<NewsLetter>();
                while (reader != null && reader.Read())
                {
                    NewsLetter newsletterobj = new NewsLetter();
                    newsletterobj.Content = reader["content_html"].ToString();
                    newsletterobj.date = DateTime.Parse(reader["date_created"].ToString());
                    newsletterobj.ChildNodeName = reader["content_title"].ToString();
                    Match match = regex.Match(newsletterobj.ChildNodeName);
                    if (match.Success)
                    {
                        newsletterobj.ChildNodeName = match.Groups["info"].Value;
                    }
                    newsletterobj.ChildNodeName = newsletterobj.ChildNodeName.ToLower().Replace("vol", "Vol ");
                    Newsletternodes.Add(newsletterobj);
                }
                return Newsletternodes;

            }
        }
       

        public IContentService getContentservice()
        {
            var application = new ConsoleApplicationBase();
            application.Start(application, new EventArgs());
            var context = Umbraco.Core.ApplicationContext.Current;
            //Write status for DatabaseContext
            var databaseContext = context.DatabaseContext;
            //Write status for Database object
            var database = databaseContext.Database;
            //Get the ServiceContext and the two services we are going to use
            var serviceContext = context.Services;
            var contentTypeService = serviceContext.ContentTypeService;
            
            return (serviceContext.ContentService);
        }
    }
}
