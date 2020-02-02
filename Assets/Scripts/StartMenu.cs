using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private Hand _hand;
    [SerializeField] private Press _pressStartButton;

    protected virtual void OnEnable()
    {
        Cursor.visible = false;

        _hand.AddInteractableListener(_pressStartButton);
    }

    protected virtual void OnDisable()
    {
        _hand.RemoveInteractableListener(_pressStartButton);
    }

    public void GoToGame()
    {
        SceneManager.LoadScene(Utils.OnePlayerScene);
    }
}
