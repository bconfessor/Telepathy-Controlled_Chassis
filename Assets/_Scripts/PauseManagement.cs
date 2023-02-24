using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManagement : MonoBehaviour {

    public float timeScaleValue = 1.0f;
    public GameObject pauseMenuGO;
    public bool gamePaused = false;
    public void PauseUnpause()
    {
        if (!gamePaused)
        {
            Debug.Log("Pausing game");
            Time.timeScale = 0.0f;
            //show cursor
            Cursor.visible = true;
            //show pause menu
            pauseMenuGO.SetActive(true);
            gamePaused = true;
        }
        else
        {
            Debug.Log("Unpausing Game");
            Time.timeScale = 1.0f;
            //hide cursor
            Cursor.visible = false;
            //hide pause menu
            pauseMenuGO.SetActive(false);
            gamePaused = false;
        }
    }


	// Use this for initialization
	void Start () {
        //at the beginning of every playthrough, start paused
        gamePaused = false;
        PauseUnpause();
	}
	
	// Update is called once per frame
	void Update () {
        //if player presses ESC, pause game
        if (Input.GetKeyDown(KeyCode.Escape))
            PauseUnpause();
	}
}
