using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Helpers;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
    public interface IDatingRepository
    {
        void Add<T>(T entity) where T: class;
        void Delete<T>(T entity)where T: class;
        Task<bool>SaveAll();
        Task<PagedList<User>> GetUsers(UserParams userParams);
        Task<User> GetUser(Guid  UserUniqueIdentity);
        Task<Photo> GetPhoto(Guid id);
        Task<Photo> GetMainPhotoForUser(Guid userId);
        Task<Like> GetLike(int userId, int recipientId);
        
        Task<Messages> GetMessage(Guid id);
        
        Task<PagedList<Messages>> GetMessages(MessageParams messageParams);
        
        Task<IEnumerable<Messages>> GetMessageThread(Guid userId, Guid recipientId);
    }
}