using UnityEngine;
using TMPro;

public class Tutotrial2 : MonoBehaviour
{
    public TextMeshProUGUI text;

    public void Show(string msg)
    {
        text.text = msg;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
