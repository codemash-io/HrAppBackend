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

        public async Task<Drive> GetUserSelectedOrDefaultDrive(string userId, string driveId = null)
        {
            string graphUrl;

            if (!string.IsNullOrEmpty(driveId))
                graphUrl = graphRepository.BaseGraphUrl + "/users/" + userId +
                    "/drives/" + driveId;
            else
                graphUrl = graphRepository.BaseGraphUrl + "/users/" + userId +
                    "/drive";

            var resultString = await graphRepository.Get(graphUrl);
            var drive = JsonConvert.DeserializeObject<Drive>(resultString);
            return drive;
        }

        public async Task<List<Drive>> GetAllUserDrives(string userId, string expand = null,
            string select = null, string skipToken = null, string top = null, string orderBy = null)
        {
            string graphUrl;
            if(string.IsNullOrEmpty(select) && string.IsNullOrEmpty(expand) && string.IsNullOrEmpty(skipToken)
                && string.IsNullOrEmpty(top) && string.IsNullOrEmpty(orderBy))
                graphUrl = graphRepository.BaseGraphUrl + "/users/" + userId + "/drives";
            else
            {
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
                string concat = String.Join(",", list.ToArray());

                graphUrl = graphRepository.BaseGraphUrl + "/users/" + userId + "/drives?" +
                   concat;
            }               

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
                concat = "?" + String.Join(",", list.ToArray());

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
                concat = "?" + String.Join(",", list.ToArray());

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

        public async Task<List<ThumbnailSet>> GetThumbnails(string type, string id, string itemId, string select = null)
        {
            string concat = "";
            if (select != null)
                concat = "?$select=" + select;

            string graphUrl;

            if (type == GraphResourceTypes.drives.ToString())
                graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id + "/items/" +
                    itemId + "/thumbnails" + concat;

            else if (type == GraphResourceTypes.me.ToString())
                graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/drive/items/" +
                    itemId + "/thumbnails" + concat;
            else
                graphUrl = graphRepository.BaseGraphUrl + "/" + type + "/" + id +
                    "/drive/items/" + itemId + "/thumbnails" + concat;
           
            var resultString = await graphRepository.Get(graphUrl);
            var resultJson = JObject.Parse(resultString);
            var driveDetails = resultJson["value"].ToString();
            var thumbnails = JsonConvert.DeserializeObject<List<ThumbnailSet>>(driveDetails);
            return thumbnails;
        }

    }
}
