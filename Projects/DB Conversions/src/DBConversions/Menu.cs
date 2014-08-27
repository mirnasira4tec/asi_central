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
        public static readonly string FOLDER_DOC_TYPE = "Issue";
        public static readonly string VOLUME_DOC_TYPE = "NewsItem";
        public static readonly int TEMPLATE_ID=1116;
        public static readonly  Regex Regex = new Regex(@"(?<info>.+)\s*(?<date>\d\d/\d+/\d+)");
       
        
        public IContentService Service = null;
       // public Dictionary<int, string> Childnodesdictionary = new Dictionary<int, string>();
        public Dictionary<int, int> nodeFolderIds = new Dictionary<int, int>()
            {
                {3227,372},
                {3228,548},
                {3229,368},
                {3230,546},
                {3231,374},
                {3232,370},
                {3233,551},
                {3234,552},
                {3373,542},
                {3374,412}
            };
        //public Dictionary<int, int> nodeFolderIds = new Dictionary<int, int>()
        //    {
        //        {3339,372},
        //        {3340,548},
        //        {3341,368},
        //        {3342,546},
        //        {3343,374},
        //        {3344,370},
        //        {3345,551},
        //        {3346,552}
                
        //    };
        
        public Menu()
        {
            InitializeComponent();
            lbl_confirmation.Text = "";
        }
        private void BtnConvert_Click(object sender, EventArgs e)
        {
            lbl_confirmation.Text = "Nodes creation started.......";
            
            List<int> parentNodeID = new List<int>();
           
            if (Service == null)
            {
                Service = getContentservice();
            }
            foreach (KeyValuePair<int, int> node in nodeFolderIds)
            {
                List<NewsLetter> Newsletters = new List<NewsLetter>();
                string sql = String.Format("SELECT content_title,content_html,date_created from asicentral..[content] where folder_ID in ('{0}')  order by date_created desc ", node.Value);
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
                if (node.Value != 542)
                {
                    Match match = Regex.Match(newsletterobj.ChildNodeName);
                    if (match.Success)
                    {
                        newsletterobj.ChildNodeName = match.Groups["info"].Value;
                    }
                    if(node.Value==551 ||node.Value==552)
                    {
                        string[] name = newsletterobj.ChildNodeName.Replace(". ",".").Split(' ');
                        newsletterobj.ChildNodeName = name[0];

                    }
                    newsletterobj.ChildNodeName = newsletterobj.ChildNodeName.ToLower().Replace("vol", "Vol ");

                }
               
                    newsletterobj.ChildNodeName = newsletterobj.ChildNodeName.Replace(".", "").Replace(",", "").Replace("  ", " ");
               
                string issuedYear = newsletterobj.date.ToString("Y").Replace(',', ' ').Replace("  "," ");
                DateTime monthIssued = DateTime.Parse(string.Concat("01 ", issuedYear));

                IContent yearContent = null;


                if (currentnodeYear != issuedYear)
                {
                    parentContentID = -1;

                    IEnumerable<IContent> rootnodes = Service.GetChildren(node.Key);
                   
                    foreach (var childnode in rootnodes)
                    {

                        if (childnode.Name == issuedYear.ToString())
                        {
                            parentContentID = childnode.Id;
                            break;
                        }
                    }
                    
                    if ( parentContentID == -1)
                    {
                        yearContent = Service.CreateContent(issuedYear.ToString(), node.Key, FOLDER_DOC_TYPE);
                        try
                        {
                            yearContent.SetValue("issueDate", monthIssued.ToShortDateString());
                            Service.Save(yearContent);
                        }
                        catch { }

                        parentContentID = yearContent.Id;
                    }
                    currentnodeYear = issuedYear;
                }
                
                var contentnodes = Service.GetChildren(parentContentID);
                int innerContentID = -1;
                foreach (var childNode in contentnodes)
                {
                    
                    if (childNode.Name == newsletterobj.ChildNodeName)
                    {
                        innerContentID = childNode.Id;
                        break;
                    }
                }
                if (innerContentID == -1)
                {
                    var volumecontent = Service.CreateContent(newsletterobj.ChildNodeName, parentContentID, VOLUME_DOC_TYPE);
                    if (node.Value == 372)
                    {
                        newsletterobj.Content = ParseContent(newsletterobj.Content);
                    }
                    try
                    {
                        volumecontent.SetValue("postedDate", newsletterobj.date.ToString());
                        volumecontent.SetValue("content", newsletterobj.Content);
                        Service.Save(volumecontent);
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
                List<NewsLetter> newsletterNodes = new List<NewsLetter>();
                SqlDataReader reader = null;
                SqlCommand cmd = new SqlCommand(sql, ektronConn);
                ektronConn.Open();
                reader = cmd.ExecuteReader();
                
                while (reader != null && reader.Read())
                {
                    NewsLetter newsletterobj = new NewsLetter();
                    newsletterobj.Content = reader["content_html"].ToString();
                    newsletterobj.date = DateTime.Parse(reader["date_created"].ToString());
                    newsletterobj.ChildNodeName = reader["content_title"].ToString();
                    newsletterNodes.Add(newsletterobj);
                }
                return newsletterNodes;

            }
        }
       

        public IContentService getContentservice()
        {
            var application = new ConsoleApplicationBase();
            application.Start(application, new EventArgs());
            var context = Umbraco.Core.ApplicationContext.Current;
            var serviceContext = context.Services;
            //var contentTypeService = serviceContext.ContentTypeService;
            
            return (serviceContext.ContentService);
        }

        private string ParseContent(string rawcontent)
        {
            string newBannertable = string.Empty;
            string oldBannertable = string.Empty;
            string htmlcontent = rawcontent;
            bool isbannerremoved = false;

            //htmlcontent = htmlcontent.TrimStart();
            if (htmlcontent.TrimStart().Substring(0, 6) == "<table")
            {
                int trindex = rawcontent.IndexOf("<tr>");
                oldBannertable = rawcontent.Substring(trindex).TrimStart();
                oldBannertable = oldBannertable.Substring(0, oldBannertable.IndexOf("</tr>") + 5);

                rawcontent = rawcontent.Remove(trindex, oldBannertable.Length);
                isbannerremoved = true;
            }
            else
            {
                 int index = rawcontent.IndexOf("<table border=\"0\" width=\"100%\">");
                string endtableToRemove;
                if (index != -1)
                {

                    newBannertable = rawcontent.Substring(index);
                    if (newBannertable.Substring(0, 6).TrimStart() == "<table")
                    {

                        string tableToRemoveinit = newBannertable.Substring(0, newBannertable.IndexOf("</table>") + 8);

                        newBannertable = newBannertable.Remove(0, tableToRemoveinit.Length);
                        endtableToRemove = newBannertable.Substring(0, newBannertable.IndexOf("</table>") + 8);

                        rawcontent = rawcontent.Remove(index, (endtableToRemove.Length + tableToRemoveinit.Length));
                        isbannerremoved = true;
                    }
                }
               
            }
            if (!isbannerremoved)
            {
               int index = rawcontent.IndexOf("<td valign=\"top\">");
                string endtableToRemove;
                if (index != -1)
                {

                    newBannertable = rawcontent.Substring(index + 17).TrimStart();
                    if (newBannertable.Substring(0, 6).TrimStart() == "<table")
                    {

                        string tableToRemoveinit = newBannertable.Substring(0, newBannertable.IndexOf("</table>") + 8);

                        newBannertable = newBannertable.Remove(0, tableToRemoveinit.Length);
                        endtableToRemove = newBannertable.Substring(0, newBannertable.IndexOf("</table>") + 8);

                        rawcontent = rawcontent.Remove(index + 17, (endtableToRemove.Length + tableToRemoveinit.Length));
                        isbannerremoved = true;
                    }
                }
            }

            return rawcontent;
        }
    }
}
