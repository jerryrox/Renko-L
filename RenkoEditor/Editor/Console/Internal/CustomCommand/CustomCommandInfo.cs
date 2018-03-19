using System;
using UnityEngine;

namespace RenkoEditor.Console.Internal
{
	public class CustomCommandInfo {

		public string CommandName;
		public string ActionName;
		public string[] Arguments;


		public CustomCommandInfo(string[] inputs) {
			CommandName = inputs[0];

			if(inputs.Length > 1)
				ActionName = inputs[1];
			
			int paramCount = Mathf.Max(0, inputs.Length - 2);
			Arguments = new string[paramCount];
			for(int i=0; i<paramCount; i++)
				Arguments[i] = inputs[i+2];
		}
	}
}

