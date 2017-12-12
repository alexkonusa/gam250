using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[ExecuteInEditMode]
public class ActionWindow : EditorWindow 
{

	IRC irc;

	string newAction;
	object Button;

	[MenuItem("Window/IRCActions")]
	public static void ShowWindow()
	{

		EditorWindow.GetWindow (typeof(ActionWindow));
		
	}

	void OnGUI()
	{
		if (GameObject.Find ("IRCManager") != null) {
			if (irc == null)
				irc = GameObject.Find ("IRCManager").GetComponent<IRC> ();

			EditorGUILayout.LabelField ("[User Commands]");

			EditorGUILayout.BeginVertical ("box");
			foreach (string s in irc.actions) {
				EditorGUILayout.LabelField (s);
			}

			EditorGUILayout.EndVertical ();

			newAction = EditorGUILayout.TextField ("Action", newAction);

			if (GUILayout.Button ("ADD ACTION")) {
				if (newAction != string.Empty) {
					irc.AddAction (newAction);
					newAction = string.Empty;
				}

			}

			if (GUILayout.Button ("REMOVE ACTION")) {
				if (newAction != string.Empty) {

					irc.actions.Remove (newAction);
					newAction = string.Empty;
				}

			}
		}
		
	}
}
