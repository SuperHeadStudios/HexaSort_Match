using UnityEngine;
using UnityEditor;
using System.IO;

public class ScreenshotTakerWindow : EditorWindow
{
    private string screenshotFolder = "Screenshots";
    private string screenshotFileName = "Screenshot";
    private int screenshotResolutionMultiplier = 1;

    [MenuItem("Tools/Screenshot Taker")]
    public static void ShowWindow()
    {
        GetWindow<ScreenshotTakerWindow>("Screenshot Taker");
    }

    private void OnGUI()
    {
        // Header
        GUILayout.Label("Screenshot Taker Tool", new GUIStyle(GUI.skin.label) { fontSize = 20, fontStyle = FontStyle.Bold, normal = { textColor = Color.gray } });
        EditorGUILayout.Space();

        // Screenshot Settings
        DrawSection("Screenshot Settings", () =>
        {
            screenshotFolder = EditorGUILayout.TextField("Screenshot Folder", screenshotFolder);
            screenshotFileName = EditorGUILayout.TextField("Screenshot File Name", screenshotFileName);
            screenshotResolutionMultiplier = EditorGUILayout.IntSlider("Resolution Multiplier", screenshotResolutionMultiplier, 1, 4);
        });

        // Buttons
        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Take Screenshot", CreateButtonStyle()))
        {
            TakeScreenshot();
        }
        GUILayout.EndHorizontal();
    }

    private void DrawSection(string title, System.Action content)
    {
        GUILayout.BeginVertical("box");
        GUILayout.Label(title, CreateLabelStyle());
        content();
        GUILayout.EndVertical();
        EditorGUILayout.Space();
    }

    private GUIStyle CreateLabelStyle()
    {
        GUIStyle labelStyle = new GUIStyle(EditorStyles.boldLabel);
        labelStyle.normal.textColor = Color.gray;
        return labelStyle;
    }

    private GUIStyle CreateButtonStyle()
    {
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize = 14;
        buttonStyle.fontStyle = FontStyle.Bold;
        buttonStyle.fixedHeight = 40;
        buttonStyle.normal.textColor = Color.gray;
        return buttonStyle;
    }

    private Texture2D MakeTexWithOutline(int width, int height, Color fillColor, Color outlineColor, int outlineWidth)
    {
        Texture2D tex = new Texture2D(width + 2 * outlineWidth, height + 2 * outlineWidth);
        Color[] fillPixels = new Color[width * height];
        Color[] outlinePixels = new Color[(width + 2 * outlineWidth) * (height + 2 * outlineWidth)];

        for (int i = 0; i < outlinePixels.Length; ++i)
        {
            outlinePixels[i] = outlineColor;
        }

        for (int y = outlineWidth; y < height + outlineWidth; y++)
        {
            for (int x = outlineWidth; x < width + outlineWidth; x++)
            {
                outlinePixels[y * (width + 2 * outlineWidth) + x] = fillColor;
            }
        }

        tex.SetPixels(outlinePixels);
        tex.Apply();
        return tex;
    }

    private void TakeScreenshot()
    {
        if (!Directory.Exists(screenshotFolder))
        {
            Directory.CreateDirectory(screenshotFolder);
        }

        string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string filePath = Path.Combine(screenshotFolder, $"{screenshotFileName}_{timestamp}.png");
        ScreenCapture.CaptureScreenshot(filePath, screenshotResolutionMultiplier);
        Debug.Log($"Screenshot saved to: {filePath}");
    }
}
