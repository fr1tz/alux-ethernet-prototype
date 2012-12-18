//------------------------------------------------------------------------------
// Revenge Of The Cats: Ethernet
// Copyright (C) 2008, mEthLab Interactive
//------------------------------------------------------------------------------

exec("./raptor.projectile.sfx.cs");
exec("./raptor.projectile.gfx.red.cs");
exec("./raptor.projectile.gfx.blue.cs");

//-----------------------------------------------------------------------------
// projectile datablock...

datablock ShotgunProjectileData(WpnRedRaptorProjectile)
{
	stat = "blaster";

	// script damage properties...
	impactDamage       = 15;
	impactImpulse      = 400;
	splashDamage       = 0;
	splashDamageRadius = 0;
	splashImpulse      = 0;

	energyDrain = 2; // how much energy does firing this projectile drain?

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

	explosion               = WpnRedRaptorProjectileImpact;
	hitEnemyExplosion       = WpnRedRaptorProjectileHit;
	hitTeammateExplosion    = WpnRedRaptorProjectileHit;
	//nearEnemyExplosion	= DefaultProjectileNearEnemyExplosion;
	//hitDeflectorExplosion = SeekerDiscBounceEffect;

	//fxLight					= WpnRedRaptorProjectileFxLight;

	missEnemyEffect		 = WpnRedRaptorProjectileMissedEnemyEffect;

   laserTail				 = WpnRedRaptorProjectileLaserTail;
	laserTailLen			 = 20.0;

	laserTrail[2] = NULL; // WpnRedRaptorProjectileLaserTrail2;
	smoothLaserTrail     = false;

	//particleEmitter	  = WpnRedRaptorProjectileParticleEmitter;

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

function WpnRedRaptorProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal,%dist)
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

//-----------------------------------------------------------------------------

datablock ShotgunProjectileData(BlueWpnRaptorProjectile : WpnRedRaptorProjectile)
{
	//projectileShapeName = "share/shapes/rotc/weapons/blaster/projectile.blue.dts";

	explosion            = BlueWpnRaptorProjectileImpact;
	hitEnemyExplosion    = BlueWpnRaptorProjectileHit;
	hitTeammateExplosion = BlueWpnRaptorProjectileHit;

	missEnemyEffect    = BlueWpnRaptorProjectileMissedEnemyEffect;

	//laserTail          = BlueWpnRaptorProjectileLaserTail;

	laserTrail[0]      = BlueWpnRaptorProjectileLaserTrail;
	laserTrail[1]      = BlueWpnRaptorProjectileLaserTrail;

	lightColor  = "0.0 0.0 1.0";
};

function BlueWpnRaptorProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal,%dist)
{
    WpnRedRaptorProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal,%dist);
}
