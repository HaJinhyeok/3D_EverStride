using UnityEngine;
using System.Collections.Generic;

public class Define
{
    #region GameObjects
    public const string GameUI = "UI_Game";
    public const string CraftUI = "UI_Craft";
    #endregion

    #region Input
    public const string MouseX = "Mouse X";
    public const string MouseY = "Mouse Y";
    public const string MouseScroll = "Mouse ScrollWheel";
    public const string Horizontal = "Horizontal";
    public const string Vertical = "Vertical";
    #endregion

    #region Constants
    public const float PlayerMaxHp = 300f;
    public const float PlayerRollStamina = 20f;
    #endregion

    #region Animator
    public readonly static int Speed = Animator.StringToHash("Speed");
    public readonly static int Roll = Animator.StringToHash("Roll");
    public readonly static int Jump = Animator.StringToHash("Jump");
    public readonly static int Die = Animator.StringToHash("Die");
    public readonly static int IsDash = Animator.StringToHash("IsDash");
    public readonly static int IsGround = Animator.StringToHash("IsGround");
    public readonly static int IsClimbing = Animator.StringToHash("IsClimbing");
    public readonly static int IsNextCombo = Animator.StringToHash("IsNextCombo");
    public readonly static int IsAttacking = Animator.StringToHash("IsAttacking");
    public readonly static int WeaponTypeHash = Animator.StringToHash("WeaponType");
    public readonly static int InteractionHash = Animator.StringToHash("IsPossibleInteraction");
    public readonly static int IsCombatMode = Animator.StringToHash("IsCombatMode");
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
    public const string WeaponPath = "Prefabs/Weapon";
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
        None = -1,
        Equipment, // 장비
        Countable, // 소비
        Etc,
    }
    public enum WeaponType : int
    {
        None = -1,      // 맨주먹
        Rock,           // 돌멩이
        Axe,            // 도끼
        Sword,          // 검
        Pickaxe,        // 곡괭이
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
    }

    // Player Initiate할 때 같이 초기화
    public static Dictionary<IngredientType, ItemData> IngredientData = new Dictionary<IngredientType, ItemData>();

    #endregion

    #region Scene
    public const string GameScene = "Game";
    public const string BossScene = "GameBoss";
    #endregion

    #region Warning
    public const string NotEnoughMineral = "재료가 부족합니다";
    public const string NotReadyBoss = "아직 준비 중입니다";
    #endregion

    #region NPCtalking
    public readonly static string[] NPC_Quest_Wood = {
        "안녕? 날씨가 참 좋아, 그렇지?",
            "\n슬슬 날씨가 추워질 것 같은데," +
            "\n뗄감으로 쓸 나무조각 좀 모아다주겠어?" };
    public readonly static string[] NPC_Quest_Golem = {
        "안녕? 만나서 반가워.",
        "\n골렘 몬스터가 나타났는데,\n네가 좀 처치해줄 수 있을까?" +
            "\n푸른 포탈을 사용하면 골렘을 처치하러 갈 수 있어." };

    public readonly static string[] NPC_TOO_BAD = { "이런...유감이군." };
    public readonly static string[] NPC_GOOD = { "좋아! 잘 부탁해!" };
    #endregion
}
