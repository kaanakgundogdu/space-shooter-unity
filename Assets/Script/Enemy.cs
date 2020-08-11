using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[Header("Enemy Stats")]
	[SerializeField] float health = 100;
	[SerializeField] int scoreValue = 150;

	[Header("Shooting")]
	float shotCounter;
	[SerializeField] float minTimeBetweenShots = 0.2f;
	[SerializeField] float maxTimeBetweenShots = 3f;
	[SerializeField] GameObject projectile;
	[SerializeField] float projectileSpeed = 10f;
	[Header("Sound Effects")]
	[SerializeField] GameObject deathVFX;
	[SerializeField] float durationOfExplosion = 1f;
	[SerializeField] AudioClip deathSound;
	[SerializeField] [Range(0, 1)] float deathSoundVolume = 0.75f;
	[SerializeField] AudioClip shootSound;
	[SerializeField] [Range(0, 1)] float shootSoundVolume = 0.25f;



	// Use this for initialization
	void Start()
	{
		shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
	}

	// Update is called once per frame
	void Update()
	{
		CountDownAndShoot();
	}
	private void CountDownAndShoot()
	{
		shotCounter -= Time.deltaTime; // Ateş etmeden önce sayıyoruz ve her saniye değeri çıkarıoruz sıfır olduğunda ateş etmeye başlayacak
		if (shotCounter <= 0f)
		{
			Fire();
			shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);//Ateş ettikten sonra bir daha yeni değer atıyoruz
		}
	}

	private void Fire()
	{
		GameObject laser = Instantiate( projectile, transform.position, Quaternion.identity ) as GameObject;

		laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
		AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
	}



	private void OnTriggerEnter2D(Collider2D other)
	{
		DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
		if (!damageDealer) { return; }
		HitTheEnemy(damageDealer);
	}

	private void HitTheEnemy(DamageDealer damageDealer)
	{
		health -= damageDealer.GetDamage();
		damageDealer.Hit();
		if (health <= 0)
		{
			Die();
		}
	}

	private void Die()
	{
		FindObjectOfType<GameSession>().AddToScore(scoreValue);
		Destroy(gameObject);
		GameObject explosion = Instantiate(deathVFX, transform.position, transform.rotation);
		Destroy(explosion, durationOfExplosion);
		AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
	}

}