using UnityEngine;
using System.Collections;

public class itweenTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        iTween.MoveTo(gameObject,iTween.Hash("y",1,"x",-3, "looptype", iTween.LoopType.pingPong, "delay", 1));
        iTween.ScaleTo(gameObject,iTween.Hash("x",2, "looptype", iTween.LoopType.pingPong));
	}
	
	// Update is called once per frame
	void Update () {
    }
}
