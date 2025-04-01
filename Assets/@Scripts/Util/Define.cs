using UnityEngine;
using System.Collections.Generic;

public class Define
{
    #region Input
    public const string MouseX = "Mouse X";
    public const string MouseY = "Mouse Y";
    public const string MouseScroll = "Mouse ScrollWheel";
    public const string Horizontal = "Horizontal";
    public const string Vertical = "Vertical";
    #endregion

    #region
    public const float PlayerMaxHp = 100f;
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

    #region Enum
    public enum ItemType : int
    {
        None = -1,
        Equipment, // ¿Â∫Ò
        Countable, // º“∫Ò
        Etc,
    }
    public enum WeaponType : int
    {
        None = -1,      // ∏«¡÷∏‘
        Rock,           // µπ∏Ê¿Ã
        Axe,            // µµ≥¢
        Sword,          // ∞À
        Pickaxe,        // ∞Ó±™¿Ã
    }
    public enum IngredientType : int
    {
        None = -1,
        Rock,
        Wood,
    }

    // Player Initiate«“ ∂ß ∞∞¿Ã √ ±‚»≠
    public static Dictionary<IngredientType, ItemData> IngredientData = new Dictionary<IngredientType, ItemData>();

    #endregion

    #region Scene
    public const string GameScene = "Game";
    public const string BossScene = "GameBoss";
    #endregion
}
