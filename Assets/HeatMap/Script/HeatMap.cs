using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeatMap : MonoBehaviour {

    [SerializeField] private int countPoint;
   [SerializeField] private Material heatMat;

    [SerializeField]
    private Vector4[] pointPoses;
    [SerializeField]
    private Vector4[] pointProper;

    
    private void Awake()
    {
        heatMat.SetFloatArray("_Points", new float[countPoint]);
    }
    // Use this for initialization
    void Start () {

        pointPoses = new Vector4[countPoint];
        pointProper = new Vector4[countPoint];

        for (int i = 0; i < pointPoses.Length; i++)
        {
            pointPoses[i] = new Vector2(Random.Range(-0.9f, 0.9f), Random.Range(-0.5f, 0.5f));
            pointProper[i] = new Vector2(Random.Range(0f, 0.25f), Random.Range(-0.25f, 1f)); // (Radius, Intensities)
        }

        heatMat.SetVectorArray("_Properties", pointProper);
        heatMat.SetInt("_Points_Length", countPoint);
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < countPoint; i++)
        {
            pointPoses[i] += new Vector4(Random.Range(-0.1f, +0.1f), Random.Range(-0.1f, +0.1f), 0f, 0f) * Time.deltaTime;
        }
        heatMat.SetVectorArray("_Points", pointPoses);
       // heatMat.SetVectorArray("_Properties", pointProper);
    }
}
