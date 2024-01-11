using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;
using KModkit;
using Newtonsoft.Json;

public class labyrinthMadness : MonoBehaviour {

    public KMAudio Audio;
    public KMBomb Bomb;
    public KMBombModule Module;
    public KMSelectable[] Buttons;
    public MeshRenderer[] Cells;
    public Material[] Lights;
    public TextMesh screenTextEmotion;
    Renderer rend;

    //Logging and Random Variable
    static int moduleIdCounter = 1;
    int moduleId = 0;
    System.Random rnd = new System.Random();

    //Maze layer 2D arrays
    //int[,] blankMaze = { { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0 } };
    //Maze Rotations
    int[,] normalWalls0 = { { 5, 7, 1, 7, 7, 1, 7, 7, 6 }, { 8, 5, 4, 1, 7, 4, 7, 6, 8 }, { 8, 8, 5, 4, 7, 1, 6, 8, 8 }, { 2, 3, 2, 1, 7, 3, 8, 2, 3 }, { 8, 8, 8, 2, 13, 8, 2, 3, 8 }, { 8, 2, 3, 9, 7, 3, 8, 8, 8 }, { 8, 8, 9, 7, 1, 4, 10, 8, 8 }, { 2, 4, 7, 1, 4, 7, 7, 3, 8 }, { 9, 7, 7, 4, 7, 7, 7, 4, 10 } };
    int[,] normalWalls90 = { { 5, 1, 7, 7, 7, 1, 7, 7, 6 }, { 8, 2, 7, 1, 7, 4, 7, 6, 8 }, { 8, 8, 5, 4, 7, 1, 6, 2, 3 }, { 2, 3, 8, 5, 1, 3, 2, 3, 8 }, { 8, 2, 3, 8, 14, 8, 8, 8, 8 }, { 8, 8, 2, 4, 7, 4, 3, 2, 3 }, { 8, 8, 9, 7, 1, 7, 10, 8, 8 }, { 2, 4, 7, 7, 4, 1, 7, 10, 8 }, { 9, 7, 7, 7, 7, 4, 7, 7, 10 } };
    int[,] normalWalls180 = { { 5, 1, 7, 7, 7, 1, 7, 7, 6 }, { 8, 2, 7, 7, 1, 4, 7, 1, 3 }, { 8, 8, 5, 1, 4, 7, 6, 8, 8 }, { 8, 8, 8, 2, 7, 6, 2, 3, 8 }, { 8, 2, 3, 8, 12, 3, 8, 8, 8 }, { 2, 3, 8, 2, 7, 4, 3, 2, 3 }, { 8, 8, 9, 4, 7, 1, 10, 8, 8 }, { 8, 9, 7, 1, 7, 4, 1, 10, 8 }, { 9, 7, 7, 4, 7, 7, 4, 7, 10 } };
    int[,] normalWalls270 = { { 5, 7, 7, 1, 7, 7, 7, 7, 6 }, { 8, 5, 7, 4, 1, 7, 7, 1, 3 }, { 8, 8, 5, 7, 4, 7, 6, 8, 8 }, { 2, 3, 2, 1, 7, 1, 3, 8, 8 }, { 8, 8, 8, 8, 11, 8, 2, 3, 8 }, { 8, 2, 3, 2, 4, 10, 8, 2, 3 }, { 2, 3, 9, 4, 7, 1, 10, 8, 8 }, { 8, 9, 7, 1, 7, 4, 7, 3, 8 }, { 9, 7, 7, 4, 7, 7, 7, 4, 10 } };
    //Maze layers
    int[,] tempWalls = new int[9, 9];
    int[,] wallsLayer = new int[9, 9];
    int[,] playerLayer = new int[9, 9];
    int[,] sludgeLayer = new int[9, 9];
    int[,] tempSludgeLayer = new int[9, 9];
    int[,] shadowLayer = new int[9, 9];
    int[,] tempShadowLayer = new int[9, 9];
    //Emotion list
    string[] emotions = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "u", "v", "w", "x", "y", "z" };
    string[] smileyEyesNorm = { "a", "c", "k", "n", "p" };
    string[] smileyEyesAngry = { "e", "h", "r", "w", "x" };
    string[] smileyEyesSad = { "f", "i", "l", "u", "z" };
    string[] smileyEyesQuestion = { "b", "d", "g", "j", "m", "o", "q", "s", "v", "y" };
    string[] smileyMouthSmile = { "a", "g", "h", "l", "o" };
    string[] smileyMouthLaugh = { "k", "q", "r", "y", "z" };
    string[] smileyMouthFrown = { "d", "j", "n", "u", "x" };
    string[] smileyMouthSad = { "b", "c", "e", "i", "s" };
    //Varriables
    string sludgeName = "";
    int tempRandom, tempInt;
    int playerSpawnPos;
    int playerXPos;
    int playerYPos;
    int sludgeXPos;
    int sludgeYPos;
    int shadowXPos;
    int shadowYPos;
    bool moveUp, moveDown, moveRight, moveLeft;
    int sludgeMovementSpaces;
    int sludgeMovementDir;
    string displayedEmotion;
    int sludgeSpawningXPos;
    int sludgeSpawningYPos;
    int sludgeSpawningDir;
    int checkAdjacentCellXOffset;
    int checkAdjacentCellYOffset;
    int spawnGrejnottConfig;
    int sludgeSpawnBodySegmentIndex;
    int sludgeMoveBodyTemp;
    int sludgeLength;
    bool noPlayer, noSludge, noShadow;
    int sludgeHeadMaterial;
    int sludgeBodyMaterial;
    int numOfMoves;
    int tempSludgeMovementDir;
    bool sludgeAlive;
    int blegsharSearchIndex;
    //int spawningGrejnottX;
    //int spawningGrejnottY;
    //int[] grejnottSegmentCoords = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
    //int[] tempGrejnottSegmentCoords = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    //int[] grejnottSpawningConfig0 = { 0, 0, 0, 2, 1, 3, 2, 1, 3, 2 };
    //int[] grejnottSpawningConfig1 = { 0, 2, 1, 1, 1, 3, 2, 0, 3, 2 };
    //int[] grejnottSpawningConfig2 = { 0, 2, 1, 0, 2, 2, 3, 1, 3, 3 };
    //int[] grejnottSpawningConfig3 = { 0, 0, 0, 3, 1, 1, 2, 0, 3, 3 };
    int movementIndex;
    bool moduleSolved;
    //Sludge Names
    const string Blegshar = "Blegshar";
    const string Grejnott = "Grejnott";
    const string Vipaeviox = "Vipaeviox";
    const string Awshfadak = "Awshfadak";
    const string Zyquizime = "Zyquizime";
    //Direction Names
    const int Up = 0;
    const int Left = 1;
    const int Down = 2;
    const int Right = 3;


    // Do stuff here
    void Start()
    {
        resetMaze();
	}

    // Set up
    void Awake()
    {
        moduleId = moduleIdCounter;
        moduleIdCounter++;
        foreach (KMSelectable button in Buttons)
        {
            KMSelectable pressedButton = button;
            button.OnInteract += delegate () { ButtonPress(pressedButton); return false; };
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if (sludgeAlive)
        {
            if (sludgeLayer[playerYPos, playerXPos] > 0 || (sludgeXPos == playerXPos && sludgeYPos == playerYPos)) resetMaze();
            if (sludgeName.Equals(Blegshar) && shadowLayer[playerYPos, playerXPos] == -1) resetMaze();
            /*/if (sludgeName.Equals(Grejnott))
            {
                for (int i = 0; i < 5; i++)
                {
                    if (grejnottSegmentCoords[2 * i] == playerYPos && grejnottSegmentCoords[2 * i + 1] == playerXPos) resetMaze();
                }
            }/*/
        }
        else
        {
            screenTextEmotion.text = "t";
        }
        if (playerXPos == 4 && playerYPos == 4 && moduleSolved == false)
        {
            Module.HandlePass();
            moduleSolved = true;
        }
    }

    void ButtonPress(KMSelectable button)
    {
        button.AddInteractionPunch(0.5f);
        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
        numOfMoves++;
        if (button.GetComponentInChildren<TextMesh>().text.Equals("Y"))
        {
            if (evalMovement(playerYPos, playerXPos, Up, playerLayer)) performPlayerMovement(Up);
        }
        else if (button.GetComponentInChildren<TextMesh>().text.Equals("B"))
        {
            if (evalMovement(playerYPos, playerXPos, Right, playerLayer)) performPlayerMovement(Right);
        }
        else if (button.GetComponentInChildren<TextMesh>().text.Equals("X"))
        {
            if (evalMovement(playerYPos, playerXPos, Left, playerLayer)) performPlayerMovement(Left);
        }
        else if (button.GetComponentInChildren<TextMesh>().text.Equals("A"))
        {
            if (evalMovement(playerYPos, playerXPos, Down, playerLayer)) performPlayerMovement(Down);
        }

        //Move the Sludge
        if (sludgeAlive)
        {
            identifySludgeMovement(screenTextEmotion.text);
            tempSludgeMovementDir = sludgeMovementDir;
            for (int i = 0; i < sludgeMovementSpaces; i++)
            {
                movementIndex = 0;
                for (int j = 0; j < 4; j++)
                {
                    if (!evalMovement(sludgeYPos, sludgeXPos, sludgeMovementDir, sludgeLayer) || checkAdjacentCell(sludgeYPos, sludgeXPos, sludgeMovementDir, wallsLayer) > 10)
                    {
                        sludgeMovementDir = (sludgeMovementDir + 1) % 4;
                    }
                }
                if (tempSludgeMovementDir == sludgeMovementDir && movementIndex == 4) sludgeAlive = false;
                else moveSludge(sludgeMovementDir);
            }
            /*/else
            {
                for (int i = 0; i < 5; i++)
                {
                    identifySludgeMovement(screenTextEmotion.text);
                    tempSludgeMovementDir = sludgeMovementDir;
                    for (int j = 0; j < sludgeMovementSpaces; j++)
                    {
                        movementIndex = 0;
                        for (int k = 0; k < 4; k++)
                        {
                            if (!evalMovement(grejnottSegmentCoords[2 * i], grejnottSegmentCoords[2 * i + 1], sludgeMovementDir, sludgeLayer) || checkAdjacentCell(grejnottSegmentCoords[2 * i], grejnottSegmentCoords[2 * i + 1], sludgeMovementDir, wallsLayer) > 10)
                            {
                                sludgeMovementDir = (sludgeMovementDir + 1) % 4;
                                movementIndex++;
                            }
                        }
                        if (tempSludgeMovementDir == sludgeMovementDir && movementIndex == 4)
                        {
                            grejnottSegmentCoords[2 * i] = 69;
                            grejnottSegmentCoords[2 * i + 1] = 69;
                        }
                        Debug.Log("The Grejnott segment at " + grejnottSegmentCoords[2 * i] + ", " + grejnottSegmentCoords[2 * i + 1] + " has moved in direction " + sludgeMovementDir + " one space.");
                        moveSludge(sludgeMovementDir, i);
                    }
                    if (grejnottSegmentCoords[2 * i] < 9 && grejnottSegmentCoords[2 * i + 1] < 9) sludgeLayer[grejnottSegmentCoords[2 * i], grejnottSegmentCoords[2 * i + 1]] = i + 1;
                }
            }/*/
            updateLayers();

            //Change emotion
            resetEmotion();
        }
    }

    void resetMaze()
    {
        //Reset and rotate The Maze by a random amount
        tempRandom = rnd.Next(0, 4);
        if (tempRandom == 0) wallsLayer = normalWalls0;
        else if (tempRandom == 1) wallsLayer = normalWalls90;
        else if (tempRandom == 2) wallsLayer = normalWalls180;
        else wallsLayer = normalWalls270;

        //Reset and spawn the player and sludge
        resetLayer(playerLayer);
        resetLayer(sludgeLayer);
        resetLayer(tempSludgeLayer);
        resetLayer(shadowLayer);
        sludgeAlive = false;

        numOfMoves = 0;
        tempRandom = rnd.Next(0, 4);
        if (tempRandom == 0)
        {
            playerXPos = 0;
            playerYPos = 0;
        }
        else if (tempRandom == 1)
        {
            playerXPos = 8;
            playerYPos = 0;
        }
        else if (tempRandom == 2)
        {
            playerXPos = 8;
            playerYPos = 8;
        }
        else
        {
            playerXPos = 0;
            playerYPos = 8;
        }
        sludgeSpawningXPos = 8 - playerXPos;
        sludgeSpawningYPos = 8 - playerYPos;
        playerLayer[playerYPos, playerXPos] = 1;
        sludgeName = randomSludge();
        resetEmotion();
        spawnConjoinedSludge(sludgeSpawningYPos, sludgeSpawningXPos, sludgeMovementDir, sludgeLength);
        updateLayers();
    }

    string randomSludge()
    {
        tempRandom = rnd.Next(0, 3);
        sludgeLength = rnd.Next(3, 8);
        if (tempRandom == 0)
        {
            sludgeHeadMaterial = 4;
            sludgeBodyMaterial = 5;
            return Blegshar;
        }
        /*/else if (tempRandom == 1)
        {
            sludgeHeadMaterial = 6;
            sludgeBodyMaterial = 6;
            return Grejnott;
        }/*/
        else if (tempRandom == 1)
        {
            sludgeHeadMaterial = 7;
            sludgeBodyMaterial = 8;
            return Vipaeviox;
        }
        else
        {
            sludgeHeadMaterial = 2;
            sludgeBodyMaterial = 3;
            return Awshfadak;
        }
        /*/else
        {
            sludgeHeadMaterial = 9;
            sludgeBodyMaterial = 10;
            return Zyquizime;
        }/*/
    }

    void updateLayers()
    {
        if (sludgeName.Equals(Blegshar)) blegsharSight(sludgeYPos, sludgeXPos);
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                Cells[9 * i + j].sharedMaterial = Lights[0];
                if (sludgeName.Equals(Grejnott) && sludgeLayer[i, j] > 0)
                {
                    if (sludgeLayer[i, j] == 1)
                    {
                        Cells[9 * i + j].sharedMaterial = Lights[7];
                    }
                    else if (sludgeLayer[i, j] == 2)
                    {
                        Cells[9 * i + j].sharedMaterial = Lights[6];
                    }
                    else if (sludgeLayer[i, j] == 3)
                    {
                        Cells[9 * i + j].sharedMaterial = Lights[4];
                    }
                    else if (sludgeLayer[i, j] == 4)
                    {
                        Cells[9 * i + j].sharedMaterial = Lights[2];
                    }
                    else if (sludgeLayer[i, j] == 5)
                    {
                        Cells[9 * i + j].sharedMaterial = Lights[9];
                    }
                }
                if (sludgeLayer[i, j] > 0 && sludgeLayer[i, j] <= sludgeLength && sludgeAlive)
                {
                    Cells[9 * i + j].sharedMaterial = Lights[sludgeBodyMaterial];
                    if (sludgeName.Equals(Blegshar)) blegsharSight(i, j);
                }
            }
        }
        if (sludgeName.Equals(Blegshar))
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (shadowLayer[i, j] == -1 && !(sludgeLayer[i, j] > 0)) Cells[9 * i + j].sharedMaterial = Lights[8];
                }
            }
        }
        Cells[9 * playerYPos + playerXPos].sharedMaterial = Lights[1];
        if (sludgeAlive && !sludgeName.Equals(Grejnott))
        {
            Cells[9 * sludgeYPos + sludgeXPos].sharedMaterial = Lights[sludgeHeadMaterial];
        }
    }

    bool evalMovement(int y, int x, int dir, int[,] layer)
    {
        if (y > 8 || y < 0 || x > 8 || x < 0)
        {
            sludgeAlive = false;
            return false;
        }
        moveUp = true;
        moveDown = true;
        moveRight = true;
        moveLeft = true;
        tempInt = wallsLayer[y, x];
        if (dir == Up && (!(tempInt == 2 || tempInt == 3 || tempInt == 4 || tempInt == 8 || tempInt == 9 || tempInt == 10 || tempInt == 14) || checkAdjacentCell(y, x, dir, layer) != 0 || checkAdjacentCell(y, x, dir, layer) == 69))
        {
            moveUp = false;
        }
        if (dir == Right && (!(tempInt == 1 || tempInt == 2 || tempInt == 4 || tempInt == 5 || tempInt == 7 || tempInt == 9 || tempInt == 12) || checkAdjacentCell(y, x, dir, layer) != 0 || checkAdjacentCell(y, x, dir, layer) == 69))
        {
            moveRight = false;
        }
        if (dir == Left && (!(tempInt == 1 || tempInt == 3 || tempInt == 4 || tempInt == 6 || tempInt == 7 || tempInt == 10 || tempInt == 13) || checkAdjacentCell(y, x, dir, layer) != 0 || checkAdjacentCell(y, x, dir, layer) == 69))
        {
            moveLeft = false;
        }
        if (dir == Down && (!(tempInt == 1 || tempInt == 2 || tempInt == 3 || tempInt == 5 || tempInt == 6 || tempInt == 8 || tempInt == 11) || checkAdjacentCell(y, x, dir, layer) != 0 || checkAdjacentCell(y, x, dir, layer) == 69))
        {
            moveDown = false;
        }

        if (!moveUp && !moveLeft && !moveDown && !moveRight)
        {
            sludgeAlive = false;
            return false;
        }

        if (dir == Up)
        {
            return moveUp;
        }
        else if (dir == Left)
        {
            return moveLeft;
        }
        else if (dir == Down)
        {
            return moveDown;
        }
        else
        {
            return moveRight;
        }
    }

    void performPlayerMovement(int dir)
    {
        if (dir == Up)
        {
            playerLayer[playerYPos, playerXPos] = 0;
            playerYPos--;
            playerLayer[playerYPos, playerXPos] = 1;
        }
        else if (dir == Right)
        {
            playerLayer[playerYPos, playerXPos] = 0;
            playerXPos++;
            playerLayer[playerYPos, playerXPos] = 1;
        }
        else if (dir == Down)
        {
            playerLayer[playerYPos, playerXPos] = 0;
            playerYPos++;
            playerLayer[playerYPos, playerXPos] = 1;
        }
        else if (dir == Left)
        {
            playerLayer[playerYPos, playerXPos] = 0;
            playerXPos--;
            playerLayer[playerYPos, playerXPos] = 1;
        }
    }

    void identifySludgeMovement(string emotion)
    {
        if (smileyEyesNorm.Contains(emotion)) sludgeMovementSpaces = 2;
        else if (smileyEyesAngry.Contains(emotion)) sludgeMovementSpaces = 3;
        else if (smileyEyesSad.Contains(emotion)) sludgeMovementSpaces = 1;
        else sludgeMovementSpaces = 2;

        if (smileyMouthSmile.Contains(emotion))
        {
            sludgeMovementDir = Up;
        }
        else if (smileyMouthSad.Contains(emotion))
        {
            sludgeMovementDir = Down;
        }
        else if (smileyMouthLaugh.Contains(emotion))
        {
            sludgeMovementDir = Left;
        }
        else if (smileyMouthFrown.Contains(emotion))
        {
            sludgeMovementDir = Right;
        }
        else
        {
            sludgeMovementDir = Up;
        }
    }

    void spawnConjoinedSludge(int y, int x, int preferedDir, int sLength)
    {
        resetLayer(tempSludgeLayer);
        tempSludgeLayer[sludgeSpawningYPos, sludgeSpawningXPos] = 1;
        sludgeXPos = sludgeSpawningXPos;
        sludgeYPos = sludgeSpawningYPos;
        sludgeSpawnBodySegmentIndex = 1;

        for (int i = 0; i < sLength - 1; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (evalMovement(sludgeSpawningYPos, sludgeSpawningXPos, (preferedDir + j) % 4, sludgeLayer))
                {
                    sludgeSpawningDir = (preferedDir + j) % 4;
                    if (sludgeSpawningDir == Up) sludgeSpawningYPos--;
                    else if (sludgeSpawningDir == Left) sludgeSpawningXPos--;
                    else if (sludgeSpawningDir == Down) sludgeSpawningYPos++;
                    else if (sludgeSpawningDir == Right) sludgeSpawningXPos++;
                    j = 4;
                }
            }
            sludgeSpawnBodySegmentIndex++;
            tempSludgeLayer[sludgeSpawningYPos, sludgeSpawningXPos] = sludgeSpawnBodySegmentIndex;
            sludgeLayer = tempSludgeLayer;
        }
        sludgeAlive = true;
    }

    /*/void spawnGrejnott()
    {
        tempRandom = rnd.Next(0, 4);
        if (tempRandom == 0)
        {
            Debug.Log("Spawning Grejnott at " + spawningGrejnottY + ", " + spawningGrejnottX);
            grejnottSegmentCoords = grejnottSpawningConfig0;
        }
        else if (tempRandom == 1)
        {
            grejnottSegmentCoords = grejnottSpawningConfig1;
        }
        else if (tempRandom == 2)
        {
            grejnottSegmentCoords = grejnottSpawningConfig2;
        }
        else
        {
            grejnottSegmentCoords = grejnottSpawningConfig3;
        }
        for (int i = 0; i < 5; i++)
        {
            grejnottSegmentCoords[2 * i] += spawningGrejnottY;
            grejnottSegmentCoords[2 * i + 1] += spawningGrejnottX;
            sludgeLayer[grejnottSegmentCoords[2 * i], grejnottSegmentCoords[2 * i + 1]] = i + 1;
        }
        sludgeAlive = true;
    }/*/

    void moveSludge(int dir)
    {
        if (sludgeName.Equals(Blegshar))
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (shadowLayer[i, j] == -1) shadowLayer[i, j] = 0;
                }
            }
        }
        if (sludgeName.Equals(Vipaeviox))
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (shadowLayer[i, j] < 0) shadowLayer[i, j] += 1;
                }
            }
        }
        tempSludgeLayer = sludgeLayer;
        if (dir == Up)
        {
            sludgeYPos--;
            tempSludgeLayer[sludgeYPos + 1, sludgeXPos] = 1;
        }
        else if (dir == Left)
        {
            sludgeXPos--;
            tempSludgeLayer[sludgeYPos, sludgeXPos + 1] = 1;
        }
        else if (dir == Down)
        {
            sludgeYPos++;
            tempSludgeLayer[sludgeYPos - 1, sludgeXPos] = 1;
        }
        else if (dir == Right)
        {
            sludgeXPos++;
            tempSludgeLayer[sludgeYPos, sludgeXPos - 1] = 1;
        }

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (tempSludgeLayer[i, j] > 0)
                {
                    tempSludgeLayer[i, j] += 1;
                }
                if (tempSludgeLayer[i, j] > sludgeLength)
                {
                    tempSludgeLayer[i, j] = 0;
                }
            }
        }
        sludgeLayer = tempSludgeLayer;
    }

    /*/void moveSludge(int dir, int index)
    {
        if (grejnottSegmentCoords[2 * index] < 9 && grejnottSegmentCoords[2 * index + 1] < 9)
        {
            if (dir == Up)
            {
                grejnottSegmentCoords[2 * index] -= 1;
                sludgeLayer[grejnottSegmentCoords[2 * index] + 1, grejnottSegmentCoords[2 * index + 1]] = 0;
            }
            else if (dir == Left)
            {
                grejnottSegmentCoords[2 * index + 1] -= 1;
                sludgeLayer[grejnottSegmentCoords[2 * index], grejnottSegmentCoords[2 * index + 1] + 1] = 0;
            }
            else if (dir == Down)
            {
                grejnottSegmentCoords[2 * index] += 1;
                sludgeLayer[grejnottSegmentCoords[2 * index] - 1, grejnottSegmentCoords[2 * index + 1]] = 0;
            }
            else if (dir == Right)
            {
                grejnottSegmentCoords[2 * index + 1] += 1;
                sludgeLayer[grejnottSegmentCoords[2 * index], grejnottSegmentCoords[2 * index + 1] - 1] = 0;
            }
        }
    }/*/

    void blegsharSight(int y, int x)
    {
        //Check Up
        blegsharSearchIndex = 0;
        while(evalMovement(y - blegsharSearchIndex, x, Up, sludgeLayer))
        {
            blegsharSearchIndex += 1;
            shadowLayer[y - blegsharSearchIndex, x] = -1;
        }
        //Check Left
        blegsharSearchIndex = 0;
        while (evalMovement(y, x - blegsharSearchIndex, Left, sludgeLayer))
        {
            blegsharSearchIndex += 1;
            shadowLayer[y, x - blegsharSearchIndex] = -1;
        }
        //Cehck Down
        blegsharSearchIndex = 0;
        while (evalMovement(y + blegsharSearchIndex, x, Down, sludgeLayer))
        {
            blegsharSearchIndex += 1;
            shadowLayer[y + blegsharSearchIndex, x] = -1;
        }
        //Check Right
        blegsharSearchIndex = 0;
        while (evalMovement(y, x + blegsharSearchIndex, Right, sludgeLayer))
        {
            blegsharSearchIndex += 1;
            shadowLayer[y, x + blegsharSearchIndex] = -1;
        }
        shadowLayer[4, 4] = 0;
    }

    int checkAdjacentCell(int y, int x, int dir, int[,] layer)
    {
        if (dir == Up) y -= 1;
        else if (dir == Left) x -= 1;
        else if (dir == Down) y += 1;
        else if (dir == Right) x += 1;

        if (x < 0 || x > 8) return 69;
        if (y < 0 || y > 8) return 69;
        else return layer[y, x];
    }

    void resetLayer(int[,] layer)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                layer[i, j] = 0;
            }
        }
    }

    void resetEmotion()
    {
        if (sludgeName.Equals(Blegshar))
        {
            tempRandom = rnd.Next(0, 5);
            screenTextEmotion.text = smileyEyesSad[tempRandom];
        }
        else if (sludgeName.Equals(Vipaeviox))
        {
            tempRandom = rnd.Next(0, 5);
            screenTextEmotion.text = smileyEyesAngry[tempRandom];
        }
        else
        {
            tempRandom = rnd.Next(0, 25);
            screenTextEmotion.text = emotions[tempRandom];
        }
        identifySludgeMovement(screenTextEmotion.text);
    }
}
