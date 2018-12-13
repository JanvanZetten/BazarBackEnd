using Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace infrastructure.Development
{
    public static class DatabaseInitialize
    {
        // This method will create and seed the database.
        public static void Initialize(BazarContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash("Abcd1234", out passwordHash, out passwordSalt);

            var users = new List<User>
            {
                new User { Username="Jan", PasswordHash=passwordHash, PasswordSalt=passwordSalt, IsAdmin=true },
                new User { Username="Alex", PasswordHash=passwordHash, PasswordSalt=passwordSalt, IsAdmin=true },
                new User { Username="Hussain", PasswordHash=passwordHash, PasswordSalt=passwordSalt, IsAdmin=false },
                new User { Username="Asbjørn", PasswordHash=passwordHash, PasswordSalt=passwordSalt, IsAdmin=false }
            };

            context.Users.AddRange(users);

            var booths = new List<Booth>
            {
                new Booth { Booker = users[0] },
                new Booth { Booker = users[1] },
                new Booth { Booker = users[2] }
            };

            context.Booth.AddRange(booths);

            var waitingList = new List<WaitingListItem>
            {
                new WaitingListItem { Booker = users[3], Date = DateTime.Now.AddDays(-1) }
            };

            context.WaitingListItem.AddRange(waitingList);

            ImageURL hal1 = new ImageURL()
            {
                URL = "https://i.imgur.com/ilIGLPC.png?1"
            };
            ImageURL hal2 = new ImageURL()
            {
                URL = "https://i.imgur.com/dOnrxpM.png"
            };

            context.ImageURL.Add(hal1);
            context.ImageURL.Add(hal2);

            context.SaveChanges();
        }
        
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
