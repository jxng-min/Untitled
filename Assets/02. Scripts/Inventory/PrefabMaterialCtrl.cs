using UnityEditor;
using UnityEngine;

public class PrefabMaterialCtrl : EditorWindow
{
    private Material m_to_add;
    private Material m_to_remove;

    [MenuItem("Tools/Jongmin/Outline System/Material to Prefab")]
    public static void ShowWindow()
    {
        GetWindow<PrefabMaterialCtrl>("Material to Prefab");
    }

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.richText = true; 

        GUILayout.Label("<color=white><b>선택한 오브젝트</b>에 <b>머티리얼</b>을 추가하거나 제거합니다.\n\n</color>", style);

        GUILayout.BeginHorizontal();
        m_to_add = (Material)EditorGUILayout.ObjectField("추가할 머티리얼", m_to_add, typeof(Material), false);
        if(GUILayout.Button("추가"))
        {
            AddMaterial();
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        m_to_remove = (Material)EditorGUILayout.ObjectField("제거할 머티리얼", m_to_remove, typeof(Material), false);
        if(GUILayout.Button("삭제"))
        {
            RemoveMaterial();
        }
        GUILayout.EndHorizontal();
    }

    private void AddMaterial()
    {
        foreach(var prefab in Selection.gameObjects)
        {
            foreach(var renderer in prefab.GetComponentsInChildren<Renderer>(true))
            {
                Material[] materials = renderer.sharedMaterials;
                bool material_exist = false;

                foreach(var material in materials)
                {
                    if(material == m_to_add)
                    {
                        material_exist = true;
                        break;
                    }
                }

                if(!material_exist)
                {
                    ArrayUtility.Add(ref materials, m_to_add);
                    renderer.sharedMaterials = materials;
                }
            }
        }
    }

    private void RemoveMaterial()
    {
        foreach(var prefab in Selection.gameObjects)
        {
            foreach(var renderer in prefab.GetComponentsInChildren<Renderer>(true))
            {
                Material[] materials = renderer.sharedMaterials;

                for(int i = 0; i < materials.Length; i++)
                {
                    if(materials[i] == m_to_remove)
                    {
                        ArrayUtility.RemoveAt(ref materials, i);
                        renderer.sharedMaterials = materials;
                        break;
                    }
                }
            }
        }
    }
}
