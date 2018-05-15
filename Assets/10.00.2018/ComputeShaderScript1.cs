using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ComputeShaderScript1 : MonoBehaviour {



    public ComputeShader shader;



    private void Start()
    {
        RunShader();
    }
    void RunShader()
    {
        //Section 2.0
        VecMatPair[] data = new VecMatPair[5];
        VecMatPair[] output = new VecMatPair[5];

        ComputeBuffer buffer = new ComputeBuffer(data.Length, 76);
        int kernel = shader.FindKernel("CSMain");

        shader.SetBuffer(kernel, "dataBuffer", buffer);
        shader.Dispatch(kernel, data.Length, 1, 1);
        buffer.GetData(output);


        for (int i = 0; i < output.Length; i++)
        {
            Debug.Log(output[i].point);
            Debug.Log(output[i].matrix);


        }


        buffer.Dispose();
        //Section 1.0
        /*
        int kernelHandle = shader.FindKernel("CSMain");

        RenderTexture tex = new RenderTexture(256, 256, 24);
        tex.enableRandomWrite = true;
        tex.Create();

        //Where latency comes into place
        shader.SetTexture(kernelHandle, "Result", tex);
        shader.Dispatch(kernelHandle, 256/8, 256/8, 1);
        */
    }

    struct VecMatPair
    {
        public Vector3 point;
        public Matrix4x4 matrix;
    }
}
