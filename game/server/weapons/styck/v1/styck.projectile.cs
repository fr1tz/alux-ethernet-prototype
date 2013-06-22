//------------------------------------------------------------------------------
// Alux Ethernet Prototype
// Copyright notices are in the file named COPYING.
//------------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// projectile datablock...

datablock ShotgunProjectileData(WpnStyckProjectile)
{
	stat = "styck";

	// script damage properties...
	impactDamage       = 17;
	impactImpulse      = 400;
	splashDamage       = 0;
	splashDamageRadius = 0;
	splashImpulse      = 0;

	energyDrain = 20; // how much energy does firing this projectile drain?

	numBullets = 5; // number of shotgun bullets

	range = 500; // shotgun range
	muzzleSpreadRadius = 0.1;
	referenceSpreadRadius = 1.0;
	referenceSpreadDistance = 50;

	explodesNearEnemies	      = false;
	explodesNearEnemiesRadius = 4;
	explodesNearEnemiesMask   = $MissedEnemyMask;

	//sound = Blaster3ProjectileFlybySound;

	projectileShapeName = "share/shapes/alux/projectile1.dts";

	explosion               = WpnStyckProjectileImpact;
	hitEnemyExplosion       = WpnStyckProjectileImpact;
	hitTeammateExplosion    = WpnStyckProjectileImpact;
	//nearEnemyExplosion	= DefaultProjectileNearEnemyExplosion;
	//hitDeflectorExplosion = SeekerDiscBounceEffect;

	//fxLight					= WpnStyckProjectileFxLight;

	missEnemyEffect		 = WpnStyckProjectileMissedEnemyEffect;

	laserTail				 = WpnStyckProjectileLaserTail;
	laserTailLen			 = 10.0;

	//laserTrail[0]			= WpnStyckProjectileLaserTrail;
	//laserTrail[1]			= WpnStyckProjectileLaserTrail;
	//smoothLaserTrail = true;

	//particleEmitter	  = WpnStyckProjectileParticleEmitter;

	muzzleVelocity   = 900;
	velInheritFactor = 0.0;

	isBallistic			= false;
	gravityMod			 = 10.0;

	armingDelay			= 1000*0;
	lifetime				= 3000;
	fadeDelay			  = 5000;

	decals[0] = WpnStyckBulletHoleDecal;

	hasLight    = false;
	lightRadius = 10.0;
	lightColor  = "1.0 0.0 0.0";
};

function WpnStyckProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal,%dist)
{
    Parent::onCollision(%this,%obj,%col,%fade,%pos,%normal,%dist);
}


