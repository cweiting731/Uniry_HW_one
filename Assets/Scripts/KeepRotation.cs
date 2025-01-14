using UnityEngine;

public class KeepRotation : MonoBehaviour {
    void Update() {
        transform.rotation = Quaternion.LookRotation(-1*Vector3.forward, Vector3.up);
    }
}