using Unity.VisualScripting;
using UnityEngine;

public enum PlayerStatement
{
    OnGround = 1,
    FirstShoot = 2,
    SecondShoot = 3,
}

public class PlayerState: MonoBehaviour
{
    public static PlayerState instance { get; private set; }
    private void Awake()
    {
        if(instance != null && instance != this)
            Destroy(instance);
        else
            instance = this;
    }

    public PlayerStatement currentState = PlayerStatement.OnGround;
    
    public void SetState()
    {
        this.currentState = (PlayerStatement)((int)this.currentState + 1);
    }

    public PlayerStatement GetState()
    {
        return this.currentState;
    }

    public void Reset()
    {
        currentState = PlayerStatement.OnGround;
    }

    public bool CheckState(PlayerStatement player)
    {
        return currentState == player;
    }
}
