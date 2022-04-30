﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotCDS.Common.Enum
{
    /// <summary>
    /// Determines how a host will will respond to a participant's delete
    /// action
    /// </summary>
    public enum RemoteDeleteBehavior
    {
        Unknown,

        /// <summary>
        /// If the host discovers that the participant has deleted the row
        /// then take no action (doesn't update the delete status locally)
        /// </summary>
        Ignore,

        /// <summary>
        /// If the host discovers that the participant has deleted the row
        /// then update the reference row with the delete data
        /// then logically delete the row 
        /// </summary>
        AutoDelete,

        /// <summary>
        /// If the host discovers that the participant has deleted the row
        /// then update the reference row with the delete data
        /// but do not perform a logical delete on the row
        /// (Note: The row still can be manually deleted on the host side
        /// at a later time)
        /// </summary>
        UpdateStatusOnly
    }
}
