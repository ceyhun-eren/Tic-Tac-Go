using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControl : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        GameManager.Instance.HandleShoot();
    }
}
