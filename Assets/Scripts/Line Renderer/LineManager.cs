using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    [SerializeField] private BaseInputManager inputManager;
    [SerializeField] private GameObject lineDrawer;
    private MaskBrushDrawer _maskBrushDrawer;
    private SimpleLineDrawer _simpleLineDrawer;
    private DynamicLineDrawing _dynamicLineDrawing;
    private VFXLineDrawer _vfxLineDrawer;

    private List<LineDrawer> _lineComponents = new List<LineDrawer>();
    // Start is called before the first frame update
    void Start()
    {
        inputManager.ChangeEffect += OnChangeEffect;
        
        _maskBrushDrawer = lineDrawer.GetComponent<MaskBrushDrawer>();
        _simpleLineDrawer = lineDrawer.GetComponent<SimpleLineDrawer>();
        _dynamicLineDrawing = lineDrawer.GetComponent<DynamicLineDrawing>();
        _vfxLineDrawer = lineDrawer.GetComponent<VFXLineDrawer>();
        
        _lineComponents.Add(_maskBrushDrawer);
        _lineComponents.Add(_simpleLineDrawer);
        _lineComponents.Add(_dynamicLineDrawing);
        //_lineComponents.Add(_vfxLineDrawer);
    }

    private void Update()
    {
        EBrushType2D brushType = inputManager.Current2DBrushType;
        switch (brushType)
        {
            case EBrushType2D.LINE:
                var lineBrush = inputManager.Current2DLineBrush;
                switch (lineBrush)
                    {
                        case ELineBrushes2D.MASK:
                            if (!_maskBrushDrawer.enabled)
                            {
                                DisableComponents();
                                _maskBrushDrawer.enabled = true;
                            }
                            break;
                        case ELineBrushes2D.STANDARD:
                            if (!_simpleLineDrawer.enabled)
                            {
                                DisableComponents();
                                _simpleLineDrawer.enabled = true;
                            }
                            break;
                        case ELineBrushes2D.NONE:
                            DisableComponents();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                break;
            case EBrushType2D.DYNAMIC:
                var dynamicBrush = inputManager.Current2DDynamicBrush;
                switch (dynamicBrush)
                    {
                        case EDynamicBrushes2D.SIZE:
                            if (!_dynamicLineDrawing.enabled)
                            {
                                DisableComponents();
                                _dynamicLineDrawing.enabled = true;
                            }
                            break;
                        case EDynamicBrushes2D.NONE:
                            DisableComponents();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                break;
            case EBrushType2D.NONE:
                break;
        }
    }

    private void OnChangeEffect()
    {
        switch (inputManager.CurrentEffect)
        {
            case EEffects.BUBBLES:
                _vfxLineDrawer.enabled = true;
                break;
            default:
                break;
        }
    }

    private void DisableComponents()
    {
        foreach (LineDrawer lineComponent in _lineComponents)
        {
            lineComponent.enabled = false;
        }
    }
}
