// Even though this is the "Pause_Menu" Script, it's really the canvas overlay script

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause_Menu : MonoBehaviour
{
    public static bool GameIsPaused = false; 
    public static bool DifficultyScreenIsUP = true;
    public static bool GameIsOver = false; // when success or defeat is triggered, set this boolean to true
    // to use this boolean: 
    // if(Pause_Menu.GameIsPaused){ ... }

    public GameObject pauseMenuUI;
    public GameObject pauseButton; 
    public GameObject successScreenUI;
    public GameObject defeatScreenUI;
    public GameObject difficultyUI;
    [SerializeField] private GameObject eval_button_obj;

    public Player_Values pl;

    // Update is called once per frame
    void Update()
    {
        
        if (!GameIsOver)
        {
            if(Input.GetKeyDown(KeyCode.Escape)){
                if(GameIsPaused)
                {
                    Resume();
                }else{
                    Pause();
                }
            }
        }
    }

    void Start(){
        pl = GameObject.Find("Canvas").GetComponent<Player_Values>();
        if(SceneManager.GetActiveScene().buildIndex != 4)
            loadDifficultyUI();
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1f;
        GameIsPaused = false;
        pl.Set_Is_Paused(false);
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        pauseButton.SetActive(false);
        Time.timeScale = 0f;
        GameIsPaused = true; 
        pl.Set_Is_Paused(true);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        //Debug.Log("Loading menu...");
        SceneManager.LoadScene("Title_menu");
    }

    public void loadDifficultyUI()
    {
        //difficultyUI.SetActive(true);
        Time.timeScale = 0f;
        pl.Set_Is_Paused(true);
        pauseButton.SetActive(false);
    }

    public void easy()
    {
        difficultyUI.SetActive(false);
        pauseButton.SetActive(true);
        pl.Set_Operand_Upper_Bound(5);
        pl.Generate_Hand_For_n_Operands();
        Time.timeScale = 1f;
        pl.Set_Is_Paused(false);
        pl.Start_Level();
    }

    public void medium()
    {
        difficultyUI.SetActive(false);
        pauseButton.SetActive(true);
        pl.Set_Operand_Upper_Bound(9);
        pl.Generate_Hand_For_n_Operands();
        Time.timeScale = 1f;
        pl.Set_Is_Paused(false);
        pl.Start_Level();
    }

    public void hard()
    {
        difficultyUI.SetActive(false);
        pauseButton.SetActive(true);
        pl.Set_Operand_Upper_Bound(12);
        pl.Generate_Hand_For_n_Operands();
        Time.timeScale = 1f;
        pl.Set_Is_Paused(false);
        pl.Start_Level();
    }

    public void Set_Difficulty_Screen_Is_UP(bool new_bool)
    {
        DifficultyScreenIsUP = new_bool;
    }

    public bool Get_Difficulty_Screen_Is_UP()
    {
        return DifficultyScreenIsUP;
    }

    public void success()
    {
        GameIsOver = true; 
        pauseButton.SetActive(false);
        successScreenUI.SetActive(true);
    }

    public void defeat()
    {
        GameIsOver = true; 
        pauseButton.SetActive(false);
        defeatScreenUI.SetActive(true);
    }

    public void Retry()
    {
        pl.Generate_Hand_For_Retry();
        pauseButton.SetActive(true);
        defeatScreenUI.SetActive(false);
        eval_button_obj.SetActive(true);
    }

    public void Next_Turn()
    {
        pl.Next_Turn();
        pauseButton.SetActive(true);
        defeatScreenUI.SetActive(false);
        successScreenUI.SetActive(false);
        eval_button_obj.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }
}
