using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ergolInTank
{
    public ergolInTank(Fuel x, float y)
    {
        this.ergolType = x;
        this.quantity = y;
    }

    public Fuel ergolType; //should be an enum
    public float quantity;
}

public class Monitor : MonoBehaviour
{
    List<ergolInTank> mErgolStack = new List<ergolInTank> {};
    List<ergolInTank> mErgolLimitsStack = new List<ergolInTank> {};
    float mPressure = 0;
    float mTemp = 0;

    List<Image> mErgolImages;
    public GameObject mTankBase;
    public GameObject mTankTop;
    public GameObject mGaz;

    // Pressure gauge
    public float mPressureZeroAngle = -40;
    public float mPressureMaxAngle = 200;
    public GameObject mPressureNeedle;

    // Temp gauge
    public float mTempZeroAngle = -40;
    public float mTempMaxAngle = 200;
    public GameObject mTempNeedle;

    public Color mMinPressure;
    public Color mMaxPressure;

    public Sprite mLimit;
    public Sprite mLimitCursor;

    public Color mE1;
    public Color mE2;
    public Color mE3;
    public Color mE4;
    public Color mE5;
    public Color mE6;
    public Color mE7;


    // Start is called before the first frame update
    void Start()
    {
        UpdateUI();
        UpdateLimitsUI();
        UpdatePressureUI();
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void setModuleInformation(List<ergolInTank> pErgolTank, float pPressure, float pTemp)
    {
        mErgolStack = pErgolTank;
        mPressure = pPressure;
        mTemp = pTemp;

        if (mErgolStack != null) UpdateUI();
        UpdatePressureUI();
        UpdateTempUI();
        UpdateLimitsUI();
    }

    
    public void setModuleGoalInformation(List<ergolInTank> pErgolGoalTank)
    {
        mErgolLimitsStack = pErgolGoalTank;
        if (mErgolLimitsStack != null) UpdateLimitsUI();
        UpdatePressureUI();
    }


    void UpdateUI()
    {

        //Update ergols images position
        foreach (Transform child in mTankBase.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        float ergolLevel = 0;
        foreach (ergolInTank ergoleElement in mErgolStack)
        {
            GameObject newObject = new GameObject("ErgolLayer");
            newObject.transform.parent = mTankBase.transform;
            RectTransform rectTransform = newObject.AddComponent<RectTransform>();
            rectTransform.localPosition = new Vector3(0, 0, 0);
            rectTransform.localScale = new Vector3(1, 1, 1);
            rectTransform.pivot = new Vector2(0.5f, 0);
            rectTransform.sizeDelta = new Vector2(100, 0);
            Image currentErgol = newObject.AddComponent<Image>();
            switch (ergoleElement.ergolType)
            {
                case Fuel.eE1:
                    currentErgol.color = mE1;
                    break;
                case Fuel.eE2:
                    currentErgol.color = mE2;
                    break;
                case Fuel.eE3:
                    currentErgol.color = mE3;
                    break;
                case (Fuel.eE1 | Fuel.eE2):
                    currentErgol.color = mE4;
                    break;
                case (Fuel.eE1 | Fuel.eE3):
                    currentErgol.color = mE5;
                    break;
                case (Fuel.eE2 | Fuel.eE3):
                    currentErgol.color = mE6;
                    break;
                case (Fuel.eE1 | Fuel.eE2 | Fuel.eE3):
                    currentErgol.color = mE7;
                    break;
                default:
                    break;
            }
            rectTransform.localPosition = new Vector3(rectTransform.transform.localPosition.x, TankRelativePosition(ergolLevel), rectTransform.transform.localPosition.z);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, TankRelativePosition(ergoleElement.quantity));
            ergolLevel += ergoleElement.quantity;
        }
    }

    void UpdateLimitsUI()
    {
        float ergolLevel = 0;
        foreach (ergolInTank ergolelimitElement in mErgolLimitsStack)
        {

            GameObject newObject = new GameObject("ErgolLimitLayer");
            newObject.transform.parent = mTankBase.transform;
            RectTransform rectTransform = newObject.AddComponent<RectTransform>();
            rectTransform.localPosition = new Vector3(0, 0, 0);
            rectTransform.localScale = new Vector3(1, 1, 1);
            rectTransform.sizeDelta = new Vector2(100, 0);

            Image currentErgol = newObject.AddComponent<Image>();
            currentErgol.sprite = mLimit;

            switch (ergolelimitElement.ergolType)
            {
                case Fuel.eE1:
                    currentErgol.color = mE1;
                    break;
                case Fuel.eE2:
                    currentErgol.color = mE2;
                    break;
                case Fuel.eE3:
                    currentErgol.color = mE3;
                    break;
                case (Fuel.eE1 | Fuel.eE2):
                    currentErgol.color = mE4;
                    break;
                case (Fuel.eE1 | Fuel.eE3):
                    currentErgol.color = mE5;
                    break;
                case (Fuel.eE2 | Fuel.eE3):
                    currentErgol.color = mE6;
                    break;
                case (Fuel.eE1 | Fuel.eE2 | Fuel.eE3):
                    currentErgol.color = mE7;
                    break;
                default:
                    break;
            }
            rectTransform.localPosition = new Vector3(rectTransform.transform.localPosition.x, TankRelativePosition(ergolelimitElement.quantity), rectTransform.transform.localPosition.z);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 10.0f);
            ergolLevel = ergolLevel + ergolelimitElement.quantity;
        }
    }

    void UpdatePressureUI()
    {
        mGaz.GetComponent<Image>().color = Color.Lerp(mMinPressure, mMaxPressure, mPressure/100.0f);
        float NeedleAngle = Mathf.Lerp(mPressureZeroAngle, mPressureMaxAngle, mPressure/100.0f);
        Vector3 currentNeedleRot = mPressureNeedle.GetComponent<RectTransform>().localRotation.eulerAngles;
        mPressureNeedle.GetComponent<RectTransform>().localRotation = Quaternion.Euler(currentNeedleRot.x, currentNeedleRot.y, NeedleAngle);
    }

    void UpdateTempUI()
    {
        float NeedleAngle = Mathf.Lerp(mTempZeroAngle, mTempMaxAngle, mTemp / 100.0f);
        Vector3 currentNeedleRot = mTempNeedle.GetComponent<RectTransform>().localRotation.eulerAngles;
        mTempNeedle.GetComponent<RectTransform>().localRotation = Quaternion.Euler(currentNeedleRot.x, currentNeedleRot.y, NeedleAngle);
    }


    private float TankRelativePosition(float pPercent)
    {
        float distance = mTankTop.transform.localPosition.y - mTankBase.transform.localPosition.y;
        return (pPercent/100 * distance);
    }


}
