/*
 * Assets内の全MaterialのShaderを一括で置換するEditor拡張
 * 使い方
 * 1.Editorディレクトリ配下にScriptを配置
 * 2.AssetsにShaderReplaceが追加されるので、それを選択するとWindowsが開く
 * 3.Beforeに置換前のShader、Afterに置換後のShaderを選択
 * 4.Replaceボタンを押すと全部のMaterialでShaderを置換
 */

using System.Linq;
using UnityEditor;
using UnityEngine;

public class SharderReplacer : EditorWindow
{
    private int selectBeforeShaderIndex;
    private int selectAfterShaderIndex;

    [MenuItem("Assets/SharderReplace", false, 2000)]
    private static void Open()
    {
        GetWindow<SharderReplacer>();
    }

    private void OnGUI()
    {
        var sharders = ShaderUtil.GetAllShaderInfo();
        var sharderNames = sharders.Select(x => x.name).ToArray();

        selectBeforeShaderIndex = EditorGUILayout.Popup("Before", selectBeforeShaderIndex, sharderNames);
        selectAfterShaderIndex = EditorGUILayout.Popup("After", selectAfterShaderIndex, sharderNames);

        if (GUILayout.Button("Replace"))
        {
            ReplaceAll(sharderNames[selectBeforeShaderIndex], sharderNames[selectAfterShaderIndex]);
        }
    }

    private void ReplaceAll(string beforeShaderName, string afterShaderName)
    {
        var beforeShader = Shader.Find(beforeShaderName);
        var afterShader = Shader.Find(afterShaderName);

        // var guids = AssetDatabase.FindAssets("t: Material", null);
        // foreach (var guid in guids)
        // {
        //     var path = AssetDatabase.GUIDToAssetPath(guid);
        //     var material = AssetDatabase.LoadAssetAtPath<Material>(path);

        //     if (material != null && material.shader == beforeShader)
        //     {
        //         material.shader = afterShader;
        //     }

        // }

        // AssetDatabase.SaveAssets();

        GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in objects)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            // if (renderer != null && renderer.sharedMaterial != null)
            // {
            //     // マテリアルがある場合の処理
            //     Material material = renderer.sharedMaterial;
            //     if (material.shader == beforeShader)
            //     {
            //         // 特定のシェーダーを使用しているマテリアルが見つかった場合の処理
            //         material.shader = afterShader;
            //     }
            // }

            if (renderer != null && renderer.materials != null)
            {
                Material[] materials = renderer.materials;

                // 各マテリアルにアクセス
                for (int i = 0; i < materials.Length; i++)
                {
                    Material material = materials[i];
                    // マテリアルに対する処理を実行

                    if (material.shader == beforeShader)
                    {
                        // 特定のシェーダーを使用しているマテリアルが見つかった場合の処理
                        material.shader = afterShader;
                    }
                }
            }
        }
    }
}