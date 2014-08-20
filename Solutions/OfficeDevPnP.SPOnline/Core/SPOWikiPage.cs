﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Publishing;

namespace OfficeDevPnP.SPOnline.Core
{
    public static class SPOWikiPage
    {
        public static void SetWikiPageContent(string pageUrl, string content, Web web, ClientContext clientContext)
        {
            File file = clientContext.Web.GetFileByServerRelativeUrl(pageUrl);

            clientContext.Load(file, f => f.ListItemAllFields);
            clientContext.ExecuteQuery();

            ListItem item = file.ListItemAllFields;

            item["WikiField"] = content;

            item.Update();

            clientContext.ExecuteQuery();
        }

        [Obsolete("Use GetWikiPageContents extension on Web in OfficeDevPnP.Core")]
        public static string GetWikiPageContent(string serverRelativePageUrl, Web web, ClientContext clientContext)
        {
            File file = web.GetFileByServerRelativeUrl(serverRelativePageUrl);

            web.Context.Load(file, f => f.ListItemAllFields);
            web.Context.ExecuteQuery();

            return file.ListItemAllFields["WikiField"] as string;
        }

        public static void AddWikiPage(string serverRelativePageUrl, Web web, ClientContext clientContext, string content = null)
        {
            string folderName = serverRelativePageUrl.Substring(0, serverRelativePageUrl.LastIndexOf("/"));
            Folder folder = web.GetFolderByServerRelativeUrl(folderName);
            File file = folder.Files.AddTemplateFile(serverRelativePageUrl, TemplateFileType.WikiPage);

            clientContext.ExecuteQuery();
            if (content != null)
            {
                SetWikiPageContent(serverRelativePageUrl, content, web, clientContext);
            }
        }

        public static void RemoveWikiPage(string serverRelativePageUrl, Web web, ClientContext clientContext)
        {
            serverRelativePageUrl = System.UrlUtility.Combine(web.Url,serverRelativePageUrl);

            File file = web.GetFileByServerRelativeUrl(serverRelativePageUrl);

            file.DeleteObject();

            clientContext.ExecuteQuery();
        }
    }
}
