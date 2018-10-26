using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


public class Profile : MonoBehaviour {

    public bool useComputeShader = false;
    public ComputeShader shader;

    struct Data
    {
        public float A;
        public float B;
        public float C;
    }

    void Update()
    {
        Data[] inputData = new Data[1000];
        Data[] outputData = new Data[1000];

        for (int i = 0; i < inputData.Length; i++)
        {
            inputData[i].A = i * 3 + 1;
            inputData[i].B = i * 3 + 2;
            inputData[i].C = i * 3 + 3;
        }
        UnityEngine.Debug.Log("Profile Start");
        if(useComputeShader)
        {
            // Data 有3個float，一個 float 有 4 Byte，所以 3 * 4 = 12
            UnityEngine.Profiling.Profiler.BeginSample("Use Computer Shader");
            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            ComputeBuffer inputbuffer = new ComputeBuffer(outputData.Length, 12);
            ComputeBuffer outputbuffer = new ComputeBuffer(outputData.Length, 12);

            int k = shader.FindKernel("CSMain1");

            // 寫入 GPU
            inputbuffer.SetData(inputData);
            shader.SetBuffer(k, "inputData", inputbuffer);

            // 計算，並輸出至 CPU
            shader.SetBuffer(k, "outputData", outputbuffer);
            shader.Dispatch(k, outputData.Length, 1, 1);
            outputbuffer.GetData(outputData);

            // 打印結果
//             for (int i = 0; i < outputData.Length; i++)
//             {
//                 UnityEngine.Debug.Log(outputData[i].A + ", " + outputData[i].B + ", " + outputData[i].C);
//             }

            // 釋放
            inputbuffer.Dispose();
            outputbuffer.Dispose();

            //sw.Stop();
            //TimeSpan ts2 = sw.Elapsed;
            //UnityEngine.Debug.Log(string.Format("使用Compute Shader总共花费{0}ms.", ts2.TotalMilliseconds));
            UnityEngine.Profiling.Profiler.EndSample();
        }
        else
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            UnityEngine.Profiling.Profiler.BeginSample("Do not Use Computer Shader");
            for (int i = 0; i < inputData.Length; i++)
            {
                inputData[i].A = 2 * inputData[i].A;
                inputData[i].B = 2 * inputData[i].B;
                inputData[i].C = 2 * inputData[i].C;
                //UnityEngine.Debug.Log(inputData[i].A + ", " + inputData[i].B + ", " + inputData[i].C);
            }
//             sw.Stop();
//             TimeSpan ts2 = sw.Elapsed;
//             UnityEngine.Debug.Log(string.Format("不使用Compute Shader总共花费{0}ms.",ts2.TotalMilliseconds));
            UnityEngine.Profiling.Profiler.EndSample();
        }
        
    }
}
