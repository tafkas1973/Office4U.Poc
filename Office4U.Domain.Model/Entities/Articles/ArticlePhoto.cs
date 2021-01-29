using System.ComponentModel.DataAnnotations.Schema;

namespace Office4U.Domain.Model.Entities.Articles
{

    // TODO1: make PhotoBase class
    // TODO2: implement discriminator

    [Table("ArticlePhotos")]
    public class ArticlePhoto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; }
        public Article Article { get; set; }
        public int ArticleId { get; set; }
    }
}