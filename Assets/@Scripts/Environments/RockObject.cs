using UnityEngine;

public class RockObject : EnvironmentObject
{
    public GameObject Rock;
    public GameObject HitEffect;

    float _radius = 0.5f;

    void Start()
    {
        _durability = 50;
        _audioSource = GetComponent<AudioSource>();
    }

    public override void DropItem()
    {
        float randAngle = Random.Range(0, 360f);
        Vector3 spownPos = new Vector3(Mathf.Cos(randAngle * Mathf.Deg2Rad) * _radius, 2f, Mathf.Sin(randAngle * Mathf.Deg2Rad) * _radius) + transform.position;
        GameObject rock = Instantiate(Rock, spownPos, Quaternion.identity);
    }

    public override void DropItemsOnDestroy()
    {
        for (int i = 0; i < 3; i++)
        {
            DropItem();
        }
    }

    public override void GetDamage(GameObject attacker, float damage, int bonus, Vector3 hitPos)
    {
        _durability -= damage * bonus;
        Instantiate(HitEffect, hitPos, Quaternion.Euler(transform.TransformDirection(Vector3.back)));
        //_audioSource.Play();
        AudioSource.PlayClipAtPoint(_audioSource.clip, hitPos);
        if (_durability > 0)
        {
            if (bonus > 1)
            {
                for (int i = 0; i < bonus; i++)
                {
                    DropItem();
                }
            }
            else
            {
                DropItem();
            }
        }
        else
        {
            DropItemsOnDestroy();
            Destroy(gameObject);
        }
    }
}
