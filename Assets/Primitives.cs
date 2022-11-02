using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Primitives : MonoBehaviour
{
    enum TypeShell
    {
        Cube = PrimitiveType.Cube,
        Sphere = PrimitiveType.Sphere,
        Cylinder = PrimitiveType.Cylinder,
        Capsule = PrimitiveType.Capsule,
    }
    [SerializeField]
    TypeShell typeShell = TypeShell.Cylinder;

    enum TypeShooting { Fly, Teleport}

    [SerializeField]
    TypeShooting typeShooting = TypeShooting.Fly;


    private Camera cameraPlayer;

    [SerializeField]
    bool isLuminescent = false;
    // Start is called before the first frame update
    void Start()
    {
        cameraPlayer = GetComponent<Camera>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)){
            Shooting();
        }
        
    }
    private void Shooting()
    {
        switch (typeShell)
        {
            case TypeShell.Cube:
                CreateShell(PrimitiveType.Cube);
                break;
            case TypeShell.Sphere:
                CreateShell(PrimitiveType.Sphere);
                break;
            case TypeShell.Cylinder:
                CreateShell(PrimitiveType.Cylinder);
                break;
            case TypeShell.Capsule:
                CreateShell(PrimitiveType.Capsule);
                break;
            
            default:
            Debug.Log("Поступил некорректный снаряд, выбрана Sphere");
                CreateShell(PrimitiveType.Sphere);
                break;
        }
    }


    void CreateShell (PrimitiveType shell)
    {
        if (typeShooting == TypeShooting.Fly)
        {
            Fly(shell);
        }
        if (typeShooting == TypeShooting.Teleport)
        {
            Teleport(shell);
        }
    }
    void Fly (PrimitiveType shell)
    {
    GameObject shellInstantiate = GameObject.CreatePrimitive(shell);
    shellInstantiate.transform.position = transform.position + transform.forward;
        //выставляем градус направления
        shellInstantiate.transform.rotation = transform.rotation;
        //делаем объект физическим
        shellInstantiate.AddComponent<Rigidbody>();
        //добавляем на него скрипт с его поведением
        // shellInstantiate.AddComponent<FoamShellPrim>();
        //если надо подсвечивать снаряд
        if (isLuminescent) OnLuminescent(shellInstantiate);
    }
    void Teleport (PrimitiveType shell)
    {
        //определяем центр экрана, откуда стрелять
        Vector3 point = new Vector3(cameraPlayer.pixelWidth / 2, cameraPlayer.pixelHeight / 2, 0);
        //пускаем луч оттуда
        Ray ray = cameraPlayer.ScreenPointToRay(point);
        //сюда сохраним инфу о попадании луча
        RaycastHit hit;
        //если луч куда-то попал
        if (Physics.Raycast(ray, out hit))
        {
           //создаем примитив
            GameObject shellInstantiate = GameObject.CreatePrimitive(shell);
            //переносим в нужное место
            shellInstantiate.transform.position = hit.point;
            //если надо подсвечивать снаряд
            if (isLuminescent) OnLuminescent(shellInstantiate); 
        }
    }
    void OnLuminescent (GameObject shellInstantiate)
    {
        // //делаем объект маленьким
        // shellInstantiate.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        //добавляем  свечение
        shellInstantiate.AddComponent<Light>();
    }
}
