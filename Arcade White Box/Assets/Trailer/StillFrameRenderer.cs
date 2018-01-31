using UnityEngine;
using System.Collections;



public class StillFrameRenderer : MonoBehaviour
{

    public int ScreenWidth = 1280;
    public int ScreenHeight = 720;
    public int StartFrame = 0;
    public int EndFrame = 30;

    public string FileName = "MyProject";
    public string NumberSequenceStart = "00";
    public string SaveDirectory = "C:\\UnityRenderer\\";

    float curFrame = 0;
    /// <summary>
    ///  You attach this script to your main camera.
    ///
    /// Still Frame Renderer is a basic script that will generate a series of screen shots at a fixed 30fps and save the screen shots in order
    /// using the start and end frames. The game will pause every 1/30th of a second, save a screen shot and then stop at the EndFrame #.
    ///
    /// HOW TO USE THIS:
    /// 1. Attach this script to the main camera!
    /// 2. Set your start and end frames
    /// 3. Set File Name (leave off the PNG)
    /// 4. Set # Number Sequence start (this adds trailing zeros to the end of the file name
    /// 5. Create and then set your save directory with trailing slash
    /// 6. Start Scene.
    /// MAKE SURE THE DIRECTORY HAS A TRAILING SLASH & THAT IT EXISTS ALREADY BEFORE USING THE SCRIPT OR WHO KNOWS WHAT THE HELL WILL HAPPEN.
    ///
    /// Who the hell would want this? The main intention here is to use the Unity engine as a still frame render engine like the mental ray or scan
    /// line renderers in 3ds max and maya, blender, etc. This script allows you to do just that!
    ///
    /// For best results pause the game before pressing play...
    ///
    /// </summary>

    float deltaTime = 0.0f;

    void Awake()
    {
        QualitySettings.vSyncCount = 2; //set game to 30fps
        curFrame = 0; //set current frame counter to zero.
        //If we set time scale to .1f then in theory the game runs at 1/30th of a second if we set the vsyncCount to 2;
        Time.timeScale = .1f; //Set time scales to 1f or 1/30th of a second.
        Time.fixedDeltaTime = .1f;
    }
    // Use this for initialization
    void Start()
    {

    }
    bool wait = false;
    bool tick = false;
    float temptime = 0.0f;


    // Update is called once per frame
    void Update()
    {

        if (curFrame <= EndFrame)
        {
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;

            if (curFrame < StartFrame)
            {
                if (deltaTime >= 0.001)
                {
                    curFrame += 1;
                }
            }
            else
            {
                if (deltaTime >= 0.001)
                {
                    temptime = deltaTime;
                    Time.timeScale = 0;
                    Time.fixedDeltaTime = 0;
                    tick = true;
                }

                if (tick)
                {
                    RenderToPng();
                }

                if (!tick && wait)
                {
                    WaitForFrame();
                }
            }
        }
    }

    //copypasta from a screen shot script...
    public bool hasSaved()
    {
        //it's not enough to just check that the file exists, since it doesn't mean it's finished saving
        //we have to check if it can actually be opened
        Texture2D image;
        image = new Texture2D(Screen.width, Screen.height);
        bool imageLoadSuccess = image.LoadImage(System.IO.File.ReadAllBytes(getImage()));
        Destroy(image);
        return imageLoadSuccess;
    }

    public void WaitForFrame()
    {
        if (hasSaved()) //The file is saved, set time back to 1f and repeat!
        {
            curFrame += 1; //increment frame by 1.
            Debug.Log("Found Screenshot saved:" + filename + " now moving to next frame:  " + curFrame);
            deltaTime = 0.0f; // reset this
            Time.fixedDeltaTime = 0.1f; //
            Time.timeScale = .1f;
            tick = false; //reset conditionals...
            wait = false;
        }
    }

    public string getImage()
    {
        return filename;
    }

    string filename = "";

    //copypasta from another script..
    public bool RenderToPng()
    {
        bool finished = false;
        RenderTexture rt = new RenderTexture(ScreenWidth, ScreenHeight, 24);
        GetComponent<Camera>().targetTexture = rt;
        Texture2D screenShot = new Texture2D(ScreenWidth, ScreenHeight, TextureFormat.RGB24, false);
        GetComponent<Camera>().Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, ScreenWidth, ScreenHeight), 0, 0);
        GetComponent<Camera>().targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        Destroy(rt);
        byte[] bytes = screenShot.EncodeToPNG();
        filename = SaveDirectory + FileName + NumberSequenceStart + curFrame + ".png";
        System.IO.File.WriteAllBytes(filename, bytes);
        Debug.Log(string.Format("Took screenshot to: {0}", filename));
        wait = true;
        tick = false;
        return finished;
    }

    //This will not render in screenshots...
    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;
        GUIStyle style = new GUIStyle();
        Rect rect = new Rect(0, 25, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
        string text = "SCENE IS RENDERING! Current Frame is: " + curFrame + " of " + EndFrame;
        GUI.Label(rect, text, style);
    }
}