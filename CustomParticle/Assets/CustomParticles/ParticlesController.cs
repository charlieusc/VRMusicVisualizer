// ParticlesController script
// Copyright (C) 2013 Sergey Taraban <http://staraban.com>
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
 
[RequireComponent(typeof(ParticleSystem))]
public class ParticlesController : MonoBehaviour
{
	public Color    color = Color.white;
	public Vector3 segmentDir = new Vector3(1.0f, 0, 0);
	public float StartAngle = 0.0f;
	
	private Mesh m_Mesh;
	private int[] m_Indices;
	private Vector2[] m_UVs;

	ParticleSystem.Particle[] particles;
	ParticleSystem mPrSystem;
	int mCurrCount = 0;
	
	static int MaxValueToHide = 10000;
	
	private int mVertexCount = 0;
	void Awake ()
	{
		mPrSystem = GetComponent<ParticleSystem>();
		mPrSystem.Play();
	}
	
	void Start()
	{
	}
	
	void LateUpdate() {
		UpdateSegments();
	}	
	
	public void UpdateSegments()
	{
		if(particles == null) {
			return;
		}
		
		mPrSystem.SetParticles(particles, mPrSystem.particleCount);
	}

	public bool IsReadyToUse()
	{
		return mPrSystem.particleCount > 0;
	}

	public void SetVertexCount(int aCount)
	{	
		if(particles == null) {
			particles = new ParticleSystem.Particle[mPrSystem.particleCount];
			mPrSystem.GetParticles(particles);
		}
		
		if(mCurrCount > aCount) {
			for(int i = aCount; i < mPrSystem.particleCount; i++) {
				particles[i].position = new Vector3(0, MaxValueToHide, 0);
			}			
		}
		
		mCurrCount = aCount < mPrSystem.particleCount ? aCount : mPrSystem.particleCount;		
		
		if(aCount > mPrSystem.particleCount) {
			Debug.LogError("SetVertexCount(): vertex count > particles cache: "+mPrSystem.particleCount);
		}
	}
	
	public void SetPosition(int aIndex, Vector3 aPosition)
	{
		if(aIndex < 0 || aIndex >= mCurrCount) {
			return;
		}	
		
		particles[aIndex].position = aPosition;
		if(aIndex > 0) {
			Vector3 dir = (particles[aIndex].position - particles[aIndex-1].position);
			particles[aIndex].rotation = AngleAroundAxis(segmentDir, dir) + StartAngle;
			if(aIndex == 1) {
				dir = -(particles[0].position - particles[1].position);
				particles[0].rotation = AngleAroundAxis(segmentDir, dir) + StartAngle;
			} 
		}
	}

	public void SetColor(int aIndex, Color aColor)
	{
		if(aIndex < 0 || aIndex >= mCurrCount) {
			return;
		}	
		
		particles[aIndex].color = aColor;
	}

	public void SetAngle(int aIndex, Vector3 Angle)
	{
		if(aIndex < 0 || aIndex >= mCurrCount) {
			return;
		}	

		particles[aIndex].angularVelocity3D = Angle;
	}
	
	public void SetScale(int aIndex, float scale)
	{
		if(aIndex < 0 || aIndex >= mCurrCount) {
			return;
		}	

		particles[aIndex].size = scale;
	}
	
	static float AngleAroundAxis ( Vector3 dirA, Vector3 dirB ) {
		float res = Vector3.Angle(dirA, dirB);
		res *= Vector3.Dot(Vector3.up, Vector3.Cross(dirA, dirB)) < 0.0f ? -1.0f : 1.0f;
		return res;
	}
}