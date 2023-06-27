using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Inner_Circle_Behavior handles the behavior of the growing and shrinking circle
public class Inner_Circle_Behavior : MonoBehaviour
{
    //GameObject vars
    //private Rhythm_Behavior rhythm_behavior;
    private RectTransform rt;

    //number vars
    private Vector3 trough_size = new Vector3(0.1f, 0.1f, 1f);
    private Vector3 peak_size = new Vector3(1f, 1f, 1f);
    [SerializeField] private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        //rhythm_behavior = GameObject.Find("Manager").GetComponent<Rhythm_Behavior>();
        rt = GetComponent<RectTransform>();
        rt.localScale = trough_size;
    }

    // Update is called once per frame
    void Update()
    {
        //increment timer
        timer += Time.deltaTime;

        Rhythm_State cur_rhythm_state = Rhythm_Behavior.cur_rhythm_state;

        if(cur_rhythm_state == Rhythm_State.IN)
            rt.localScale = Vector3.Lerp(trough_size, peak_size, timer / Rhythm_Behavior.in_time);
        else if(cur_rhythm_state == Rhythm_State.PEAK_HOLD)
        {
            rt.localScale = peak_size;
            timer = 0f;
        }
        else if(cur_rhythm_state == Rhythm_State.OUT)        
            rt.localScale = Vector3.Lerp(peak_size, trough_size, timer / Rhythm_Behavior.out_time);
        else if(cur_rhythm_state == Rhythm_State.TROUGH_HOLD)
        {
            rt.localScale = trough_size;
            timer = 0f;
        }


    }

    public void Reset_Inner_Circle()
    {
        //reset scale
        rt.localScale = trough_size;

        //reset timer
        timer = 0f;
    }
}
