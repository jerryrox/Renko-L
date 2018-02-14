#if NGUI
using System;

namespace Renko.Extensions
{
	public static class NGUIExtensionUILabel {
		
		/// <summary>
		/// Sets the label's text that ends with '...' if label's bounds can't hold all the specified text's content.
		/// Only works with clamped text mode.
		/// </summary>
		public static void SetIncompleteText(this UILabel context, string text) {
			context.text = text;

			if(context.processedText.Length >= text.Length)
				return;

			context.text = context.processedText.Substring(0, context.processedText.LastIndexOf(' '));
			context.text += " ...";
		}
	}
}
#endif