

public class ButtonHandlerTab : ButtonHandler<string>
{
    public override void HandleClick()
    {
        _ = UIManager.Instance.HandleButton(ButtonType.ChangeTab, Data);
    }
}
