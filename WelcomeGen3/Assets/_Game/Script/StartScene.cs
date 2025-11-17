using UnityEngine;

public class StartScene : MonoBehaviour
{
    private void Start()
    {
        Application.targetFrameRate = 60;
        Application.runInBackground = true;
    }
}
