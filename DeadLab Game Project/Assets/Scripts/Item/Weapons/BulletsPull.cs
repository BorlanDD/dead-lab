using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsPull : MonoBehaviour
{

    public IList<Bullet> bullets;
	public GameObject bulletPrefab;
	public int pullSize;

	private static BulletsPull bulletsPull;
	public static BulletsPull GetInstnace()
	{
		return bulletsPull;
	}

    public void Awake()
    {
		bulletsPull = this;
		pullSize = 100;

		bullets = new List<Bullet>();
		Bullet bullet;
		for (int i = 0; i < pullSize; i++)
		{
			bullet = Instantiate(bulletPrefab, transform).GetComponent<Bullet>();
			bullet.gameObject.SetActive(false);
			bullets.Add(bullet);
		}
    }

	public void ReturnBullet(Bullet bullet)
	{
		bullet.gameObject.SetActive(false);
		bullet.damage = 0;
		bullets.Add(bullet);
	}

	public Bullet GetBullet()
	{
		Bullet bullet = null;
		if (bullets.Count > 0)
		{
			bullet = bullets[0];
			bullets.Remove(bullet);
			return bullet;
		}
		return bullet;
	}
}
