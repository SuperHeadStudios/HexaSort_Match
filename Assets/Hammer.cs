using UnityEngine;

public class Hammer : MonoBehaviour
{
    public BoardController boardController;
    
    private void Awake()
    {
        
    }

    public void attack()
    {
        boardController.hammerEffect.Play();
    }
}
