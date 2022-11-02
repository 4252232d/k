using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarrisGun : MonoBehaviour
{
    public Camera mainCamera; // ������ ��������� �� ������� ����
    public float impulseForce = 2500; // ���� ������������
    public float impulseDistance = 5; // ���������� � �������� ����� �������� ������
    public float maxImpulseMass = 15; // ������������ ����� �������, ������� ����� ��������
    public float gravityForce = 20; // ���� � ������� ����� ����������� ������
    public float gravityDistance = 25; // ���������� � �������� ����� ��������� ������
    public float maxGravityMass = 100; // ������������ ����� �������, ������� ����� ���������
    public float minDistance = 5; // ��������� � ������� ������ ����� ��������� �����������
    public float movementSpeed = 25; // �������� �������� ������������ ������� (�����������)

    private Transform obj;
    private GameObject localPoint;
    private GameObject clone;
    private bool gravity = true;
    private bool move;
    private float curTimeout;

    void Awake()
    {
        localPoint = new GameObject();
        localPoint.transform.parent = mainCamera.transform;
        localPoint.transform.forward = mainCamera.transform.forward;
        localPoint.transform.localPosition = new Vector3(0, 0, 3); // ���������� �� ������� �������� ����������� ������
    }

    void FixedUpdate()
    {
        if (obj) obj.GetComponent<Rigidbody>().position = Vector3.Lerp(obj.GetComponent<Rigidbody>().position, localPoint.transform.position, movementSpeed * Time.smoothDeltaTime);
    }

    void ResetObj()
    {
        if (obj)
        {
            obj.GetComponent<Rigidbody>().useGravity = true;
            obj.GetComponent<Rigidbody>().freezeRotation = false;
            obj = null;
            Destroy(clone);
        }
    }

    void Update()
    {
        Vector3 center = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(center);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.rigidbody) // ������, ��� ������� � �������
            {
                if (Input.GetMouseButtonDown(0) && hit.distance < impulseDistance && hit.rigidbody.mass < maxImpulseMass)
                {
                    if (obj) gravity = false;
                    ResetObj();
                    hit.rigidbody.AddForce(ray.direction.normalized * impulseForce);
                }

                if (Input.GetMouseButton(1) && hit.distance < gravityDistance && gravity && hit.rigidbody.mass < maxGravityMass && !obj)
                {
                    if (hit.distance > minDistance)
                    {
                        hit.rigidbody.AddForce(-ray.direction.normalized * gravityForce);
                    }
                    else
                    {
                        move = true;
                        obj = hit.transform;
                        obj.GetComponent<Rigidbody>().Sleep();
                        obj.GetComponent<Rigidbody>().useGravity = false;
                        obj.GetComponent<Rigidbody>().freezeRotation = true;

                        // �������� ��������, ����������� ���������� � ���������� �������� localPoint
                        clone = new GameObject();
                        clone.transform.position = obj.transform.position;
                        clone.transform.rotation = obj.transform.rotation;
                        clone.transform.parent = localPoint.transform;
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            gravity = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            if (!move) ResetObj();
            move = false;
        }

        if (!obj)
        {
            return;
        }
        obj.transform.rotation = clone.transform.rotation; // �������� �������� �� �����, ����� ������ �������� ��� ��������

        float dis = Vector3.Distance(obj.transform.position, localPoint.transform.position); // ��������� ����� ������� ������� � ������ ��� ����������

        // ���� ��� ������ ���������� ��������, � ������� 3 ������, �� �����
        // ��� ����� �� ������, ����� ������ "������� � �������"
        if (dis > 0.8f)
        {
            curTimeout += Time.deltaTime;
            if (curTimeout > 3)
            {
                curTimeout = 0;
                gravity = false;
                ResetObj();
            }
        }
        else
        {
            curTimeout = 0;
        }
    }
}