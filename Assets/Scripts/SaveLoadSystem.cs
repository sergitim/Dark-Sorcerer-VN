using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveLoadSystem : MonoBehaviour
{
    public VisualNovelController visualNovelController; // Reference to the VisualNovelController script
    public SettingsUI SUI;
    public AudioSource AS;

    // Save the game state to a file




    [System.Serializable]
    public class SaveData
    {
        //settings
        public int screenWidth;
        public int screenHeight;
        public bool isFullscreen;

        //gamestate
        public string act;
        public int lineIndex;
        public string dataFilePath;
        public string currentMusicFile;
        public float musicVolume;

        

        // Add other relevant fields to store the game state

        // Add constructors or methods if needed
    }




    public void SaveGame()
    {
        SaveData saveData = new SaveData();
        string saveDataJson = "";

        if (File.Exists(GetSaveFilePath()))
        {
            saveDataJson = File.ReadAllText(GetSaveFilePath());
            saveData = JsonUtility.FromJson<SaveData>(saveDataJson);
        }

        saveData.lineIndex = visualNovelController.currentLineIndex;
        saveData.dataFilePath = visualNovelController.dataFilePath;
        saveData.act = visualNovelController.currentAct;
        saveData.currentMusicFile = visualNovelController.GetCurrentMusicFile(); // Get the current music file from visualNovelController


        // Save other relevant data to the saveData object

        saveDataJson = JsonUtility.ToJson(saveData);
        File.WriteAllText(GetSaveFilePath(), saveDataJson);

        Debug.Log("Game saved!");
        Debug.Log("Music Track: " + saveData.currentMusicFile);
    }

    // Load the game state from a file
    public void LoadGame()
    {
        if (File.Exists(GetSaveFilePath()))
        {
            string saveDataJson = File.ReadAllText(GetSaveFilePath());
            SaveData saveData = JsonUtility.FromJson<SaveData>(saveDataJson);

            // Set the necessary fields in the game based on the loaded data

            visualNovelController.Initialize();

            visualNovelController.currentLineIndex = saveData.lineIndex; // Example: Set the current line index
            visualNovelController.dataFilePath = saveData.dataFilePath;
            visualNovelController.LoadDialogueData();
            visualNovelController.currentAct = saveData.act; // Example: Set the current act

            // Set other relevant fields based on the loaded data


            //Apply the volume changes
            AS.volume = saveData.musicVolume;

            // Load the music file from the "bgm" subfolder
            string musicFilePath = "bgm/" + saveData.currentMusicFile;
            AudioClip musicClip = Resources.Load<AudioClip>(musicFilePath);
            if (musicClip != null)
            {
                visualNovelController.BGM.clip = musicClip;
                visualNovelController.BGM.Play();
            }
            else
            {
                Debug.Log("Failed to load music file: " + musicFilePath);
            }


            //image loading

            visualNovelController.imageChange(saveData.act, saveData.lineIndex, true);
            visualNovelController.ShowNextLine();

            Debug.Log("Game loaded!");
        }
        else
        {
            Debug.Log("No saved game found!");
        }



    }


    public void SaveSettings()
    {
        string saveDataJson = "";

        SaveData saveData = new SaveData();

        if (File.Exists(GetSaveFilePath()))
        {
            saveDataJson = File.ReadAllText(GetSaveFilePath());
            saveData = JsonUtility.FromJson<SaveData>(saveDataJson);
        }

        // Get the current screen resolution
        saveData.screenWidth = Screen.width;
        saveData.screenHeight = Screen.height;

        // Store whether the game is in fullscreen mode
        saveData.isFullscreen = Screen.fullScreen;

        //store the volume slider state
        saveData.musicVolume = SUI.musicVolumeSlider.value;
        Debug.Log("Music Volume: " + saveData.musicVolume);

        saveDataJson = JsonUtility.ToJson(saveData);
        File.WriteAllText(GetSaveFilePath(), saveDataJson);



    }


    public void LoadSettings()
    {
        if (File.Exists(GetSaveFilePath()))
        {
            string saveDataJson = File.ReadAllText(GetSaveFilePath());
            SaveData saveData = JsonUtility.FromJson<SaveData>(saveDataJson);

            // Apply the screen resolution
            Screen.SetResolution(saveData.screenWidth, saveData.screenHeight, Screen.fullScreen);

            // Apply the fullscreen mode
            Screen.fullScreen = saveData.isFullscreen;

            //Apply the volume changes
            AS.volume = saveData.musicVolume;
            if (SUI != null)
            {
                SUI.musicVolumeSlider.value = saveData.musicVolume;
            }

            Debug.Log("Settings loaded!");
        }
        else
        {
            Debug.Log("No saved settings found!");
        }
    }



    // Get the path to the save file
    private string GetSaveFilePath()
    {
         string saveDirectory = Application.persistentDataPath;
        string saveFileName = "save.json";
        return Path.Combine(saveDirectory, saveFileName);
    }
}