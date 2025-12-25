using Unity.Netcode;
using UnityEngine;

public class SimpleMove : NetworkBehaviour
{
    public float speed = 5f;
    
    void Update()
    {
        if (!IsOwner) return; // Only YOU control YOUR player
        
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(h, 0, v) * speed * Time.deltaTime);
    }
}