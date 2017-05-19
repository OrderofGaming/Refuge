using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class CharacterController : MonoBehaviour
{
    public bool isMale;
    [Range(1, 6)]
    public int hairdo = 1;
    public Color shirtColor, hairColor, skinTone;
    [SerializeField]
    private GameObject maleHair, femaleHair;
    [SerializeField]
    private Sprite girlShirt, guyShirt;

    [Range(0, 4)]
    public int smile = 0;
    [SerializeField]
    private SpriteRenderer smileObject;
    [SerializeField]
    private Sprite[] smiles;
    [Range(0, 3)]
    public int sleeveLength;

    [SerializeField]
    private GameObject bars;

    public bool inJail = false;

    private List<Transform>  children;

    public void UpdateCharacter()
    {
        var shirtRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        shirtRenderer.sprite = isMale ? guyShirt : girlShirt;

		foreach (GameObject g in GameObject.FindGameObjectsWithTag("shirt"))
        {
			if (!children.Contains (g.transform))
				continue;
			
            var renderer = g.GetComponent<SpriteRenderer>();

            renderer.color = (shirtColor);
            if (g.name.Contains("arm"))
            {
                if (g.name.Contains(sleeveLength == 0 ? "NULL" : sleeveLength == 1 ? "shorter" : sleeveLength == 2 ? "short" : sleeveLength == 3 ? "long" : "NULL"))
                    renderer.enabled = true;
                else
                    renderer.enabled = false;
            }
        }
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("hair"))
		{
			if (!children.Contains (g.transform))
				continue;
            g.GetComponent<SpriteRenderer>().color = (hairColor);
        }
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("skin"))
		{
			if (!children.Contains (g.transform))
				continue;
            g.GetComponent<SpriteRenderer>().color = (skinTone);
        }

        var mhair = maleHair.GetComponentsInChildren<Transform>();
        var fhair = femaleHair.GetComponentsInChildren<Transform>();
        for (int i = 1; i < mhair.Length; i++)
        {
            if (int.Parse(mhair[i].name) != hairdo) // This is really hardcoded, but it's easy for me right now
            {
                mhair[i].gameObject.GetComponent<SpriteRenderer>().enabled = (false);
                fhair[i].gameObject.GetComponent<SpriteRenderer>().enabled = (false);
            }
            else
            {
                mhair[i].gameObject.GetComponent<SpriteRenderer>().enabled = (isMale ? true : false);
                fhair[i].gameObject.GetComponent<SpriteRenderer>().enabled = (isMale ? false : true);
            }
        }

        smileObject.sprite = smiles[smile];

		bars.SetActive (inJail);

		if (inJail) {
			foreach (GameObject g in GameObject.FindGameObjectsWithTag("shirt"))
			{
				if (!children.Contains (g.transform))
					continue;

				var renderer = g.GetComponent<SpriteRenderer>();

				Color color;
				if (ColorUtility.TryParseHtmlString ("#FFA011FF", out color)) {
					renderer.color = color;
				}

				if (g.name.Contains("arm"))
				{
					if (g.name.Contains("short"))
						renderer.enabled = true;
					else
						renderer.enabled = false;
				}
			}
		}
    }

	public void LookHappy(float duration = -1.0f)
	{
		StartCoroutine (ShowExpression (2, duration));
	}

	public void LookSad(float duration = -1.0f)
	{
		StartCoroutine (ShowExpression (4, duration));
	}

	public void LookOkay(float duration = -1.0f)
	{
		StartCoroutine (ShowExpression (3, duration));
	}

	public void LookSmiling(float duration = -1.0f)
	{
		StartCoroutine (ShowExpression (1, duration));
	}

	public void LookShocked(float duration = -1.0f)
	{
		StartCoroutine (ShowExpression (5, duration));
	}

	IEnumerator ShowExpression(int ID, float seconds)
	{
		smileObject.sprite = smiles[ID];

		//float time = Time.realtimeSinceStartup; bool infinite = (seconds < 0.0f ? true : false);
		//yield return new WaitUntil (() => ((time - Time.realtimeSinceStartup >= seconds) || infinite));
		yield return null;
	}

    void Awake()
    {
		children = new List<Transform>(GetComponentsInChildren<Transform> ());
        UpdateCharacter();
    }

    void OnValidate()
	{
		children = new List<Transform>(GetComponentsInChildren<Transform> ());
        UpdateCharacter();
    }
}
