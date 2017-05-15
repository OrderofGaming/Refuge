using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class UpdateLabel : MonoBehaviour {

    private Text m_sliderLabel;
	private Slider m_slider;

	void Awake () {
		m_slider = GetComponent<Slider> ();	
		m_sliderLabel = GetComponentInChildren<Text> ();

		m_slider.value = 0;

		UpdateValues (0);
	}
	
	public void UpdateValues(int a_newVal)
	{
        m_slider.value = a_newVal;
		m_sliderLabel.text = a_newVal.ToString () + "%";
	}
}