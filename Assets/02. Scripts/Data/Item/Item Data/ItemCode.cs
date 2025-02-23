[System.Serializable]
public enum ItemCode
{
    NONE = -1,

    // 소비 아이템 0~
    SMALL_HP_POTION = 0,
    SMALL_MP_POTION = 1,

    // 장비 아이템 100~
    BASIC_HELMET = 100,
    BASIC_ARMORPLATE = 101,
    BASIC_WEAPON = 102,
    BASIC_SHIELD = 103,
    BASIC_SHOES = 104,

    // 재료 아이템 1000~
    MAGIC_SCROLL = 1000,
    IRON = 1001,
    GEM = 1002,
    MEAT = 1003,
    APPLE = 1004,

    // 스킬 10000~
    MAGICAL_PRAY = 10000,
    DESIRE_OF_WAR = 10001,
    PLANE_SMASH = 10002,

}