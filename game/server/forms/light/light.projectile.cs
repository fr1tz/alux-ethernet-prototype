//------------------------------------------------------------------------------
// Alux
// Copyright (C) 2013 Michael Goldener <mg@wasted.ch>
//------------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// projectile datablock...

datablock ProjectileData(FrmLightProjectile)
{
	stat = "lightform";

	// script damage properties...
	impactDamage       = 0;
	impactImpulse      = 0;
	splashDamage       = 0;
	splashDamageRadius = 0;
	splashImpulse      = 0;
	bypassDamageBuffer = false;
	
	trackingAgility = 0;
	
	explodesNearEnemies			= false;
	explodesNearEnemiesRadius	= 2;
	explodesNearEnemiesMask	  = $TypeMasks::PlayerObjectType;

	sound = FrmLightProjectileSound;
 
   projectileShapeName = "share/shapes/alux/light.dts";

	explosion             = FrmLightProjectileExplosion;
//	bounceExplosion       = RedAssaultRifleProjectileBounceExplosion;
//	hitEnemyExplosion     = RedAssaultRifleProjectileExplosion;
//	nearEnemyExplosion    = RedAssaultRifleProjectileExplosion;
//	hitTeammateExplosion  = RedAssaultRifleProjectileExplosion;
//	hitDeflectorExplosion = DiscDeflectedEffect;

//	missEnemyEffectRadius = 10;
//	missEnemyEffect = AssaultRifleProjectileMissedEnemyEffect;

	particleEmitter = FrmLightProjectileEmitter;
	laserTrail[0]   = FrmLight_LaserTrailOne;
	laserTrail[1]   = FrmLight_LaserTrailTwo;
//	laserTail	    = RedAssaultRifleProjectileLaserTail;
//	laserTailLen    = 2;

	energyDrain = 0;
	muzzleVelocity	= 100 * $Server::Game.slowpokemod;
	velInheritFactor = 0.0 * $Server::Game.slowpokemod;
	
	isBallistic			= false;
	//gravityMod			 = 5.0 * $Server::Game.slowpokemod;

	armingDelay			= 0;
	lifetime				= 1000*10;
	fadeDelay			= 1000*10;
	
//	numBounces = 2;
	
	hasLight	 = true;
	lightRadius = 4.0;
	lightColor  = "1.0 1.0 1.0";
};


function FrmLightProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal,%dist)
{
   %client = %obj.client;
	%ownTeamId = %client.player.getTeamId();

	%inOwnZone = false;
	%inOwnTerritory = false;
	%inEnemyZone = false;

	InitContainerRadiusSearch(%pos, 0.0001, $TypeMasks::TacticalSurfaceObjectType);
	while((%srchObj = containerSearchNext()) != 0)
	{
		// object actually in this zone?
		//%inSrchZone = false;
		//for(%i = 0; %i < %srchObj.getNumObjects(); %i++)
		//{
      //   error(%srchObj.getObject(%i).getDataBlock().getName());
		//	if(%srchObj.getObject(%i) == %this.player)
		//	{
		//		%inSrchZone = true;
		//		break;
		//	}
		//}
		//if(!%inSrchZone)
		//	continue;

		%zoneTeamId = %srchObj.getTeamId();
		%zoneBlocked = %srchObj.zBlocked;

		if(%zoneTeamId != %ownTeamId && %zoneTeamId != 0)
		{
			%inEnemyZone = true;
			break;
		}
		else if(%zoneTeamId == %ownTeamId)
		{
			%inOwnZone = true;
			if(%srchObj.getDataBlock().getName() $= "TerritorySurface"
			|| %srchObj.getDataBlock().isTerritoryZone)
				%inOwnTerritory = true;
		}
	}

   %spawn = true;
	if(%inEnemyZone)
	{
		%client.beepMsg("You can not manifest in an enemy zone!");
		%spawn = false;
	}
	else if(%inOwnZone && !%inOwnTerritory)
	{
		%client.beepMsg("This is not a territory zone!");
		%spawn = false;
	}
	else if(!%inOwnZone)
	{
		%client.beepMsg("You can only manifest in your team's territory zones!");
		%spawn = false;
	}
	else if(%zoneBlocked)
	{
		%client.beepMsg("This zone is currently blocked!");
		%spawn = false;
	}

   if(%spawn)
   {
      %form = $Server::Game.form[getWord(%client.activeLoadout, 0)];
      if(!isObject(%form))
         %spawn = false;
   }

   if(%spawn)
   {
      %player = %form.materialize(%client, %pos, %normal, %client.camera.getTransform());
      %player.setTransform(%client.proxy.getTransform());
      %client.proxy.removeClientFromGhostingList(%client);
      %client.proxy.setTransform("0 0 0");
      $aiTarget = %player;

      %player.inv[1] = getWord(%client.activeLoadout, 4);
   }
   else
   {
		%data = %client.getEtherformDataBlock();
		%player = new Etherform() {
			dataBlock = %data;
			client = %client;
			teamId = %client.team.teamId;
		};
      MissionCleanup.add(%player);
      %player.setTransform(%pos);

   }
   %client.player = %player;
   %client.control(%player);
}

