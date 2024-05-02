using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �ε� �̹����� ��Ʈ���ϴ� UI
/// </summary>
public class LoadingImageUI : MonoBehaviour
{
    /// <summary>
    /// �ε� �̹�����
    /// </summary>
    public Image[] loadImages;

    /// <summary>
    /// ��ȯ�� �ε� �̹��� ( 0 : ��ĭ, 1 : ä���� ĭ )
    /// </summary>
    public Sprite[] onoffImages;

    const int offImage = 0;
    const int onImage = 1;

    /// <summary>
    /// �ε� �̹��� ����
    /// </summary>
    const int loadImageCount = 4;

    /// <summary>
    /// �ε� �̹��� �ε���
    /// </summary>
    public int index = 0;

    /// <summary>
    /// ����� �ð�
    /// </summary>
    public float timeElapsed = 0.0f;

    /// <summary>
    /// �ð� �ӵ�
    /// </summary>
    public float speed = 2.0f;

    private void Awake()
    {
        // �ʱ�ȭ
        loadImages = new Image[loadImageCount];
        loadImages = GetComponentsInChildren<Image>();
    }

    /// <summary>
    /// ȣ�� �� ������ �ε� �̹����� �ٲٴ� �Լ�
    /// </summary>
    public void ChangeLoadingImages()
    {
        StartCoroutine(ImageCoroutine());
    }

    IEnumerator ImageCoroutine()
    {
        while(true)
        {
            timeElapsed += Time.deltaTime * speed;

            if (timeElapsed > 1.0f)
            {
                if (index > loadImages.Length - 1)
                {
                    // index���� loadImages ������ ������ �ʱ�ȭ
                    index = 0;

                    // �̹��� �ʱ�ȭ
                    foreach (Image image in loadImages)
                    {
                        image.sprite = onoffImages[offImage];
                    }
                }

                loadImages[index].sprite = onoffImages[onImage];  // �̹��� �ٲٱ�
                index++;

                timeElapsed = 0f;
            }

            yield return null;
        }
    }

    /// <summary>
    /// �ε��� ������ �� �̹����� �ٲٴ� �Լ�
    /// </summary>
    public void FinishLoadingImage()
    {
        StopAllCoroutines();

        foreach (Image image in loadImages)
        {
            image.sprite = onoffImages[onImage];
        }
    }
}
