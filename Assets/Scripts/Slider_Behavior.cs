using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Slider_Behavior handles changes of a slider that will be reflected in the game
public class Slider_Behavior : MonoBehaviour
{
    //GameObject vars
    //all GameObject vars must be set from the inspector!
    [SerializeField] private GameObject IN_text_obj;
    private TMPro.TextMeshProUGUI IN_text;
    [SerializeField] private GameObject PEAK_HOLD_text_obj;
    private TMPro.TextMeshProUGUI PEAK_HOLD_text;
    [SerializeField] private GameObject OUT_text_obj;
    private TMPro.TextMeshProUGUI OUT_text;
    [SerializeField] private GameObject TROUGH_HOLD_text_obj;
    private TMPro.TextMeshProUGUI TROUGH_HOLD_text;

    void Start()
    {
        IN_text = IN_text_obj.GetComponent<TMPro.TextMeshProUGUI>();
        PEAK_HOLD_text = PEAK_HOLD_text_obj.GetComponent<TMPro.TextMeshProUGUI>();
        OUT_text = OUT_text_obj.GetComponent<TMPro.TextMeshProUGUI>();
        TROUGH_HOLD_text = TROUGH_HOLD_text_obj.GetComponent<TMPro.TextMeshProUGUI>();
    }

    public void IN_OnSliderChanged(float value)
    {
        //display value on text
        IN_text.text = value.ToString();

        //Update value in Rhythm_Behavior
        Rhythm_Behavior.in_time = value;
    }

    public void PEAK_HOLD_OnSliderChanged(float value)
    {
        //display value on text
        PEAK_HOLD_text.text = value.ToString();

        //Update value in Rhythm_Behavior
        Rhythm_Behavior.peak_hold_time = value;
    }

    public void OUT_OnSliderChanged(float value)
    {
        //display value on text
        OUT_text.text = value.ToString();

        //Update value in Rhythm_Behavior
        Rhythm_Behavior.out_time = value;
    }

    public void TROUGH_HOLD_OnSliderChanged(float value)
    {
        //display value on text
        TROUGH_HOLD_text.text = value.ToString();

        //Update value in Rhythm_Behavior
        Rhythm_Behavior.trough_hold_time = value;
    }
}
