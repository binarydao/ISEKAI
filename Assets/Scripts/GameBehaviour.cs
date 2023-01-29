using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;

public class GameBehaviour : MonoBehaviour
{
    public static GameObject GameBackground;

    private const int BASE_CHIP_TYPES = 6;
    private const int TIME_BEFORE_HINT = 1000000;

    //min 4, max 9
    private const int MAX_ROWS = 8;

    //min 4, max 18
    private const int MAX_COLS = 8;

    internal static GameBehaviour instance;

    private static ChipBehaviour[,] chipArray = new ChipBehaviour[MAX_ROWS, MAX_COLS];


    private static Vector4 noMatchVector4 = new Vector4(-1, -1, -1, -1);


    //private readonly float[,] previousPositionY = new float[MAX_ROWS, MAX_COLS];
    private AudioSource audioSource;

    private int[] bonusRow = new int[MAX_COLS];

    //how much chips we should wait before reenabling physics?
    internal int destroyWaiting;

    private GameObject field;
    internal float fieldHalfHeight;

    internal float fieldHalfWidth;
    public static bool isFieldActive;
    private static bool isHintShowed;

    private bool isMovingBack;
    private readonly bool isMuted;
    private bool isPaused;

    private bool isWaitingChipsFall;

    private AudioClip matchSound;

    //we should get both chips moving end before processing next
    private int movingCounter;
    private GameObject pauseButton;

    private static Random random;

    private int scorePoints;

    private ChipBehaviour secondChip;

    public static ChipBehaviour selectedChip;

    private static float startTurnTime;

    private int turnsLeft;

    private List<ChipBehaviour> DeleteChipsList;



    private static int HeroHP = 20;
    private static int HeroMaxHP = 20;
    private static int EnemyHP = 20;
    private static int EnemyMaxHP = 20;

    private static bool Was4Plus = false;

    private static GameObject StateCaption;

    private static int EnemyDamage;

    private static int DamageAccumulated;

    public static LootStruct GlobalLoot = new LootStruct();
    public static LootStruct LocalLoot = new LootStruct();

    private bool IsAutoBattle = false;
    private bool IsPlayerTurn;

    public static bool GameOver;

    // Use this for initialization
    private void Start()
    {
        instance = this;

        GameBackground = GameObject.Find("Game Background");

        IsAutoBattle = false;
        IsPlayerTurn = true;

        random = new Random();
        GenerateField();
        CenterField();

        audioSource = gameObject.AddComponent<AudioSource>();
        matchSound = (AudioClip)Resources.Load("Sounds/matchSound");

        isHintShowed = true;

        GameObject enemyIcon = GameObject.Find("EnemyPortrait");
        SpriteRenderer enemySprite = enemyIcon.GetComponent<SpriteRenderer>();
        enemySprite.sprite = Resources.Load<Sprite>("Enemies/Enemy" + MapLogic.enemyId);

        StateCaption = GameObject.Find("StateCaption");

        HeroHP = 20;
        EnemyHP = 20;

        if(GlobalLoot.attack == 0)
        {
            GlobalLoot.attack = 1;
            GlobalLoot.defense = 1;
        }

        LocalLoot = new LootStruct();

        RefreshLabels();

        GameOver = false;
        EnablePlayerControl(true);
    }

    internal int ScorePoints
    {
        get { return scorePoints; }

        set
        {
            scorePoints = value;
            UpdateScore();
        }
    }

    private int TurnsLeft
    {
        get { return turnsLeft; }

        set
        {
            turnsLeft = value;
            UpdateScore();
        }
    }
    
    private void FixedUpdate()
    {
        //some chips are still falling using physics?
        if (isWaitingChipsFall)
        {
            isWaitingChipsFall = false;
            for (int i = 0; i < MAX_ROWS; i++)
            {
                for (int j = 0; j < MAX_COLS; j++)
                {
                    if (chipArray[i, j].transform.position.y > -3.4 + chipArray[i, j].row*ChipBehaviour.ICON_HEIGHT)
                    {
                        chipArray[i, j].transform.position = new Vector3(chipArray[i, j].transform.position.x, chipArray[i, j].transform.position.y - 0.1f, chipArray[i, j].transform.position.z);
                        isWaitingChipsFall = true;
                    }
                        
                }
            }

            if (!isWaitingChipsFall)
            {
                if (!FullMatchCheck())
                {
                    if(IsPlayerTurn)
                        NextTurnHandler();
                }
            }
        }
        else
        {
            if (!isHintShowed && Time.time - startTurnTime > TIME_BEFORE_HINT)
            {
                ShowHint();
            }
        }
    }

    public static void Win()
    {
        GameOver = true;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("RewardWindow", LoadSceneMode.Additive);
        RewardWindow.Win();
    }

    public static void Lose()
    {
        GameOver = true;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("RewardWindow", LoadSceneMode.Additive);
        RewardWindow.Lose();
    }


    private void NextTurnHandler()
    {
        CheckWinLose();
        IsPlayerTurn = !IsPlayerTurn;
        if (IsPlayerTurn)
            TurnToPlayer();
        else
            TurnToEnemy();
    }

    
    private void TurnToPlayer()
    {
        Debug.Log("TurnToPlayer");
        EnablePlayerControl(false);
    }

    private void TurnToEnemy()
    {
        Debug.Log("TurnToEnemy");
        DelayedEnemyAttack(3);
    }


    //uses same halo as in check
    private void ShowHint()
    {
        isHintShowed = true;
        var hintCoords = GetAnyPossibleMove();
        chipArray[(int)hintCoords.x, (int)hintCoords.y].halo.enabled = true;
        chipArray[(int)hintCoords.z, (int)hintCoords.w].halo.enabled = true;
    }

    //three predefined points for chips, other place at random
    private static void Shuffle()
    {
        selectedChip = null;
        SetPhysics(false);

        var sameChips = GetAnySameChips(0);
        var excludes = new List<int>();

        //linear array is much better to randomize
        var linear = new ChipBehaviour[MAX_ROWS * MAX_COLS];

        //move 3 found close to each other to make sure of one turn
        linear[GetLinearIndex(1, 0)] = sameChips[0];
        excludes.Add(GetLinearIndex(1, 0));

        linear[GetLinearIndex(1, 1)] = sameChips[1];
        excludes.Add(GetLinearIndex(1, 1));

        linear[GetLinearIndex(2, 2)] = sameChips[2];
        excludes.Add(GetLinearIndex(2, 2));

        for (int i = 0; i < MAX_ROWS; i++)
        {
            for (int j = 0; j < MAX_COLS; j++)
            {
                if (sameChips.Contains(chipArray[i, j]))
                {
                    continue;
                }

                int randomPlace = GetRandomWithExcludes(MAX_ROWS * MAX_COLS, excludes);
                linear[randomPlace] = chipArray[i, j];
                excludes.Add(randomPlace);
            }
        }

        //move
        for (int i = 0; i < linear.Length; i++)
        {
            linear[i].row = GetRowOfLinear(i);
            linear[i].col = GetColOfLinear(i);
            linear[i].MoveTo(chipArray[GetRowOfLinear(i), GetColOfLinear(i)].gameObject.transform.position);
        }

        //move linear array back to 2D
        for (int i = 0; i < linear.Length; i++)
        {
            chipArray[GetRowOfLinear(i), GetColOfLinear(i)] = linear[i];
        }
    }

    //2D to linear: row*MAX_ROWS+col
    private static int GetLinearIndex(int row, int col)
    {
        return row * MAX_ROWS + col;
    }

    //linear to 2D: row = index / MAX_ROWS; col = index % MAX_ROWS
    private static int GetRowOfLinear(int index)
    {
        return index / MAX_ROWS;
    }

    private static int GetColOfLinear(int index)
    {
        return index % MAX_ROWS;
    }

    //looking for 3 same chips recursively
    private static List<ChipBehaviour> GetAnySameChips(int currentType)
    {
        var sameChips = new List<ChipBehaviour>();
        for (int i = 0; i < MAX_ROWS; i++)
        {
            for (int j = 0; j < MAX_COLS; j++)
            {
                if (SafeGetType(i, j) == currentType)
                {
                    sameChips.Add(chipArray[i, j]);
                    if (sameChips.Count >= 3)
                    {
                        return sameChips;
                    }
                }
            }
        }

        //did not find enough chips of this type, search next recursively
        return GetAnySameChips(currentType + 1);
    }

    //are we done yet?
    private void CheckWinLose()
    {
        if(HeroHP <=0 )
        {
            Lose();
        } else if (EnemyHP <=0)
        {
            Win();
        }
    }

    //check and collect chips
    private bool FullMatchCheck()
    {

        bool isNewCombinations = false;
        var line = new List<ChipBehaviour>();
        DeleteChipsList = new List<ChipBehaviour>();

        //uses to generate bonus for both horizontal and vertical line at once
        //var horizontalLine = new List<ChipBehaviour>();

        int currentType = -2;

        bonusRow = new int[MAX_COLS];

        //horizontal check
        for (int i = 0; i < MAX_ROWS; i++)
        {
            for (int j = 0; j < MAX_COLS; j++)
            {
                //new line
                if (j == 0 || currentType < 0 || currentType != chipArray[i, j].Type)
                {
                    if (line.Count > 3)
                    {
                        Was4Plus = true;
                    }
                        
                    if (line.Count >= 3)
                    {
                        isNewCombinations = true;
                        AddListOfChipsToDelete(line);
                    }

                    line.Clear();
                    currentType = chipArray[i, j].Type;
                    line.Add(chipArray[i, j]);
                }
                else
                {
                    line.Add(chipArray[i, j]);
                }
            }
        }

        line = new List<ChipBehaviour>();
        //vertical check
        for (int j = 0; j < MAX_COLS; j++)
        {
            for (int i = 0; i < MAX_ROWS; i++)
            {
                //new line
                if (i == 0 || currentType < 0 || currentType != chipArray[i, j].Type)
                {
                    if (line.Count > 3)
                    {
                        Was4Plus = true;
                    }
                        
                    if (line.Count >= 3)
                    {
                        isNewCombinations = true;
                        AddListOfChipsToDelete(line);
                    }

                    line.Clear();
                    currentType = chipArray[i, j].Type;
                    line.Add(chipArray[i, j]);
                }
                else
                {
                    line.Add(chipArray[i, j]);
                }
            }
        }

        for (int j = 0; j < MAX_COLS; j++)
        {
            for (int i = 0; i < MAX_ROWS; i++)
            {
                isNewCombinations = SquareCheck(chipArray[i,j]) || isNewCombinations;
            }
        }
        
        if (isNewCombinations)
        {
            CollectMatches();
            audioSource.PlayOneShot(matchSound);
        }

        return isNewCombinations;
    }

    private void CollectMatches()
    {
        DamageAccumulated = 0;
        foreach (var iterChip in DeleteChipsList)
        {
            CollectLoot(iterChip);
            iterChip.StartDestroy();
        }
    }

    private void CollectLoot(ChipBehaviour iterChip)
    {
        if(iterChip.Type == 0)
        {
            DamageAccumulated++;
        }
        if (iterChip.Type == 1)
        {
            GlobalLoot.defense++;
            LocalLoot.defense++;
        }
        if (iterChip.Type == 2)
        {
            GlobalLoot.money++;
            LocalLoot.money++;
        }
        if (iterChip.Type == 3)
        {
            HeroHP++;
            if(HeroHP>20)
            {
                HeroHP = 20;
            }
        }
        if (iterChip.Type == 4)
        {
            GlobalLoot.experience++;
            LocalLoot.experience++;
        }
        if (iterChip.Type == 5)
        {
            GlobalLoot.mana++;
            LocalLoot.mana++;
        }
    }

    internal void Mute()
    {
        var audioSource = GetComponent<AudioSource>();
        audioSource.mute = !audioSource.mute;
    }

    //initial field generation. No autocollect at start but at least one possible move
    private void GenerateField()
    {
        if (MAX_ROWS < 4 || MAX_COLS < 4)
        {
            throw new Exception("Too small field for this game. Generate at least 4*4 field using MAX_ROWS and MAX_COLS constants.");
        }

        if (MAX_ROWS > 9 || MAX_COLS > 18)
        {
            throw new Exception("Too big field for this game. Generate no more than 9*18 field using MAX_ROWS and MAX_COLS constants.");
        }

        field = GameObject.Find("Game Field");
        for (int i = 0; i < MAX_ROWS; i++)
        {
            bool useGravity = i != 0;
            for (int j = 0; j < MAX_COLS; j++)
            {
                var chip = (GameObject)Instantiate(Resources.Load("chips/ChipPrefab"));
                chip.transform.parent = field.transform;
                var chipBehaviour = chip.GetComponent<ChipBehaviour>();
                chipArray[i, j] = chipBehaviour;
                int type = GetRandomTypeForChip(i, j);
                chipBehaviour.Create(type, i, j, useGravity);
            }
        }

        if (GetAnyPossibleMove() != noMatchVector4)
        {
            //There ARE possible moves."
        }
        else
        {
            //ok, there is no turns at start, so we generate a one turn as an exception
            GeneratePatchForField();
        }
    }

    //immediate chip destroying, after animation
    internal void DestroyChip(ChipBehaviour chip)
    {
        destroyWaiting--;

        for (int i = chip.row; i < MAX_ROWS - 1; i++)
        {
            chipArray[i, chip.col] = chipArray[i + 1, chip.col];
            //can be null for deleted chips
            if (chipArray[i, chip.col])
            {
                chipArray[i, chip.col].row = i;
            }
        }

        chipArray[MAX_ROWS - 1, chip.col] = null;

        if (destroyWaiting <= 0)
        {
            FillNewChips();
            SetPhysics(true);
            /*for (int i = 0; i < MAX_ROWS; i++)
                for (int j = 0; j < MAX_COLS; j++)
                    if (chipArray[i, j] && chipArray[i, j].rigidbody)
                        previousPositionY[i, j] = chipArray[i, j].rigidbody.transform.position.y;*/
            isWaitingChipsFall = true;
        }
    }

    private void FillNewChips()
    {
        for (int col = 0; col < MAX_COLS; col++)
        {
            int fillerForCol = 0;
            for (int row = 0; row < MAX_ROWS; row++)
            {
                if (!chipArray[row, col])
                {
                    fillerForCol++;
                }
            }

            if (fillerForCol > 0)
            {
                FillCol(col, fillerForCol);
            }
        }
    }

    private void FillCol(int col, int count)
    {
        for (int i = 0; i < count; i++)
        {
            var chip = (GameObject)Instantiate(Resources.Load("Chips/ChipPrefab"));
            chip.transform.parent = field.transform;
            var chipBehaviour = chip.GetComponent<ChipBehaviour>();
            chipArray[MAX_ROWS - count + i, col] = chipBehaviour;

            int type;
            if (i == 0 && bonusRow[col] > BASE_CHIP_TYPES)
            {
                type = bonusRow[col];
            }
            else
            {
                type = random.Next(0, BASE_CHIP_TYPES);
            }

            chipBehaviour.Create(type, MAX_ROWS + i, col, true);
            //set true coords
            chipBehaviour.row = MAX_ROWS - count + i;
            chip.transform.position = new Vector2(chip.transform.position.x - fieldHalfWidth, -fieldHalfHeight+(MAX_COLS + i)*ChipBehaviour.ICON_HEIGHT);
        }
    }

    //set another type for two adjacent chips to generate one move in case of no moves
    private void GeneratePatchForField()
    {
        chipArray[0, 2].Type = chipArray[0, 0].Type;
        chipArray[1, 1].Type = chipArray[0, 0].Type;
    }

    private void CenterField()
    {
        fieldHalfWidth = (MAX_COLS - 1) * ChipBehaviour.ICON_WIDTH / 2.0f;
        fieldHalfHeight = (MAX_ROWS - 1) * ChipBehaviour.ICON_HEIGHT / 2.0f;
        field.transform.position = new Vector2(-fieldHalfWidth, -fieldHalfHeight);
    }

    internal void TrySwipeWith(ChipBehaviour secondChip)
    {
        for (int i = 0; i < MAX_ROWS; i++)
        {
            for (int j = 0; j < MAX_COLS; j++)
            {
                if (chipArray[i, j])
                {
                    chipArray[i, j].halo.enabled = false;
                }
            }
        }

        bonusRow = new int[MAX_COLS];
        isFieldActive = false;
        SetPhysics(false);
        this.secondChip = secondChip;
        movingCounter = 0;
        selectedChip.MoveTo(secondChip.gameObject.transform.position);
        secondChip.MoveTo(selectedChip.gameObject.transform.position);
    }

    //bad move, return chips back
    private void MoveChipsBack()
    {
        movingCounter = 0;
        isMovingBack = true;
        selectedChip.MoveTo(secondChip.gameObject.transform.position);
        secondChip.MoveTo(selectedChip.gameObject.transform.position);
    }

    //used both for direct and back moves
    internal void OnMovingEnd()
    {
        movingCounter++;
        if (movingCounter < 2)
        {
            return;
        }

        //shuffle end
        if (!selectedChip)
        {
            if (!FullMatchCheck())
            {
                EnablePlayerControl(true);
            }

            return;
        }

        int transitRow = selectedChip.row;
        int transitCol = selectedChip.col;
        selectedChip.row = secondChip.row;
        selectedChip.col = secondChip.col;
        secondChip.row = transitRow;
        secondChip.col = transitCol;

        chipArray[selectedChip.row, selectedChip.col] = selectedChip;
        chipArray[secondChip.row, secondChip.col] = secondChip;

        if (isMovingBack)
        {
            isMovingBack = false;
            selectedChip = null;
            secondChip = null;
            EnablePlayerControl(true);
            return;
        }

        //evade lazy boolean evaluation
        bool firstSuccessful = СheckAndDestroyForChip(selectedChip, secondChip.Type);
        bool secondSuccessful = СheckAndDestroyForChip(secondChip, selectedChip.Type);
        if (firstSuccessful || secondSuccessful)
        {
            HeroAttack(DamageAccumulated);
            if (!Was4Plus)
            {
                NextTurnHandler();
            }
            else
            {
                StateCaption.GetComponent<Text>().text = "4+ chips - Free move!";
                TurnToPlayer();
            }
            Was4Plus = false;
            audioSource.PlayOneShot(matchSound);
            TurnsLeft--;
        }
        else
        {
            MoveChipsBack();
        }
    }

    //is there any lines with this chip? If so, collect it
    private bool СheckAndDestroyForChip(ChipBehaviour chip, int pairType)
    {
        if (chip.Type > BASE_CHIP_TYPES)
        {
            ActivateBonus(chip, pairType);
            return true;
        }

        var horizontalLine = new List<ChipBehaviour>();
        var verticalLine = new List<ChipBehaviour>();

        DeleteChipsList = new List<ChipBehaviour>();

        //first chip is it itself
        horizontalLine.Add(chip);
        verticalLine.Add(chip);

        int row = chip.row;
        int col = chip.col;

        int currentType = SafeGetType(chip.row, chip.col);

        SquareCheck(chip);

        //UP
        if (currentType == SafeGetType(row + 1, col))
        {
            verticalLine.Add(chipArray[row + 1, col]);
            if (currentType == SafeGetType(row + 2, col))
            {
                verticalLine.Add(chipArray[row + 2, col]);
            }
        }

        //DOWN
        if (currentType == SafeGetType(row - 1, col))
        {
            verticalLine.Add(chipArray[row - 1, col]);
            if (currentType == SafeGetType(row - 2, col))
            {
                verticalLine.Add(chipArray[row - 2, col]);
            }
        }

        //LEFT
        if (currentType == SafeGetType(row, col - 1))
        {
            horizontalLine.Add(chipArray[row, col - 1]);
            if (currentType == SafeGetType(row, col - 2))
            {
                horizontalLine.Add(chipArray[row, col - 2]);
            }
        }

        //RIGHT
        if (currentType == SafeGetType(row, col + 1))
        {
            horizontalLine.Add(chipArray[row, col + 1]);
            if (currentType == SafeGetType(row, col + 2))
            {
                horizontalLine.Add(chipArray[row, col + 2]);
            }
        }

        bool isMatch = false;

        if (horizontalLine.Count > 3 || verticalLine.Count > 3)
        {
            Was4Plus = true;
        }

        if (horizontalLine.Count >= 3)
        {
            AddListOfChipsToDelete(horizontalLine);
            isMatch = true;
        }

        if (verticalLine.Count >= 3)
        {
            AddListOfChipsToDelete(verticalLine);
            isMatch = true;
        }

        
        if (isMatch)
        {
            CollectMatches();
        }


        return isMatch;
    }

    private void AddListOfChipsToDelete(List<ChipBehaviour> addList)
    {
        foreach (var iterChip in addList)
        {
            AddToDeleteChips(iterChip);
        }
    }

    private void AddToDeleteChips(ChipBehaviour chip)
    {
        if (!DeleteChipsList.Contains(chip))
        {
            DeleteChipsList.Add(chip);
        }
    }

    private bool SquareCheck(ChipBehaviour chip)
    {        
        bool isMatch = false;

        int currentType = SafeGetType(chip.row, chip.col);
        int row = chip.row;
        int col = chip.col;

        isMatch = isMatch || TryToAddSquare(row + 1, col - 1);
        isMatch = isMatch || TryToAddSquare(row + 1, col);
        isMatch = isMatch || TryToAddSquare(row, col - 1);
        isMatch = isMatch || TryToAddSquare(row, col);

        return isMatch;
    }

    private bool TryToAddSquare(int row, int col)
    {
        int currentType = SafeGetType(row, col);
        if(currentType == SafeGetType(row, col + 1) && currentType == SafeGetType(row - 1, col) && currentType == SafeGetType(row - 1, col + 1))
        {
            AddToDeleteChips(chipArray[row, col]);
            AddToDeleteChips(chipArray[row, col + 1]);
            AddToDeleteChips(chipArray[row - 1, col]);
            AddToDeleteChips(chipArray[row - 1, col + 1]);
            return true;
        }
        return false;
    }

    //let 'em BOOM
    private void ActivateBonus(ChipBehaviour chip, int pairType)
    {
        //prevent duplicate activations
        if (chip.isDestroying)
        {
            return;
        }

        chip.StartDestroy();
        if (chip.Type == (int)ChipBehaviour.chipTypes.bomb)
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (chip.row + i >= 0 && chip.col + j >= 0 && chip.row + i < MAX_ROWS && chip.col + j < MAX_COLS &&
                    chipArray[chip.row + i, chip.col + j])
                {
                    if (chipArray[chip.row + i, chip.col + j].Type > BASE_CHIP_TYPES)
                        {
                            ActivateBonus(chipArray[chip.row + i, chip.col + j], pairType);
                        }

                        chipArray[chip.row + i, chip.col + j].StartDestroy();
                }
                }
            }
        }

        if (chip.Type == (int)ChipBehaviour.chipTypes.rainbow)
        {
            for (int i = 0; i < MAX_ROWS; i++)
            {
                for (int j = 0; j < MAX_COLS; j++)
                {
                    if (chipArray[i, j] && chipArray[i, j].Type == pairType)
                    {
                        chipArray[i, j].StartDestroy();
                    }
                }
            }

            chip.StartDestroy();
        }

        if (chip.Type == (int)ChipBehaviour.chipTypes.rocket)
        {
            for (int i = chip.row; i >= 0; i--)
            {
                chipArray[i, chip.col].StartDestroy();
                if (chipArray[i, chip.col].Type > BASE_CHIP_TYPES)
                {
                    ActivateBonus(chipArray[i, chip.col], pairType);
                }

                chipArray[i, chip.col].StartDestroy();
            }
        }
    }

    //we don't need Unity physics in shuffle, chips exchange etc...
    private static void SetPhysics(bool enabled)
    {
        for (int i = 0; i < MAX_ROWS; i++)
        {
            for (int j = 0; j < MAX_COLS; j++)
            {
                if (chipArray[i, j])
                {

                }
            }
        }
    }

    //adjacent chips should be not the same at first generation
    private int GetRandomTypeForChip(int row, int col)
    {
        var excludes = new List<int>();
        if (row > 0)
        {
            if (chipArray[row - 1, col])
            {
                excludes.Add(chipArray[row - 1, col].Type);
            }
        }

        if (col > 0)
        {
            if (chipArray[row, col - 1])
            {
                excludes.Add(chipArray[row, col - 1].Type);
            }
        }

        return GetRandomWithExcludes(BASE_CHIP_TYPES, excludes);
    }

    //generate one of the random number [0, maxNumber) but except excludes, DON'T try to use it if excludes more than maxNumber or equal
    private static int GetRandomWithExcludes(int maxNumber, List<int> excludes)
    {
        if (excludes.Count >= maxNumber)
        {
            throw new Exception("GetRandomWithExcludes can't be done with so much excludes!");
        }

        int answer = random.Next(0, maxNumber);
        for (int i = 0; i < excludes.Count; i++)
        {
            if (excludes.Contains(answer))
            {
                answer++;
                if (answer >= maxNumber)
                {
                    answer = 0;
                }
            }
            else
            {
                return answer;
            }
        }

        return answer;
    }

    //first pair - first chip, second pair - second chip
    private static Vector4 GetAnyPossibleMove()
    {
        var noMatchVector = new Vector2(-1, -1);
        for (int i = 0; i < MAX_ROWS; i++)
        {
            for (int j = 0; j < MAX_COLS; j++)
        {
            var answerVector = GetPossibleMovesForChip(i, j);
            if (answerVector != noMatchVector)
                {
                    return new Vector4(i, j, answerVector.x, answerVector.y);
                }
            }
        }

        return new Vector4(-1, -1, -1, -1);
    }

    private static Vector2 GetPossibleMovesForChip(int row, int col)
    {
        //try move up
        if (row + 1 < MAX_ROWS)
        {
            if (VirtualMoveCheck(row + 1, col, row, col))
            {
                return new Vector2(row + 1, col);
            }
        }

        //try move right
        if (col + 1 < MAX_COLS)
        {
            if (VirtualMoveCheck(row, col + 1, row, col))
            {
                return new Vector2(row, col + 1);
            }
        }

        //try move down
        if (row > 0)
        {
            if (VirtualMoveCheck(row - 1, col, row, col))
            {
                return new Vector2(row - 1, col);
            }
        }

        //try move left
        if (col > 0)
        {
            if (VirtualMoveCheck(row, col - 1, row, col))
            {
                return new Vector2(row, col - 1);
            }
        }

        return new Vector2(-1, -1);
    }

    //looking for a potential move
    private static bool VirtualMoveCheck(int newRow, int newCol, int initRow, int initCol)
    {
        //HORIZONTAL CHECK
        if (newRow != initRow)
        {
            //CENTRAL HORIZONTAL
            if (SafeGetType(initRow, initCol) == SafeGetType(newRow, newCol - 1) &&
                SafeGetType(initRow, initCol) == SafeGetType(newRow, newCol + 1))
            {
                return true;
            }

            //LEFT HORIZONTAL
            if (SafeGetType(initRow, initCol) == SafeGetType(newRow, newCol - 1) &&
                SafeGetType(initRow, initCol) == SafeGetType(newRow, newCol - 2))
            {
                return true;
            }

            //RIGHT HORIZONTAL
            if (SafeGetType(initRow, initCol) == SafeGetType(newRow, newCol + 1) &&
                SafeGetType(initRow, initCol) == SafeGetType(newRow, newCol + 2))
            {
                return true;
            }
        }

        //VERTICAL CHECK
        if (newCol != initCol)
        {
            //CENTRAL VERTICAL
            if (SafeGetType(initRow, initCol) == SafeGetType(newRow - 1, newCol) &&
                SafeGetType(initRow, initCol) == SafeGetType(newRow + 1, newCol))
            {
                return true;
            }

            //UP VERTICAL
            if (SafeGetType(initRow, initCol) == SafeGetType(newRow + 1, newCol) &&
                SafeGetType(initRow, initCol) == SafeGetType(newRow + 2, newCol))
            {
                return true;
            }

            //DOWN VERTICAL
            if (SafeGetType(initRow, initCol) == SafeGetType(newRow - 1, newCol) &&
                SafeGetType(initRow, initCol) == SafeGetType(newRow - 2, newCol))
            {
                return true;
            }
        }

        return false;
    }

    //return type of chip is available, -1 if not
    private static int SafeGetType(int row, int col)
    {
        if (row < 0 || row >= MAX_ROWS || col < 0 || col >= MAX_COLS)
        {
            return -1;
        }

        if (chipArray[row, col])
        {
            return chipArray[row, col].Type;
        }

        return -1;
    }

    private void UpdateScore()
    {
        /*
        var textFieldObject = GameObject.Find("Score Text");
        var textField = textFieldObject.GetComponent<Text>();
        textField.text = "Score: " + ScorePoints + "     Turns left: " + TurnsLeft;
        */
    }

    private void EnablePlayerControl(bool skipCheck)
    {

        if (skipCheck || GetAnyPossibleMove() != noMatchVector4)
        {
            isFieldActive = true;
            if (IsAutoBattle)
            {
                AutoTurn();
            }
            else
            { 
                SetPhysics(true);
                startTurnTime = Time.time;
                isHintShowed = false;
            }            
        }
        else
        {
            Shuffle();
        }
    }

    private void AutoTurn()
    {
        if(!isFieldActive)
        {
            return;
        }
        isFieldActive = false;
        if(GlobalLoot.mana>5)
        {
            if(HeroHP <10 )
            {
                TryHeal();
            }
            else
            {
                TryFireball();
            }
            AutoTurn();
            return;
        }
        Vector4 possibleMove = GetAnyPossibleMove();
        selectedChip = chipArray[(int)possibleMove.x, (int)possibleMove.y];
        secondChip = chipArray[(int)possibleMove.z, (int)possibleMove.w];
        TrySwipeWith(secondChip);
    }

    private int testCase = 25;

    internal void TestMatch()
    {

        switch(testCase)
        {
            case 1:
                chipArray[0, 0].Type = 4;
                chipArray[1, 0].Type = 4;
                chipArray[2, 0].Type = 4;
                break;
            case 2:
                chipArray[0, 0].Type = 4;
                chipArray[0, 1].Type = 4;
                chipArray[0, 2].Type = 4;
                break;
            case 3:
                chipArray[0, 0].Type = 4;
                chipArray[0, 1].Type = 4;
                chipArray[0, 2].Type = 4;
                chipArray[0, 3].Type = 4;
                break;
            case 4:
                chipArray[0, 0].Type = 4;
                chipArray[0, 1].Type = 4;
                chipArray[0, 2].Type = 4;
                chipArray[0, 3].Type = 4;
                chipArray[0, 4].Type = 4;
                break;
            case 5:
                chipArray[0, 0].Type = 4;
                chipArray[1, 0].Type = 4;
                chipArray[2, 0].Type = 4;
                chipArray[3, 0].Type = 4;
                break;
            case 6:
                chipArray[0, 0].Type = 4;
                chipArray[1, 0].Type = 4;
                chipArray[2, 0].Type = 4;
                chipArray[3, 0].Type = 4;
                chipArray[4, 0].Type = 4;
                break;
            case 7:
                chipArray[0, 0].Type = 4;
                chipArray[1, 0].Type = 4;
                chipArray[0, 1].Type = 4;
                chipArray[1, 1].Type = 4;
                break;
            case 8:
                chipArray[0, 0].Type = 4;
                chipArray[1, 0].Type = 4;
                chipArray[0, 1].Type = 4;
                chipArray[1, 1].Type = 4;
                chipArray[2, 0].Type = 4;
                break;
            case 9:
                chipArray[0, 0].Type = 4;
                chipArray[1, 0].Type = 4;
                chipArray[0, 1].Type = 4;
                chipArray[1, 1].Type = 4;
                chipArray[2, 1].Type = 4;
                break;
            case 10:
                chipArray[0, 0].Type = 4;
                chipArray[1, 0].Type = 4;
                chipArray[0, 1].Type = 4;
                chipArray[1, 1].Type = 4;
                chipArray[1, 2].Type = 4;
                break;
            case 11:
                chipArray[0, 0].Type = 4;
                chipArray[1, 0].Type = 4;
                chipArray[0, 1].Type = 4;
                chipArray[1, 1].Type = 4;
                chipArray[0, 2].Type = 4;
                break;
            case 12:
                chipArray[1, 0].Type = 4;
                chipArray[0, 1].Type = 4;
                chipArray[1, 1].Type = 4;
                chipArray[2, 0].Type = 4;
                chipArray[2, 1].Type = 4;
                break;
            case 13:
                chipArray[0, 0].Type = 4;
                chipArray[1, 0].Type = 4;
                chipArray[1, 1].Type = 4;
                chipArray[2, 0].Type = 4;
                chipArray[2, 1].Type = 4;
                break;
            case 14:
                chipArray[0, 0].Type = 4;
                chipArray[0, 1].Type = 4;
                chipArray[1, 0].Type = 4;
                chipArray[0, 2].Type = 4;
                chipArray[2, 0].Type = 4;
                break;
            case 15:
                chipArray[1, 1].Type = 4;
                chipArray[0, 1].Type = 4;
                chipArray[1, 0].Type = 4;
                chipArray[0, 2].Type = 4;
                chipArray[1, 2].Type = 4;
                break;
            case 16:
                chipArray[0, 0].Type = 4;
                chipArray[1, 0].Type = 4;
                chipArray[2, 0].Type = 4;
                chipArray[2, 1].Type = 4;
                chipArray[2, 2].Type = 4;
                break;
            case 17:
                chipArray[2, 2].Type = 4;
                chipArray[1, 2].Type = 4;
                chipArray[0, 2].Type = 4;
                chipArray[2, 1].Type = 4;
                chipArray[2, 0].Type = 4;
                break;
            case 18:
                chipArray[0, 0].Type = 4;
                chipArray[0, 1].Type = 4;
                chipArray[0, 2].Type = 4;
                chipArray[1, 2].Type = 4;
                chipArray[2, 2].Type = 4;
                break;
            case 19:
                chipArray[0, 0].Type = 4;
                chipArray[1, 0].Type = 4;
                chipArray[2, 0].Type = 4;
                chipArray[0, 1].Type = 4;
                chipArray[0, 2].Type = 4;
                break;
            case 20:
                chipArray[2, 2].Type = 4;
                chipArray[1, 2].Type = 4;
                chipArray[0, 2].Type = 4;
                chipArray[2, 1].Type = 4;
                chipArray[2, 0].Type = 4;
                chipArray[2, 3].Type = 4;
                chipArray[2, 4].Type = 4;
                break;
            case 21:
                chipArray[0, 0].Type = 4;
                chipArray[0, 1].Type = 4;
                chipArray[0, 2].Type = 4;
                chipArray[0, 3].Type = 4;
                chipArray[0, 4].Type = 4;
                chipArray[1, 2].Type = 4;
                chipArray[2, 2].Type = 4;
                break;
            case 22:
                chipArray[0, 0].Type = 4;
                chipArray[1, 0].Type = 4;
                chipArray[2, 0].Type = 4;
                chipArray[3, 0].Type = 4;
                chipArray[4, 0].Type = 4;
                chipArray[2, 1].Type = 4;
                chipArray[2, 2].Type = 4;
                break;
            case 23:
                chipArray[2, 2].Type = 4;
                chipArray[3, 2].Type = 4;
                chipArray[4, 2].Type = 4;
                chipArray[1, 2].Type = 4;
                chipArray[0, 2].Type = 4;
                chipArray[2, 1].Type = 4;
                chipArray[2, 0].Type = 4;
                break;
            case 24:
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        chipArray[i, j].Type = 4;
                    }
                }

                break;
            case 25:
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        chipArray[i, j].Type = 4;
                    }
                }

                break;
            case 26:
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        chipArray[i, j].Type = 4;
                    }
                }

                break;
            case 27:
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        chipArray[i, j].Type = 4;
                    }
                }

                break;
            case 28:
                for (int i = 0; i < 7; i++)
                {
                    for (int j = 0; j < 7; j++)
                    {
                        chipArray[i, j].Type = 4;
                    }
                }

                break;
            case 29:
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        chipArray[i, j].Type = 4;
                    }
                }

                break;

            default:
                testCase = 0;
                break;
        }

        testCase++;

        Task.Delay(200).ContinueWith(t => FullMatchCheck());

    }

    private static void RefreshLabels()
    {
        
        var HeroHPLabel = GameObject.Find("HealthCaption");
        HeroHPLabel.GetComponent<Text>().text = "HP: " + HeroHP + "/" + HeroMaxHP;

        var HeroProgressBar = GameObject.Find("HeroProgressBar");
        HeroProgressBar.GetComponent<ProgressBarClass>().SetProgress(HeroHP);

        var EnemyHPLabel = GameObject.Find("HealthEnemy");
        EnemyHPLabel.GetComponent<Text>().text = "HP: " + EnemyHP + "/" + EnemyMaxHP;

        var EnemyProgressBar = GameObject.Find("EnemyProgressBar");
        EnemyProgressBar.GetComponent<ProgressBarClass>().SetProgress(EnemyHP);


        var AttackCaption = GameObject.Find("AttackCaption");
        AttackCaption.GetComponent<Text>().text = "Attack: " + GlobalLoot.attack;

        var DefenceCaption = GameObject.Find("DefenceCaption");
        DefenceCaption.GetComponent<Text>().text = "Defence: " + GlobalLoot.defense;

        var CoinCaption = GameObject.Find("CoinCaption");
        CoinCaption.GetComponent<Text>().text = "Coins: " + GlobalLoot.money;

        var ManaCaption = GameObject.Find("ManaCaption");
        ManaCaption.GetComponent<Text>().text = "Mana: " + GlobalLoot.mana;

        var ExpCaption = GameObject.Find("ExpCaption");
        ExpCaption.GetComponent<Text>().text = "Exp: " + GlobalLoot.experience;
    }

    public void DelayedEnemyAttack(int number)
    {
        HeroHP -= number;
        RefreshLabels();
        
        EnemyDamage = number;
        Invoke("EnemyAttackPart2", 1);
    }

    private void EnemyAttackPart2()
    {
        StateCaption.GetComponent<Text>().text = "Enemy attacked: " + EnemyDamage + " HP";
        CheckWinLose();
        NextTurnHandler();
    }

    public void ImmediateEnemyAttack(int number)
    {
        HeroHP -= number;
        RefreshLabels();
        CheckWinLose();
        StateCaption.GetComponent<Text>().text = "Enemy attacked: " + number + " HP";
    }


    public void HeroAttack(int number)
    {
        EnemyHP -= number;
        RefreshLabels();
        CheckWinLose();
        StateCaption.GetComponent<Text>().text = "You attacked: " + number + " HP";
    }

    internal void ToggleAutoBattle()
    {
        IsAutoBattle = !IsAutoBattle;
        var AutoBattleText = GameObject.Find("AutoBattleText");
        if(IsAutoBattle)
        {
            AutoBattleText.GetComponent<Text>().color = Color.green;

            if (!isWaitingChipsFall)
            {
                AutoTurn();
            }
        }
        else
        {
            AutoBattleText.GetComponent<Text>().color = Color.red;
        }
    }

    internal void TryFireball()
    {
        if(GlobalLoot.mana >= 5)
        {
            GlobalLoot.mana -= 5;
            HeroHP += 10;
            if (HeroHP > 20)
                HeroHP = 20;
            RefreshLabels();
            StateCaption.GetComponent<Text>().text = "You healed 10 HP";

        }
        else
        {
            StateCaption.GetComponent<Text>().text = "Not enough mana (5)";
        }
    }

    internal void TryHeal()
    {
        var AutoBattleText = GameObject.Find("StateCaption");
        if (GlobalLoot.mana >= 5)
        {
            GlobalLoot.mana -= 5;
            HeroAttack(10);
        }
        else
        {
            StateCaption.GetComponent<Text>().text = "Not enough mana (5)";
        }
    }

}