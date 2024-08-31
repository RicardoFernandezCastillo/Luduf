using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarCam : MonoBehaviour
{
    // Start is called before the first frame update
    Transform Cam;
    void Start()
    {
        
    }
    private void LateUpdate()
    {
        if (Cam != null)
        {
            transform.LookAt(transform.position + Cam.forward);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
