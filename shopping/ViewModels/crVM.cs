using shopping.Models;

namespace shopping.ViewModels
{
    public class crVM : Role
    {
        public int uid { get; set; }
        
        public bool IsCheck()
        {
            foreach (var item in IdMs)
            {
                if (item.Id == uid)
                    return true;
            }
            return false;
        }
    }
}
