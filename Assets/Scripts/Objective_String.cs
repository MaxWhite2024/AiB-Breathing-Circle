using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Objective_String : MonoBehaviour
{
    //call from player values
    public List<int> nums;
    public List<char> ops;


    public GameObject objectiveTextGameObj;
    public TextMeshProUGUI objectiveObj;

    public GameObject generateObjectiveOBJ;

    public (string objStr, (objType type, int target) objToken) objectiveToken; //need to save in order to evaluate later

    // display objective into 'objective text' text box
    // Start is called before the first frame update
    void Start()
    {
        //public (string objStr, (objType type, int target) objToken) generateObjective(List<int> nums, List<char> ops)

        
        // generateObjectiveOBJ = GameObject.Find("Canvas");
        // GenerateObjective oj = generateObjectiveOBJ.GetComponent<GenerateObjective>();

        // objectiveTextGameObj = GameObject.Find("objective_text");
        // objectiveObj = objectiveTextGameObj.GetComponent<TextMeshProUGUI>();

        // //grab numbers and operands from player values (for now random)
        // Player_Values playerValuesOBJ = generateObjectiveOBJ.GetComponent<Player_Values>();
        // nums = new List<int>(playerValuesOBJ.Get_Hand_Nums());
        // ops = new List<char>(playerValuesOBJ.Get_Hand_Ops());

        // Debug.Log(nums.Count);
        // Debug.Log(nums[0]);
        // Debug.Log(ops[0]);
        // objectiveToken = oj.generateObjective(nums, ops);
        // objectiveObj.text = objectiveToken.objStr;

    }

    public void DisplayObjective(){
        generateObjectiveOBJ = GameObject.Find("Canvas");
        GenerateObjective oj = generateObjectiveOBJ.GetComponent<GenerateObjective>();

        //objectiveTextGameObj = GameObject.Find("objective_text");
        objectiveObj = objectiveTextGameObj.GetComponent<TextMeshProUGUI>();

        //grab numbers and operands from player values (for now random)
        Player_Values playerValuesOBJ = generateObjectiveOBJ.GetComponent<Player_Values>();
        nums = new List<int>(playerValuesOBJ.Get_Hand_Nums());
        ops = new List<char>(playerValuesOBJ.Get_Hand_Ops());

        objectiveToken = oj.generateObjective(nums, ops);
        objectiveObj.text = objectiveToken.objStr;
    }

}
