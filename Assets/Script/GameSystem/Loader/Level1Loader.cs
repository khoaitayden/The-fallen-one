using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class Level1Loader : MonoBehaviour
{
    [SerializeField] private GameObject stage3;
    [SerializeField] private GameObject stage1;
    [SerializeField] private InputAction startAction;
    [SerializeField] private Rigidbody2D playerrb;
    
    // Reference to the Bossstage script
    private Bossstage bossStageScript;
    
    // Static flag accessible to other scripts
    public static bool PreLoaded = false;

    void Awake()
    {
        // Start the optimized prewarm process
        StartCoroutine(OptimizedPrewarmStage3());
        playerrb.gravityScale = 0f;
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

    IEnumerator OptimizedPrewarmStage3()
    {
        if (stage3 != null)
        {
            // Get the Bossstage script before activation
            bossStageScript = stage3.GetComponent<Bossstage>();
            
            // 1. First activate the GameObject
            stage3.SetActive(true);
            
            // 2. Wait a short time to allow initialization but not animations
            yield return new WaitForSeconds(0.2f);
            
            // 3. Set the PreLoaded flag to signal that prewarm has completed
            PreLoaded = true;
            
            // 4. Wait another brief moment to ensure all components are initialized
            yield return new WaitForSeconds(0.3f);
            
            // 5. Deactivate the stage until needed
            stage3.SetActive(false);
            
            // Debug log to confirm prewarm is complete
            Debug.Log("Boss stage prewarmed successfully");
        }
        else
        {
            Debug.LogWarning("Stage3 reference is missing in Level1Loader");
        }
    }

    private void PressToStart(InputAction.CallbackContext context)
    {
        startAction.performed -= PressToStart; 
        startAction.Disable();
        playerrb.gravityScale = 3f;
        stage1.SetActive(true);
    }
}