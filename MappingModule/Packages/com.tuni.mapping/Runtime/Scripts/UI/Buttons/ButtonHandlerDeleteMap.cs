

public class ButtonHandlerDeleteMap : ButtonHandler<string>
{
    public override void HandleClick()
    {
        _ = UIManager.Instance.HandleButton(ButtonType.DeleteMap, gameObject.transform.parent.name);
    }

}
