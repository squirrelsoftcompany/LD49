using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenu : MonoBehaviour
{
    public Vector2 mInputPosition;
    public float mInputDistance;

    public GameObject mMenuGO;
    public GameObject[] mHoveredGO;
    public GameObject[] mNormalGO;

    private int mHoveredElement =-1;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mInputPosition.x = Input.mousePosition.x - (Screen.width / 2f);
        mInputPosition.y = Input.mousePosition.y - (Screen.height / 2f);
        mInputDistance = mInputPosition.magnitude;
        mInputPosition.Normalize();

        if(mInputDistance < 100.0f)
        {
            mHoveredElement = -1;
            for (int i = 0; i < mNormalGO.Length; ++i)
            {
                mHoveredGO[i].active = false;
                mNormalGO[i].active = true;
            }
        }
        else if ( mInputPosition != Vector2.zero)
        {
            float angle = Mathf.Atan2(mInputPosition.y, -mInputPosition.x) / Mathf.PI;
            angle *= 180;
            angle -= 30;
            if(angle<0)
            {
                angle += 360;
            }

            float portionAngle = (360.0f / mNormalGO.Length);

            for (int i = 0; i < mNormalGO.Length; ++i)
            {
                if (angle  > (i * portionAngle) && angle < ((i+1) * portionAngle))
                {
                    mHoveredGO[i].active = true;
                    mNormalGO[i].active = false;
                    mHoveredElement = i;
                }
                else
                {
                    mHoveredGO[i].active = false;
                    mNormalGO[i].active = true;
                }
            }
        }

        if(Input.GetMouseButtonDown(0))
        {
            //TODO : Trigger actions here
            mMenuGO.active = false;
        }

    }
}
