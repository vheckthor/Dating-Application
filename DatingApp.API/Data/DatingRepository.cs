using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DatingRepository : IDatingRepository
    {
        private readonly DataContext _context;
        public DatingRepository(DataContext context)
        {
            _context = context;

        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);

        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<Photo> GetMainPhotoForUser(Guid userId)
        {
            return await _context.Photos.Where(x => x.UserUniqueID == userId)
                            .FirstOrDefaultAsync(y=>y.IsMain);
        }

        public async Task<Photo> GetPhoto(Guid id)
        {
            var photo = await _context.Photos.FirstOrDefaultAsync(p => p.PhotoUniqueIdentifier == id);
            return photo;
        }

        public async Task<User> GetUser(Guid  UserUniqueIdentity)
        {
            var user = await _context.Users.Include(p=>p.Photos).FirstOrDefaultAsync(u=>u.UserUniqueIdentity == UserUniqueIdentity); 
         return user;
       }

        public async Task<PagedList<User>> GetUsers(UserParams param)
        {
            var users = _context.Users.Include(p=>p.Photos).AsQueryable();
            users = users.Where(x=> x.UserUniqueIdentity != param.UserId);
            users = users.Where( x => x.Gender == param.Gender);
            return await PagedList<User>.CreateAsync(users, param.PageNUmber,param.PageSize);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}