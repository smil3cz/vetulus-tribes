using GreenFoxAcademy.SpaceSettlers.Database;
using GreenFoxAcademy.SpaceSettlers.Helpers;
using GreenFoxAcademy.SpaceSettlers.Models.DTOs;
using GreenFoxAcademy.SpaceSettlers.Models.Entities;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GreenFoxAcademy.SpaceSettlers.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext dbContext;
        private static string APIKey { get => AppSettings.SendGridAPIKey; }

        public UserService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<User> GetUser(UserDto userDto)
        {
            return await dbContext.Users.FirstOrDefaultAsync(u => u.Username == userDto.Username);
        }

        public async Task RegUser(UserDto userDto)
        {
            byte[] salt = new byte[128 / 8];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: userDto.Password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            var user = new User()
            {
                Avatar = "https://forums.opera.com/assets/uploads/profile/192104-profileavatar.png",
                Username = userDto.Username,
                Email = userDto.Email,
                Password = hashed,
                Salt = Convert.ToBase64String(salt),
                VerificationToken = Guid.NewGuid(),
                IsVerified = false
            };
            var kingdom = new Kingdom()
            {
                Name = userDto.KingdomName,
                Location = await CreateLocation(),
                Buildings = new List<Building>(),
                Resources = new List<Resource>(),
                Ships = new List<Ship>()
            };
            var buildings = new List<Building>()
            {
                new Building()
                {
                    Type = 0,
                    Level = 1,
                    HP = 100,
                    StartedAt = DateTime.UtcNow,
                    FinishedAt = DateTime.UtcNow
                }
            };
            var resources = new List<Resource>()
            {
                new Resource() {
                    Type = 0,
                    Amount = 0,
                    NetGeneration = 10,
                    LastUpdated = DateTime.UtcNow,
                    MaxAmount = 1000
                },
                new Resource() {
                    Type = (ResourceType)1,
                    Amount = 250,
                    NetGeneration = 10,
                    LastUpdated = DateTime.UtcNow,
                    MaxAmount = 1000
                },
            };
            user.Kingdom = kingdom;
            user.Kingdom.Buildings = buildings;
            user.Kingdom.Resources = resources;
            await dbContext.AddAsync(user);
            await dbContext.SaveChangesAsync();
        }

        public async Task<User> AuthenticateUser(UserDto userDto)
        {
            var user = await GetUser(userDto);
            if (user != null && !user.IsVerified)
            {
                return null;
            }
            if (user != null && user.IsVerified)
            {
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: userDto.Password,
                salt: Convert.FromBase64String(user.Salt),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

                return hashed == user.Password ? user : null;
            }
            return user;
        }

        public async Task<string> GenerateJSONWebToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("This is my secret"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[] {
                new Claim("Username", user.Username)
            };

            var token = new JwtSecurityToken(
               "Server",
              "Server",
              claims,
              expires: DateTime.UtcNow.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task SaveToken(string token)
        {
            var jwt = new Token { Jwt = token };
            await dbContext.Whitelist.AddAsync(jwt);
            await dbContext.SaveChangesAsync();
        }
        public virtual async Task<User> GetUser(long id)
        {
            return await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<HttpStatusCode> SendConfirmationEmail(User user)
        {
            var client = new SendGridClient("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");
            var queryToken = Convert.ToBase64String(Encoding.Unicode.GetBytes(String.Format("{0}&{1}", user.Id, user.VerificationToken)));
            var subject = "Verify Your Email Address";
            var plainTextContent = $"Welcome {user.Username}! {user.Kingdom.Name} is ready! You just need to confirm your email address and then you are ready to conquer the world!  Please confirm your email address by clicking the following link:";
            var htmlContent = $"{plainTextContent} <a href=\"https://thespacesettlers.herokuapp.com/verify?token={queryToken}\">Click Here!</a> Best, The Tribes Team.";
            var msg = new SendGridMessage();
            msg.AddTo(user.Email, user.Username);
            msg.SetFrom("marek.schutz@centrum.cz", "The Tribes Team");
            msg.AddContent(MimeType.Text, plainTextContent);
            msg.AddContent(MimeType.Html, htmlContent);
            msg.SetSubject(subject);
            var response = await client.SendEmailAsync(msg);
            var body = await response.Body.ReadAsStringAsync();
            return response.StatusCode;
        }

        public async Task<bool> VerifyEmail(string verificationToken)
        {
            var tokenData = Encoding.Unicode.GetString(Convert.FromBase64String(verificationToken)).Split(new Char[] { '&' });
            var user = dbContext.Users.FirstOrDefault(u => u.Id == Convert.ToInt64(tokenData[0]));
            if (user.VerificationToken == Guid.Parse(tokenData[1]))
            {
                user.IsVerified = true;
                await dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Location> CreateLocation()
        {
            var random = new Random();
            var randomX = 0;
            var randomY = 0;
            var location = new Location();
            var allPositions = await dbContext.Kingdoms.Select(k => k.Location).AsNoTracking().ToListAsync();
            var check = true;
            while (check)
            {
                randomX = random.Next(1, 1000);
                randomY = random.Next(1, 1000);
                location.PositionX = randomX;
                location.PositionY = randomY;
                check = allPositions.Any(l => l.PositionX == location.PositionX && l.PositionY == location.PositionY);
            }
            return location;
        }
    }
}
