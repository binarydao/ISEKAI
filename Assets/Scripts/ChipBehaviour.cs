using System;
using UnityEngine;

public class ChipBehaviour : MonoBehaviour
{
    internal const float ICON_WIDTH = 1.6f;
    internal const float ICON_HEIGHT = 1.6f;

    //destroying animation
    private const float DESTROY_TIME = 0.3f;

    //moving animation
    private const float MOVE_TIME = 4f;

    //swipe support
    internal static ChipBehaviour startSwipeChip;
    internal static ChipBehaviour lastMouseEnterSprite;
    internal int col;
    private Vector2 endPosition;
    private Color finalColor;

    //selected halo
    internal Behaviour halo;

    //private bool isCreated;
    internal bool isDestroying;
    private bool isMoving;

    internal int row;
    private SpriteRenderer spriteRenderer;
    private Vector2 startPosition;
    private float startTime;

    internal enum chipTypes { base0, base1, base2, base3, base4, base5, bomb = 10, rainbow = 20, rocket = 30 };
    private int type;

    internal int Type
    {
        get { return type; }
        set { ChangeType(value); }
    }

    // init
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        finalColor = new Color(0.5f, 0, 1, 1);
        halo = gameObject.GetComponent("Halo") as Behaviour;
        halo.enabled = false;
    }

    private void Update()
    {
        if (isDestroying)
        {
            
            float part = (Time.time - startTime) / DESTROY_TIME;
            if (part >= 1)
            {
                isDestroying = false;
                GameBehaviour.instance.DestroyChip(this);
                Destroy(gameObject);
            }
            else
            {
                spriteRenderer.color = Color.Lerp(Color.white, finalColor, part);
            }
        }
        else if (isMoving)
        {
            float part = (Time.time - startTime) / MOVE_TIME;
            if (part >= 1)
            {
                isMoving = false;
                GameBehaviour.instance.OnMovingEnd();
            }
            else
            {
                transform.position = Vector2.Lerp(startPosition, endPosition, part);
            }
        }
    }

    //uses to generate and place chip
    internal void Create(int type, int row, int col, bool useGravity)
    {
        this.row = row;
        this.col = col;
        ChangeType(type);
        transform.position = new Vector2(col * ICON_WIDTH, row * ICON_HEIGHT);
    }

    //change both logic and graphic of chip
    private void ChangeType(int type)
    {
        this.type = type;
        /*var texture = (Texture2D) Resources.Load("Chips/chip" + type);
        var runtimeSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
            new Vector2(ICON_WIDTH, ICON_HEIGHT), 100.0f);*/
        gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load("Chips/chip" + type, typeof(Sprite)) as Sprite;

    }

    //uses GRAPHIC positioning, not logic
    internal void MoveTo(Vector2 newPosition)
    {
        startTime = Time.time;
        startPosition = transform.position;
        endPosition = newPosition;
        isMoving = true;
    }

    private void OnMouseDown()
    {
        if (GameBehaviour.GameOver)
        {
            return;
        }
        startSwipeChip = this;
    }

    private void OnMouseEnter()
    {
		return; //no click anymore
        if (GameBehaviour.GameOver)
        {
            return;
        }
        lastMouseEnterSprite = this;
    }
	
    private static bool IsChipsAdjacent(ChipBehaviour firstChip, ChipBehaviour secondChip)
    {
        int xRange = Math.Abs(firstChip.col - secondChip.col);
        int yRange = Math.Abs(firstChip.row - secondChip.row);
        return xRange + yRange == 1;
    }

    //it's... magic gone
    internal void StartDestroy()
    {
        if (isDestroying)
        {
            return;
        }
            

        GameBehaviour.isFieldActive = false;

        GameBehaviour.instance.destroyWaiting++;

        //GameBehaviour.instance.ScorePoints += GameBehaviour.SCORES_FOR_CHIP;

        //startTime = Time.time;

        isDestroying = true;
    }
}