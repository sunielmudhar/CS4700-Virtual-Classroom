using UnityEngine;
using Photon.Pun;

public class ModelSelection : MonoBehaviour
{
    [SerializeField] public GameObject[] img_ModelSelection;

    public int int_ModelNumber = 0;

    public void OnClickArrow(int index)
    {
        if (index == 1)
        {
            int_ModelNumber = int_ModelNumber + 1;
        }
        else
        {
            int_ModelNumber = int_ModelNumber - 1;
        }

        ModelSelect(int_ModelNumber);
    }

    public void ModelSelect(int index)
    {

        if (index > img_ModelSelection.Length - 1)
        {
            index = 0;
        }
        else if (index < 0)
        {
            index = img_ModelSelection.Length;
        }

        Debug.Log("Meshnumber: " + index);

        for (int i = 1; i < img_ModelSelection.Length; i++)
        {
            img_ModelSelection[i].SetActive(false);
        }

        Debug.Log("Model Selection index: " + index);

        img_ModelSelection[index].SetActive(true);

        int_ModelNumber = index;
    }

}
