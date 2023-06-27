using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Breath_Handler handles the player inputs, and breathing
public class Breath_Handler : MonoBehaviour
{
    //GameObject vars
    private Rhythm_Behavior rhythm_behavior;
    private Boat_Behavior boat_behavior;
    private Player_State_Behavior player_state_behavior;

    //number vars
    [SerializeField] private float breath_amount;
    [SerializeField] private bool is_breath_held_down = false;

    void Start()
    {
        //find rhythm_behavior
        rhythm_behavior = GetComponent<Rhythm_Behavior>();

        //find boat_behavior
        boat_behavior = GameObject.FindWithTag("Player_Boat_Tag").GetComponent<Boat_Behavior>();

        //find player_state_behavior
        player_state_behavior = GameObject.FindWithTag("Player_Boat_Tag").GetComponent<Player_State_Behavior>();
    }

    void Update()
    {
        if(!Pause_Menu_Behavior.is_paused)
        {
            //if breath button is pressed and current state is IN then...
            if(is_breath_held_down)
            {
                if(Rhythm_Behavior.cur_rhythm_state == Rhythm_State.IN)
                    breath_amount += Time.deltaTime;
                else if(Rhythm_Behavior.cur_rhythm_state == Rhythm_State.OUT)
                    breath_amount -= Time.deltaTime / 2f;
            }

            if(Rhythm_Behavior.cur_rhythm_state == Rhythm_State.TROUGH_HOLD)
                breath_amount = 0f;
        }
    }

    public void Breath(InputAction.CallbackContext context)
    {
        if(!Pause_Menu_Behavior.is_paused)
        {
            //If spacebar held down:
            if(context.performed)
            {
                //Debug.Log("Spacebar held!");
                is_breath_held_down = true;
            }
            else
                is_breath_held_down = false;

            //If spacebar released:
            if(context.canceled)
            {
                //Debug.Log("Spacebar released!");
                //if current state is OUT or PEAK_HOLD then:
                if(Rhythm_Behavior.cur_rhythm_state == Rhythm_State.OUT || Rhythm_Behavior.cur_rhythm_state == Rhythm_State.PEAK_HOLD)
                {
                    //move boat by percentage of breath ammount divided by in_time
                    boat_behavior.Move_Boat(breath_amount / Rhythm_Behavior.in_time);

                    //if current state is STRESS then...
                    if(Player_State_Behavior.cur_player_state == Player_State.STRESS)
                    {
                        //change health by percentage of breath ammount divided by in_time
                        player_state_behavior.Change_Health(breath_amount / Rhythm_Behavior.in_time);
                    }
                }

                //set breath_amount to 0f
                breath_amount = 0f;
            }  
        }
    }


}
