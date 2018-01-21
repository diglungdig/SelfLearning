using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    public Texture2D image;

    public int blocksPerLine = 4;
    public int shuffleLength = 20;

    bool blockIsMoving = false;

    public float defaultMoveDuration = .2f;
    public float shuffleMoveDuration = .1f;

    enum PuzzleState { Solved, Shuffling, InPlay};
    PuzzleState state;


    int shuffleMovesRemaining;
    Vector2Int preShuffleOffset;

    Block emptyBlock;
    Block[,] blocks;
    Queue<Block> inputs;

    private void Start()
    {
        CreatePuzzle();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && state == PuzzleState.Solved)
        {
            StartShuffle();
        }
    }

    void StartShuffle()
    {
        state = PuzzleState.Shuffling;
        shuffleMovesRemaining = shuffleLength;
        emptyBlock.gameObject.SetActive(false);
        MakeNextShuffleMove();

    }

    void CreatePuzzle()
    {
        blocks = new Block[blocksPerLine, blocksPerLine];
        Texture2D[,] imageSlices = ImageSlicer.GetSlices(image, blocksPerLine);

        for (int y = 0; y < blocksPerLine; y++)
        {
            for (int x = 0; x < blocksPerLine; x++)
            {
                GameObject blockObject = GameObject.CreatePrimitive(PrimitiveType.Quad);

                blockObject.transform.position = -Vector2.one * (blocksPerLine - 1) * 0.5f + new Vector2(x, y);

                blockObject.transform.parent = transform;


                Block block = blockObject.AddComponent<Block>();

                block.OnBlockPressed += PlayerMoveBlockInput;
                block.OnFinishMoving += OnBlockFinishMoving;

                block.Init(new Vector2Int(x,y), imageSlices[x,y]);
                blocks[x, y] = block;

                if (y == 0 && x == blocksPerLine - 1)
                {
                    emptyBlock = block;
                }
            }
        }

        Camera.main.orthographicSize = blocksPerLine * 0.55f;
        inputs = new Queue<Block>();
    }


    void PlayerMoveBlockInput(Block blockToMove)
    {
        if (state == PuzzleState.InPlay)
        {
            inputs.Enqueue(blockToMove);

            MakeNextPlayerMove();
        }
    }


    void MakeNextPlayerMove()
    {
        while (inputs.Count > 0 && !blockIsMoving)
        {
            MoveBlock(inputs.Dequeue(), defaultMoveDuration);
        }
    }


    void MoveBlock(Block blockToMove, float duration)
    {
        if ((blockToMove.coord - emptyBlock.coord).sqrMagnitude == 1)
        {
            blocks[blockToMove.coord.x, blockToMove.coord.y] = emptyBlock;
            blocks[emptyBlock.coord.x, emptyBlock.coord.y] = blockToMove;

            Vector2Int targetCoord = emptyBlock.coord;
            emptyBlock.coord = blockToMove.coord;
            blockToMove.coord = targetCoord;

            Vector2 targetPosition = emptyBlock.transform.position;
            emptyBlock.transform.position = blockToMove.transform.position;
            blockToMove.MoveToPosition(targetPosition, duration);
            blockIsMoving = true;
        }
    }


    void OnBlockFinishMoving()
    {
        blockIsMoving = false;
        CheckIfSolved();
        if (state == PuzzleState.InPlay)
        {
            MakeNextPlayerMove();
        }
        if (state == PuzzleState.Shuffling)
        {
            if (shuffleMovesRemaining > 0)
            {
                MakeNextShuffleMove();
            }
            else
            {
                state = PuzzleState.InPlay;
            }
        }
    }



    void MakeNextShuffleMove()
    {
        Vector2Int[] offsets = {new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1)};

        int randomIndex = Random.Range(0, offsets.Length);

        for (int i = 0; i < offsets.Length; i++)
        {
            Vector2Int offset = offsets[(randomIndex + i) % offsets.Length];
            if (offset != preShuffleOffset * -1)
            {

                Vector2Int moveBlockCoord = emptyBlock.coord + offset;

                if (moveBlockCoord.x >= 0 && moveBlockCoord.x < blocksPerLine && moveBlockCoord.y >= 0 && moveBlockCoord.y < blocksPerLine)
                {
                    MoveBlock(blocks[moveBlockCoord.x, moveBlockCoord.y], shuffleMoveDuration);
                    shuffleMovesRemaining--;
                    preShuffleOffset = offset;
                    break;
                }
            }
        }
    }

    void CheckIfSolved()
    {
        foreach (var block in blocks)
        {
            if (!block.IsAtStartingCoord())
            {
                return;
            }
        }
        state = PuzzleState.Solved;

        emptyBlock.gameObject.SetActive(true);
    }
}
