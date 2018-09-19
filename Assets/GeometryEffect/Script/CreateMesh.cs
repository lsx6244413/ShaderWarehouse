using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMesh : MonoBehaviour {

	// Use this for initialization
    public int vertexCount = 1000;
    public MeshTopology topology;
    public Material mat;
	void Start () {
		
         Vector3[] vertices = new Vector3[vertexCount];
 
         int[] indices = new int[vertexCount];
 
         for(int i = 0; i < vertexCount; i++)
         {
             vertices[i] = new Vector3(Random.value, Random.value, Random.value);
             indices[i] = i;
         }
 
         Mesh m = new Mesh();
         m.vertices = vertices;
         m.SetIndices(indices, topology, 0);
         m.RecalculateBounds();
 
         MeshFilter mf = GetComponent<MeshFilter>();
         MeshRenderer mr =  GetComponent<MeshRenderer>();
         mr.sharedMaterial = mat;
         mf.mesh = m;
         Debug.Log(m.bounds);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
