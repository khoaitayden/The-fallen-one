using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
public class Level2Loader : MonoBehaviour
{
    [SerializeField] private InputAction startAction;
    [SerializeField] private GameObject Instruction;
    [SerializeField] private SceneTransition sceneTransitionl;

    void Start()
    {
        Time.timeScale=0f;
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

    private void PressToStart(InputAction.CallbackContext context)
    {
        Time.timeScale=1f;
        startAction.performed -= PressToStart; 
        startAction.Disable();
        Instruction.SetActive(false);
    }
}
