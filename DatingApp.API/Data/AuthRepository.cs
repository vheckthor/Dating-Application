using System.Text;
using System.Text.Encodings;
using System.Security.Cryptography;
using System;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            _context = context;

        }
        public async Task<User> Login(string username, string password)
        {
           var finder =await _context.Users.Include(x => x.Photos).FirstOrDefaultAsync(x=>x.Username == username); 
           if(finder==null){
                return null;
           }
           if(!VerifyPasswordHash(password,finder.PasswordHash,finder.PasswordSalt)){
               return null;
           }

           return finder;
         
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
             using( var haser = new HMACSHA512(passwordSalt)){
             var ComputedpasswordHash = haser.ComputeHash(Encoding.UTF8.GetBytes(password));  
           
            for(int i = 0; i<ComputedpasswordHash.Length; i++){
                if(ComputedpasswordHash[i]!=passwordHash[i]){
                    return false;
                }   
            }

           }
           return true;
           
        }

        public async Task<User> Register(User user, string password)
        {
            byte [] passwordhash, passwordSalt;
            CreatePasswordHash(password,out passwordhash, out passwordSalt);
            user.PasswordHash = passwordhash;
            user.PasswordSalt = passwordSalt;
            await _context.Users.AddAsync(user);
            var success =await _context.SaveChangesAsync()>0;
            if(success){
                return user;
            }
            return null;

        }

        private void CreatePasswordHash(string password, out byte[] passwordhash, out byte[] passwordSalt)
        {
           using( var haser = new HMACSHA512()){
             passwordSalt=haser.Key;
             passwordhash = haser.ComputeHash(Encoding.UTF8.GetBytes(password));  
           }
        }

        public async Task<bool> UserExists(string username)
        {
           if(await _context.Users.AnyAsync(x=>x.Username==username)){
                return true;
           }

           return false;

        }
    }
}