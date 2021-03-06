using Interfaces;
using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageableType<float>
{
    // regular type 
    private SpriteRenderer _spriteDamageIndicator = null;
    private Color _defaultColor;
    private readonly WaitForSeconds _waitForSecondsDamage = new WaitForSeconds(0.05f);

    public SpriteRenderer SpriteColor { get { return _spriteDamageIndicator; } set { _spriteDamageIndicator=value; } }

    private bool reHealth=false;
    public bool ReSetHealth { get { return reHealth; } set {reHealth=value; } }
    private void Awake()
    {
        _spriteDamageIndicator = GetComponentInChildren<SpriteRenderer>();
        _defaultColor = _spriteDamageIndicator.color;
    }

    private float m_health = 0.0f;

    public float health
    {
        get { return m_health; }
        set { m_health = value; }
    }

    //Review code (Cyro): Refactoring this part because cause to much update call  (Unity Profile), just to check if the enemy has health
    // I moved this code inside the coroutine to improve our performance in gameplay
    //private void Update()
    //{
    //    if (reHealth)
    //    {
    //        _spriteDamageIndicator.color = _defaultColor;
    //        reHealth = false;
    //    }
    //    if (!IsAlive())
    //    {
    //        _spriteDamageIndicator.color = Color.gray;
    //    }
    //}
    public void Set()
    {
        _spriteDamageIndicator.color = _defaultColor;
    }

    public void TakeDamage(float takingDamage, DispenserData.Type damageType)
    {
        if (CompareTag("ShieldEnemy"))
        {
            if (!GetComponentInChildren<EnemyShieldHealth>().ShieldIsActive)
            {
                HpLogic(takingDamage, damageType);
            }
            else
            {
                ShieldLogic(takingDamage, damageType);
            }
        }


        if (CompareTag("Enemy"))
        {
            HpLogic(takingDamage, damageType);
        }
    }

    public bool IsAlive()
    {
        return health > 0.0f;
    }

    void HpLogic(float takingDamage, DispenserData.Type damageType)
    {
        health -= takingDamage;
        _ = StartCoroutine(DamageIndicator());
    }

    void ShieldLogic(float takingDamage, DispenserData.Type damageType)
    {

        if (damageType != DispenserData.Type.LaserBeam)
        {
            GetComponentInChildren<EnemyShieldHealth>().TakeDamage(takingDamage);
        }

    }


    private IEnumerator DamageIndicator()
    {
        _spriteDamageIndicator.color = Color.red;
        yield return _waitForSecondsDamage;
        _spriteDamageIndicator.color = IsAlive() ? _defaultColor : Color.gray;
    }

    public void DefaulSpriteColor() => _spriteDamageIndicator.color = _defaultColor;
}
