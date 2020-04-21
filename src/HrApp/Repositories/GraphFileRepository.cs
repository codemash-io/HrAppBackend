using Microsoft.Graph;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HrApp
{
    public class GraphFileRepository : IGraphFileRepository
    {
        private readonly GraphRepository graphRepository = new GraphRepository();

        public async Task<Drive> GetDrive(string type, string id = null)
        {
            string graphUrl;
            type = type.ToLower();
            if (type == GraphResourceTypes.me.ToString())
                graphUrl = graphRepository.BaseGraphUrl + '/' + type + "/drive";
            else if (type == GraphResourceTypes.drives.ToString() && !string.IsNullOrEmpty(id))
                graphUrl = graphRepository.BaseGraphUrl + '/' + type + '/' + id;
            else
                if (!string.IsNullOrEmpty(id))
                graphUrl = graphRepository.BaseGraphUrl + '/' + type + '/' + id +
                    "/drive";
            else
                throw new BusinessException("Resource id not defined");

            var resultString = await graphRepository.Get(graphUrl);
            var drive = JsonConvert.DeserializeObject<Drive>(resultString);
            return drive;
        }

        public async Task<List<Drive>> ListAllDrives(string type, string id, string expand = null,
            string select = null, string skipToken = null, string top = null, string orderBy = null)
        {          
            type = type.ToLower();
            var list = new List<string>
            {
                expand != null ? "$expand=" + expand : null,
                select != null ? "$select=" + select : null,
                skipToken != null ? "$skipToken=" + skipToken : null,
                top != null ? "$top=" + top : null,
                orderBy != null ? "$orderBy=" + orderBy : null
            };
            //removes all nulls from a list
            list.RemoveAll(x => x == null);
            //merged list content
            string concat = "";
            if (list.Count != 0)
                concat = "?" + String.Join("&", list.ToArray());

            string graphUrl;

            if (type == GraphResourceTypes.me.ToString())
                graphUrl = graphRepository.BaseGraphUrl + '/' + type + "/drives" + concat;
            else if (type == GraphResourceTypes.drives.ToString())
                throw new BusinessException("This endpoint is not available for drives resource!");
            else
                graphUrl = graphRepository.BaseGraphUrl + '/' + type + '/' + id + "/drives" + concat;


            var resultString = await graphRepository.Get(graphUrl);
            var resultJson = JObject.Parse(resultString);
            var driveDetails = resultJson["value"].ToString();
            var drive = JsonConvert.DeserializeObject<List<Drive>>(driveDetails);
            return drive;
        }

        public async Task<DriveItem> GetSpecialFolder(string userId, string folderName)
        {        
            if (string.IsNullOrEmpty(folderName))
                throw new BusinessException("FolderName cannot be empty");
            

            string graphUrl = graphRepository.BaseGraphUrl + "/users/" + userId +
                    "/drive/special/" + folderName;

            var resultString = await graphRepository.Get(graphUrl);
            var drive = JsonConvert.DeserializeObject<DriveItem>(resultString);
            return drive;
        }

        public async Task<List<DriveItem>> SharedWithMe(string userId, string driveId = null)
        {
            string graphUrl;

            if (string.IsNullOrEmpty(driveId))
                graphUrl = graphRepository.BaseGraphUrl + "/users/" + userId +
                    "/drive/sharedWithMe";
            else
                graphUrl = graphRepository.BaseGraphUrl + "/users/" + userId +
                    "/drive/" + driveId + "/sharedWithMe";

            var resultString = await graphRepository.Get(graphUrl);
            var resultJson = JObject.Parse(resultString);
            var itemsDetails = resultJson["value"].ToString();
            var sharedItems = JsonConvert.DeserializeObject<List<DriveItem>>(itemsDetails);
            return sharedItems;
        }

        public async Task<List<DriveItem>> ListChildren(string type, string id,
            string itemId = null, string path = null, string expand = null, string select = null, 
            string skipToken = null, string top = null, string orderBy = null)
        {
            if (!string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(itemId))
                throw new BusinessException("Only one or non of the following fileds (itemId, path) can be filed!");
            type = type.ToLower();
            var list = new List<string>
            {
                expand != null ? "$expand=" + expand : null,
                select != null ? "$select=" + select : null,
                skipToken != null ? "$skipToken=" + skipToken : null,
                top != null ? "$top=" + top : null,
                orderBy != null ? "$orderBy=" + orderBy : null
            };
            //removes all nulls from a list
            list.RemoveAll(x => x == null);
            //merged list content
            string concat = "";
            if (list.Count != 0)
                concat = "?" + String.Join("&", list.ToArray());

            string graphUrl;

            if (type == GraphResourceTypes.drives.ToString())
            {
                //default drive root children
                if (string.IsNullOrEmpty(path) && string.IsNullOrEmpty(itemId))
                    graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id +
                        "/root/children" + concat;

                //selected drive path children
                else if (!string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(id))
                    graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id +
                        "/root:/" + path + ":/children" + concat;

                //selected drive selected item children
                else
                    graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id +
                            "/items/" + itemId + "/children" + concat;
            }
            else if (type == GraphResourceTypes.me.ToString())
            {
                //default drive root children
                if (string.IsNullOrEmpty(path) && string.IsNullOrEmpty(itemId))
                    graphUrl = graphRepository.BaseGraphUrl + "/" + type +
                        "/drive/root/children" + concat;

                //selected drive path children
                else if (!string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(id))
                    graphUrl = graphRepository.BaseGraphUrl + "/" + type +
                        "/drive/root:/" + path + ":/children" + concat;

                //selected drive selected item children
                else
                    graphUrl = graphRepository.BaseGraphUrl + "/" + type +
                            "/drive/items/" + itemId + "/children" + concat;
            }
            else
            {
                //default drive root children
                if (string.IsNullOrEmpty(path) && string.IsNullOrEmpty(itemId))
                    graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id +
                        "/drive/root/children" + concat;

                //selected drive path children
                else if (!string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(id))
                    graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id +
                        "/drive/root:/" + path + ":/children" + concat;

                //selected drive selected item children
                else
                    graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id +
                        "/drive/items/" + itemId + "/children" + concat;

            }
            var resultString = await graphRepository.Get(graphUrl);
            var resultJson = JObject.Parse(resultString);
            var driveDetails = resultJson["value"].ToString();
            var children = JsonConvert.DeserializeObject<List<DriveItem>>(driveDetails);
            return children;
        }

        public async Task<DriveItem> GetItem(string type, string id, string itemId = null, 
            string path = null, string expand = null, string select = null)
        {
            if (!string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(itemId))
                throw new BusinessException("Only one or non of the following fileds (itemId, path) can be filed!");
            type = type.ToLower();
            var list = new List<string>
            {
                expand != null ? "$expand=" + expand : null,
                select != null ? "$select=" + select : null
            };
            //removes all nulls from a list
            list.RemoveAll(x => x == null);
            //merged list content
            string concat = "";
            if (list.Count != 0)
                concat = "?" + String.Join("&", list.ToArray());

            string graphUrl;

            if (type == GraphResourceTypes.drives.ToString())
            {
                //default drive root children
                if (string.IsNullOrEmpty(path) && string.IsNullOrEmpty(itemId))
                    graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id +
                        "/root" + concat;

                //selected drive path children
                else if (!string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(id))
                    graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id +
                        "/root:/" + path + concat;

                //selected drive selected item children
                else
                    graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id +
                            "/items/" + itemId + concat;
            }
            else if (type == GraphResourceTypes.me.ToString())
            {
                //default drive root children
                if (string.IsNullOrEmpty(path) && string.IsNullOrEmpty(itemId))
                    graphUrl = graphRepository.BaseGraphUrl + "/" + type +
                        "/drive/root" + concat;

                //selected drive path children
                else if (!string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(id))
                    graphUrl = graphRepository.BaseGraphUrl + "/" + type +
                        "/drive/root:/" + path + concat;

                //selected drive selected item children
                else
                    graphUrl = graphRepository.BaseGraphUrl + "/" + type +
                            "/drive/items/" + itemId + concat;
            }
            else
            {
                // drive root item
                if (string.IsNullOrEmpty(path) && string.IsNullOrEmpty(itemId))
                    graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id +
                        "/drive/root" + concat;

                // drive path item
                else if (!string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(id))
                    graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id +
                        "/drive/root:/" + path + concat;

                // drive selected item
                else
                    graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id +
                        "/drive/items/" + itemId + concat;

            }

            var resultString = await graphRepository.Get(graphUrl);
            var driveItem = JsonConvert.DeserializeObject<DriveItem>(resultString);
            return driveItem;
        }

        public async Task<List<ThumbnailSet>> GetThumbnails(string type, string id, string itemId,
            string select = null)
        {
            string concat;
            if (select != null)
                concat = "$select=" + select;
            else
                concat = "";
            type = type.ToLower();
            string graphUrl;

            if (type == GraphResourceTypes.drives.ToString())
                graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id + "/items/" +
                    itemId + "/thumbnails?" + concat;

            else if (type == GraphResourceTypes.me.ToString())
                graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/drive/items/" +
                    itemId + "/thumbnails?" + concat;
            else
                graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id +
                    "/drive/items/" + itemId + "/thumbnails?" + concat;
           
            var resultString = await graphRepository.Get(graphUrl);
            var resultJson = JObject.Parse(resultString);
            var driveDetails = resultJson["value"].ToString();
            var thumbnails = JsonConvert.DeserializeObject<List<ThumbnailSet>>(driveDetails);
            return thumbnails;
        }
        public async Task<ThumbnailSet> GetSingleThumbnail(string type, string id, string itemId,
            string thumId, string size = null)
        {
            if (size == null)
                size = "";
            string graphUrl;
            type = type.ToLower();
            if (type == GraphResourceTypes.drives.ToString())
                graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id + "/items/" +
                    itemId + "/thumbnails/" + thumId + "/" + size;

            else if (type == GraphResourceTypes.me.ToString())
                graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/drive/items/" +
                    itemId + "/thumbnails/" + thumId + "/" + size;
            else
                graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id +
                    "/drive/items/" + itemId + "/thumbnails/" + thumId + "/" + size;

            var resultString = await graphRepository.Get(graphUrl);
            var resultJson = JObject.Parse(resultString);
            var driveDetails = resultJson["value"].ToString();
            var thumbnails = JsonConvert.DeserializeObject<ThumbnailSet>(driveDetails);
            return thumbnails;
        }
        public async Task<DriveItem> CreateFolder(string type, string id, string itemId, DriveItem item)
        {
            string graphUrl;
            type = type.ToLower();
            if (type == GraphResourceTypes.drives.ToString())
                graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id + "/items/" +
                    itemId + "/children";

            else if (type == GraphResourceTypes.me.ToString())
                graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/drive/items/" +
                    itemId + "/children";
            else
                graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id +
                    "/drive/items/" + itemId + "/children";

            var resultString = await graphRepository.Post(graphUrl, item);
            var newFile = JsonConvert.DeserializeObject<DriveItem>(resultString);
            return newFile;
        }
        public async Task<DriveItem> UpdateItem(string type, string id, string itemId, string name)
        {
            string graphUrl;
            type = type.ToLower();
            if (type == GraphResourceTypes.drives.ToString())
                graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id + "/items/" +
                    itemId;

            else if (type == GraphResourceTypes.me.ToString())
                graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/drive/items/" +
                    itemId;
            else
                graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id +
                    "/drive/items/" + itemId;

            var resultString = await graphRepository.Patch(graphUrl, new { name });
            var newFile = JsonConvert.DeserializeObject<DriveItem>(resultString);
            return newFile;
        }
        public async Task<bool> DeleteItem(string type, string id, string itemId)
        {
            string graphUrl;
            type = type.ToLower();
            if (type == GraphResourceTypes.drives.ToString())
                graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id + "/items/" +
                    itemId;

            else if (type == GraphResourceTypes.me.ToString())
                graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/drive/items/" +
                    itemId;
            else
                graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id +
                    "/drive/items/" + itemId;

            var resultString = await graphRepository.Delete(graphUrl);
            return resultString;
        }
        public async Task<DriveItem> MoveItem(string type, string id, string itemId, string parentFolderId)
        {
            string graphUrl;
            type = type.ToLower();
            var body = new DriveItem { ParentReference = new ItemReference { Id = parentFolderId } };

            if (type == GraphResourceTypes.drives.ToString())
                graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id + "/items/" +
                    itemId;

            else if (type == GraphResourceTypes.me.ToString())
                graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/drive/items/" +
                    itemId;
            else
                graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id +
                    "/drive/items/" + itemId;

            var resultString = await graphRepository.Patch(graphUrl, body);
            var driveItem = JsonConvert.DeserializeObject<DriveItem>(resultString);
            return driveItem;
        }

        public async Task<DriveItem> CopyItem(string type, string id, string itemId, DriveItem item)
        {
            string graphUrl;
            type = type.ToLower();
            if (item.ParentReference == null)
                throw new BusinessException("Preference item cannot be null");

            if (type == GraphResourceTypes.drives.ToString())
                graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id + "/items/" +
                    itemId;

            else if (type == GraphResourceTypes.me.ToString())
                graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/drive/items/" +
                    itemId;
            else
                graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id +
                    "/drive/items/" + itemId;

            var resultString = await graphRepository.Patch(graphUrl, item);
            var driveItem = JsonConvert.DeserializeObject<DriveItem>(resultString);
            return driveItem;
        }
        public async Task<byte[]> DownloadFile(string type, string id, string itemId, string format = null)
        {
            string graphUrl;
            type = type.ToLower();
            var formatTypes = "csv, doc, docx, odp, ods, odt, pot, potm, potx, pps, ppsx, " +
                "ppsxm, ppt, pptm, pptx, rtf, xls, xlsx";
            if (format != null)
                format = "format=" + format;
            else if (!formatTypes.Contains(format))
                throw new BusinessException("Provided format is not valid");

           
            if (type == GraphResourceTypes.drives.ToString())
                graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id + "/items/" +
                    itemId + "/content?"+ format;

            else if (type == GraphResourceTypes.me.ToString())
                graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/drive/items/" +
                    itemId + "/content?" + format;
            else
                graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id +
                    "/drive/items/" + itemId + "/content?" + format;

            var resultString = await graphRepository.Get(graphUrl);
            var content = Encoding.ASCII.GetBytes(resultString);
            return content;
        }
        public async Task<List<DriveItem>> TrackChanges(string type, string id, string select = null, 
            string expand = null, string top = null, string token = null)
        {
            string graphUrl;
            type = type.ToLower();
            var list = new List<string>
            {
                expand != null ? "$expand=" + expand : null,
                select != null ? "$select=" + select : null,
                top != null ? "$top=" + top : null,
            };
            //removes all nulls from a list
            list.RemoveAll(x => x == null);
            //merged list content
            string concat = "";
            if (list.Count != 0)
                concat = String.Join("&", list.ToArray());


            if (type == GraphResourceTypes.drives.ToString())
                if(string.IsNullOrEmpty(token))
                    graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id + "/root/delta?" + concat;
                else
                    graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id + 
                        "/root/delta?(token='" + token + "')&" + concat;

            else if (type == GraphResourceTypes.me.ToString())
                if (string.IsNullOrEmpty(token))
                    graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/drive/root/delta?" + concat;
                else
                    graphUrl = graphRepository.BaseGraphUrl + "/" + type +
                        "/drive/root/delta?(token='" + token + "')&" + concat;
            else
                if (string.IsNullOrEmpty(token))
                    graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id +
                        "/drive/root/delta?" + concat;
                else
                    graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id +
                        "/drive/root/delta?(token='" + token + "')&" + concat;

            var resultString = await graphRepository.Get(graphUrl);
            var resultJson = JObject.Parse(resultString);
            var driveDetails = resultJson["value"].ToString();
            var items = JsonConvert.DeserializeObject<List<DriveItem>>(driveDetails);
            return items;
        }

        public async Task<DriveItem> UploadNew(string type, string id, string path, byte[] file)
        {
            string graphUrl;
            type = type.ToLower();

            if (type == GraphResourceTypes.drives.ToString())
                graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id +
                        "/items/root:/" + path + ":/content";
            else if (type == GraphResourceTypes.me.ToString())
                graphUrl = graphRepository.BaseGraphUrl + "/" + type +
                        "/drive/items/root:/" + path + ":/content";
            else        
                graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id +
                    "/drive/items/root:/" + path + ":/content";


            var token = Environment.GetEnvironmentVariable("token");
            if (string.IsNullOrEmpty(token))
                token = await graphRepository.GetAccessToken();

            var byteArrayContent = new ByteArrayContent(file.ToArray());

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.PutAsync(graphUrl, byteArrayContent);
                if (!response.IsSuccessStatusCode)
                    throw new BusinessException("Something went wrong. Please check your input data and try again.");

                var resultString = await response.Content.ReadAsStringAsync();
                var item = JsonConvert.DeserializeObject<DriveItem>(resultString);
                return item;
            }

        }
        public async Task<DriveItem> UploadReplace(string type, string id, string itemId, byte[] file)
        {
            string graphUrl;
            type = type.ToLower();

            if (type == GraphResourceTypes.drives.ToString())
                graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id +
                        "/items/" + itemId + "/content";
            else if (type == GraphResourceTypes.me.ToString())
                graphUrl = graphRepository.BaseGraphUrl + "/" + type +
                        "/drive/items/" + itemId + "/content";
            else
                graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id +
                    "/drive/items/" + itemId + "/content";


            var token = Environment.GetEnvironmentVariable("token");
            if (string.IsNullOrEmpty(token))
                token = await graphRepository.GetAccessToken();

            var byteArrayContent = new ByteArrayContent(file.ToArray());

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.PutAsync(graphUrl, byteArrayContent);
                if (!response.IsSuccessStatusCode)
                    throw new BusinessException("Something went wrong. Please check your input data and try again.");

                var resultString = await response.Content.ReadAsStringAsync();
                var item = JsonConvert.DeserializeObject<DriveItem>(resultString);
                return item;
            }

        }

        public async Task<List<DriveItemVersion>> ListVersions(string type, string id, string itemId)
        {
            string graphUrl;
            type = type.ToLower();

            if (type == GraphResourceTypes.drives.ToString())
                    graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id + 
                    "/items/" + itemId + "/versions";

            else if (type == GraphResourceTypes.me.ToString())
                    graphUrl = graphRepository.BaseGraphUrl + "/" + type + 
                    "/drive/items/" + itemId + "/versions";

            else
                graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id +
                    "/drive/items/" + itemId + "/versions";


            var resultString = await graphRepository.Get(graphUrl);
            var resultJson = JObject.Parse(resultString);
            var driveDetails = resultJson["value"].ToString();
            var items = JsonConvert.DeserializeObject<List<DriveItemVersion>>(driveDetails);
            return items;
        }

        public async Task<object> PreviewItem(string type, string id, string itemId)
        {
            string graphUrl;
            type = type.ToLower();

            if (type == GraphResourceTypes.drives.ToString())
                graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id +
                "/items/" + itemId + "/preview";

            else if (type == GraphResourceTypes.me.ToString())
                graphUrl = graphRepository.BaseGraphUrl + "/" + type +
                "/drive/items/" + itemId + "/preview";

            else
                graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id +
                    "/drive/items/" + itemId + "/preview";

            var resultString = await graphRepository.Post(graphUrl, "");

            var response = JsonConvert.DeserializeObject<object>(resultString);
            return response;
        }

        //permissions section

        public async Task<Permission> CreateShareLink(string type, string id, string itemId, string linkType, 
            string scope = null)
        {
            string graphUrl;
            type = type.ToLower();
            var links = "view edit embed";
            if (!links.Contains(linkType))
                throw new BusinessException("Provided link type is incorrect");

            if (scope == null)
                scope = "";

            var body = new { type = linkType, scope };

            if (type == GraphResourceTypes.drives.ToString())
                graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id +
                "/items/" + itemId + "/createLink";

            else if (type == GraphResourceTypes.me.ToString())
                graphUrl = graphRepository.BaseGraphUrl + "/" + type +
                "/drive/items/" + itemId + "/createLink";

            else
                graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id +
                    "/drive/items/" + itemId + "/createLink";

            var resultString = await graphRepository.Post(graphUrl, body);

            var response = JsonConvert.DeserializeObject<Permission>(resultString);
            return response;
        }
        public async Task<List<Permission>> ListPermissions(string type, string id, string itemId)
        {
            string graphUrl;
            type = type.ToLower();


            if (type == GraphResourceTypes.drives.ToString())
                graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id +
                "/items/" + itemId + "/permissions";

            else if (type == GraphResourceTypes.me.ToString())
                graphUrl = graphRepository.BaseGraphUrl + "/" + type +
                "/drive/items/" + itemId + "/permissions";

            else
                graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id +
                    "/drive/items/" + itemId + "/permissions";

            var resultString = await graphRepository.Get(graphUrl);
            var resultJson = JObject.Parse(resultString);
            var driveDetails = resultJson["value"].ToString();
            var permissions = JsonConvert.DeserializeObject<List<Permission>>(driveDetails);

            return permissions;
        }

        public async Task<Permission> GetPermission(string type, string id, string itemId,
            string permissionId)
        {
            string graphUrl;
            type = type.ToLower();

            if (type == GraphResourceTypes.drives.ToString())
                graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id +
                "/items/" + itemId + "/permissions/" + permissionId;

            else if (type == GraphResourceTypes.me.ToString())
                graphUrl = graphRepository.BaseGraphUrl + "/" + type +
                "/drive/items/" + itemId + "/permissions/" + permissionId;

            else
                graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id +
                    "/drive/items/" + itemId + "/permissions/" + permissionId;

            var resultString = await graphRepository.Get(graphUrl);

            var permissions = JsonConvert.DeserializeObject<Permission>(resultString);

            return permissions;
        }

        public async Task<Permission> AddPermission(string type, string id, string itemId, object data)
        {
            string graphUrl;
            type = type.ToLower();

            if (type == GraphResourceTypes.drives.ToString())
                graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id +
                "/items/" + itemId + "/invite";

            else if (type == GraphResourceTypes.me.ToString())
                graphUrl = graphRepository.BaseGraphUrl + "/" + type +
                "/drive/items/" + itemId + "/invite";

            else
                graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id +
                    "/drive/items/" + itemId + "/invite";

            var resultString = await graphRepository.Post(graphUrl, data);
            var permissions = JsonConvert.DeserializeObject<Permission>(resultString);

            return permissions;
        }

        public async Task<Permission> UpdatePermission(string type, string id, string itemId,
            string permissionId, object data)
        {
            string graphUrl;
            type = type.ToLower();

            if (type == GraphResourceTypes.drives.ToString())
                graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id +
                "/items/" + itemId + "/permissions/" + permissionId;

            else if (type == GraphResourceTypes.me.ToString())
                graphUrl = graphRepository.BaseGraphUrl + "/" + type +
                "/drive/items/" + itemId + "/permissions/" + permissionId;

            else
                graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id +
                    "/drive/items/" + itemId + "/permissions/" + permissionId;

            var resultString = await graphRepository.Patch(graphUrl, data);
            var permissions = JsonConvert.DeserializeObject<Permission>(resultString);

            return permissions;
        }

        public async Task<bool> DeletePermission(string type, string id, string itemId,
            string permissionId)
        {
            string graphUrl;
            type = type.ToLower();

            if (type == GraphResourceTypes.drives.ToString())
                graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id +
                "/items/" + itemId + "/permissions/" + permissionId;

            else if (type == GraphResourceTypes.me.ToString())
                graphUrl = graphRepository.BaseGraphUrl + "/" + type +
                "/drive/items/" + itemId + "/permissions/" + permissionId;

            else
                graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id +
                    "/drive/items/" + itemId + "/permissions/" + permissionId;

            var isDeleted = await graphRepository.Delete(graphUrl);

            return isDeleted;
        }

    }
}
