using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class DisableOnClick : MonoBehaviour {
    
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            gameObject.SetActive(false);
        }
    }

}
