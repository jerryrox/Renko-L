using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RenkoEditor.Console.Internal
{
	public class ConsoleView : EditorWindow {

		private Vector2 outputScroll;
		private string curCommand;


		[MenuItem("Renko-L/Window/DevConsole")]
		public static void Initialize() {
			ConsoleView window = (ConsoleView)EditorWindow.GetWindow(typeof(ConsoleView), false, "DevConsole");
			window.Show();
		}

		void OnGUI() {
			RenderTitleArea();
			RenderOutputArea();
			RenderInputArea();
		}

		void RenderTitleArea() {
			EditorGUILayout.BeginVertical(GUILayout.Height(60));
			EditorGUILayout.LabelField(
				"DevConsole", new GUIStyle() {
					fontSize = 20,
					padding = new RectOffset(
						(int)(position.width*0.5f - 55),
						0,
						20,
						0
					)
				}
			);
			EditorGUILayout.EndVertical();
		}

		void RenderOutputArea() {
			outputScroll = EditorGUILayout.BeginScrollView(outputScroll);
			Rect rect = EditorGUILayout.BeginVertical(GUILayout.Height(position.height - 120));
			int outputCount = OutputHistory.Count;
			if(outputCount == 0) {
				EditorGUILayout.LabelField("");
			}
			else {
				GUIStyle textStyle = new GUIStyle();
				textStyle.normal.textColor = Color.white;
				textStyle.wordWrap = true;

				for(int i=0; i<outputCount; i++) {
					var curOutputInfo = OutputHistory.GetOutput(i);
					EditorGUILayout.LabelField(curOutputInfo.Text, textStyle);
				}
			}
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndScrollView();
		}

		void RenderInputArea() {
			EditorGUILayout.BeginVertical();
			curCommand = EditorGUILayout.TextArea(curCommand, GUILayout.ExpandHeight(true), GUILayout.MinHeight(30));
			EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button("Evaluate")) {
				OnEvaluateButton();
			}
			if(GUILayout.Button("Clear")) {
				OutputHistory.Clear();
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndHorizontal();
		}

		void OnEvaluateButton() {
			if(string.IsNullOrEmpty(curCommand)) {
				return;
			}

			DevConsole.Evaluate(curCommand, null);

			ClearCommandInput();
			ScrollToBottom();
		}

		void ScrollToBottom() {
			outputScroll.y = 99999999;
		}

		void ClearCommandInput() {
			curCommand = string.Empty;
		}
	}
}