using UnityEngine;

public class RockObject : EnvironmentObject
{
    public GameObject Rock;

    Transform _player;
    float _radius = 0.5f;

    void Start()
    {
        _durability = 50;
        //_player = GameObject.FindGameObjectWithTag(Define.PlayerTag).transform.GetChild(0);        
    }

    public override void DropItem()
    {
        float randAngle = Random.Range(0, 360f);
        Vector3 spownPos = new Vector3(Mathf.Cos(randAngle * Mathf.Deg2Rad) * _radius, 2f, Mathf.Sin(randAngle * Mathf.Deg2Rad) * _radius) + transform.position;
        GameObject rock = Instantiate(Rock, spownPos, Quaternion.identity);        
    }

    public override void DropItemsOnDestroy()
    {
        Debug.Log("Many Rocks Dropped!!!");
        for (int i = 0; i < 3; i++)
        {
            DropItem();
        }
    }

    public override void GetDamage(GameObject attacker, float damage, Vector3 hitPos)
    {
        _durability -= damage;
        if (_durability > 0)
        {
            DropItem();

        }
        else
        {
            DropItemsOnDestroy();
            Destroy(gameObject);
        }
    }
}
