

namespace Renko.Data
{
    /// <summary>
    /// Options that affect a json serialization process.
    /// </summary>
    public struct JsonSerializeOptions {

        /// <summary>
        /// Default options to apply during serialization when none was specified.
        /// </summary>
        private static JsonSerializeOptions DefaultOption = new JsonSerializeOptions { IgnoreSafetyChecks = true };

        /// <summary>
        /// Whether all safety checks should be ignored.
        /// </summary>
        public bool IgnoreSafetyChecks;

        /// <summary>
        /// Whether circular reference check should be ignored.
        /// </summary>
        public bool IgnoreCircularReference;


        /// <summary>
        /// Returns the default options:
        /// - Does not perform any safety checks.
        /// </summary>
        public static JsonSerializeOptions Default {
            get { return DefaultOption; }
        }
    }
}