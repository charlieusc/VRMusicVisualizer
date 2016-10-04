using UnityEngine;
using System.Collections;

public class spin : MonoBehaviour {
	public float spinSpeedz = 0.01f;
	public float spinSpeedy = 0.01f;
	public float spinSpeedx = 0.01f;
	float startAnglez = 0;
	float startAngley = 0;
	float startAnglex = 0;
	bool forwardz = true;
	bool forwardy = true;
	bool forwardx = true;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (startAnglez < 45 && forwardz == true) {
			gameObject.transform.eulerAngles = new Vector3 (startAnglex, startAngley, startAnglez += spinSpeedz);
		} else {
			forwardz = false;
		}

		if (startAnglez > -45 && forwardz == false) {
			gameObject.transform.eulerAngles = new Vector3 (startAnglex, startAngley, startAnglez -= spinSpeedz);
		} else {
			forwardz = true;
		}

		if (startAngley < 0 && forwardy == true) {
			gameObject.transform.eulerAngles = new Vector3 (startAnglex, startAngley += spinSpeedy, startAnglez);
		} else {
			forwardy = false;
		}

		if (startAngley > -45 && forwardy == false) {
			gameObject.transform.eulerAngles = new Vector3 (startAnglex, startAngley -= spinSpeedy, startAnglez);
		} else {
			forwardy = true;
		}

		if (startAnglex < 0 && forwardx == true) {
			gameObject.transform.eulerAngles = new Vector3 (startAnglex += spinSpeedx, startAngley, startAnglez);
		} else {
			forwardx = false;
		}

		if (startAnglex > -45 && forwardx == false) {
			gameObject.transform.eulerAngles = new Vector3 (startAnglex -= spinSpeedx, startAngley, startAnglez);
		} else {
			forwardx = true;
		}


	}
}
