﻿using System;
using UnityEngine;

public class ChipBehaviour : MonoBehaviour
{
    internal const float ICON_WIDTH = 0.5f;
    internal const float ICON_HEIGHT = 0.5f;

    //destroying animation
    private const float DESTROY_TIME = 0.3f;

    //moving animation
    private const float MOVE_TIME = 0.5f;

    //swipe support
    internal static ChipBehaviour startSwipeChip;
    internal static ChipBehaviour lastMouseEnterSprite;
    internal int col;
    private Vector2 endPosition;
    private Color finalColor;

    //selected halo
    internal Behaviour halo;

    private bool isCreated;
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

        isCreated = true;
    }

    //change both logic and graphic of chip
    private void ChangeType(int type)
    {
        this.type = type;
        var texture = (Texture2D) Resources.Load("Chips/chip" + type);
        var runtimeSprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height),
            new Vector2(ICON_WIDTH, ICON_HEIGHT), 100.0f);
        gameObject.GetComponent<SpriteRenderer>().sprite = runtimeSprite;
        
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
        startSwipeChip = this;
    }

    private void OnMouseEnter()
    {
        lastMouseEnterSprite = this;
    }

    private void OnMouseUp()
    {
        Debug.Log("OnMouseUp");
        //some animation or moving in process, skip click
        if (!GameBehaviour.instance.isFieldActive) return;
        if (startSwipeChip != lastMouseEnterSprite)
        {
            Debug.Log("OnMouseUp");
            GameBehaviour.instance.selectedChip = startSwipeChip;
            if (IsChipsAdjacent(startSwipeChip, lastMouseEnterSprite))
                GameBehaviour.instance.TrySwipeWith(lastMouseEnterSprite);
            return;
        }

        //uncheck chip
        if (GameBehaviour.instance.selectedChip == this)
        {
            startSwipeChip = null;
            halo.enabled = false;
            GameBehaviour.instance.selectedChip = null;
        }
        //no previous checks
        else if (!GameBehaviour.instance.selectedChip)
        {
            halo.enabled = true;
            GameBehaviour.instance.selectedChip = this;
            Debug.Log("Selected chip row:" + row + "; col: " + col);
        }
        //try to change places?
        else if (IsChipsAdjacent(GameBehaviour.instance.selectedChip, this))
        {
            halo.enabled = false;
            GameBehaviour.instance.selectedChip.halo.enabled = false;
            GameBehaviour.instance.TrySwipeWith(this);
        }
        //not adjacent, lets check another chip
        else
        {
            GameBehaviour.instance.selectedChip.halo.enabled = false;
            halo.enabled = true;
            GameBehaviour.instance.selectedChip = this;
            Debug.Log("Selected chip row:" + row + "; col: " + col);
        }
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
            Debug.Log("Already destroying, return");
            return;
        }
            

        GameBehaviour.instance.isFieldActive = false;

        GameBehaviour.instance.destroyWaiting++;

        //GameBehaviour.instance.ScorePoints += GameBehaviour.SCORES_FOR_CHIP;

        //startTime = Time.time;

        isDestroying = true;
    }
}