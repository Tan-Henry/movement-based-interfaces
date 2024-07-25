using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    [SerializeField] private BaseInputManager inputManager;
    private MaskBrushDrawer _maskBrushDrawer;
    private SimpleLineDrawer _simpleLineDrawer;
    private DynamicLineDrawing _dynamicLineDrawing;
    private VFXLineDrawer _vfxLineDrawer;
    private TemporaryLineBrush _temporaryLineBrush;

    private List<LineDrawer> _lineComponents = new List<LineDrawer>();
    // Start is called before the first frame update
    void Start()
    {
        _maskBrushDrawer = GetComponent<MaskBrushDrawer>();
        _simpleLineDrawer = GetComponent<SimpleLineDrawer>();
        _dynamicLineDrawing = GetComponent<DynamicLineDrawing>();
        _vfxLineDrawer = GetComponent<VFXLineDrawer>();
        _temporaryLineBrush = GetComponent<TemporaryLineBrush>();
        
        _lineComponents.Add(_maskBrushDrawer);
        _lineComponents.Add(_simpleLineDrawer);
        _lineComponents.Add(_dynamicLineDrawing);
        _lineComponents.Add(_vfxLineDrawer);
        _lineComponents.Add(_temporaryLineBrush);
    }

    private void Update()
    {
        EMode mode = inputManager.CurrentMode;
        switch (mode)
        {
            case EMode.CREATE:
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
                break;
            case EMode.PRESENT:
                switch (inputManager.CurrentEffect)
                {
                    case EEffects.BUBBLES:
                        _vfxLineDrawer.enabled = true;
                        _temporaryLineBrush.enabled = false;
                        Debug.Log("Bubbles");
                        break;
                    case EEffects.COOL:
                        _temporaryLineBrush.enabled = true;
                        _vfxLineDrawer.enabled = false;
                        break;
                    default:
                        _vfxLineDrawer.enabled = false;
                        break;
                }
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
