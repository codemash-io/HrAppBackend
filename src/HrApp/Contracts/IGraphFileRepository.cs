using Microsoft.Graph;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HrApp
{
    public interface IGraphFileRepository
    {
        Task<Drive> GetDrive(string type, string id = null, string select = null);
        Task<List<Drive>> ListAllDrives(string type, string id = null, string expand = null,
                    string select = null, string skipToken = null, string top = null, string orderBy = null);
        Task<DriveItem> GetSpecialFolder(string type, string folderName, string id = null);
        Task<List<DriveItem>> SharedWithMe(string type, string id = null);
        Task<List<DriveItem>> ListChildren(string type, string id = null,
                    string itemId = null, string path = null, string expand = null, string select = null,
                    string skipToken = null, string top = null, string orderBy = null);
        Task<DriveItem> GetItem(string type, string id = null, string itemId = null,
                    string path = null, string expand = null, string select = null);
        Task<List<ThumbnailSet>> GetThumbnails(string type, string itemId, string id = null,
                    string select = null);
        Task<ThumbnailSet> GetSingleThumbnail(string type, string itemId,
                    string thumId, string id = null, string size = null);
        Task<DriveItem> CreateFolder(string type, string itemId, DriveItem item, string id = null);
        Task<DriveItem> UpdateItem(string type, string itemId, string name, string id = null);
        Task<bool> DeleteItem(string type, string itemId, string id = null);
        Task<DriveItem> MoveItem(string type, string itemId, string parentFolderId, string id = null);
        Task<DriveItem> CopyItem(string type, string itemId, DriveItem item, string id = null);
        Task<byte[]> DownloadFile(string type, string itemId, string id = null, string format = null);
        Task<List<DriveItem>> TrackChanges(string type, string id = null, string select = null,
                    string expand = null, string top = null, string token = null);
        Task<DriveItem> UploadNew(string type, string path, byte[] file, string id = null);
        Task<DriveItem> UploadReplace(string type, string itemId, byte[] file, string id = null);
        Task<List<DriveItemVersion>> ListVersions(string type, string itemId, string id = null);
        Task<object> PreviewItem(string type, string itemId, string id = null);
        Task<Permission> CreateShareLink(string type, string itemId, string linkType, string id = null,
                    string scope = null);
        Task<List<Permission>> ListPermissions(string type, string itemId, string id = null);
        Task<Permission> GetPermission(string type, string itemId,
                    string permissionId, string id = null);
        Task<Permission> AddPermission(string type, string itemId, object data, string id = null);
        Task<Permission> UpdatePermission(string type, string itemId,
                    string permissionId, object data, string id = null);
        Task<bool> DeletePermission(string type, string itemId,
                    string permissionId, string id = null);
        Task<List<DriveItem>> ListRecent(string type, string id = null);
        Task<List<DriveItem>> Search(string type, string searchText, string id = null, string top = null,
            string expand = null, string select = null, string skipToken = null, string orderBy = null);
    }
}
