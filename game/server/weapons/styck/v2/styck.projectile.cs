//------------------------------------------------------------------------------
// Alux Prototype
// Copyright notices are in the file named COPYING.
//------------------------------------------------------------------------------

datablock TracerProjectileData(WpnStyckPseudoProjectile)
{
	energyDrain = 100;
	lifetime = 1000;
	muzzleVelocity = 250 * $Server::Game.slowpokemod;
	velInheritFactor = 0.5 * $Server::Game.slowpokemod;
};

function WpnStyckPseudoProjectile::onAdd(%this, %obj)
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

	for(%i = 0; %i < 9; %i++)
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

datablock ProjectileData(WpnStyckProjectile)
{
	stat = "styck";

	// script damage properties...
	impactDamage       = 10;
	impactImpulse      = 400;
	splashDamage       = 0;
	splashDamageRadius = 0;
	splashImpulse      = 0;

	explodesNearEnemies	      = false;
	explodesNearEnemiesRadius = 4;
	explodesNearEnemiesMask   = $MissedEnemyMask;

	//sound = Blaster3ProjectileFlybySound;

	projectileShapeName = "share/shapes/alux/projectile1.dts";

	explosion               = WpnStyckProjectileImpact;
	hitEnemyExplosion       = WpnStyckProjectileImpact;
	hitTeammateExplosion    = WpnStyckProjectileImpact;
	//nearEnemyExplosion	= DefaultProjectileNearEnemyExplosion;
	//hitDeflectorExplosion = SeekerDiscBounceEffect;

	//fxLight					= WpnStyckProjectileFxLight;

	missEnemyEffect		 = WpnStyckProjectileMissedEnemyEffect;

	laserTail				 = WpnStyckProjectileLaserTail;
	laserTailLen			 = 3.0;

	laserTrail[0]			= WpnStyckProjectileLaserTrail;
	//laserTrail[1]			= WpnStyckProjectileLaserTrail;
	//smoothLaserTrail = true;

	//particleEmitter	  = WpnStyckProjectileParticleEmitter;

	muzzleVelocity	= 0; // Handled by pseudo projectile
	velInheritFactor = 0; // Handled by pseudo projectile

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


