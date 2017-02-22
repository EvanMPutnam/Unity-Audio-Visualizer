using System.Collections;
using System.Collections.Generic;
using UnityEngine;





//Requires component and adds it if not available
[RequireComponent (typeof(AudioSource))]
public class AudioViz : MonoBehaviour {

	//Sample Audio size
	private const int sampleSize = 512;


	//Audio source and audio array with sample values
	public AudioSource audioVal;	//Defined in Unity
	private static float[] sampleArray = new float[sampleSize];


	//Shader stuff
	public Material materialVal;	//Defined in Unity


	//Cube object to instantiate and array to store objects.
	public GameObject cubeObject;	//Defined in Unity
	GameObject[] cubeArray = new GameObject[sampleSize];

	//Value to scale blocks by
	public float scaleVal = 3000f;

	//Whether to use the microphone or the audio of the object.  By default true.
	public bool micClip = true;




	// Use this for initialization
	void Start () {
		//Gets component on object
		audioVal = GetComponent<AudioSource> ();


		//Create cubes
		for (int i = 0; i < sampleSize; i++) {
			cubeArray[i] = (GameObject)Instantiate(cubeObject, new Vector3(i,0,0), transform.rotation);
		}


		//Uses internal microphone if the micClip val is true
		if (micClip == true) {
			audioVal.clip = Microphone.Start("Built-in Microphone", true, 10, 44100);
			audioVal.loop = true;
			//Empty loop to wait for microphone to initialize.
			while (!(Microphone.GetPosition (null) > 0)) {
			}
			audioVal.Play ();
		}


	}



	// Update is called once per frame
	void Update () {
		//Updates the audio spectrum array
		UpdateAudioSpectrum ();


		//Updates block size with new array size
		for (int i = 0; i < sampleSize; i++) {
			cubeArray [i].transform.localScale = new Vector3 (1, (sampleArray [i] * scaleVal)*2, 1);
			cubeArray[i].GetComponent<MeshRenderer>().material.SetColor("_Color",Color.Lerp(Color.green, Color.blue, Mathf.PingPong(Time.time, 1)));
		}

	}



	//Function to update the audio spectrum values
	void UpdateAudioSpectrum(){
		//Updates sampleArray with values of the spectrum
		audioVal.GetSpectrumData (sampleArray, 0, FFTWindow.Blackman);

	}


}
