using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Operand_Card_Behavior : MonoBehaviour
{
    //Numbers vars
    private Vector3 rest_position; 
    [SerializeField] private bool is_touched;
    [SerializeField] private bool is_following;
    [SerializeField] private float move_speed;
    [SerializeField] private float raise_dist;
    private Vector3 mouse_pos;
    [SerializeField] LayerMask mask;
    private bool can_be_clicked = true;

    //Gameobject vars
    [SerializeField] private GameObject canvas_obj;
    [SerializeField] private Player_Values player_vals;
    [SerializeField] private GameObject card_obj;
    private Transform card_trans;
    private Transform collider_trans;
    private BoxCollider2D bc;
    [SerializeField] private int my_operand = 1;
    [SerializeField] private Sprite[] card_sprites;
    [SerializeField] private SpriteRenderer card_spr; //NOTE: Do not remove [SerializeField] for this variable!

    // Start is called before the first frame update
    void Start()
    {
        //Setup
        mask = LayerMask.GetMask("Operand Cards");
        canvas_obj = GameObject.Find("Canvas");
        player_vals = canvas_obj.GetComponent<Player_Values>();
        is_touched = false;
        is_following = false;
        rest_position = GetComponent<Transform>().position;
        card_obj = this.gameObject.transform.GetChild(0).gameObject;
        card_trans = card_obj.transform;
        collider_trans = gameObject.GetComponent<Transform>();
        bc = gameObject.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player_vals.Get_Is_Paused())
            return;
            
        //setup step
        float step = move_speed * Time.deltaTime;

        //setup mouse_pos
        mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //cast ray from camera to mouse position
        RaycastHit2D[] hits = Physics2D.RaycastAll(mouse_pos, Vector2.zero, Mathf.Infinity, mask);

        if(hits.Length == 1)
            can_be_clicked = true;
        else    
            can_be_clicked = false;

        //if above ray hit this collider then,
        if(can_be_clicked && hits[0].collider != null && hits[0].collider.gameObject == this.gameObject && player_vals.Get_Is_Holding() == false)
        {
            //Debug.Log("Touch!");
            //set is_touched to true
            is_touched = true;

            //if left mouse button was pressed, 
            if(Input.GetMouseButtonDown(0))
            {
                //Rest Inputs
                Input.ResetInputAxes();

                //set is_following to true
                is_following = true;

                //set is_holding in Player_Values script to true;
                player_vals.Set_Is_Holding(true);

                //set held card to this card
                player_vals.Set_Held_Card(gameObject);

                //set held_card_type to "operand"
                player_vals.Set_Held_Card_Type("operand");

                //Debug.Log("Pick up Card!");
            }
        }
        else
        {
            //set is_touched to false
            is_touched = false;
        }

        if(is_touched == true && is_following == false && player_vals.Get_Is_Holding() == false)
        {
            //raise card
            card_trans.position = Vector2.MoveTowards(card_trans.position, transform.position + new Vector3(0,raise_dist,0), step);
        }
        else if(is_touched == false && is_following == false)
        {
            //move card to rest position
            card_trans.position = Vector2.MoveTowards(card_trans.position, rest_position, step);
        }
        else if(is_following == true)
        {
            //follow mouse position
            collider_trans.position = new Vector3(mouse_pos.x, mouse_pos.y, 0);

            //if right mouse button was pressed,
            if(Input.GetMouseButtonDown(1))
            {
                Reset_Card();
            }
        }
    }

    public void Reset_Card()
    {
        //set is_following to false
        is_following = false; 

        //reset position
        collider_trans.position = Vector2.MoveTowards(collider_trans.position, rest_position, 10000f);

        //gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;

        //set is_holding in Player_Values script to false;
        player_vals.Set_Is_Holding(false);

        //set held_card_type to "none"
        player_vals.Set_Held_Card_Type("none");

        //Debug.Log("Reset Card");
    }

    public void Set_Rest_Position(Vector3 new_pos)
    {
        rest_position = new_pos;
    }

    public bool Get_Can_Be_Clicked()
    {
        return can_be_clicked;
    }

    public void Set_My_Operand(int new_op)
    {
        my_operand = new_op;

        card_spr.sprite = card_sprites[my_operand - 1];
    }

    public int Get_My_Operand()
    {
        return my_operand;
    }
}
