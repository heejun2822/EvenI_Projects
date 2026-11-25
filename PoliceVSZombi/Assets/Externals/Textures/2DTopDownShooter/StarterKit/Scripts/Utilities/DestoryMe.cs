using UnityEngine;

/// <summary>
/// A small utility class to destroy GameObjects
/// </summary>
public class DestoryMe : MonoBehaviour {
    /// <summary>
    /// Amount of time to delay before destroying the object.
    /// </summary>
    public float In;
    
    private void Start() {
        Destroy(gameObject, In);
    }
}
