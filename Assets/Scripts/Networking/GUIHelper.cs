using System;
using UnityEngine;

public static class GUIHelper
{
	public static void RenderInsideArea(Action renderAreaContent)
	{
		GUILayout.BeginArea(new Rect((Screen.width - 400) / 2, (Screen.height - 300) / 2, 400, 300));

		renderAreaContent();

		GUILayout.EndArea();
	}
}