using UnityEngine;
using System.Collections;

public class ParticlesManager : MonoBehaviour {

	public ParticleSystem perlinNoise;
	public ParticleSystem starTravel;
	public ParticleSystem musicVisualizer1;
	public ParticleSystem musicVisualizer2;
	public GameObject musicPlayer;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Teleport.particleChanged == true) {
			perlinNoise.gameObject.SetActive (false);
			starTravel.gameObject.SetActive (false);
			musicVisualizer1.gameObject.SetActive (false);
			musicVisualizer2.gameObject.SetActive (false);
			musicPlayer.gameObject.SetActive (false);
			switch (Teleport.particleSelected){
			case 0:
				musicPlayer.gameObject.SetActive (true);
				musicVisualizer1.gameObject.SetActive (true);
				break;

			case 1:
				musicPlayer.gameObject.SetActive (true);
				musicVisualizer2.gameObject.SetActive (true);
				break;
			case 2:
				starTravel.gameObject.SetActive (true);
				if (StarTravel.enterTheStarTravelMode == false) {
					StarTravel.enterTheStarTravelMode = true;
					StarTravel.timer = 0f;
				}
				break;
			case 3:
				musicPlayer.gameObject.SetActive (true);
				musicVisualizer1.gameObject.SetActive (true);
				musicVisualizer2.gameObject.SetActive (true);
				starTravel.gameObject.SetActive (true);
				if (StarTravel.enterTheStarTravelMode == false) {
					StarTravel.enterTheStarTravelMode = true;
					StarTravel.timer = 0f;
				}
				break;
			default:
				break;
			}
			Teleport.particleChanged = false;
		}
	}
}
