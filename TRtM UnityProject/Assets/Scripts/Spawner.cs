using Articy.Unity;
using Articy.Unity.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public static PrefabManager pm = FindObjectOfType<PrefabManager>();

    public static ArticyFlowPlayer flowPlayer = FindObjectOfType<ArticyFlowPlayer>();
    
    public static GameObject SpawnPhrase(string text)
    {
        GameObject p = Instantiate(pm.phrasePrefab, pm.content);
        p.GetComponentInChildren<Text>().text = text;
        return p;
    }

    public static GameObject SpawnChoice(List<Branch> candidates)
    {
        GameObject bg = SpawnButtonGroup();
        foreach (Branch p in candidates)
        {
            IObjectWithMenuText target = p.Target as IObjectWithMenuText;
            SpawnButton(bg.transform, target.MenuText, p);
        }
        return bg;
    }

    public static GameObject SpawnButtonGroup()
    {
        GameObject bg = Instantiate(pm.buttonGroup, pm.content);
        return bg;
    }

    public static Button SpawnButton(Transform buttonGroup, string text, Branch exit)
    {
        Button b = Instantiate(pm.buttonPrefab, buttonGroup);
        b.GetComponentInChildren<Text>().text = text;
        b.onClick.AddListener(() => flowPlayer.Play(exit));
        return b;
    }

    public static Slider SpawnSlider(DateTime start, double delay)
    {
        DateTime end = start.AddMinutes(delay);
        double timePassed = (DateTime.Now - start).TotalMinutes;
        Slider s = Instantiate(pm.sliderPrefab, pm.content);
        s.value = (float)(timePassed / delay);
        return s;
    }
}
