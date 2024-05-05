using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonHandlerDeleteMap : ButtonHandler<string>
{
    public override void HandleClick()
    {
        _ = UIManager.Instance.HandleButton(ButtonType.DeleteMap, gameObject.transform.parent.name);
    }

}
