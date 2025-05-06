using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class Level1Loader : MonoBehaviour
{
    [SerializeField] private GameObject stage3;
    [SerializeField] private GameObject stage1;
    [SerializeField] private GameObject creature3;
    [SerializeField] private InputAction startAction;
    [SerializeField] private Rigidbody2D playerrb;
    [SerializeField] private UIController uIController;
    [SerializeField] private GameObject Instruction;
    public static bool PreLoaded=false;

    void Start()
    {
        playerrb.gravityScale=0f;
        if (PreLoaded) return;
        StartCoroutine(LoadScene());
        uIController.RestartGame();
        PreLoaded=true;
    }
    void OnEnable()
    {
        startAction.Enable();
        startAction.performed += PressToStart; 
    }
    void OnDisable()
    {
        startAction.performed -= PressToStart; 
        startAction.Disable();
    }

    IEnumerator LoadScene()
    {
        stage3.SetActive(true);
        stage1.SetActive(true);
        creature3.SetActive(true);
        new WaitForSeconds(0.5f);
        yield return null;
    }

    private void PressToStart(InputAction.CallbackContext context)
    {
        startAction.performed -= PressToStart; 
        startAction.Disable();
        playerrb.gravityScale=3f;
        stage1.SetActive(true);
        Instruction.SetActive(false);
    }
}