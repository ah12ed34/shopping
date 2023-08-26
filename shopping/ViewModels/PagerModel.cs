namespace shopping.ViewModels
{
    public class PagerModel
    {
       
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }
        public int TotalItems { get; private set; }
        public int TotalPages { get; private set; }
        public int StartPage { get; private set; }
        public int EndPage { get; private set; }
        public int StartRecord { get; private set; }
        public int EndRecord { get; private set; }


        public string Action { get; set; } = "Index";
        public string Search { get; set; }
        public string Sort { get; set; }
        public string Filter { get; set; }

        public PagerModel(int totalItems,int currentPage , int pageSize = 5)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalItems = totalItems;

            
            TotalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
            int startPage = CurrentPage - 5;
            int endPage = CurrentPage + 4;
            if (startPage <= 0)
            {
                endPage =endPage - (startPage - 1);
                startPage = 1;
            }
            if(endPage > TotalPages)
            {
                endPage = TotalPages;
                if (pageSize > 5)
                    startPage = pageSize - 4;
            }
            StartRecord = (currentPage - 1) * pageSize + 1;
            EndRecord = startPage - 1 + pageSize;

            if(EndPage >TotalPages)EndPage = TotalPages;
            if (totalItems == 0)
            {
                StartRecord = 0;
                EndRecord = 0;
                StartPage = 1;
                EndPage = 1;
                CurrentPage = 1;

            }
            else
            {
                StartPage = startPage;
                EndPage = endPage;
            }
        }
    }
}
