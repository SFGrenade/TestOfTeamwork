using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public static class PolygonTempEditor
{
	[MenuItem("CONTEXT/PolygonCollider2D/*0.64...")]
	public static void Times064 (MenuCommand menuCommand) {
		PolygonCollider2D mf = menuCommand.context as PolygonCollider2D;
		for (int p = 0; p < mf.pathCount; p++)
		{
			var path = mf.GetPath(p);
			for (int i = 0; i < path.Length; i++)
			{
				path[i].x = path[i].x / 0.64f;
				path[i].y = path[i].y / 0.64f;
			}
			mf.SetPath(p, path);
		}
	}
}
