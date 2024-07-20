using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleUsage : MonoBehaviour
{
    // I created this script to give you an idea of how to use the input manager and how to incoporate it
    // into your scripts. If you have any questions, feel free to ask Susi or me (Paul).

    // Make sure to get a reference to the BaseInputManager and not the specific implementation of it. This makes sure
    // that we can easily change the input manager implementation without changing the scripts that use it.
    [SerializeField] private BaseInputManager inputManager;

    void Start()
    {
        // This is a trigger event, which means it doesn't have any parameters it just makes you aware that something 
        // is happening. The input manager should make sure that the event is only triggered when the conditions are met.
        // For example this ChangeEffect event should only be triggered when the user is doing the gesture and the app
        // is currently in the correct performance mode.
        inputManager.ChangeEffect += OnChangeEffect;
    }

    // This is the method that gets called when the ChangeEffect event is triggered.
    private void OnChangeEffect()
    {
        // Do something when the ChangeEffect event is triggered. For example make use of the state that is being held
        // by the input manager. In this case the available effects and the currently selected effect. Then use these to 
        // change the selected effect. In this case we just take the next one.

        // Get the current effect
        EEffects currentEffect = inputManager.CurrentEffect;

        // Get the available effects
        List<EEffects> availableEffects = inputManager.AvailableEffects;

        // Set the next effect 
        inputManager.CurrentEffect =
            availableEffects[(availableEffects.IndexOf(currentEffect) + 1) % availableEffects.Count];

        // ideally you never have to worry about the implementation of the input method and whether or not you are allowed
        // to perform the action of an event. The input manager should take care of that. But still make sure to check if
        // if the input manager is checking the conditions correctly or if it missed something.
    }

    private void Update()
    {
        // The other type of event is a continuous event. This event is triggered every frame as long as the conditions are met.
        // For example the drawing event is a boolean that is true when the user is currently drawing. These events need
        // to be checked every frame to make sure that the action is performed as long as the conditions are met.

        // Check if the user is currently drawing a 2D line
        if (inputManager.RightHandIsDrawing2D)
        {
            // Then if the conditions are met, which the input manager figures out for you, you have to gather the information
            // that you need to perform the action. In this case the position we should be drawing and the brush that should be used.

            float brushSize = inputManager.Current2DBrushSettings.brushSize;
            float opacity = inputManager.Current2DBrushSettings.opacity;
            Vector3 position = inputManager.RightHandPosition;

            // We make heavy use of enums to make the selection and availability of options very verbose and easy to understand.
            // This means we also have to check some things to figure out which brush is active. I am not sure this is the 
            // best option yet. We might have to change the granularity of the enums to make it easier to use.
            EBrushType2D brushType = inputManager.Current2DBrushType;

            // Based on one item of the state, so the selected brush type, we can then select the correct brush to use.
            switch (brushType)
            {
                case EBrushType2D.NONE:
                    break;
                case EBrushType2D.LINE:
                    var lineBrush = inputManager.Current2DLineBrush;
                    // the resulting brush is also an enum, so here would be the place to select the correct brush from 
                    // your script.
                    break;
                case EBrushType2D.DYNAMIC:
                    var dynamicBrush = inputManager.Current2DDynamicBrush;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Then we can use the gathered information to perform the action. In this case we would draw a line at the position
            // of the right hand with the selected brush and the selected settings.

            // Draw the line
        }
    }
}