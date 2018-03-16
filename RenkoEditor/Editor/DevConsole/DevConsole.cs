/*
Features TODO:

Should only work in Editor.

Editor Window
	Output area
		Formats
			> INPUT_DATA_HERE
			: OUTPUT_DATA_HERE
	Suggestion area
		Display auto-completion when period is pressed.
			Use reflection to display public methods, fields, or properties.
	Command line area
		Store session variables
			Variable format: $VAR_NAME
			Use Dictionary to store them

Editor Settings Window
	User-definables
		Max variable count
		Multi-threading

Save states using ScriptableObject with AssetDatabase trick

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RenkoEditor
{
	public static class DevConsole {
		
	}
}