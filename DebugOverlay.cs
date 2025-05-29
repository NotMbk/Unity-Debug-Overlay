using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class DebugOverlay : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float updateInterval = 0.5f;
    [SerializeField][Range(0f, 1f)] private float backgroundOpacity = 0.2f;
    [SerializeField] private Color textColor = Color.green;
    [SerializeField] private int fontSize = 25;
    [SerializeField] private Vector2 padding = new (10, 10);

    private Canvas debugCanvas;
    private Image backgroundPanel;
    private TextMeshProUGUI debugText;

    
    private System.Text.StringBuilder stringBuilder = new(256);
    private readonly Dictionary<string, string> debugValues = new();

    private readonly object lockObject = new object(); // Lock for thread safety

    // FPS calculation variables
    private float fpsUpdateTimer;
    private int frameCount;
    private float fps;

    private static DebugOverlay _instance;
    public static DebugOverlay Instance{
        get
        {
            if (_instance == null)
            {
                // Try to find an existing instance
                _instance = FindFirstObjectByType<DebugOverlay>();

                // If no instance exists, create one
                if (_instance == null)
                {
                    GameObject debuggerObj = new ("VisualDebugger");
                    _instance = debuggerObj.AddComponent<DebugOverlay>();
                    DontDestroyOnLoad(debuggerObj);
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        if (debugCanvas == null)
        {
            InitializeUI();
        }
    }

    private void InitializeUI()
    {
        // Canvas
        GameObject canvasObj = new("DebugCanvas");
        canvasObj.transform.SetParent(transform);

        debugCanvas = canvasObj.AddComponent<Canvas>();
        debugCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        debugCanvas.sortingOrder = 1000; // Ensure it's on top of everything

        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        // Background panel
        GameObject panelObj = new("DebugPanel");
        panelObj.transform.SetParent(canvasObj.transform, false);

        backgroundPanel = panelObj.AddComponent<Image>();
        backgroundPanel.color = new Color(0, 0, 0, backgroundOpacity);

        RectTransform panelRect = panelObj.GetComponent<RectTransform>();
        panelRect.anchorMin = new(0, 1);
        panelRect.anchorMax = new(0, 1);
        panelRect.pivot = new(0, 1);
        panelRect.anchoredPosition = new(padding.x, -padding.y);
        panelRect.sizeDelta = new(400, 200);

        // Debug text
        GameObject textObj = new("DebugText");
        textObj.transform.SetParent(panelObj.transform, false);

        debugText = textObj.AddComponent<TextMeshProUGUI>();
        debugText.color = textColor;
        debugText.fontSize = fontSize;
        debugText.alignment = TextAlignmentOptions.TopLeft;
        debugText.textWrappingMode = TextWrappingModes.Normal;

        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.pivot = new(0.5f, 0.5f);
        textRect.anchoredPosition = Vector2.zero;
        textRect.sizeDelta = new(-15, -15); 
        
        SetDebugValue("FPS", "0");
    }

    private void Update()
    {
        CalculateAndUpdateFPS();
        UpdateDebugDisplay();
    }

    private void CalculateAndUpdateFPS()
    {
        frameCount++;
        fpsUpdateTimer += Time.unscaledDeltaTime;

        if (fpsUpdateTimer >= updateInterval)
        {
            fps = frameCount / fpsUpdateTimer;
            frameCount = 0;
            fpsUpdateTimer = 0;

            SetDebugValue("FPS", ((int)fps).ToString());
        }
    }

    private void UpdateDebugDisplay()
    {
        if (debugText == null) return;

       stringBuilder.Clear();

        foreach (KeyValuePair<string, string> pair in debugValues)
        {
            stringBuilder.AppendLine($"{pair.Key}: {pair.Value}");
        }

        debugText.text = stringBuilder.ToString();

        RectTransform panelRect = backgroundPanel.GetComponent<RectTransform>();
        float height = Mathf.Min(debugText.preferredHeight + 20, Screen.height * 0.7f);
        panelRect.sizeDelta = new (400, height);
        
    }

    public static void SetDebugValue(string key, object value)
    {
        Instance.SetDebugValueInternal(key, value);
    }

    private void SetDebugValueInternal(string key, object value)
    {
        lock (lockObject)
        {
            string valueText = value?.ToString() ?? "null";
            debugValues[key] = valueText;
        }
    }
}