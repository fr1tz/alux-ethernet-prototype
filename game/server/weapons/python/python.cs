//------------------------------------------------------------------------------
// Alux
// Copyright (C) 2013 Michael Goldener <mg@wasted.ch>
//------------------------------------------------------------------------------

exec("./python.sfx.cs");
exec("./python.gfx.red.cs");
exec("./python.gfx.blue.cs");

//-----------------------------------------------------------------------------
// projectile datablock...

datablock ShotgunProjectileData(WpnRedPythonProjectile)
{
	// script damage properties...
	impactDamage       = 6;
	impactImpulse      = 100;
	splashDamage       = 00;
	splashDamageRadius = 0;
	splashImpulse      = 0;
	bypassDamageBuffer = false;
	
	// how much energy does firing this projectile drain?...
	energyDrain = 2;

	numBullets = 9; // number of shotgun bullets

	range = 500; // shotgun range
	muzzleSpreadRadius = 1.0;
	referenceSpreadRadius = 3.0;
	referenceSpreadDistance = 80;

	trackingAgility = 0;
	
	explodesNearEnemies			= false;
	explodesNearEnemiesRadius	= 5;
	explodesNearEnemiesMask	  = $TypeMasks::PlayerObjectType;

	//sound = AssaultRifleProjectileFlybySound;
 
//  projectileShapeName = "share/shapes/rotc/weapons/blaster/projectile.red.dts";

	explosion             = WpnRedPythonProjectileExplosion;
//	bounceExplosion		  = WpnRedPythonProjectileBounceExplosion;
//	hitEnemyExplosion     = AssaultRifleProjectileImpact;
//	nearEnemyExplosion    = AssaultRifleProjectileExplosion;
//	hitTeammateExplosion  = AssaultRifleProjectileImpact;
//	hitDeflectorExplosion = DiscDeflectedEffect;

//   particleEmitter	= AssaultRifleProjectileParticleEmitter;
	laserTrail[0]   = WpnRedPythonProjectileLaserTrail;
	laserTrail[1]   = WpnRedPythonProjectileLaserTrail;
	smoothLaserTrail = false;
//	laserTail	    = WpnRedPythonProjectileLaserTail;
//	laserTailLen    = 10;

	muzzleVelocity   = 9999;
	velInheritFactor = 0.0;
	
	isBallistic = false;
	gravityMod	= 7.5;

	armingDelay	= 0;
	lifetime    = 1000*10;
	fadeDelay   = 5000;
	
	decals[0] = BulletHoleDecalOne;
	
	hasLight	 = false;
	lightRadius = 6.0;
	lightColor  = "1.0 0.8 0.0";
};

function WpnRedPythonProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal,%dist)
{
    Parent::onCollision(%this,%obj,%col,%fade,%pos,%normal,%dist);

//	if( !(%col.getType() & $TypeMasks::ShapeBaseObjectType) )
//		return;
//
//    %src =  %obj.getSourceObject();
//    if(%src)
//        %src.setDiscTarget(%col);
}

//--------------------------------------------------------------------------

datablock ShotgunProjectileData(WpnBluePythonProjectile : WpnRedPythonProjectile)
{
//	projectileShapeName = "share/shapes/rotc/weapons/blaster/projectile.blue.dts";
//	explosion = WpnBluePythonProjectileExplosion;
//	bounceExplosion = WpnBluePythonProjectileBounceExplosion;
	laserTrail[0]   = WpnBluePythonProjectileLaserTrail;
//	laserTail = WpnBluePythonProjectileLaserTail;
//	lightColor  = "0.0 0.0 1.0";
};

function WpnBluePythonProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal,%dist)
{
    WpnRedPythonProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal,%dist);
}

//--------------------------------------------------------------------------
// weapon image which does all the work...
// (images do not normally exist in the world, they can only
// be mounted on ShapeBase objects)

datablock ShapeBaseImageData(WpnRedPythonImage)
{
	// add the WeaponImage namespace as a parent
	className = WeaponImage;
	
	// basic item properties
	shapeFile = "share/shapes/rotc/weapons/assaultrifle/image3.red.dts";
	emap = true;

	// mount point & mount offset...
	mountPoint  = 0;
	offset		= "0 0 0";
	rotation	 = "0 0 0";
	eyeOffset	= "0.275 0.1 -0.05";
	eyeRotation = "0 0 0 0";

	// Adjust firing vector to eye's LOS point?
	correctMuzzleVector = true;

	usesEnergy = true;
	minEnergy = 3;

	projectile = WpnRedPythonProjectile;

	// script fields...
	iconId = 7;
	specialWeapon = true;
	armThread  = "holdrifle";  // armThread to use when holding this weapon
	crosshair  = "assaultrifle"; // crosshair to display when holding this weapon

	//-------------------------------------------------
	// image states...
	//
		// preactivation...
		stateName[0]                     = "Preactivate";
		stateTransitionOnAmmo[0]         = "Activate";
		stateTransitionOnNoAmmo[0]		 = "NoAmmo";
		stateTimeoutValue[0]             = 0.25;
		stateSequence[0]                 = "idle";

		// when mounted...
		stateName[1]                     = "Activate";
		stateTransitionOnTimeout[1]      = "Ready";
		stateTimeoutValue[1]             = 0.25;
		stateSequence[1]                 = "idle";
		stateSpinThread[1]               = "SpinUp";

		// ready to fire, just waiting for the trigger...
		stateName[2]                     = "Ready";
  		stateTransitionOnNotLoaded[2]    = "Disabled";
		stateTransitionOnTriggerDown[2]  = "Fire";
        stateArmThread[2]                = "holdrifle";
		
		stateName[3]                     = "Fire";
		stateTransitionOnNoAmmo[3]       = "Reload";
		stateTransitionOnTimeout[3]      = "Fire";
		stateTransitionOnTriggerUp[3]    = "Ready";
		stateTimeoutValue[3]             = 0.1;
		stateFire[3]                     = true;
		stateFireProjectile[3]           = WpnRedPythonProjectile;
		stateAllowImageChange[3]         = false;
		stateEjectShell[3]               = true;
		stateRecoil[3]                   = LightRecoil;
		stateArmThread[3]                = "aimrifle";
		stateSequence[3]                 = "Fire";
		stateSound[3]                    = WpnPythonFireSound;
		
		stateName[4]                     = "KeepAiming";
		stateTransitionOnNoAmmo[4]       = "NoAmmo";
		stateTransitionOnNotLoaded[4]    = "Disabled";
		stateTransitionOnTriggerDown[4]  = "Fire";
		stateTransitionOnTimeout[4]      = "Ready";
		stateWaitForTimeout[4]           = false;
		stateTimeoutValue[4]             = 2.00;

        // no ammo...
		stateName[5]                     = "NoAmmo";
		stateTransitionOnAmmo[5]         = "Ready";
        stateTransitionOnTriggerDown[5]  = "DryFire";
		stateTimeoutValue[5]             = 0.50;
		stateSequence[5]                 = "NoAmmo";
  
        // dry fire...
		stateName[6]                     = "DryFire";
		stateTransitionOnTriggerUp[6]    = "NoAmmo";
		stateSound[6]                    = WeaponEmptySound;
    
		// disabled...
		stateName[7]                     = "Disabled";
		stateTransitionOnLoaded[7]       = "Ready";
		stateAllowImageChange[7]         = false;
	//
	// ...end of image states
	//-------------------------------------------------
};

function WpnRedPythonImage::getBulletSpread(%this, %obj)
{
   return 0.05;
}

function WpnRedPythonImage::onMount(%this, %obj, %slot)
{
   Parent::onMount(%this, %obj, %slot);

   // Set up recoil
   %obj.setImageRecoilEnabled(%slot, true);
   %obj.setImageCurrentRecoil(%slot, 80);
   %obj.setImageMaxRecoil(%slot, 80);
   %obj.setImageRecoilAdd(%slot, 0);
   %obj.setImageRecoilDelta(%slot, -0);
}

//------------------------------------------------------------------------------

datablock ShapeBaseImageData(WpnBluePythonImage : WpnRedPythonImage)
{
	shapeFile = "share/shapes/rotc/weapons/assaultrifle/image3.blue.dts";
	projectile = WpnBluePythonProjectile;
    stateFireProjectile[3] = WpnBluePythonProjectile;
};

function WpnBluePythonImage::getBulletSpread(%this, %obj, %slot)
{
    WpnRedPythonImage::getBulletSpread(%this, %obj, %slot);
}

function WpnBluePythonImage::onMount(%this, %obj, %slot)
{
    WpnRedPythonImage::onMount(%this, %obj, %slot);
}


