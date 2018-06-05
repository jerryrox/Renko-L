using System.Collections;
using System.Collections.Generic;
using Renko.Diagnostics;

namespace Renko.Data.Internal
{
    /// <summary>
    /// A class used for performing safety-checks before serializing.
    /// </summary>
    public class JsonSerializeSafety {

        /// <summary>
        /// Returns whether specified json data is safe, according to checks filtered with options.
        /// </summary>
        public static bool IsJsonSafe(JsonData data, JsonSerializeOptions options) {
            // No checks required.
            if(options.IgnoreSafetyChecks)
                return true;

            // Circular reference check.
            if(!options.IgnoreCircularReference && IsCircularReference(data)) {
                RenLog.Log(LogLevel.Error, "JsonSerializeSafety.IsJsonSafe - Failed to pass circular reference check.");
                return false;
            }

            // Everything looks good
            return true;
        }

        /// <summary>
        /// Returns whether specified data contains any circular referencing issue.
        /// </summary>
        private static bool IsCircularReference(JsonData data) {
            return CircularReferenceChecker.Process(data);
        }
    }
}