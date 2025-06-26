using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BeginningPreview : MonoBehaviour
{

    [SerializeField] Transform cameraSpotsParent;
    [SerializeField] Transform cameraOGPosition;

    [SerializeField] int childIndex = 0;

    float panDuration = 0;

    Vector3[] directions = new Vector3[] { Vector3.right, Vector3.forward, Vector3.left, Vector3.back };

    float panSpeed = 10;

    [SerializeField] bool previewDone = false;

    [SerializeField] GameObject titleLogo;

    [SerializeField] GameObject goText;

    // Start is called before the first frame update
    void Start()
    {
        titleLogo.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!previewDone)
        {
            if (panDuration <= 0)
            {
                if (childIndex >= cameraSpotsParent.childCount)
                {
                    transform.position = cameraOGPosition.position + Vector3.back * 12 + Vector3.up * 6;
                }
                else
                {
                    transform.position = cameraSpotsParent.GetChild(childIndex).position;
                }
                panDuration = 2.5f;
            }
            else
            {
                if (childIndex >= cameraSpotsParent.childCount)
                {
                    transform.position = cameraOGPosition.position + Vector3.back * 12 + Vector3.up * 6;
                    previewDone = true;
                }
                else
                {
                    transform.localPosition += directions[childIndex] * Time.deltaTime * panSpeed;
                }
                panDuration -= Time.deltaTime;
            }
            if (panDuration <= 0)
            {
                childIndex++;
            }
        }
        if (previewDone)
        {
            transform.position = Vector3.MoveTowards(transform.position, cameraOGPosition.position, Time.deltaTime * panSpeed);

            if (Vector3.Distance(transform.position, cameraOGPosition.position) < 0.1f)
            {
                titleLogo.SetActive(false);
                goText.SetActive(true);

                transform.position = cameraOGPosition.position;
                GameController.GC.begin = true;
                enabled = false;
            }
        }

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                // Click was on UI — ignore
                return;
            }
            titleLogo.SetActive(false);
            goText.SetActive(true);
            Debug.Log("skipping...");
            transform.position = cameraOGPosition.position;
            GameController.GC.begin = true;
            enabled = false;
        }
    }
}
