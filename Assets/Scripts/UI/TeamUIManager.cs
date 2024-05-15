using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamUIManager : MonoBehaviour
{
    private static TeamUIManager _instance;
    public static TeamUIManager Instance { get { return _instance; } }
    [SerializeField] GameObject HEALTH_BAR_PREFAB;
    [SerializeField] List<HealthBarUI> _healthBars;

    void Awake()
    {
        _instance = this;
        _healthBars = new List<HealthBarUI>();

        TeamSwitcher.OnSwitch += UpdateGradients;
    }

    public HealthBarUI CreateHealthBar(HealthManager healthManager)
    {
        HealthBarUI healthBar = Instantiate(HEALTH_BAR_PREFAB, this.transform).GetComponent<HealthBarUI>();
        healthBar.Initialize(healthManager);
        _healthBars.Add(healthBar);
        return healthBar;
    }

    void UpdateGradients(int index)
    {
        Debug.Log($"Index: {index}");
        for (int i = 0; i < _healthBars.Count; i++)
        {
            _healthBars[i].UpdateIndicator(i == index);
        }
    }
}
