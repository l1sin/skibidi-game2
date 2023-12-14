using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    [SerializeField] private int _level;
    [SerializeField] private int _totalLevels;
    [SerializeField] private Light _sun;
    [SerializeField] private float _minSunDegree;
    [SerializeField] private float _maxSunDegree;
    [SerializeField] private AnimationCurve _sunIntencity;
    [SerializeField] private List<GameObject> _eyes;
    [SerializeField] private List<GameObject> _decals;
    [SerializeField] private List<GameObject> _lords;
    [SerializeField] private GameObject _redLight;
    [SerializeField][Range(0, 1)] private float _decalsStartSpawn;
    [SerializeField][Range(0, 1)] private float _eyesStartSpawn;
    [SerializeField][Range(0, 1)] private float _lordsStartSpawn;
    [SerializeField][Range(0, 1)] private float _redLightStartSpawn;

    private void Start()
    {
        _level = SaveManager.Instance.CurrentProgress.Level;
        SetEnvironment();
    }

    public void SetEnvironment()
    {
        if (_level > _totalLevels) _level = _totalLevels;
        float degreePerDay = (_maxSunDegree - _minSunDegree) / _totalLevels;
        float plusDegree = degreePerDay * _level;
        float evilAmount = (float)_level / (float)_totalLevels;
        evilAmount = Mathf.Clamp(evilAmount, 0, 1);

        int eyesToSpawn = Mathf.CeilToInt(_eyes.Count * evilAmount);
        int decalsToSpawn = Mathf.CeilToInt(_decals.Count * evilAmount);
        int lordsToSpawn = Mathf.CeilToInt(_lords.Count * evilAmount);

        if (evilAmount >= _eyesStartSpawn)
        {
            for (int i = 0; i < eyesToSpawn; i++)
            {
                _eyes[i].SetActive(true);
            }
        }

        if (evilAmount >= _decalsStartSpawn)
        {
            for (int i = 0; i < decalsToSpawn; i++)
            {
                _decals[i].SetActive(true);
            }
        }

        if (evilAmount >= _lordsStartSpawn)
        {
            for (int i = 0; i < lordsToSpawn; i++)
            {
                _lords[i].SetActive(true);
            }
        }

        if (evilAmount >= _redLightStartSpawn)
        {
            _redLight.SetActive(true);
        } 

        _sun.gameObject.transform.Rotate(Vector3.right * plusDegree);
        _sun.intensity = _sunIntencity.Evaluate(evilAmount);
    }
}
