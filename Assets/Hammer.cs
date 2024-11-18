using UnityEngine;

public class Hammer : MonoBehaviour
{
    public BoardController boardController;
    
    [SerializeField] bool isHammerStart = false;

    private void Awake()
    {
        
    }

    public void attack()
    {
        if (isHammerStart) return;
        isHammerStart = true; 
        //boardController.ClearColumn();
        boardController.hammerEffect.Play();

    }


    private void OnDisable()
    {
        isHammerStart = false;
    }

}
