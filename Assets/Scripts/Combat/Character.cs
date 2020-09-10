using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Character : MonoBehaviour
{
    [SerializeField] private string _name = "";
    [SerializeField] [Range(1f, 10000f)] private float _maxHealth = 100f;
    [SerializeField] [Range(1f, 10000f)] private float _currentHealth = 100f;
    [SerializeField] private WeaponData _weapon = null;
    [SerializeField] private SpriteRenderer _portraitRenderer = null;
    [SerializeField] private SpriteRenderer _weaponRenderer = null;
    [SerializeField] private List<Sprite> _portraitSprites = new List<Sprite>();
    [SerializeField] private List<Sprite> _weaponSprites = new List<Sprite>();
    [SerializeField] private TextMeshPro _nameText = null;
    [SerializeField] private TextMeshPro _healthText = null;

    private Vector3 _origin;
    private Vector3 _moveOffset;
    private float _nextAttackTime = 0f;

    public BattleController Battle {get; set;}
    public Character Target {get; private set;}
    public int Team {get; private set;}
    
    public bool IsAlive
    {
        get
        {
            return _currentHealth > 0f;
        }
    }

    public bool ReadyToAttack
    {
        get
        {
            return Target != null && Time.time >= _nextAttackTime && IsAlive;
        }
    }
    
    public void Populate(CharacterData characterData, WeaponData weaponData, int team)
    {
        _origin = transform.position;
        _name = characterData.Name;
        _maxHealth = characterData.Health;
        _currentHealth = characterData.Health;
        _weapon = weaponData;
        _nameText.text = _name;
        Team = team;
        SetPortrait(characterData.Portrait);
        SetWeapon(weaponData.SpriteIndex);
        UpdateHealthText();
    }

    private void SetPortrait(int portraitIndex)
    {
        if (_portraitSprites.Count > 0)
        {
            portraitIndex = Mathf.Clamp(portraitIndex, 0, _portraitSprites.Count);
            _portraitRenderer.sprite = _portraitSprites[portraitIndex];
        }
    }

    private void SetWeapon(int spriteIndex)
    {
        if (_weaponSprites.Count > 0)
        {
            spriteIndex = Mathf.Clamp(spriteIndex, 0, _weaponSprites.Count);
            _weaponRenderer.sprite = _weaponSprites[spriteIndex];
        }
    }

    private void UpdateHealthText()
    {
        if (IsAlive)
        {
            _healthText.text = string.Format("{0} / {1}", Mathf.Floor(_currentHealth), Mathf.Floor(_maxHealth));
        }
        else
        {
            _healthText.text = "DEAD";
            _healthText.color = Color.red;
        }
    }

    public void SetTarget(Character target)
    {
        Target = (target != null && target.IsAlive) ? target : null;
        SetAttackCooldown();
    }

    private void SetAttackCooldown()
    {
        _nextAttackTime = Time.time + _weapon.AttackSpeed;
    }

    public void Damage(float damage)
    {
        if (IsAlive)
        {
            damage = Mathf.Abs(damage);
            _currentHealth = Mathf.Clamp(_currentHealth - damage, 0f, _maxHealth);
            UpdateHealthText();
            StartCoroutine(DamageRoutine());
        }
    }

    private IEnumerator DamageRoutine()
    {
        var shakeRange = 0.1f;
        var shakeTime = Time.time + 0.25f;
        while (Time.time < shakeTime)
        {
            transform.position = _origin + _moveOffset + new Vector3(Random.Range(-shakeRange, shakeRange), Random.Range(-shakeRange, shakeRange), 0f);
            yield return null;
        }
        transform.position = _origin + _moveOffset;
    }

    public void Attack()
    {
        if (ReadyToAttack)
        {
            if (!Target.IsAlive)
            {
                Target = null;
                return;
            }
            Target.Damage(_weapon.Damage);
            Target = null;
            StartCoroutine(AttackMoveRoutine());
        }
    }

    private IEnumerator AttackMoveRoutine()
    {
        _moveOffset = Vector3.zero;
        var moveRange = 0.25f;
        var moveSpeed = 1f;
        while (_moveOffset.y < moveRange)
        {
            _moveOffset.y += moveSpeed * Time.deltaTime;
            transform.position = _origin + _moveOffset;
            yield return null;
        }
        _moveOffset.y = moveRange;
        transform.position = _origin + _moveOffset;
        while (_moveOffset.y > 0f)
        {
            _moveOffset.y -= moveSpeed * Time.deltaTime;
            transform.position = _origin + _moveOffset;
            yield return null;
        }
        _moveOffset.y = 0f;
        transform.position = _origin + _moveOffset;
    }

    private void Update()
    {
        if (IsAlive && Target == null)
        {
            SetTarget(Battle.GetRandomTarget(Team));
        }
        if (ReadyToAttack)
        {
            Attack();
        }
    }

}
