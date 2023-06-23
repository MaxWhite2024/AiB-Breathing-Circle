using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Allows for the changing of scenes

public class MainMenu : MonoBehaviour
{
    public Animator transition;

    public float transitionTime = 1f;

    // Function is called whenever the play button is pressed
    public void PlayGame ()
    {       
        // Load the next scene of the game
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1)); 
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        //Play animation
        transition.SetTrigger("Start");

        //Wait
        yield return new WaitForSeconds(transitionTime);

        //Load scene
        SceneManager.LoadScene(levelIndex);
    }

    // public void QuitGame ()
    // {
    //     Debug.Log("QUIT!");
    //     Application.Quit();
    // }
}
