using UnityEngine;
using System.Collections;

public class Spectrum : MonoBehaviour {

	// Instantiates a prefab in a circle

	public GameObject prefab;
	public int numberOfObjects = 20;
	public float radius = 5f;
	public GameObject[] cubes;

	void Start() {
		for (int i = 0; i < numberOfObjects; i++) {
			float angle = i * Mathf.PI * 2 / numberOfObjects;
			Vector3 pos = new Vector3(Mathf.Cos(angle), -5f/radius, Mathf.Sin(angle)+15f/radius) * radius;
			Instantiate(prefab, pos, Quaternion.identity);
		}
		cubes = GameObject.FindGameObjectsWithTag("cubes");
	}
	
	// Update is called once per frame
	void Update () {
		float[] spectrum = AudioListener.GetSpectrumData (1024, 0, FFTWindow.Hamming);
		//Debug.Log (spectrum.Length);
		for (int i = 0; i < numberOfObjects; i++) {
			Vector3 previousScale = cubes [i].transform.localScale;
			previousScale.y = spectrum [i] * 100;
			cubes [i].transform.localScale = previousScale;
		}
	}
}
