using UnityEngine;

public class ScrollZoom : MonoBehaviour {

    [Range(0, 1)]
    public float TargetZoomAmount;
    [Range(0, 0.1f)]
    public float ScroolToZoomFactor = 0.035f;
    [Range(0, 0.5f)]
    public float ZoomSpeed = 0.25f;
    float zoomAmount;

    [Range(0, 20)]
    public float MinSize = 2;
    [Range(0, 20)]
    public float MaxSize = 12;

    Camera camera;

    void Awake() {
        camera = Camera.main;
        zoomAmount = TargetZoomAmount;
    }

    void FixedUpdate() {
        TargetZoomAmount = Mathf.Clamp01(TargetZoomAmount - Input.mouseScrollDelta.y * ScroolToZoomFactor);
        zoomAmount = Mathf.Lerp(zoomAmount, TargetZoomAmount, ZoomSpeed);
        camera.orthographicSize = Mathf.Lerp(MinSize, MaxSize, zoomAmount);
    }
}
