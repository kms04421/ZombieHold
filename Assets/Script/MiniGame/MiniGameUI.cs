using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class MiniGameUI : MonoBehaviour
{
    public List<GameObject> MiniGameList;
    public MiniGameImageText[] MiniGamesUi;
 
    public void SetText(int index, int num)
    {
        if( num == -1)
        {
            MiniGamesUi[index].TextMeshProUGUI.text ="";
        }
        else
        {
            MiniGamesUi[index].TextMeshProUGUI.text = num.ToString();
        }
          
    }
    public void ShowMiniGame(int i)
    {
        MiniGameList[i].gameObject.SetActive(true);
    }
    public void HideMiniGame(int i)
    {
        MiniGameList[i].gameObject.SetActive(false);
    }
}
[System.Serializable]
public struct MiniGameImageText
{
    public TextMeshProUGUI TextMeshProUGUI;
    public Image image;
}