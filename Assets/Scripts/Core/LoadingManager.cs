using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager Instance { get; private set; }

    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private Slider sliderComponent;

    [Header("Settings")]
    [SerializeField] private float defaultDuration = 3f;

    private Coroutine loadingRoutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        StartLoading();
    }

    public void StartLoading(float duration = -1f)
    {
        if (duration <= 0)
            duration = defaultDuration;

        if (loadingRoutine != null)
            StopCoroutine(loadingRoutine);

        loadingRoutine = StartCoroutine(FakeLoading(duration));
    }

    private IEnumerator FakeLoading(float duration)
    {
        panel.SetActive(true);

        float time = 0f;

        int easingType = Random.Range(0, 3);

        while (time < duration)
        {
            time += Time.deltaTime;

            float t = Mathf.Clamp01(time / duration);

            float progress = EvaluateEasing(t, easingType);

            UpdateUI(progress);

            yield return null;
        }

        UpdateUI(1f);

        panel.SetActive(false);
    }

    private float EvaluateEasing(float t, int type)
    {
        switch (type)
        {
            case 0:
                return 1f - Mathf.Pow(1f - t, 3f);

            case 1:
                return t < 0.5f
                    ? 2f * t * t
                    : 1f - Mathf.Pow(-2f * t + 2f, 2f) / 2f;

            case 2:
                return t == 1f
                    ? 1f
                    : 1f - Mathf.Pow(2f, -10f * t);

            default:
                return t;
        }
    }

    private void UpdateUI(float progress)
    {
        if (sliderComponent != null)
            sliderComponent.value = progress;

        if (progressText != null)
            progressText.text = $"{Mathf.RoundToInt(progress * 100)}%";
    }
}