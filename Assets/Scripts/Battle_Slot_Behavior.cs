using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle_Slot_Behavior : MonoBehaviour
{
    //Number vars
    [SerializeField] private string slot_type;
    private Vector3 mouse_pos;
    [SerializeField] private LayerMask mask;
    [SerializeField] private int my_equation_position;

    //GameObject vars
    [SerializeField] private GameObject canvas_obj;
    [SerializeField] private Player_Values player_vals;
    [SerializeField] private GameObject card;

    void Start()
    {
        //setup
        canvas_obj = GameObject.Find("Canvas");
        player_vals = canvas_obj.GetComponent<Player_Values>();
    }

    public void Start_Hand()
    {
        //make sure this slot is not both an operand slot and an operator slot
        if(slot_type != "operand" && slot_type != "operator")
            Debug.Log("ERROR! Invalid slot type given!");
    }

    // Update is called once per frame
    void Update()
    {
        if(player_vals.Get_Is_Paused())
            return;

        //setup mouse_pos
        mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //cast ray from camera to mouse position through all colliders
        RaycastHit2D hit = Physics2D.Raycast(mouse_pos, Vector2.zero, Mathf.Infinity, mask);

        //if above ray hit something and the player is holding the correct card type then...
        if(hit.collider != null && player_vals.Get_Is_Holding() && player_vals.Get_Held_Card_Type() == slot_type)
        {
            //if the collider of this object was found at index 1 of hits 
            if(hit.collider.gameObject == this.gameObject)
            {
                //Debug.Log("This slot touched!");
                //if left mouse button is pressed then...
                if(Input.GetMouseButtonDown(0))
                {
                    //Rest Inputs
                    Input.ResetInputAxes();

                    //get held card
                    card = player_vals.Get_Held_Card();

                    if(slot_type == "operator")
                    {
                        //get held card's "Operator_Card_Behavior" Component
                        Operator_Card_Behavior operator_card_component = card.GetComponent<Operator_Card_Behavior>();

                        if(operator_card_component.Get_Can_Be_Clicked())
                        {
                            //Set card's rest position to this slot's position
                            operator_card_component.Set_Rest_Position(gameObject.transform.position);

                            //update operator at my position in the equation
                            player_vals.Set_Battle_Ops(operator_card_component.Get_My_Operator(), my_equation_position);

                            //Reset Card
                            operator_card_component.Reset_Card();

                            StartCoroutine(Late_Operator_Reset());
                        }
                    }
                    else if(slot_type == "operand")
                    {
                        //get held card's "Operand_Card_Behavior" Component
                        Operand_Card_Behavior operand_card_component = card.GetComponent<Operand_Card_Behavior>();

                        if(operand_card_component.Get_Can_Be_Clicked())
                        {
                            //Set card's rest position to this slot's position
                            operand_card_component.Set_Rest_Position(gameObject.transform.position);

                            //gameObject.GetComponent<SpriteRenderer>().color = Color.blue;

                            //update operand at my position in the equation
                            player_vals.Set_Battle_Nums(operand_card_component.Get_My_Operand(), my_equation_position);

                            //Reset Card
                            operand_card_component.Reset_Card();

                            StartCoroutine(Late_Operand_Reset());
                        }
                    }
                    //Debug.Log("Battle Slotted!");
                }
            }
        }

        if(card != null)
        {
            //if card does NOT have same rest position as slot's position and the player is NOT holding a card then...
            if(card.transform.position != gameObject.transform.position && !player_vals.Get_Is_Holding())
            {
                //update operand at my position in the equation
                if(slot_type == "operand")
                    player_vals.Set_Battle_Nums(0, my_equation_position);
                else if(slot_type == "operator")
                    player_vals.Set_Battle_Ops((char) 0, my_equation_position);
            }
        }
        else
        {
            //update operand at my position in the equation
            if(slot_type == "operand")
                player_vals.Set_Battle_Nums(0, my_equation_position);
            else if(slot_type == "operator")
                player_vals.Set_Battle_Ops((char) 0, my_equation_position);
        }
    }

    public void Set_My_Position(int new_pos)
    {
        my_equation_position = new_pos;
    }

    IEnumerator Late_Operator_Reset()
    {
        //get held card
        GameObject card = player_vals.Get_Held_Card();

        //get held card's "Operator_Card_Behavior" Component
        Operator_Card_Behavior operator_card_component = card.GetComponent<Operator_Card_Behavior>();
        
        yield return new WaitForSeconds(0.1f);
        
        //Reset Card
        operator_card_component.Reset_Card();
    }

    IEnumerator Late_Operand_Reset()
    {
        //get held card
        GameObject card = player_vals.Get_Held_Card();

        //get held card's "Operand_Card_Behavior" Component
        Operand_Card_Behavior operand_card_component = card.GetComponent<Operand_Card_Behavior>();

        yield return new WaitForSeconds(0.1f);
        
        //Reset Card
        operand_card_component.Reset_Card();
    }
}