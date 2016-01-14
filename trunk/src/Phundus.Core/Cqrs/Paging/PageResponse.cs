namespace Phundus.Cqrs.Paging
{
    public class PageResponse : PageRequest
    {
        public int Total { get; set; }

        public static PageResponse From(PageRequest request, int total)
        {
            return new PageResponse
                       {
                           Index = request.Index,
                           Size = request.Size,
                           Total = total
                       };
        }
    }
}