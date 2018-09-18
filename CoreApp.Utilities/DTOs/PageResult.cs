using System.Collections;
using System.Collections.Generic;

namespace CoreApp.Utilities.DTOs
{
    public class PageResult<T>:PageResultBase where T:class
    {
        public PageResult()
        {
            Results= new List<T>();
        }
        public IList Results { get; set; }
    }
}
