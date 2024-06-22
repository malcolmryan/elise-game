using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WormMove : MonoBehaviour
{
#region Actions
    private Actions actions;
    private InputAction northAction;
    private InputAction southAction;
    private InputAction eastAction;
    private InputAction westAction;
#endregion Actions

#region Types
    public enum Move {
        NORTH, SOUTH, EAST, WEST, 
    };

    public enum Orientation {
        UP, DOWN, NORTH, SOUTH, EAST, WEST
    };

    [Serializable]
    public struct Configuration {
        public Vector2Int headPosition;
        public Orientation orientation;        
    } 
#endregion Types

#region Public Fields
    [SerializeField] private int length;
    [SerializeField] private Configuration configuration;
#endregion Private Fields

#region Private Fields
    private Queue<Move> queuedMoves = new Queue<Move>();
    private bool moving = false;

    private Vector2Int[,] dPosition;
    private Orientation[,] dOrientation;
#endregion Private Fields

#region Init
    void Awake()
    {
        actions = new Actions();
        northAction.performed += context => QueueMove(Move.NORTH);
        southAction.performed += context => QueueMove(Move.SOUTH);
        eastAction.performed += context => QueueMove(Move.EAST);
        westAction.performed += context => QueueMove(Move.WEST);
    }

    void OnEnable()
    {
        actions.movement.Enable();
    }

    void OnDisable()
    {
        actions.movement.Disable();
    }

    void Start()
    {
        InitDeltas();
    }

    private void InitDeltas()
    {
        dPosition = new Vector2Int[6,4];
        dOrientation = new Orientation[6,4];

        int oNorth = (int)Orientation.NORTH;
        int oSouth = (int)Orientation.SOUTH;
        int oEast = (int)Orientation.EAST;
        int oWest = (int)Orientation.WEST;
        int oUp = (int)Orientation.UP;
        int oDown = (int)Orientation.DOWN;

        int mNorth = (int)Move.NORTH;
        int mSouth = (int)Move.SOUTH;
        int mEast = (int)Move.EAST;
        int mWest = (int)Move.WEST;

        dPosition[oUp,mNorth] = new Vector2Int(0,+length);
        dPosition[oUp,mSouth] = new Vector2Int(0,-length);
        dPosition[oUp,mEast] = new Vector2Int(+length,0);
        dPosition[oUp,mWest] = new Vector2Int(-length,0);

        dPosition[oDown,mNorth] = new Vector2Int(0,+1);
        dPosition[oDown,mSouth] = new Vector2Int(0,-1);
        dPosition[oDown,mEast] = new Vector2Int(+1,0);
        dPosition[oDown,mWest] = new Vector2Int(-1,0);

        dPosition[oNorth,mNorth] = new Vector2Int(0,+1);
        dPosition[oNorth,mSouth] = new Vector2Int(0,-length);
        dPosition[oNorth,mEast] = new Vector2Int(+1,0);
        dPosition[oNorth,mWest] = new Vector2Int(-1,0);

        dPosition[oSouth,mNorth] = new Vector2Int(0,+length);
        dPosition[oSouth,mSouth] = new Vector2Int(0,-1);
        dPosition[oSouth,mEast] = new Vector2Int(+1,0);
        dPosition[oSouth,mWest] = new Vector2Int(-1,0);

        dPosition[oEast,mNorth] = new Vector2Int(0,+1);
        dPosition[oEast,mSouth] = new Vector2Int(0,-1);
        dPosition[oEast,mEast] = new Vector2Int(+1,0);
        dPosition[oEast,mWest] = new Vector2Int(-length,0);

        dPosition[oWest,mNorth] = new Vector2Int(0,+1);
        dPosition[oWest,mSouth] = new Vector2Int(0,-1);
        dPosition[oWest,mEast] = new Vector2Int(+length,0);
        dPosition[oWest,mWest] = new Vector2Int(-1,0);

        dOrientation[oUp,mNorth] = Orientation.NORTH;
        dOrientation[oUp,mSouth] = Orientation.SOUTH;
        dOrientation[oUp,mEast] = Orientation.EAST;
        dOrientation[oUp,mWest] = Orientation.WEST;

        dOrientation[oDown,mNorth] = Orientation.SOUTH;
        dOrientation[oDown,mSouth] = Orientation.NORTH;
        dOrientation[oDown,mEast] = Orientation.WEST;
        dOrientation[oDown,mWest] = Orientation.EAST;

        dOrientation[oNorth,mNorth] = Orientation.DOWN;
        dOrientation[oNorth,mSouth] = Orientation.UP;
        dOrientation[oNorth,mEast] = Orientation.NORTH;
        dOrientation[oNorth,mWest] = Orientation.NORTH;

        dOrientation[oSouth,mNorth] = Orientation.UP;
        dOrientation[oSouth,mSouth] = Orientation.DOWN;
        dOrientation[oSouth,mEast] = Orientation.SOUTH;
        dOrientation[oSouth,mWest] = Orientation.SOUTH;

        dOrientation[oEast,mNorth] = Orientation.EAST;
        dOrientation[oEast,mSouth] = Orientation.EAST;
        dOrientation[oEast,mEast] = Orientation.DOWN;
        dOrientation[oEast,mWest] = Orientation.UP;

        dOrientation[oWest,mNorth] = Orientation.WEST;
        dOrientation[oWest,mSouth] = Orientation.WEST;
        dOrientation[oWest,mEast] = Orientation.UP;
        dOrientation[oWest,mWest] = Orientation.DOWN;
    }
#endregion Init

#region Update
    void Update()
    {
        if (!moving && queuedMoves.Count > 0)
        {
            Move move = queuedMoves.Dequeue();
            StartMove(move);
        }
    }
#endregion Update

#region Private Methods
    private void QueueMove(Move move)
    {
        queuedMoves.Enqueue(move);
    }

    private void StartMove(Move move)
    {
        Configuration next = NextConfiguration(configuration, move);

        if (!ValidConfiguration(next))
        {
            return;
        }

        moving = true;
        StartCoroutine(AnimateMove(move));
    }

    private Configuration NextConfiguration(Configuration old, Move move)
    {
        Configuration next;
        next.headPosition = old.headPosition + dPosition[(int)old.orientation, (int)move]; 
        next.orientation = dOrientation[(int)old.orientation, (int)move];
        return next;
    }

    private bool ValidConfiguration(Configuration conf)
    {
        

        return true;
    }

    private IEnumerator AnimateMove(Move move)
    {
        while (true)
        {
            yield return null;
        }

        moving = false;
    }
#endregion Private Methods

}


