using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Api;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Services
{
    public class UserService: IUser
    {
        private readonly AppDbContext db;
        public UserService(AppDbContext m)
        {
            db = m;
        }

        public async Task<User> Create(User t)
        {
            try
            {
                // 1. קודם כל שומר את המשתמש (בלי הלקוח)
                var userEntry = await db.Users.AddAsync(new User
                {
                    Email = t.Email,
                    Password = t.Password,
                    RoleId = t.RoleId,
                    DateCreated = t.DateCreated,
                    IsActive = t.IsActive
                });

                await db.SaveChangesAsync(); // כדי שיתקבל ה־ID

                var userId = userEntry.Entity.Id;

                // 2. אם יש לקוח, מקשר אותו עם ה־UserId
                if (t.Customer != null)
                {
                    t.Customer.UserId = userId;
                    await db.Customers.AddAsync(t.Customer);
                    await db.SaveChangesAsync();
                }

                return userEntry.Entity;
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating User", ex);
            }
        }


        public async Task<User> Delete(User t)
        {
            try
            {
                var User = await db.Users.FindAsync(t.Id);
                if (User == null)
                {
                    return null;
                }

                db.Users.Remove(User);
                await db.SaveChangesAsync();
                return User;
            }
            catch (Exception ex)
            {
                throw new Exception("Error Deleteing User", ex);
            }
        }

        public async Task<User> Update(User User)
        {
            try
            {
                db.Users.Update(User);
                await db.SaveChangesAsync();
                return User;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating User", ex);
            }
        }

        public async Task<List<User>> GetAll()
        {
            try
            {
                return await db.Users
                    .Include(u => u.Role)
                    .Include(u => u.Customer)
                        .ThenInclude(c => c.AgeGroupNavigation).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving User data", ex);
            }
        }

        public async Task<User?> GetUserById(int id)
        {
            try
            {
                return await db.Users
                    .Include(u => u.Role)
                    .Include(u => u.Customer)
                        .ThenInclude(c => c.AgeGroupNavigation)
                    .FirstOrDefaultAsync(u => u.Id == id);

            }
            catch (Exception ex)
            {
                throw new Exception("שגיאה בהחזרת משתמש לפי מזהה", ex);
            }
        }

    }
}
