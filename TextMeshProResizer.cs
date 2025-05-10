using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Dynamically adjusts the font size of multiple TextMeshPro labels based on the one with the longest text.
/// Only the longest label uses AutoSizing; the others adopt its font size.
/// </summary>
[ExecuteAlways]
public class TextMeshProResizer : MonoBehaviour
{
    [Tooltip("List of TextMeshPro labels to be resized uniformly")]
    [SerializeField] private List<TMP_Text> labels = new List<TMP_Text>();
    [SerializeField] private bool executeOnUpdate;
    private int _currentIndex;

    private void Update()
    {
        if (executeOnUpdate) Execute();

        OnUpdateCheck();
    }

    /// <summary>
    /// Finds the label with the longest parsed text and triggers update if it changed.
    /// </summary>
    private void Execute()
    {
        if (labels.Count == 0) return;

        var count = labels.Count;

        var index = 0;
        float maxLength = 0;

        for (var i = 0; i < count; i++)
        {
            float length = labels[i].GetParsedText().Length;
            if (!(length > maxLength)) continue;
            
            maxLength = length;
            index = i;
        }

        if (_currentIndex != index)
        {
            OnChanged(index);
        }
    }
    
    /// <summary>
    /// Switches AutoSizing to the new longest label and disables it on the previous one.
    /// </summary>
    /// <param name="index">Index of the label with the longest text.</param>
    private void OnChanged(int index)
    {
        // Disable auto size on previous
        labels[_currentIndex].enableAutoSizing = false;

        _currentIndex = index;

        // Force an update of the candidate text object so we can retrieve its optimum point size.
        labels[index].enableAutoSizing = true;
        labels[index].ForceMeshUpdate();
    }
    
    /// <summary>
    /// Applies the calculated optimum font size to all other labels (disabling their AutoSizing).
    /// </summary>
    private void OnUpdateCheck()
    {
        var optimumPointSize = labels[_currentIndex].fontSize;

        // Iterate over all other text objects to set the point size
        var count = labels.Count;

        for (var i = 0; i < count; i++)
        {
            if (_currentIndex == i) continue;

            labels[i].enableAutoSizing = false;

            labels[i].fontSize = optimumPointSize;
        }
    }
}
