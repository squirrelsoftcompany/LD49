using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivableBehaviour : MonoBehaviour
{

    protected bool isMouseOver = false;
    protected bool isActive = false;

    public ModuleBehavior mParentModule;

    public float actionCooldown = 0.0f;
    public float cooldowntimeRemaining = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseOver()
    {
        //Leave action if cooldown is not over
        //if (cooldowntimeRemaining > 0)
        //{
        //    cooldowntimeRemaining -= Time.deltaTime;
        //    return;
        //}

        if (IsActivable())
        {
            if (!isMouseOver)
            {
                Material[] materials = this.GetComponent<MeshRenderer>().materials;
                foreach (Material currentMat in materials)
                {
                    if (currentMat.name.Contains("Hovered"))
                    {
                        currentMat.color = new Color(currentMat.color.r, currentMat.color.g, currentMat.color.b, 0.5f);
                    }
                }
                isMouseOver = true;
            }
            
            //Mouse is over the object, we can interact with it
            if (Input.GetMouseButtonDown(0))
            {
                clickDownBehavior();
            }
            if (Input.GetMouseButtonUp(0))
            {
                clickUpBehavior();
            }
        }
    }

    void OnMouseExit()
    {
        isMouseOver = false;
        Material[] materials = this.GetComponent<MeshRenderer>().materials;
        foreach (Material currentMat in materials)
        {
            if (currentMat.name.Contains("Hovered"))
            {
                currentMat.color = new Color(currentMat.color.r, currentMat.color.g, currentMat.color.b, 0f);
            }
        }
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
        return PovManager.Inst.CurrentRocketPOV == mParentModule.RocketPOV;
    }
}
