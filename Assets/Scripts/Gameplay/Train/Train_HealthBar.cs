using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train_HealthBar : MonoBehaviour
{
    public GameObject[] Armor;
    public Vector2 force;
    [SerializeField] private TrainData _trainData = null;
    [SerializeField] private ParticleSystem _explosionParticle = null;
    private bool isPlay = false;
    private bool isPlay1 = false;
    private bool isPlay2 = false;
    private bool isPlay3 = false;
    public void Update()
    {
        if(!isPlay)
        {
            if (_trainData.CurrentHealth < _trainData.MaxHealth * 0.75 && _trainData.CurrentHealth > _trainData.MaxHealth * 0.5)
            {
                HealthBarExp(0);
                isPlay = true;
            }
        }
        if (!isPlay1)
        {
            if (_trainData.CurrentHealth > _trainData.MaxHealth * 0.25 && _trainData.CurrentHealth < _trainData.MaxHealth * 0.5)
            {

                HealthBarExp(1);
                isPlay1 = true;
            }
        }
        if(!isPlay2)
        {
            if (_trainData.CurrentHealth < _trainData.MaxHealth * 0.25)
            {

                HealthBarExp(2);
                isPlay2 = true;
            }
        }
        if(!isPlay3)
        {
            if (_trainData.CurrentHealth <= 0.5f)
            {
                HealthBarExp(3);
                isPlay3 = true;
            }
        }
    }

    private void HealthBarExp(int num)
    {

        ParticleSystem particle = Instantiate(_explosionParticle, Armor[num].transform.position, Quaternion.identity);
        Armor[num].GetComponent<Rigidbody2D>().AddForceAtPosition(force, new Vector2 (Armor[num].transform.position.x, -Armor[num].transform.position.y));
        Destroy(Armor[num],5.0f);
        particle.Play();
        Destroy(particle, particle.main.duration);
    }

}
