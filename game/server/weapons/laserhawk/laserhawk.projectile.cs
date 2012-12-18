//------------------------------------------------------------------------------
// Revenge Of The Cats: Ethernet
// Copyright (C) 2008, mEthLab Interactive
//------------------------------------------------------------------------------

exec("./laserhawk.projectile.gfx.red.cs");
exec("./laserhawk.projectile.gfx.blue.cs");

//-----------------------------------------------------------------------------
// projectile datablock...

datablock ShotgunProjectileData(WpnRedLaserhawkProjectile)
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

	explosion               = WpnRedLaserhawkProjectileExplosion;
	hitEnemyExplosion       = WpnRedLaserhawkProjectileHit;
    hitTeammateExplosion    = WpnRedLaserhawkProjectileHit;
    //nearEnemyExplosion	= DefaultProjectileNearEnemyExplosion;
    //hitDeflectorExplosion = SeekerDiscBounceEffect;

    //fxLight					= WpnRedLaserhawkProjectileFxLight;

	missEnemyEffect		 = WpnRedLaserhawkProjectileMissedEnemyEffect;

	fireExplosion = WpnRedLaserhawkProjectileFireExplosion; // script field

    //laserTail	 = WpnRedLaserhawkProjectileLaserTail;
    //laserTailLen = 10.0;

	laserTrail[0] = WpnRedLaserhawkProjectileLaserTrailHit;
	laserTrail[1] = WpnRedLaserhawkProjectileLaserTrailHit;
	laserTrail[2] = WpnRedLaserhawkProjectileLaserTrailMissed;

	//particleEmitter = WpnRedLaserhawkProjectileEmitter;

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

function WpnRedLaserhawkProjectile::onAdd(%this, %obj)
{
	Parent::onAdd(%this, %obj);
	%vel = %obj.initialVelocity;
	%pos = %obj.initialPosition;
	%pos = VectorAdd(VectorScale(VectorNormalize(%vel),4), %pos);
	%norm = "0 0 1";
	createExplosion(%this.fireExplosion, %pos, %norm);
}

function WpnRedLaserhawkProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal,%dist)
{
    Parent::onCollision(%this,%obj,%col,%fade,%pos,%normal,%dist);
    
	if( !(%col.getType() & $TypeMasks::ShapeBaseObjectType) )
		return;

    %src =  %obj.getSourceObject();
    if(!%src)
        return;
        
    %src.sniperTarget = %col;
}

//-----------------------------------------------------------------------------

datablock ShotgunProjectileData(WpnBlueLaserhawkProjectile : WpnRedLaserhawkProjectile)
{
    //projectileShapeName = "share/shapes/rotc/weapons/blaster/projectile.blue.dts";

	explosion               = WpnBlueLaserhawkProjectileExplosion;
	hitEnemyExplosion       = WpnBlueLaserhawkProjectileHit;
    hitTeammateExplosion    = WpnBlueLaserhawkProjectileHit;

	missEnemyEffect    = WpnBlueLaserhawkProjectileMissedEnemyEffect;

	//laserTail          = WpnBlueLaserhawkProjectileLaserTail;

	fireExplosion = WpnBlueLaserhawkProjectileFireExplosion; // script field

	laserTrail[0] = WpnBlueLaserhawkProjectileLaserTrailHit;
	laserTrail[1] = WpnBlueLaserhawkProjectileLaserTrailHit;
	laserTrail[2] = WpnBlueLaserhawkProjectileLaserTrail;

	lightColor  = "1.0 0.5 0.0";
};

function WpnBlueLaserhawkProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal,%dist)
{
    WpnRedLaserhawkProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal,%dist);
}

function WpnBlueLaserhawkProjectile::onAdd(%this, %obj)
{
	WpnRedLaserhawkProjectile::onAdd(%this, %obj);
}
