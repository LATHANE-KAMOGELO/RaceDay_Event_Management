namespace RaceDayAPI.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public int EventID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int AgeMinimum { get; set; }
        public int AgeMaximum { get; set; }
        public decimal Distance { get; set; }
    }
}