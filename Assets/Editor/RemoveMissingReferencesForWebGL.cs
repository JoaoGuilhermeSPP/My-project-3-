#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class BrokenPrefabDeleter : EditorWindow
{
    [MenuItem("Tools/Deletar Prefabs Corrompidos")]
    public static void ShowWindow()
    {
        GetWindow<BrokenPrefabDeleter>("Deletar Prefabs Corrompidos");
    }

    void OnGUI()
    {
        GUILayout.Label("Deleta permanentemente objetos com prefabs faltando", EditorStyles.boldLabel);

        if (GUILayout.Button("DELETAR agora (sem volta!)"))
        {
            int deletados = DeleteBrokenPrefabs();
            Debug.Log($"[Limpeza de Prefabs] Total deletados: {deletados}");
        }
    }

    int DeleteBrokenPrefabs()
    {
        int count = 0;
        foreach (GameObject obj in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            count += ProcessObject(obj);
        }

        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        return count;
    }

    int ProcessObject(GameObject obj)
    {
        int deleted = 0;

        // Se for instância de prefab E o asset estiver faltando
        if (PrefabUtility.GetPrefabInstanceStatus(obj) == PrefabInstanceStatus.MissingAsset &&
            PrefabUtility.GetCorrespondingObjectFromSource(obj) == null)
        {
            Debug.LogWarning($"[DELETADO] {obj.name} (Prefab quebrado)");
            Object.DestroyImmediate(obj);
            return 1;
        }

        // Precisamos clonar a lista porque vamos possivelmente destruir objetos
        var children = new Transform[obj.transform.childCount];
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            children[i] = obj.transform.GetChild(i);
        }

        foreach (Transform child in children)
        {
            if (child != null)
                deleted += ProcessObject(child.gameObject);
        }

        return deleted;
    }
}
#endif
