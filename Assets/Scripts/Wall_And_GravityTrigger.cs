using UnityEngine;

public class Wall_And_GravityTrigger : MonoBehaviour
{
    public Vector3 playerRotation;
    Gravity gravity;
    void Start()
    {
        gravity = FindFirstObjectByType<Gravity>();
    }
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            gravity.SetGravity(playerRotation);
    }
}
