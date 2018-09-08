using Core1.Model;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core1.Infrastructure.Identity
{
    public class AppIdentityRole : IdentityRole
    {
        public AppIdentityRole() : base()
        {
        }

        public AppIdentityRole(string roleName) : base(roleName)
        {
        }

        public string PermissionsString { get; set; }
        public IEnumerable<Permission> Permissions => GetPermissions();

        public void AddPermission(Permission permission)
        {
            var permissionAsInt = (int)permission;

            if (String.IsNullOrWhiteSpace(PermissionsString))
            {
                PermissionsString = permissionAsInt.ToString();
                return;
            }

            if (!HasPermission(permission))
            {
                PermissionsString = PermissionsString + $",{permissionAsInt}";
            }
        }

        public void AddPermissions(IEnumerable<Permission> permissions)
        {
            if (permissions == null) throw new ArgumentNullException(nameof(permissions));

            permissions.ForEach(p => AddPermission(p));
        }

        public bool HasPermission(Permission permission)
        {
            if (String.IsNullOrWhiteSpace(PermissionsString)) return false;

            return PermissionsString
                .Split(',')
                .Contains(((int)permission).ToString());
        }

        public void RemovePermission(Permission permission)
        {
            if (String.IsNullOrWhiteSpace(PermissionsString)) return;

            PermissionsString = RemoveStringFromCSV(((int)permission).ToString(), PermissionsString);
        }

        public void RemovePermissions(IEnumerable<Permission> permissions)
        {
            if (permissions == null) throw new ArgumentNullException(nameof(permissions));

            permissions.ForEach(p => RemovePermission(p));
        }

        private IEnumerable<Permission> GetPermissions()
        {
            if (String.IsNullOrWhiteSpace(PermissionsString)) return Enumerable.Empty<Permission>();

            return PermissionsString
                .Split(',')
                .Select(str => (Permission)Enum.Parse(typeof(Permission), str));
        }

        private string RemoveStringFromCSV(string stringToBeRemoved, string csv)
        {
            if (String.IsNullOrWhiteSpace(stringToBeRemoved)) throw new ArgumentNullException(nameof(stringToBeRemoved));
            if (String.IsNullOrWhiteSpace(csv)) throw new ArgumentNullException(nameof(csv));

            return csv
                .Split(',')
                .Remove(stringToBeRemoved)
                .Join(",");
        }
    }
}