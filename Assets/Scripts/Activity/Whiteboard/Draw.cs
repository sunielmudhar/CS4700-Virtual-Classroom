using UnityEngine;
using Photon.Pun;
using System.IO;

public class Draw : MonoBehaviour
{
    [SerializeField] public GameObject brush, participant;
    [SerializeField] float fl_BrushSize = 0.01f;
    [SerializeField] static bool bl_CanDraw = false, bl_Turn = false;

    /*
     Find a way to smooth out the lines
     */

    void Update()
    {
        participant = GameObject.Find("CurrentParticipant");

        if (Input.GetMouseButton(0) && participant.tag.Equals("Student") && bl_CanDraw)   //Only allow students to draw
        {
            var Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(Ray, out hit) && hit.collider.gameObject.tag.Equals("Plane"))   //Check to see if the raycast is hitting the plane
            {
                Quaternion rotation = Quaternion.Euler(0f, 0f, 90f);
                var go = PhotonNetwork.Instantiate(Path.Combine("Prefabs/Activities", "Brush"), hit.point + Vector3.up * 0.1f, Quaternion.identity);
                go.transform.localScale = Vector3.one * fl_BrushSize;
            }
        }
        else if (!bl_CanDraw)
        {
            Debug.Log("Cannot draw");
        }
    }

    public static void CanDraw(bool state)
    {
        bl_CanDraw = state;
    }
}
