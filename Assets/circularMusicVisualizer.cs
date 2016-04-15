using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class circularMusicVisualizer : MonoBehaviour {
	public float size = 10f;
	public float amplitude = 1.0f;
	public int cutoffSample = 128;

	public FFTWindow ffWindow;
	public static float[] samples = new float[1024];

	private LineRenderer lineRenderer;
	private float stepSize;

	// Use this for initialization
	void Start () {
		lineRenderer = GetComponent<LineRenderer> ();
		lineRenderer.SetVertexCount (cutoffSample);
		stepSize = size / cutoffSample;
	}

	// Update is called once per frame
	void Update () {
		AudioListener.GetSpectrumData (samples, 0, ffWindow);

		int i = 0;
		float x;
		float y;
		float z = 0f;
		float angle = 180f;

		for (; i < cutoffSample; i++) {
			//Vector3 pos = new Vector3 (i*stepSize - size/2f,samples[i] * amplitude, 0.0f);
			x = Mathf.Sin (Mathf.Deg2Rad * angle) * (samples[i]*amplitude + 5f);
			y = Mathf.Cos (Mathf.Deg2Rad * angle) * (samples[i]*amplitude + 5f);


			lineRenderer.SetPosition (i,new Vector3(x,y,z));
			angle += (360f / cutoffSample);
		}
	}
}
