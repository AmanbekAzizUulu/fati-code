using UnityEngine;
using UnityEngine.UI;

public class LoadSceneButton : MonoBehaviour
{
    [SerializeField] private Button buttonComponent;
    [SerializeField] private string sceneName;

    private void Start()
    {
        buttonComponent.onClick.AddListener(OnClick);
    }
    private void OnClick()
    {
        LoadingManager.Instance.StartLoading();
        SceneManager.Instance.LoadScene(sceneName);
    }

    private void OnValidate()
    {
        if (buttonComponent == null)
        {
            buttonComponent = GetComponent<Button>();
        }
    }
}
