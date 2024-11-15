using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Wall : MonoBehaviour, IRecipe
{
    private PlayerBuild playerBuild;
    private Grid grid;
    private SpriteRenderer spriteRenderer;
    public ItemsData itemData;
    public float craftTime;
    public Vector3Int gridPos;
    private HashSet<Vector3Int> wallPositions;
    public Dictionary<int, Sprite> wallVariants = new Dictionary<int, Sprite>();

    private Dictionary<int, int> bitmaskMapping = new Dictionary<int, int>
    {
        { 6 , 10},
        { 14, 2},
        { 12 , 0},
        { 15, 3 },
        {11, 7},
        {9, 5},
        {3, 15},
        {7,11},
        {13, 1},
        {5, 9},
        {1, 13},
        {10, 6},
        {2, 14},
        {0, 12},
        {4, 8},
        {8, 4}


    };

    [SerializeField]
    public Sprite[] WallSprites
    {
        //Load all sprites from the Resources folder
        get => Resources.LoadAll<Sprite>("Sprites/Building/Wall/" + itemData.itemName);
    }
    public RecipeData[] ItemsRecipe 
    { 
        get => itemData.itemRecipe;
        set => itemData.itemRecipe = value;
    }
    public float CraftTime
    {   get => itemData.itemBuildTime;
        set => itemData.itemBuildTime = value;
    }
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public void Init()
    {
        playerBuild = GameObject.FindObjectOfType<PlayerBuild>();
        wallPositions = playerBuild.wallPositions;
        grid = GameObject.FindObjectOfType<Grid>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = WallSprites[0];

        foreach (Sprite sprite in WallSprites)
        {
            Debug.Log(sprite.name);
        }
        // Set the wall positions
        SetWallVariants(gridPos);
    }
    void PopulateCorrectedWall()
    {
        

    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private int wbitmask = 0;
    public void SetWallVariants(Vector3Int gridPos)
    {
        if (grid == null)
        {
            Debug.LogError("Grid is null");
            return;
        }
        gridPos = grid.WorldToCell(transform.position);
        // Get the bitmask for the wall at the given position
        int bitmask = 0;
        // Check neighboring cells and set bits accordingly
        if (IsWall(gridPos + Vector3Int.up)) bitmask |= 1;       // Top
        if (IsWall(gridPos + Vector3Int.right)) bitmask |= 2;    // Right
        if (IsWall(gridPos + Vector3Int.down)) bitmask |= 4;     // Bottom
        if (IsWall(gridPos + Vector3Int.left)) bitmask |= 8;     // Left
        
        if (bitmaskMapping.ContainsKey(bitmask))
        {
            bitmask = bitmaskMapping[bitmask];
        }
        // Get the sprite for the bitmask
        Sprite wallSprite = WallSprites[bitmask];
        Debug.LogError("Wall bitmask: " + bitmask);
        wbitmask = bitmask;
        // Set the sprite
        spriteRenderer.sprite = wallSprite;

    }

    private void UpdateWallVariants(Vector3Int position)
    {
        int bitmask = 0;

        if (IsWall(position + Vector3Int.up)) bitmask |= 1;       // Top
        if (IsWall(position + Vector3Int.right)) bitmask |= 2;    // Right
        if (IsWall(position + Vector3Int.down)) bitmask |= 4;     // Bottom
        if (IsWall(position + Vector3Int.left)) bitmask |= 8;     // Left
        wbitmask = bitmask;
        if (bitmaskMapping.ContainsKey(bitmask))
        {
            bitmask = bitmaskMapping[bitmask];
        }
        Sprite wallSprite = WallSprites[bitmask];
        Debug.LogError("Wall bitmask: " + bitmask);
        spriteRenderer.sprite = wallSprite;
    }
    
    bool IsWall(Vector3Int position)
    {
        if (wallPositions == null)
        {
            Debug.LogError("wallPositions is null");
            return false;
        }

        return wallPositions.Contains(position);
    }

    public void UpdateNeighborWalls()
    {
        Vector3Int[] neighborOffsets = new Vector3Int[]
        {
            Vector3Int.up,
            Vector3Int.right,
            Vector3Int.down,
            Vector3Int.left
        };

        foreach (Vector3Int offset in neighborOffsets)
        {
            Vector3Int neighborPos = gridPos + offset;
            if (IsWall(neighborPos))
            {
                Wall neighborWall = GetWallAtPosition(neighborPos);
                if (neighborWall != null)
                {
                    neighborWall.UpdateWallVariants(neighborPos);
                }
            }
        }
    }

    private Wall GetWallAtPosition(Vector3Int position)
    {
        foreach (Wall wall in FindObjectsOfType<Wall>())
        {
            if (wall.gridPos == position)
            {
                return wall;
            }
        }
        return null;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(1, 1, 1));

        Handles.Label(transform.position, wbitmask.ToString(), new GUIStyle() { normal = new GUIStyleState() { textColor = Color.blue } });
        //Resize the labe
        
    
    }
}

[CustomEditor(typeof(Wall))]
public class WallEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Wall wall = (Wall)target;

        EditorGUILayout.LabelField("Wall Variants", EditorStyles.boldLabel);

        foreach (KeyValuePair<int, Sprite> kvp in wall.wallVariants)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Key: " + kvp.Key, GUILayout.Width(50));
            EditorGUILayout.ObjectField(kvp.Value, typeof(Sprite), allowSceneObjects: true);
            EditorGUILayout.EndHorizontal();
        }
    }
}