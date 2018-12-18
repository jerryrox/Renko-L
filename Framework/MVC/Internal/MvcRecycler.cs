#if NGUI
using System;
using System.Collections.Generic;

namespace Renko.MVCFramework.Internal
{
	public class MvcRecycler {

		private Stack<IMvcView> views;
		private Func<IMvcView> createView;


		/// <summary>
		/// Returns the number of reusable objects currently stored.
		/// </summary>
		public int Count { get { return views.Count; } }


		public MvcRecycler(Func<IMvcView> createView)
		{
			views = new Stack<IMvcView>();
			this.createView = createView;
		}

		/// <summary>
		/// Adds the specified view to the recycling pool.
		/// </summary>
		public void ReturnView(IMvcView view)
		{
			view.ViewObject.SetActive(false);
			views.Push(view);
		}

		/// <summary>
		/// Returns a reusable object if exists. If not, a new view will be created and returned.
		/// </summary>
		public IMvcView GetView()
		{
			if(views.Count == 0)
				return createView();
			return views.Pop();
		}
	}
}
#endif