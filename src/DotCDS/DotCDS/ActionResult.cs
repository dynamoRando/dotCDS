using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotCDS
{
    /// <summary>
    /// Represents the result of an action requested and if successful returns a result of type T
    /// </summary>
    /// <typeparam name="T">The type of result</typeparam>
    internal record struct ActionResult<T>
    {
        public T Result { get; set; }
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
    }

    /// <summary>
    /// Represents the result of an action requested and indicates if successful or not
    /// </summary>
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
