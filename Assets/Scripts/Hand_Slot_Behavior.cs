using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand_Slot_Behavior : MonoBehaviour
{
    //Number vars
    [SerializeField] private string slot_type;
    private Vector3 mouse_pos;
    [SerializeField] LayerMask mask;
    [SerializeField] private int slot_symbol;

    //GameObject vars
    [SerializeField] private GameObject card_prefab;
    [SerializeField] private GameObject canvas_obj;
    [SerializeField] private Player_Values player_vals;
    private GameObject spawned_card;

    void Start()
    {
        //setup
        canvas_obj = GameObject.Find("Canvas");
        player_vals = canvas_obj.GetComponent<Player_Values>();
    }

    public void Start_Hand()
    {
        //create a card
        spawned_card = Instantiate(card_prefab, transform.position, Quaternion.identity);
        if(slot_type == "operand")
            spawned_card.GetComponent<Operand_Card_Behavior>().Set_My_Operand(slot_symbol);
        else if(slot_type == "operator")
            spawned_card.GetComponent<Operator_Card_Behavior>().Set_My_Operator((char) slot_symbol);

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

        //if above ray hit at least two things and the player is holding the correct card type then...
        if(hit.collider != null && player_vals.Get_Is_Holding() && player_vals.Get_Held_Card_Type() == slot_type)
        {    
            //if the collider of this object was found at index 1 of hits
            if(hit.collider.gameObject == this.gameObject)
            {
                //Debug.Log("This slot touched!");
                //if left mouse button is pressed then...
                if(Input.GetMouseButtonDown(0))
                {
                    //set is_slotted to true
                    //is_slotted = true;

                    //Rest Inputs
                    Input.ResetInputAxes();

                    //get held card
                    GameObject card = player_vals.Get_Held_Card();

                    if(slot_type == "operator")
                    {
                        //get held card's "Operator_Card_Behavior" Component
                        Operator_Card_Behavior operator_card_component = card.GetComponent<Operator_Card_Behavior>();

                        if(operator_card_component.Get_Can_Be_Clicked())
                        {
                            //Set card's rest position to this slot's position
                            operator_card_component.Set_Rest_Position(gameObject.transform.position);

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

                            //Reset Card
                            operand_card_component.Reset_Card();

                            StartCoroutine(Late_Operand_Reset());
                        }
                    }

                    //Debug.Log("Hand Slotted!");
                }
            }
        }
    }

    public void Set_Slot_Symbol(int symbol)
    {
        slot_symbol = symbol;
    }

    public void Destroy_Card()
    {
        if(spawned_card == null)
            Debug.Log("ERROR! No card to destroy!");
        else
            Destroy(spawned_card);
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
