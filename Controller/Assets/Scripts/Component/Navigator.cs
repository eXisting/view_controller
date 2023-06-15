using System;
using System.Collections.Generic;
using Screen;

namespace Component
{
  public static class Navigator
  {
    private static List<Page> _screens;

    private static Page _current;

    public static void Configure(List<Page> route)
    {
      if (route.Count != System.Enum.GetValues(typeof(Enum.Screen)).Length)
        throw new ArgumentException("Need to double check quantity of screens");

      foreach (var page in route) 
        page.Close();

      _screens = route;
    }
    
    public static Page Open(Enum.Screen next)
    {
      var nextPage = _screens[(int)next];
      
      if (_current == nextPage)
        return _current;
      
      if (_current != null) 
        _current.Close();
      
      _current = nextPage;
      _current.Open();

      return _current;
    }
  }
}