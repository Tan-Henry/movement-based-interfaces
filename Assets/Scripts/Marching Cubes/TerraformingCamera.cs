using System.Collections;
using System.Collections.Generic;
using Marching_Cubes;
using UnityEngine;

public class TerraformingCamera : MonoBehaviour
{
    
    public float BrushSize = 2f;
    
    Vector3 _hitPoint;
    Camera _cam;
    
    public WorldGenerator _worldGenerator;
    
    public Vector3 worldPosition;
    
    private void Awake() {
        _cam = GetComponent<Camera>();
    }
    
    private void Terraform(bool add) {
        RaycastHit hit;

        if (
            Physics.Raycast(_cam.ScreenPointToRay(Input.mousePosition), 
                out hit, 10000)
        ) {
            Chunk hitChunk = hit.collider.gameObject.GetComponent<Chunk>();

            _hitPoint = hit.point;

            Debug.Log("hit point: " + _hitPoint);

            hitChunk.EditWeights(_hitPoint, BrushSize, add);  
        }
    }
    
    private void LateUpdate() {
        if (Input.GetMouseButton(0)) {
            Debug.Log("left click");
            Terraform(true);
        }
        else if (Input.GetMouseButton(1)) {
            Debug.Log("right click");
            Terraform(false);
        }
        
        worldPosition = transform.position;
        _worldGenerator.hideChunks(worldPosition);
    }
    
    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_hitPoint, BrushSize);
    }
}
