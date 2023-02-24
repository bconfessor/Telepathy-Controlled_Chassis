using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectiblesManager : MonoBehaviour {

    public static CollectiblesManager instance = null;

    //Electronic gate Route
    public bool hasQuantumNotebook = false;
    public int cypherKeysCollected = 0;
    public float hackDuration;
    public GameObject hackValueHeader, hackValueText;//header and value for duration of hack

    public GameObject[] quantumNotebookLocations;//possible spawn locations of the quantum notebook
    public GameObject[] cypherKeyLocations; //locations of "arcades" with cypher keys
    public GameObject HackGatePanelLocation; //Location where player must stand to begin hacking the electronic gate
    public GameObject HackGateController; //Location that activates Gate Controller Opening
    public GameObject HackGateEscapeRoute; //final location for the hack gate escape route

    //Boulders Route
    public bool hasExplosives = false;
    public bool hasRemoteDetonator = false;
    public bool hasExplodedBoulders = false;

    public GameObject[] explosivesLocations; //possible spawn locations of the explosives
    public GameObject[] remoteDetonatorLocations; //possible spawn of remote detonator
    public GameObject BoulderExplosionLocation; //location where player must stand to explode boulders
    public GameObject BoulderEscapeRoute; //Final location for the boulder escape route

    //Military Checkpoint Route
    public bool hasOverclockCircuit = false;
    public bool hasExplodedPowerPlant = false;

    public GameObject[] overclockCircuitLocations;//possible spawn locations of the Overclock Circuit
    public GameObject PowerPlantFuseBoxLocation;
    public GameObject PowerPlantExplosionParentGO;//Explosion and fire Game Objects (to be used in explosion animation/coroutine)
    public GameObject gateGO; //Gate that opens once power plant explodes
    public GameObject MilitaryEscapeRoute; //Final location for the Military Checkpoint escape route

    public GameObject[] BouldersList; //list of boulders


    //Phone booths extra collectibles (NOT IMPLEMENTED FOR NOW)
    public int phoneSecretsUncovered = 0;

    public int score = 0; //player's score
    public GameObject scoreUIValGO; //GameObject of the Score's UI value

    //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\ INITIALIZATION FUNCTIONS \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

    //When game starts, only the first step of each escape route (quantum computer, explosives and overclock circuit) will be active
    public void ActivateFirstStepLocations()
    {
        int i;
        for (i = 0; i < quantumNotebookLocations.Length; i++)
            quantumNotebookLocations[i].SetActive(true);

        for (i = 0; i < explosivesLocations.Length; i++)
            explosivesLocations[i].SetActive(true);

        for (i = 0; i < overclockCircuitLocations.Length; i++)
            overclockCircuitLocations[i].SetActive(true);


    }

    
    /// <summary>
    /// Chooses which locations of each escape route path will have items
    /// </summary>
    public void ChooseLocationsToHaveItems()
    {
        int locationIndex = 0; //saves index of each location that will get a key item

        //Military checkpoint path

        //overclock circuit chosen location
        locationIndex = (int)Mathf.Floor(Random.Range(0, overclockCircuitLocations.Length));
        //Debug.Log("Overclock Circuit is at ResearchLab[" + locationIndex + "]");
        overclockCircuitLocations[locationIndex].GetComponent<KeyItemLocation>().hasKeyItem = true;

        //no more random locations in this path
        
        //Boulders path

        //explosives chosen location
        locationIndex = (int)Mathf.Floor(Random.Range(0, explosivesLocations.Length));
        //Debug.Log("Explosive is at ExplosivesFactory[" + locationIndex + "]");
        explosivesLocations[locationIndex].GetComponent<KeyItemLocation>().hasKeyItem = true;

        //Remote Detonator chosen location
        locationIndex = (int)Mathf.Floor(Random.Range(0, remoteDetonatorLocations.Length));
        //Debug.Log("Remote Detonator is at Warehouse[" + locationIndex + "]");
        remoteDetonatorLocations[locationIndex].GetComponent<KeyItemLocation>().hasKeyItem = true;

        //Gate hack path

        locationIndex = (int)Mathf.Floor(Random.Range(0, quantumNotebookLocations.Length));
        //Debug.Log("Quantum Notebook is at Location[" + locationIndex + "]");
        quantumNotebookLocations[locationIndex].GetComponent<KeyItemLocation>().hasKeyItem = true;

        //Cypher frags are a little different; all have a fragment

    }



    //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\ MILITARY ROUTE FUNCTIONS \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\


    public void GotOverclockCircuit()
    {
        //sets bool to true
        hasOverclockCircuit = true;

        //shows it on screen for player
        UIManager.instance.ShowInformation("Got Overclock Circuit. \nNow get to the Power Plant!");

        //deactivates the current step of this route, the Research labs
        for (int i = 0; i < overclockCircuitLocations.Length; i++)
            overclockCircuitLocations[i].SetActive(false);

        //activates next step of this route, the power plant fuse box
        PowerPlantFuseBoxLocation.SetActive(true);
    }

    public void ExplodedPowerPlant()
    {
        hasExplodedPowerPlant = true;

        //shows it on screen for player
        UIManager.instance.ShowInformation("Exploded Power Plant! Now run to the Military Exit!");

        //deactivates current step of this route, power plant Fuse Box
        PowerPlantFuseBoxLocation.SetActive(false);

        //activates final step of current route, the military exit point
        MilitaryEscapeRoute.SetActive(true);

        //Run Power Plant explosion 'animation'
        StartCoroutine(PowerPlantExplosion());

        //TODO: Run 'military leaving checkpoint' cutscene (deactivate player while doing that)

        //Run "gate opening" co-routine
        gateGO.GetComponent<gateControl>().OpenGate();
    }

    IEnumerator PowerPlantExplosion()
    {
        //Gets each Fire element from the Power Plant and activate it in sequence and with respective explosion SFX
        float timeToWaitForNext = 1.0f;
        foreach (Transform explosionGO in PowerPlantExplosionParentGO.transform)
        {
            explosionGO.gameObject.SetActive(true);
            //time it takes to get to new one depends on current one's tag
            //TODO: If it's explosion, get corresponding sound to play
            switch (explosionGO.tag)
            {
                case "Explosion":
                    timeToWaitForNext = 0.3f;
                    break;
                case "ExplosionBig":
                    timeToWaitForNext = 0.5f;
                    break;
                case "ExplosionSmall":
                    timeToWaitForNext = 0.2f;
                    break;
                case "Fire":
                    timeToWaitForNext = 0.1f;
                    break;
                default:
                    break;
            }
            yield return new WaitForSeconds(timeToWaitForNext);
        }
    }





    //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\ BOULDER ROUTE FUNCTIONS \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

    public void GotExplosive()
    {
        hasExplosives = true;

        //shows it on screen for player
        UIManager.instance.ShowInformation("Got Explosives! Now look for a Remote Detonator!");


        //deactivate current step of this route, explosive locations
        for (int i = 0; i < explosivesLocations.Length; i++)
            explosivesLocations[i].SetActive(false);

        //activate next step of this route, remote control locations
        foreach (GameObject location in remoteDetonatorLocations)
        {
            location.SetActive(true);
        }

    }

    public void GotRemoteControl()
    {

        hasRemoteDetonator = true;

        //shows it on screen for player
        UIManager.instance.ShowInformation("Got the Remote Detonator! Now go blow up the Boulders in front of the North Exit!");

        //deactivate current step of this route, remote detonators
        foreach (GameObject location in remoteDetonatorLocations)
        {
            location.SetActive(false);
        }

        //activate next step of this route, explosion location
        BoulderExplosionLocation.SetActive(true);
    }

    public void ExplodedBoulders()
    {
        hasExplodedBoulders = true;

        //shows it on screen for player
        UIManager.instance.ShowInformation("Boulders out of the way, now get yourself out of there!");

        //deactivate current step of this route, boulder explosion location
        BoulderExplosionLocation.SetActive(false);

        //TODO: MAKE BOULDER EXPLOSION ANIMATION

        foreach (GameObject boulder in BouldersList)
            boulder.SetActive(false);


        //Activate final step of this route, boulder escape route
        BoulderEscapeRoute.SetActive(true);
    }


    //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\ HACK GATE ROUTE FUNCTIONS \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\


    public void GotNewCypherKey()
    {
        //shows it on screen for player
        UIManager.instance.ShowInformation("Got a Cypher Key!");
        cypherKeysCollected++;
    }

    public void GotQuantumComputer()
    {
        hasQuantumNotebook = true;

        //shows it on screen for player
        UIManager.instance.ShowInformation("Got Quantum Computer! So shiny! Now go get some Cypher Keys or go straight to hacking the Retina Gate!");

        //deactivate current step of this route, quantum computer locations
        for (int i = 0; i < quantumNotebookLocations.Length; i++)
            quantumNotebookLocations[i].SetActive(false);

        //activates de next step of this route, the cypher locations
        foreach(GameObject cypherRing in cypherKeyLocations)
        {
            cypherRing.SetActive(true);
        }

        //also activates the Gate Panel Location
        HackGatePanelLocation.SetActive(true);
    }

    public void StartedHack()
    {
        Debug.Log("Starting hack...");
        //once hack starts, deactivate all remaining cypher key locations
        foreach (GameObject cypherRing in cypherKeyLocations)
        {
            cypherRing.SetActive(false);
        }
        //defines hack duration based on how many cypher keys player got

        switch (cypherKeysCollected)
        {
            case 1:
                hackDuration = 90.0f;
                break;
            case 2:
                hackDuration = 75.0f;
                break;
            case 3:
                hackDuration = 45.0f;
                break;
            case 4:
                hackDuration = 30.0f;
                break;
            case 5:
                hackDuration = 10.0f;
                break;
            default://0
                hackDuration = 120.0f;//seconds
                break;
        }
        
        //after time is defined, start hack
        Debug.Log("Player collected " + cypherKeysCollected + " cypher key fragments, so hack will take " + hackDuration + " seconds.");
        StartCoroutine(UIManager.instance.WaitForHackToFinish(hackDuration));
    }

    

    public void HackFinished()
    {
        //shows it on screen for player
        UIManager.instance.ShowInformation("Hack Complete! Retina Gate open! Get outta there, you H4CK3R!");
        Debug.Log("Hack Complete! Electronic gate open!");
        //once hack finishes, deactivate current step, hack panel
        HackGatePanelLocation.SetActive(false);

        //finally, open gate and activate escape route
        HackGateController.GetComponent<HackGateControl>().OpenGate();

        HackGateEscapeRoute.SetActive(true);
    }

    
    public void EnteredEmptyLocation(string LocationType)
    {
        UIManager.instance.ShowInformation("Nothing of interest in this " + LocationType+ ". Try another location!");
    }


    public void ReachedEscapeRoute()
    {
        //Player won game; deactivate it and enemies, and show victory UI
        VictoryDefeatManagement.instance.PlayerWonGame();
    }

    //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\ SCORE FUNCTIONS \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

    public void AddScore(int scoreVal)
    {
        score += scoreVal;
        //Updates Score Value in UI
        scoreUIValGO.GetComponent<TextMeshProUGUI>().text = score.ToString();

    }



    // Use this for initialization
    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Destroyed " + gameObject.name + " because it's not singleton in CollectiblesManager");
            Destroy(this.gameObject);
        }
        else
        {

            //create singleton
            instance = this;
            ChooseLocationsToHaveItems();
            ActivateFirstStepLocations();
        }


        //locks cursor inside of game screen
        Cursor.lockState = CursorLockMode.Confined;
        //also makes it invisible
        Cursor.visible = false;
    }


    void Update()
    {

    }

}
