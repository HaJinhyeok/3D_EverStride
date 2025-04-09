using UnityEngine;

public class TreeObject : EnvironmentObject
{
    public GameObject Wood;
    public GameObject HitEffect;

    float _radius = 0.5f;

    void Start()
    {
        _durability = 50;
    }

    public override void DropItem()
    {
        float randAngle = Random.Range(0, 360f);
        Vector3 spownPos = new Vector3(Mathf.Cos(randAngle * Mathf.Deg2Rad) * _radius, 2f, Mathf.Sin(randAngle * Mathf.Deg2Rad) * _radius) + transform.position;
        GameObject wood = Instantiate(Wood, spownPos, Quaternion.identity);
    }

    public override void DropItemsOnDestroy()
    {
        Debug.Log("Many Woods Dropped!!!");
        for (int i = 0; i < 3; i++)
        {
            DropItem();
        }
    }

    public override void GetDamage(GameObject attacker, float damage, int bonus, Vector3 hitPos)
    {
        _durability -= damage * bonus;
        Instantiate(HitEffect, hitPos, Quaternion.Euler(transform.TransformDirection(Vector3.back)));
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
