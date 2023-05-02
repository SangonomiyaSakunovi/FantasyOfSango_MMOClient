using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Developer : SangonomiyaSakunovi
//Discription: Base system, need define the UI method.

public class BaseSystem : MonoBehaviour
{
    protected NetService netService;
    protected ResourceService resourceService;
    protected AudioService audioService;

    public virtual void InitSystem()
    {
        netService = NetService.Instance;
        resourceService = ResourceService.Instance;
        audioService = AudioService.Instance;
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
