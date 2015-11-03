﻿using UnityEngine;
using System.Collections;

public class ProjectileControl : MonoBehaviour 
{
	public int type; // 0 for enemy projectile, 1 for player
	public float travelSpeed = 1f;
	public float timeAfterColl = 0f;
	public int damage = 1;
	public GameObject explosionPrefab;
    public SoundManager audioManager;
    public AudioClip explosionSound;

//	private GameObject tempObject;
	private Rigidbody2D thisRigidbody;
	private Vector2 travelVelocity;

	private float collisionTime = 0f;
	private bool exploded = false;
	private SpriteRenderer thisRenderer;

	// Use this for initialization
	void Start () 
	{
		thisRigidbody = this.GetComponent<Rigidbody2D>();
		thisRenderer = this.GetComponent<SpriteRenderer>();
		RefreshVelocity();
	}

	void FixedUpdate () 
	{
		if(!exploded)
		{
			thisRigidbody.velocity = travelVelocity;
		}
		else
		{
			thisRigidbody.velocity = Vector2.zero;
			if(Time.time > (collisionTime+timeAfterColl))
			{
				this.gameObject.SetActive(false);
			}
		}
	}

	public void RefreshVelocity()
	{
		exploded = false;
		travelVelocity = new Vector2(-Mathf.Sin(Mathf.Deg2Rad*transform.rotation.eulerAngles.z)*travelSpeed, Mathf.Cos(Mathf.Deg2Rad*transform.rotation.eulerAngles.z)*travelSpeed);
	}


	void OnTriggerEnter2D(Collider2D coll)
	{
		//Debug.Log("Hit " + coll.gameObject.tag);
		switch(coll.gameObject.tag)
		{
		case "Wall":
			exploded = true;
			if (explosionSound != null)
			{
				audioManager.playModulatedSound(explosionSound, .5f);
			}
			this.GetComponent<Collider2D>().enabled = false;
			if(thisRenderer!=null)
			{
				thisRenderer.enabled = false;
			}
			collisionTime = Time.time;
			OnHitEffects(coll.gameObject);
			break;
		case "Enemy":
			if(type==1)
			{
				exploded = true;
	            if (explosionSound != null)
	            {
	                audioManager.playModulatedSound(explosionSound, .5f);
	            }
				this.GetComponent<Collider2D>().enabled = false;
				if(thisRenderer!=null)
				{
					thisRenderer.enabled = false;
				}
				collisionTime = Time.time;
				OnHitEffects(coll.gameObject);
			}
			break;
		case "Player":
			if(type==0)
			{
				exploded = true;
				if (explosionSound != null)
				{
					audioManager.playModulatedSound(explosionSound, .5f);
				}
				this.GetComponent<Collider2D>().enabled = false;
				if(thisRenderer!=null)
				{
					thisRenderer.enabled = false;
				}
				collisionTime = Time.time;
				OnHitEffects(coll.gameObject);
			}
			break;
		}
	}


	void OnHitEffects(GameObject other)
	{
		if((type==1) && (other.tag == "Enemy"))
		{
			if(other.GetComponent<BomberControl>()!=null)
			{
				other.GetComponent<BomberControl>().DestroyBomber();
			}
			else if(other.GetComponent<BasicEnemyControl>()!=null)
			{
				other.GetComponent<BasicEnemyControl>().TakeDamage(damage);
			}

		}
		if((type==0) && (other.tag == "Player"))
		{
			other.GetComponent<PlayerControl>().TakeDamage(damage);
		}

		// Spawn explosion particles
		Instantiate(explosionPrefab, transform.position, Quaternion.identity);

	}

}
