using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneObject : MonoBehaviour
{
    /// <summary>
    /// Ʈ���� ����
    /// </summary>
    public float range = 2f;

    /// <summary>
    /// ������ �� �̸�
    /// </summary>
    public string targetSceneName;

    /// <summary>
    /// ���� ���� ���� ��ġ
    /// </summary>
    [Tooltip("������ ���� ���� ��ġ")]
    public Vector3 nextSpawnPosition;

    /// <summary>
    /// ���� ���� �ʵ����� Ȯ���ϴ� ����
    /// </summary>
    [Tooltip("���� ���� �ʵ������ üũ ( �� ������ ����� )")]
    public bool isField;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameManager.Instance.spawnPoint = nextSpawnPosition;
            GameManager.Instance.isField = this.isField;
            GameManager.Instance.ChangeToTargetScene(targetSceneName, other.gameObject);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.green;
        Handles.DrawWireDisc(transform.position, transform.up, range);
    }

#endif
}
