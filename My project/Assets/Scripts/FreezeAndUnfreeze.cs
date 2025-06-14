using UnityEngine;

public class FreezeAndUnfreeze : MonoBehaviour
{

    PlayerController playerController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FreezeMovement()
    {
        playerController.FreezeMovement();
    }

    public void UnfreezeMovement()
    {
        playerController.UnfreezeMovement();
    }
}
