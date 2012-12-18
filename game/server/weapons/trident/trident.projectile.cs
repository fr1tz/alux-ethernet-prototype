//------------------------------------------------------------------------------
// Revenge Of The Cats: Ethernet
// Copyright (C) 2008, mEthLab Interactive
//------------------------------------------------------------------------------

exec("./trident.projectile.sfx.cs");
exec("./trident.projectile.gfx.red.cs");
exec("./trident.projectile.gfx.blue.cs");

//-----------------------------------------------------------------------------
// projectile datablock...

datablock ShotgunProjectileData(WpnRedTridentProjectile)
{
	stat = "blaster";

	// script damage properties...
	impactDamage       = 12;
	impactImpulse      = 400;
	splashDamage       = 0;
	splashDamageRadius = 0;
	splashImpulse      = 0;

	energyDrain = 3; // how much energy does firing this projectile drain?

	numBullets = 9; // number of shotgun bullets

	range = 500; // shotgun range
	muzzleSpreadRadius = 0.1;
	referenceSpreadRadius = 1.0;
	referenceSpreadDistance = 10;

	explodesNearEnemies	      = false;
	explodesNearEnemiesRadius = 4;
	explodesNearEnemiesMask   = $TypeMasks::PlayerObjectType;

	//sound = Blaster3ProjectileFlybySound;

	//projectileShapeName = "share/shapes/rotc/weapons/blaster/projectile.red.dts";

	explosion               = WpnRedTridentProjectileImpact;
	hitEnemyExplosion       = WpnRedTridentProjectileHit;
	hitTeammateExplosion    = WpnRedTridentProjectileHit;
	//nearEnemyExplosion	= DefaultProjectileNearEnemyExplosion;
	//hitDeflectorExplosion = SeekerDiscBounceEffect;

	//fxLight					= WpnRedTridentProjectileFxLight;

	missEnemyEffect		 = WpnRedTridentProjectileMissedEnemyEffect;

	//laserTail				 = WpnRedTridentProjectileLaserTail;
	//laserTailLen			 = 10.0;

	laserTrail[0]			= WpnRedTridentProjectileLaserTrail;
	laserTrail[1]			= WpnRedTridentProjectileLaserTrail;
	smoothLaserTrail = true;

	//particleEmitter	  = WpnRedTridentProjectileParticleEmitter;

	muzzleVelocity   = 9999;
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

function WpnRedTridentProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal,%dist)
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
        %col.numBlaster3BulletHits += 1;
        if(%col.numBlaster3BulletHits == 4)
            %src.setDiscTarget(%col);
    }
    else
    {
        %obj.hitTime = %currTime;
        %col.numBlaster3BulletHits = 1;
    }
}

//-----------------------------------------------------------------------------

datablock ShotgunProjectileData(WpnBlueTridentProjectile : WpnRedTridentProjectile)
{
	//projectileShapeName = "share/shapes/rotc/weapons/blaster/projectile.blue.dts";

	explosion            = WpnBlueTridentProjectileImpact;
	hitEnemyExplosion    = WpnBlueTridentProjectileHit;
	hitTeammateExplosion = WpnBlueTridentProjectileHit;

	missEnemyEffect    = WpnBlueTridentProjectileMissedEnemyEffect;

	//laserTail          = WpnBlueTridentProjectileLaserTail;

	laserTrail[0]      = WpnBlueTridentProjectileLaserTrail;
	laserTrail[1]      = WpnBlueTridentProjectileLaserTrail;

	lightColor  = "0.0 0.0 1.0";
};

function WpnBlueTridentProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal,%dist)
{
    WpnRedTridentProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal,%dist);
}
