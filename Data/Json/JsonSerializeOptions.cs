

namespace Renko.Data
{
    /// <summary>
    /// Options that affect a json serialization process.
    /// </summary>
    public struct JsonSerializeOptions {
		
        /// <summary>
        /// Whether all safety checks should be ignored.
        /// </summary>
        public bool IgnoreSafetyChecks;

        /// <summary>
        /// Whether circular reference check should be ignored.
        /// </summary>
        public bool IgnoreCircularReference;

		/// <summary>
		/// Whether unicode characters should be saved as-is.
		/// </summary>
		public bool IgnoreUnicodeEncode;


        /// <summary>
        /// Returns the default options:
        /// - Does not perform any safety checks.
        /// </summary>
        public static JsonSerializeOptions Default {
			get { return new JsonSerializeOptions { IgnoreSafetyChecks = true }; }
        }
    }
}