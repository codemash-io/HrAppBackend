using System.Threading.Tasks;

namespace HrApp
{
    public interface IAbsenceRepository
    {
        Task<AbsenceRequestEntity> GetAbsenceById(string id);
        Task<string> GetAbsenceByIdWithNames(string id);
        Task<AbsenceRequestEntity> GetAbsenceByIdWithTypes(string id);
        Task InsertAbsenceWithSignature(string fileId, string entityId);
    }
}
