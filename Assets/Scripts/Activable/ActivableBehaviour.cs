using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActivableBehaviour : MonoBehaviour
{

    protected bool isMouseOver = false;
    protected bool isActive = false;

    public ModuleBehavior mParentModule;

    private Outline outlineScript = null;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        outlineScript = gameObject.AddComponent<Outline>();
        outlineScript.OutlineColor = Color.green;
        outlineScript.OutlineMode = Outline.Mode.OutlineAll;
        outlineScript.OutlineWidth = 2;
        outlineScript.enabled = false;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    void OnMouseOver()
    {
        if (IsActivable())
        {
            if (!isMouseOver)
            {
                outlineScript.enabled = true;
                isMouseOver = true;
            }
            
            //Mouse is over the object, we can interact with it
            if (Input.GetMouseButtonDown(0))
            {
                if ( ! EventSystem.current.IsPointerOverGameObject())
                {
                    clickDownBehavior();
                }
                    
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    clickUpBehavior();
                }
            }
        }
    }

    protected void OnMouseExit()
    {
        isMouseOver = false;
        outlineScript.enabled = false;
        mouseExit();
    }

    public virtual void clickDownBehavior()
    {
        //Do nothing. Override it to active action.
    }

    public virtual void clickUpBehavior()
    {
        //Do nothing. Override it to active action.
    }

    public virtual void Active()
    {
        //Do nothing. Override it to active action.
    }

    public virtual void Stop()
    {
        //Do nothing. Override it to active action.
    }

    public virtual void mouseExit()
    {
        //Do nothing. Override it to active action.
    }

    public virtual bool IsActivable()
    {
        return PovManager.Inst.CurrentRocketPOV == GetRocketPOV();
    }

    public virtual PovManager.RocketPOV GetRocketPOV()
    {
        return mParentModule.RocketPOV;
    }
}
