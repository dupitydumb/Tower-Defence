using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public float roamSpeed = 10.0f;
    public float scrollSpeed = 2.0f;
    public float minSize = 3.0f;
    public float maxSize = 15.0f;

    // Update is called once per frame
    void LateUpdate()
    {
        if (isMovingToEnemySpawn)
        {
            return;
        }
        if (GameManager.instance.gameState == GameState.Night)
        {
            FreeRoamCamera();
            return;
        }
        // Desired position of the camera
        Vector3 desiredPosition = player.position + offset;
        // Smoothly interpolate between the current position and the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        // Update the camera's position
        transform.position = smoothedPosition;
    }

    void FreeRoamCamera()
    {
        Vector3 pos = transform.position;

        // Panning with WASD keys
        if (Input.GetKey("w"))
        {
            pos.y += roamSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s"))
        {
            pos.y -= roamSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d"))
        {
            pos.x += roamSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a"))
        {
            pos.x -= roamSpeed * Time.deltaTime;
        }

        //if middle mouse button is pressed
        if (Input.GetMouseButton(2))
        {
            //Get the direction of the mouse movement
            Vector3 move = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0);
            //Move the camera
            pos -= move * roamSpeed * Time.deltaTime;
        }

        // Zooming with mouse scroll wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Camera.main.orthographicSize -= scroll * scrollSpeed;
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minSize, maxSize);
        transform.position = pos;
    }
    bool isMovingToEnemySpawn = false;
    public void MoveToEnemySpawn()
    {
        // Move the camera to the enemy spawn point
        Vector3 desiredPosition = GameManager.instance.enemySpawner.spawnPoint.transform.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}