using System.Collections.Generic;
using UnityEngine;

public abstract class LineDrawer : MonoBehaviour
{
    protected List<Vector3> linePoints;
    protected float timer;
    public float timerDelay;

    protected GameObject newLine;
    protected LineRenderer drawLine;
    public float lineWidth;
    
    public OVRHand rightHand;
    public OVRSkeleton skeleton;
    private bool isIndexFingerPinching;
    private Transform handIndexTipTransform;
    private bool isDrawing;

    protected virtual void Start()
    {
        linePoints = new List<Vector3>();
        timer = timerDelay;
    }

    protected virtual void Update()
    {
        //ToDo: Handtracking in anderer Klasse implementieren
        if (rightHand.IsTracked)
        {
            // Gather info whether left hand is pinching
            isIndexFingerPinching = rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index);

            // Proceed only if left hand is pinching
            if (isIndexFingerPinching)
            {
                if (!isDrawing)
                {
                    isDrawing = true;
                    InitializeLine();
                }
                // Loop through all the bones in the skeleton
                foreach (var b in skeleton.Bones)
                {
                    // If bone is the the hand index tip
                    if (b.Id == OVRSkeleton.BoneId.Hand_IndexTip)
                    {
                        // Store its transform and break the loop
                        handIndexTipTransform = b.Transform;
                        break;
                    }
                }
                linePoints.Add(handIndexTipTransform.position);
                drawLine.positionCount = linePoints.Count;
                drawLine.SetPositions(linePoints.ToArray());
                
            }
            // If the user is not pinching
            else
            {
                if (isDrawing)
                {
                    OnLineComplete();
                    linePoints.Clear();
                    isDrawing = false;
                }
            }
        }
        /*if (Input.GetMouseButtonDown(0))
        {
            InitializeLine();
        }*/
    }

    protected virtual void InitializeLine()
    {
        newLine = new GameObject();
        drawLine = newLine.AddComponent<LineRenderer>();
        drawLine.material = new Material(Shader.Find("Sprites/Default"));
        drawLine.startWidth = lineWidth;
        drawLine.endWidth = lineWidth;
        drawLine.startColor = Color.clear;
        drawLine.endColor = Color.clear;
    }

    protected virtual void OnLineComplete() { }
}