using Unity.Entities;
using UnityEngine;

public class ScrollUV : MonoBehaviour {

    public Vector2 Offset;
    public Vector2 Speed;
    Renderer renderer;

    void Awake() {
        renderer = GetComponent<Renderer>();
    }

    void Update() {
        Offset.x = (Offset.x + Speed.x) % 1;
        Offset.y = (Offset.y + Speed.y) % 1;
        renderer.sharedMaterial.SetFloat("_OffsetX", Offset.x);
        renderer.sharedMaterial.SetFloat("_OffsetY", Offset.y); // TODO: copy sprite shader and add offsets
    }

}
