using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class LoadingTxLooper : MonoBehaviour
{
    private List<string> dots = new List<string>() { ".", "..", "..." };

    [SerializeField] private Text _loadingDotTx;

    private bool isLooper = false;

    private void Awake()
    {
        StartCoroutine(StartLooper());
    }

    private IEnumerator StartLooper()
    {
        isLooper = true;
        while (isLooper)
        {
            yield return DotLooper();
        }
    }

    private IEnumerator DotLooper()
    {
        foreach (var d in dots)
        {
            _loadingDotTx.text = d;
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void OnDestroy()
    {
        isLooper = false;
    }
}
