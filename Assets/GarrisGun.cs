using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarrisGun : MonoBehaviour
{
    public Camera mainCamera; // камера персонажа от первого лица
    public float impulseForce = 2500; // сила отталкивания
    public float impulseDistance = 5; // расстояние с которого можно толкнуть объект
    public float maxImpulseMass = 15; // максимальная масса объекта, который можно толкнуть
    public float gravityForce = 20; // сила с которой будем притягивать объект
    public float gravityDistance = 25; // расстояние с которого можно притянуть объект
    public float maxGravityMass = 100; // максимальная масса объекта, который можно притянуть
    public float minDistance = 5; // дистанция с которой объект будет подхвачен гравипушкой
    public float movementSpeed = 25; // скорость движения захваченного объекта (сглаживание)

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
        localPoint.transform.localPosition = new Vector3(0, 0, 3); // расстояние на котором держится захваченный объект
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
            if (hit.rigidbody) // фильтр, все объекты с физикой
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

                        // создание пустышки, копирование трансформа и назначение родителя localPoint
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
        obj.transform.rotation = clone.transform.rotation; // передача вращения из клона, чтобы объект вращался как дочерний

        float dis = Vector3.Distance(obj.transform.position, localPoint.transform.position); // дистанция между центром объекта и точкой его назначения

        // если она больше указанного значения, в течении 3 секунд, то сброс
        // это нужно на случай, когда объект "застрял в графике"
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