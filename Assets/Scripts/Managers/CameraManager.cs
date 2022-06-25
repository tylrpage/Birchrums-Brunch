using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private SerializableDictionary<CanvasOption, Transform> positions;
    [SerializeField] private float moveDuration;
    [SerializeField] private AnimationCurve easeCurve;

    public enum CanvasOption
    {
        Title, Level, Game
    }

    public void MoveToCanvas(CanvasOption canvasOption)
    {
        if (positions.TryGetValue(canvasOption, out Transform targetTransform))
        {
            // Keep the camera's z position the same
            Vector3 targetPosition = new Vector3(targetTransform.position.x, targetTransform.position.y,
                camera.transform.position.z);
            camera.transform.DOMove(targetPosition, moveDuration).SetEase(easeCurve);
        }
    }
}
