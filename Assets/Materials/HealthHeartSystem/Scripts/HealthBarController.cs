/*
 *  Author: ariel oliveira [o.arielg@gmail.com]
 */

using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    private GameObject[] heartContainers;
    private Image[] heartFills;

    public Transform heartsParent;
    public GameObject heartContainerPrefab;

    private void Start()
    {
        // Should I use lists? Maybe :)
        heartContainers = new GameObject[(int)PlayerStats.Instance.MaxTotalHealth];
        heartFills = new Image[(int)PlayerStats.Instance.MaxTotalHealth];

        PlayerStats.Instance.onHealthChangedCallback += UpdateHeartsHUD;
        InstantiateHeartContainers();
        UpdateHeartsHUD();
    }

    public void UpdateHeartsHUD()
    {
        SetHeartContainers();
        SetFilledHearts();
    }

    void SetHeartContainers()
    {
        for (int i = 0; i < heartContainers.Length; i++)
        {
            if (i < PlayerStats.Instance.MaxHealth)
            {
                heartContainers[i].SetActive(true);
            }
            else
            {
                heartContainers[i].SetActive(false);
            }
        }
    }

    void SetFilledHearts()
    {
        for (int i = 0; i < heartFills.Length; i++)
        {
            if (i < PlayerStats.Instance.Health)
            {
                heartFills[i].fillAmount = 1;
            }
            else
            {
                heartFills[i].fillAmount = 0;
            }
        }

        if (PlayerStats.Instance.Health % 1 != 0)
        {
            int lastPos = Mathf.FloorToInt(PlayerStats.Instance.Health);
            heartFills[lastPos].fillAmount = PlayerStats.Instance.Health % 1;
        }
    }

    void InstantiateHeartContainers()
    {
        InitializeHearts();

        //for (int i = 0; i < PlayerStats.Instance.MaxTotalHealth; i++)
        //{
        //    GameObject temp = Instantiate(heartContainerPrefab);
        //    temp.transform.SetParent(heartsParent, false);
        //    heartContainers[i] = temp;
        //    heartFills[i] = temp.transform.Find("HeartFill").GetComponent<Image>();
        //}
    }

    private void OnEnable()
    {
        PlayerStats.Instance.onHealthChangedCallback += UpdateHeartsHUD;
        InitializeHearts();
    }

    private void OnDisable()
    {
        PlayerStats.Instance.onHealthChangedCallback -= UpdateHeartsHUD;
    }

    public void InitializeHearts()
    {
        int heartsCount = (int)PlayerStats.Instance.MaxHealth; // Исправлено здесь

        // Очистка старых сердец
        foreach (Transform child in heartsParent)
        {
            Destroy(child.gameObject);
        }

        heartContainers = new GameObject[heartsCount];
        heartFills = new Image[heartsCount];

        for (int i = 0; i < heartsCount; i++)
        {
            GameObject temp = Instantiate(heartContainerPrefab, heartsParent);
            heartContainers[i] = temp;
            heartFills[i] = temp.transform.Find("HeartFill").GetComponent<Image>();
        }

        UpdateHeartsHUD();
    }
}
