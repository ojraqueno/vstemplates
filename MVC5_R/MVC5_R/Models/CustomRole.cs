using System;
using System.Collections.Generic;
using System.Linq;

namespace MVC5_R.Models
{
    public class CustomRole
    {
        public DateTime AddedOn { get; set; }
        public int Id { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string Name { get; set; }
        public string PermissionsString { get; set; }
        public ICollection<User> Users { get; set; }

        public void AddPermission(Permission permission)
        {
            if (String.IsNullOrWhiteSpace(PermissionsString))
            {
                PermissionsString = permission.ToString();
            }
            else
            {
                PermissionsString = PermissionsString + $",{permission}";
            }
        }

        public void AddPermissions(IEnumerable<Permission> permissions)
        {
            if (permissions == null) throw new ArgumentNullException(nameof(permissions));

            foreach (var permission in permissions)
            {
                AddPermission(permission);
            }
        }

        public void RemovePermission(Permission permission)
        {
            if (String.IsNullOrWhiteSpace(PermissionsString)) return;            

            PermissionsString = RemoveStringFromCSV(permission.ToString(), PermissionsString);
        }

        public void RemovePermissions(IEnumerable<Permission> permissions)
        {
            if (permissions == null) throw new ArgumentNullException(nameof(permissions));

            foreach (var permission in permissions)
            {
                RemovePermission(permission);
            }
        }

        private string RemoveStringFromCSV(string stringToBeRemoved, string csv)
        {
            var split = csv.Split(',').ToList();
            if (!split.Contains(stringToBeRemoved)) return csv;

            split.Remove(stringToBeRemoved);

            return String.Join(",", split);
        }
    }
}