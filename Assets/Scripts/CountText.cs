using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class CountText : MonoBehaviour {

    Text text;

    void Awake() {
        text = GetComponent<Text>();
    }

    void Start() {
        CountValueSystem.OnNewCount += count => {
            var dollars = count / 100.0;
            text.text = dollars.ToString("C");
        };
    }

    void Update() {
    }

}
