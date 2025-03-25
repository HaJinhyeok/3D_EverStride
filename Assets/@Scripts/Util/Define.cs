using UnityEngine;

public class Define
{
    #region Input
    public const string MouseX = "Mouse X";
    public const string MouseY = "Mouse Y";
    public const string MouseScroll = "Mouse ScrollWheel";
    public const string Horizontal = "Horizontal";
    public const string Vertical = "Vertical";
    #endregion

    #region Animation
    public readonly static int Speed = Animator.StringToHash("Speed");
    public readonly static int Roll = Animator.StringToHash("Roll");
    public readonly static int Jump = Animator.StringToHash("Jump");
    public readonly static int IsDash = Animator.StringToHash("IsDash");
    public readonly static int IsGround = Animator.StringToHash("IsGround");
    public readonly static int IsClimbing = Animator.StringToHash("IsClimbing");
    public readonly static int IsNextCombo = Animator.StringToHash("IsNextCombo");
    public readonly static int IsAttacking = Animator.StringToHash("IsAttacking");
    public readonly static int AttackComboCount = Animator.StringToHash("AttackComboCount");
    public readonly static int WeaponTypeHash = Animator.StringToHash("WeaponType");
    #endregion

    #region Tag
    public const string PlayerTag = "Player";
    public const string WeaponTag = "Weapon";
    public const string GroundTag = "Ground";
    public const string ClimbableTag = "Climbable";
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
        None=-1,    // ∏«¡÷∏‘
        Rock,       // µπ∏Ê¿Ã
        Axe,        // µµ≥¢
        Sword,      // ∞À
    }
    #endregion
}
