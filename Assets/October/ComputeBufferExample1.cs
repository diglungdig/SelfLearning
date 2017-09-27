using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputeBufferExample1 : MonoBehaviour {


    public int Count = 100000;
    public Mesh instanceMesh;
    public Material instanceMaterial;

    private int cachedInstanceCount = 1;
    private ComputeBuffer positionBuffer;
    private ComputeBuffer argsBuffer;

    private uint[] args = new uint[5];

	// Use this for initialization
	void Start () {
        argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
        UpdateBuffers();
	}
	
    void UpdateBuffers()
    {
        //positions
        if (positionBuffer != null)
            positionBuffer.Release();
        positionBuffer = new ComputeBuffer(Count, 16);
        Vector4[] positions = new Vector4[Count];
        for (int i = 0; i < Count; i++)
        {
            float angle = Random.Range(0f, Mathf.PI * 2.0f);
            float distance = Random.Range(20f, 100f);
            float height = Random.Range(-2.0f, 2.0f);
            float size = Random.Range(0.0f, 0.25f);
            positions[i] = new Vector4(Mathf.Sin(angle) * distance, height, Mathf.Cos(angle) * distance, size);
        }
        positionBuffer.SetData(positions);
        instanceMaterial.SetBuffer("positionBuffer", positionBuffer);

        //indirect args
        uint numIndices = (instanceMesh != null) ? (uint)instanceMesh.GetIndexCount(0) : 0;
        args[0] = numIndices;
        args[1] = (uint)Count;
        argsBuffer.SetData(args);

        cachedInstanceCount = Count;
    }


    private void OnDisable()
    {
        if(positionBuffer != null)
        {
            positionBuffer.Release();
        }
        positionBuffer = null;

        if(argsBuffer != null)
        {
            argsBuffer.Release();
        }
        argsBuffer = null;
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(265, 25, 200, 30), "iNSTANCE cOUNT: " + Count.ToString());
        Count = (int)GUI.HorizontalSlider(new Rect(25, 20, 200, 30), (float)Count, 1f, 5000000f);
;    }

    // Update is called once per frame
    void Update () {
        if (cachedInstanceCount != Count)
            UpdateBuffers();

        //Render with Graphics.DrawMeshInstancedIndirect
        Graphics.DrawMeshInstancedIndirect(instanceMesh, 0, instanceMaterial, new Bounds(Vector3.zero, new Vector3(100f, 100f, 100f)), argsBuffer);

	}
}
