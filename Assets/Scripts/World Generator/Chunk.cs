using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public Vector2Int position;
    private Transform player;
    public float disableDistance = 70.0f;
    public float enableDistance = 100.0f;

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
}
