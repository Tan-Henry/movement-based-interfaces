using UnityEngine;

public class SparkleBrush : MonoBehaviour
{
    public ParticleSystem sparkleParticleSystem;
    private ParticleSystem.EmissionModule emissionModule;
    private ParticleSystem.ShapeModule shapeModule;

    void Start()
    {
        emissionModule = sparkleParticleSystem.emission;
        shapeModule = sparkleParticleSystem.shape;

        // Adjust shape module to fit the brush
        shapeModule.shapeType = ParticleSystemShapeType.Sphere;
        shapeModule.radius = 0.1f; // Adjust the radius as needed
    }

    void Update()
    {
        if (Input.GetMouseButton(0)) // Left mouse button or touch
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10; // Distance from the camera
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = worldPosition;

            // Emit particles while moving
            emissionModule.rateOverDistance = 10;
        }
        else
        {
            // Stop emitting particles
            emissionModule.rateOverDistance = 0;
        }
    }
}
