using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace shopping.ViewModels
{
    public class LayoutVM
    {
        public int[][] vl { get; set; }
        public int Conut { get { return vl[0].Length; } }
        public string Convert { get { return JsonConvert.SerializeObject(vl); } }
        public string name1 { get; set; }
        public string name2 { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime dateTime { get; set; }
        public string m()
        {
            int[][] values = new int[][] {
             new int[] {4,3,10,9,29,19,25,9,12,7,19,5,13,9,17,2,7,5},
              new int[] {2,3,20,7,22,16,23,7,11,5,12,5,10,4,15,2,6,2}
                };
            
           return JsonConvert.SerializeObject(values);
        }
    }
}
