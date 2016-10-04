using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicParticle2 : MonoBehaviour {

	ParticlesController mParticlesController = null;

	public int MaxParticlesNum = 1000;
	public int particlesNum = 0;
	[Range(1f, 10f)]
	public float Height = 1f;
	float[] spectrum;
	public float heightDcreaseSpeed = 0.01f;
	[Range(0,1)]
	public float ColorScaler1 = 0.5f;
	[Range(0,1)]
	public float ColorScaler2 = 0.5f;

	private List<List<Vector3>> ParticlePosition;
	private List<Vector3> verticleParticles;
	private List<List<Vector3>> ParticlePositionMax;
	private List<Vector3> verticleParticlesMax;
	private List<Vector3> m_targetVerticesList;
	private Vector3[] m_targetVertices;
	public GameObject TargetPrimitive;
	public static float timer = 0f;
	private bool timeForward = true;
	
	// Use this for initialization
	void Start () {
		m_targetVerticesList = new List<Vector3> ();
		m_targetVertices = TargetPrimitive.GetComponent<MeshFilter> ().sharedMesh.vertices;

		for (int j = 0; j < m_targetVertices.Length; j++) {
			m_targetVerticesList.Add(m_targetVertices [j]);
		}

		m_targetVerticesList.Sort((a, b) => a.y.CompareTo(b.y));
		int i = 0;
		float tempY = m_targetVerticesList [i].y;

		ParticlePosition = new List<List<Vector3>>();
		verticleParticles = new List<Vector3> ();
		ParticlePositionMax = new List<List<Vector3>>();
		verticleParticlesMax = new List<Vector3> ();
		while (i < m_targetVerticesList.Count) {
			if (m_targetVerticesList [i].y - tempY<0.0001f) {
				verticleParticles.Add (m_targetVerticesList [i]);
				verticleParticlesMax.Add (m_targetVerticesList [i]);
				i++;
			} else {
				tempY = m_targetVerticesList [i].y;
				ParticlePosition.Add (verticleParticles);
				ParticlePositionMax.Add (verticleParticlesMax);
				verticleParticles = new List<Vector3> ();
				verticleParticlesMax = new List<Vector3> ();
				//Debug.Log (i);
			}
		}
		ParticlePosition.Add (verticleParticles);
		ParticlePositionMax.Add (verticleParticlesMax);
		//Debug.Log (i);

		particlesNum = i + 1;
		Debug.Log (particlesNum);
		mParticlesController = GetComponent<ParticlesController>();
	}

	// Update is called once per frame
	void Update () {
		drawParticle ();
	}

	public void drawParticle(){
		if (timeForward == true) {
			timer += Time.deltaTime;
		} else {
			timer -= Time.deltaTime;
		}
		spectrum = AudioListener.GetSpectrumData (1024, 0, FFTWindow.Hamming);

		if(mParticlesController.IsReadyToUse())
		{
			//Debug.Log ("Ready!");
			mParticlesController.SetVertexCount(particlesNum);
			int row = ParticlePosition.Count;
			int m = 0;
			for (int i = 0; i < row; i++) {
				int col = ParticlePosition [i].Count;
				//Debug.Log (row+","+col+","+m);
				for (int j = 0; j < col; j++) {
					setPosition (i, j, m);

					m++;
				}
			}
		}
	}
	public void setPosition(int i, int j, int m){
		int k = i;
		if (k > 1023)
			k = 1023;
		//Debug.Log (i+":"+ParticlePosition[i][j].x + "," + j +":"+ParticlePosition[i][j].z);
		float Scaler = 1+spectrum[k*3]*Height;
		//ParticlePosition [i] [j] = new Vector3(ParticlePosition[i][j].x * newX, ParticlePosition[i][j].y, ParticlePosition[i][j].z * newZ);
		if (Mathf.Pow(ParticlePositionMax [i] [j].x, 2)< Mathf.Pow(ParticlePosition [i] [j].x*Scaler, 2)) {
			ParticlePositionMax [i] [j] = new Vector3(ParticlePosition[i][j].x*Scaler, ParticlePositionMax[i][j].y, ParticlePosition[i][j].z*Scaler);

		}
		ParticlePositionMax [i] [j] = new Vector3(ParticlePositionMax[i][j].x*(1-heightDcreaseSpeed), ParticlePositionMax[i][j].y, ParticlePositionMax[i][j].z*(1-heightDcreaseSpeed));
		float colorFactor1 = Mathf.Pow(ParticlePositionMax [i] [j].y*2,2);
		float colorFactor2 = Mathf.Pow (ParticlePositionMax [i] [j].x*2, 2);
		mParticlesController.SetPosition(m, ParticlePositionMax[i][j]  + transform.position);
		//mParticlesController.SetAngle (i*maxRow + j, Random.insideUnitSphere);
		if (timer > 10) {
			timeForward = false;
		} else if (timer < 0) {
			timeForward = true;
		}
		Color ShiftColor = new Color(0,0,(timer/10f),0);
		//mParticlesController.SetColor (m, ShiftColor);
		mParticlesController.SetColor (m, new Color (0.5f, colorFactor1*ColorScaler1*(spectrum[2]+spectrum[3]+spectrum[4])*30, colorFactor1*ColorScaler2, Mathf.Min(colorFactor1,colorFactor2))+ShiftColor);
	}
}
