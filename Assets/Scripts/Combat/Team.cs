using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team
{
    public string Name;
    public List<Character> Characters;

    public Team(string name)
    {
        Name = name;
        Characters = new List<Character>();
    }

    public void DestroyTeam()
    {
        foreach (var chr in Characters)
        {
            chr.gameObject.SetActive(false);
            GameObject.Destroy(chr.gameObject);
        }
    }

    public Character GetRandomTarget()
    {
        var characters = new List<Character>();
        foreach (var chr in Characters)
        {
            if (chr.IsAlive)
            {
                characters.Add(chr);
            }
        }
        if (characters.Count > 0)
        {
            return characters[Random.Range(0, characters.Count)];
        }
        return null;
    }

    public bool IsAlive
    {
        get
        {
            foreach (var chr in Characters)
            {
                if (chr.IsAlive)
                {
                    return true;
                }
            }
            return false;
        }
    }

}