using Microsoft.Graph;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HrApp
{
    public interface IGraphFileRepository
    {
        Task<Drive> GetDrive(string type, string id = null);
        Task<List<Drive>> ListAllDrives(string type, string id, string expand = null,
                    string select = null, string skipToken = null, string top = null, string orderBy = null);
        Task<DriveItem> GetSpecialFolder(string userId, string folderName);
        Task<List<DriveItem>> SharedWithMe(string userId, string driveId = null);
        Task<List<DriveItem>> ListChildren(string type, string id,
                    string itemId = null, string path = null, string expand = null, string select = null,
                    string skipToken = null, string top = null, string orderBy = null);
        Task<DriveItem> GetItem(string type, string id, string itemId = null,
                    string path = null, string expand = null, string select = null);
        Task<List<ThumbnailSet>> GetThumbnails(string type, string id, string itemId,
                    string select = null);
        Task<ThumbnailSet> GetSingleThumbnail(string type, string id, string itemId,
                    string thumId, string size = null);
        Task<DriveItem> CreateFolder(string type, string id, string itemId, DriveItem item);
        Task<DriveItem> UpdateItem(string type, string id, string itemId, string name);
        Task<bool> DeleteItem(string type, string id, string itemId);
        Task<DriveItem> MoveItem(string type, string id, string itemId, string parentFolderId);
        Task<DriveItem> CopyItem(string type, string id, string itemId, DriveItem item);
        Task<byte[]> DownloadFile(string type, string id, string itemId, string format = null);
        Task<List<DriveItem>> TrackChanges(string type, string id, string select = null,
                    string expand = null, string top = null, string token = null);
        Task<DriveItem> UploadNew(string type, string id, string path, byte[] file);
        Task<DriveItem> UploadReplace(string type, string id, string itemId, byte[] file);
        Task<List<DriveItemVersion>> ListVersions(string type, string id, string itemId);
        Task<object> PreviewItem(string type, string id, string itemId);
        Task<Permission> CreateShareLink(string type, string id, string itemId, string linkType,
                    string scope = null);
        Task<List<Permission>> ListPermissions(string type, string id, string itemId);
        Task<Permission> GetPermission(string type, string id, string itemId,
                    string permissionId);
        Task<Permission> AddPermission(string type, string id, string itemId, object data);
        Task<Permission> UpdatePermission(string type, string id, string itemId,
                    string permissionId, object data);
        Task<bool> DeletePermission(string type, string id, string itemId,
                    string permissionId);

    }
}
