using UnityEngine;

public class RockObject : EnvironmentObject
{
    public GameObject Rock;

    Transform _player;

    void Start()
    {
        _durability = 50;
        _player = GameObject.FindGameObjectWithTag(Define.PlayerTag).transform.GetChild(0);
        //_player = GameObject.FindGameObjectWithTag(Define.PlayerTag).transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 웨폰 태그 + 공격 모션일 때
        if (other.CompareTag(Define.WeaponTag) && _player.GetComponent<Animator>().GetBool(Define.IsAttacking))
        {
            _durability -= 10;
            if (_durability > 0)
            {
                DropItems();

            }
            else
            {
                DropItemsOnDestroy();
                Destroy(gameObject);
            }
        }
    }

    public override void DropItems()
    {
        float randAngle = Random.Range(0, 360f);
        Vector3 spownPos = new Vector3(Mathf.Cos(randAngle * Mathf.Deg2Rad), 2f, Mathf.Sin(randAngle * Mathf.Deg2Rad)) + transform.position;
        GameObject rock = Instantiate(Rock, spownPos, Quaternion.identity);
        rock.GetComponent<Rigidbody>().AddForce(new Vector3(spownPos.x, 0, spownPos.z) * 5f);
    }

    public override void DropItemsOnDestroy()
    {
        Debug.Log("Many Rocks Dropped!!!");
        for (int i = 0; i < 3; i++)
        {
            DropItems();
        }
    }
}
