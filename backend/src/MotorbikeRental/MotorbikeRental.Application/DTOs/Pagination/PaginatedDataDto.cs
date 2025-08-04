namespace MotorbikeRental.Application.DTOs.Pagination
{
    public class PaginatedDataDto<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int TotalCount { get; set; }
        public PaginatedDataDto(IEnumerable<T> data, int totalCount)
        {
            this.Data = data;
            this.TotalCount = totalCount;
        }
    }
}