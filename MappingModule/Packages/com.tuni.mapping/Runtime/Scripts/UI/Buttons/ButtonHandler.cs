using UnityEngine;
using UnityEngine.UI;

public abstract class ButtonHandler<T> : MonoBehaviour
{
    private Button button;
    public T Data;
    public ButtonType Type;

    public void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(HandleClick);
    }

    public virtual void HandleClick()
    {
        _ = UIManager.Instance.HandleButton(Type, Data);
    }

}
