using System.ComponentModel.DataAnnotations.Schema;

namespace Office4U.Domain.Model.Users.Entities
{
    [Table("AppUserPhotos")]
    public class AppUserPhoto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; }
        public AppUser AppUser { get; set; }
        public int AppUserId { get; set; }
    }
}
