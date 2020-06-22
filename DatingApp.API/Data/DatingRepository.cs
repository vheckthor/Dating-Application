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


       public async Task<Like> GetLike(int userId, int recipientId)
       {
           return await _context.Likes.FirstOrDefaultAsync(u => u.LikerId ==userId && u.LikeeId == recipientId);
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
            var users = _context.Users.Include(p=>p.Photos).OrderByDescending(u => u.LastActive).AsQueryable();
            users = users.Where(x=> x.UserUniqueIdentity != param.UserId);
            users = users.Where( x => x.Gender == param.Gender);
            
            if (param.Likers)
            {
                var userLikers = await GetUserLikes(param.UserId,param.Likers);
                users = users.Where(u => userLikers.Contains(u.UserUniqueIdentity));
            }
            if (param.Likees)
            {
                var userLikee = await GetUserLikes(param.UserId,param.Likers);
                users = users.Where(u => userLikee.Contains(u.UserUniqueIdentity));
            }
            if( param.MinAge != 18 || param.MaxAge != 99){
                users = users.Where( x => x.DateofBirth.GetAge()>= param.MinAge && x.DateofBirth.GetAge() <= param.MaxAge);
            }
            if(!string.IsNullOrEmpty(param.OrderBy))
            {
                switch (param.OrderBy)
                {
                    case "created":
                        users = users.OrderByDescending(u => u.Created);
                        break;
                    default:
                        users = users.OrderByDescending(u => u.LastActive);
                        break;
                    
                }
            }
            
            return await PagedList<User>.CreateAsync(users, param.PageNUmber,param.PageSize);
        }
        private async Task<IEnumerable<Guid>> GetUserLikes(Guid id, bool likers)
        {
            var user = await _context.Users.Include(x => x.Liker)
                                .Include(y => y.Likee)
                                .FirstOrDefaultAsync(s => s.UserUniqueIdentity == id);
            if (likers)
            {
                return user.Liker.Where(x => x.LikeeUniqueId == id).Select(i => i.LikerUniqueId);
            }
            else
            {
                return user.Likee.Where(u => u.LikerUniqueId ==id).Select(i => i.LikeeUniqueId);
            }
        }
        public async Task<Messages> GetMessage(Guid id)
        {
            return await _context.Messages.FirstOrDefaultAsync(x=>x.UniqueId == id);
        }

        public async Task<PagedList<Messages>> GetMessages(MessageParams messageParams)
        {
            var messages = _context.Messages
                            .Include(u => u.Sender).ThenInclude(p => p.Photos)
                            .Include(u => u.Recipient).ThenInclude(p => p.Photos)
                            .AsQueryable();
            switch (messageParams.MessageContainer)
            {
                case "Inbox":
                    messages = messages.Where(u => u.RecipientUniqueId == messageParams.UserId);
                break;
                case "Outbox":
                    messages = messages.Where(u => u.SenderUniqueId == messageParams.UserId);
                break;
                default:
                    messages = messages.Where(u => u.RecipientUniqueId == messageParams.UserId && u.IsRead == false);
                break;
            }
            messages = messages.OrderBy(d => d.MessageSent);     
            return await PagedList<Messages>.CreateAsync(messages, messageParams.PageNUmber,messageParams.PageSize);
        }

        public async Task<IEnumerable<Messages>> GetMessageThread(Guid userId, Guid recipientId)
        {
            var messages = await _context.Messages
                .Include(u => u.Sender).ThenInclude(p => p.Photos)
                .Include(u => u.Recipient).ThenInclude(p => p.Photos)
                .Where(m => m.RecipientUniqueId == userId && m.SenderUniqueId == recipientId 
                || m.RecipientUniqueId ==recipientId && m.SenderUniqueId ==userId)
                .OrderBy(m => m.MessageSent)
                .ToListAsync();
            return messages;
        }
        
        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

    }
}