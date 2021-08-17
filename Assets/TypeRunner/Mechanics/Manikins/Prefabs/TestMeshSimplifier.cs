using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMeshSimplifier : MonoBehaviour
{
	void OnGUI()
	{
		if (GUI.Button(Rect.MinMaxRect(0, 0, 200, 200),"Simplify Mesh"))
		{
			var originalMesh = GetComponent<MeshFilter>().sharedMesh;
			float quality = 0.2f;
			var meshSimplifier = new UnityMeshSimplifier.MeshSimplifier();
			meshSimplifier.Initialize(originalMesh);
			meshSimplifier.SimplifyMesh(quality);
			var destMesh = meshSimplifier.ToMesh();
			GetComponent<MeshFilter>().sharedMesh = destMesh;
		}
	}
}
