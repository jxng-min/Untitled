using UnityEngine;

public class InventorySample : MonoBehaviour
{
    [Header("인벤토리 메인")]
    [SerializeField] private InventoryMain mInventoryMain;

    [Header("획득할 아이템")]
    [SerializeField] private Item mHPItem, mManaItem;
    

    private void OnGUI()
    {
        if (GUI.Button(new Rect(20, 20, 300, 40), "체력포션 아이템 획득"))
        {
            mInventoryMain.AcquireItem(mHPItem);
        }

        if (GUI.Button(new Rect(400, 20, 300, 40), "마나포션 아이템 획득"))
        {
            mInventoryMain.AcquireItem(mManaItem);
        }        
    }
}