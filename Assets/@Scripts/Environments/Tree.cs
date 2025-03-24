using UnityEngine;

public class Tree : EnvironmentObject
{
    void Start()
    {
        _durability = 50;
    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        // 웨폰 태그 + 공격 모션일 때
        if (other.CompareTag(Define.WeaponTag))
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
        Debug.Log("Wood Dropped!!");
    }

    public override void DropItemsOnDestroy()
    {
        Debug.Log("Many Woods Dropped!!!");
    }
}
