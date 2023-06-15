using System.Collections.Generic;
using Enum;
using UI;
using UnityEngine;

namespace Screen
{
  public class MainScreen : Page
  {
    [SerializeField] private List<GameObject> displays;
    [SerializeField] private SwitcherPanel switcherPanel;

    private void Start() => 
      switcherPanel.FeatureSelected += ChangeDisplay;

    private void OnDestroy() => 
      switcherPanel.FeatureSelected -= ChangeDisplay;

    public void ChangeDisplay(Feature feature)
    {
      for (var index = 0; index < displays.Count; index++) 
        displays[index].SetActive(index == (int)feature);
    }
  }
}