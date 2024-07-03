using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInputManager : MonoBehaviour
{
    //draw-action right hand
    public abstract bool RightHandIsDrawing { get; protected set; }
    public abstract Vector3 RightHandDrawPosition { get; protected set; }
    
    //draw-action left hand
    public abstract bool LeftHandIsDrawing { get; protected set; }
    public abstract Vector3 LeftHandDrawPosition { get; protected set; }
}
