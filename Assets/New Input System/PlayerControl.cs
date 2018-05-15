using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputNew;

public class PlayerControl : MonoBehaviour {

    public PlayerInput playerInput;


    public ButtonAction jump;
	// Use this for initialization
	void Start () {
        //StartCoroutine(Assign());
        //newactionmap = playerInput.GetActions<NewActionMap>();
        jump.Bind(playerInput.handle);
    }


    IEnumerator Assign()
    {
        yield return new WaitForSeconds(2f);
    }

    // Update is called once per frame
    void Update () {


            if (jump.control.isHeld)
            {
                Debug.Log("Space is pressed: Jump!");
            }
        
	}
}
