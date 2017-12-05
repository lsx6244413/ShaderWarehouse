using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMgr : MonoBehaviour {

    public float speed = 1.0f;
	// Use this for initialization
	
	// Update is called once per frame
	void Update () {
            gameObject.transform.Rotate(new Vector3(0, Time.deltaTime * speed * 10, 0));
	}
}
