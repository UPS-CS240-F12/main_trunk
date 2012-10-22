using UnityEngine;
using System.Collections;

public abstract class Powerup : AutoDestroyable
{
	// Called if we collide with something else
	void OnTriggerEnter(Collider collider)
	{
        if (collider.CompareTag("Player") == true)
            OnPowerupReceived(collider);
		
		Destroy(gameObject);
	}

    // Implement this in a derived class.
    protected abstract void OnPowerupReceived(Collider player);
}
