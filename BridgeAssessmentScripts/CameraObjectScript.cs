using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraObjectScript : MonoBehaviour
{
    [Tooltip("This controls how fast the camera rotates")]
    [SerializeField] private float cameraMovementSpeed = 0.25f;
    [Tooltip("This controls how fast the camera zooms, keep it low for best results")]
    [SerializeField] private float zoomSpeed = 0.005f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInt =  Input.GetAxisRaw("Horizontal")*-1f;
        float verticalInt = Input.GetAxisRaw("Vertical");

        
        Vector3 cameraRotationInput = new Vector3(verticalInt, horizontalInt)*cameraMovementSpeed;

        //Adds the rotation input with the current rotation
        transform.rotation =  Quaternion.Euler(cameraRotationInput + transform.rotation.eulerAngles);

        if (Input.GetKey(KeyCode.Equals))
        {
            transform.localScale *= (1-zoomSpeed);
        }

        if (Input.GetKey(KeyCode.Minus))
        {
            transform.localScale *= (1+zoomSpeed);

        }
    }

   
}
