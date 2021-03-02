using UnityEngine;

public class Train_HealthBar : MonoBehaviour
{
    public GameObject[] Armor;
    public Vector2 force;
    [SerializeField] private TrainData _trainData = null;
    [SerializeField] private ParticleSystem _explosionParticle = null;
    public AudioClip[] steam;
    private AudioSource Audio;
    private bool isPlay = false;
    private bool isPlay1 = false;
    private bool isPlay2 = false;
    private bool isPlay3 = false;

    private void Start()
    {
        Audio = gameObject.AddComponent<AudioSource>();
        Audio.volume = 0.5f;
    }

    private void Update()
    {
        if(!isPlay)
        {
            if (_trainData.CurrentHealth < _trainData.MaxHealth * 0.75 && _trainData.CurrentHealth > _trainData.MaxHealth * 0.5)
            {
                HealthBarExp(0);
                Audio.clip = steam[0];
                Audio.Play();
                isPlay = true;

            }
        }
        if (!isPlay1)
        {
            if (_trainData.CurrentHealth > _trainData.MaxHealth * 0.25 && _trainData.CurrentHealth < _trainData.MaxHealth * 0.5)
            {
                HealthBarExp(1);
                Audio.clip = steam[1];
                Audio.Play();
                isPlay1 = true;
            }
        }
        if(!isPlay2)
        {
            if (_trainData.CurrentHealth < _trainData.MaxHealth * 0.25)
            {
                HealthBarExp(2);
                Audio.clip = steam[2];
                Audio.Play();
                isPlay2 = true;
            }
        }
        if(!isPlay3)
        {
            if (_trainData.CurrentHealth <= 0.5f)
            {
                HealthBarExp(3);
                Audio.clip = steam[3];
                Audio.Play();
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
        Destroy(particle.gameObject, particle.main.duration);
    }

}
