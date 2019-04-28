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
        Offset.x = (Offset.x + Speed.x * Time.deltaTime) % 1;
        Offset.y = (Offset.y + Speed.y * Time.deltaTime) % 1;
        renderer.material.SetFloat("_OffsetX", Offset.x);
        renderer.material.SetFloat("_OffsetY", Offset.y); // TODO: copy sprite shader and add offsets
    }

}
