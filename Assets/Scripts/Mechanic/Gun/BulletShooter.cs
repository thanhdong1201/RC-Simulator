using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private InputReaderSO inputReader;

    [Header("Bullet Settings")]
    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private float cooldown = 0.5f;

    [Header("ObjectPool")]
    [SerializeField] private GameObject prefab;
    [SerializeField] private int poolSize = 10;
    [SerializeField] private List<GameObject> activeBullets = new List<GameObject>();

    private Queue<GameObject> pool = new Queue<GameObject>();
    private bool canShoot = true;

    private void OnEnable()
    {
        inputReader.InteractEvent += Shoot;
    }
    private void OnDestroy()
    {
        inputReader.InteractEvent -= Shoot;
        StopAllCoroutines();
    }
    private void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    private GameObject GetObject()
    {
        if (pool.Count == 0)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            return obj;
        }

        GameObject pooledObj = pool.Dequeue();
        return pooledObj;
    }

    private void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
        activeBullets.Remove(obj);
    }

    private void Shoot()
    {
        if (!canShoot) return;
        GameObject bulletObj = GetObject();
        bulletObj.transform.position = firePoint.position;
        bulletObj.transform.rotation = firePoint.rotation;
        bulletObj.SetActive(true);
        activeBullets.Add(bulletObj);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        bullet.Fire(firePoint.forward, speed);

        StartCoroutine(ResetCoolDown());
        StartCoroutine(DisableBullet(bulletObj));
    }
    private IEnumerator DisableBullet(GameObject obj)
    {
        yield return new WaitForSeconds(lifeTime);
        ReturnObject(obj);
    }
    private IEnumerator ResetCoolDown()
    {
        canShoot = false;
        yield return new WaitForSeconds(cooldown);
        canShoot = true;
    }
}
