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
    public partial class ASIShowMenu : Form
    {
        public IContentService Service = null;

        //For ASIShow Education-Content :: By Pavan on Nov 12th, 2014.

        public ASIShowMenu()
        {
            InitializeComponent();
            lblConfirmation.Text = "";
        }
        public IContentService getContentservice()
        {
            var application = new ConsoleApplicationBase();
            application.Start(application, new EventArgs());
            var context = Umbraco.Core.ApplicationContext.Current;
            var serviceContext = context.Services;

            return (serviceContext.ContentService);
        }

        private void btnAddContent_Click(object sender, EventArgs e)
        {
            try
            {
                lblConfirmation.Text = "";
                if (!string.IsNullOrEmpty(txtNodeId.Text))
                {
                    int _nodeId = int.Parse(txtNodeId.Text);
                    if (Service == null)
                    {
                        Service = getContentservice();
                    }
                    var _contentNode = Service.GetById(_nodeId);

                    _contentNode.SetValue("content", rtbContent.Text);
                    Service.SaveAndPublish(_contentNode);

                    lblConfirmation.Text = "Content Added";
                    rtbContent.Text = "";
                    txtNodeId.Text = "";
                }

            }
            catch (Exception ex)
            {
                lblConfirmation.Text = ex.Message;
            }
        }

    }
}
