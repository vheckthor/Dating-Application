using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using DatingApp.API.Models;
using Newtonsoft.Json;

namespace DatingApp.API.Data
{
    public class Seed
    {
        public static void SeedUsers(DataContext context)
        {
            if(!context.Users.Any())
            {
                var userData = File.ReadAllText("Data/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);

                foreach (var user in users)
                {
                    byte[] passwordHash, passwordSalt;
                    CreatePasswordHash("password",out passwordHash, out passwordSalt);
                    user.PasswordHash=passwordHash;
                    user.PasswordSalt=passwordSalt;
                    user.Username=user.Username.ToLower();
                    user.UserUniqueIdentity = Guid.NewGuid();
                    user.Photos.ToList().ForEach(
                        x => {x.UserUniqueID = user.UserUniqueIdentity;
                        x.PhotoUniqueIdentifier = Guid.NewGuid();}
                        );
                    context.Users.Add(user);
                }

                context.SaveChanges();
            }
        }

         private static void CreatePasswordHash(string password, out byte[] passwordhash, out byte[] passwordSalt)
        {
           using( var haser = new HMACSHA512()){
             passwordSalt=haser.Key;
             passwordhash = haser.ComputeHash(Encoding.UTF8.GetBytes(password));  
           }
        }

    }
}