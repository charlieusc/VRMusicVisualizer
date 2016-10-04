using UnityEngine;
using System.Collections;

public class StarTravel : MonoBehaviour {


	ParticleSystem mPS = null;
	ParticleSystemRenderer mPSR = null;
	ParticleSystemRenderMode renderMode = ParticleSystemRenderMode.Stretch;
	ParticleSystem.VelocityOverLifetimeModule VelocityModule;
	ParticleSystem.ForceOverLifetimeModule ForceModule;
	//speedScale = -0.2 is a good start
	public float speedScale;
	[Range(0.001f,0.1f)]
	public float StrechStep = 0.001f;
	[Range(-0.3f,-2f)]
	public float MaxScale = -1.5f;
	[Range(3f,5f)]
	public float EngineStartTime = 3f;
	[Range(3f,7f)]
	public float TravelTime = 3f;
	public static float timer = 0f;
	public static bool enterTheStarTravelMode = true;
	// Use this for initialization
	void Start () {
		mPS = GetComponent<ParticleSystem> ();
		mPSR = GetComponent<ParticleSystemRenderer> ();
		mPSR.renderMode = renderMode;
		VelocityModule = mPS.velocityOverLifetime;
		ForceModule = mPS.forceOverLifetime;
	}

	// Update is called once per frame
	void Update () {
		mPSR.velocityScale = speedScale;
		drawStarTravel ();
	}
	void drawStarTravel(){
		timer += Time.deltaTime;
		if (enterTheStarTravelMode && timer>EngineStartTime) {
			if (speedScale > MaxScale) {
				speedScale -= StrechStep;
				if (ForceModule.enabled == false) {
					ForceModule.enabled = true;
				}
			}
			if (timer > TravelTime+EngineStartTime) {
				enterTheStarTravelMode = false;
			}
		} else {
			if (speedScale < -0.2f) {
				//Debug.Log (speedScale);
				speedScale += StrechStep;
				if (ForceModule.enabled == true) {
					ForceModule.enabled = false;
				}
			}
		}
	}
}
