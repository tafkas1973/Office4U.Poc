using System;

namespace Office4U.Presentation.Controller.Controllers.DTOs.AppUser
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