using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteManager : MonoBehaviour
{
    private const string PiecesTexture = "Graphics/Sprites/Pieces/Texture/Pieces";
    private Dictionary<string, Sprite> _cachedPieceSprites;

    private Dictionary<PieceType, string> pieceSpriteNames = new Dictionary<PieceType, string>
    {
        { PieceType.WhitePawn, "PawnW" },
        { PieceType.BlackPawn, "PawnB" },
        { PieceType.WhiteKnight, "KnightW" },
        { PieceType.BlackKnight, "KnightB" },
        { PieceType.WhiteBishop, "BishopW" },
        { PieceType.BlackBishop, "BishopB" },
        { PieceType.WhiteRook, "RookW" },
        { PieceType.BlackRook, "RookB" },
        { PieceType.WhiteQueen, "QueenW" },
        { PieceType.BlackQueen, "QueenB" },
        { PieceType.WhiteKing, "KingW" },
        { PieceType.BlackKing, "KingB" }
    };

    private static SpriteManager _instance;

    /// <summary>
    /// Get the <c>SpriteManager</c> instance, or create if it doesn't exist.
    /// </summary>
    public static SpriteManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SpriteManager>();
                if (_instance == null)
                {
                    GameObject spriteManagerObject = new GameObject("SpriteManager");
                    _instance = spriteManagerObject.AddComponent<SpriteManager>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _cachedPieceSprites = LoadPieceSprites();
    }

    /// <summary>
    /// Set the sprite of the <c>GameObject</c> according to the <c>pieceType</c>.
    /// </summary>
    /// <param name="pieceType">The <c>PieceType</c> to retrieve the sprite from.</param>
    /// <param name="pieceImage">The sprite to replace the image to.</param>
    public void SetImageSprite(PieceType pieceType, Image pieceImage)
    {
        if (!pieceSpriteNames.TryGetValue(pieceType, out string spriteName))
        {
            Debug.LogError($"No sprite name found for {pieceType}");
            return;
        }

        if (!_cachedPieceSprites.TryGetValue(spriteName, out Sprite sprite))
        {
            Debug.LogError($"No sprite found for {spriteName}");
            return;
        }

        pieceImage.sprite = sprite;
    }

    /// <summary>
    /// Loads all the sprites of the pieces in the dictionary
    /// </summary>
    public Dictionary<string, Sprite> LoadPieceSprites()
    {
        Sprite[] allPieceSprites = Resources.LoadAll<Sprite>(PiecesTexture);
        Dictionary<string, Sprite> spriteDictionary = new Dictionary<string, Sprite>();

        foreach (Sprite pieceSprite in allPieceSprites)
        {
            spriteDictionary[pieceSprite.name] = pieceSprite;
        }

        return spriteDictionary;
    }
}
