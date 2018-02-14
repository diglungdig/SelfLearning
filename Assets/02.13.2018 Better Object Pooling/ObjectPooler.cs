using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {

    public static ObjectPooler instance;

    private void Awake()
    {
        instance = this;
    }

    private void OnDestroy()
    {
        instance = null;
    }
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

	// Use this for initialization
	void Start () {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
	}


    public GameObject SpawnFromPool(string tag, Vector3 pos, Quaternion rot)
    {
        if (poolDictionary.ContainsKey(tag))
        {
            Debug.Log("This is wrong no key");
            return null;
        }
        GameObject newOne = poolDictionary[tag].Dequeue();

        newOne.SetActive(true);
        newOne.transform.position = pos;
        newOne.transform.rotation = rot;


        poolDictionary[tag].Enqueue(newOne);

        return newOne;
    }

}
