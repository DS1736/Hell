using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
namespace InIWorkspace.Data.Entities
{
    [SQLite.Table("Users")]
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public required string Username { get; set; }
        public string[] ActivationCode { get; set; } = new string[4];

        public required string MacAddress { get; set; }

        public required string CreatedDate { get; set; }

        public bool IsActive { get; set; }
    }
}
