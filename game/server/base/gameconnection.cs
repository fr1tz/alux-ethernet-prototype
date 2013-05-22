//------------------------------------------------------------------------------
// Alux Prototype
// Copyright notices are in the file named COPYING.
//------------------------------------------------------------------------------

datablock StaticShapeData(PointerShape)
{
   allowColorization = true;
	shadowEnable = false;
	shapeFile = "share/shapes/alux/pointer.dts";
   emap = false;
   hudImageNameFriendly = "~/client/ui/hud/pixmaps/pointer.png";
};

//------------------------------------------------------------------------------

function GameConnection::control(%this, %shapebase)
{
	%oldcontrol = %this.getControlObject();
	%newcontrol = %shapebase;
	
	if(%newcontrol.isTransformed)
		%newControl = %newcontrol.transformObj;

	%currTagged = %oldcontrol.getCurrTagged();
	if(%currTagged)
		%newcontrol.setCurrTagged(%currTagged);
	else
	{
		%newcontrol.setCurrTagged(0);
		%newcontrol.setCurrTaggedPos(%oldcontrol.getCurrTaggedPos());
	}
	
	%this.setControlObject(%newcontrol);
}

//------------------------------------------------------------------------------

// *** callback function: called by script code in "common"
function GameConnection::prepareCookies(%this, %cookies)
{
   %cookies.push_back("Alux_HudColor", "");
   %cookies.push_back("Alux_HudMenuTMode", "");
   %cookies.push_back("Alux_Handicap", "");
   %cookies.push_back("Alux_DamageScreenMode", "");
   // Loadouts
	for(%i = 1; %i <= 10; %i++)
   {
		%cookies.push_back("ALUX_LNAME" @ %i, "");
		%cookies.push_back("ALUX_LCODE" @ %i, "");
   }
}

// *** callback function: called by script code in "common"
function GameConnection::onCookiesReceived(%this, %cookies)
{
	%this.setHandicap(arrayGetValue(%cookies, "Alux_Handicap"));
	%this.hudColor = arrayGetValue(%cookies, "Alux_HudColor");
	%this.initialTopHudMenu = arrayGetValue(%cookies, "Alux_HudMenuTMode");
	if(%this.initialTopHudMenu $= "")
		%this.initialTopHudMenu = "pieces";
	%this.damageScreenMode = arrayGetValue(%cookies, "Alux_DamageScreenMode");
	if(%this.damageScreenMode $= "")
		%this.damageScreenMode = 1;
   // Loadouts
	for(%i = 0; %i <= 100; %i++)
   {
      %this.loadDefaultLoadout(%i);
      %name = arrayGetValue(%cookies, "ALUX_LNAME" @ %i);
      %code = arrayGetValue(%cookies, "ALUX_LCODE" @ %i);
      if(%name !$= "")
   		%this.loadoutName[%i] = %name;
      if(%code !$= "")
   		%this.loadoutCode[%i] = %code;
   }
   %this.cookiesDone = true;
	%this.updateQuickbar();
}

//------------------------------------------------------------------------------

// *** callback function: called by script code in "common"
function GameConnection::onClientLoadMission(%this)
{
	%this.loadingMission = true;
   %this.cookiesDone = false;
	%this.updateQuickbar();
	serverCmdMainMenu(%this);
}


// *** callback function: called by script code in "common"
function GameConnection::onClientEnterGame(%this)
{
	%this.loadingMission = false;
	%this.ingame = true;

	commandToClient(%this, 'SyncClock', $Sim::Time - $Game::StartTime);
	
	// ScriptObject used to store raw statistics...
	%this.stats                    = new ScriptObject();
	%this.stats.joinTime           = $Sim::Time;
	%this.stats.lastReceivedDamage = new ScriptObject();
	%this.stats.dmgDealtApplied    = new Array();
	%this.stats.dmgDealtCaused     = new Array();
	%this.stats.dmgReceivedApplied = new Array();
	%this.stats.dmgReceivedCaused  = new Array();
	%this.stats.healthLost         = new Array();
	%this.stats.healthRegained     = new Array();
	%this.stats.fired              = new Array();
	// ScriptObject used to store processed statistics...
	%this.pstats = new ScriptObject();
	
	// "simple control (tm)" info...
	%this.simpleControl = new Array();
	MissionCleanup.add(%this.simpleControl);

	// HUD Backgrounds...
	for(%i = 1; %i <= 3; %i++)
	{
		%this.hudBackgroundBitmap[%i] = "";
		%this.hudBackgroundColor[%i] = "";
		%this.hudBackgroundRepeat[%i] = "";
		%this.hudBackgroundAlphaDt[%i] = "";
	}

	// HUD Warnings...
	for(%i = 1; %i <= 6; %i++)
	{
		%this.hudWarningString[%i] = "";
		%this.hudWarningVisible[%i] = "";
	}
	%this.updateHudWarningsThread();

	// HUD Menus...
	%this.setHudMenuL("*", " ", 1, 0);
	%this.setHudMenuR("*", " ", 1, 0);
	%this.setHudMenuT("*", " ", 1, 0);
	%this.setHudMenuC("*", " ", 1, 0);
	
	// Top HUD Menu...
	%this.topHudMenu = %this.initialTopHudMenu;
	%this.updateTopHudMenuThread();

   // Crosshair...
   %this.setDefaultCrosshair();
 	
	//
	// setup observer camera object...
	//
	
	%this.camera = new Camera() {
		dataBlock = ObserverCamera;
	};
	MissionCleanup.add(%this.camera);
	%this.camera.scopeToClient(%this);
		
	%this.camera.setTransform(pickObserverPoint(%this));
	%this.camera.setVelocity("0 0 0");

   // Loadout
	%this.defaultLoadout();
	%this.updateLoadout();

   // Inventory
   %this.inventory = new ScriptObject();
   %this.updateInventoryThread();

	//
	// join team with less players...
	//
	if($Team1.numPlayers > $Team2.numPlayers)
   	%this.joinTeam(2);
   else
      %this.joinTeam(1);

	// Start sky color thread.
	%this.updateSkyColor();

	%this.updateQuickbar();
	if(%this.menu $= "mainmenu")
		serverCmdMainMenu(%this);

	// Start thread to process player stats...
	%this.processPlayerStats();	

   %this.updateProxyThread();
}

function GameConnection::createPointer(%this)
{
   if(isObject(%this.pointer))
      %this.pointer.delete();

   %this.pointer = new StaticShape() {
	  dataBlock = PointerShape;
	  client = %this;
     teamId = %this.team.teamId;
   };
   MissionCleanup.add(%this.pointer);
   %this.pointer.setGhostingListMode("GhostOnly");
   %this.pointer.getHudInfo().setActive(true);
   %this.pointer.setCollisionsDisabled(true);

   return;

   %this.pointer.startFade(0, 0, true);

   %this.pointer.shapeFxSetTexture(0, 0);
   %this.pointer.shapeFxSetColor(0, 0);
   %this.pointer.shapeFxSetBalloon(0, 1.0, 0.0);
   %this.pointer.shapeFxSetFade(0, 1.0, 0.0);
   %this.pointer.shapeFxSetActive(0, true, true);

   %this.pointer.shapeFxSetTexture(1, 1);
   %this.pointer.shapeFxSetColor(1, 0);
   %this.pointer.shapeFxSetBalloon(1, 1.0, 0.0);
   %this.pointer.shapeFxSetFade(1, 1.0, 0.0);
   %this.pointer.shapeFxSetActive(1, true, true);
}

// *** callback function: called by script code in "common"
function GameConnection::onClientLeaveGame(%this)
{
	%this.team.numPlayers--;
 
   %this.removeForms();

	%this.clearFullControl();
	%this.clearSimpleControl();
 
	if(isObject(%this.stats))
	{
		%this.stats.lastReceivedDamage.delete();
		%this.stats.dmgDealtApplied.delete();
		%this.stats.dmgDealtCaused.delete();
		%this.stats.dmgReceivedApplied.delete();
		%this.stats.dmgReceivedCaused.delete();
		%this.stats.healthLost.delete();
		%this.stats.healthRegained.delete();
		%this.stats.fired.delete();
		%this.stats.delete();
	}

	if(isObject(%this.pstats))
		%this.pstats.delete();

	if(isObject(%this.simpleControl))
		%this.simpleControl.delete();
	
	if(isObject(%this.thirdEye))
		%this.thirdEye.delete();

	if(isObject(%this.camera))
		%this.camera.delete();

	if(isObject(%this.inventory))
		%this.inventory.delete();
  
  	if(isObject(%this.player) && %this.player.getClassName() $= "Etherform")
		%this.player.delete();
  
	if(isObject(%this.proxy))
		%this.proxy.delete();

	if(isObject(%this.pointer))
		%this.pointer.delete();
		
	%count = ClientGroup.getCount();
	for(%cl= 0; %cl < %count; %cl++)
   {
		%client = ClientGroup.getObject(%cl);
      if(%client.menuVisible && %client.menu $= "teams")
         %client.showTeamsMenu();
   }
}

//------------------------------------------------------------------------------

function GameConnection::onRecordingDemo(%this, %isRecording)
{
   if(!%isRecording)
      return;

   //echo(%this.getId() SPC "started recording a demo");

   //---------------------------------------------------------------------------
	// HACK HACK HACK: find way to update object colorizations
   // only for this client
	%group = nameToID("TerritorySurfaces");
	if(%group != -1)
	{
		%count = %group.getCount();
		if (%count != 0)
		{
				for (%i = 0; %i < %count; %i++)
				{
					%zone = %group.getObject(%i);
					%zone.setTeamId(%zone.getTeamId());
				}
		}
	}
   //---------------------------------------------------------------------------

   %this.updateHudColors();
   %this.setDefaultCrosshair();
}

//------------------------------------------------------------------------------

function GameConnection::updateHudColors(%this)
{
	%c1 = "255 255 0";
	%c2 = "255 128 0";
	commandToClient(%this,'SetHudColor', %c1, %c2);
   return;

	if(getFieldCount(%this.hudColor) == 2)
	{
		%c1 = getField(%this.hudColor,0);
		%c2 = getField(%this.hudColor,1);
	}
	else if(%this.hudColor $= "fr1tz")
	{
		%c1 = "200 200 200";
		%c2 = "255 0 255";
	}
	else if(%this.hudColor $= "kurrata")
	{
		%c1 = "0 150 0";
		%c2 = "255 50 255";
	}
	else if(%this.hudColor $= "c&c")
	{
		%c1 = "63 151 48";
		%c2 = "202 180 130";
	}	
	else if(%this.hudColor $= "cga1dark")
	{
		%c1 = "170 0 170";
		%c2 = "0 170 170";
	}
	else if(%this.hudColor $= "cga1light")
	{
		%c1 = "255 80 255";
		%c2 = "85 255 255";
	}	
	else if(%this.hudColor $= "based_on_team")
	{
	   %teamId = %this.team.teamId;
		if(%teamId == 0)
		{
			%c1 = "150 150 150";
			%c2 = "255 255 255";
		}
		else if(%teamId == 1)
		{
   		%c1 = "255 80 255";
   		%c2 = "200 200 200";
		}
		else if(%teamId == 2)
		{
			%c1 = "85 255 255";
   		%c2 = "200 200 200";
		}
	}
	else
	{
		%player = %this.player;
		%data = %player.getDataBlock();
		%v = %player.getDamagePercent();
		%c1 = mFloatLength(255*%v, 0) SPC
			mFloatLength(255*(1-%v), 0) SPC
			0;
		%v = mFloatLength(125*(1-%v)+125, 0);
		%c2 =  %v SPC %v SPC %v;
	}
	commandToClient(%this,'SetHudColor', %c1, %c2);	
}

//------------------------------------------------------------------------------

function GameConnection::setDefaultCrosshair(%this)
{
   commandToClient(%this, 'Crosshair', 0);
   commandToClient(%this, 'Crosshair', 2, 2);
   //commandToClient(%this, 'Crosshair', 3, 2, 20);
   commandToClient(%this, 'Crosshair', 5, "./rotc/ch1");
   commandToClient(%this, 'Crosshair', 1);
}

//------------------------------------------------------------------------------

function GameConnection::removeForms(%this)
{
	for( %idx = MissionCleanup.getCount()-1; %idx >= 0; %idx-- )
	{
		%obj = MissionCleanup.getObject(%idx);
		if(%obj.getType() & $TypeMasks::ProjectileObjectType
		|| %obj.getType() & $TypeMasks::PlayerObjectType
		|| %obj.getType() & $TypeMasks::VehicleObjectType
		|| %obj.getType() & $TypeMasks::DamagableItemObjectType
		|| %obj.getType() & $TypeMasks::CorpseObjectType)
      {
         if(%obj.client == %this)
			   %obj.delete();
      }
	}
}

//------------------------------------------------------------------------------

function GameConnection::joinTeam(%this, %teamId)
{
	if (%teamid > 2 || %teamid < 0)
		return false;

	if( %this.team != 0 && %teamId == %this.team.teamId)
		return false;

   %this.removeForms();

	// remove from old team...
	if(%this.team == $Team0)
		$Team0.numPlayers--;
	else if(%this.team == $Team1)
		$Team1.numPlayers--;
	else if(%this.team == $Team2)
		$Team2.numPlayers--;

	// add client to new team...
	if(%teamId == 0)
	{
		%this.team = $Team0;
		$Team0.numPlayers++;
		
		%this.setNewbieHelp("You are in observer mode. Click on 'Switch Team' at the top" SPC
			"of the arena window to join a team. Press @bind01 if the arena window is not visible.");		
   
      %this.setLoadingBarText("Use the 'Switch Team' menu to join" SPC
         $Team1.name SPC "or" SPC $Team2.name @ "!");
	}
	else
   {
      if(%teamId == 1)
   	{
   		%this.team = $Team1;
   		$Team1.numPlayers++;
   	}
   	else if(%teamId == 2)
   	{
   		%this.team = $Team2;
   		$Team2.numPlayers++;
   	}
    
      %this.setLoadingBarText("Press @bind01 to play" SPC  $Server::MissionType);
   }

	// full and simple control cleanup...
	%this.clearFullControl();
	%this.clearSimpleControl();

	// notify all clients of team change...
	MessageAll('MsgClientJoinTeam', '\c2%1 joined the %2.',
		%this.name,
		%this.team.name,
		%this.team.teamId,
		%this,
		%this.sendGuid,
		%this.score,
		%this.isAiControlled(),
		%this.isAdmin,
		%this.isSuperAdmin);

   %this.createPointer();
	%this.spawnPlayer();
	%this.updateHudColors();
   %this.displayInventory();
	
	%count = ClientGroup.getCount();
	for(%cl= 0; %cl < %count; %cl++)
   {
		%client = ClientGroup.getObject(%cl);
      if(%client.menuVisible && %client.menu $= "teams")
         %client.showTeamsMenu();
   }

   //---------------------------------------------------------------------------
	// HACK HACK HACK: find way to update object colorizations
   // only for the client that switched teams.
	for( %idx = MissionCleanup.getCount()-1; %idx >= 0; %idx-- )
	{
		%obj = MissionCleanup.getObject(%idx);
      %obj.setTeamId(%obj.getTeamId());
	}
	%group = nameToID("TerritorySurfaces");
	if(%group != -1)
	{
		%count = %group.getCount();
		if (%count != 0)
		{
				for (%i = 0; %i < %count; %i++)
				{
					%zone = %group.getObject(%i);
					%zone.setTeamId(%zone.getTeamId());
				}
		}
	}
   //---------------------------------------------------------------------------


	return true;
}

function GameConnection::spawnPlayer(%this)
{
   %this.resetInventory();

	// Remove existing etherform
	if(%this.player > 0 && %this.player.getClassName() $= "Etherform")
		%this.player.delete();

	// observers have no players...
	if( %this.team == $Team0 )
	{
		%this.camera.setFlyMode();
		%this.setControlObject(%this.camera);
		return;
	}

	%spawnSphere = pickSpawnSphere(%this.team.teamId);

	%data = %this.getEtherformDataBlock();
	%obj = new Etherform() {
		dataBlock = %data;
		client = %this;
		teamId = %this.team.teamId;
	};
   MissionCleanup.add(%obj);

	// player setup...
	%obj.setTransform(%spawnSphere.getTransform());
	%obj.setCurrTagged(0);
	%obj.setCurrTaggedPos("0 0 0");

	// update the client's observer camera to start with the player...
	%this.camera.setMode("Observer");
	%this.camera.setTransform(%this.player.getEyeTransform());

	// give the client control of the player...
	%this.player = %obj;
	%this.setControlObject(%obj);
	%this.updateHudColors();
}

//-----------------------------------------------------------------------------

function GameConnection::beepMsg(%this, %reason)
{
	//MessageClient(%this, 'MsgBeep', '\c0Fail: %1', %reason);
	//bottomPrint(%this, %reason, 3, 1 );
   %this.setHudWarning(2, %reason, true);
   if(%this.clearBeepMsgThread !$= "")
      cancel(%this.clearBeepMsgThread);
   %this.clearBeepMsgThread = %this.schedule(4000, "setHudWarning", 2, "", false);
	%this.play2D(BeepMessageSound);
}

function GameConnection::onFormDestroyed(%this, %obj)
{
   %obj.zFormDestroyed = true;
   %obj.removePiecesFromPlay();
}

function GameConnection::enterForm(%this)
{
   %etherform = %this.player;
   %pos = %etherform.getWorldBoxCenter();
   %closest = 0;
   %distmin = 4.0;
   InitContainerRadiusSearch(%pos, %distmin, $TypeMasks::ShapeBaseObjectType);
	while((%srchObj = containerSearchNext()) != 0)
	{
      if(%srchObj == %this.player)
         continue;
      if(%srchObj.getTeamId() != %this.player.getTeamId())
         continue;
      if(isObject(%srchObj.getControllingClient()))
         continue;
      if(!isObject(%srchObj.getDataBlock()))
         continue;
      if(!%srchObj.getDataBlock().isMethod("dematerialize"))
         continue;
      %dist = VectorLen(VectorSub(%srchObj.getWorldBoxCenter(), %pos));
      if(%dist < %distmin)
      {
         %closest = %srchObj;
         %distmin = %dist;
      }
   }
   if(isObject(%closest))
   {
      if(%closest.zBlocked)
      {
         %this.play2D(BeepMessageSound);
         return;
      }

      if(isObject(%this.proxy))
      {
		   //%this.proxy.delete();
         %this.proxy.removeClientFromGhostingList(%client);
         %this.proxy.setTransform("0 0 0");
         %this.pointer.removeClientFromGhostingList(%client);
         %this.pointer.setTransform("0 0 0");
         %this.pointer.getHudInfo().setActive(false);
      }
      %this.player = %closest;
      %this.control(%closest);
      %etherform.delete();

      %this.inventoryMode = "";
      %this.displayInventory();
   }
   else
   {
      %this.play2D(BeepMessageSound);
   }
}

function GameConnection::leaveForm(%this, %obj, %dematerialize)
{
   if(%obj.getClassName() $= "Etherform" && %dematerialize)
   {
      return;

   	%pos = %this.player.getWorldBoxCenter();
      %closest = 0;
      %distmin = 5.0;
		InitContainerRadiusSearch(%pos, %distmin, $TypeMasks::ShapeBaseObjectType);
		while((%srchObj = containerSearchNext()) != 0)
		{
         if(%srchObj == %this.player)
            continue;
         if(%srchObj.getTeamId() != %this.player.getTeamId())
            continue;
         if(isObject(%srchObj.getControllingClient()))
            continue;
         if(!isObject(%srchObj.getDataBlock()))
            continue;
         if(!%srchObj.getDataBlock().isMethod("dematerialize"))
            continue;
         %dist = VectorLen(VectorSub(%srchObj.getWorldBoxCenter(), %pos));
         if(%dist < %distmin)
         {
            %closest = %srchObj;
            %distmin = %dist;
         }
      }
      if(isObject(%closest))
         %closest.getDataBlock().dematerialize(%closest);
      return;
   }

   // Can't leave a form we're not actually controlling
   %form = %this.player;
	if(!isObject(%form) || %obj != %form)
		return;

	//%tagged = %form.isTagged();
	%pos = %form.getWorldBoxCenter();

	if($Server::NewbieHelp)
	{
		%this.newbieHelpData_NeedsRepair =
			(%this.player.getDamageLevel() > %this.player.getDataBlock().maxDamage*0.75);
		%this.newbieHelpData_LowEnergy =
			(%this.player.getEnergyLevel() < 50);
	}
	
	%data = %this.getEtherformDataBlock();
	%obj = new Etherform() {
		dataBlock = %data;
		client = %this;
		teamId = %this.team.teamId;
	};
   MissionCleanup.add(%obj);
	
	%obj.setTransform(%form.getTransform());
	%obj.setTransform(%form.getWorldBoxCenter());

	%nrg = %form.getEnergyLevel();
   %maxdmg = %form.getDataBlock().maxDamage;
   %prevhealth = (%maxdmg - %dmg)/%maxdmg;
   %newenergy = (%data.maxEnergy*%prevhealth)/2;
   %obj.setEnergyLevel(%newenergy);
	%obj.applyImpulse(%pos, VectorScale(%form.getVelocity(),100));
	%obj.playAudio(0, EtherformSpawnSound);

	%this.control(%obj);
	%this.player = %obj;
	
//	if(%tagged)
//		%obj.setTagged();

   if(%dematerialize)
   {
      if(%form.getDataBlock().isMethod("dematerialize"))
         %form.getDataBlock().dematerialize(%form);
   }
   else
   {
		%form.setShapeName("");
		//%this.player.getHudInfo().markAsControlled(0, 0);
   }
}

function GameConnection::dematerializeFormAllowed(%this, %obj)
{
   if(%obj == %this.player)
      return false;
   if(%obj.getTeamId() != %this.player.getTeamId())
      return false;
   if(isObject(%obj.getControllingClient()))
      return false;
   if(%obj.client != %this)
      return false;
   if(!isObject(%obj.getDataBlock()))
      return false;
   if(!%obj.getDataBlock().isMethod("dematerialize"))
      return false;
   return true;
}

function GameConnection::dematerializeFormsRadius(%this, %pos, %radius)
{
   %delay = 0;
	InitContainerRadiusSearch(%pos, %radius, $TypeMasks::ShapeBaseObjectType);
	while((%srchObj = containerSearchNext()) != 0)
	{
      if(!%this.dematerializeFormAllowed(%srchObj))
         continue;
      %srchObj.schedule(%delay, "dematerialize");
      %delay += 200;
   }
}

function GameConnection::dematerializeForm(%this, %nr)
{
   %this.leftHudMenu = "";
   %this.updateLeftHudMenu();

   %client = %this;
   %obj = %this.player;
   if(!isObject(%client) || !isObject(%obj))
      return;

   %eyeVec = %obj.getEyeVector();
   %start = %obj.getEyePoint();
   %end = VectorAdd(%start, VectorScale(%eyeVec, 1000));

   %c = containerRayCast(%start, %end, $TypeMasks::ShapeBaseObjectType |
      $TypeMasks::TerrainObjectType | $TypeMasks::InteriorObjectType, %obj);

   if(%nr == 0)
   {
      %form = getWord(%c, 0);
      if(%form && %this.dematerializeFormAllowed(%form))
         %form.dematerialize();
      return;
   }

   %pos = getWords(%c, 1, 3);

   switch(%nr)
   {
      case 1: %radius = 2;
      case 2: %radius = 10;
      case 3: %radius = 9999;
   }

   %this.dematerializeFormsRadius(%pos, %radius);
}


// called by script code
function GameConnection::spawnForm(%this)
{
   %client = %this;
   %obj = %this.player;

   if(%this.spawnError !$= "")
   {
      %this.play2D(BeepMessageSound);
      return;
   }

   %form = $Server::Game.form[getWord(%this.activeLoadout, 0)];
   if(!isObject(%form))
      return;

   %player = %form.materialize(%client, %pos, %normal, %this.camera.getTransform());
   %player.setTransform(%this.proxy.getTransform());
   //%client.proxy.removeClientFromGhostingList(%client);
   //%client.proxy.setTransform("0 0 0");
   %player.setLoadoutCode(%this.activeLoadout);

   %player.zBlocked = true;
   %player.schedule(2000, "setFieldValue", "zBlocked", false);

   $aiTarget = %player;
}

// called by script code
function GameConnection::updateProxyThread(%this)
{
   %this.schedule(32, "updateProxyThread");

   %client = %this;
   %obj = %this.player;
   if(!isObject(%client) || !isObject(%client.proxy) || !isObject(%obj))
      return;

   %prevSpawnError = %client.spawnError;

   %eyeVec = %obj.getEyeVector();
   %start = %obj.getEyePoint();
   %end = VectorAdd(%start, VectorScale(%eyeVec, 1000));

   %c = "";
   if(%this.player.getClassName() $= "Etherform")
      %c = containerRayCast(%start, %end, $TypeMasks::TerrainObjectType |
         $TypeMasks::InteriorObjectType, %obj);

   if(!%c)
   {
      %client.spawnError = "No surface in range";
      %client.proxy.removeClientFromGhostingList(%client);
      %client.proxy.setTransform("0 0 0");
      %client.pointer.removeClientFromGhostingList(%client);
      %client.pointer.setTransform("0 0 0");
      %client.pointer.getHudInfo().setActive(false);
      return;
   }

   if(isObject(%client.pointer))
   {
      %pos = getWords(%c, 1, 3);
      %normal = getWords(%c, 4, 6);

      %client.proxy.basePos = %pos;
      
      %transform = createOrientFromDir(%normal);
      %pos = VectorAdd(%pos, VectorScale(%normal, 0.25));
      %transform = setWord(%transform, 0, getWord(%pos, 0));
      %transform = setWord(%transform, 1, getWord(%pos, 1));
      %transform = setWord(%transform, 2, getWord(%pos, 2));

      %client.pointer.addClientToGhostingList(%client);
      %client.pointer.setTransform(%transform);
      %this.pointer.getHudInfo().setActive(true);
   }

   if(%obj.getEnergyLevel() < 0) // %this.maxEnergy)
   {
      %client.spawnError = "Not enough energy to materialize.";
      %client.proxy.shapeFxSetColor(0, 2);
      %client.proxy.shapeFxSetColor(1, 2);
   }
   else
      %client.spawnError = "";

   %x = getWord(%c,1);
   if(true)
   {
      //%r = %x - mFloor(%x);
      %r = %x % 2;
      if(%r > 1)
         %x = mCeil(%x);
      else
         %x = mFloor(%x);
   }
   //   %x = mFloor(%x);
   //else
   //   %x = mCeil(%x);
   //%x -= (%x % 2);
   %y = getWord(%c,2);
   if(true)
   {
      //%r = %y - mFloor(%y);
      %r = %y % 2;
      if(%r > 1)
         %y = mCeil(%y);
      else
         %y = mFloor(%y);
   }
   //if(%y > 0)
   //   %y = mFloor(%y);
   //else
   //   %y = mCeil(%y);
   //%y -= (%y % 2);
   %z = getWord(%c,3);
   %pos = %x SPC %y SPC %z;
   %normal = getWords(%c, 4, 6);
   if(%pos $= %client.proxy.getPosition()
   && (%client.spawnError $= %prevSpawnError))
      return;

   %transform = %pos SPC %normal SPC "0";
   if(%client.proxy.getDataBlock().isMethod("adjustTransform"))
   {
      %transform = %client.proxy.getDataBlock().adjustTransform(
         %pos, %normal, %eyeVec);
   }
   %client.proxy.addClientToGhostingList(%client);
   %client.proxy.setTransform(%transform);

   if(%obj.getEnergyLevel() < %this.maxEnergy)
      return;

   %pieces = sLoadoutcode2Pieces(%client.activeLoadout);
   %missing = "";
   for(%f = 0; %f < getFieldCount(%pieces); %f++)
   {
      %field = getField(%pieces, %f);
      %piece = getWord(%field, 0);
      %count = getWord(%field, 1);

      %used = %client.inventory.pieceUsed[%piece];
      %free = %client.inventory.pieceCount[%piece] - %used;

      %piecestring = sPiece2String(%piece);

      if(%free < %count)
      {
         if(%missing !$= "")
            %missing = %missing @ " and ";
         %missing = %missing @ %piecestring;
      }
   }
   if(%missing !$= "")
      %client.spawnError = "Bank is missing" SPC %missing SPC "piece";


   if(%client.spawnError $= "")
   {
      if(%client.proxy.getDataBlock().form.isMethod("canMaterialize"))
      {
         %client.spawnError = %client.proxy.getDataBlock().form.canMaterialize(
            %client, %pos, %normal, %transform);
      }
   }

   if(%client.spawnError $= "")
   {
      %client.proxy.shapeFxSetColor(0, 3);
      %client.proxy.shapeFxSetColor(1, 3);
   }
   else
   {
      %client.proxy.shapeFxSetColor(0, 1);
      %client.proxy.shapeFxSetColor(1, 1);
   }
}


//-----------------------------------------------------------------------------

function GameConnection::setSkyColor(%this, %color, %elementMask)
{
	//error("GameConnection::setSkyColor():" SPC %this.getId() SPC %color SPC %elementMask);

   if(%elementMask $= "")
      %elementMask = 1023;

   %mask = 1;
   for(%i = 0; %i <= 9; %i++)
   {
      if(%elementMask & %mask)
      {
         if(%this.skyColor[%i] $= %color)
            %elementMask = (%elementMask & ~%mask);
         else
            %this.skyColor[%i] = %color;
      }
      %mask *= 2;
   }

	if(!(%elementMask > 0))
		return;

   commandToClient(%this, 'SkyColor', %color, %elementMask);
}

function GameConnection::updateSkyColor(%this)
{
	if(%this.skyColorThread !$= "")
		cancel(%this.skyColorThread);
	%this.skyColorThread = %this.schedule(500, "updateSkyColor");

	if(Sky.NoColorChange)
		return;

	// In certain gametypes the sky is the same for every client
	if($Game::GameType == $Game::TeamDragRace)
		return;

	%player = %this.player;
	if(!isObject(%player))
		%player = %this.camera;

	%pos = %player.getPosition();
	InitContainerRadiusSearch(%pos, 0.0001, $TypeMasks::TacticalZoneObjectType);
	%zone = containerSearchNext();

	if(%zone == 0)
	{
		%this.setSkyColor("0.2 0.2 0.2");
	}
	else if(%zone.getTeamId() == 0 && %zone.zProtected)
	{
		%this.setSkyColor("0 1 0");
	}
	else if(%zone.getTeamId() == 0 && !%zone.zHasNeighbour)
	{
		%this.setSkyColor("0 0.5 0");
	}	
	else if(%zone.getTeamId() == 0 && %zone.zNumReds == 0 && %zone.zNumBlues == 0)
	{
		%this.setSkyColor("1 1 1");
	}
	else if(%zone.getTeamId() == 1 && !%zone.zBlocked)
	{
		%this.setSkyColor("1 0 0");
	}
	else if(%zone.getTeamId() == 2 && !%zone.zBlocked)
	{
		%this.setSkyColor("0 0 1");
	}
	else
	{
		%health[1] = 0;
		%health[2] = 0;
		for(%i = 0; %i < %zone.getNumObjects(); %i++)
		{
			%obj = %zone.getObject(%i);
			if(%obj.getType() & $TypeMasks::PlayerObjectType && %obj.isCAT)
			{
				%health[%obj.teamId] += %obj.getDataBlock().maxDamage
					- %obj.getDamageLevel() + %obj.getDamageBufferLevel();
			}
		}
		if(%health[1] > %health[2])
		{
			%ratio = %health[2] / %health[1];
			%this.setSkyColor("1 0.75" SPC %ratio);
		}
		else
		{
			%ratio = %health[1] / %health[2];
			%this.setSkyColor(%ratio SPC "0.75 1");
		}
	}
}

//-----------------------------------------------------------------------------

function GameConnection::setLoadingBarText(%this, %text)
{
   if(%this.loadingBarText $= %text)
      return;
	commandToClient(%this, 'LoadingBarTxt', %text);
   %this.loadingBarText = %text;
}

//------------------------------------------------------------------------------

function GameConnection::beginQuickbarText(%this, %update)
{
	commandToClient(%this, 'BeginQuickbarTxt', %update);
}

function GameConnection::addQuickbarText(%this, %text, %layerMask)
{
	%l = strlen(%text); %n = 0;
	while(%n < %l)
	{
		%chunk = getSubStr(%text, %n, 255);
		commandToClient(%this, 'AddQuickbarTxt', %chunk, %layerMask);
		%n += 255;
	}	
}

function GameConnection::endQuickbarText(%this)
{
	commandToClient(%this, 'EndQuickbarTxt');
}

//------------------------------------------------------------------------------

function GameConnection::beginMenuText(%this, %update)
{
	commandToClient(%this, 'BeginMenuTxt', %update);
}

function GameConnection::addMenuText(%this, %text, %layerMask)
{
	%l = strlen(%text); %n = 0;
	while(%n < %l)
	{
		%chunk = getSubStr(%text, %n, 255);
		commandToClient(%this, 'AddMenuTxt', %chunk, %layerMask);
		%n += 255;
	}	
}

function GameConnection::endMenuText(%this)
{
	commandToClient(%this, 'EndMenuTxt');
}

//-----------------------------------------------------------------------------

function GameConnection::setHudBackground(%this, %slot, %bitmap, %color,
	%repeat, %alpha, %alphaDt)
{
	if(%this.hudBackgroundBitmap[%slot] $= %bitmap)
		%bitmap = "";
	else
		%this.hudBackgroundBitmap[%slot] = %bitmap;	

	if(%this.hudBackgroundColor[%slot] $= %color)
		%color = "";
	else
		%this.hudBackgroundColor[%slot] = %color;

	if(%this.hudBackgroundRepeat[%slot] $= %repeat)
		%repeat = "";
	else
		%this.hudBackgroundRepeat[%slot] = %repeat;		

	if(%this.hudBackgroundAlphaDt[%slot] $= %alphaDt)
		%alphaDt = "";
	else
		%this.hudBackgroundAlphaDt[%slot] = %alphaDt;

	commandToClient(%this, 'SetHudBackground', %slot, %bitmap, %color, 
		%repeat, %alpha, %alphaDt);
}

//-----------------------------------------------------------------------------

function GameConnection::setHudWarning(%this, %slot, %text, %visibility)
{
	if(%this.hudWarningString[%slot] $= %text)
		%text = "";
	else
		%this.hudWarningString[%slot] = %text;	

	if(%this.hudWarningVisible[%slot] $= %visibility)
		%visibility = "";
	else
		%this.hudWarningVisible[%slot] = %visibility;	

	if(%text $= "" && %visibility $= "")
		return;

	//error("Sending MsgWarning to" SPC %this SPC ": [" SPC %slot SPC "][" SPC %text SPC "][" SPC %visibility SPC "]");
	messageClient(%this, 'MsgWarning', "", %slot, %text, %visibility);
}

function GameConnection::updateHudWarningsThread(%this)
{
	cancel(%this.updateHudWarningsThread);
	%this.updateHudWarningsThread = %this.schedule(100,"updateHudWarningsThread");

	%player = %this.player;
	if(!isObject(%player))
	{
		%this.setHudWarning(1, "", false);
		%this.setHudWarning(3, "", false);	
		return;
	}

//	%health = %player.getDataBlock().maxDamage
//		- %player.getDamageLevel()
//		+ %player.getDamageBufferLevel();
//	%health = %health / %player.getDataBlock().maxDamage;

//	%this.setHudWarning(1, "[HEALTH]", %health < 0.25);
//   if(%player.isCAT)
//	  %this.setHudWarning(3, "[DAMPER]", %player.getEnergyPercent() < 0.5);
//   else
//	  %this.setHudWarning(3, "[ENERGY]", %player.getEnergyPercent() < 0.5);

   if(%player.getClassName() $= "Etherform")
   {
      if(%this.spawnError $= "")
         %this.setHudWarning(5, "Click left mouse button to materialize.", false);
      else
         %this.setHudWarning(5, %this.spawnError, true);
   }
   else
   {
      %form = %this.player;
      %this.setHudWarning(5, "", false);
      //if(%form.isReloading)
      //   %this.setHudWarning(5, "Reloading...", true);
      //else
      //   %this.setHudWarning(5, "Press 'r' to reload!", %form.getEnergyLevel() == 0);
   }
}

//-----------------------------------------------------------------------------

function GameConnection::resetInventory1(%this, %piece, %current, %max, %cost)
{
   %this.inventory.pieceExists[%piece] = true;
   %this.inventory.pieceUsed[%piece]   = 0;
   %this.inventory.pieceCount[%piece]  = %current;
   %this.inventory.pieceMax[%piece]    = %max;
   %this.inventory.pieceCost[%piece]   = %cost;
}

function GameConnection::resetInventory(%this)
{
   %this.resetInventory1(0, 1, 1, 10); // Infantry
   %this.resetInventory1(1, 1, 1, 15); // Air
   %this.resetInventory1(2, 1, 1, 15); // Missile
   %this.resetInventory1(3, 15, 15, 10); // Box
   %this.resetInventory1(4, 1, 1, 15); // Pistol
   %this.resetInventory1(5, 1, 1, 15); // Shotgun
   %this.resetInventory1(6, 1, 1, 30); // Sniper
   %this.resetInventory1(7, 1, 1, 15); // Magnum
   %this.resetInventory1(8, 1, 1, 15); // SMG
}

function GameConnection::updateInventoryThread(%this)
{
   //error("GameConnection::updateInventoryThread()");

	cancel(%this.updateInventoryThread);
	%this.updateInventoryThread = %this.schedule(1000,"updateInventoryThread");

   if(!isObject(%this.inventory))
      return;

   %piece = 0;
   while(%this.inventory.pieceExists[%piece])
   {
      %oldCount = %this.inventory.pieceCount[%piece];
      %newCount = %oldCount + 1 / %this.inventory.pieceCost[%piece];
      if(%newCount > %this.inventory.pieceMax[%piece])
         %newCount = %this.inventory.pieceMax[%piece];

      %this.inventory.pieceCount[%piece] = %newCount;
      
      if(%piece == 0)
      {
         if(%this.player.getClassName() $= "Etherform")
            %this.player.updateVisuals();
      }

      %piece++;
   }
}

//-----------------------------------------------------------------------------

function GameConnection::switchTopHudMenuMode(%this)
{
	if(%this.topHudMenu $= "newbiehelp")
	{
		%this.topHudMenu = "healthbalance";
	}
	else if(%this.topHudMenu $= "healthbalance")
	{
		%this.topHudMenu = "nothing";
	}
	else 
	{
		%this.topHudMenu = "newbiehelp";
	}
	
	%this.setHudMenuT("*", " ", 1, 0);
}

function byteToHex(%byte)
{
	%chars = "0123456789ABCDEF";
	
	%digit[0] = "0";
	%digit[1] = "0";
	
	if(%byte > 15)
		%digit[0] = getSubStr(%chars, %byte / 16, 1);

	%digit[1] = getSubStr(%chars, %byte % 16, 1);

	return %digit[0] @ %digit[1];
}

datablock AudioProfile(NewbieHelperSound)
{
	filename = "share/sounds/rotc/charge1.wav";
	description = AudioCritical2D;
	preload = true;
};

datablock AudioProfile(ClockTickSound)
{
	filename = "share/sounds/rotc/charge3.wav";
	description = AudioCritical2D;
	preload = true;
};

function GameConnection::updateTopHudMenuThread(%this)
{
	cancel(%this.updateTopHudMenuThread);
	%this.updateTopHudMenuThread = %this.schedule(200,"updateTopHudMenuThread");
	
	if(%this.topHudMenu $= "invisible")
		return;

   %text = "<color:00FF00><tab:240,260,280,300,340,360,380,400,420,440>";
	%this.setHudMenuT(0, %text, 1, 1);
	//%this.setHudMenuT(0, "\n<just:center><color:888888>Showing: ", 1, 1);
	//%this.setHudMenuT(2, "(@bind66 to change)\n<just:left>", 1, 1);
	%i = 2;

   %line1 = "";
   %line3 = "";
   %piece = 0;
   while(%this.inventory.pieceExists[%piece])
   {
      %used = %this.inventory.pieceUsed[%piece];
      %free = %this.inventory.pieceCount[%piece] - %used;

      %used = mFloor(%used);
      if(%used == 0)
      {
         %used = "   ";
      }
      else
      {
         switch(strlen(%used))
         {
            case 1: %used = "  " @ %used;
            case 2: %used = " " @ %used;
         }
      }
      %line2 = %line2 TAB %used;

      %free = mFloor(%free);
      if(%free == 0)
      {
         %free = "   ";
      }
      else
      {
         switch(strlen(%free))
         {
            case 1: %free = "  " @ %free;
            case 2: %free = " " @ %free;
         }
      }
      %line1 = %line1 TAB %free;

      %piece++;
   }

   %text = "<spush><font:arial:8>\n<spop>" @ %line1;
	%this.setHudMenuT(1, %text, 1, 1);

   %icons1 = "\n" TAB
      "<bitmap:share/hud/alux/piece.infantry.16x16.png>" TAB
      "<bitmap:share/hud/alux/piece.air.16x16.png>" TAB
      "<bitmap:share/hud/alux/piece.missile.16x16.png>" TAB
      "<bitmap:share/hud/alux/piece.box.16x16.png>";

   %icons2 = "" TAB
      "<bitmap:share/hud/alux/piece.pistol.16x16.png>" TAB
      "<bitmap:share/hud/alux/piece.shotgun.16x16.png>" TAB
      "<bitmap:share/hud/alux/piece.sniper.16x16.png>" TAB
      "<bitmap:share/hud/alux/piece.magnum.16x16.png>" TAB
      "<bitmap:share/hud/alux/piece.smg.16x16.png>";

	%this.setHudMenuT(2, %icons1, 1, 1);
	%this.setHudMenuT(3, %icons2, 1, 1);

   %text = "<spush><font:arial:16>\n<spop>" @ %line2;
	%this.setHudMenuT(4, %text, 1, 1);

   return;
	
	if(%this.topHudMenu $= "newbiehelp")
	{
		//if(%this.newbieHelpAge == 0)
		//	%this.play2D(NewbieHelperSound);
	
		%this.newbieHelpAge++;	
		//%this.setHudMenuT(1, "Tip", 1, 1);

		%color = "FFFFFF";
		%alpha = 255;		
		
		%text = %this.newbieHelpText;
		//if(%this.newbieHelpAge < 6)
      if(false)
		{
			%text = "[ Downloading... ]";
			if(%this.newbieHelpAge % 2 == 0)
				%color = "88FF88";			
		}
		
		if(%this.newbieHelpTime > 0 && %this.newbieHelpAge > %this.newbieHelpTime)
			%alpha = 255 - (%this.newbieHelpAge-%this.newbieHelpTime)*15;
		
		if(%alpha <= 0)
		{
			%alpha = 0;
			%this.setHudMenuT(5, "", 1, 0);
			%this.setHudMenuT(8, "", 1, 0);		
         %this.setHudMenuT(%i++, "", 1, false);
         %this.setHudMenuT(%i++, "", 1, false);
         %this.setHudMenuT(%i++, "", 1, false);
         %this.setHudMenuT(%i++, "", 1, false);
         %this.setHudMenuT(%i++, "", 1, false);
         %this.setHudMenuT(%i++, "", 1, false);
		}
      else
      {
   		%this.setHudMenuT(%i++, "<spush><just:center><font:Arial:18><color:FFFFFF", 1, 1);
         %this.setHudMenuT(%i++, byteToHex(%alpha), 1, 1);
         %this.setHudMenuT(%i++, ">Tip:\n<color:", 1, 1);
         %this.setHudMenuT(%i++, %color, 1, 1);
         %this.setHudMenuT(%i++, byteToHex(%alpha), 1, 1);
         %this.setHudMenuT(%i++, ">" @ %text @ "\n<spop>", 1, 1);
      }
		%this.setHudMenuT(%i++, "(Press @bind65 for a random hint)", 1, 1);
	}	
	else if(%this.topHudMenu $= "healthbalance")
	{	
		%this.setHudMenuT(1, "Health balance", 1, 1);
		%this.setHudMenuT(3, "<lmargin:180><bitmap:share/hud/rotc/spec><sbreak>", 1, 1);
		%this.setHudMenuT(4, "<bitmap:share/hud/rotc/spacer.1x14>", $Server::GameStatus::HealthBalance::Spacers, 1);
		%this.setHudMenuT(5, "<bitmap:share/hud/rotc/marker.up>", 1, 1);			
	}
	else if(%this.topHudMenu $= "teamjoustclock")
	{		
		%this.setHudMenuT(1, "Clock", 1, 1);

	}
	else if(%this.topHudMenu $= "nothing")
	{		
		%this.setHudMenuT(1, "Nothing", 1, 1);
	}
}

function GameConnection::updateLeftHudMenu1(%this, %i)
{
   if(%i < 50)
      %tmp = "@bind" @ 34+%i @ ":" TAB %this.loadoutName[%i];
   else
      %tmp = "@bind" @ 64+%i-50 @ ":" TAB %this.loadoutName[%i];
   %pieces = sLoadoutCode2Pieces(%this.loadoutCode[%i]);
   for(%f = 0; %f < getFieldCount(%pieces); %f++)
   {
      %field = getField(%pieces, %f);
      %piece = getWord(%field, 0);
      %count = getWord(%field, 1);
      switch(%piece)
      {
         case 0: %icon = "infantry";
         case 1: %icon = "air";
         case 2: %icon = "missile";
         case 3: %icon = "box";
         case 4: %icon = "pistol";
         case 5: %icon = "shotgun";
         case 6: %icon = "sniper";
         case 7: %icon = "magnum";
         case 8: %icon = "smg";
      }
      %icons = "";
      while(%count > 0)
      {
         %icons = %icons @ "<bitmap:share/hud/alux/piece." @ %icon @ ".16x16.png>";
         %count--;
      }
      %tmp = %tmp @ %icons;
   }
   //%tmp = %tmp @ "\n";
   return %tmp;
}

function GameConnection::updateLeftHudMenu(%this)
{
   if(%this.leftHudMenu $= "dmenu")
   {
		%this.setHudMenuL(0, "\n", 8, 1);
		%this.setHudMenuL(1, "<lmargin:100><font:Arial:18><tab:120,175,200>" @
         "Dematerialize:\n\n", 1, 1);

      %slot = 2;
      %text = "@bind" @ 51 @ ":" TAB "Single form" @ "\n";
      %this.setHudMenuL(%slot, %text, 1, 1); %slot++;
      %text = "@bind" @ 32+%slot @ ":" TAB "Close" @ "\n";
      %this.setHudMenuL(%slot, %text, 1, 1); %slot++;
      %text = "@bind" @ 32+%slot @ ":" TAB "Group" @ "\n";
      %this.setHudMenuL(%slot, %text, 1, 1); %slot++;
      %text = "@bind" @ 32+%slot @ ":" TAB "Everything" @ "\n";
      %this.setHudMenuL(%slot, %text, 1, 1); %slot++;

		for(%i = %slot; %i < 10; %i++)
			%this.setHudMenuL(%i, "", 1, 0);

      return;
   }

   if(%this.team == $Team0 || %this.inventoryMode $= "")
   {
      %this.setHudMenuL("*", " ", 1, 0);
      %this.setHudMenuR("*", " ", 1, 0);
      return;
   }

	%iconname[$CatEquipment::Blaster] = "blaster";
	%iconname[$CatEquipment::BattleRifle] = "rifle";
	%iconname[$CatEquipment::SniperRifle] = "sniper";
	%iconname[$CatEquipment::MiniGun] = "minigun";
	%iconname[$CatEquipment::RepelGun] = "grenadelauncher";
	%iconname[$CatEquipment::GrenadeLauncher] = "grenadelauncher";
	%iconname[$CatEquipment::SlasherDisc] = "slasherdisc";
	%iconname[$CatEquipment::RepelDisc] = "repeldisc";
	%iconname[$CatEquipment::ExplosiveDisc] = "explosivedisc";
	%iconname[$CatEquipment::Damper] = "damper";
	%iconname[$CatEquipment::Shield] = "shield";
	%iconname[$CatEquipment::Barrier] = "barrier";
	%iconname[$CatEquipment::VAMP] = "vamp";
	%iconname[$CatEquipment::Anchor] = "anchor";
	%iconname[$CatEquipment::Stabilizer] = "stabilizer";
	%iconname[$CatEquipment::Permaboard] = "permaboard";
	%iconname[$CatEquipment::Grenade] = "grenade";
	%iconname[$CatEquipment::Bounce] = "bounce";
	%iconname[$CatEquipment::Etherboard] = "etherboard";
	%iconname[$CatEquipment::Regeneration] = "regen";

	%item[1] = $CatEquipment::Blaster;
	%item[2] = $CatEquipment::BattleRifle;
	%item[3] = $CatEquipment::SniperRifle;
	%item[4] = $CatEquipment::Minigun;
	if($Game::GameType == $Game::Ethernet)
		%item[5] = $CatEquipment::RepelGun;
	else
		%item[5] = $CatEquipment::GrenadeLauncher;
	%item[6] = $CatEquipment::Etherboard;
	%item[7] = $CatEquipment::Regeneration;
	%numItems = $Game::GameType == $Game::Ethernet ? 7 : 5;

	%itemname[1] = "Blaster";
	%itemname[2] = "Battle Rifle";
	%itemname[3] = "Sniper ROFL";
	%itemname[4] = "Minigun";
	%itemname[5] = $Game::GameType == $Game::Ethernet ? "Bubblegun" : "Gren. Launcher";
	%itemname[6] = "Etherboard";
	%itemname[7] = "Regeneration";

	%fixed = false;
	if($Game::GameType == $Game::mEthMatch)
		%fixed = true;

	if(%this.inventoryMode $= "showicon")
	{
		%margin = "\n\n\n\n\n\n\n\n\n\n\n\n";

		%numDiscs = 0; // %obj.numDiscs;
		%this.setHudMenuL(0, %margin, 1, 1);
		%this.setHudMenuL(1, "<bitmap:share/hud/rotc/icon.disc.png><sbreak>", %numDiscs, 1);
		for(%i = 2; %i < 10; %i++)
			%this.setHudMenuL(%i, "", 1, 0);
	}
	else if(%this.inventoryMode $= "show")
	{
		%this.setHudMenuL(0, "\n", 8, 1);
		%this.setHudMenuL(1, "<lmargin:25><font:Arial:18><tab:45,100,125>" @
         "Select Form:\n\n", 1, 1);

		%slot = 2;
//		%prefix = "<bitmap:share/hud/rotc/icon.";
//		%suffix = ".50x15> ";
//		for(%i = 1; %i <= 3; %i++)
//		{
//		   for(%j = 1; %j <= %numItems; %j++)
//		   {
//				if(%this.loadout[%i] == %item[%j])
//				{
//					%icon = %iconname[%item[%j]];
//					%this.setHudMenuL(%slot, %prefix @ %icon @ %suffix, 1, 1);
//					%slot++;
//		 	   }
//		   }
//		}
//		%this.setHudMenuL(%slot, "<sbreak><font:Arial:14>" @
//         "<bitmap:share/hud/rotc/icon.quickswitch.50x15>" @
//         "Press @bind51 to exchange\n\nLoad:\n", 1, 1);
//      %slot++;

      %tmp = "";
      
		for(%i = 0; %i <= 10; %i++)
         if(%this.loadoutName[%i] !$= "")
            %tmp = %tmp @ %this.updateLeftHudMenu1(%i) @ "\n";

      %tmp = %tmp @ "\nMaterialize FDV:\n\n";
                        
		for(%i = 50; %i <= 60; %i++)
         if(%this.loadoutName[%i] !$= "")
            %tmp = %tmp @ %this.updateLeftHudMenu1(%i) @ "\n";
            
      %tmp = %tmp @ "\n";
      
      %l = strlen(%tmp); %n = 0;
   	while(%n < %l)
	   {
		   %chunk = getSubStr(%tmp, %n, 255);
   		%this.setHudMenuL(%slot, %chunk, 1, 1);
		   %n += 255;
         %slot++;
	   }

      if(%this.loadout[1] == 0)
      {
         %this.setHudMenuL(%slot, "!!! ILLEGAL LOADOUT !!!", 1, 1);
         return;
      }

		for(%i = %slot; %i < 10; %i++)
			%this.setHudMenuL(%i, "", 1, 0);
	}
	else if(%this.inventoryMode $= "dematerialize")
	{

   }
	else if(%this.inventoryMode $= "oldshow")
	{
		for(%i = 1; %i <= 3; %i++)
			%icon[%i] = %iconname[%this.loadout[%i]];

		%this.setHudMenuL(0, "<font:Arial:12>" @ %margin, 1, 1);

		%this.setHudMenuL(1, "Slot #1:\n", 1, 1);
		%this.setHudMenuL(2, "<bitmap:share/hud/rotc/icon." @ %icon[1] @ ".50x15>", 1, 1);
		if(%fixed)
			%this.setHudMenuL(3, "<sbreak>(FIXED)", 1, 1);
		else
			%this.setHudMenuL(3, "<sbreak>(Press @bind35 to change)", 1, 1);

		%this.setHudMenuL(4, "\n\n\n\n\n\Slot #2:\n", 1, 1);
		%this.setHudMenuL(5, "<bitmap:share/hud/rotc/icon." @ %icon[2] @ ".50x15>", 1, 1);
		if(%fixed)
			%this.setHudMenuL(6, "<sbreak>(FIXED)", 1, 1);
		else
			%this.setHudMenuL(6, "<sbreak>(Press @bind36 to change)", 1, 1);

		%this.setHudMenuL(7, "\n\n\n\n\n\Slot #3:\n", 1, 1);
		%this.setHudMenuL(8, "<bitmap:share/hud/rotc/icon." @ %icon[3] @ ".50x15>", 1, 1);
		if(%fixed)
			%this.setHudMenuL(9, "<sbreak>(FIXED)", 1, 1);
		else
			%this.setHudMenuL(9, "<sbreak>(Press @bind37 to change)", 1, 1);
	}
	else if(%this.inventoryMode $= "fill")
	{
		if(%this.gameVersionString $= "p.4-testing")
			%margin = "\n\n";
		else
			%margin = "\n\n\n\n\n\n\n\n\n\n\n\n";

		%this.setHudMenuL(0, "<font:Arial:12>" @ %margin, 1, 1);
		%slot = 1;
		for(%i = 1; %i <= %numItems; %i++)
		{
			%amount = (%i == 7 ? 3 : 1);
			for(%j = 0; %j < %this.inventoryMode[1]; %j++)
			{
				if(%this.loadout[%j] == %item[%i])
					%amount--;
			}

			%text = "\n<sbreak>@bind" @ (%i < 6 ? 34 : 41) + %i @ ": " @ %itemname[%i];
			%icon = "<bitmap:share/hud/rotc/icon." @ %iconname[%item[%i]] @ ".50x15>";
			if(%amount == 1)
			{
				%this.setHudMenuL(%slot, %text @ "\n" @ " " @ %icon, 1, 1);
				%slot++;
			}
			else if(%amount > 1)
			{
				%this.setHudMenuL(%slot, %text @ "\n" @ " ", 1, 1);
				%this.setHudMenuL(%slot+1, %icon, %amount, 1);
				%slot += 2;
			}
		}

		for(%i = %slot; %i <= 9; %i++)
			%this.setHudMenuL(%i, "", 1, 0);
	}
	else if(%this.inventoryMode $= "select")
	{
		%item[1] = $CatEquipment::Blaster;
		%item[2] = $CatEquipment::BattleRifle;
		%item[3] = $CatEquipment::SniperRifle;
		%item[4] = $CatEquipment::Minigun;
		if($Game::GameType == $Game::Ethernet)
			%item[5] = $CatEquipment::RepelGun;
		else
			%item[5] = $CatEquipment::GrenadeLauncher;
		%item[6] = $CatEquipment::Etherboard;
		%item[7] = $CatEquipment::Regeneration;

		%itemname[1] = "Blaster";
		%itemname[2] = "Battle Rifle";
		%itemname[3] = "Sniper ROFL";
		%itemname[4] = "Minigun";
		%itemname[5] = $Game::GameType == $Game::Ethernet ? "Bubblegun" : "Gren. Launcher";
		%itemname[6] = "Etherboard";
		%itemname[7] = "Regeneration";

		%numItems = $Game::GameType == $Game::Ethernet ? 7 : 5;

		%this.setHudMenuL(0, "\n", 8, 1);
		%this.setHudMenuL(1, "<lmargin:100><font:Arial:12>Select slot #" @ %this.inventoryMode[1] @ ":\n\n", 1, 1);
		for(%i = 1; %i <= %numItems; %i++)
			%this.setHudMenuL(%i+1, "@bind" @ (%i < 6 ? 34 : 41) + %i @ ": " @ %itemname[%i]  @  "\n" @
				"   <bitmap:share/hud/rotc/icon." @ %iconname[%item[%i]] @ ".50x15>" @ "<sbreak>", 1, 1);
		for(%i = %numItems + 2; %i <= 9; %i++)
			%this.setHudMenuL(%i, "", 1, 0);
	}

   return;

	%i = -1;
	if($Game::GameType == $Game::Ethernet)
	{
		%this.setHudMenuR(0, "<just:right>\n\n\n\n", 1, 1);
		%i++; %icon[%i] = "damper";
		%i++; %icon[%i] = "-";
		%i++; %icon[%i] = "barrier";
		%i++; %icon[%i] = "shield";
		%i++; %icon[%i] = "-";
		%i++; %icon[%i] = "anchor";
		%i++; %icon[%i] = "vamp";
		%i++; %icon[%i] = "--";
		%i++; %icon[%i] = "explosivedisc";
		%i++; %icon[%i] = "repeldisc";
		%i++; %icon[%i] = "--";
		%i++; %icon[%i] = "bounce";
		%i++; %icon[%i] = "grenade";
		%i++; %icon[%i] = "-";
	}
	else
	{
		%this.setHudMenuR(0, "<just:right>\n\n\n\n", 1, 1);
		%i++; %icon[%i] = "slasherdisc";
		%i++; %icon[%i] = "repeldisc";
		%i++; %icon[%i] = "explosivedisc";
		%i++; %icon[%i] = "damper";
		%i++; %icon[%i] = "shield";
		%i++; %icon[%i] = "barrier";
		%i++; %icon[%i] = "vamp";
		%i++; %icon[%i] = "anchor";
		%i++; %icon[%i] = "stabilizer";
		%i++; %icon[%i] = "permaboard";
		%i++; %icon[%i] = "grenade";
		%i++; %icon[%i] = "bounce";
		%i++; %icon[%i] = "-";
	}

	if(%this.gameVersionString $= "p.4-testing")
		%margin = "";
	else
		%margin = "\n\n\n\n\n\n";

	%slot = 1;
	%text = %margin;
	%max = %i;
	for(%i = 0; %i <= %max; %i++)
	{
		%string = %icon[%i];
		if(%icon[%i] $= "-")
		{
			%text = %text @ "<sbreak>";
			%this.setHudMenuR(%slot, %text, 1, 1);
			%slot++;
			%text = "";
		}
		else if(%icon[%i] $= "--")
		{
			%text = %text @ "<sbreak><bitmap:share/hud/rotc/sep.48x5>\n";
			%this.setHudMenuR(%slot, %text, 1, 1);
			%slot++;
			%text = "";
		}
		else
		{
			%text = %text @ "<bitmap:share/hud/rotc/icon." @ %icon[%i] @ ".20x20>";
			%text = %text @ "<bitmap:share/hud/rotc/spacer.8x20>";
		}
	}
}

//-----------------------------------------------------------------------------

function GameConnection::setNewbieHelp(%this, %msg, %time)
{
	if(%msg $= "random")
	{
		%isCAT = %time; // hackety hack hack
		%msg = getRandomHint();
		%time = (%isCAT ? 60 : 0);
	}

	%this.newbieHelpText = %msg;
	%this.newbieHelpTime = %time;
	%this.newbieHelpAge = 0;

	if(this.topHudMenu $= "newbiehelp")
		%this.updateTopHudMenuThread();
}

//-----------------------------------------------------------------------------

function GameConnection::setHudMenuL(%this, %slot, %text, %repetitions, %visible)
{
	if(%slot $= "*")
	{
		for(%i = 0; %i < 10; %i++)
		{
			if(%text !$= "") %this.hudMenuLText[%i] = %text;
			if(%repetitions !$= "") %this.hudMenuLRepetitions[%i] = %repetitions;
			if(%visible !$= "") %this.hudMenuLVisible[%i] = %visible;
		}
	}
	else
	{
		if(%this.hudMenuLText[%slot] $= %text)
			%text = "";
		else
			%this.hudMenuLText[%slot] = %text;	

		if(%this.hudMenuLRepetitions[%slot] $= %repetitions)
			%repetitions= "";
		else
			%this.hudMenuLRepetitions[%slot] = %repetitions;

		if(%this.hudMenuLVisible[%slot] $= %visible)
			%visible = "";
		else
			%this.hudMenuLVisible[%slot] = %visible;	

		if(%text $= "" && %repetitions $= "" && %visible $= "")
			return;
	}

	//error("Sending 'MsgHudMenuL' to" SPC %this SPC ": [" SPC %slot SPC "][" SPC %text SPC "][" SPC %repetitions SPC "][" SPC %visible SPC "]");
	messageClient(%this, 'MsgHudMenuL', "", %slot, %text, %repetitions, %visible);
}

function GameConnection::setHudMenuR(%this, %slot, %text, %repetitions, %visible)
{
	if(%slot $= "*")
	{
		for(%i = 0; %i < 10; %i++)
		{
			if(%text !$= "") %this.hudMenuRText[%i] = %text;
			if(%repetitions !$= "") %this.hudMenuRRepetitions[%i] = %repetitions;
			if(%visible !$= "") %this.hudMenuRVisible[%i] = %visible;
		}
	}
	else
	{
		if(%this.hudMenuRText[%slot] $= %text)
			%text = "";
		else
			%this.hudMenuRText[%slot] = %text;	

		if(%this.hudMenuRRepetitions[%slot] $= %repetitions)
			%repetitions= "";
		else
			%this.hudMenuRRepetitions[%slot] = %repetitions;

		if(%this.hudMenuRVisible[%slot] $= %visible)
			%visible = "";
		else
			%this.hudMenuRVisible[%slot] = %visible;	

		if(%text $= "" && %repetitions $= "" && %visible $= "")
			return;
	}

	//error("Sending 'MsgHudMenuR' to" SPC %this SPC ": [" SPC %slot SPC "][" SPC %text SPC "][" SPC %repetitions SPC "][" SPC %visible SPC "]");
	messageClient(%this, 'MsgHudMenuR', "", %slot, %text, %repetitions, %visible);
}

function GameConnection::setHudMenuT(%this, %slot, %text, %repetitions, %visible)
{
	if(%slot $= "*")
	{
		for(%i = 0; %i < 10; %i++)
		{
			if(%text !$= "") %this.hudMenuTText[%i] = %text;
			if(%repetitions !$= "") %this.hudMenuTRepetitions[%i] = %repetitions;
			if(%visible !$= "") %this.hudMenuTVisible[%i] = %visible;
		}
	}
	else
	{
		if(%this.hudMenuTText[%slot] $= %text)
			%text = "";
		else
			%this.hudMenuTText[%slot] = %text;	

		if(%this.hudMenuTRepetitions[%slot] $= %repetitions)
			%repetitions= "";
		else
			%this.hudMenuTRepetitions[%slot] = %repetitions;

		if(%this.hudMenuTVisible[%slot] $= %visible)
			%visible = "";
		else
			%this.hudMenuTVisible[%slot] = %visible;	

		if(%text $= "" && %repetitions $= "" && %visible $= "")
			return;
	}

	//error("Sending 'MsgHudMenuT' to" SPC %this SPC ": [" SPC %slot SPC "][" SPC %text SPC "][" SPC %repetitions SPC "][" SPC %visible SPC "]");
	messageClient(%this, 'MsgHudMenuT', "", %slot, %text, %repetitions, %visible);
}

function GameConnection::setHudMenuC(%this, %slot, %text, %repetitions, %visible)
{
	if(%slot $= "*")
	{
		for(%i = 0; %i < 10; %i++)
		{
			if(%text !$= "") %this.hudMenuCText[%i] = %text;
			if(%repetitions !$= "") %this.hudMenuCRepetitions[%i] = %repetitions;
			if(%visible !$= "") %this.hudMenuCVisible[%i] = %visible;
		}
	}
	else
	{
		if(%this.hudMenuCText[%slot] $= %text)
			%text = "";
		else
			%this.hudMenuCText[%slot] = %text;

		if(%this.hudMenuCRepetitions[%slot] $= %repetitions)
			%repetitions= "";
		else
			%this.hudMenuCRepetitions[%slot] = %repetitions;

		if(%this.hudMenuCVisible[%slot] $= %visible)
			%visible = "";
		else
			%this.hudMenuCVisible[%slot] = %visible;

		if(%text $= "" && %repetitions $= "" && %visible $= "")
			return;
	}

	//error("Sending 'MsgHudMenuC' to" SPC %this SPC ": [" SPC %slot SPC "][" SPC %text SPC "][" SPC %repetitions SPC "][" SPC %visible SPC "]");
	messageClient(%this, 'MsgHudMenuC', "", %slot, %text, %repetitions, %visible);
}


//-----------------------------------------------------------------------------

function GameConnection::updateQuickbar(%this)
{
   %head = "<just:center><font:Arial:16><linkcolor:FFFFFF><linkcolorhl:FF0000>";

   %B1 = true;
   %B2 = true;
   %B3 = true;
   %B4 = true;
   %B5 = true;
   %B6 = true;
   %B7 = true;

   if(%this.loadingMission)
      %B4 = false;

   if(!%this.cookiesDone)
   {
      %B3 = false;
      %B5 = false;
   }

   %msk = "             ";
   %spc = "  ";
   %tmp = "" @
      (%B1?"<B:1:cmd MainMenu>":"") @ %msk @ (%B1?"</b>":"") @ %spc @
      (%B2?"<B:2:cmd ShowPlayerList>":"") @ %msk @ (%B2?"</b>":"") @ %spc @
      (%B3?"<B:3:cmd Loadout>":"") @ %msk @ (%B3?"</b>":"") @ %spc @
      (%B4?"<B:4:cmd Teams>":"") @ %msk @ (%B4?"</b>":"") @ %spc @
      (%B5?"<B:5:cmd ShowSettings>":"") @ %msk @ (%B5?"</b>":"") @ %spc @
      (%B6?"<B:6:cmd Admin>":"") @ %msk @ (%B6?"</b>":"") @ %spc @
      %msk @ %spc @
      (%B7?"<B:7:cmd Help 0>":"") @ %msk @ (%B7?"</b>":"") @
      "\n";

   %B1Link1 = (%B1?"<B:1:cmd MainMenu>":"") @ "Server Info" @ (%B1?"</b>":"");
   %B2Link1 = (%B2?"<B:2:cmd ShowPlayerList>":"") @ "Player List" @ (%B2?"</b>":"");
   %B3Link1 = (%B4?"<B:4:cmd Teams>":"") @ "Switch Team" @ (%B4?"</b>":"");
   %B4Link1 = (%B7?"<B:7:cmd Manual 0>":"") @ "Manual" @ (%B7?"</b>":"");

   %text = "\n"
      @ %B1Link1 SPC "|" SPC %B2Link1 SPC "|"
      SPC %B3Link1 SPC "|" SPC %B4Link1 @ "\n";

	%this.beginQuickbarText();
	%this.addQuickbarText(%head, 1);
	%this.addQuickbarText(%text, 1);
	%this.endQuickbarText();

   return;






















   %head = "<just:center><font:Arial:16><linkcolor:FFFFFF><linkcolorhl:FF0000>";

   %B1 = true;
   %B2 = true;
   %B3 = true;
   %B4 = true;
   %B5 = true;
   %B6 = true;
   %B7 = true;

   if(%this.loadingMission)
      %B4 = false;

   if(!%this.cookiesDone)
   {
      %B3 = false;
      %B5 = false;
   }

   %msk = "             ";
   %spc = "  ";
   %tmp = "" @
      (%B1?"<B:1:cmd MainMenu>":"") @ %msk @ (%B1?"</b>":"") @ %spc @
      (%B2?"<B:2:cmd ShowPlayerList>":"") @ %msk @ (%B2?"</b>":"") @ %spc @
      (%B3?"<B:3:cmd Loadout>":"") @ %msk @ (%B3?"</b>":"") @ %spc @
      (%B4?"<B:4:cmd Teams>":"") @ %msk @ (%B4?"</b>":"") @ %spc @
      (%B5?"<B:5:cmd ShowSettings>":"") @ %msk @ (%B5?"</b>":"") @ %spc @
      (%B6?"<B:6:cmd Admin>":"") @ %msk @ (%B6?"</b>":"") @ %spc @
      %msk @ %spc @
      (%B7?"<B:7:cmd Help 0>":"") @ %msk @ (%B7?"</b>":"") @
      "\n";

   %B1Link1 = (%B1?"<B:1:cmd MainMenu>":"") @ "Arena" @ (%B1?"</b>":"");
   %B1Link2 = (%B1?"<B:1:cmd MainMenu>":"") @ "Info" @ (%B1?"</b>":"");
   %B2Link1 = (%B2?"<B:2:cmd ShowPlayerList>":"") @ "Player" @ (%B2?"</b>":"");
   %B2Link2 = (%B2?"<B:2:cmd ShowPlayerList>":"") @ "List" @ (%B2?"</b>":"");
   %B3Link1 = (%B3?"<B:3:cmd Loadout>":"") @ "Edit" @ (%B3?"</b>":"");
   %B3Link2 = (%B3?"<B:3:cmd Loadout>":"") @ "Loadouts" @ (%B3?"</b>":"");
   %B4Link1 = (%B4?"<B:4:cmd Teams>":"") @ "Switch" @ (%B4?"</b>":"");
   %B4Link2 = (%B4?"<B:4:cmd Teams>":"") @ "Team" @ (%B4?"</b>":"");
   %B5Link1 = (%B5?"<B:5:cmd ShowSettings>":"") @ "Game" @ (%B5?"</b>":"");
   %B5Link2 = (%B5?"<B:5:cmd ShowSettings>":"") @ "Settings" @ (%B5?"</b>":"");
   %B6Link1 = (%B6?"<B:6:cmd Admin>":"") @ "Arena" @ (%B6?"</b>":"");
   %B6Link2 = (%B6?"<B:6:cmd Admin>":"") @ "Admin" @ (%B6?"</b>":"");
   %B7Link1 = (%B7?"<B:7:cmd Help>":"") @ "Help" @ (%B7?"</b>":"");

   %tabsFG = %tmp @ %tmp @ %tmp @
      "  "@%B1Link1@"      " @
      %B2Link1@"       " @
      %B3Link1@"        " @
      %B4Link1@"  " @
      " " @ // center of line
      "   "@%B5Link1@"       " @
      %B6Link1@"         " @
      "             " @
      %B7Link1@"   " @
      "\n" @
      %B1Link2@"          " @
      %B2Link2@"     " @
      %B3Link2@"     " @
      %B4Link2@"   " @
      "" @ // center of line
      "  "@%B5Link2@"    " @
      %B6Link2@"                           " @
      "";

   %tabsBG = "<bitmap:share/ui/rotc/qbbg>";

	%this.beginQuickbarText();
	%this.addQuickbarText(%head, 3);
	%this.addQuickbarText(%tabsFG, 1);
	%this.addQuickbarText(%tabsBG, 2);

	%this.endQuickbarText();
}

//-----------------------------------------------------------------------------

function GameConnection::setHandicap(%this, %handicap)
{
	if(%handicap $= "")
		%this.handicap = 1;
	else if(0 <= %handicap && %handicap <= 1)
		%this.handicap = %handicap;
	else
		%this.handicap = 1;
		
	if(isObject(%this.player))
		%this.player.getDataBlock().updateShapeName(%this.player);
}

//-----------------------------------------------------------------------------

function GameConnection::getEtherformDataBlock(%this)
{
   return FrmLight;
}



