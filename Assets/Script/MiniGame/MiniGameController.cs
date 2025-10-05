using System;
using System.Collections.Generic;
using UnityEngine;
public class MiniGameController : Singleton<MiniGameController>
{
    [Header("상태 변환용 플레이어 컨트롤러")]
    [SerializeField] private PlayerController player;
    [Header("미니게임Ui")]
    [SerializeField] private MiniGameUI miniGameUI;
    [Header("문제 정답 (확인용)")]
    [SerializeField] private int[] answerNumbers;
    [Header("현재 상태")]
    public bool isWaiting = false;
    [Header("보상 지급 스크립트")]
    [SerializeField] private CompensationInventory compensation;
    //비밀번호 게임용 
    private int[] numbers; //현재 입력 배열
    private const int numberCount = 4; // 최대 입력수
    //비밀번호 게임용 end
    private Action onSuccessCallback; //성공 메서드 캐싱용
    private void Start()
    {
        numbers = new int[numberCount];       
        ResetPasswordGame();
    }
    #region 비밀번호 정답 맞추기 게임
    /// <summary>
    /// 새로운 정답 번호를 부여
    /// </summary>
    /// <returns></returns>
    public int[] newAnswerNumbers()
    {
        int[] newNumbers = new int[numberCount];
        System.Random rand = new System.Random();

        for (int i = 0; i < numberCount; i++)
        {
            newNumbers[i] = rand.Next(0, 10); // 0~9 범위
        }
        return newNumbers;
    }
    /// <summary>
    /// 숫자게임 정답과 일치하는지 비교 
    /// </summary>
    private void ChkAnswer()
    {
        int AnswerCount = numberCount;
        for (int i = 0; i < numberCount; i++)
        {
            if (answerNumbers[i] != numbers[i])
            {
                miniGameUI.MiniGamesUi[i].image.color = Color.red;
            }
            else
            {
                miniGameUI.MiniGamesUi[i].image.color = Color.blue;
                AnswerCount--;
            }
        }
        if(AnswerCount != 0)
        {
            Debug.Log("ChkAnswer");
            isWaiting = true;
            Invoke("ResetPasswordGame",1f);
        }
        else // 성공(나중에 메서드로)
        {
            SuccessMiniGame(0);
            ResetPasswordGame();
        }
    }
    /// <summary>
    /// 비밀번호 게임 초기화
    /// </summary>
    private void ResetPasswordGame()
    {
        for (int i = 0; i < numberCount; i++)
        {
            numbers[i] = -1;
            miniGameUI.SetText(i, -1);
        }
        isWaiting = false;
    }
    /// <summary>
    /// 클릭한 버튼의 값을 받아 순서대로 칸채우기
    /// </summary>
    /// <param name="num">받은 값</param>
    public void SetBtnNumber(int num)
    {
        if (isWaiting) return;
        for (int i = 0; i < numberCount; i++)
        {
            if (numbers[i] == -1)
            {
                numbers[i] = num;
                miniGameUI.SetText(i, num);
                if (i == 3)
                { 
                    ChkAnswer();
                    break;
                }
                break;
            }
        }
        
    }

    #endregion

    /// <summary>
    ///  게임 성공 메서드 
    /// </summary>
    public void SuccessMiniGame(int index)
    {
        isWaiting = false;
        miniGameUI.HideMiniGame(index);
        player.ChangeState(player.InventoryState);
        onSuccessCallback?.Invoke();       
    }
    /// <summary>
    /// 미니게임 요청받는 메서드(비밀번호 맞추기)
    /// </summary>
    /// <param name="ary">정답 배열</param>
    /// <param name="onSuccess">성공메서드 캐싱용</param>
    public void RequestMiniGame(int[] ary, Action onSuccess)
    {
        player.ChangeState(player.MenuState);
        answerNumbers = ary;
        miniGameUI.ShowMiniGame(0);
        onSuccessCallback = onSuccess;

    }
    /// <summary>
    /// 게임 성공보상 활성화
    /// </summary>
    /// <param name="isbool"></param>
    public void OnCompensation(List<Item> items)
    {
        compensation.AddItems(items);
        compensation.gameObject.SetActive(true);
    }
    /// <summary>
    /// 게임 성공보상 비활성화
    /// </summary>
    /// <param name="isbool"></param>
    public void OffCompensation()
    {
        //compensation.AddItem(items);
        compensation.gameObject.SetActive(false);
    }
    /// <summary>
    /// 미니게임창 닫기 
    /// </summary>
    /// <param name="index"></param>
    public void MiniGameExit(int index)
    {
        ResetPasswordGame();
        miniGameUI.HideMiniGame(index);
        player.ChangeState(player.InventoryState);
    }

}
