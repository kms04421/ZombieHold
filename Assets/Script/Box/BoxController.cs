using UnityEngine;
using System.Collections;
public class BoxController : Interactable
{
    private Animation anim;
    public GameObject MiniGame;
    [Header("상호작용 설정")]
    public float interactTime = 15f; // 몇 초 동안 버튼 눌러야 열리는지
    private Coroutine interactCoroutine;
    

    private void Awake()
    {
        anim = GetComponent<Animation>();
    }

    private IEnumerator InteractRoutine()
    {
        float currentTime = 0f;
        Debug.Log("작동");
        MiniGame.gameObject.SetActive(true);
        while (currentTime < interactTime)
        {
           
            // 버튼이 계속 눌러져있다고 가정 (외부에서 StopInteract 호출하면 종료)
            currentTime += Time.deltaTime;
            yield return null;
        }
        OpenBox();
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
