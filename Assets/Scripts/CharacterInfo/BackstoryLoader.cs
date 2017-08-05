using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BackstoryLoader : MonoBehaviour {

    private string[] introductions;
    private string[] stories;
    private string[] aspirations;

    public TextAsset introTexts;
    public TextAsset storyTexts;
    public TextAsset aspirationTexts;

	public UnityEngine.UI.Text biography;

    [Multiline]
    private string formatString = "{0}\n\n{1}\n\n{2}";

    // Use this for initialization
    void Start ()
    {

	}

    private string[] RemoveEmpty(string[] text)
    {
        return text.Where(t => !string.IsNullOrEmpty(t.Trim())).ToArray();
    }
	
	public string GetBackstory()
    {
        var backstory = string.Format(formatString,
                                introductions[Random.Range(0, introductions.Length)],
                                stories[Random.Range(0, stories.Length)],
                                aspirations[Random.Range(0, aspirations.Length)]
                );
        return backstory;
    }
}
