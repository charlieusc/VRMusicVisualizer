// RopeTape script
// Copyright (C) 2013 Sergey Taraban <http://staraban.com>
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticlesController))]
public class RopeTape : MonoBehaviour {

	ParticlesController mParticlesController = null;

	float timer = 0;
	float mPhase = 0;

	public float Amplitude = 10.0f;
	public float AmplitudeZ = 5.0f;
	public float XIncrement = 1.0f;
	public float dx = 1.0f;
	public float PhaseIncrement = 0.1f;
	public float Width = 1.0f;
	
	// Use this for initialization
	void Start () {
		mParticlesController = GetComponent<ParticlesController>();
	}
	
	// Update is called once per frame
	void Update () {
		mPhase += PhaseIncrement*Time.deltaTime*Mathf.PI;
		if(mParticlesController.IsReadyToUse()) 
		{
			timer += Time.deltaTime;
			int particlesNum = mParticlesController.GetComponent<ParticleSystem>().particleCount;
			mParticlesController.SetVertexCount(particlesNum);
			float t = 0;
			float fy = 0;
			float fz = 0;
			float dl = 0.01f;
			for(int i = 0; i < particlesNum; i++) 
			{
				fy = Amplitude * Mathf.Sin(t + mPhase);
				fz = AmplitudeZ * Mathf.Sin(t + mPhase);
				mParticlesController.SetPosition(i, transform.position + new Vector3(dx*i, fy, fz));
				t += XIncrement*0.001f*Mathf.PI;
				mParticlesController.SetScale(i, Width);
			}
		}
	}
}
