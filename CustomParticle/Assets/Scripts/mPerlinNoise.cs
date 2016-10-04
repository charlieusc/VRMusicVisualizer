using UnityEngine;
using System.Collections;
public class mPerlinNoise : MonoBehaviour {

	ParticlesController mParticlesController = null;
	mPerlinNoise MPS = null;

	public int MaxParticlesNum = 10000;
	public int particlesNum = 0;
	[Range(0.1f, 1f)]
	public float xStep = 0.1f;
	[Range(0.1f, 1f)]
	public float zStep = 0.1f;
	float oldXStep;
	float oldZStep;
	public float width = 10f;
	public float lenth = 10f;
	public float startX = -5f;
	public float startZ = -5f;
	[Range(1f, 5f)]
	public float Height = 1f;
	float oldHeight;


	private Vector3[] ParticlePosition;



	public bool showAllAprticles = false;
	float timer = 0;
	float mOldCangeColorTime = -10.0f;

	// Use this for initialization
	void Start () {
		ParticlePosition = new Vector3[MaxParticlesNum];
		mParticlesController = GetComponent<ParticlesController>();
		//MPS = GetComponent<mPerlinNoise> ();
		preCalculatePosition ();
		//mParticlesController.SetVertexCount(50);
		oldXStep = xStep;
		oldZStep = zStep;
		oldHeight = Height;
	}

	// Update is called once per frame
	void Update () {
		//Perlin Noise Surface
		drawPerlinNoise();

	}



	public void drawPerlinNoise(){
		if (oldXStep != xStep || oldZStep != zStep || oldHeight != Height) {
			preCalculatePosition();
			oldXStep = xStep;
			oldZStep = zStep;
			oldHeight = Height;
		}
		if(mParticlesController.IsReadyToUse())
		{
			timer += Time.deltaTime;
			//int particlesNum = MaxParticlesNum;
			mParticlesController.SetVertexCount(particlesNum);

			for(int i = 0; i < particlesNum; i++) 
			{
				ParticlePosition [i].y = (0.5f-Mathf.PerlinNoise (ParticlePosition [i].x + 5f + timer, ParticlePosition [i].z + 5f + timer))*Height;
				mParticlesController.SetPosition(i, ParticlePosition[i] + transform.position );
				mParticlesController.SetColor (i, new Color (0f, 1f, 0f,1f));
			}
		}
	}

	public void preCalculatePosition (){
		float x = -5f;
		float z = -5f;
		float y = 0f;
		int k = 0; 
		//y = Height;
		while (x < width + startX) {
			x += xStep;
			while (z < lenth + startZ) {
				z += zStep;
				if(k<MaxParticlesNum){
					//y = -Mathf.PerlinNoise (x+5f, z+5f)-5f;
					ParticlePosition [k] = new Vector3 (x, y*2, z);
					//Debug.Log (x + "," + y + "," + z + "," + i + "," + j);
					k++;
				}
			}
			z = -5f;
		}
		particlesNum = k;
	}
}
