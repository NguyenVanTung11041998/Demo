namespace DemoWebApi.Helpers
{
    public class GridResult<T>
    {
        public int TotalCount { get; set; }
        public IReadOnlyList<T> Items { get; set; }

        public GridResult(int totalCount, IReadOnlyList<T> items)
        {
            TotalCount = totalCount;
            Items = items;
        }
    }
}
