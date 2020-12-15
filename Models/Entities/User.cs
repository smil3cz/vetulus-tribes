using System;
using System.ComponentModel.DataAnnotations;

namespace GreenFoxAcademy.SpaceSettlers.Models.Entities

{
    public class User
    {
        [Key]
        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public Guid VerificationToken { get; set; }
        public bool IsVerified { get; set; }
        public string Salt { get; set; }
        public Kingdom Kingdom { get; set; }
        [Url]
        public string? Avatar { get; set; }
        public int Points { get; set; }

        public User()
        {
        }

        public User(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }
    }
}
