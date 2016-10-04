// ChristmasTree script
// Copyright (C) 2013 Sergey Taraban <http://staraban.com>
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using UnityEngine;
using System.Collections;

public class ChristmasTree : MonoBehaviour {

	ParticlesController mParticlesController = null;
	float mCurrPhaseOffset = 0.0f;

	public int MaxParticlesNum = 800;
	public bool showAllAprticles = false;
	public float Height = 0.1f;
	public float startRadius = 0.5f;
	public bool fixedLineStep = true;
	public float lineStep = 0.05f;
	public float angleStep = 0.5f;
	public float particleScaleMax = 4.0f;
	public float rotateSpeed = 1.0f;

	float timer = 0;
	float mOldCangeColorTime = -10.0f;
	
	// Use this for initialization
	void Start () {
		mParticlesController = GetComponent<ParticlesController>();
		//mParticlesController.SetVertexCount(50);
	}
	
	// Update is called once per frame
	void Update () {
		if(mParticlesController.IsReadyToUse()) 
		{
			timer += Time.deltaTime;
			int particlesNum = showAllAprticles ? MaxParticlesNum : (int)Mathf.Lerp(1, MaxParticlesNum, timer/8.0f);
			mParticlesController.SetVertexCount(particlesNum);
			mCurrPhaseOffset += rotateSpeed * Time.deltaTime;
			if(mCurrPhaseOffset >= 360.0f) 
			{
				mCurrPhaseOffset = 360 - mCurrPhaseOffset;
			}
			float dAlpha = 0;
			float dL = lineStep;
			float heightOffset = 0.01f;
			float offset = heightOffset;
			bool isChangeColor = ((int)timer - mOldCangeColorTime >= 1.0f);
			if(isChangeColor)
			{
				mOldCangeColorTime = timer;
			}
			for(int i = 0; i < particlesNum; i++) 
			{
				float R = offset * startRadius;
				float angle = dAlpha + mCurrPhaseOffset;
				if(angle >= 360.0f) {
					angle = 360.0f - angle;
				}

				float x = R * Mathf.Cos(angle);
				float z = R * Mathf.Sin(angle);
				mParticlesController.SetPosition(i, new Vector3(x, Height*i, z) + transform.position );

				float coeff = (float)i/(float)particlesNum;
				if(isChangeColor) {
					Vector3 color = new Vector3(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
					color = color.normalized;
					mParticlesController.SetColor(i, new Color(color.x, color.y, color.z, 1.0f));
				}

				mParticlesController.SetScale(i, Mathf.Lerp(1.0f, particleScaleMax, coeff));

				if(fixedLineStep) 
				{
					dAlpha += dL * 180.0f / (Mathf.PI * R);
				} 
				else
				{
					dAlpha += angleStep;
				}
				offset += heightOffset;
			}
		}
	}
}
