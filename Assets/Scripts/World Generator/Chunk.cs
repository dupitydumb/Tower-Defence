using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public Vector2Int position;
    private Transform player;
    public float disableDistance = 70.0f;
    public float enableDistance = 100.0f;
    public bool isActive = false;
    public Dictionary<Vector3Int, GameObject> objects = new Dictionary<Vector3Int, GameObject>();
    [SerializeField]
    private float distanceToPlayer;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnActivate()
    {
        //if objects gameobject position is not the same as the key in the dictionary, move the object to the key position
        foreach (KeyValuePair<Vector3Int, GameObject> obj in objects)
        {
            if (obj.Value.transform.position != obj.Key)
            {
                Debug.LogWarning("Moving object to correct position");
                obj.Value.transform.position = obj.Key;
            }
        }
    }
}
