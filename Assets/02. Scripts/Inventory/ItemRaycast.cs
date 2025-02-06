using UnityEngine;

public class ItemRaycast : MonoBehaviour
{
    private RaycastHit m_hit;
    private bool m_is_pick_up_active = false;
    private ItemPickUp m_current_item;

    [Header("레이어 마스크")]
    [SerializeField] private LayerMask m_layer_mask;


    [Header("레이 캐스팅 거리")]
    [SerializeField] private float m_ray_distance;

    [Header("레이를 발사하는 카메라")]
    [SerializeField] private Camera m_ray_camera;

    [Header("인벤토리 UI")]
    [SerializeField] private InventoryMain m_inventory;

    private void Update()
    {
        CheckItem();

        if(m_is_pick_up_active)
        {
            TryPickItem();
        }
    }

    private void TryPickItem()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(m_current_item.Item.Type > ItemType.NONE)
            {
                InventorySlot[] all_items = m_inventory.GetAllItems();

                int count = 0;
                for(count = 0; count < all_items.Length; count++)
                {
                    if(all_items[count].Item == null)
                    {
                        break;
                    }

                    if(all_items[count].Item.ID == m_current_item.Item.ID && all_items[count].Item.Overlap)
                    {
                        break;
                    }
                }

                if(count == all_items.Length)
                {
                    return;
                }

                TryPickUp();
                ItemInfoDisappear();
            }
        }
    }

    private void CheckItem()
    {
        Debug.DrawRay(m_ray_camera.transform.position, m_ray_camera.transform.forward * m_ray_distance, Color.yellow);
        if(Physics.Raycast(m_ray_camera.transform.position, m_ray_camera.transform.forward, out m_hit, m_ray_distance, m_layer_mask))
        {
            if(m_hit.transform.CompareTag("Item"))
            {
                ItemPickUp raycasted_item = m_hit.transform.GetComponent<ItemPickUp>();

                if(m_current_item == raycasted_item)
                {
                    return;
                }

                m_current_item = raycasted_item;
                m_is_pick_up_active = true;

                return;
            }
            else
            {
                ItemInfoDisappear();
            }

        }
        else
        {
            ItemInfoDisappear();
        }
    }

    private void ItemInfoDisappear()
    {
        m_is_pick_up_active = false;

        m_current_item = null;
    }

    private void TryPickUp()
    {
        if(m_is_pick_up_active)
        {
            if(m_current_item.Item.Type != ItemType.NONE)
            {
                m_inventory.AcquireItem(m_current_item.Item);
                Destroy(m_current_item.gameObject);
            }

            ItemInfoDisappear();
        }
    }
}
