namespace JWTAuthAPI.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string label { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public string EditedBy { get; set; } = "API";

    }
}
