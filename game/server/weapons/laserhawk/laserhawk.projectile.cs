//------------------------------------------------------------------------------
// Alux
// Copyright (C) 2013 Michael Goldener <mg@wasted.ch>
//------------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// projectile datablock...

datablock ShotgunProjectileData(WpnLaserhawkProjectile)
{
	stat = "sniper";

	// script damage properties...
	impactDamage       = 100;
	impactImpulse      = 1500;
	splashDamage       = 0;
	splashDamageRadius = 0;
	splashImpulse      = 0;
	bypassDamageBuffer = alsef;

	energyDrain = 30; // how much energy does firing this projectile drain?

	numBullets = 1; // number of shotgun bullets
	range = 2000; // shotgun range
	muzzleSpreadRadius = 0.0;
	referenceSpreadRadius = 0.0;
	referenceSpreadDistance = 50.0;

	explodesNearEnemies	      = false;
	explodesNearEnemiesRadius = 4;
	explodesNearEnemiesMask   = $TypeMasks::PlayerObjectType;

    //sound = SniperProjectileFlybySound;

    //projectileShapeName = "share/shapes/rotc/weapons/blaster/projectile.red.dts";

	explosion               = WpnLaserhawkProjectileExplosion;
	hitEnemyExplosion       = WpnLaserhawkProjectileHit;
    hitTeammateExplosion    = WpnLaserhawkProjectileHit;
    //nearEnemyExplosion	= DefaultProjectileNearEnemyExplosion;
    //hitDeflectorExplosion = SeekerDiscBounceEffect;

    //fxLight					= WpnLaserhawkProjectileFxLight;

	missEnemyEffect		 = WpnLaserhawkProjectileMissedEnemyEffect;

	fireExplosion = WpnLaserhawkProjectileFireExplosion; // script field

    //laserTail	 = WpnLaserhawkProjectileLaserTail;
    //laserTailLen = 10.0;

	laserTrail[0] = WpnLaserhawkProjectileLaserTrailHit;
	laserTrail[1] = WpnLaserhawkProjectileLaserTrailHit;
	laserTrail[2] = WpnLaserhawkProjectileLaserTrailMissed;

	//particleEmitter = WpnLaserhawkProjectileEmitter;

	muzzleVelocity   = 9999;
	velInheritFactor = 0.0;

	isBallistic			= false;
	gravityMod			 = 10.0;

	armingDelay			= 1000*0;
	lifetime				= 5000;
	fadeDelay			  = 5000;

	decals[0] = BulletHoleDecalOne;

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


