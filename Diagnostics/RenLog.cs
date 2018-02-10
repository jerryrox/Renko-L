using UnityEngine;

namespace Renko.Diagnostics
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
				Debug.Log(message);
				break;

			case LogLevel.Warning:
				Debug.LogWarning(message);
				break;

			case LogLevel.Error:
				Debug.LogError(message);
				break;
			}
		}
	}
}