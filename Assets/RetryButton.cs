using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButton : MonoBehaviour
{
    public GameObject sceneEnd;
    Vector3 startSize;

    void Start()
    {
        startSize = transform.localScale;
    }

    void Update()
    {

    }

    private void OnMouseOver()
    {
        transform.localScale = startSize * 1.1f;
    }

    private void OnMouseExit()
    {
        transform.localScale = startSize;
    }

    private void OnMouseUp()
    {
        transform.localScale = startSize;
        gameObject.SetActive(false);
        SceneManager.LoadScene("SampleScene");
    }
}
