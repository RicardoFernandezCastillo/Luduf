using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraPlayer : MonoBehaviour
{
    // intancia del Player para que la camara lo siga
    public Player player;

   

    void Start()
    {
        GameObject g = GameObject.FindGameObjectWithTag("Player");
        player = g.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // La camara no debe girar en el eje y


}
