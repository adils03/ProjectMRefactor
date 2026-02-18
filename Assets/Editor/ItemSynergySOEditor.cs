using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemSynergySO))]
public class ItemSynergySOEditor : Editor
{
    const int CELL = 25;

    public override void OnInspectorGUI()
    {
        var synergy = (ItemSynergySO)target;

        synergy.width = EditorGUILayout.IntField("Width", synergy.width);
        synergy.height = EditorGUILayout.IntField("Height", synergy.height);

        synergy.baseItemShape = (ItemShapeSO)EditorGUILayout.ObjectField(
            "Base Item Shape",
            synergy.baseItemShape,
            typeof(ItemShapeSO),
            false
        );

        EditorGUILayout.LabelField(
            "SHIFT + Click = Set Item Origin",
            EditorStyles.helpBox
        );

        if (synergy.baseItemShape == null || synergy.width <= 0 || synergy.height <= 0)
            return;

        GUILayout.Space(10);

        DrawGrid(synergy);

        if (GUI.changed)
            EditorUtility.SetDirty(synergy);
    }

    void DrawGrid(ItemSynergySO synergy)
    {
        var shape = synergy.baseItemShape;

        for (int y = synergy.height - 1; y >= 0; y--)
        {
            GUILayout.BeginHorizontal();

            for (int x = 0; x < synergy.width; x++)
            {
                var gridPos = new Vector2Int(x, y);
                var localItemPos = gridPos - synergy.itemOrigin;

                bool isItemCell = shape.itemCells.Contains(localItemPos);
                bool isSynergyCell = synergy.syngergyCells.Contains(gridPos);
                bool isOrigin = gridPos == synergy.itemOrigin;

                if (isOrigin)
                    GUI.backgroundColor = Color.yellow;
                else if (isItemCell)
                    GUI.backgroundColor = Color.green;
                else if (isSynergyCell)
                    GUI.backgroundColor = Color.cyan;
                else
                    GUI.backgroundColor = Color.gray;

                if (GUILayout.Button("", GUILayout.Width(CELL), GUILayout.Height(CELL)))
                {
                    if (Event.current.shift)
                    {
                        synergy.itemOrigin = gridPos;
                        GUI.changed = true;
                        return;
                    }

                    if (!isItemCell)
                    {
                        if (isSynergyCell)
                            synergy.syngergyCells.Remove(gridPos);
                        else
                            synergy.syngergyCells.Add(gridPos);

                        GUI.changed = true;
                    }
                }
            }

            GUILayout.EndHorizontal();
        }

        GUI.backgroundColor = Color.white;
    }
}