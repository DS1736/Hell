using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
namespace InIWorkspace.Data.Entities
{
    [SQLite.Table("Admin")]
    public class Admin
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [SQLite.NotNull]
        public required string Username { get; set; }

        [SQLite.NotNull]
        public required string Password { get; set; }

    }
}
