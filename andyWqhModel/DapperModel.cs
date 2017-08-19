using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace andyWqhModel
{
    public class CICUser
    {
        public CICUser()
        {
            CICRole = new List<CICRole>();
        }
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsFirstTimeLogin { get; set; }

        public int AccessFailedCount { get; set; }

        public DateTime CreateDate { get; set; }

        public bool IsActive { get; set; }

        public List<CICRole> CICRole { get; set; }
    }

    public class CICRole
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }

    public class Customer
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsFirstTimeLogin { get; set; }

        public int AccessFailedCount { get; set; }

        public DateTime CreationDate { get; set; }

        public bool IsActive { get; set; }

        public CICRole CICRole { get; set; }
    }
}

