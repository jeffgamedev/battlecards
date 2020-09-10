using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CharacterData
{
    public string Name;
    public float Health;
    public int Portrait;
}

[System.Serializable]
public class CharacterCollection
{
    public List<CharacterData> Characters;
}
