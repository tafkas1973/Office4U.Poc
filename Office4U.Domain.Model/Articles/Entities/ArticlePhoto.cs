using System.ComponentModel.DataAnnotations.Schema;

namespace Office4U.Domain.Model.Articles.Entities
{
    [Table("ArticlePhotos")]
    public class ArticlePhoto
    {
        public int Id { get; private set; }
        public string Url { get; private set; }
        public bool IsMain { get; private set; }
        public string PublicId { get; private set; }
        public Article Article { get; private set; }
        public int ArticleId { get; private set; }

        public static ArticlePhoto Create(string url, bool isMain)
        {
            return new ArticlePhoto()
            {
                Url = url,
                IsMain = isMain
            };
        }
    }
}