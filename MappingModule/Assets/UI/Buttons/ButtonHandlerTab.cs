using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandlerTab : ButtonHandler<string>
{
    public override void HandleClick()
    {
        _ = UIManager.Instance.HandleButton(ButtonType.ChangeTab, Data);
    }
}
