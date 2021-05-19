namespace ShopT.Models
{
    public partial class ErrorReport
    {
        public int ErrorReportId { get; set; }
        public string Text { get; set; }
        public int? UserId { get; set; }

        public virtual User User { get; set; }
    }
}
