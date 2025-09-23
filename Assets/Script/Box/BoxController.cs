using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
public class BoxController : Interactable
{
    private Animation anim;
    public GameObject MiniGame;
    [Header("상호작용 설정")]
    private Coroutine interactCoroutine;
    public bool Success = false;
    private MiniGameController miniGameController;
    [SerializeField]private int[] answerNumbers;
    private void Awake()
    {
        anim = GetComponent<Animation>();
        miniGameController = MiniGame.GetComponent<MiniGameController>();
    }
    private void Start()
    {
        answerNumbers = miniGameController.newAnswerNumbers();
        miniGameController.SetAnswerNumber(answerNumbers);
    }
    private IEnumerator InteractRoutine()
    {
        if(Success)
        {
            yield break;
        }
        MiniGame.gameObject.SetActive(true);
        while (MiniGame.gameObject.activeSelf)
        {
            if (miniGameController.isSuccess)
            {
                Success = true;
                OpenBox();
                MiniGame.gameObject.SetActive(false);
                break;
            }
            yield return null;
        }
        interactCoroutine = null;
    }

    private void OpenBox()
    {
        anim.Play();
    }
    protected override void OnStartInteract()
    {
        if (interactCoroutine == null)
            interactCoroutine = StartCoroutine(InteractRoutine());
    }
    protected override void OnStopInteract()
    {
        if (interactCoroutine != null)
        {
            StopCoroutine(interactCoroutine);
            interactCoroutine = null;
        }
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
                MiniGame.gameObject.SetActive(false);
            }
        }
    }
}
