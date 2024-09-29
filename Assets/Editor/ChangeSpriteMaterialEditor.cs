using UnityEngine;
using UnityEditor;

public class ChangeSpriteMaterialEditor : EditorWindow
{
    public Material newMaterial;

    [MenuItem("Tools/Change All Sprite Materials")]
    public static void ShowWindow()
    {
        GetWindow<ChangeSpriteMaterialEditor>("Change All Sprite Materials");
    }

    void OnGUI()
    {
        GUILayout.Label("새로운 Material 선택", EditorStyles.boldLabel);
        newMaterial = (Material)EditorGUILayout.ObjectField("Material", newMaterial, typeof(Material), false);

        if (GUILayout.Button("모든 스프라이트 Material 변경"))
        {
            ChangeAllSpriteMaterials();
        }
    }

    void ChangeAllSpriteMaterials()
    {
        if (newMaterial == null)
        {
            Debug.LogError("Material이 설정되지 않았습니다!");
            return;
        }

        SpriteRenderer[] spriteRenderers = FindObjectsOfType<SpriteRenderer>();

        foreach (SpriteRenderer sr in spriteRenderers)
        {
            sr.sharedMaterial = newMaterial;
        }

        Debug.Log(spriteRenderers.Length + "개의 스프라이트 material이 변경되었습니다.");
    }
}
