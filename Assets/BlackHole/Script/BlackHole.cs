using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour {

    public GameObject[] goes;
    private List<Material> matList = new List<Material>();
	// Use this for initialization
	void Start () {
        if (goes.Length > 0)
        {
            foreach (var go in goes)
            {
                var rds = go.GetComponentsInChildren<Renderer>();
                foreach(var rd in rds)
                {
                    var mat = rd.sharedMaterial;
                    if(mat)
                    {
                        //mat.SetVector("_BlackHolePos", gameObject.transform.position);
                        matList.Add(mat);
                        Debug.LogWarning("Set BlackHolePosition");
                    }
                    
                }
            }
        }
		
	}
	
	// Update is called once per frame
	void Update () {
        if (matList.Count > 0)
        {
            foreach (var mat in matList)
            {
                mat.SetVector("_BlackHolePos", gameObject.transform.position);
            }
        }
		
	}
}
