using UnityEngine;

public class TreeObject : EnvironmentObject
{
    public GameObject Wood;

    Transform _player;
    float _radius = 0.5f;

    void Start()
    {
        _durability = 50;
        _player = GameObject.FindGameObjectWithTag(Define.PlayerTag).transform.GetChild(0);
        //_player = GameObject.FindGameObjectWithTag(Define.PlayerTag).transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        // ���� �±� + ���� ����� ��
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
        Vector3 spownPos = new Vector3(Mathf.Cos(randAngle * Mathf.Deg2Rad) * _radius, 2f, Mathf.Sin(randAngle * Mathf.Deg2Rad) * _radius) + transform.position;
        GameObject wood = Instantiate(Wood, spownPos, Quaternion.identity);
        // wood.GetComponent<Rigidbody>().AddForce(new Vector3(spownPos.x, 0, spownPos.z) * 5f);
    }

    public override void DropItemsOnDestroy()
    {
        Debug.Log("Many Woods Dropped!!!");
    }
}
