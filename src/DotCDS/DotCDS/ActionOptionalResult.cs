using DotCDS.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotCDS
{
    /// <summary>
    /// Represents the result of an action requested that accounts for potential
    /// side effects
    /// </summary>
    /// <typeparam name="T">The type of result</typeparam>
    /// <remarks>This is related to <seealso cref="ActionResult{T}"/>
    /// but with more information</remarks>
    internal record struct ActionOptionalResult<T>
    {
        public T Result { get; set; }
        public ActionOptionalResultStatus Status { get; set; }
        public string Message { get; set; }
    }

    /// <summary>
    /// Represents the result of an action requested that accounts for potential
    /// side effects
    /// </summary>
    /// <remarks>This is related to <seealso cref="ActionResult"/>
    /// but with more information</remarks>
    internal record struct ActionOptionalResult
    {
        public ActionOptionalResultStatus Status { get; set; }
        public string Message { get; set; }

        public ActionOptionalResult()
        {
            Status = ActionOptionalResultStatus.Unknown;
            Message = string.Empty;
        }
    }
}
