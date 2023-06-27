using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Boat_Behavior handles the movements of the boat based on the Input_Handler's calculations
public class Boat_Behavior : MonoBehaviour
{   
    //Gameobject vars
    private Rigidbody rb;

    //number vars
    [SerializeField] private float dst_multiplier; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Move_Boat(float dst)
    {
        //Make sure dst is not less than zero
        if(dst < 0f)
            dst = 0f;
        
        //Move boat forward by dst
        Debug.Log((int)(dst * 100) + "% accurate!");
        rb.AddForce(transform.forward * dst * dst_multiplier);
    }
}