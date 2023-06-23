using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Stores all the variables and functions of the current player
public class Player_Values : MonoBehaviour
{
    //operand_upper_bound stores the upper bound of the operand range and is changed by the selected difficulty
    //This value must be above 0
    private int operand_upper_bound;

    //num_operands_in_deck stores the number of operand card that will appear in the player's hand at the start of a turn
    private int num_operands_in_deck; 

    //current_turn stores the current turn of the scene
    [SerializeField] private int current_turn = 1; 

    //current_level stores the current level the player is in
    [SerializeField] private int current_level;

    //is_holding_card stores whether or not the player is holding a card
    private bool is_holding_card = false; 

    //held_card stores the GameObject representing the LAST held card
    private GameObject held_card; 

    //held_card_type stores the type of the currently held card (should only be either "none", "operand", or "operator")
    private string held_card_type = "none";

    //number_of_operands stores the number of operands that Player_Values should generate for the current scene
    //This value must be set from the inspector!
    [SerializeField] private int number_of_operands;


    /*** Player hand variables ***/
    //hand_operands stores a List of all the integer operands currently in the player's hand
    [SerializeField] private List<int> hand_operands;

    //hand_operand_objs stores a List of the operand card objects currently in the player's hand
    //These values must be set from the inspector!
    [SerializeField] private List<GameObject> hand_operand_objs;

    //hand_operators stores a List of all the character operators currently in the player's hand
    [SerializeField] private List<char> hand_operators;

    //hand_operator_objs stores a List of the operator card objects currently in the player's hand
    //These values must be set from the inspector!
    [SerializeField] private List<GameObject> hand_operator_objs;


    /*** Battle Slot variables ***/
    //battle_objects stores a List of the operator and operand cards currently in the battle slots
    //These values must be set from the inspector and must be in correct order!
    [SerializeField] private List<GameObject> battle_objects;

    //battle_nums stores the operands in the battle equation in order from left to right
    //if NONE of the elements of battle_nums are 0 and NONE of the elements of battle_ops are 0 then, the battle equation can be evaluated
    [SerializeField] private List<int> battle_nums;

    //battle_ops stores the operators in the battle equation in order from left to right
    //if NONE of the elements of battle_nums are 0 and NONE of the elements of battle_ops are 0 then, the battle equation can be evaluated
    [SerializeField] private List<char> battle_ops;

    //is_equation_complete stores whether or not the battle equation can be evaluated
    [SerializeField] private bool is_equation_complete = false;

    private bool is_paused = false; 

    public Pause_Menu pl;

    [SerializeField] private GameObject objective_obj;

    [SerializeField] private GameObject eval_button_obj;

    void Awake()
    {
        pl = GameObject.Find("Canvas").GetComponent<Pause_Menu>();
        //objective_obj = GameObject.Find("objective_text");
        //eval_button_obj = GameObject.Find("EvalButton");

        //initailize current_level
        current_level = SceneManager.GetActiveScene().buildIndex;
    }

    public void Start_Level()
    {
        Time.timeScale = 1f;

        //Start each Hand slot
        for(int i = 0; i < hand_operand_objs.Count; i++)
        {
            hand_operand_objs[i].GetComponent<Hand_Slot_Behavior>().Start_Hand();
        }
        for(int i = 0; i < hand_operator_objs.Count; i++)
        {
            hand_operator_objs[i].GetComponent<Hand_Slot_Behavior>().Start_Hand();
        }

        //generate and display correct objective
        GameObject generateObjectiveOBJ = GameObject.Find("Canvas");
        Objective_String oj = generateObjectiveOBJ.GetComponent<Objective_String>();

        //display relevant gameplay UI elements
        objective_obj.SetActive(true);
        eval_button_obj.SetActive(true);

        oj.DisplayObjective();
    }

    void Update()
    {
        if(is_paused)
            return;

        //start off temp_bool as true then run it through the gauntlet
        bool temp_bool = true;

        //check to see if equation is complete and is therefor ready to be evaluated
        //if any of the elements of battle_nums or battle_ops is 0 then equation can NOT be evaluated
        for(int i = 0; i < battle_nums.Count; i++)
        {
            if(battle_nums[i] == 0)
            {
                temp_bool = false;
                break;
            }
        }
        //if equation is already incomplete, don't even bother with checking battle_ops
        if(temp_bool == true)
        {
            for(int i = 0; i < battle_ops.Count; i++)
            {
                if(battle_ops[i] == 0)
                {
                    temp_bool = false;
                    break;
                }
            }
        }

        if(temp_bool == true)
            is_equation_complete = true;
        else    
            is_equation_complete = false;
        //Debug.Log(is_equation_complete);
        //Debug.Log("Battle equation: " + battle_nums[0].ToString() + " " + battle_ops[0].ToString() + " " + battle_nums[1].ToString() + " " + battle_ops[1].ToString() + " " + battle_nums[2].ToString());
    }

    //Move to next turn with number_of_operands operands
    public void Next_Turn()
    {
        Time.timeScale = 1f;

        //reset is_holding_card
        is_holding_card = false;

        //reset is_paused
        is_paused = false;

        //increment current turn
        current_turn++;

        //if current_turn is greater than or equal to 4, move on to next level
        if(current_turn >= 4)
            SceneManager.LoadScene(current_level + 1);

        //tell hand slots to delete the cards they spawned
        for(int i = 0; i < number_of_operands; i++)
            hand_operand_objs[i].GetComponent<Hand_Slot_Behavior>().Destroy_Card();
        for(int i = 0; i < (number_of_operands - 1); i++)
            hand_operator_objs[i].GetComponent<Hand_Slot_Behavior>().Destroy_Card();

        //generate next hand
        Generate_Hand_For_n_Operands();

        Start_Level();
    }

    //generate hand for level with number_of_operands operands
    public void Generate_Hand_For_n_Operands()
    {
        //let equation_size be the number of operands plus the number of operators
        int equation_size = number_of_operands + (number_of_operands - 1);

        //reset is_holding_card
        is_holding_card = false;

        //set number of operands to n
        num_operands_in_deck = number_of_operands;

        //set hand_operands and hand_operand_objs
        hand_operands = new List<int>(new int[number_of_operands]);
        if(number_of_operands > hand_operands.Count){
            Debug.Log("ERROR!: nunber of OPERAND cards not correct!");
            Debug.Log("Number of OPERAND cards is: " + hand_operands.Count + ". While the input number is: " + number_of_operands);
        }
        for(int i = 0; i < number_of_operands; i++)
        {
            hand_operands[i] = (int) Random.Range(1f, ((float) operand_upper_bound) + 0.99999f);
            hand_operand_objs[i].GetComponent<Hand_Slot_Behavior>().Set_Slot_Symbol(hand_operands[i]);
        }
        //Debug.Log("***************** Operands are: " + hand_operands[0] + ", " + hand_operands[1] + ", " + hand_operands[2]);

        //set hand_operators
        hand_operators = new List<char>(new char[number_of_operands - 1]);
        if((number_of_operands - 1) > hand_operators.Count){
            Debug.Log("ERROR!: nunber of OPERATOR cards not correct!");
            Debug.Log("Number of cards is: " + hand_operands.Count + ". While the input number is: " + number_of_operands);
        }
        for(int i = 0; i < (number_of_operands - 1); i++)
        {
            if(i == 0)
                hand_operators[i] = '+';
            else if(i == 1)
                hand_operators[i] = '*';
            else if(i == 2)
                hand_operators[i] = '-';
            else if(i == 3)
                hand_operators[i] = '/';
            else
                Debug.Log("ERROR! number of OPERATOR cards exceeds 4!");
            hand_operator_objs[i].GetComponent<Hand_Slot_Behavior>().Set_Slot_Symbol(hand_operators[i]);
        }
        //Debug.Log("***************** Operators are: " + hand_operators[0] + ", " + hand_operators[1]);

        //give each battle object a position
        for(int i = 0; i < equation_size; i++)
        {
            if(i == 0)
                battle_objects[i].GetComponent<Battle_Slot_Behavior>().Set_My_Position(0);
            else if(i == 1)
                battle_objects[i].GetComponent<Battle_Slot_Behavior>().Set_My_Position(0);
            else if(i == 2)
                battle_objects[i].GetComponent<Battle_Slot_Behavior>().Set_My_Position(1);
            else if(i == 3)
                battle_objects[i].GetComponent<Battle_Slot_Behavior>().Set_My_Position(1);
            else if(i == 4)
                battle_objects[i].GetComponent<Battle_Slot_Behavior>().Set_My_Position(2);
            else if(i == 5)
                battle_objects[i].GetComponent<Battle_Slot_Behavior>().Set_My_Position(2);
            else if(i == 6)
                battle_objects[i].GetComponent<Battle_Slot_Behavior>().Set_My_Position(3);
            else if(i == 7)
                battle_objects[i].GetComponent<Battle_Slot_Behavior>().Set_My_Position(3);
            else if(i == 8)
                battle_objects[i].GetComponent<Battle_Slot_Behavior>().Set_My_Position(4);
            else
                Debug.Log("ERROR! equation is too large!");
        } 

        //reset battle_nums
        battle_nums = new List<int>(new int[number_of_operands]);

        //reset battle_ops
        battle_ops = new List<char>(new char[number_of_operands - 1]);
    }

    public void Generate_Hand_For_Retry()
    {
        Time.timeScale = 1f;

        //reset is_holding_card
        is_holding_card = false;

        //reset is_paused
        is_paused = false;
    }

    public void Set_Operand_Upper_Bound(int new_int)
    {
        operand_upper_bound = new_int;
    }

    public int Get_Num_Operands()
    {
        return num_operands_in_deck;
    }

    public void Set_Num_Operands(int new_num)
    {
        num_operands_in_deck = new_num;
    }

    public bool Get_Is_Holding()
    {
        return is_holding_card;
    }

    public void Set_Is_Holding(bool new_bool)
    {
        is_holding_card = new_bool;
    }

    public GameObject Get_Held_Card()
    {
        return held_card;
    }

    public void Set_Held_Card(GameObject new_obj)
    {
        held_card = new_obj;
    }

    public string Get_Held_Card_Type()
    {
        return held_card_type;
    }

    public void Set_Held_Card_Type(string new_string)
    {
        held_card_type = new_string;
    }
    
    public void Set_Upper_Bound(int new_bound)
    {
        operand_upper_bound = new_bound;
    }

    public List<int> Get_Battle_Nums()
    {
        return battle_nums;
    }

    public void Set_Battle_Nums(int number, int index)
    {
        battle_nums[index] = number;
    }

    public List<char> Get_Battle_Ops()
    {
        return battle_ops;
    }

    public List<char> Get_Hand_Ops()
    {
        return hand_operators;
    }

    public List<int> Get_Hand_Nums()
    {
        return hand_operands;
    }

    public void Set_Battle_Ops(char symbol, int index)
    {
        battle_ops[index] = symbol;
    }

    //Function Definition for Get_Is_Equation_Complete:
    //if the equation is ready to be evaluated, then Get_Is_Equation_Complete return true
    //else, Get_Is_Equation_Complete return false
    public bool Get_Is_Equation_Complete()
    {
        return is_equation_complete;
    }
    

    public bool Get_Is_Paused()
    {
        return is_paused;     
    }

    public void Set_Is_Paused(bool noow)
    {
        is_paused = noow;     
    }
}
