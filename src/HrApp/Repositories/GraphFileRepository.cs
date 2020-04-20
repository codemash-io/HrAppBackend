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

        public async Task<List<DriveItem>> SharedWithMe(string userId)
        {
            string graphUrl = graphRepository.BaseGraphUrl + "/users/" + userId +
                    "/drive/sharedWithMe";

            var resultString = await graphRepository.Get(graphUrl);
            var resultJson = JObject.Parse(resultString);
            var itemsDetails = resultJson["value"].ToString();
            var sharedItems = JsonConvert.DeserializeObject<List<DriveItem>>(itemsDetails);
            return sharedItems;
        }

    }
}
