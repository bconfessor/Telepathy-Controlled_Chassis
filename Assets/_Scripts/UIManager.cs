using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour {

    public static UIManager instance = null;

    public GameObject hackTimerUIHeader, hackTimerUIValue; //hack UI elements

    public GameObject scoreUIValue, HPUIValue;
    public GameObject currentAmmoUIValue, maxAmmoUIValue, currentMissileUIValue, maxMissileUIValue ; // ammo UI elements
    public GameObject informationTextGO, informationBackgroundGO;

    public GameObject countdownUIValue;
    public int gameTimeLimit = 600;//in seconds
    public float initialTime;//gets Time.time when game starts
    public float currentTime;

    Color baseAmmoColor, baseMissileColor;


    /// <summary>
    /// Gets Score text in integer form
    /// </summary>
    /// <returns>Int for the score value</returns>
    public int GetCurrentScore()
    {
        return int.Parse(scoreUIValue.GetComponent<TextMeshProUGUI>().text);
    }

    /// <summary>
    /// Adds value to current score
    /// </summary>
    /// <param name="valueToAdd">pretty self-explanatory</param>
    public void AddValueToScore(int valueToAdd)
    {
        scoreUIValue.GetComponent<TextMeshProUGUI>().text= (GetCurrentScore() + valueToAdd).ToString("000000");
    }

    /// <summary>
    /// Changes current score to new given
    /// </summary>
    /// <param name="newScoreValue">New score value</param>
    public void ResetScoreValue(int newScoreValue)
    {
        scoreUIValue.GetComponent<TextMeshProUGUI>().text = "000000";
    }


    //Ammo UI functions
    public void ChangeCurrentAmmoUIValue(int newAmmoValue)
    {
        currentAmmoUIValue.GetComponent<TextMeshProUGUI>().text = newAmmoValue.ToString();
    }

    public void AddCurrentAmmoUIValue(int ammoToAdd)
    {
        string currentVal = currentAmmoUIValue.GetComponent<TextMeshProUGUI>().text;
        int newVal = int.Parse(currentVal) + ammoToAdd;

        //can't get higher than max
        if (newVal > int.Parse(maxAmmoUIValue.GetComponent<TextMeshProUGUI>().text))
            newVal = int.Parse(maxAmmoUIValue.GetComponent<TextMeshProUGUI>().text);

        //and get get lower than min
        else if (newVal <= 0)
        {
            newVal = 0;   
        }

        //change text color
        if(newVal<=0)
            currentAmmoUIValue.GetComponent<TextMeshProUGUI>().color = Color.red;
        else
            currentAmmoUIValue.GetComponent<TextMeshProUGUI>().color = baseAmmoColor;

        currentAmmoUIValue.GetComponent<TextMeshProUGUI>().text = newVal.ToString();
    }

    public void ChangeMaxAmmoUIValue(int newAmmoValue)
    {
        maxAmmoUIValue.GetComponent<TextMeshProUGUI>().text = newAmmoValue.ToString();
    }


    //Missile UI functions
    public void ChangeCurrentMissileUIValue(int newAmmoValue)
    {
        currentMissileUIValue.GetComponent<TextMeshProUGUI>().text = newAmmoValue.ToString();
    }

    public void AddCurrentMissileUIValue(int ammoToAdd)
    {
        string currentVal = currentMissileUIValue.GetComponent<TextMeshProUGUI>().text;
        int newVal = int.Parse(currentVal) + ammoToAdd;

        //can't get higher than max
        if (newVal > int.Parse(maxMissileUIValue.GetComponent<TextMeshProUGUI>().text))
            newVal = int.Parse(maxMissileUIValue.GetComponent<TextMeshProUGUI>().text);

        //and get get lower than min
        else if (newVal <= 0)
        {
            newVal = 0;
        }

        //change text color
        if (newVal <= 0)
            currentMissileUIValue.GetComponent<TextMeshProUGUI>().color = Color.red;
        else
            currentMissileUIValue.GetComponent<TextMeshProUGUI>().color = baseMissileColor;

        currentMissileUIValue.GetComponent<TextMeshProUGUI>().text = newVal.ToString();
    }

    public void ChangeMaxMissileUIValue(int newAmmoValue)
    {
        maxMissileUIValue.GetComponent<TextMeshProUGUI>().text = newAmmoValue.ToString();
    }


    
    //HP UI functions

    public void ChangeCurrentHPUIValue(float newHPValue)
    {
        HPUIValue.GetComponent<TextMeshProUGUI>().text = newHPValue.ToString()+"%";
    }

    public void AddHPUIValue(float HPToAdd)
    {
        float currentVal = float.Parse(HPUIValue.GetComponent<TextMeshProUGUI>().text.Replace("%", ""));
        
        if (currentVal + HPToAdd >= PlayerHealth.instance.playerMaxHealth)
            HPUIValue.GetComponent<TextMeshProUGUI>().text = PlayerHealth.instance.playerMaxHealth.ToString()+"%";
        else
            HPUIValue.GetComponent<TextMeshProUGUI>().text = (currentVal + HPToAdd).ToString()+"%";
    }



    public void TimeRanOut()
    {
        //player lost game
        VictoryDefeatManagement.instance.PlayerLostGame();
    }

    IEnumerator UpdateScoreUI()
    {
        currentTime = Time.time;
        yield return new WaitForSeconds(Time.deltaTime);
        //only runs if player is still alive and has not won yet
        if (!(VictoryDefeatManagement.instance.playerHasWon || VictoryDefeatManagement.instance.playerWasDefeated))
        {
            //current time left, in seconds
            float currentTimeLeft = gameTimeLimit - (currentTime - initialTime);
            
            //if time reached half of max time, change countdown letter color to yellow
            if(currentTimeLeft<=gameTimeLimit/2)
            {
                countdownUIValue.GetComponent<TextMeshProUGUI>().color = Color.yellow;
            }

            //else, if it reaches one minute left, change it to red
            if(currentTimeLeft<=60.0f)
            {
                countdownUIValue.GetComponent<TextMeshProUGUI>().color = Color.red;
            }

            //if time is up, don't bother calculating countdown timer and show player defeat
            if (currentTimeLeft<=0.0f)
            {
                TimeRanOut();
                countdownUIValue.GetComponent<TextMeshProUGUI>().text= "00:00";

            }
            else//, show current time
            {
                int minutesLeft = (int)currentTimeLeft / 60;
                float secondsLeft = currentTimeLeft % 60;
                countdownUIValue.GetComponent<TextMeshProUGUI>().text = minutesLeft.ToString("00")+":"+secondsLeft.ToString("0.00");
            }
        }


        //either way, runs it again at the end of the iteration
        StartCoroutine(UpdateScoreUI());
    }






    //Changes value of hack UI value
    public IEnumerator WaitForHackToFinish(float hackDuration)
    {
        //activate hack duration UI text and header
        hackTimerUIValue.SetActive(true);
        hackTimerUIHeader.SetActive(true);
        float currentTimerValue = hackDuration;
        hackTimerUIValue.GetComponent<TextMeshProUGUI>().text = hackDuration.ToString("F");
        while (currentTimerValue > 0.0f)
        {
            currentTimerValue -= Time.deltaTime;
            hackTimerUIValue.GetComponent<TextMeshProUGUI>().text = currentTimerValue.ToString("F");
            yield return new WaitForSeconds(Time.deltaTime);
        }

        //at the end, just fix it to 0
        hackTimerUIValue.GetComponent<TextMeshProUGUI>().text = "0.00";

        //After hack is done, call final step of this route
        CollectiblesManager.instance.HackFinished();

    }


    
    IEnumerator FlashTextThenDisable(GameObject objectToFlash)
    {
        //activate GO if not already
        objectToFlash.SetActive(true);
        //flash it three times, each with a period of 1s
        for(int i = 0;i<3;i++)
        {
            yield return new WaitForSeconds(0.5f);
            //turn off
            objectToFlash.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            //turn on again
            objectToFlash.SetActive(true);
        }

        //after this, remains on for 10s, then vanishes/turns off for good
        yield return new WaitForSeconds(10f);
        objectToFlash.SetActive(false);

    }

    /// <summary>
    /// Writes given string to InformationTextGO, then make it flash on screen
    /// </summary>
    /// <param name="infoToShow"></param>
    public void ShowInformation(string infoToShow)
    {
        informationTextGO.GetComponent<TextMeshProUGUI>().text = infoToShow;
        //UPDATE 24/02/23: Make BG flash instead of text itself, is enough
        StartCoroutine(FlashTextThenDisable(informationBackgroundGO));
    }



    // Use this for initialization
    void Awake () {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("Destroyed " + gameObject.name + " because it's not singleton in UIManager");
            Destroy(this.gameObject);
        }

        //gets initial time
        initialTime = Time.time;
        StartCoroutine(UpdateScoreUI());
        baseAmmoColor = currentAmmoUIValue.GetComponent<TextMeshProUGUI>().color;
        baseMissileColor = currentMissileUIValue.GetComponent<TextMeshProUGUI>().color;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
