using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class VisualNovelController : MonoBehaviour
{
    public Text dialogueText;
    public GameObject canvas;
    public GameObject dialogueBox;
    public Text nameText;
    public GameObject nameBox;
    public GameObject hideTextButton;
    public GameObject backlogButton;
    public GameObject backlogPlusButton;
    public GameObject backlogMinusButton;
    public GameObject loadButton;
    public GameObject saveButton;
    public GameObject backlogPanel;
    public int backlogLine = 1; //How far back to go. If it reaches 0, exit backlog and set back to 1
    public Text backlogText;
    public string dataFilePath;
    public string currentAct;
    public GameObject buttonPrefab;
    public List<string> dialogueLines = new List<string>();
    public Dictionary<string, string> choiceFileMap;
    public int currentLineIndex = 0;
    private bool isChoiceDisplayed = false;
    bool isSkipping = false;
    public Image Background;
    public Sprite InterfaceBox;
    public AudioSource BGM;
    public SaveLoadSystem sls;


    public List<Color> colors;
    public List<Texture2D> BackgroundImages;

    public List<AudioClip> Music;

    public Dictionary<string, Dictionary<int, int>> songChangingLinesByAct;

    public Dictionary<string, Dictionary<int, int>> imageChangingLinesByAct;

    void Start()
    {

     choiceFileMap = new Dictionary<string, string>()
     {
    { "Sword", "Data/sword" },
    { "Axe", "Data/axe" },
    { "Bow", "Data/bow" },
    {"Act 1","Data/act1" },
    {"Eldric the Wizard" , "Data/act1A"},
    {"Sabel the Battle Mage", "Data/act1B"},
    {"Let her come", "Data/act2BA"},
    {"Don't let her come", "Data/Act2BB"},
    {"Banish her", "Data/act3BAA"},
    {"Don't banish her", "Data/act3BAB"},
    {"Finish off Eryndor", "Data/act3BBA"},
    {"Stop the dragon", "Data/act3BBB"},
    {"Sacrifice his magic power", "Data/act2AA"},
    { "Don't sacrifice his magic power", "Data/act2AB"},

     };




        Initialize();
        sls.LoadSettings();
    




        LoadDialogueData();
        ShowNextLine();
    }



    public void Initialize()
    {

        // Initialize the songChangingLinesByAct dictionary
        songChangingLinesByAct = new Dictionary<string, Dictionary<int, int>>();

        // Populate the songChangingLinesByAct dictionary with act-specific song changing lines
        Dictionary<int, int> act1SongChangingLines = new Dictionary<int, int>()
        {
            { 0, 6 },
            { 20, 1 },
            { 36, 6 },
            { 46, 1 },
            { 56, 5 },
            { 77, 2 },
            { 87, 1 },
            { 119, 5 },
        };

        songChangingLinesByAct.Add("Act1", act1SongChangingLines);


        Dictionary<int, int> act1ASongChangingLines = new Dictionary<int, int>()
        {
            { 0, 3 },

        };

        songChangingLinesByAct.Add("act1A", act1ASongChangingLines);


        Dictionary<int, int> act1BSongChangingLines = new Dictionary<int, int>()
        {
            { 0, 3 },
            { 7, 2 },
            { 22, 1 },
            { 36, 6 },
            { 42, 1 },
            { 50, 5 },
            { 54, 1 },
            { 77, 2 },
            { 85, 5 },
            { 100, 1 },
            { 147, 4 },
            { 170, 1 },

        };

        songChangingLinesByAct.Add("act1B", act1BSongChangingLines);


        Dictionary<int, int> act2BASongChangingLines = new Dictionary<int, int>()
        {
            { 0, 3 },
            { 13, 1 },
            { 33, 4 },
            { 45, 6 },
            { 71, 4 },
            { 88, 0 },
            { 101, 6 },
            { 126, 5 },
            { 133, 6 },
            { 142, 1 },
            { 153, 3 },
            { 182, 5 },

        };

        songChangingLinesByAct.Add("act2BA", act2BASongChangingLines);


        Dictionary<int, int> act3BAASongChangingLines = new Dictionary<int, int>()
        {
            { 0, 5 },

        };

        songChangingLinesByAct.Add("act3BAA", act3BAASongChangingLines);



        Dictionary<int, int> act3BABSongChangingLines = new Dictionary<int, int>()
        {
            { 0, 5 },

        };

        songChangingLinesByAct.Add("act3BAB", act3BABSongChangingLines);



        // Add other acts' song changing lines as needed...


        // Initialize the imageChangingLinesByAct dictionary
        imageChangingLinesByAct = new Dictionary<string, Dictionary<int, int>>();





        // Populate the imageChangingLinesByAct dictionary with act-specific image changing lines
        Dictionary<int, int> act1ImageChangingLines = new Dictionary<int, int>()
        {
            { 0, 0 },
            { 3,  1},
            { 6,  2},
            { 8,  3},
            { 10,  4},
            { 12,  5},
            { 13,  6},
            { 16,  7},
            { 20,  8},
            { 25,  9},
            { 29,  10},
            { 31,  11},
            { 35,  12},
            { 37,  13},
            { 39,  14},
            { 41,  15},
            { 46,  16},
            { 50,  17},
            { 56,  18},
            { 62,  19},
            { 65,  20},
            { 73,  21},
            { 77,  22},
            { 81,  27},
            { 87,  28},
            { 89,  29},
            { 92,  30},
            { 95,  31},
            { 99,  32},
            { 107,  33},
            { 109,  34},
            { 117,  35},
            { 121,  36},
            { 130,  37},
            { 132,  38},
            { 140,  39},
            { 142,  40},
        };
        imageChangingLinesByAct.Add("Act1", act1ImageChangingLines);


        Dictionary<int, int> act1AImageChangingLines = new Dictionary<int, int>()
        {
            { 0, 40 },
            { 5, 34 },
            { 8, 32 }, //Needs to be changed by a picture of Arin's little sister, when she's still a little girl
            { 11, 158 },
            { 16, 34 }, //Consider changing this one. It requires Sabel looking slightly unhappy.
            { 22, 32 }, //Change it to one with Sabel standing next to Irina and her mother.
            { 23, 158 }, //Eldric.
            { 28, 157 }, // Arin and Eldric sit, as they discuss their training
            { 37, 28 }, //Chpter 4: Training begins.

            { 38, 29 }, //Arin reading a book, studying.
            { 41, 29 }, //Arin with his family. He took Irina on walks in the forest, played games with her, and helped his mother care for his bedridden father.
            { 43, 29 }, //Arin laying in bed, contemplative, at night.
            { 46, 29 }, // Transition scene, the following day... Arin packing up his bags OR picture of the village.
            { 48, 28 }, //Eldric's tower. (DONE)
            { 51, 29 }, //Eldric greets arin and leads him to his room.
            { 59, 29 }, //Arin's new room at the tower.
            { 62, 29 }, // Training/meditation room. Arin followed Eldric to a spacious room filled with books, potions, and various magical artifacts. There was a large circle etched onto the floor, and Eldric gestured for Arin to stand in the center of it.
            { 65, 29 }, // Arin, with his eyes closed. Magic glow around him, or gathered around his hand, if it appears in the image.
            { 71, 29 }, //Eldric talking to Arin, while Arin practices magic diligently.
            { 75, 29 }, //Training rom 2, with training dummies.
            { 77, 29 }, //Arin focusing and casting magic, trying to push away the dummies.
            { 84, 29 }, //Arin and Eldric have dinner
            { 85, 29 }, // Morning, Arin wakes up Maybe a repeat of Arin's room.
            { 86, 29 }, //Eldric holding a small wooden box
            //Consider adding a picture in this spot: Arin struggling to lift the stones from the cage.
            { 91, 29 }, //Eryndor offering advice to Arin
            { 92, 29 }, // Arin makes a stone float with his magic.
            { 98, 29 }, // Eldric watches Arin train.
            { 100, 29 }, //Arin spends the rest of the day levitating one stone.

            { 103, 160 }, //Chapter 5: Training and Revelations

            { 104, 29 }, //Same as the previous one, Arin levitating one stone.
            { 111, 29 }, //Arin and Eldric have dinner, re-use the picture used in line 84
            { 114, 61 }, // Picture of Eryndor, the Dark Sorcerer, since they are talking about him. Use one of the images that are already there. (DONE)
            { 117, 29 }, // Re-use the picture of Arin and Eldric having dinner.
            { 125, 29 }, // Eldric drags out a chest from the corner of the training room.
            { 136, 29 }, // Arin makes the chest float in the air
            { 143, 29 }, // Arin, thinking/having an idea.
            { 146, 29 }, // Same as the earlier line, Arin makes the chest float in the air.
            { 150, 29 }, // Arin makes the thre stones float at once. They are all attached to each other, as if connected by a magical string.
            { 154, 29 }, //Eldric imparting wisdom. It should be possible to re-use one of the pictures in which he appears, such as line 91
            { 158, 29 }, //Eldric casts a spell. It raises in the air the training dummies, and all the other furniture in the room.
            { 159, 29 }, // All the furnitue and training dummies together form a wall.
            { 164, 29 }, //Eldric returns the furniture that was floating to the floor. Maybe re-use the picture from line 158?
            { 166, 29 }, //Dinner with Eldric again. Re-use the image from 117.

            { 169, 158 }, //Chapter 6: The Graduation Exam

            { 170, 29 }, //Arin casting the spell that raises furniture in the air.
            { 178, 135 }, // Eldric and Sabel standing together in a chamber of the tower, surrounded by chunks of wood, steel and rock.
            { 188, 29 }, // Arin nodding, serious.
            { 190, 29 }, // Eldric, with his arms crossed, in the middle of the room.
            { 195, 29 }, // Sabel raises a magical barrier, her eyes glowing with light.
            { 197, 29 }, // Arin raises a wall of debris and rock chunks from all over the place.
            { 200, 29 }, // A magical blast from Eldric breaks the barrier.

            { 236, 160 }, //Chapter 7: Call for adventure
            { 272, 158 }, //Chapter 8: Infiltration 
            { 337, 158 },

        };

        imageChangingLinesByAct.Add("act1A", act1AImageChangingLines);



        Dictionary<int, int> act2AAImageChangingLines = new Dictionary<int, int>()
        {
            { 1, 149 },

        };

        imageChangingLinesByAct.Add("act2AA", act2AAImageChangingLines);




        Dictionary<int, int> act2ABImageChangingLines = new Dictionary<int, int>()
        {
            { 1, 149 },

        };

        imageChangingLinesByAct.Add("act2AB", act2ABImageChangingLines);





        Dictionary<int, int> act1BImageChangingLines = new Dictionary<int, int>()
        {
            { 0, 40 },
            { 1, 34 },
            { 7, 41 },
            { 19, 28 },
            { 22, 42 },
            { 26, 43 },
            { 28, 44 },
            { 30, 46 },
            { 32, 45 },
            { 35, 25 },
            { 37, 47 },
            { 40, 48 },
            { 42, 49 },
            { 46, 50 },
            { 50, 51 },
            { 54, 52 },
            { 59, 53 },
            { 63, 54 },
            { 68, 55 },
            { 72, 56 },
            { 77, 57 },
            { 85, 58 },
            { 97, 59 },
            { 100, 60 },
            { 101, 62 },
            { 103, 61 },
            { 105, 63 },
            { 107, 64 },
            { 109, 62 },
            { 113, 65 },
            { 117, 66 },
            { 129, 67 },
            { 137, 68 },
            { 147, 69 },
            { 148, 70 },
            { 150, 71 },
            { 155, 72 },
            { 157, 73 },
            { 160, 74 },
            { 165, 75 },
            { 167, 76 },
            { 170, 26 },
            { 173, 77 },
            { 176, 78 },

        };

        imageChangingLinesByAct.Add("act1B", act1BImageChangingLines);



        Dictionary<int, int> act2BAImageChangingLines = new Dictionary<int, int>()
        {
            { 0, 78 },
            { 2, 79 },
            { 4, 81 },
            { 10, 80 },
            { 13, 82 },
            { 14, 83 },
            { 17, 84 },
            { 19, 85 },
            { 22, 86 },
            { 26, 87 },
            { 31, 88 },
            { 33, 90 },
            { 34, 93 },
            { 36, 89 },
            { 37, 95 },
            { 38, 96 },
            { 40, 97 },
            { 45, 99 },
            { 49, 98 },
            { 56, 100 },
            { 58, 102 },
            { 63, 101 },
            { 64, 89 },
            { 69, 103 },
            { 71, 92 },
            { 72, 107 },
            { 75, 104 },
            { 80, 105 },
            { 84, 108 },
            { 88, 109 },
            { 94, 110 },
            { 97, 111 },
            { 100, 114 },
            { 103, 115 },
            { 106, 110 },
            { 110, 117 },
            { 115, 118 },
            { 118, 119 },
            { 124, 123 },
            { 128, 122 },
            { 131, 116 },
            { 135, 124 },
            { 137, 121 },
            { 139, 127 },
            { 142, 128 },
            { 150, 117 },
            { 153, 120 },
            { 155, 125 },
            { 157, 128 },
            { 159, 129 },
            { 160, 130 },
            { 163, 131 },
            { 166, 116 },
            { 171, 132 },
            { 174, 133 },
            { 179, 126 },
            { 182, 134 },
            { 190, 135 },
            { 193, 136 },
            { 198, 137 },
            { 201, 138 },








        };

        imageChangingLinesByAct.Add("act2BA", act2BAImageChangingLines);




        Dictionary<int, int> act2BBImageChangingLines = new Dictionary<int, int>()
        {
            { 1, 149 },

        };

        imageChangingLinesByAct.Add("Act2BB", act2BBImageChangingLines);


        Dictionary<int, int> act3BBAImageChangingLines = new Dictionary<int, int>()
        {
            { 1, 149 },

        };

        imageChangingLinesByAct.Add("act3BBA", act3BBAImageChangingLines);



        Dictionary<int, int> act3BBBImageChangingLines = new Dictionary<int, int>()
        {
            { 1, 149 },

        };

        imageChangingLinesByAct.Add("act3BBB", act3BBBImageChangingLines);


        Dictionary<int, int> act3BAAImageChangingLines = new Dictionary<int, int>()
        {
            { 0, 79 },
            { 1, 136 },
            { 2, 149 },
            { 4, 150 },
            { 6, 151 },
            { 9, 152 },
            { 10, 131 },
            { 15, 153 },
            { 19, 154 },
            { 22, 155 },
            { 26, 132 },
            { 33, 156 },
            //{ 37, 134 },
            { 38, 143 },
            { 41, 53 },
            { 43, 157 },
            { 48, 146 },
            { 50, 91 },


        };


        imageChangingLinesByAct.Add("act3BAA", act3BAAImageChangingLines);



        Dictionary<int, int> act3BABImageChangingLines = new Dictionary<int, int>()
        {
            { 0, 143 },
            { 1, 139 },
            { 7, 140 },
            { 13, 142 },
            { 15, 145 },
            { 19, 143 },
            { 24, 144 },
            { 26, 146 },
            { 28, 147 },
            { 29, 148 },
        };


        imageChangingLinesByAct.Add("act3BAB", act3BABImageChangingLines);



        // Add other acts' image changing lines as needed...





        colors = new List<Color>();
        colors.Add(new Color(1f, 0.2f, 0.2f)); // Arin 0
        colors.Add(new Color(0.7f, 0.2f, 0.2f)); // Mom 1
        colors.Add(new Color(1f, 0.5f, 0.5f)); // Irina 2
        colors.Add(new Color(0.0f, 0.8f, 0f)); // Sabel 3
        colors.Add(new Color(0.5f, 0.5f, 1f)); // Eldric 4
        colors.Add(new Color(0.5f, 0.0f, 05f)); // Eryndor 5
        colors.Add(new Color(0.9f, 0.5f, 0f)); // May 6
        colors.Add(new Color(0.7f, 0f, 0.2f)); // Dad 7



        // Get the RectTransform component of the background image
        RectTransform backgroundRectTransform = Background.GetComponent<RectTransform>();

        // Stretch the background image to fill the entire screen
        backgroundRectTransform.anchorMin = Vector2.zero;
        backgroundRectTransform.anchorMax = Vector2.one;
        backgroundRectTransform.offsetMin = Vector2.zero;
        backgroundRectTransform.offsetMax = Vector2.zero;



        dialogueBox.SetActive(true);


        // Get the RectTransform component of the game object
        RectTransform objectRectTransform = dialogueBox.GetComponent<RectTransform>();

        // Set the anchors to stretch horizontally
        objectRectTransform.anchorMin = new Vector2(0, 0);
        objectRectTransform.anchorMax = new Vector2(1, 0.3f);

        // Set the offsets to zero to align with the edges of the screen
        objectRectTransform.offsetMin = Vector2.zero;
        objectRectTransform.offsetMax = Vector2.zero;

    }

    public void LoadDialogueData()
    {
        // Load the dialogue data from the specified file
        TextAsset textAsset = Resources.Load<TextAsset>(dataFilePath);

        // Split the data into individual dialogue lines
        dialogueLines = new List<string>(textAsset.text.Split('\n'));
    }


    public void ShowNextLine()
    {
        if (currentLineIndex >= dialogueLines.Count)
        {
            // End of dialogue
            SceneManager.LoadScene("TitleScreen");
            return;
        }

        // Display the next line of dialogue or a choice
        string line = dialogueLines[currentLineIndex];
        imageChange(currentAct, currentLineIndex,false);
        musicChange(currentAct,currentLineIndex);

        if (line.StartsWith("["))
        {
            // This is a choice
            nameBox.SetActive(false);
            dialogueText.text = line;
            dialogueText.color = Color.black;

            ShowChoices(line);
            Debug.Log("Choice detected!");
        }
        else
        {
            // This is a line of dialogue or a non-dialogue line
            string[] dialogueParts = line.Split(':');

            if (dialogueParts.Length == 2)
            {
                string characterName = dialogueParts[0];

                nameText.text = characterName;
                nameBox.SetActive(true);
                dialogueText.text = dialogueParts[1];

                // Set the text color for dialogue lines
                dialogueText.color = Color.white;

                // Check if the line contains specific character names and update text color accordingly
                switch (characterName)
                {
                    case "Arin":
                        dialogueText.color = colors[0];
                        break;
                    case "Mom":
                        dialogueText.color = colors[1];
                        break;
                    case "Irina":
                        dialogueText.color = colors[2];
                        break;
                    case "Sabel":
                        dialogueText.color = colors[3];
                        break;
                    case "Eldric":
                        dialogueText.color = colors[4];
                        break;
                    case "Eryndor":
                        dialogueText.color = colors[5];
                        break;
                    case "May":
                        dialogueText.color = colors[6];
                        break;
                    case "Mermaid":
                        dialogueText.color = colors[6];
                        break;
                    case "Dad":
                        dialogueText.color = colors[7];
                        break;
                    default:
                        dialogueText.color = Color.black;
                        break;
                }
                // Add more character name checks and color assignments as needed

                //currentLineIndex++;
            }
            else
            {
                // This is a non-dialogue line
                nameBox.SetActive(false);
                dialogueText.text = line;
                dialogueText.color = Color.black;
                //currentLineIndex++;
            }
        }
    }


    void ShowChoices(string line)
    {
        //Hide the dialogue tbox
        dialogueBox.SetActive(false);


        // Parse the choices from the line
        string[] choices = line.Substring(1).Split('|');

        // Create a new panel to hold the choice buttons
        GameObject choicePanel = new GameObject("ChoicePanel", typeof(RectTransform));
        choicePanel.transform.SetParent(canvas.transform);
        choicePanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);

        // Position the panel above the dialogue text
        RectTransform panelRectTransform = choicePanel.GetComponent<RectTransform>();
        RectTransform textRectTransform = dialogueText.GetComponent<RectTransform>();
        panelRectTransform.anchoredPosition = textRectTransform.anchoredPosition + new Vector2(0, -190+ textRectTransform.sizeDelta.y);

        // Create a button for each choice
        for (int i = 0; i < choices.Length; i++)
        {
            if (i == 0) // First item is the label
            {

                // Create a parent object for the choice label and image
                GameObject choiceItem = new GameObject("ChoiceItem", typeof(RectTransform));
                choiceItem.transform.SetParent(choicePanel.transform);


                // Create the image component for the choice
                Image choiceImage = choiceItem.AddComponent<Image>();
                // Set the sprite or image for the choice image component
                choiceImage.sprite =InterfaceBox;



                // Create a label for the choices
                GameObject choiceLabel = new GameObject("ChoiceLabel", typeof(RectTransform), typeof(Text));
                choiceLabel.transform.SetParent(choiceItem.transform);
                Text choiceLabelText = choiceLabel.GetComponent<Text>();
                choiceLabelText.text = choices[i];
                choiceLabelText.fontSize = 20;
                choiceLabelText.fontStyle = FontStyle.Bold;
                choiceLabelText.color = Color.black;
                choiceLabelText.font = dialogueText.font;

                // Adjust the size of the label
                RectTransform labelRectTransform = choiceLabel.GetComponent<RectTransform>();
                labelRectTransform.anchorMin = new Vector2(0, 0);
                labelRectTransform.anchorMax = new Vector2(1, 1); // Set the anchor preset to stretch
                labelRectTransform.pivot = new Vector2(0.5f, 0.5f);

                // Calculate the preferred width and height of the text box
                float preferredWidth = choiceLabelText.preferredWidth + 10;
                float preferredHeight = 30f; // Set a fixed height that suits your design

                // Set the size of the choice item to match the preferred width and fixed height
                RectTransform choiceItemRectTransform = choiceItem.GetComponent<RectTransform>();
                choiceItemRectTransform.sizeDelta = new Vector2(preferredWidth, preferredHeight);

                // Set the position of the choice item within the panel
                choiceItemRectTransform.anchoredPosition = new Vector2(0, -preferredHeight / 2f);

                // Set the position of the choice image within the choice item
                RectTransform imageRectTransform = choiceImage.GetComponent<RectTransform>();
                //imageRectTransform.anchorMin = new Vector2(0, 0);
                //imageRectTransform.anchorMax = new Vector2(1, 1); // Set the anchor preset to stretch
                imageRectTransform.pivot = new Vector2(0.5f, 0.5f);
                imageRectTransform.anchoredPosition = Vector2.zero;

                // Adjust the size of the choice image to match the size of the choice item
                //choiceImage.GetComponent<RectTransform>().sizeDelta = choiceItemRectTransform.sizeDelta;

                /*Left*/
                labelRectTransform.offsetMin = new Vector2(0,0);
                labelRectTransform.offsetMax = new Vector2(0, 0);
                /*Right*/
                //rectTransform.offsetMax.x;
                /*Top*/
                //rectTransform.offsetMax.y;
                /*Bottom*/
                //rectTransform.offsetMin.y;





            }
            else // Other items are the choices
            {
                string choiceText = choices[i];
                GameObject choiceButton = Instantiate(buttonPrefab, choicePanel.transform);
                choiceButton.GetComponentInChildren<Text>().text = choiceText;

                // Adjust the vertical position of the button
                RectTransform buttonRectTransform = choiceButton.GetComponent<RectTransform>();
                buttonRectTransform.anchoredPosition = new Vector2(0, -50 * i);

                choiceButton.GetComponent<Button>().onClick.AddListener(() => ChooseOption(choiceText));
            }
        }

        // Disable the dialogue text until the player has made a choice
        dialogueText.enabled = false;
        isChoiceDisplayed = true;
        isSkipping = false;



    }


    void ChooseOption(string choiceText)
    {
        // Disable the choice panel and re-enable the dialogue text
        Destroy(GameObject.Find("ChoicePanel"));
        dialogueText.enabled = true;
        isChoiceDisplayed = false;
        dialogueBox.SetActive(true);

        // Load the appropriate text file for the selected choice
        Debug.Log("Choice: "+ choiceText);
        string fileName = choiceFileMap[choiceText]; // fill in with the appropriate file name
        dataFilePath = choiceFileMap[choiceText];
        currentAct = choiceFileMap[choiceText].Remove(0,5);
        LoadDialogueData();
        TextAsset textAsset = Resources.Load<TextAsset>(dataFilePath);

        // Reset the CurrentLineIndex variable to zero
        currentLineIndex = 0;

        // Set the dialogue text to the first line of the new file
        dialogueText.text = dialogueLines[currentLineIndex];
        backlogText.text = dialogueLines[currentLineIndex];
        
        // set the new image and music for the first line
        imageChange(currentAct, currentLineIndex, false);
        musicChange(currentAct, currentLineIndex);


    }

    public void hideText()
    {

        dialogueBox.SetActive(!dialogueBox.activeSelf);
        isSkipping = false;

    }

    public void skip()
    {
        isSkipping = !isSkipping;
    }

    public void backlog()
    {
        backlogPanel.SetActive(true);
        backlogLine = 1;
        backlogText.text = dialogueLines[currentLineIndex-backlogLine];
        backlogButton.SetActive(false);

        Debug.Log("Backlog Enabled");
    }

    public void backlogPlus()
    {
        if (currentLineIndex - (backlogLine + 1) <= 0)
        {
            Debug.Log("Backlog can't go further!");
            return;
        }
        else
        {
            backlogLine += 1;
            backlogText.text = dialogueLines[currentLineIndex - backlogLine];

        }

    }

    public void backlogMinus()
    {
        if (backlogLine == 1)
        {
            backlogPanel.SetActive(false);
            backlogButton.SetActive(true);
            Debug.Log("Backlog reached current line");
            return;
        }
        else
        {
            backlogLine -= 1;
            backlogText.text = dialogueLines[currentLineIndex - backlogLine];

        }
    }

    public void imageChange(string act, int currentLine, bool loading)
    {

        if (loading == true)
        {

            

            if (imageChangingLinesByAct != null)
            {
                if (imageChangingLinesByAct.ContainsKey(act))
                {
                    Debug.Log("Yessss~  Load " + act);
                    Dictionary<int, int> imageChangingLines = imageChangingLinesByAct[act];
                    int previousKey = -1; // Variable to store the key with a value immediately lesser



                    foreach (var key in imageChangingLines.Keys)
                    {
                        if (key < currentLine && (previousKey == -1 || key > previousKey))
                        {
                            previousKey = key;
                        }
                    }

                    Debug.Log(previousKey);
                    if (previousKey == -1)
                    {
                        Background.sprite = Sprite.Create(BackgroundImages[imageChangingLines[0]], new Rect(0.0f, 0.0f, BackgroundImages[imageChangingLines[0]].width, BackgroundImages[imageChangingLines[0]].height), new Vector2(0.5f, 0.5f), 100.0f);

                    }
                    else
                    {
                        Background.sprite = Sprite.Create(BackgroundImages[imageChangingLines[previousKey]], new Rect(0.0f, 0.0f, BackgroundImages[imageChangingLines[previousKey]].width, BackgroundImages[imageChangingLines[previousKey]].height), new Vector2(0.5f, 0.5f), 100.0f);
                    }







                }
            }













        }
        else
        {

            if (imageChangingLinesByAct.ContainsKey(act))
            {
                Debug.Log(act);
                Dictionary<int, int> imageChangingLines = imageChangingLinesByAct[act];



                if (imageChangingLines.ContainsKey(currentLine))
                {
                    Debug.Log(currentLine);
                    // Set the background image
                    Background.sprite = Sprite.Create(BackgroundImages[imageChangingLines[currentLine]], new Rect(0.0f, 0.0f, BackgroundImages[imageChangingLines[currentLine]].width, BackgroundImages[imageChangingLines[currentLine]].height), new Vector2(0.5f, 0.5f), 100.0f);
                }
            }
            else
            {
                Debug.Log("act " + act + " not contained!");
            }

        }


    }

    public void musicChange(string act, int currentLine)
    {
        if (songChangingLinesByAct.ContainsKey(act))
        {
            Dictionary<int, int> songChangingLines = songChangingLinesByAct[act];
            if (songChangingLines.ContainsKey(currentLine))
            {
                BGM.clip = Music[songChangingLines[currentLine]];
                BGM.Play();
            }
        }

        else
        {
            Debug.Log("Music: Act not contained!");
        }


    }

    public string GetCurrentMusicFile()
    {
        if (BGM.clip != null)
        {
            return BGM.clip.name; // Assuming the music file name is used as the clip name
        }
        else
        {
            return string.Empty; // No music is currently playing
        }
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("TitleScreen");
    }


    void Update()
    {
        if (isSkipping)
        {
            currentLineIndex++;
            ShowNextLine();
        }

        if (Input.GetMouseButtonDown(0))
        {

            Debug.Log("Click Detected.");


            if (backlogPanel.activeInHierarchy)
            {
                // Check if the user is clicking the BacklogPlusButton or BacklogMinusButton
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    if (EventSystem.current.currentSelectedGameObject == backlogPlusButton || EventSystem.current.currentSelectedGameObject == backlogMinusButton)
                    {
                        // Clicked on one of the buttons, do not set backlogPanel to inactive
                        return;
                    }
                }

                // Clicked outside of the buttons, set backlogPanel to inactive
                backlogPanel.SetActive(false);
                backlogButton.SetActive(true);  // Set backlog button back to active
                return;
            }

            if (EventSystem.current.IsPointerOverGameObject())
            {
                if (EventSystem.current.currentSelectedGameObject == hideTextButton)
                {
                    // Hide the dialogue box if it is active
                    if (dialogueBox.activeSelf)
                    {
                        dialogueBox.SetActive(false);
                        return;
                    }
                }
                else if (EventSystem.current.currentSelectedGameObject == backlogButton)
                {
                    Debug.Log("backlog button clicked");
                    backlog();
                    return;
                }

                else if (EventSystem.current.currentSelectedGameObject == saveButton)
                {
                    Debug.Log("Save clicked");
                    return;

                }

                else if (EventSystem.current.currentSelectedGameObject == loadButton)
                {
                    Debug.Log("Load clicked");
                    return;
                }




            }

            if (isChoiceDisplayed)
            {
                Debug.Log("Choice Displayed.");
                return;
            }
            else if (dialogueBox.activeSelf == false)
            {
                dialogueBox.SetActive(true);
                
                return;
            }

            // Clicked outside of the buttons, call ShowNextLine
            currentLineIndex++;
            ShowNextLine();
        }
    }

}