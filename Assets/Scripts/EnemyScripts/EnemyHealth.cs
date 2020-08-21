using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable<float>
{
    // regular type 
    public float health;




    public void TakeDamage(float takingDamage, DispenserData.Type damageType)
    {


        if (CompareTag("ShieldEnemy"))
        {
            if (!GetComponentInChildren<EnemyShieldHealth>().ShieldIsActive())
            {
                HpLogic(takingDamage, damageType);
            }
            else
            {
                ShieldLogic(takingDamage, damageType);
            }
        }

        if (CompareTag("ArmorEnemy"))
        {
            ArmorLogic(takingDamage, damageType);
        }

       if (CompareTag("ShieldArmorEnemy"))
       {
           if (GetComponentInChildren<EnemyShieldHealth>().ShieldIsActive())
           {
               ShieldLogic(takingDamage, damageType);
           }
           else
           {
               ArmorLogic(takingDamage, damageType);
           }
       }
        else if (true)
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
        Debug.Log(tag + "Lost " + takingDamage + "hp. Current health: " + health);
        if (health <= 0.0f)
        {
            Destroy(gameObject);
            Debug.Log("I will be back!");
        }
    }

    void ShieldLogic(float takingDamage, DispenserData.Type damageType)
    {

        if (damageType != DispenserData.Type.LaserBeam)
        {
            GetComponentInChildren<EnemyShieldHealth>().TakeDamage(takingDamage);
        }
        else
        {
            Debug.Log("ZaZaZa, I do not know what are you doing!!!");
        }
    }
    void ArmorLogic(float takingDamage, DispenserData.Type damageType)
    {
        if (!GetComponentInChildren<EnemyArmorHealth>().ArmorIsActive() || damageType == DispenserData.Type.Railgun)
        {
            HpLogic(takingDamage, damageType);
        }
        else
        {
            GetComponentInChildren<EnemyArmorHealth>().TakeDamage(takingDamage);
        }

    }


}
