namespace Renko.Debug
{
	/// <summary>
	/// Levels of log for display.
	/// </summary>
	public enum LogLevel {
		/// <summary>
		/// Displays miscellaneous messages.
		/// May display some sensitive values.
		/// </summary>
		Info = 0,

		/// <summary>
		/// Displays warnings that are less critical.
		/// </summary>
		Warning,

		/// <summary>
		/// Displays errors that highly recommend checking
		/// </summary>
		Error,

		/// <summary>
		/// Setting current level to this will not allow any debug logs to appear.
		/// </summary>
		Never
	}
}