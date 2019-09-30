using System.Threading.Tasks;

namespace BeSafe.Core.Interfaces
{
    public interface IPermissionService
    {
        void PermissionRequest(string permissionCode);
    }
}