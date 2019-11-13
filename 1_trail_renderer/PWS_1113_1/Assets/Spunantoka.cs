﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class Spunantoka : MonoBehaviour
{
	private const int FRAME_MAX = 10;

	private List<Vector3> points0 = new List<Vector3>(); //根元
	private List<Vector3> points1 = new List<Vector3>(); //先端

	private Mesh mesh;

	// Start is called before the first frame update
	void Start()
    {
		mesh = GetComponent<MeshFilter>().mesh;
    }

    // Update is called once per frame
    void Update()
    {
        if(FRAME_MAX <= points0.Count)
		{
			points0.RemoveAt(0);
			points1.RemoveAt(0);
		}

		points0.Add(transform.position * 1.3f);
		points1.Add(transform.TransformPoint(new Vector3(0.0f, 1.0f, 0.0f)));

		if(2 <= points0.Count)
		{
			//CreateMash();
			CreateMashSubDivide(5);
		}
    }

	private void CreateMash()
	{
		mesh.Clear();

		int n = points0.Count;
		Vector3[] vertexArray = new Vector3[2 * n];
		Vector2[] uvArray = new Vector2[2 * n];
		int[] indexArray = new int[6 * (n - 1)];

		int idV = 0, idI = 0;
		float dUV = 1.0f / ((float)n - 1.0f);	

		for(int i = 0; i < n; i++)
		{
			vertexArray[idV + 0] = transform.InverseTransformPoint(points0[i]);
			vertexArray[idV + 1] = transform.InverseTransformPoint(points1[i]);

			uvArray[idV + 0].x =
			uvArray[idV + 1].x = 1.0f - dUV * (float)i;
			uvArray[idV + 0].y = 0.0f;
			uvArray[idV + 1].y = 1.0f;

			if (i == n - 1) break; //最後の辺では帯を作らない
			indexArray[idI + 0] = idV;
			indexArray[idI + 1] = idV + 1;
			indexArray[idI + 2] = idV + 2;
			indexArray[idI + 3] = idV + 2;
			indexArray[idI + 4] = idV + 1;
			indexArray[idI + 5] = idV + 3;

			idV += 2;
			idI += 6;
		}

		mesh.vertices = vertexArray;
		mesh.uv = uvArray;
		mesh.triangles = indexArray;

		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
	}

	private void CreateMashSubDivide(int s)
	{
		mesh.Clear();

		int n = points0.Count;
		Vector3[] vertexArray = new Vector3[2 * s * (n - 1) + 2];
		Vector2[] uvArray =     new Vector2[2 * s * (n - 1) + 2];
		int[] indexArray = new int[6 * s * (n - 1)];

		int idV = 0, idI = 0;
		float dUV = 1.0f / ((float)s * (float)(n - 1.0f));
		int id0 = 0, id1 = 0, id2 = 1, id3 = 2;
		if (n - 1 < id3) id3 = n - 1;

		for (int i = 0; i < n - 1; i++)
		{
			for (int j = 0; j < s; j++)
			{
				float t = (float)j / (float)s;

				Vector3 p0 = CatmullRom(points0[id0], points0[id1], points0[id2], points0[id3], t);
				Vector3 p1 = CatmullRom(points1[id0], points1[id1], points1[id2], points1[id3], t);

				vertexArray[idV + 0] = transform.InverseTransformPoint(p0);
				vertexArray[idV + 1] = transform.InverseTransformPoint(p1);

				uvArray[idV + 0].x =
				uvArray[idV + 1].x = 1.0f - dUV * (float)(i*s+j);
				uvArray[idV + 0].y = 0.0f;
				uvArray[idV + 1].y = 1.0f;

				if (i == n - 1) break; //最後の辺では帯を作らない
				indexArray[idI + 0] = idV;
				indexArray[idI + 1] = idV + 1;
				indexArray[idI + 2] = idV + 2;
				indexArray[idI + 3] = idV + 2;
				indexArray[idI + 4] = idV + 1;
				indexArray[idI + 5] = idV + 3;

				idV += 2;
				idI += 6;
			}
			if (i != 0) id0++;
			if (n - 1 < ++id1) id1 = n - 1;
			if (n - 1 < ++id2) id2 = n - 1;
			if (n - 1 < ++id3) id3 = n - 1;
		}

		vertexArray[idV + 0] = transform.InverseTransformPoint(points0[n - 1]);
		vertexArray[idV + 1] = transform.InverseTransformPoint(points1[n - 1]);

		uvArray[idV + 0].x = uvArray[idV + 1].x = 0.0f;
		uvArray[idV + 0].y = 0.0f;
		uvArray[idV + 1].y = 1.0f;

		mesh.vertices = vertexArray;
		mesh.uv = uvArray;
		mesh.triangles = indexArray;

		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
	}

	private Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
	{
		float t2 = t * t;
		float t3 = t * t2;
		float c0 = -0.5f * t3 + t2 - 0.5f * t;
		float c1 = 1.5f * t3 - 2.5f * t2 + 1.0f;
		float c2 = -1.5f * t3 + 2.0f * t2 + 0.5f * t;
		float c3 = 0.5f * t3 - 0.5f * t2;

		return p0 * c0 + p1 * c1 + p2 * c2 + p3 * c3;
	}
}
