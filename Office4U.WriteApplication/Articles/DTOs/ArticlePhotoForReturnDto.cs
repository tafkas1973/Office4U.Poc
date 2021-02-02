namespace Office4U.WriteApplication.Articles.DTOs
{
    public class ArticlePhotoForReturnDto
    {
        public int Id { get; private set; }
        public string Url { get; private set; }
        public bool IsMain { get; private set; }
        public string PublicId { get; private set; }
    }
}