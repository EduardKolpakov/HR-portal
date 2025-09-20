namespace HRPortalApi.Requests
{
    public class NewNotification
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
        public bool IsRead { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
