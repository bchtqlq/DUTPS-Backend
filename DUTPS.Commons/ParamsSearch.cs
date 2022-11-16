namespace DUTPS.Commons
{
    public class ParamsSearch
    {
        
        public ParamsSearch ()
        {
            Sort = "ASC";
            SortBy = "id";
            Limit = 10;
            Page = 1;
        }

        public int Page { set; get; }

        public int Limit { set; get; }

        public string Sort { set; get; }

        public string SortBy { set; get; }

        public virtual string Order
        {
            get
            {
                return String.IsNullOrEmpty(Sort) ? "ASC" : Sort;
            }
        }

        public virtual string OrderBy
        {
            get
            {
                return String.IsNullOrEmpty(SortBy) ? "id" : SortBy;
            }
        }
    }
}