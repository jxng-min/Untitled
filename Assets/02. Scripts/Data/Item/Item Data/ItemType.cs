[System.Flags]
public enum ItemType
{
    NONE = 0b0,
    SKILL = 0b1,

    Equipment_HELMET            = 0b10,
    Equipment_ARMORPLATE        = 0b100,
    Equipment_WEAPON            = 0b1000,
    Equipment_SHIELD            = 0b10000,
    Equipment_SHOES             = 0b100000,

    Etc                         = 0b1000000,
    Consumable                  = 0b10000000,
    Ingredient                  = 0b100000000,
    Quest                       = 0b1000000000,

    Money                       = 0b10000000000
}