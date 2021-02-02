using System;

namespace Office4U.ReadApplication.Users.DTOs
{
    public class AppUserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int Age { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
    }
}