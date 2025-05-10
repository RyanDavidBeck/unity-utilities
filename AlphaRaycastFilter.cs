using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Filters raycasts on a UI Image based on the alpha value of its pixels.
/// Only areas with sufficient opacity will register raycast hits.
/// </summary>
/// <remarks>
/// This script implements ICanvasRaycastFilter and can be used to make only the visible (non-transparent)
/// parts of a UI Image interactable (e.g., for precise button shapes).
/// </remarks>
[RequireComponent(typeof(Image))]
public class AlphaRaycastFilter : MonoBehaviour, ICanvasRaycastFilter
{
    [SerializeField] private float alphaThreshold = 0.1f; //Threshold value - everything with alpha below this value is ignored
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    /// <summary>
    /// Determines whether the given screen point is valid for a raycast hit,
    /// based on the alpha of the underlying pixel in the Image's texture.
    /// </summary>
    /// <param name="screenPosition">The screen point of the raycast.</param>
    /// <param name="eventCamera">The camera associated with the event (used for coordinate conversion).</param>
    /// <returns>
    /// True if the pixel under the point has an alpha greater than the threshold; otherwise, false.
    /// </returns>
    public bool IsRaycastLocationValid(Vector2 screenPosition, Camera eventCamera)
    {
        if (_image == null || _image.sprite == null) return true;

        //Converts the click position from screen coordinates to local UI coordinates
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _image.rectTransform, screenPosition, eventCamera, out var local);

        //Allows to determine the position in the sprite regardless of its size or scaling
        var rect = _image.GetPixelAdjustedRect();
        var normalized = new Vector2(
            (local.x - rect.x) / rect.width,
            (local.y - rect.y) / rect.height);

        var tex = _image.sprite.texture;
        var spriteRect = _image.sprite.textureRect;

        //Converts the normalised coordinates into Texel positions on the texture
        var x = Mathf.FloorToInt(spriteRect.x + spriteRect.width * normalized.x);
        var y = Mathf.FloorToInt(spriteRect.y + spriteRect.height * normalized.y);

        //If the calculated position is outside the texture it is no match
        if (x < 0 || x >= tex.width || y < 0 || y >= tex.height) return false;

        return tex.GetPixel(x, y).a > alphaThreshold;
    }
}
