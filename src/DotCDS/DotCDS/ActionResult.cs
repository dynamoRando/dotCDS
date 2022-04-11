using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotCDS
{
    internal record struct ActionResult<T>
    {
        public T Result { get; set; }
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
    }

    internal record struct ActionResult
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; } 

        public ActionResult()
        {
            IsSuccessful = false;
            Message = string.Empty;
        }
    }
}
