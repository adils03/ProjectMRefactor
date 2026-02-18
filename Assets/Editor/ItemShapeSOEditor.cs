using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemShapeSO))]
public class ItemShapeSOEditor : Editor
{
    const int CELL_SIZE = 25;

    public override void OnInspectorGUI()
    {
        var shape = (ItemShapeSO)target;

        shape.width = EditorGUILayout.IntField("Width", shape.width);
        shape.height = EditorGUILayout.IntField("Height", shape.height);

        GUILayout.Space(10);

        for (int y = shape.height - 1; y >= 0; y--)
        {
            GUILayout.BeginHorizontal();
            for (int x = 0; x < shape.width; x++)
            {
                var pos = new Vector2Int(x, y);

                bool isItem = shape.itemCells.Contains(pos);

                if (isItem)
                    GUI.backgroundColor = Color.green;
                else
                    GUI.backgroundColor = Color.gray;

                if (GUILayout.Button("", GUILayout.Width(CELL_SIZE), GUILayout.Height(CELL_SIZE)))
                {
                    if (!isItem)
                    {
                        shape.itemCells.Add(pos);
                    }
                    else if (isItem)
                    {
                        shape.itemCells.Remove(pos);
                    }

                    EditorUtility.SetDirty(shape);
                }
            }
            GUILayout.EndHorizontal();
        }

        GUI.backgroundColor = Color.white;
    }
}