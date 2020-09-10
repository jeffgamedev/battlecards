using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private string _name;
    [SerializeField] [Range(0.1f, 1000f)] private float _damage = 5f;
    [SerializeField] [Range(0.1f, 60f)] private float _attackSpeed = 3f;
    [SerializeField] [Range(0f, 1000f)] private float _range = 1f;

    public void Populate(WeaponData weaponData)
    {
        _name = weaponData.Name;
        _damage = weaponData.Damage;
        _attackSpeed = weaponData.AttackSpeed;
        _range = weaponData.Range;
    }
}
