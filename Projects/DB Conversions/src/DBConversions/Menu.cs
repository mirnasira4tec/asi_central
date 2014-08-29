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
using ASICentralDBConversions;

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

        public Dictionary<int, int> nodeFolderIds = new Dictionary<int, int>()
            {
                {3339,372},//Promogram
                {7338,548},//Promogram-Canada
                {7339,368},//SuccessFul Promotions
                {7340,546},//Successful Promotions Canada
                {7341,374},//Wearables Style
                {7342,370},//Advantages hot Deals
                {7343,551},//Esp Websites
                {7344,552},//ESP Tips
                {7346,412},//Advantages Case studies
                {7345,542}//spot asi savings
            };
        //For Press Releases :: By Pavan on Aug 21st, 2014.

        public Dictionary<int, int> nodePressReleaseIds = new Dictionary<int, int>()
            {
                {3651,134}
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
           
            if (Service == null)
            {
                Service = getContentservice();
            }
            foreach (KeyValuePair<int, int> node in nodeFolderIds)
            {
                List<NewsLetter> Newsletters = new List<NewsLetter>();
                string sql = String.Format("SELECT  content_title,content_html,date_created from asicentral..[content] where folder_ID in ('{0}') and content_status = 'A' order by date_created desc ", node.Value);
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
            
            return (serviceContext.ContentService);
        }

        private string ParseContent(string rawcontent)
        {
            string newBannertable = string.Empty;
            string oldBannertable = string.Empty;
            string htmlcontent = rawcontent;
            bool isbannerremoved = false;
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

        //======================================
        //Press Releases by Pavan..



        private void btnPressReleases_Click(object sender, EventArgs e)
        {
            List<int> parentNodeID = new List<int>();

            if (Service == null)
            {
                Service = getContentservice();
            }
            foreach (KeyValuePair<int, int> node in nodePressReleaseIds)
            {
                List<PressReleases> lstPressRelease = new List<PressReleases>();

                // For checking first 20 records
                string sql = String.Format("SELECT c.content_id, c.content_title,c.image,c.content_teaser,c.content_html,c.date_created FROM asicentral..[content] c WHERE	c.content_status = 'A' AND c.folder_id = {0} ORDER BY date_created DESC", node.Value);
                lstPressRelease = RetrieveDatabaseValues(sql);
                CreateNodes(node, lstPressRelease);
            }
            lbl_confirmation.Text = "Nodes Creation Completed!!";
        }

        internal List<PressReleases> RetrieveDatabaseValues(string sql)
        {
            using (SqlConnection ektronConn = new SqlConnection(ConfigurationManager.ConnectionStrings["Ektron.DbConnection"].ToString()))
            {
                List<PressReleases> pressreleaseNodes = new List<PressReleases>();
                SqlDataReader reader = null;
                SqlCommand cmd = new SqlCommand(sql, ektronConn);
                ektronConn.Open();
                reader = cmd.ExecuteReader();

                while (reader != null && reader.Read())
                {
                    PressReleases pressreleaseObj = new PressReleases();
                    pressreleaseObj.Content = reader["content_html"].ToString();
                   // pressreleaseObj.ContentText = reader["content_text"].ToString();
                    pressreleaseObj.DatePublished = DateTime.Parse(reader["date_created"].ToString());
                    pressreleaseObj.ChildNodeName = reader["content_title"].ToString();
                    pressreleaseNodes.Add(pressreleaseObj);
                }
                return pressreleaseNodes;

            }
        }

        internal void CreateNodes(KeyValuePair<int, int> node, List<PressReleases> pressreleaseNodes)
        {

            string currentnodeYear = string.Empty;
            int parentContentID = -1;

            #region code to insert in to umbraco DB

            foreach (PressReleases pressreleaseObj in pressreleaseNodes)
            {
                string issuedYear = pressreleaseObj.DatePublished.ToString("Y").Replace(',', ' ');
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

                    if (parentContentID == -1)
                    {
                        yearContent = Service.CreateContent(issuedYear.ToString(), node.Key, FOLDER_DOC_TYPE);
                        try
                        {
                            yearContent.SetValue("issueDate", monthIssued.ToString("yyyy-MM-dd"));
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

                    if (childNode.Name == pressreleaseObj.ChildNodeName)
                    {
                        innerContentID = childNode.Id;
                        break;
                    }
                }
                if (innerContentID == -1)
                {
                    var volContent = Service.CreateContent(pressreleaseObj.ChildNodeName, parentContentID, VOLUME_DOC_TYPE);
                    
                    if (node.Value == 134)
                    {
                        pressreleaseObj.Content = ParseContent(pressreleaseObj.Content);
                    }
                    try
                    {
                        

                        string innerContent = pressreleaseObj.Content.Replace("<center>", "<p>");
                        innerContent = innerContent.Replace("</center>", "</p></header>");
                        string objContent = "<h1>PRESS RELEASES</h1> " + "<article><header><h1>" + pressreleaseObj.ChildNodeName.ToFirstUpper() + "</h1>" + innerContent.TrimStart() + "<article>";

                        volContent.SetValue("postedDate", pressreleaseObj.DatePublished.ToString("yyyy-MM-dd hh:mm:ss"));
                        volContent.SetValue("content",objContent);
                      //volContent.SetValue("contentText", pressreleaseObj.ContentText);
                        
                        Service.Save(volContent);
                    }
                    catch { }

                }

            }
            #endregion code to insert in to umbraco DB
        }

    }
}
