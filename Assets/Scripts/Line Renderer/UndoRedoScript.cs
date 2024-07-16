using System.Collections.Generic;
using UnityEngine;

public class UndoRedoScript : MonoBehaviour
{
    private List<GameObject> _lines = new List<GameObject>();
    private Stack<GameObject> _undoStack = new Stack<GameObject>();
    private Stack<GameObject> _redoStack = new Stack<GameObject>();
    private int _amountOfSkips = 5; // Number of actions to keep track of

    public void AddLastLineGameObject(GameObject lineObject)
    {
        if (_lines.Count >= _amountOfSkips)
        {
            _lines.RemoveAt(0);
        }
        _lines.Add(lineObject);

        // Clear the redo stack since we are adding a new action
        _redoStack.Clear();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) // Undo
        {
            Undo();
        }
        if (Input.GetKeyDown(KeyCode.L)) // Redo
        {
            Redo();
        }
    }

    private void Undo()
    {
        if (_lines.Count > 0)
        {
            GameObject lastLine = _lines[_lines.Count - 1];
            if (lastLine.activeSelf)
            {
                lastLine.SetActive(false);
                _undoStack.Push(lastLine);
                _lines.RemoveAt(_lines.Count - 1);
            }
        }
    }

    private void Redo()
    {
        if (_undoStack.Count > 0)
        {
            GameObject lineToRedo = _undoStack.Pop();
            lineToRedo.SetActive(true);
            _redoStack.Push(lineToRedo);
            _lines.Add(lineToRedo);
        }
    }
}