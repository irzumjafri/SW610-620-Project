using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandlerMapping : ButtonHandler<object>
{
    public TextMeshProUGUI text;
    void Update()
    {
        text.text = Mapper.Instance.state.ToString();
    }
    public override void HandleClick()
    {
        State state = Mapper.Instance.state;
        if (state == State.Floor)
        {
            state = State.Initial;
            _ = UIManager.Instance.HandleButton(ButtonType.ChangeTab, "Initial");
        } else if(state == State.Mapping)
        {
            state = State.Finished;
            Mapper.Instance.SaveCurrentMap();
        } else if (state == State.Finished)
        {
            _ = UIManager.Instance.HandleButton(ButtonType.ChangeTab, "Initial");
            _ = UIManager.Instance.HandleButton(ButtonType.Reset, null);
            _ = UIManager.Instance.HandleButton(ButtonType.ChangeState, State.Initial);
            return;
        }
        _ = UIManager.Instance.HandleButton(ButtonType.ChangeState, state);
    }

}
