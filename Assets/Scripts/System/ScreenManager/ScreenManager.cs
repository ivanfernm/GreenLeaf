using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScreenManager : MonoBehaviour
{
    //public ISubject _thingToObserv;
    public Button DesactivateOnPause; 
    public static ScreenManager instance;
    Stack<Iscreen> screens = new Stack<Iscreen>();

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        //_thingToObserv = FindObjectOfType<PauseScreen>();
        /*if (_thingToObserv != null)
        {
            _thingToObserv.Subscribe(this);     
        }
        else
        {
           // Debug.Log(this.name + "no se pudo subscribir");
        }*/
    }

    public void Push(Iscreen screen)
    {
        if (screens.Count > 0)
        {
            screens.Peek().Deactivate();
        }
        screens.Push(screen);
        screen.Activate();
    }
    public void Push(string resource) 
    {
        var go = Instantiate(Resources.Load<GameObject>(resource));
        Push(go.GetComponent<Iscreen>());
    }
    public void Pop() 
    {
        if (screens.Count < 1)
        {
            return;
        }
        screens.Pop().Free();
        if (screens.Count > 0)
        {
            screens.Peek().Activate();
        }
    }
    public void Clear() 
    {
        while(screens.Count  > 0) 
        {
            screens.Pop().Free();
        }
    }
    public void FreezeTime(){ Time.timeScale = 0;}
    public void ReanudeTime(){Time.timeScale = 1;}

    public void OnNotify(string ActionToMake)
    {      
    }
    public void TurnOnMenuButton()
    {
            DesactivateOnPause.interactable = true;
    }
    public void TurnOffMenuButton()
    {
          DesactivateOnPause.interactable = false;   

    }
}

