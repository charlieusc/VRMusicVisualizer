using UnityEngine;
using System.Collections;

public class MusicParticle : MonoBehaviour {

	ParticlesController mParticlesController = null;

	public int MaxParticlesNum = 1000;
	public int maxRow = 10;
	public int maxCol = 10;
	public int particlesNum = 0;
	public float width = 10f;
	public float lenth = 10f;
	public float startX = -5f;
	public float startZ = -5f;
	[Range(1f, 100f)]
	public float Height = 1f;
	float oldHeight;
	float[] spectrum;
	public float heightDcreaseSpeed = 0.01f;
	public bool rainBowColor = false;

	private Vector3[][] ParticlePosition;
	private Vector3[][] ParticlePositionMax;

	// Use this for initialization
	void Start () {
		ParticlePosition = new Vector3[maxRow][];
		ParticlePositionMax = new Vector3[maxRow][];
		for (int i = 0; i < maxRow; i++) {
			ParticlePosition [i] = new Vector3[maxCol];
			ParticlePositionMax [i] = new Vector3[maxCol];
		}
		particlesNum = maxRow * maxCol;
		mParticlesController = GetComponent<ParticlesController>();
		preCalculatePosition ();
		oldHeight = Height;
	}
	
	// Update is called once per frame
	void Update () {
		drawParticle ();
	}

	public void drawParticle(){
		spectrum = AudioListener.GetSpectrumData (1024, 0, FFTWindow.Hamming);
		if ( oldHeight != Height) {
			preCalculatePosition();
			oldHeight = Height;
		}

		if(mParticlesController.IsReadyToUse())
		{
			mParticlesController.SetVertexCount(particlesNum);

			int rowS = 0, rowE = maxRow-1;
			int colS = 0, colE = maxCol-1;
			int leyer = 1;
			while(colS<colE && rowS<rowE){
				for(int i=colS; i<colE; i++){
					setPosition (rowS, i, colE, rowE, leyer);
				}
				for(int i=rowS; i<rowE; i++){
					setPosition (i, colE, colE, rowE, leyer);
				}
				//go backwards, i start form 0
				for(int i=0; i<colE-colS; i++){
					setPosition (rowE, colE-i, colE, rowE, leyer);
				}
				for(int i=0; i<rowE-rowS; i++){
					setPosition (rowE-i, colS, colE, rowE, leyer);
				}
				colS++;
				rowS++;
				colE--;
				rowE--;
				leyer+=1;
			}
			//process the situation when only one row or col left
			if(rowS==rowE){
				for(int i=colS; i<=colE; i++){
					setPosition (rowS, i, colE, rowE, leyer);
				}
			}else if(colS==colE){
				for(int i=rowS; i<=rowE; i++){
					setPosition (i, colS, colE, rowE, leyer);
				}
			}

		}
	}
	public void setPosition(int i, int j, int colE, int rowE, int leyer){
		int k = Mathf.Max(colE,rowE);
		if (k>250)
			k = 250;
		//Debug.Log (i+":"+ParticlePosition[i][j].x + "," + j +":"+ParticlePosition[i][j].z);
		ParticlePosition [i][j].y = (spectrum[k*2]+spectrum[k*2+1])*Height*leyer;
		if (ParticlePositionMax [i] [j].y < ParticlePosition [i] [j].y) {
			ParticlePositionMax [i] [j].y = ParticlePosition [i] [j].y;
		}
		ParticlePositionMax [i] [j].y -= heightDcreaseSpeed;
		float colorFactor = ParticlePositionMax [i] [j].y/leyer*3;
		mParticlesController.SetPosition(i*maxRow + j, ParticlePositionMax[i][j] + transform.position );
		//mParticlesController.SetAngle (i*maxRow + j, Random.insideUnitSphere);
		mParticlesController.SetColor (i*maxRow + j, new Color (colorFactor*3, colorFactor*3, 0, colorFactor*7));
		if (rainBowColor == true) {
			float r, g, b;
			if (leyer <= 2) {
				r = 1f;
				g = 0;
				b = 0;
			} else if (leyer <= 4) {
				r = 1f;
				g = 127f / 255f;
				b = 0;
			} else if (leyer <= 6) {
				r = 1;
				g = 1;
				b = 0;
			} else if (leyer <= 8) {
				r = 0;
				g = 1;
				b = 0;
			} else if (leyer <= 10) {
				r = 0;
				g = 0;
				b = 1;
			} else if (leyer <= 12) {
				r = 75f / 255f;
				g = 0;
				b = 130f / 255f;
			} else if (leyer <= 14) {
				r = 143f / 255f;
				g = 0;
				b = 1;
			} else {
				r = 1f;
				g = 0;
				b = 0;
			}
			mParticlesController.SetColor (i*maxRow + j, new Color(r,g,b, colorFactor*7));
		}
		//Debug.Log (spectrum [k]);
	}

	public void preCalculatePosition(){
		for(int i = 0; i < maxRow; i++)
		{
			for (int j = 0; j < maxCol; j++) {
				float x = startX + i * ((float)width/maxRow);
				float z = startZ + j * ((float)lenth/maxCol);
				float y = 1f*Height;
				ParticlePosition [i] [j] = new Vector3 (x, y, z);
				ParticlePositionMax [i] [j] = new Vector3 (x, y-100f, z);
			}
		}
	}
}
