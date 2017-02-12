using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowRotate : MonoBehaviour {

	public float distance = 10.0f;
	private float startRot;
	float randomOffset;

	void Start()
	{
		startRot = transform.rotation.eulerAngles.z;
		randomOffset = Random.Range (0.0f, Mathf.PI * 2.0f);
	}

	void FixedUpdate () {
		transform.rotation = Quaternion.Euler (new Vector3 (0.0f, 0.0f, startRot + Mathf.Sin (Time.time + randomOffset) * distance));
	}
}
