#if NGUI
using System;

namespace Renko.MVCFramework
{
	/// <summary>
	/// Defines type of methods to use for UIs' lifecycle.
	/// </summary>
	public enum MvcLifeType {

		/// <summary>
		/// Use the default type specified in MVC.
		/// </summary>
		Default = 0,

		/// <summary>
		/// Destroys the UI gameObject when a view is closed.
		/// </summary>
		Destroy,

		/// <summary>
		/// Recycles the UI gameObject when a view is closed.
		/// </summary>
		Recycle
	}
}
#endif