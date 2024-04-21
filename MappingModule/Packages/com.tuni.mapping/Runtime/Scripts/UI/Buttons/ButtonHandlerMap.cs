using TMPro;

public class ButtonHandlerMap : ButtonHandler<string>
{
    public TextMeshProUGUI text;
    void Update()
    {
        text.text = gameObject.name;
    }
    public override void HandleClick()
    {
        _ = UIManager.Instance.HandleButton(ButtonType.LoadMap, gameObject.name);
    }

}
