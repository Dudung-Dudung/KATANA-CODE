using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationManager : MonoBehaviour
{
    public RectTransform currentView;
    // Start is called before the first frame update

    public void NavTabBarClick(RectTransform newview)
    {
        if(currentView != null)
        {
            currentView.gameObject.SetActive(false);
        }
        currentView = newview;
        currentView.gameObject.SetActive(true);
    }
}
