using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//global enumerator for rthythm states
public enum Rhythm_State
{
    IN = 0, PEAK_HOLD = 1, OUT = 2, TROUGH_HOLD = 3
} 

//Rhythm_Behavior handles the changes in rhythm states
public class Rhythm_Behavior : MonoBehaviour
{
    //number vars
    [Header("Setup Vars")]
    [SerializeField] public static float in_time = 3f;
    [SerializeField] public static float peak_hold_time = 1f;
    [SerializeField] public static float out_time = 3f;
    [SerializeField] public static float trough_hold_time = 1f;
    [Header("Debug Vars")]
    [SerializeField] private float time_counter = 0f;
    [SerializeField] public static Rhythm_State cur_rhythm_state = Rhythm_State.IN;
    private float cut_1, cut_2, cut_3, cut_4;

    // Start is called before the first frame update
    void Start()
    {
        Update_Amounts();
    }

    //Update_Amounts updates all the amounts and cuts 
    //Update_Amounts is called by Rhythm_Behavior's Start function and Pause_Menu_Behavior's Resume function
    public void Update_Amounts()
    {
        cut_1 = in_time;
        cut_2 = cut_1 + peak_hold_time;
        cut_3 = cut_2 + out_time;
        cut_4 = cut_3 + trough_hold_time;

        //Set time_counter to 0f
        time_counter = 0f;

        //Set cur_rhythm_state to IN
        cur_rhythm_state = Rhythm_State.IN;
    }

    // Update is called once per frame
    void Update()
    {
        //Determine current rhythm state based on time_counter
        //rhythm = 2.5s in, 1s peak hold, 2.5s out, 1s trough hold, repeat
        if(time_counter <= cut_1)
            cur_rhythm_state = Rhythm_State.IN;
        else if(time_counter <= cut_2)
            cur_rhythm_state = Rhythm_State.PEAK_HOLD;
        else if(time_counter <= cut_3)
            cur_rhythm_state = Rhythm_State.OUT;
        else if(time_counter <= cut_4)
            cur_rhythm_state = Rhythm_State.TROUGH_HOLD;
        else if(time_counter > cut_4)
            time_counter = 0;

        //increment time_counter
        time_counter += Time.deltaTime;
    }
}
