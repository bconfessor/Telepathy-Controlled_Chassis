using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Used to Store recorded voice clips for instructions
public class RadioVoiceScript : MonoBehaviour {

    public string[] dialogueTexts;

    public AudioClip[] audioClips;

    public GameObject dialogueTextGO;

    public AudioSource radioAudio;//gets the gameobject's inner audio source to generate the sounds

    /// <summary>
    /// Plays the sound clip and shows the text determined by the index given, then waits for end of audio and clears text screen
    /// </summary>
    /// <param name="soundTextIndex">Index of BOTH the sound and text.</param>
    /// <returns></returns>
    IEnumerator RunSoundAndShowText(int soundTextIndex)
    {
        dialogueTextGO.GetComponent<TextMeshProUGUI>().text = dialogueTexts[soundTextIndex];
        radioAudio.PlayOneShot(audioClips[soundTextIndex]);
        //wait for clip to finish, then erases text
        yield return new WaitForSeconds(audioClips[soundTextIndex].length+0.2f);

    }


    //TODO: Gonna need to slowly put each text part and sub-part into functions...

    public void PlayBeginningDialogue()
    {

    }


    public void PlayGotOverdriveSphereAudio()
    {

    }


    public void PlayDestroyedPowerPlantAudio()
    {

    }




    //plays the audio for when player reaches one exit and wins game
    public void PlayCompletedGameAudio()
    {

    }

	// Use this for initialization
	void Start () {
        radioAudio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
