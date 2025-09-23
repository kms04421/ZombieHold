using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameUI : MonoBehaviour
{
    public MiniGameImageText[] MiniGamesUi;
 
    public void SetText(int index, int num)
    {
        MiniGamesUi[index].TextMeshProUGUI.text = num.ToString(); 
    }
}
[System.Serializable]
public struct MiniGameImageText
{
    public TextMeshProUGUI TextMeshProUGUI;
    public Image image;
}