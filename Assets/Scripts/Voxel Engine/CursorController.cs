using System.Collections;
using System.Collections.Generic;
using Marching_Cubes;
using UnityEngine;
using Voxel_Engine;

public class CursorController : MonoBehaviour
{
    public float speed = 1.0f;
    public int brushType = 0; 
    
    public bool isPainting = false;
    public WorldGenerator WorldGenerator;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            // move the cursor up
            transform.position += (new Vector3(0, 1, 0) * (Time.deltaTime * speed));
        }

        if (Input.GetKey(KeyCode.Q))
        {
            // move the cursor down
            transform.position += new Vector3(0, -1, 0) * (Time.deltaTime * speed);
        }

        if (Input.GetKey(KeyCode.A))
        {
            // move the cursor left
            transform.position += new Vector3(-1, 0, 0) * (Time.deltaTime * speed);
        }

        if (Input.GetKey(KeyCode.D))
        {
            // move the cursor right
            transform.position += new Vector3(1, 0, 0) * (Time.deltaTime * speed);
        }

        if (Input.GetKey(KeyCode.W))
        {
            // move the cursor forward
            transform.position += new Vector3(0, 0, 1) * (Time.deltaTime * speed);
        }

        if (Input.GetKey(KeyCode.S))
        {
            // move the cursor back
            transform.position += new Vector3(0, 0, -1) * (Time.deltaTime * speed);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isPainting = !isPainting;
        }

        // deactivate the voxel at the cursor position and the surrounding voxels
        var worldPosition = transform.position;

        ChangeVoxelAt(worldPosition);
    }

    private void ChangeVoxelAt(Vector3 worldPosition)
    {
        WorldGenerator.TerraformAtPoint(worldPosition, 3, isPainting, brushType);
    }
}