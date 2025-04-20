using System.Collections.Generic;
using UnityEngine;

public class GolemController : BossController
{
    void Start()
    {
        _attackRange = 1.5f;
        _speed = 3f;
        _hp = Define.GolemMaxHp;
        _audioSources = GetComponents<AudioSource>();
    }

    public override void Die()
    {
        base.Die();
        foreach (KeyValuePair<Define.QuestName, Quest> quest in GameManager.Instance.QuestDictionary)
        {
            if (quest.Value.name == Define.QuestName.Golem)
            {
                quest.Value.AddNum(1);
            }
        }
    }
}
