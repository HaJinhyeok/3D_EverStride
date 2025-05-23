using System.Collections.Generic;
using UnityEngine;

public class OrcController : BossController
{
    void Start()
    {
        _attackRange = 1.5f;
        _speed = 4f;
        _hp = Define.OrcMaxHp;
        _audioSources = GetComponents<AudioSource>();
    }

    public override void Die()
    {
        base.Die();
        foreach (KeyValuePair<Define.QuestName, Quest> quest in GameManager.Instance.QuestDictionary)
        {
            if (quest.Value.name == Define.QuestName.Orc)
            {
                quest.Value.AddNum(1);
            }
        }
    }
}
