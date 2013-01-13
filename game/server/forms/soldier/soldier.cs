//------------------------------------------------------------------------------
// Alux
// Copyright (C) 2013 Michael Goldener <mg@wasted.ch>
//------------------------------------------------------------------------------

datablock StaticShapeData(FrmSoldierProxy : FrmCrateProxy)
{
   form = FrmSoldier; // script field
	shapeFile = "share/shapes/alux/soldier.dts";
};

function FrmSoldierProxy::adjustTransform(%this, %pos, %normal, %eyeVec)
{
   %pos = VectorAdd(%pos, VectorScale(%normal, 0.25));
   %vec = getWord(%eyeVec,0) SPC getWord(%eyeVec,1) SPC 0;
   %transform = createOrientFromDir(%vec);
   %transform = setWord(%transform, 0, getWord(%pos, 0));
   %transform = setWord(%transform, 1, getWord(%pos, 1));
   %transform = setWord(%transform, 2, getWord(%pos, 2));
   return %transform;
}

//------------------------------------------------------------------------------

datablock TSShapeConstructor(FrmSoldierDts)
{
	baseShape = "share/shapes/alux/soldier.dts";
	sequenceBaseDir = "share/shapes/rotc/players/a/";

	// movement when standing...
	sequence0  = "tl/root.dsq root";
	sequence1  = "nm/run.dsq run";
	sequence2  = "nm/back.dsq back";
	sequence3  = "nm/side.dsq side";

	// movement when marching...
	sequence4  = "tl/root.dsq rootMarching";
	sequence5  = "nm/run.dsq runMarching";
	sequence6  = "nm/back.dsq backMarching";
	sequence7  = "nm/side.dsq sideMarching";

	// movement when crouched...
	sequence8  = "tl/rootcrouched.dsq UNUSED_rootCrouched";
	sequence9  = "nm/run.dsq runCrouched";
	sequence10  = "nm/back.dsq backCrouched";
	sequence11  = "nm/side.dsq sideCrouched";

	// movement when prone...
	sequence12 = "fb/rootprone.dsq rootProne";
	sequence13 = "fb/rootprone.dsq rootProne";
	sequence14 = "fb/rootprone.dsq rootProne";
	sequence15 = "fb/rootprone.dsq rootProne";

	// arm threads...
	sequence16 = "a/holdnoweapon.dsq look";
	sequence17 = "a/discdeflect_left_base.dsq discdeflect_left_base";
	sequence18 = "a/holdgun_onehand.dsq look2";
	sequence52 = "a/holdblaster.dsq holdblaster";
	sequence53 = "ub/aimblaster.dsq aimblaster";
	sequence19 = "ub/holdrifle.dsq holdrifle";
	sequence51 = "ub/aimrifle.dsq aimrifle";
	sequence20 = "ub/holdshield.dsq holdshield";
	sequence46 = "a/holdspear.dsq holdspear";
	sequence47 = "a/holdaimspear.dsq holdaimspear";

	// other...
	sequence21 = "nm/diehead.dsq death1";
	sequence22 = "nm/diechest.dsq death2";
	sequence23 = "nm/dieback.dsq death3";
	sequence24 = "nm/diesidelf.dsq death4";
	sequence25 = "nm/diesidert.dsq death5";
	sequence26 = "nm/dieleglf.dsq death6";
	sequence27 = "nm/dielegrt.dsq death7";
	sequence28 = "nm/dieslump.dsq death8";
	sequence29 = "nm/dieknees.dsq death9";
	sequence30 = "nm/dieforward.dsq death10";
	sequence31 = "nm/diespin.dsq death11";

	sequence32 = "nm/headside.dsq headside";
	sequence33 = "nm/recoilde.dsq light_recoil";
	sequence34 = "nm/sitting.dsq sitting";
	sequence35 = "fb/cel_headbang.dsq celsalute";
	sequence36 = "nm/tauntbest.dsq celwave";
	sequence37 = "nm/standjump.dsq standjump";

	sequence38 = "nm/head.dsq head";
	sequence39 = "nm/fall.dsq fall";
	sequence40 = "nm/land.dsq land";
	sequence41 = "nm/jump.dsq jump";

	sequence42 = "fb/cel_hail.dsq celhail";

	sequence43 = "ub/throwsidearm.dsq throwSidearm";
	sequence44 = "ub/aimarmcannon.dsq aimarmcannon";
	sequence48 = "ub/aimspear.dsq aimSpear";
	sequence49 = "ub/throwSpear.dsq throwSpear";
	sequence50 = "ub/discdeflect_left_anim.dsq discdeflect_left_anim";
	sequence54 = "ub/throwinterceptor.dsq throwInterceptor";

	sequence45 = "fb/flyer.dsq flyer";

	sequence55  = "b/slide.dsq slide";
};

datablock PlayerData(FrmSoldier)
{
   proxy = FrmSoldierProxy; // script field

	className = StandardCat;
	
	firstPersonOnly = false;
	
	targetLockTimeMS = 200;
	
	hudImageNameFriendly = "~/client/ui/hud/pixmaps/teammate.cat.png";
	hudImageNameEnemy = "~/client/ui/hud/pixmaps/enemy.cat.png";

	useEyePoint = true;
	renderFirstPerson = true;
	emap = true;

   eyeOffset = "0 -0.2 -0.02";
   cameraDefaultFov = 110.0;
	cameraMinFov	  = 10.0;
	cameraMaxFov	  = 130.0;
	cameraMinDist = 1;
	cameraMaxDist = 5;

	shapeFile = "share/shapes/alux/soldier.dts";
 
	//cloakTexture = "share/shapes/rotc/effects/explosion_white.png";
	shapeFxTexture[0] = "share/textures/alux/shiny.png";
	shapeFxTexture[1] = "share/textures/alux/grid1.png";
	shapeFxTexture[2] = "share/textures/alux/grid2.png";
	shapeFxColor[0] = "1.0 1.0 1.0 1.0";  

	computeCRC = true;

	canObserve = true;
	cmdCategory = "Clients";

	renderWhenDestroyed = false;
	//debrisShapeName = "share/shapes/rotc/players/standardcat/debris.red.dts";
	//debris = StandardCatDebris;

	aiAvoidThis = true;

	minLookAngle = -1.5;
	maxLookAngle = 1.5;
	minLookAngleMarching = -1.5;
	maxLookAngleMarching = 1.5;
	minLookAngleCrouched = -1.5;
	maxLookAngleCrouched = 1.5;
	minLookAngleProne = -0.8;
	maxLookAngleProne = 0.8;

	maxFreelookAngle = 3.0;

	mass = 90;
	drag = 0.0;
	density = 10;
	gravityMod = 1.0;

	maxDamage = 100;
	damageBuffer = 0;
	maxEnergy = 800;

	repairRate = 0.8;
	damageBufferRechargeRate = 0.0;
	damageBufferDischargeRate = 0.0;
	energyRechargeRate = 0.0;

	skidSpeed = 20;
	skidFactor = 0.4;

	flyForce = 0;

	runForce = 50 * 90; // formerly 48 * 90
	runEnergyDrain = 0;
	minRunEnergy = 0;
	
	slideForce = 20 * 90;
	slideEnergyDrain = 0;
	minSlideEnergy = 0;

	maxForwardSpeed = 10;
	maxBackwardSpeed = 8;
	maxSideSpeed = 8;
	maxForwardSpeedSliding = 30;
	maxBackwardSpeedSliding = 25;
	maxSideSpeedSliding = 12;
	maxForwardSpeedMarching = 8;
	maxBackwardSpeedMarching = 8;
	maxSideSpeedMarching = 8;
	maxForwardSpeedCrouched = 10;
	maxBackwardSpeedCrouched = 8;
	maxSideSpeedCrouched = 8;
//	maxForwardSpeedProne = 3; NOT USED
//	maxBackwardSpeedProne = 3; NOT USED
//	maxSideSpeedProne = 2; NOT USED

	maxUnderwaterForwardSpeed = 8.4;
	maxUnderwaterBackwardSpeed = 7.8;
	maxUnderwaterSideSpeed = 7.8;
	// [todo: insert values for other body poses here?]

	jumpForce = 8 * 90;  // 12 * 90
	jumpEnergyDrain = 0;
   reJumpForce = 10 * 90; // script field
   reJumpEnergyDrain = 20; // script field
	minJumpEnergy = 0;
	jumpDelay = 0;
	
	recoverDelay = 9;
	recoverRunForceScale = 1.2;

	minImpactSpeed = 30; //
	speedDamageScale = 3.0; // dynamic field: impact damage multiplier

	boundingBox = "1.2 1.1 2.7";
	pickupRadius = 0.75;

	// Controls over slope of runnable/jumpable surfaces
	maxStepHeight = 1.0;
	maxStepHeightMarching = 1.0;
	maxStepHeightCrouched = 1.0;
	maxStepHeightProne = 0.2;
	runSurfaceAngle  = 35;
	runSurfaceAngleMarching  = 35;
	runSurfaceAngleCrouched  = 35;
	runSurfaceAngleProne  = 35;

	jumpSurfaceAngle = 30;

	minJumpSpeed = 20;
	maxJumpSpeed = 30;

	horizMaxSpeed = 200;
	horizResistSpeed = 30;
	horizResistFactor = 0.35;

	upMaxSpeed = 65;
	upResistSpeed = 25;
	upResistFactor = 0.3;

	footstepSplashHeight = 0.35;

	// Damage location details
	boxNormalHeadPercentage		 = 0.83;
	boxNormalTorsoPercentage		= 0.49;
	boxHeadLeftPercentage			= 0;
	boxHeadRightPercentage		  = 1;
	boxHeadBackPercentage			= 0;
	boxHeadFrontPercentage		  = 1;

	// footprints
	decalData	= FrmSoldierFootprint;
	decalOffset = 0.25;

	// foot puffs
	footPuffEmitter = FrmSoldierFootPuffEmitter;
	footPuffNumParts = 10;
	footPuffRadius = 0.5;
	
	// ground connection beam
	groundConnectionBeam = FrmSoldierGroundConnectionBeam;

   numShapeTrails = 0;

	// slide emitters
	//slideContactParticleFootEmitter = RedSlideEmitter;
	slideContactParticleTrailEmitter[0] = CatSlideContactTrailEmitter;
	//slideParticleFootEmitter = RedCatSlideFootEmitter;
	//slideParticleTrailEmitter[0] = BlueSlideEmitter;
	skidParticleFootEmitter = CatSkidFootEmitter;
	skidParticleTrailEmitter[0] = CatSkidTrailEmitter0;
	skidParticleTrailEmitter[1] = CatSkidTrailEmitter1;
	
	// damage info eyecandy...
	//damageBufferParticleEmitter = RedCatDamageBufferEmitter;
	//repairParticleEmitter = RedCatRepairEmitter;
	//bufferRepairParticleEmitter = RedCatBufferRepairEmitter;
	damageParticleEmitter = FrmCrate_DamageEmitter;
	//bufferDamageParticleEmitter = RedCatBufferDamageEmitter;
	//damageDebris = RedCatDamageDebris;
	//bufferDamageDebris = CatBufferDamageDebris;

	// not implemented in engine...
	// dustEmitter = StandardCatLiftoffDustEmitter;

	splash = StandardCatSplash;
	splashVelocity = 4.0;
	splashAngle = 67.0;
	splashFreqMod = 300.0;
	splashVelEpsilon = 0.60;
	bubbleEmitTime = 0.4;
	splashEmitter[0] = StandardCatFoamDropletsEmitter;
	splashEmitter[1] = StandardCatFoamEmitter;
	splashEmitter[2] = StandardCatBubbleEmitter;
	mediumSplashSoundVelocity = 10.0;
	hardSplashSoundVelocity = 20.0;
	exitSplashSoundVelocity = 5.0;

	//NOTE:  some sounds commented out until wav's are available

	// Footstep Sounds
	LeftFootSoftSound		= FrmSoldierLeftFootSoftSound;
	LeftFootHardSound		= FrmSoldierLeftFootHardSound;
	LeftFootMetalSound	  = FrmSoldierLeftFootMetalSound;
	LeftFootSnowSound		= FrmSoldierLeftFootSnowSound;
	LeftFootShallowSound	= FrmSoldierLeftFootShallowSplashSound;
	LeftFootWadingSound	 = FrmSoldierLeftFootWadingSound;
	RightFootSoftSound	  = FrmSoldierRightFootSoftSound;
	RightFootHardSound	  = FrmSoldierRightFootHardSound;
	RightFootMetalSound	 = FrmSoldierRightFootMetalSound;
	RightFootSnowSound	  = FrmSoldierRightFootSnowSound;
	RightFootShallowSound  = FrmSoldierRightFootShallowSplashSound;
	RightFootWadingSound	= FrmSoldierRightFootWadingSound;
	FootUnderwaterSound	 = FrmSoldierFootUnderwaterSound;
	//FootBubblesSound	  = FootBubblesSound;
	//movingBubblesSound	= ArmorMoveBubblesSound;
	//waterBreathSound	  = WaterBreathMaleSound;

	impactSoftSound		= FrmSoldierImpactSoftSound;
	impactHardSound		= FrmSoldierImpactHardSound;
	impactMetalSound	  = FrmSoldierImpactMetalSound;
	impactSnowSound		= FrmSoldierImpactSnowSound;

	//impactWaterEasy		= ImpactLightWaterEasySound;
	//impactWaterMedium	 = ImpactLightWaterMediumSound;
	//impactWaterHard		= ImpactLightWaterHardSound;

	groundImpactMinSpeed	 = 10.0;
	groundImpactShakeFreq	= "4.0 4.0 4.0";
	groundImpactShakeAmp	 = "1.0 1.0 1.0";
	groundImpactShakeDuration = 0.8;
	groundImpactShakeFalloff = 10.0;

	//exitingWater			= ExitingWaterLightSound;
	slideSound = PlayerSlideSound;
	slideContactSound = PlayerSlideContactSound;
	skidSound = PlayerSkidSound;

	observeParameters = "0.5 4.5 4.5";
};

// *** callback function: called by engine
function FrmSoldier::onAdd(%this, %obj)
{
   Parent::onAdd(%this, %obj);
   %obj.setEnergyLevel(%this.maxEnergy);

   if(isObject(%obj.client))
   {
      %c = %obj.client;
      commandToClient(%c, 'Hud', "health", true);
   }
}

function FrmSoldier::canMaterialize(%this, %client, %pos, %normal, %transform)
{
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
		return "You cannot materialize on an enemy's surface!";
	}
	else if(%inOwnZone && !%inOwnTerritory)
	{
		return "This surface can't be used to materialize!";
	}
	else if(!%inOwnZone)
	{
		return "You can only materialize on your team's surfaces!";
	}
	else if(%zoneBlocked)
	{
		return "This surface is currently blocked!";
	}

   return "";
}

function FrmSoldier::materialize(%this, %client, %pos, %normal, %transform)
{
//	if( %this.team == $Team1 )
		%data = FrmSoldier;
//	else
//		%data = BlueFrmSoldier;
	%player = new Player() {
		dataBlock = %data;
		client = %client;
		teamId = %client.team.teamId;
	};
   MissionCleanup.add(%player);

   %player.setTransform(%pos SPC getWords(%transform, 3));

   %this.materializeFx(%player);

	%player.playAudio(0, CatSpawnSound);

   return %player;
}

function FrmSoldier::materializeFx(%this, %obj)
{
   FrmCrate::materializeFx(%this, %obj);
}

function FrmSoldier::dematerialize(%this, %obj)
{
   %obj.damage(0, %obj.getPosition(), 100, $DamageType::Force);
}

function FrmSoldier::reload(%this, %obj)
{
   if(%obj.getEnergyLevel() == %this.maxEnergy)
      return;
   %state = %obj.getImageState(0);
   if(!(%state $= "Ready" || %state $= "NoAmmo" || %state $= "KeepAiming"))
      return;
   %reloadTime = %obj.getMountedImage(0).reloadTime;
   %reloadAmount = %obj.getMountedImage(0).reloadAmount;
   if(%reloadAmount $= "")
      %reloadAmount = %this.maxEnergy;
   %newEnergyLevel = %obj.getEnergyLevel() + %reloadAmount;
   %obj.unmountImage(0);
   %obj.setArmThread("look");
   %obj.setEnergyLevel(0);
   %obj.schedule(%reloadTime, "setEnergyLevel", %newEnergyLevel);
   %obj.schedule(%reloadTime, "useWeapon", 1);
}



