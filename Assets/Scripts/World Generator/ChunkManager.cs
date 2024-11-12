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
            if (Vector2.Distance(player.position, chunk.transform.position) > destroyDistance)
            {
                
            }
            else if (Vector2.Distance(player.position, chunk.transform.position) > disableDistance && chunk.isActive)
            {
                chunk.isActive = false;
                //Deactivate children
                for (int i = 0; i < chunk.transform.childCount; i++)
                {
                    chunk.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
            else if (Vector2.Distance(player.position, chunk.transform.position) < enableDistance && !chunk.isActive)
            {
                chunk.isActive = true;
                //Activate children
                for (int i = 0; i < chunk.transform.childCount; i++)
                {
                    chunk.transform.GetChild(i).gameObject.SetActive(true);
                }
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
