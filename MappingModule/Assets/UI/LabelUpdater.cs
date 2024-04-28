using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LabelUpdater : MonoBehaviour
{
    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (text != null)
        {
            switch (Mapper.Instance.state)
            {
                case State.Initial:
                    text.text = "To start mapping, press start mapping";
                    return;
                case State.LocatingAnchors:
                    text.text = "Locating anchors...";
                    break;
                case State.Mapping:
                    text.text = "Use the a button on the right controller to place walls and press 'Mapping' once you're done";
                    break;
                case State.Finished:
                    text.text = "Map saved, press finished to go back to main menu";
                    return;
            }
        }
    }
}
