using InIWorkspace.Data.Entities;
using InIWorkspace.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SQLite;
namespace InIWorkspace.Services
{
    public class UserService
    {
        private readonly AppDbContext _dbContext;
        public UserService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region User
        public async Task<List<User>> GetUsersAsync()
        {
            // Get all users
            return await _dbContext.Users.ToListAsync();
        }

        public int SaveUser(User user)
        {
            if (user.Id != 0)
            {
                // Update user
                _dbContext.Users.Update(user);
                _dbContext.SaveChanges();
                return 0;
            }
            else
            {
                // Insert user
                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();
                return 0;
            }
        }

        public int DeleteUserAsync(User user)
        {
            // Delete user
            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();
            return 0;
        }

        // Delete all Users
        public int DeleteAllUsers()
        {
            // Dellete All users
            _dbContext.Users.RemoveRange(_dbContext.Users);
            _dbContext.SaveChanges();
            return 0;
        }
        #endregion

        #region Admin
        public async Task<List<Admin>> GetAdminsAsync()
        {
            // Get all admins
            return await _dbContext.Admins.ToListAsync();
        }

        // Get Admin by Username
        public Admin GetAdminByUsername(string username)
        {
            // Get admin by username
            var admin = _dbContext.Admins.Where(a => a.Username == username).FirstOrDefault();
            if (admin == null)
            {
                return default;
            }
            return admin;
        }
        #endregion
    }
}
