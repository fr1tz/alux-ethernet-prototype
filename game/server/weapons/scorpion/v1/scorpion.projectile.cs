//------------------------------------------------------------------------------
// Alux Prototype
// Copyright notices are in the file named COPYING.
//------------------------------------------------------------------------------

datablock TracerProjectileData(WpnScorpionPseudoProjectile)
{
	energyDrain = 200;
	lifetime = 1000;
	muzzleVelocity = 600 * $Server::Game.slowpokemod;
	velInheritFactor = 0.0 * $Server::Game.slowpokemod;
};

function WpnScorpionPseudoProjectile::onAdd(%this, %obj)
{
	%player = %obj.sourceObject;
	%slot = %obj.sourceSlot;
	%image = %player.getMountedImage(%slot);

	%muzzlePoint = %obj.initialPosition;
	%muzzleVector = %obj.initialVelocity;
	%muzzleTransform = createOrientFromDir(VectorNormalize(%muzzleVector));

	%spread = 0;
	%randX = %spread * ((getRandom(1000)-500)/1000);
	%randZ = %spread * ((getRandom(1000)-500)/1000);

   %pos[0] = "0 0 0";
   %vec[0] = %randX SPC "1" SPC %randZ;

	%position =	VectorAdd(
		%muzzlePoint,
		MatrixMulVector(%muzzleTransform, %pos[0])
	);
	%muzzleVec = MatrixMulVector(%muzzleTransform, %vec[0]);

   %projectile = %image.fireprojectile[0];

	for(%i = 0; %i < 1; %i++)
	{
      %spread = %image.getBulletSpread(%player);
      %randX = %spread * ((getRandom(1000)-500)/1000);
      %randZ = %spread * ((getRandom(1000)-500)/1000);
      %vec = %randX SPC "1" SPC %randZ;
      %mat = createOrientFromDir(VectorNormalize(%muzzleVec));
      %vel = VectorScale(MatrixMulVector(%mat, %vec), %this.muzzleVelocity);

		// create the projectile object...
		%p = new Projectile() {
			dataBlock       = %projectile;
			teamId          = %obj.teamId;
			initialVelocity = %vel;
			initialPosition = %position;
			sourceObject    = %player;
			sourceSlot      = %slot;
			client          = %player.client;
		};
		MissionCleanup.add(%p);
	}

	// no need to ghost pseudo projectile to clients...
	%obj.delete();
}


//-----------------------------------------------------------------------------
// projectile datablock...

datablock ProjectileData(WpnScorpionProjectile)
{
	stat = "scorpion";

	// script damage properties...
	impactDamage       = 75;
	impactImpulse      = 1000;
	splashDamage       = 0;
	splashDamageRadius = 0;
	splashImpulse      = 0;

	explodesNearEnemies	     = true;
	explodesNearEnemiesRadius = 2;
	explodesNearEnemiesMask   = $TypeMasks::PlayerObjectType |
                               $TypeMasks::VehicleObjectType;

	//sound = Blaster3ProjectileFlybySound;

	//projectileShapeName = "share/shapes/alux/projectile1.dts";

	explosion               = WpnScorpionProjectileExplosion;
	hitEnemyExplosion       = WpnScorpionProjectileImpact;
	hitTeammateExplosion    = WpnScorpionProjectileImpact;
	nearEnemyExplosion	   = WpnScorpionProjectileExplosion;
	//hitDeflectorExplosion = SeekerDiscBounceEffect;

	//fxLight					= WpnScorpionProjectileFxLight;

	missEnemyEffect		 = WpnScorpionProjectileMissedEnemyEffect;

	laserTail				 = WpnScorpionProjectileLaserTail;
	laserTailLen			 = 20.0;

	//laserTrail[0]			= WpnScorpionProjectileLaserTrail;
	//laserTrail[1]			= WpnScorpionProjectileLaserTrail;
	//smoothLaserTrail = true;

	//particleEmitter	  = WpnScorpionProjectileParticleEmitter;

	muzzleVelocity	= 0; // Handled by pseudo projectile
	velInheritFactor = 0; // Handled by pseudo projectile

	isBallistic			= true;
	gravityMod			 = 1.0;

	armingDelay			= 1000*0;
	lifetime				= 3000;
	fadeDelay			  = 5000;

	decals[0] = WpnScorpionBulletHoleDecal;

	hasLight    = false;
	lightRadius = 10.0;
	lightColor  = "1.0 0.0 0.0";
};

function WpnScorpionProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal,%dist)
{
    Parent::onCollision(%this,%obj,%col,%fade,%pos,%normal,%dist);
}


