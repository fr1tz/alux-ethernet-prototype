//------------------------------------------------------------------------------
// Alux
// Copyright (C) 2013 Michael Goldener <mg@wasted.ch>
//------------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// projectile datablock...

datablock ShotgunProjectileData(WpnBadgerProjectile)
{
	stat = "badger";

	// script damage properties...
	impactDamage       = 20;
	impactImpulse      = 200;
	splashDamage       = 0;
	splashDamageRadius = 0;
	splashImpulse      = 0;

	energyDrain = 50; // how much energy does firing this projectile drain?

	numBullets = 1; // number of shotgun bullets

	range = 500; // shotgun range
	muzzleSpreadRadius = 0.0;
	referenceSpreadRadius = 0.0;
	referenceSpreadDistance = 50;

	explodesNearEnemies	      = false;
	explodesNearEnemiesRadius = 4;
	explodesNearEnemiesMask   = $TypeMasks::PlayerObjectType;

	//sound = WpnRaptorProjectileFlybySound;

	//projectileShapeName = "share/shapes/rotc/weapons/blaster/projectile.red.dts";

	explosion               = WpnBadgerProjectileImpact;
	hitEnemyExplosion       = WpnBadgerProjectileImpact;
	hitTeammateExplosion    = WpnBadgerProjectileImpact;
	//nearEnemyExplosion	= DefaultProjectileNearEnemyExplosion;
	//hitDeflectorExplosion = SeekerDiscBounceEffect;

	//fxLight					= WpnBadgerProjectileFxLight;

	missEnemyEffect		 = WpnBadgerProjectileMissedEnemyEffect;

   laserTail				 = WpnBadgerProjectileLaserTail;
	laserTailLen			 = 20.0;

	laserTrail[2] = NULL; // WpnBadgerProjectileLaserTrail2;
	smoothLaserTrail     = false;

	//particleEmitter	  = WpnBadgerProjectileParticleEmitter;

	muzzleVelocity   = 900; //900;
	velInheritFactor = 0.0;

	isBallistic			= false;
	gravityMod			 = 10.0;

	armingDelay			= 1000*0;
	lifetime				= 3000;
	fadeDelay			  = 5000;

	decals[0] = BulletHoleDecalOne;

	hasLight    = false;
	lightRadius = 10.0;
	lightColor  = "1.0 0.0 0.0";
};

function WpnBadgerProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal,%dist)
{
    Parent::onCollision(%this,%obj,%col,%fade,%pos,%normal,%dist);
}


