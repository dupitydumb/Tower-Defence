using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Direction direction;
    // 0 South, 1 North, 2 East
    public Sprite[] headSprites;
    public Sprite[] bodySprites;
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Capture input
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");


    }

    void FixedUpdate()
    {
        // Move the player
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        SetPlayerSprites();
    }

    public void SetPlayerSprites()
    {
        SpriteRenderer headSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        SpriteRenderer bodySpriteRenderer = transform.GetChild(1).GetComponent<SpriteRenderer>();

        headSpriteRenderer.flipX = false;
        bodySpriteRenderer.flipX = false;
        switch (GetDirection())
        {
            case Direction.North:
                headSpriteRenderer.sprite = headSprites[1];
                bodySpriteRenderer.sprite = bodySprites[1];
                break;
            case Direction.East:
                headSpriteRenderer.sprite = headSprites[2];
                bodySpriteRenderer.sprite = bodySprites[2];
                break;
            case Direction.South:
                headSpriteRenderer.sprite = headSprites[0];
                bodySpriteRenderer.sprite = bodySprites[0];
                break;
            case Direction.West:
                headSpriteRenderer.sprite = headSprites[2];
                bodySpriteRenderer.sprite = bodySprites[2];
                headSpriteRenderer.flipX = true;
                bodySpriteRenderer.flipX = true;
                break;
        }
    }

    Direction GetDirection()
    {
        if (movement.x > 0)
        {
            direction = Direction.East;
        }
        else if (movement.x < 0)
        {
            direction = Direction.West;
        }
        else if (movement.y > 0)
        {
            direction = Direction.North;
        }
        else if (movement.y < 0)
        {
            direction = Direction.South;
        }
        return direction;
    }
}

public enum Direction
{
    North,
    East,
    South,
    West
}

