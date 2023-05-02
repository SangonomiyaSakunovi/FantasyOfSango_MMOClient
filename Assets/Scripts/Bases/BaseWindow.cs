using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Developer : SangonomiyaSakunovi
//Discription: Base window, need define UI method.

public class BaseWindow : MonoBehaviour
{
    protected NetService netService = null;
    protected ResourceService resourceService = null;
    protected AudioService audioService = null;
    public void SetWindowState(bool isActive = true)
    {
        if (gameObject.activeSelf != isActive)
        {
            SetActive(gameObject, isActive);
        }
        if (isActive)
        {
            InitWindow();
        }
        else
        {
            ClearWindow();
        }
    }

    protected virtual void InitWindow()
    {
        netService = NetService.Instance;
        resourceService = ResourceService.Instance;
        audioService = AudioService.Instance;
    }

    protected virtual void ClearWindow()
    {
        netService = null;
        resourceService = null;
        audioService = null;
    }

    #region SetText Tools
    protected void SetText(TMP_Text tMP_Text, string text)
    {
        tMP_Text.text = text;
    }
    protected void SetText(TMP_Text tMP_Text, int number)
    {
        tMP_Text.text = number.ToString();
    }
    protected void SetText(Transform transform, string text)
    {
        transform.GetComponent<TMP_Text>().text = text;
    }
    protected void SetText(Transform transform, int number)
    {
        transform.GetComponent<TMP_Text>().text = number.ToString();
    }
    #endregion

    #region SetActive Tools
    protected void SetActive(GameObject gameObject, bool isActive = true)
    {
        gameObject.SetActive(isActive);
    }
    protected void SetActive(Transform transform, bool isActive = true)
    {
        transform.gameObject.SetActive(isActive);
    }
    protected void SetActive(RectTransform rectTransform, bool isActive = true)
    {
        rectTransform.gameObject.SetActive(isActive);
    }
    protected void SetActive(Image image, bool isActive = true)
    {
        image.transform.gameObject.SetActive(isActive);
    }
    protected void SetActive(TMP_Text tMP_Text, bool isActive = true)
    {
        tMP_Text.transform.gameObject.SetActive(isActive);
    }
    protected void SetActive(Button button, bool isActive = true)
    {
        button.transform.gameObject.SetActive(isActive);
    }
    #endregion
}
