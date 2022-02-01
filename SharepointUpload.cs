using System;
using File = System.IO.File;
using System.Security;
using System.IO;
using SP = Microsoft.SharePoint.Client;
using System.Collections;
using OfficeDevPnP.Core;
using System.Collections.Generic;

namespace SharepointUploadLibrary
{
    public class SharepointUpload
    {
        public void UploadFileToSharePoint(string SiteUrl, string DocLibrary, string ClientSubFolder, string FileName, string appId, string appSecret, Dictionary<string, object> metadataValues, byte[] FileContent)
        {
            try
            {
                #region Insert the data
                using (var CContext = new AuthenticationManager().GetAppOnlyAuthenticatedContext(SiteUrl, appId, appSecret))
                {
                    Microsoft.SharePoint.Client.Web web = CContext.Web;
                    SP.FileCreationInformation newFile = new SP.FileCreationInformation();
                    newFile.ContentStream = new MemoryStream(FileContent);
                    newFile.Url = Path.GetFileName(FileName);
                    SP.List DocumentLibrary = web.Lists.GetByTitle(DocLibrary);
                    SP.Folder Clientfolder = DocumentLibrary.RootFolder.Folders.Add(ClientSubFolder);
                    Clientfolder.Update();
                    SP.File uploadFile = Clientfolder.Files.Add(newFile);

                    foreach (var item in metadataValues)
                    {
                        uploadFile.ListItemAllFields[item.Key] = item.Value;
                    }
                    uploadFile.ListItemAllFields.Update();

                    CContext.Load(DocumentLibrary);
                    CContext.Load(uploadFile);
                    CContext.ExecuteQuery();
                }

                #endregion
            }
            catch (Exception exp)
            {
            }
            finally
            {

            }
        }
    }
}
