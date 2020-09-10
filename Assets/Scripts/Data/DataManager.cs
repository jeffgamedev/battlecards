using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField] private TextAsset _weaponJson = null;
    [SerializeField] private TextAsset _characterJson = null;

    private static DataManager _instance;

    private List<CharacterData> _characterData;
    private List<WeaponData> _weaponData;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
            LoadData();
        }
    }

    public static List<CharacterData> Characters
    {
        get
        {
            return _instance._characterData;
        }
    }

    public static List<WeaponData> Weapons
    {
        get
        {
            return _instance._weaponData;
        }
    }

    public static CharacterData GetRandomCharacter()
    {
        return Characters[Random.Range(0, Characters.Count)];
    }

    public static void Shuffle()
    {
        Characters.Shuffle();
        Weapons.Shuffle();
    }

    private void LoadData()
    {
        var weaponCollection = JsonUtility.FromJson<WeaponCollection>(_weaponJson.text);
        var characterCollection = JsonUtility.FromJson<CharacterCollection>(_characterJson.text);
        _characterData = characterCollection.Characters;
        _weaponData = weaponCollection.Weapons;
    }
}
