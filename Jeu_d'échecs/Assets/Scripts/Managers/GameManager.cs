using System;
using UnityEngine;

public enum GameFinishedCondition
{
    Stalemate,
    Insufficient_Material,
    Threefold_Repitition,
    Fifty_Move_Rule,
    Draw_by_agreement,
    Checkmate,
    Timeout,
    Resignation,
    Dead_Position
}

public class GameManager : MonoBehaviour
{
    private Team _currentTurn = Team.White;
    private int _turnCount = 0;
    private GameFinishedCondition _gameFinished;
    private Team _winner;
    private Board _board;

    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();

                if (_instance == null)
                {
                    GameObject gameManagerObject = new GameObject("GameManager");
                    _instance = gameManagerObject.AddComponent<GameManager>();
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _board = Board.Instance;
    }

    public void StartGame()
    {
        throw new NotImplementedException();
    }

    public void EndGame()
    {
        throw new NotImplementedException();
    }

    public void SwitchTurn()
    {
        if (_currentTurn == Team.White)
        {
            _currentTurn = Team.Black;
        }
        else
        {
            _currentTurn = Team.White;
        }

        ++_turnCount;
    }

    public Team GetCurrentTurn
    {
        get { return _currentTurn; }
    }

    public void CheckIfGameFinished()
    {
        throw new NotImplementedException();
    }
}
