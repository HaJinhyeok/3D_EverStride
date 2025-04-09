using UnityEngine;
using System.Collections.Generic;
using System;

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
    public const float PlayerMaxHp = 100f;
    public const float PlayerRollStamina = 20f;
    public const float PotionHealing = 20f;

    public const float GolemMaxHp = 200f;
    #endregion

    #region Animator
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
    public const string IngredientPath = "Prefabs/Ingredient";
    public const string ConsumptionPath = "Prefabs/Consumption";
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
        Equipment, // ���
        Countable, // �Һ�
        Etc,
    }
    public enum WeaponType : int
    {
        None = -1,      // ���ָ�
        Rock,           // ������
        Axe,            // ����
        Sword,          // ��
        Pickaxe,        // ���
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

    public enum QuestName
    {
        Wood,
        Golem,
    }

    // Player Initiate�� �� ���� �ʱ�ȭ
    public static Dictionary<IngredientType, ItemData> IngredientData = new Dictionary<IngredientType, ItemData>();

    #endregion

    #region Scene
    public const string MainScene = "Main";
    public const string GameScene = "Game";
    public const string BossScene = "GameBoss";
    #endregion

    #region Warning
    public const string NotEnoughMineral = "��ᰡ �����մϴ�";
    public const string NotReadyBoss = "���� �غ� ���Դϴ�";
    public const string DuplicatedQuest = "�̹� ���� ���� ����Ʈ�Դϴ�";
    public const string AlreadyFullHP = "ü���� �̹� ���� á���ϴ�";
    #endregion

    #region NPCtalking
    public readonly static string[] NPC_Quest_Wood = {
        "�ȳ�? ������ �� ����, �׷���?",
            "\n���� ������ �߿��� �� ������," +
            "\n�������� �� �������� �� ��ƴ��ְھ�?" };
    public readonly static string[] NPC_Quest_Golem = {
        "�ȳ�? ������ �ݰ���.",
        "\n�� ���Ͱ� ��Ÿ���µ�,\n�װ� �� óġ���� �� ������?" +
            "\nǪ�� ��Ż�� ����ϸ� ���� óġ�Ϸ� �� �� �־�." };

    public readonly static string[] NPC_TOO_BAD = { "�̷�...�����̱�." };
    public readonly static string[] NPC_GOOD = { "����! �� ��Ź��!" };
    public readonly static string[] NPC_QUEST_ING = { "���� ����Ʈ�� �Ϸ����� ���߱���." };
    public readonly static string[] NPC_QUEST_COMPLETE = { "����Ʈ�� �Ϸ��߱���!\n����, ������ �ٰ�." };
    public readonly static string[] NPC_BYE = { "������ �� ���ڱ�." };
    #endregion

    #region QuestContext
    public const string GolemQuest = "�� óġ";
    public const string WoodQuest = "�������� ȹ��";
    #endregion

}
