using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evaluate : MonoBehaviour
{

    [SerializeField] private GameObject successScreenUI;
    [SerializeField] private GameObject defeatScreenUI;
    [SerializeField] private GameObject eval_button_obj;
    private Player_Values PlayerValuesOBJ;

    void Start()
    {
        PlayerValuesOBJ = GameObject.Find("Canvas").GetComponent<Player_Values>();
    }

    public void evaluate(){
        if(!PlayerValuesOBJ.Get_Is_Equation_Complete())
            return;
        //grab objectiveToken from objective string script
        Objective_String ObjectiveStringOBJ = GameObject.Find("Canvas").GetComponent<Objective_String>();
        (string objStr, (objType type, int target) objToken) objectiveToken;
        objectiveToken = ObjectiveStringOBJ.objectiveToken;

        //battle ops/ battlenums
        List<int> nums = new List<int>(PlayerValuesOBJ.Get_Battle_Nums());
        List<char> ops = new List<char>(PlayerValuesOBJ.Get_Battle_Ops());

        // //check if they passed or failed
        GenerateObjective generateObjectiveOBJ = GameObject.Find("Canvas").GetComponent<GenerateObjective>();
        bool passed = generateObjectiveOBJ.evaluator(nums, ops, objectiveToken.objToken);
        
        //display correct screen
        if(passed){
            //Debug.Log("passed");
            successScreenUI.SetActive(true);
        } else {
            //Debug.Log("failed");
            defeatScreenUI.SetActive(true);
        }

        Time.timeScale = 0f;
        PlayerValuesOBJ.Set_Is_Paused(true);
        eval_button_obj.SetActive(false);
    }
}
