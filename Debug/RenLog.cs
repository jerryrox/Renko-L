namespace Renko.Debug
{
	/// <summary>
	/// A class used by Renko L for outputting log messages.
	/// </summary>
	public static class RenLog {
		
		/// <summary>
		/// Current debug log level.
		/// Set it to higher level if you don't like to see specific log levels.
		/// </summary>
		public static LogLevel CurrentLevel {
			get; set;
		}

		/// <summary>
		/// Outputs a standard debug log with given level and message.
		/// </summary>
		public static void Log(LogLevel level, object message) {
			if((int)level < (int)CurrentLevel)
				return;
			
			switch(level) {
			case LogLevel.Info:
				UnityEngine.Debug.Log(message);
				break;

			case LogLevel.Warning:
				UnityEngine.Debug.LogWarning(message);
				break;

			case LogLevel.Error:
				UnityEngine.Debug.LogError(message);
				break;
			}
		}
	}
}