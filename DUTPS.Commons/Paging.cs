namespace DUTPS.Commons
{
    public class Paging
    {
        public int Page { set; get; }

        public int TotalPages { set; get; }

        public int Limit { set; get; }

        public int Total { set; get; }

        public Paging()
        {
            this.Limit = 10;
            this.TotalPages = 1;
            this.Page = 1;
            this.Total = 0;
        }

        public Paging(int TotalRecord, int CurrenPage, int NumberOfRecord = 30)
        {
            this.Total = TotalRecord;
            this.Limit = NumberOfRecord;
            if (this.Limit == 0)
            {
                this.Limit = TotalRecord;
            }
            this.TotalPages = TotalRecord / this.Limit + (TotalRecord % this.Limit > 0 ? 1 : 0);
            if (CurrenPage > this.TotalPages)
            {
                CurrenPage = this.TotalPages == 0 ? 1 : this.TotalPages;
            }
            if (CurrenPage < 1)
            {
                CurrenPage = 1;
            }
            this.Page = CurrenPage;
        }
    }

    public class PaginatedList<T>
    {
        public List<T> List { get; set; }
        public Paging Paging { set; get; }
    }
}