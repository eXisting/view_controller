using System;
using System.Collections.Generic;
using Component;
using Enum;
using UnityEngine;

namespace UI
{
  public class SwitcherPanel : MonoBehaviour
  {
    internal event Action<Feature> FeatureSelected; 

    [SerializeField] private List<Switcher> switchers;

    private void Start()
    {
      foreach (Feature feature in System.Enum.GetValues(typeof(Feature)))
        switchers[(int)feature].button.onClick.AddListener(() => Select(feature));
    }

    private void OnDestroy()
    {
      foreach (var switcher in switchers) 
        switcher.button.onClick.RemoveAllListeners();
    }

    private void Select(Feature selected)
    {
      foreach (Feature feature in System.Enum.GetValues(typeof(Feature)))
      {
        if (feature == selected)
          FeatureSelected?.Invoke(feature);

        var switcher = switchers[(int)feature];
        switcher.image.sprite = feature == selected ? switcher.onLook : switcher.offLook;
      }
    }
  }
}