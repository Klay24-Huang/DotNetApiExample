using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Db
{
    public class TCoeusDbContext : DbContext
    {
        public TCoeusDbContext(DbContextOptions<TCoeusDbContext> options) : base(options)
        {
        }

        // 定義資料表對應
        public DbSet<User> Users { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
