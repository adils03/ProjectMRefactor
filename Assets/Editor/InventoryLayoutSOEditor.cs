using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[CustomEditor(typeof(InventoryLayoutSO))]
public class InventoryLayoutSOEditor : Editor
{
    const int CELL = 25;

    ItemSO selectedItemType;
    Direction selectedDir = Direction.Down;

    Vector2 shapeScroll;

    public override void OnInspectorGUI()
    {
        var layout = (InventoryLayoutSO)target;

        layout.width = EditorGUILayout.IntField("Width", layout.width);
        layout.height = EditorGUILayout.IntField("Height", layout.height);

        GUILayout.Space(10);

        selectedDir = (Direction)EditorGUILayout.EnumPopup("Dir", selectedDir);

        GUILayout.Space(10);

        DrawShapePalette();

        GUILayout.Space(10);

        DrawGrid(layout);

        GUILayout.Space(10);

        if (GUILayout.Button("Clear All"))
        {
            layout.placedItems.Clear();
            EditorUtility.SetDirty(layout);
        }
    }

    void DrawShapePalette()
    {
        GUILayout.Label("Items", EditorStyles.boldLabel);

        var items = AssetDatabase.FindAssets("t:ItemSO")
            .Select(g => AssetDatabase.LoadAssetAtPath<ItemSO>(AssetDatabase.GUIDToAssetPath(g)))
            .ToList();

        shapeScroll = GUILayout.BeginScrollView(shapeScroll, GUILayout.Height(120));

        foreach (var s in items)
        {
            GUI.backgroundColor = selectedItemType == s ? Color.green : Color.white;

            if (GUILayout.Button(s.name))
                selectedItemType = s;
        }

        GUILayout.EndScrollView();
        GUI.backgroundColor = Color.white;
    }

    void DrawGrid(InventoryLayoutSO layout)
    {
        for (int y = layout.height - 1; y >= 0; y--)
        {
            GUILayout.BeginHorizontal();

            for (int x = 0; x < layout.width; x++)
            {
                Vector2Int cell = new(x, y);

                bool occupied = IsCellOccupied(layout, cell);
                bool isSynergy = IsSynergyCell(layout, cell);

                if (occupied)
                    GUI.backgroundColor = Color.green;
                else if (isSynergy)
                    GUI.backgroundColor = new Color(0.6f, 0.3f, 0.8f); // yeşil-mor karışımı
                else
                    GUI.backgroundColor = Color.gray;

                if (GUILayout.Button("", GUILayout.Width(CELL), GUILayout.Height(CELL)))
                {
                    if (occupied)
                        RemoveShapeAt(layout, cell);
                    else
                        TryPlaceShape(layout, cell);
                }
            }

            GUILayout.EndHorizontal();
        }

        GUI.backgroundColor = Color.white;
    }

    bool IsCellOccupied(InventoryLayoutSO layout, Vector2Int cell)
    {
        foreach (var ps in layout.placedItems)
        {
            foreach (var c in GetShapeCells(ps))
            {
                if (c == cell)
                    return true;
            }
        }
        return false;
    }

    bool IsSynergyCell(InventoryLayoutSO layout, Vector2Int cell)
    {
        foreach (var ps in layout.placedItems)
        {
            var synergy = ps.itemType.itemSynergy;
            if (synergy == null)
                continue;

            foreach (var s in synergy.syngergyCells)
            {
                Vector2Int localOffset = s - synergy.itemOrigin;
                Vector2Int rotated = Rotate(localOffset, ps.dir);
                Vector2Int worldCell = ps.origin + rotated;

                if (worldCell == cell)
                    return true;
            }
        }
        return false;
    }

    void TryPlaceShape(InventoryLayoutSO layout, Vector2Int origin)
    {
        if (selectedItemType == null)
            return;

        var test = new PlacedShapeData
        {
            itemType = selectedItemType,
            origin = origin,
            dir = selectedDir
        };

        foreach (var c in GetShapeCells(test))
        {
            if (c.x < 0 || c.y < 0 || c.x >= layout.width || c.y >= layout.height)
                return;

            if (IsCellOccupied(layout, c))
                return;
        }

        layout.placedItems.Add(test);
        EditorUtility.SetDirty(layout);
    }

    void RemoveShapeAt(InventoryLayoutSO layout, Vector2Int cell)
    {
        var target = layout.placedItems.FirstOrDefault(ps =>
            GetShapeCells(ps).Any(c => c == cell));

        if (target != null)
        {
            layout.placedItems.Remove(target);
            EditorUtility.SetDirty(layout);
        }
    }

    List<Vector2Int> GetShapeCells(PlacedShapeData ps)
    {
        var list = new List<Vector2Int>();
        var cells = ps.itemType?.itemShape?.itemCells;
        if (cells == null)
            return list;

        var rotated = new List<Vector2Int>();

        foreach (var p in cells)
        {
            rotated.Add(Rotate(p, ps.dir));
        }

        int minX = rotated.Min(v => v.x);
        int minY = rotated.Min(v => v.y);
        Vector2Int offset = new(-minX, -minY);

        foreach (var r in rotated)
            list.Add(ps.origin + r + offset);

        return list;
    }

    Vector2Int Rotate(Vector2Int p, Direction dir)
    {
        return dir switch
        {
            Direction.Down => p,
            Direction.Left => new Vector2Int(p.y, -p.x),
            Direction.Up => new Vector2Int(-p.x, -p.y),
            Direction.Right => new Vector2Int(-p.y, p.x),
            _ => p
        };
    }
}