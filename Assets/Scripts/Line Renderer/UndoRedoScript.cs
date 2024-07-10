using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UndoRedoScript : MonoBehaviour
{
    private List<GameObject> _lines = new List<GameObject>();
    private int _amountOfSkips = 3; //Wie viele Aktionen man zurueck/vor gehen kann

    public void AddLastLineGameObject(GameObject lineObject)
    {
        //Wenn es noch nicht genug Aktionen gab, werden keine Lines geloescht
        if (!(_lines.Count < _amountOfSkips))
        {
            _lines.RemoveAt(0);
            _lines.Add(lineObject);
        }
        else
        {
            _lines.Add(lineObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            for (int i = _lines.Count - 1; i >= 0; i--)
            {
                if (_lines[i].activeSelf)
                {
                    _lines[i].SetActive(false);
                    break;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            for (int i = 0; i <= _lines.Count - 1; i++)
            {
                if (!_lines[i].activeSelf)
                {
                    _lines[i].SetActive(true);
                    break;
                }
            }
        }
    }
}
