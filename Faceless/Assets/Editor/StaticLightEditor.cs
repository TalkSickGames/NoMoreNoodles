using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(StaticLight))]
public class StaticLightEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		
		StaticLight myScript = (StaticLight)target;
		if(GUILayout.Button("Bake Light"))
		{
			myScript.Bake();
		}
	}
}