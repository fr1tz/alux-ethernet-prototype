//------------------------------------------------------------------------------
// Alux Ethernet Prototype
// Copyright notices are in the file named COPYING.
//------------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// projectile datablock...

datablock ShotgunProjectileData(WpnRaptorProjectile)
{
	stat = "raptor";

	// script damage properties...
	impactDamage       = 15;
	impactImpulse      = 600;
	splashDamage       = 0;
	splashDamageRadius = 0;
	splashImpulse      = 0;

	energyDrain = 25; // how much energy does firing this projectile drain?

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

	explosion               = WpnRaptorProjectileImpact;
	hitEnemyExplosion       = DefaultProjectileHit;
	hitTeammateExplosion    = DefaultProjectileHit;
	//nearEnemyExplosion	= DefaultProjectileNearEnemyExplosion;
	//hitDeflectorExplosion = SeekerDiscBounceEffect;

	//fxLight					= WpnRaptorProjectileFxLight;

	missEnemyEffect		 = WpnRaptorProjectileMissedEnemyEffect;

   laserTail				 = WpnRaptorProjectileLaserTail;
	laserTailLen			 = 15.0;

	laserTrail[2] = NULL; // WpnRaptorProjectileLaserTrail2;
	smoothLaserTrail     = false;

	//particleEmitter	  = WpnRaptorProjectileParticleEmitter;

	muzzleVelocity   = 900; //900;
	velInheritFactor = 0.0;

	isBallistic			= false;
	gravityMod			 = 10.0;

	armingDelay			= 1000*0;
	lifetime				= 3000;
	fadeDelay			  = 5000;

	decals[0] = WpnRaptorBulletHoleDecal;

	hasLight    = false;
	lightRadius = 10.0;
	lightColor  = "1.0 0.0 0.0";
};

function WpnRaptorProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal,%dist)
{
    Parent::onCollision(%this,%obj,%col,%fade,%pos,%normal,%dist);

	if( !(%col.getType() & $TypeMasks::ShapeBaseObjectType) )
		return;

    %src =  %obj.getSourceObject();
    if(!%src)
        return;

    %currTime = getSimTime();

	// FIXME: strange linux version bug:
	//        after the game has been running a long time
	//        (%currTime == %obj.hitTime)
	//        often evaluates to false even if the
	//        values appear to be equal.
	//        (%currTime - %obj.hitTime) evaluates to 1
	//        in those cases.
    //if(%currTime == %obj.hitTime)
	if(%currTime - %obj.hitTime <= 1)
    {
        %col.numWpnRaptorBulletHits += 1;
        if(%col.numWpnRaptorBulletHits == 4)
            %src.setDiscTarget(%col);
    }
    else
    {
        %obj.hitTime = %currTime;
        %col.numWpnRaptorBulletHits = 1;
    }
}


