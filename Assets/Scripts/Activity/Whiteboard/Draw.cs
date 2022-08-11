using UnityEngine;
using Photon.Pun;
using System.IO;

public class Draw : MonoBehaviour
{
    [SerializeField] public GameObject brush, drawPlane;
    [SerializeField] float fl_BrushSize = 0.01f;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(Ray, out hit))
            {
                Quaternion rotation = Quaternion.Euler(0f, 0f, 90f);
                var go = PhotonNetwork.Instantiate(Path.Combine("Prefabs/Activities", "Brush"), hit.point + Vector3.up * 0.1f, Quaternion.identity);
                go.transform.localScale = Vector3.one * fl_BrushSize;
            }
        }
    }

    public void Drawing(bool state)
    {

    }
}
