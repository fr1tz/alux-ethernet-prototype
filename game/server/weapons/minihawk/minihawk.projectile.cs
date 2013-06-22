//------------------------------------------------------------------------------
// Alux Ethernet Prototype
// Copyright notices are in the file named COPYING.
//------------------------------------------------------------------------------

exec("./minihawk.projectile.sfx.cs");
exec("./minihawk.projectile.gfx.cs");

//-----------------------------------------------------------------------------
// projectile datablock...

datablock ShotgunProjectileData(WpnMinihawkProjectile)
{
	stat = "minihawk";

	// script damage properties...
	impactDamage       = 45;
	impactImpulse      = 1000;
	splashDamage       = 0;
	splashDamageRadius = 0;
	splashImpulse      = 0;

	energyDrain = 100; // how much energy does firing this projectile drain?

	numBullets = 1; // number of shotgun bullets

	range = 500; // shotgun range
	muzzleSpreadRadius = 0.0;
	referenceSpreadRadius = 0.0;
	referenceSpreadDistance = 50;

	explodesNearEnemies	      = false;
	explodesNearEnemiesRadius = 4;
	explodesNearEnemiesMask   = $MissedEnemyMask;

	//sound = WpnRaptorProjectileFlybySound;

	//projectileShapeName = "share/shapes/rotc/weapons/blaster/projectile.red.dts";

	explosion               = WpnMinihawkProjectileImpact;
	hitEnemyExplosion       = WpnMinihawkProjectileImpact;
	hitTeammateExplosion    = WpnMinihawkProjectileImpact;
	//nearEnemyExplosion	= DefaultProjectileNearEnemyExplosion;
	//hitDeflectorExplosion = SeekerDiscBounceEffect;

	//fxLight					= WpnMinihawkProjectileFxLight;

	missEnemyEffect		 = WpnMinihawkProjectileMissedEnemyEffect;

   laserTail				 = WpnMinihawkProjectileLaserTail;
	laserTailLen			 = 20.0;

	//laserTrail[0]        = WpnMinihawkProjectileLaserTrail;
	//laserTrail[1]        = WpnMinihawkProjectileLaserTrail;
	smoothLaserTrail     = false;

	//particleEmitter	  = WpnMinihawkProjectileParticleEmitter;

	muzzleVelocity   = 900; //900;
	velInheritFactor = 0.0;

	isBallistic			= false;
	gravityMod			 = 10.0;

	armingDelay			= 1000*0;
	lifetime				= 3000;
	fadeDelay			  = 5000;

	decals[0] = WpnMinihawkBulletHoleDecal;

	hasLight    = false;
	lightRadius = 10.0;
	lightColor  = "1.0 0.0 0.0";
};

function WpnMinihawkProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal,%dist)
{
    Parent::onCollision(%this,%obj,%col,%fade,%pos,%normal,%dist);
}


