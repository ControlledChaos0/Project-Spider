using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour
{
    private SmallMoveableObject heldObject;
    private Image thisSlotImage;

    private bool hovered;
    private Color transparent = new Color(1,1,1,0);
    private Color opaque = new Color(1, 1, 1, 1);
    public void initializeSlot()
    {
        thisSlotImage = GetComponent<Image>();
        setObject(null);
    }

    public SmallMoveableObject getObject()
    {
        return heldObject;
    }

    public void setObject(SmallMoveableObject curObject)
    {
        if (curObject == null)
        {
            thisSlotImage.sprite = null;
            thisSlotImage.color = transparent;
        } else
        {
            heldObject = curObject;
            thisSlotImage.sprite = curObject.icon;
            thisSlotImage.color = opaque;
        }
        
    }

}