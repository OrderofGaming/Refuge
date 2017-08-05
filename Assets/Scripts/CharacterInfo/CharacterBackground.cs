using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBackground : MonoBehaviour
{
    // Background Info
    public enum Disadvantage
    {
        ADDICT = 0,
        BIPOLAR,
        PARAPLEGIC,
        CRIMINAL_RECORD,
        PHYSICAL_DISABILITY,
        MENTAL_ILLNESS,
        MINORITY
    }

    public List<Disadvantage> Disadvantages;

    // Level of Homelessness
    public enum LevelOfHomelessness
    {
        AT_RISK = 0,
        ALMOST_HOMELESS,
        HOMELESS
    }

    public LevelOfHomelessness LOH = LevelOfHomelessness.AT_RISK; // Level of Homelessness

}
