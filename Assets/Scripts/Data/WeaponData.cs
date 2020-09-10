using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class WeaponData
{
    public string Name;
    public float AttackSpeed;
    public float Damage;
    public float Range;
    public int SpriteIndex;
}

[System.Serializable]
public class WeaponCollection
{
    public List<WeaponData> Weapons;
}
