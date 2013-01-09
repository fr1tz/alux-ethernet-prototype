//------------------------------------------------------------------------------
// Alux
// Copyright (C) 2013 Michael Goldener <mg@wasted.ch>
//------------------------------------------------------------------------------


//-----------------------------------------------------------------------------

function EtherformData::useWeapon(%this, %obj, %nr)
{
	%client = %obj.client.changeInventory(%nr);
}

function EtherformData::damage(%this, %obj, %sourceObject, %pos, %damage, %damageType)
{
	// ignore damage
}

function EtherformData::onAdd(%this, %obj)
{
	Parent::onAdd(%this, %obj);
	
	// start singing...
	%obj.playAudio(1, FrmLightSingSound);

	// Make sure grenade ammo bar is not visible...
	messageClient(%obj.client, 'MsgGrenadeAmmo', "", 1);

	// lights...
	if(%obj.getTeamId() == 1)
		%obj.mountImage(FrmLightLightImage, 3);
	else
		%obj.mountImage(FrmLightLightImage, 3);

	%obj.client.inventoryMode = "show";
	%obj.client.displayInventory();
	
	if($Server::NewbieHelp && isObject(%obj.client))
	{
		%client = %obj.client;
		if(!%client.newbieHelpData_HasManifested)
		{
			%client.setNewbieHelp("You are in etherform! Press @bind34 inside a" SPC 
				(%client.team == $Team1 ? "red" : "blue") SPC "zone to change into CAT form.");
		}
		else if(%client.newbieHelpData_NeedsRepair && !%client.newbieHelpData_HasRepaired)
		{
			%client.setNewbieHelp("If you don't have enough health to change into CAT form," SPC
				"fly into one of your team's zones to regain your health.");
		}		
		else
		{
			%client.setNewbieHelp("random", false);
		}
	}			
}

function EtherformData::onDamage(%this, %obj, %delta)
{
	Parent::onDamage(%this, %obj, %delta);
}

//-----------------------------------------------------------------------------

datablock EtherformData(FrmLight)
{
	hudImageNameFriendly = "~/client/ui/hud/pixmaps/teammate.etherform.png";
	hudImageNameEnemy = "~/client/ui/hud/pixmaps/enemy.etherform.png";
	
	thirdPersonOnly = true;

    //category = "Vehicles"; don't appear in mission editor
	shapeFile = "share/shapes/rotc/vehicles/etherform/vehicle.red.dts";
	emap = true;
 
	cameraDefaultFov = 110.0;
	cameraMinFov     = 110.0;
	cameraMaxFov     = 130.0;
	cameraMinDist    = 2;
	cameraMaxDist    = 3;
	
	//renderWhenDestroyed = false;
	//explosion = FlyerExplosion;
	//defunctEffect = FlyerDefunctEffect;
	//debris = BomberDebris;
	//debrisShapeName = "share/shapes/rotc/vehicles/bomber/vehicle.dts";

	mass = 90;
	drag = 0.99;
	density = 10;

	maxDamage = 75;
	damageBuffer = 25;
	maxEnergy = 100;

	damageBufferRechargeRate = 0.15;
	damageBufferDischargeRate = 0.05;
	energyRechargeRate = 0.4;
 
    // collision box...
    boundingBox = "1.0 1.0 1.0";
 
    // etherform movement...
    accelerationForce = 100;

	// impact damage...
	minImpactSpeed = 1;		// If hit ground at speed above this then it's an impact. Meters/second
	speedDamageScale = 0.0;	// Dynamic field: impact damage multiplier

	// damage info eyecandy...
	damageBufferParticleEmitter = FrmLightDamageBufferEmitter;
	repairParticleEmitter = FrmLightRepairEmitter;
//	bufferRepairParticleEmitter = FrmLightBufferRepairEmitter;

	// laser trail...
	laserTrail[0] = FrmLight_LaserTrailOne;
	laserTrail[1] = FrmLight_LaserTrailTwo;

	// contrail...
	minTrailSpeed = 1;
	//particleTrail = FrmLight_ContrailEmitter;
	
	// various emitters...
	//forwardJetEmitter = FlyerJetEmitter;
	//downJetEmitter = FlyerJetEmitter;

	//
//	jetSound = Team1ScoutFlyerThrustSound;
//	engineSound = EtherformSound;
	softImpactSound = FrmLightImpactSound;
	hardImpactSound = FrmLightImpactSound;
	//wheelImpactSound = WheelImpactSound;
};

// callback function: called by engine
function FrmLight::onAdd(%this,%obj)
{
	Parent::onAdd(%this,%obj);
   FrmLight_updateProxyThread(%this, %obj);
}

function FrmLight::onTrigger(%this, %obj, %triggerNum, %val)
{
	if(%triggerNum == 0 && %val)
	{
      %pos = %obj.getWorldBoxCenter();
      %vec = %obj.getEyeVector();
 		%vel = VectorScale(%vec, FrmLightProjectile.muzzleVelocity);

		// create the projectile object...
		%p = new Projectile() {
			dataBlock       = FrmLightProjectile;
			teamId          = %obj.teamId;
			initialVelocity = %vel;
			initialPosition = %pos;
			sourceObject    = %obj;
			//sourceSlot      = %slot;
			client	        = %obj.client;
		};
		MissionCleanup.add(%p);

      %obj.client.camera.setOrbitMode(%p, 0, 0, 10, 10, true);
      %obj.client.camera.setTransform(%obj.getTransform());
      %obj.client.control(%obj.client.camera);
      %obj.client.player = %p;

      %obj.schedule(0, "delete");
	}
}

function FrmLight_updateProxyThread(%this, %obj)
{
   schedule(32, %obj, "FrmLight_updateProxyThread", %this, %obj);

   %client = %obj.client;
   if(!isObject(%client) || !isObject(%client.form))
      return;

   %start = %obj.getWorldBoxCenter();
   %end = VectorAdd(%start, VectorScale(%obj.getEyeVector(), 9999));

   %c = containerRayCast(%start, %end, $TypeMasks::TerrainObjectType |
      $TypeMasks::InteriorObjectType , %obj);

   if(!%c)
      return;

   %x = getWord(%c,1); %x = mFloor(%x); %x -= (%x % 2);
   %y = getWord(%c,2); %y = mFloor(%y); %y -= (%y % 2);
   %z = getWord(%c,3);
   %pos = %x SPC %y SPC %z;
   %normal = getWord(%c,4) SPC getWord(%c,5) SPC getWord(%c,6);
   if(%pos $= %client.form.getPosition())
      return;

   error("yay");
   %transform = %pos SPC %normal SPC "0";
   %client.form.setTransform(%transform);
   %materializeError = "The selected form can't materialize";
   if(%client.form.getDataBlock().isMethod("canMaterialize"))
   {
      %materializeError = %client.form.getDataBlock().canMaterialize(%client,
         %pos, %normal, %transform);
   }

   if(%materializeError $= "")
   {
      %client.form.shapeFxSetTexture(1, 1);
      %client.form.shapeFxSetColor(1, 6);
      %client.form.shapeFxSetBalloon(1, 1.0, 0.0);
      %client.form.shapeFxSetFade(1, 1.0, 0.0);
      %client.form.shapeFxSetActive(1, true, true);
   }
   else
   {
      %client.form.shapeFxSetTexture(1, 1);
      %client.form.shapeFxSetColor(1, 1);
      %client.form.shapeFxSetBalloon(1, 1.0, 0.0);
      %client.form.shapeFxSetFade(1, 1.0, 0.0);
      %client.form.shapeFxSetActive(1, true, true);
   }
   %client.form.startFade(0, 0, true);
}
