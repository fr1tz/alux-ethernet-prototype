//------------------------------------------------------------------------------
// Alux Prototype
// Copyright notices are in the file named COPYING.
//------------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// projectile datablock...

datablock ShotgunProjectileData(WpnLaserhawkProjectile)
{
	stat = "laserhawk";

	// script damage properties...
	impactDamage       = 100;
	impactImpulse      = 1500;
	splashDamage       = 0;
	splashDamageRadius = 0;
	splashImpulse      = 0;
	bypassDamageBuffer = alsef;

	energyDrain = 200; // how much energy does firing this projectile drain?

	numBullets = 1; // number of shotgun bullets
	range = 2000; // shotgun range
	muzzleSpreadRadius = 0.0;
	referenceSpreadRadius = 0.0;
	referenceSpreadDistance = 50.0;

	explodesNearEnemies	      = false;
	explodesNearEnemiesRadius = 4;
	explodesNearEnemiesMask   = $MissedEnemyMask;

    //sound = SniperProjectileFlybySound;

    //projectileShapeName = "share/shapes/rotc/weapons/blaster/projectile.red.dts";

   explosion               = WpnLaserhawkProjectileExplosion;
   hitEnemyExplosion       = DefaultProjectileHit;
   hitTeammateExplosion    = DefaultProjectileHit;
    //nearEnemyExplosion	= DefaultProjectileNearEnemyExplosion;
    //hitDeflectorExplosion = SeekerDiscBounceEffect;

    //fxLight					= WpnLaserhawkProjectileFxLight;

	missEnemyEffect		 = WpnLaserhawkProjectileMissedEnemyEffect;

	fireExplosion = WpnLaserhawkProjectileFireExplosion; // script field

    //laserTail	 = WpnLaserhawkProjectileLaserTail;
    //laserTailLen = 10.0;

   laserTrail[0]        = WpnLaserhawkProjectileLaserTrailOne;
   laserTrailFlags[0]   = 4;
	laserTrail[1]        = WpnLaserhawkProjectileLaserTrailTwo;
   laserTrailFlags[1]   = 4;
	laserTrail[2]        = WpnLaserhawkProjectileLaserTrailThree;
   laserTrailFlags[2]   = 4;

	//particleEmitter = WpnLaserhawkProjectileEmitter;

	muzzleVelocity   = 9999;
	velInheritFactor = 0.0;

	isBallistic			= false;
	gravityMod			 = 10.0;

	armingDelay			= 1000*0;
	lifetime				= 5000;
	fadeDelay			  = 5000;

	decals[0] = WpnLaserhawkBulletHoleDecal;

	hasLight    = false;
	lightRadius = 10.0;
	lightColor  = "0.0 1.0 0.0";
};

function WpnLaserhawkProjectile::onAdd(%this, %obj)
{
	Parent::onAdd(%this, %obj);
	%vel = %obj.initialVelocity;
	%pos = %obj.initialPosition;
	%pos = VectorAdd(VectorScale(VectorNormalize(%vel),4), %pos);
	%norm = "0 0 1";
	createExplosion(%this.fireExplosion, %pos, %norm);
}

function WpnLaserhawkProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal,%dist)
{
    Parent::onCollision(%this,%obj,%col,%fade,%pos,%normal,%dist);
    
	if( !(%col.getType() & $TypeMasks::ShapeBaseObjectType) )
		return;

    %src =  %obj.getSourceObject();
    if(!%src)
        return;
        
    %src.sniperTarget = %col;
}


