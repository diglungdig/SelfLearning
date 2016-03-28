using UnityEngine;
using System.Collections;

public class circulingObject : MonoBehaviour {

    public GameObject timeRobot;
    public GameObject player;
	// Update is called once per frame
	void Update () {
        transform.position = player.transform.position;
        timeRobot.transform.RotateAround(transform.position,Vector3.up, 120 * Time.deltaTime);
        timeRobot.transform.Rotate(0f, 80f * Time.deltaTime, 0f, Space.Self);
    }
}
