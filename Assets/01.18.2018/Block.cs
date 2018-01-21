using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

    public event System.Action<Block> OnBlockPressed;
    public event System.Action OnFinishMoving;



    public Vector2Int coord;
    Vector2Int startingCoord;


    private void OnMouseDown()
    {
        if(OnBlockPressed != null)
        {
            OnBlockPressed(this);
        }
    }


    public void MoveToPosition(Vector2 target, float duration)
    {
        StartCoroutine(AnimateMove(target, duration));
    }


    public void Init(Vector2Int startingcoord, Texture2D image)
    {
        coord = startingcoord;
        this.startingCoord = startingcoord;
        GetComponent<MeshRenderer>().material = Resources.Load<Material>("Block");
        GetComponent<MeshRenderer>().material.mainTexture = image;
    }



    IEnumerator AnimateMove(Vector2 target, float duration)
    {
        Vector2 initialPos = transform.position;

        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime / duration;
            transform.position = Vector2.Lerp(initialPos, target, percent);
            yield return null;
        }

        if(OnFinishMoving != null)
        {
            OnFinishMoving();
        }
    }


    public bool IsAtStartingCoord()
    {
        if (coord == startingCoord)
            return true;
        else
            return false;
       
    }
}
