using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Scrollbar))]
public class UpdateLabel : MonoBehaviour {

	private Text m_scrollBarLabel;

	private Scrollbar m_scrollBar;

	void Start () {
		m_scrollBar = GetComponent<Scrollbar> ();	
		m_scrollBarLabel = GetComponentInChildren<Text> ();

		m_scrollBar.value = 0.0f;

		UpdateValues (0.0f);
	}
	
	public void UpdateValues(float a_newVal)
	{
		m_scrollBar.size = a_newVal;
		m_scrollBarLabel.text = Mathf.RoundToInt(a_newVal * 100).ToString () + "%";
	}
}