using UnityEngine;
using UnityEngine.UI;

public class LocalizeText : MonoBehaviour
{
    public Text Text;
    public int LineID;

    public void Start()
    {
        Text.text = SaveManager.Instance.Localization[LineID];
    }
}
