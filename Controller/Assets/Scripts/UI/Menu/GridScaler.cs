using UnityEngine;
using UnityEngine.UI;


namespace UI
{
  public class GridScaler : MonoBehaviour
  {
    public GridLayoutGroup gridLayoutGroup;

    private void Start() => 
      ScaleGrid();

    private void ScaleGrid()
    {
      float screenWidth = UnityEngine.Screen.width;
      float screenHeight = UnityEngine.Screen.height;

      var cellWidth = screenWidth / 4f;
      var cellHeight = screenHeight / 8f;

      gridLayoutGroup.cellSize = new Vector2(cellWidth, cellHeight);
    }
  }
}