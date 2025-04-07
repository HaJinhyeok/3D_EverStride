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
    public const string NPCHello = "안녕? 만나서 반가워.";
    #endregion
}
