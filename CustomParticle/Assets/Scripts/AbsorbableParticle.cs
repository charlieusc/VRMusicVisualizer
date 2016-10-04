using UnityEngine;
using System.Collections;

public class AbsorbableParticle : MonoBehaviour {
	public GameObject TargetPrimitive;

	[Range(0.0f, 1.0f)]
	public float ParticleSpeed;
	public int particleSet = 4;

	private Vector3[] m_targetVertices;
	private int tarVerNum;
	private ParticleSystem.Particle[] m_targetParticles;

	void Start () {
		m_targetVertices = TargetPrimitive.GetComponent<MeshFilter>().sharedMesh.vertices;
		tarVerNum = m_targetVertices.Length;
		m_targetParticles = new ParticleSystem.Particle[tarVerNum*particleSet];
	}

	void Update () {
		tarVerNum = m_targetVertices.Length;
		m_targetVertices = TargetPrimitive.GetComponent<MeshFilter>().sharedMesh.vertices;

		setTargetVertices ();
		for (int i = 0; i < particleSet; i++) {
			setParticles (i);
		}
		GetComponent<ParticleSystem> ().SetParticles (m_targetParticles, tarVerNum*particleSet);
		//GetComponent<ParticleSystem> ().SetParticles (m_targetParticles, m_targetParticles.Length);
	}

	public void setTargetVertices (){
		for(int i=0; i<tarVerNum; i++) {
			m_targetVertices[i] = TargetPrimitive.transform.TransformPoint(m_targetVertices[i]);
		}
	}

	public void setParticles(int i){
		for(int j=0; j<tarVerNum; j++) {

			m_targetParticles[i*tarVerNum+j].position = m_targetParticles[i*particleSet+j].position * (1f - (ParticleSpeed+0.1f*i)) + m_targetVertices[j] * (ParticleSpeed+0.1f*i);
			m_targetParticles[i*tarVerNum+j].color = new Color(1f - m_targetVertices[j].x % 1f, 0.2f + m_targetVertices[j].y % 0.8f, 0.5f + m_targetVertices[j].z % 0.5f, 255-50*i);
			m_targetParticles[i*tarVerNum+j].size = 0.05f;

			m_targetParticles[i*tarVerNum+j].lifetime = 10f;
			m_targetParticles[i*tarVerNum+j].startLifetime = 10f;
		}
	}
}