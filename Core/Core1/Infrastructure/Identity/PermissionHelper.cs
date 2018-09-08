using Core1.Model;
using System.Collections.Generic;

namespace Core1.Infrastructure.Identity
{
    public class PermissionHelper
    {
        private static IList<Permission> _applicationAdminPermissions;
        public static IList<Permission> ApplicationAdminPermissions
        {
            get
            {
                if (_applicationAdminPermissions == null)
                {
                    _applicationAdminPermissions = new List<Permission>
                    {
                    };
                }

                return _applicationAdminPermissions;
            }
        }

        private static IList<Permission> _customerAdminPermissions;
        public static IList<Permission> CustomerAdminPermissions
        {
            get
            {
                if (_customerAdminPermissions == null)
                {
                    _customerAdminPermissions = new List<Permission>
                    {
                    };
                }

                return _customerAdminPermissions;
            }
        }
    }
}