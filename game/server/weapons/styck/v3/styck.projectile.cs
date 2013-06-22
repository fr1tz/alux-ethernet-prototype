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
	impactDamage       = 9;
	impactImpulse      = 80;
	splashDamage       = 0;
	splashDamageRadius = 0;
	splashImpulse      = 0;

	energyDrain = 10; // how much energy does firing this projectile drain?

	numBullets = 10; // number of shotgun bullets

	range = 500; // shotgun range
	muzzleSpreadRadius = 0.1;
	referenceSpreadRadius = 1.0;
	referenceSpreadDistance = 30;

	explodesNearEnemies	      = false;
	explodesNearEnemiesRadius = 4;
	explodesNearEnemiesMask   = $MissedEnemyMask;

	//sound = Blaster3ProjectileFlybySound;

	projectileShapeName = "share/shapes/alux/projectile1.dts";

	explosion               = WpnStyckProjectileImpact;
	hitEnemyExplosion       = DefaultProjectileHit;
	hitTeammateExplosion    = DefaultProjectileHit;
	//nearEnemyExplosion	= DefaultProjectileNearEnemyExplosion;
	//hitDeflectorExplosion = SeekerDiscBounceEffect;

	//fxLight					= WpnStyckProjectileFxLight;

	missEnemyEffect		 = WpnStyckProjectileMissedEnemyEffect;

	//laserTail				 = WpnStyckProjectileLaserTail;
	//laserTailLen			 = 10.0;

	laserTrail[0]        = WpnStyckProjectileLaserTrailOne;
   laserTrailFlags[0]   = 4;
   laserTrail[1]        = WpnStyckProjectileLaserTrailTwo;
   laserTrailFlags[1]   = 4;
	laserTrail[2]        = WpnStyckProjectileLaserTrailThree;
   laserTrailFlags[2]   = 4;

	//particleEmitter	  = WpnStyckProjectileParticleEmitter;

	muzzleVelocity   = 9999;
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


