using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    public Transform player;
    public float disableDistance = 70.0f;
    public float enableDistance = 100.0f;
    public float destroyDistance = 200.0f;

    public List<Chunk> chunks = new List<Chunk>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Chunk chunk in chunks)
        {
            float distance = Vector2.Distance(player.position, chunk.transform.position);
            if (distance > disableDistance)
            {
                chunk.gameObject.SetActive(false);
            }
            else if (distance < enableDistance)
            {
                chunk.gameObject.SetActive(true);
            }
            if (distance > destroyDistance)
            {
                RemoveChunk(chunk);
                Destroy(chunk.gameObject);
            }
        }
    }

    public void AddChunk(Chunk chunk)
    {
        if (!chunks.Contains(chunk))
        {
            chunks.Add(chunk);
        }
    }

    public void RemoveChunk(Chunk chunk)
    {
        if (chunks.Contains(chunk))
        {
            chunks.Remove(chunk);
        }
    }
}
