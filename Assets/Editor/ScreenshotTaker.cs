using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.ShortcutManagement;

public class ScreenshotTakerWindow : EditorWindow
{
    private string screenshotFolder = "Screenshots";
    private string screenshotFileName = "Screenshot";
    private int screenshotResolutionMultiplier = 1;
    private string fileFormat = "PNG";
    private string[] fileFormats = { "PNG", "JPEG" };
    private Texture2D lastScreenshotPreview;
    private int customWidth = 1920;
    private int customHeight = 1080;
    private bool useCustomResolution = false;
    private string namingFormat = "{ProjectName}_{FileName}_{Timestamp}";
    private string watermarkText = "";
    private bool addWatermark = false;
    private string watermarkPosition = "Bottom Right";
    private string[] watermarkPositions = { "Top Left", "Top Right", "Bottom Left", "Bottom Right" };

    // New fields for transparent screenshots
    private bool captureTransparent = false;
    private LayerMask captureLayer;

    [MenuItem("Tools/Screenshot Taker")]
    public static void ShowWindow()
    {
        GetWindow<ScreenshotTakerWindow>("Screenshot Taker");
    }

    private void OnGUI()
    {
        GUILayout.Label("Screenshot Taker Tool", new GUIStyle(GUI.skin.label) { fontSize = 20, fontStyle = FontStyle.Bold, normal = { textColor = Color.gray } });
        EditorGUILayout.Space();

        // Screenshot Settings
        DrawSection("Screenshot Settings", () =>
        {
            screenshotFolder = EditorGUILayout.TextField("Screenshot Folder", screenshotFolder);
            screenshotFileName = EditorGUILayout.TextField("Screenshot File Name", screenshotFileName);
            screenshotResolutionMultiplier = EditorGUILayout.IntSlider("Resolution Multiplier", screenshotResolutionMultiplier, 1, 4);
            fileFormat = fileFormats[EditorGUILayout.Popup("File Format", System.Array.IndexOf(fileFormats, fileFormat), fileFormats)];
        });

        // Custom Resolution Settings
        DrawSection("Custom Resolution", () =>
        {
            useCustomResolution = EditorGUILayout.Toggle("Use Custom Resolution", useCustomResolution);
            if (useCustomResolution)
            {
                customWidth = EditorGUILayout.IntField("Width", customWidth);
                customHeight = EditorGUILayout.IntField("Height", customHeight);
            }
        });

        // Naming Format Customization
        DrawSection("Naming Format", () =>
        {
            namingFormat = EditorGUILayout.TextField("Naming Format", namingFormat);
            EditorGUILayout.HelpBox("Use {ProjectName}, {FileName}, and {Timestamp} as placeholders.", MessageType.Info);
        });

        // Watermark Settings
        DrawSection("Watermark", () =>
        {
            addWatermark = EditorGUILayout.Toggle("Add Watermark", addWatermark);
            if (addWatermark)
            {
                watermarkText = EditorGUILayout.TextField("Watermark Text", watermarkText);
                watermarkPosition = watermarkPositions[EditorGUILayout.Popup("Watermark Position", System.Array.IndexOf(watermarkPositions, watermarkPosition), watermarkPositions)];
            }
        });

        // Transparent Screenshot Settings
        DrawSection("Transparent Screenshot", () =>
        {
            captureTransparent = EditorGUILayout.Toggle("Capture Transparent Background", captureTransparent);
            captureLayer = EditorGUILayout.LayerField("Capture Layer", captureLayer);
        });

        // Screenshot Preview
        DrawSection("Last Screenshot Preview", () =>
        {
            if (lastScreenshotPreview != null)
            {
                GUILayout.Label(lastScreenshotPreview, GUILayout.Width(200), GUILayout.Height(200));
            }
            else
            {
                GUILayout.Label("No screenshot taken yet.");
            }
        });

        // Buttons
        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Take Screenshot", CreateButtonStyle()))
        {
            TakeScreenshot();
        }
        if (GUILayout.Button("Open Screenshot Folder", CreateButtonStyle()))
        {
            OpenScreenshotFolder();
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

    private void TakeScreenshot()
    {
        if (!Directory.Exists(screenshotFolder))
        {
            Directory.CreateDirectory(screenshotFolder);
        }

        string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string projectName = Application.productName;
        string formattedFileName = namingFormat
            .Replace("{ProjectName}", projectName)
            .Replace("{FileName}", screenshotFileName)
            .Replace("{Timestamp}", timestamp);

        string filePath = Path.Combine(screenshotFolder, $"{formattedFileName}.{fileFormat.ToLower()}");

        if (useCustomResolution)
        {
            if (captureTransparent)
                CaptureTransparentScreenshot(filePath, customWidth, customHeight, captureLayer);
            else
                CaptureCustomResolutionScreenshot(filePath, customWidth, customHeight);
        }
        else
        {
            if (captureTransparent)
                CaptureTransparentScreenshot(filePath, Screen.width * screenshotResolutionMultiplier, Screen.height * screenshotResolutionMultiplier, captureLayer);
            else
                ScreenCapture.CaptureScreenshot(filePath, screenshotResolutionMultiplier);
        }

        Debug.Log($"Screenshot saved to: {filePath}");

        EditorApplication.delayCall += () => LoadLastScreenshotPreview(filePath);

        if (addWatermark)
        {
            EditorApplication.delayCall += () => AddWatermarkToScreenshot(filePath);
        }
    }

    private void CaptureCustomResolutionScreenshot(string filePath, int width, int height)
    {
        Camera camera = Camera.main;
        RenderTexture rt = new RenderTexture(width, height, 24);
        camera.targetTexture = rt;

        Texture2D screenShot = new Texture2D(width, height, TextureFormat.RGB24, false);
        camera.Render();

        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenShot.Apply();

        camera.targetTexture = null;
        RenderTexture.active = null; // Clean up

        byte[] bytes = fileFormat == "PNG" ? screenShot.EncodeToPNG() : screenShot.EncodeToJPG();
        File.WriteAllBytes(filePath, bytes);

        RenderTexture.ReleaseTemporary(rt);
    }

    // Method to capture transparent screenshots
    private void CaptureTransparentScreenshot(string filePath, int width, int height, LayerMask captureLayer)
    {
        Camera camera = Camera.main;

        // Set camera properties to render only the specified layer and clear with transparency
        camera.cullingMask = captureLayer;
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color(0, 0, 0, 0);  // Transparent background

        RenderTexture rt = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
        camera.targetTexture = rt;

        Texture2D screenShot = new Texture2D(width, height, TextureFormat.RGBA32, false);
        camera.Render();

        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenShot.Apply();

        camera.targetTexture = null;
        RenderTexture.active = null; // Clean up

        byte[] bytes = screenShot.EncodeToPNG();  // PNG supports transparency
        File.WriteAllBytes(filePath, bytes);

        RenderTexture.ReleaseTemporary(rt);
    }

    private void LoadLastScreenshotPreview(string filePath)
    {
        byte[] screenshotData = File.ReadAllBytes(filePath);
        lastScreenshotPreview = new Texture2D(1, 1);
        lastScreenshotPreview.LoadImage(screenshotData);
    }

    private void AddWatermarkToScreenshot(string filePath)
    {
        byte[] screenshotData = File.ReadAllBytes(filePath);
        Texture2D screenshot = new Texture2D(1, 1);
        screenshot.LoadImage(screenshotData);

        Color watermarkColor = Color.white;
        int x = 10, y = 10;

        switch (watermarkPosition)
        {
            case "Top Left":
                x = 10; y = screenshot.height - 20;
                break;
            case "Top Right":
                x = screenshot.width - 100; y = screenshot.height - 20;
                break;
            case "Bottom Left":
                x = 10; y = 10;
                break;
            case "Bottom Right":
                x = screenshot.width - 100; y = 10;
                break;
        }

        AddTextToTexture(screenshot, watermarkText, x, y, watermarkColor);

        byte[] watermarkedBytes = screenshot.EncodeToPNG();
        File.WriteAllBytes(filePath, watermarkedBytes);
    }

    private void AddTextToTexture(Texture2D texture, string text, int x, int y, Color color)
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 20;
        style.normal.textColor = color;

        RenderTexture rt = RenderTexture.GetTemporary(texture.width, texture.height);
        Graphics.Blit(texture, rt);

        RenderTexture.active = rt;
        GL.PushMatrix();
        GL.LoadPixelMatrix(0, texture.width, texture.height, 0);

        GUI.Label(new Rect(x, y, 100, 20), text, style);

        GL.PopMatrix();
        RenderTexture.active = null;

        RenderTexture.ReleaseTemporary(rt);
    }

    private void OpenScreenshotFolder()
    {
        EditorUtility.RevealInFinder(screenshotFolder);
    }
}
