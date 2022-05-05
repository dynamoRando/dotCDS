using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotCDS.Enum
{
    /// <summary>
    /// Represents the result of an action with 
    /// various outcomes
    /// </summary>
    internal enum ActionOptionalResultStatus
    {
        /// <summary>
        /// Default
        /// </summary>
        Unknown,

        /// <summary>
        /// A critical failure has occured with the action
        /// </summary>
        Error,

        /// <summary>
        /// The action has partially succeeded but with potential side 
        /// effects
        /// </summary>
        Information,

        /// <summary>
        /// The action has succeeded
        /// </summary>
        Success
    }
}
