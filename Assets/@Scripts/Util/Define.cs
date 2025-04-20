using UnityEngine;
using System.Collections.Generic;
using System;

public class Define
{
    #region GameObjects
    public const string GameUI = "UI_Game";
    public const string CraftUI = "UI_Craft";
    public const string CamAxis = "CamAxis";
    public const string PlateSetPrefix = "PlateSet1_";
    #endregion

    #region Input
    public const string MouseX = "Mouse X";
    public const string MouseY = "Mouse Y";
    public const string MouseScroll = "Mouse ScrollWheel";
    public const string Horizontal = "Horizontal";
    public const string Vertical = "Vertical";
    #endregion

    #region Constants
    public const float PlayerMaxHp = 100f;
    public const float PlayerRollStamina = 20f;
    public const float PotionHealing = 20f;

    public const float GolemMaxHp = 200f;
    public const float OrcMaxHp = 150f;
    #endregion

    #region Animator Parameter
    public readonly static int Speed = Animator.StringToHash("Speed");
    public readonly static int Roll = Animator.StringToHash("Roll");
    public readonly static int Jump = Animator.StringToHash("Jump");
    public readonly static int Die = Animator.StringToHash("Die");
    public readonly static int Drink = Animator.StringToHash("Drink");

    public readonly static int IsDash = Animator.StringToHash("IsDash");
    public readonly static int IsGround = Animator.StringToHash("IsGround");
    public readonly static int IsClimbing = Animator.StringToHash("IsClimbing");
    public readonly static int IsNextCombo = Animator.StringToHash("IsNextCombo");
    public readonly static int IsAttacking = Animator.StringToHash("IsAttacking");
    public readonly static int IsCombatMode = Animator.StringToHash("IsCombatMode");
    public readonly static int IsCombo = Animator.StringToHash("IsCombo");
    public readonly static int IsCrafting = Animator.StringToHash("IsCrafting");

    public readonly static int WeaponTypeHash = Animator.StringToHash("WeaponType");
    public readonly static int InteractionHash = Animator.StringToHash("IsPossibleInteraction");
    public readonly static int TakeDamage = Animator.StringToHash("TakeDamage");
    public readonly static int NoDamageMode = Animator.StringToHash("NoDamageMode");

    public readonly static int AttackComboCount = Animator.StringToHash("AttackComboCount");
    public readonly static int ComboCount = Animator.StringToHash("ComboCount");

    // NPC animator
    public readonly static int NPCHey = Animator.StringToHash("Hey");
    public readonly static int NPCWhy = Animator.StringToHash("Why");
    public readonly static int NPCClapping = Animator.StringToHash("Clapping");
    public readonly static int NPCPointing = Animator.StringToHash("Pointing");
    public readonly static int NPCSuggest = Animator.StringToHash("Suggest");
    #endregion

    #region Path
    public const string SlotPath = "Prefabs/Slot";
    public const string WeaponPath = "Prefabs/Weapon";
    public const string IngredientPath = "Prefabs/Ingredient";
    public const string ConsumptionPath = "Prefabs/Consumption";
    public const string PreviewCharacter = "Prefabs/Character/PreviewCharacter";
    public const string BossRaidAnimatorPath = "Animator/BossRaidAnimator";
    #endregion

    #region Tag
    public const string PlayerTag = "Player";
    public const string WeaponTag = "Weapon";
    public const string GroundTag = "Ground";
    public const string ClimbableTag = "Climbable";
    public const string EnemyTag = "Enemy";
    public const string EnemyHandTag = "EnemyHand";
    #endregion

    #region Layer
    public const string NPCMask = "NPC";
    #endregion

    #region Enum
    public enum ItemType : int
    {
        Etc = -1,
        Equipment,  // 장비
        Weapon,     // 무기
        Countable,  // 소비
    }
    public enum WeaponType : int
    {
        None = -1,      // 맨주먹
        Rock,           // 돌멩이
        Axe,            // 도끼
        Sword,          // 검
        Pickaxe,        // 곡괭이
        Hammer,         // 망치
    }

    public enum GameState : int
    {
        Default = 0,
        PlayerDie,
    }

    public enum IngredientType : int
    {
        None = -1,
        Rock,
        Wood,
        Ingot,
    }

    public enum EquipmentStatus : int
    {
        None,
        Base,
        Iron,
    }

    public enum EquipmentType : int
    {
        Helmet = 26,
        Shoulders,
        Chest,
        Gloves,
        Pants,
        Boots,
    }

    public enum QuestName
    {
        Wood,
        Golem,
        Orc,
        Villain,
    }

    // Player Initiate할 때 같이 초기화
    public static Dictionary<IngredientType, ItemData> IngredientData = new Dictionary<IngredientType, ItemData>();

    #endregion

    #region Scene
    public const string MainScene = "Main";
    public const string GameScene = "Game";
    public const string GolemScene = "GameGolem";
    public const string OrcScene = "GameOrc";
    #endregion

    #region Warning
    public const string NotEnoughMineral = "재료가 부족합니다";
    public const string NotReadyBoss = "아직 준비 중입니다";
    public const string DuplicatedQuest = "이미 진행 중인 퀘스트입니다";
    public const string AlreadyFullHP = "체력이 이미 가득 찼습니다";
    public const string DontDiscardEquipments = "장비 아이템은 버릴 수 없습니다";
    public const string AlreadyEquipSameGrade = "이미 같은 등급의 장비를 장착 중입니다";
    #endregion

    #region NPCtalking
    public readonly static string[] NPC_Quest_Wood =
        {
        "안녕? 날씨가 참 좋아, 그렇지?",
            "\n슬슬 날씨가 추워질 것 같은데," +
            "\n뗄감으로 쓸 나무조각 좀 모아다주겠어?"
    };
    public readonly static string[] NPC_Quest_Golem =
        {
        "안녕? 만나서 반가워.",
        "\n골렘 몬스터가 나타났는데,\n네가 좀 처치해줄 수 있을까?" +
            "\n푸른 포탈을 사용하면 골렘을 처치하러 갈 수 있어."
    };
    public readonly static string[] NPC_Quest_Orc =
    {
        "안녕! 시간 괜찮을까?",
        "\n못생긴 오크가 나타나서 사람들을 괴롭히는데,\n아주 골칫거리야."+
            "\n초록 포탈을 타면 오크를 잡으러 갈 수 있어! 도와줄래?"
    };

    public readonly static string[] NPC_TOO_BAD = { "이런...유감이군." };
    public readonly static string[] NPC_GOOD = { "좋아! 잘 부탁해!" };
    public readonly static string[] NPC_QUEST_ING = { "아직 퀘스트를 완료하지 못했구나." };
    public readonly static string[] NPC_QUEST_COMPLETE = { "퀘스트를 완료했구나!\n좋아, 보상을 줄게." };
    public readonly static string[] NPC_BYE = { "다음에 또 보자구." };
    #endregion

    #region QuestContext
    public const string GolemQuest = "골렘 처치";
    public const string WoodQuest = "나무조각 획득";
    #endregion

}
