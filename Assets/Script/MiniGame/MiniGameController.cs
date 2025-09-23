using UnityEngine;

public class MiniGameController : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private MiniGameUI gameUI;
    private int[] numbers;
    [SerializeField] private int numberCount;
    [SerializeField] private int[] answerNumbers;
    [SerializeField] public bool isSuccess = false;
    private void Start()
    {
        numbers = new int[numberCount];       
        Init();
    }
    public int[] newAnswerNumbers()
    {
        int[] newNumbers = new int[numberCount];
        System.Random rand = new System.Random();

        for (int i = 0; i < numberCount; i++)
        {
            newNumbers[i] = rand.Next(0, 10); // 0~9 ¹üÀ§
        }
        return newNumbers;
    }
    public void SetAnswerNumber(int[] ary)
    {
        answerNumbers = ary;
    }
    private void ChkAnswer()
    {
        int AnswerCount = numberCount;
        for (int i = 0; i < numberCount; i++)
        {
            if (answerNumbers[i] != numbers[i])
            {
                gameUI.MiniGamesUi[i].image.color = Color.red;
            }
            else
            {
                gameUI.MiniGamesUi[i].image.color = Color.blue;
                AnswerCount--;
            }
        }
        if(AnswerCount != 0)
        {
            Init();
        }
        else
        {
            isSuccess = true;
        }
    }
    private void Init()
    {
        for (int i = 0; i < numberCount; i++)
        {
            numbers[i] = -1; 
        }
    }
    private void OnEnable()
    {
        player.ChangeState(new MenuState(player));
    }

    private void OnDisable()
    {
        player.ChangeState(new NormalState(player));
    }

    public void SetBtnNumber(int num)
    {
        for (int i = 0; i < numberCount; i++)
        {
            if (numbers[i] == -1)
            {
                numbers[i] = num;
                gameUI.SetText(i, num);
                if (i == 3)
                {
                    Debug.Log(i);
                    ChkAnswer();
                    break;
                }
                break;
            }
        }
        
    }
}
