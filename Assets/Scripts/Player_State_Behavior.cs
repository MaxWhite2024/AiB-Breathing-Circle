using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//global enumerator for player states
public enum Player_State
{
    //player is either in PEACE, STRESS, or FAIL states
    PEACE = 0, STRESS = 1, FAIL = 2
} 

//Player_State_Behavior handles when different player states are triggered and what results
//This script is on the Player_Boat object which has the Player_Boat_Tag
//Note that the Player_Boat object has a trigger collider
public class Player_State_Behavior : MonoBehaviour
{
    //Gameobject vars
    //private Pause_Menu_Behavior pause_menu_behavior;

    //number vars
    [SerializeField] public static Player_State cur_player_state = Player_State.STRESS;
    [SerializeField] private int health = 500;
    [SerializeField] private int checkpoint_layer;
    [SerializeField] private int scenario_trigger_layer;

    // Start is called before the first frame update
    void Start()
    {
        //get Checkpoints layer
        checkpoint_layer = LayerMask.NameToLayer("Checkpoints");

        //get ScenarioTriggers layer
        scenario_trigger_layer = LayerMask.NameToLayer("ScenarioTriggers");

        //find pause_menu_behavior
        //pause_menu_behavior = GameObject.FindWithTag("Pause_Menu_Tag").GetComponent<Pause_Menu_Behavior>();

        Spawn_Player();
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }

    public void Spawn_Player()
    {
        //set cur_player_state to PEACE
        cur_player_state = Player_State.STRESS;
    }

    void OnTriggerEnter(Collider other_collider)
    {
        //Debug.Log("Boat touched " + other.transform.gameObject.name);

        //Perform appropraite action based on the layer of the object that was touched
        //Note that the Player_Boat can only touch checkpoints and scenario-trigger layers
        if(other_collider.transform.gameObject.layer == checkpoint_layer)
        {
            Debug.Log("Checkpoint Touched!");
        }
        else if(other_collider.transform.gameObject.layer == scenario_trigger_layer)
        {
            Debug.Log("Scenario Trigger Touched!");
        }
        else
            Debug.Log("ERROR! Player_Boat Touched: " + other_collider.transform.gameObject.name + "! Which is of layer: " + other_collider.transform.gameObject.layer.ToString());
    }

    public void Change_Health(float change_amount)
    {
        int true_change_amount = (int)(change_amount * 100);
        
        //if true_change_amount < 70...
        if(true_change_amount < 70f)
        {
            //subtract 70 from health
            health -= 70;
        }
        else
        {
            //add true_change_amount to health
            health += true_change_amount;
        }


        //make sure that health is not over 1000 (max health)
        if(health > 1000)
            health = 1000;

        //check if player should fail (health <= 0)
        if(health <= 0)
            Fail();
    }

    public void Fail()
    {
        //change current state to FAIL
        cur_player_state = Player_State.FAIL;

        //CONTINUE LOGIC HERE:
        //...
    }
}
