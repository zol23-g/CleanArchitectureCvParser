namespace Core.Common
{
    public class CursorPagedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int PageSize { get; set; }
        public int? LastSeenId { get; set; }
        public int? NextCursor { get; set; }
        public bool HasMore { get; set; }
    }
}
