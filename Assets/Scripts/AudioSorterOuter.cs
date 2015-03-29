using UnityEngine;
using System.Collections;

public class AudioSorterOuter : MonoBehaviour {

	AudioSource musaks;

	// Use this for initialization
	void Start () 
	{
		musaks = this.GetComponent<AudioSource> ();
		musaks.PlayDelayed (0.5f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
