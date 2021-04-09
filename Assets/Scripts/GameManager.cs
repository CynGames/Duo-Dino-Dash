using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TMP_Text debugOutputOnScreen;

    private void Start()
    {
        Instance = this;
    }

    public void WriteOnScreen(InputHandler.TouchDirection content)
    {
        debugOutputOnScreen.text = content.ToString();
    }
}
