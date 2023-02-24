using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


//Used for controlling Victory/defeat UI, enemy behaviour and triggers
public class VictoryDefeatManagement : MonoBehaviour {

    public static VictoryDefeatManagement instance=null;

    public bool playerHasWon = false;
    public bool playerWasDefeated = false;

    public GameObject EndgameUIGO, EndgameTextGO, EndgameScoreGO;

    public void PlayerWonGame()
    {
        playerHasWon = true;
        //Debug.ClearDeveloperConsole();
        Debug.Log("PLAYER HAS WON GAME");
        PlayerHealth.instance.isAlive = false; //so it locks the player
        //also reset its movements in animation part
        PlayerController.instance.anim.SetFloat("moveForward-Backward", 0);
        PlayerController.instance.anim.SetBool("IsRotating", false);

        //TODO: SHOW VICTORY UI
        EndgameUIGO.SetActive(true);
        EndgameTextGO.GetComponent<TextMeshProUGUI>().text = "You won!!";
        EndgameScoreGO.GetComponent<TextMeshProUGUI>().text = "Final Score: " + UIManager.instance.scoreUIValue.GetComponent<TextMeshProUGUI>().text;
    }

    public void PlayerLostGame()
    {
        playerWasDefeated = true;
        Debug.ClearDeveloperConsole();
        Debug.Log("PLAYER HAS LOST GAME");
        PlayerHealth.instance.isAlive = false;
        //also reset its movements in animation part
        PlayerController.instance.anim.SetFloat("moveForward-Backward", 0);
        PlayerController.instance.anim.SetBool("IsRotating", false);

        //TODO: SHOW DEFEAT UI
        EndgameUIGO.SetActive(true);
        EndgameTextGO.GetComponent<TextMeshProUGUI>().text = "You were defeated...";
        EndgameScoreGO.GetComponent<TextMeshProUGUI>().text = "Final Score: " + UIManager.instance.scoreUIValue.GetComponent<TextMeshProUGUI>().text;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Main Scene");
    }



	// Use this for initialization
	void Start () {
        if (instance == null)
        {
            instance=this;
        }
        else
            Destroy(this.gameObject);

        playerHasWon = false;
        playerWasDefeated = false;



	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
