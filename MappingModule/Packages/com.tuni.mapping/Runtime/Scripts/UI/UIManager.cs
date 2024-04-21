using System.Threading.Tasks;
using UnityEngine;
public enum ButtonType
{
    Reset,
    ChangeState,
    LoadMap,
    DeleteMap,
    ChangeTab
}

public class UIManager : Singleton<UIManager>
{

    public GameObject[] Screens;

    public async Task HandleButton(ButtonType type, object o)
    {
        if (type == ButtonType.Reset)
        {
            Mapper.Instance.RestartMapping();
        }
        if(type == ButtonType.ChangeState)
        {
            // o should be state
            // cast
            // could be done safer
            State state = (State)o;
            Mapper.Instance.ChangeStatus(state);
        }
        if(type == ButtonType.LoadMap)
        {
            // should be string
            string s = (string)o;
            await Mapper.Instance.TryLoadMap(s);
        }
        if(type == ButtonType.DeleteMap)
        {
            string s = (string)o;
            MapManager.Instance.DeleteMap(s);
        }
        if(type == ButtonType.ChangeTab)
        {

            // should be string
            string s = (string)o;
            for (int i = 0; i < Screens.Length; i++)
            {
                GameObject screen = Screens[i];
                if (screen.name == s)
                {
                    screen.SetActive(true);
                } else
                {
                    screen.SetActive(false);
                }
            }
        }
    }
}
