using UnityEngine;

public class CarMovebyPlayer : MonoBehaviour
{
    public float speed = 5.0f; 
    public float turnSpeed = 200.0f; 


    void Start()
    {
        
    }

    void Update()
    {
    
        float verticalInput = Input.GetAxis("Vertical");

        float horizontalInput = Input.GetAxis("Horizontal");


        transform.Translate(Vector3.forward * verticalInput * speed * Time.deltaTime);
 
        transform.Rotate(Vector3.up, horizontalInput * turnSpeed * Time.deltaTime);
    }
}
