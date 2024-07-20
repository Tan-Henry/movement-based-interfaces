using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSensorServer : MonoBehaviour
{
    public abstract event Action ShakeLeft;
    public abstract event Action ShakeRight;
    public abstract event Action ShakeBoth;
}
