using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScreenController : MonoBehaviour
{
    public Sprite backgroundImage;
    public GameObject background;
    public Sprite buttonImage;
    public GameObject canvas;
    public Sprite buttonSprite;

    public SaveLoadSystem sls;


    void Start()
    {

        sls.LoadSettings();

        canvas = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;



        // Create the background image object and set the sprite


       background = CreateBackground(backgroundImage);


        CreateButton("Start New Game", StartNewGame, new Vector2(0f, 60f), new Vector2(200, 100), background.transform);
        CreateButton("Load Game", LoadGame, new Vector2(0f, -60f), new Vector2(200, 100), background.transform);
        CreateButton("Settings", ShowSettings, new Vector2(-150f, -150f), new Vector2(200, 50), background.transform);
        CreateButton("Quit", QuitGame, new Vector2(150f, -150f), new Vector2(200, 50), background.transform);


    }

    void CreateButton(string buttonText, UnityEngine.Events.UnityAction buttonAction, Vector2 buttonPosition,Vector2 buttonSize, Transform parent)
    {
        // Create a new GameObject for the button
        GameObject newButtonObject = new GameObject(buttonText + "Button");
        newButtonObject.transform.SetParent(canvas.transform);

        // Add a Button component to the GameObject
        Button newButton = newButtonObject.AddComponent<Button>();

        // Add an Image component to the GameObject and set its sprite to the default button sprite
        // Create a new game object for the button image
        GameObject buttonObject = new GameObject(buttonText + " image");

        // Add a RectTransform component to the GameObject
        RectTransform buttonRectTransform = newButtonObject.AddComponent<RectTransform>();

        // Add a RectTransform component to the background object
        RectTransform backgroundRectTransform = buttonObject.AddComponent<RectTransform>();

        // Set the size and position of the background RectTransform to match the camera viewport
        Camera mainCamera = Camera.main;

        // Add an Image component to the background object
        Image backgroundImageComponent = buttonObject.AddComponent<Image>();

        // Set the sprite of the background image to the provided sprite
        backgroundImageComponent.sprite = buttonImage;

        // Set the background image to stretch to fill the RectTransform
        backgroundImageComponent.type = Image.Type.Simple;
        backgroundImageComponent.preserveAspect = false;

        // Set the parent of the text object to the canvas
        buttonObject.transform.SetParent(newButtonObject.transform, false);


        // Set the button text
        GameObject textObject = new GameObject("Text");
        Text buttonTextComponent = textObject.AddComponent<Text>();
        buttonTextComponent.text = buttonText;
        buttonTextComponent.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        buttonTextComponent.color = Color.black;
        buttonTextComponent.alignment = TextAnchor.MiddleCenter;
        buttonTextComponent.transform.SetParent(newButtonObject.transform);
        buttonTextComponent.fontSize = 40; // set font size

        // Set the size of the button RectTransform
        buttonRectTransform.sizeDelta = new Vector2(buttonSize.x, buttonSize.y);

        //set the size of the button image.

        backgroundRectTransform.sizeDelta = new Vector2(buttonSize.x, buttonSize.y);

        //set the size of the textbox containing the text
        RectTransform textRectTransform = buttonTextComponent.GetComponent<RectTransform>();
        textRectTransform.sizeDelta = new Vector2(buttonSize.x, buttonSize.y);


        // Set the button action
        newButton.onClick.AddListener(buttonAction);

        // Set the button position
        buttonRectTransform.anchoredPosition = buttonPosition;

    }



    GameObject CreateBackground(Sprite backgroundImage)
    {
        // Create a new game object for the background image
        GameObject backgroundObject = new GameObject("Background");

        // Add a RectTransform component to the background object
        RectTransform backgroundRectTransform = backgroundObject.AddComponent<RectTransform>();

        // Set the size and position of the background RectTransform to match the camera viewport
        Camera mainCamera = Camera.main;
        Vector3 bottomLeftCorner = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRightCorner = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));
        Vector3 size = topRightCorner - bottomLeftCorner;
        backgroundRectTransform.sizeDelta = new Vector2(size.x, size.y);
        backgroundRectTransform.position = new Vector3(0,0,-10);

        // Add an Image component to the background object
        Image backgroundImageComponent = backgroundObject.AddComponent<Image>();

        // Set the sprite of the background image to the provided sprite
        backgroundImageComponent.sprite = backgroundImage;

        // Set the background image to stretch to fill the RectTransform
        backgroundImageComponent.type = Image.Type.Simple;
        backgroundImageComponent.preserveAspect = false;



        // Stretch the background image to fill the entire screen
        backgroundRectTransform.anchorMin = Vector2.zero;
        backgroundRectTransform.anchorMax = Vector2.one;
        backgroundRectTransform.offsetMin = Vector2.zero;
        backgroundRectTransform.offsetMax = Vector2.zero;




        // Set the parent of the background object to the canvas
        backgroundObject.transform.SetParent(canvas.transform, false);

        return backgroundObject;
    }


    void StartNewGame()
    {
        // Code to start a new game goes here
        SceneManager.LoadScene("VisualNovel");
    }

    void LoadGame()
    {
        // Code to load a saved game goes here
        SceneManager.LoadScene("VisualNovel");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void ShowSettings()
    {
        // Code to show settings goes here
        SceneManager.LoadScene("Settings");
    }

    void QuitGame()
    {
        // Code to quit the game goes here
            Debug.Log("Quit");
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }



    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "VisualNovel")
        {
            // Find the canvas object in the loaded scene
            GameObject canvas = GameObject.Find("Canvas1");
            if (canvas != null)
            {
                // Get the script component from the canvas object
                SaveLoadSystem saveLoadSystem = canvas.GetComponent<SaveLoadSystem>();
                if (saveLoadSystem != null)
                {
                    // Call the function on the script
                    saveLoadSystem.LoadGame();
                }
                else
                {
                    Debug.Log("SaveLoadSystem script not found on the Canvas object.");
                }
            }
            else
            {
                Debug.Log("Canvas object not found in the loaded scene.");
            }
        }
        else
        {
            Debug.Log("Wrong scene!");
        }

        // Unsubscribe the event to prevent multiple invocations
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


}
