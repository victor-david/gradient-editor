/**
 * Restless Animal Development
 * This program is provided freely without restrictions, but please retain this notice in any distributions, modifications ,etc.
 * http://dev.restlessanimal.com
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xam.Applications.GradientEditor
{
    /// <summary>
    /// Provides an enumeration for instaniating an AssemblyInfo object.
    /// </summary>
    public enum AssemblyInfoType
    {
        /// <summary>
        /// Information is retrieved from the entry assembly.
        /// </summary>
        Entry,
        /// <summary>
        /// Information is retrieved from the calling assembly.
        /// </summary>
        Calling,
        /// <summary>
        /// Information is retrieved from the executing assembly.
        /// </summary>
        Executing
    }
}
