using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenu : MonoBehaviour
{
    public Vector2 mInputPosition;
    public float mInputDistance;

    public ActivableConnector mConnector;

    public GameObject mMenuGO;
    public GameObject[] mHoveredGO;
    public GameObject[] mNormalGO;

    private int mHoveredElement =-1;
    private bool isVisible;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 menuPosition = mMenuGO.GetComponent<RectTransform>().localPosition;
        mInputPosition.x = ( Input.mousePosition.x - (Screen.width / 2f)) - menuPosition.x;
        mInputPosition.y = ( Input.mousePosition.y - (Screen.height / 2f))- menuPosition.y;
        mInputDistance = mInputPosition.magnitude;
        mInputPosition.Normalize();

        if(mInputDistance < 100.0f)
        {
            mHoveredElement = -1;
            for (int i = 0; i < mNormalGO.Length; ++i)
            {
                mHoveredGO[i].SetActive(false);
                mNormalGO[i].SetActive(true);
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
                    mHoveredGO[i].SetActive(true);
                    mNormalGO[i].SetActive(false);
                    mHoveredElement = i;
                }
                else
                {
                    mHoveredGO[i].SetActive(false);
                    mNormalGO[i].SetActive(true);
                }
            }
        }

        if(Input.GetMouseButtonDown(0))
        {
            if (mConnector && mMenuGO.activeSelf)
            {
                if( isVisible ) //Add visible boolean to delay the menu display. Otherwise when the menu is display by clic, the selection catching the same clic and close immediately the menu...
                {
                    switch (mHoveredElement)
                    {
                        case 0:
                            mConnector.connectFuel(Fuel.eE1);
                            break;
                        case 1:
                            mConnector.connectFuel(Fuel.eE2);
                            break;
                        case 2:
                            mConnector.connectFuel(Fuel.eE3);
                            break;
                        default:
                            break;
                    }
                    hide();
                    isVisible = false;
                }
                else
                {
                    isVisible = true;
                }
            }
        }

    }

    public void setConnector(ActivableConnector pConnector)
    {
        mConnector = pConnector;
    }

    public void show()
    {
        float lX = Input.mousePosition.x - (Screen.width / 2f);
        float lY = Input.mousePosition.y - (Screen.height / 2f);
        mMenuGO.GetComponent<RectTransform>().localPosition = new Vector3(lX, lY, 0);


        mMenuGO.SetActive(true);
    }

    public void hide()
    {
        mMenuGO.SetActive(false);
    }
}
