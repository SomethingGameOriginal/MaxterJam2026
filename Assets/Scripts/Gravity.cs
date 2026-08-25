using UnityEngine;

public class Gravity : MonoBehaviour
{
    public Vector3 gravityRotation;
    void Start()
    {
        
    }
    void Update()
    {
        transform.eulerAngles = gravityRotation;
    }
    public void SetGravity(Vector3 rotation)
    {
        gravityRotation = rotation;
    }
}
