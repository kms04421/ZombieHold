using System.Collections.Generic;
using UnityEngine;
public class Box : Interactable
{
    private Animation anim;
    [Header("박스 현상태 확인용")]
    public bool Success = false;
    [Header("정답 배열")]
    [SerializeField] private int[] answerNumbers;
    [SerializeField] private List<Item> compensationItem;
    //미니게임 컨트롤러 캐싱
    private MiniGameController miniGame;
    private void Awake()
    {
        anim = GetComponent<Animation>();

    }
    private void OnEnable()
    {
        miniGame = MiniGameController.Instance;
        answerNumbers = miniGame.newAnswerNumbers();
        compensationItem = ItemDatabase.Instance.GetRandomItems();
    }
    private void OnDisable()
    {
        compensationItem.Clear();
    }
    private void StartMiniGame()
    {
        if (Success)
        {
            DestroyWithExplosion();
            return;
        }
        //미니게임 컨트롤러에 게임요청
        miniGame.RequestMiniGame(answerNumbers, OpenBox);

    }
    private void OpenBox()
    {
        miniGame.OnCompensation(compensationItem);
        Invoke("DestroyWithExplosion", 60);
        Success = true;
        anim.Play();
    }

    private void DestroyWithExplosion()
    {
        gameObject.SetActive(false);
    }
    protected override void OnStartInteract()
    {
        StartMiniGame();
    }
    protected override void OnStopInteract()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inputHandler.interactable = this;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (inputHandler.interactable == this)
            {
                inputHandler.interactable = null;
                miniGame.OffCompensation();
                StopInteract();
            }
        }
    }
}
