using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleUI : MonoBehaviour
{
    public void OnClicked()
    {
        GameManager.Instance.CameraManager.MoveToCanvas(CameraManager.CanvasOption.Level);
    }
}
