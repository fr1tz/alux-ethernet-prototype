//------------------------------------------------------------------------------
// Alux Prototype
// Copyright notices are in the file named COPYING.
//------------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Torque Game Engine 
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

//-----
// commands.cs
// Server commands available to clients
//-----

function serverCmdPlayerAction(%client, %nr, %val)
{	
	%player = %client.player;
	if(%player == 0)
		return false;

	if(%nr <= 9 && %val)
	{
		%client.getControlObject().useWeapon(%nr);
	}
	else if(%nr == 10 && %val && %client.menuVisible == false)
	{
      %client.leaveForm(%player, true);
	}
	else if(%nr == 11 && %val && %client.menuVisible == false)
	{
      if(%client.player.getClassName() $= "Etherform")
         %client.enterForm();
      else
         %client.leaveForm(%player, false);
	}
	else if(%nr == 12 && %val)
	{
      %client.getControlObject().useWeapon(0);
	}
	else if(%nr == 13 && %val)
	{
      %form = %client.player;
      if(!isObject(%form))
         return;
      if(%form.isReloading)
         return;
      if(!%form.getDataBlock().isMethod("reload"))
         continue;
      %form.getDataBlock().reload(%form);
	}
	else if(%nr == 17 && %val)
	{		
      //%client.spawnForm();
		//%client.getControlObject().useWeapon(-17);
	}
	else if(%nr >= 21 && %nr <= 30 && %val)
	{
		%client.getControlObject().useWeapon(%nr-20);
	}
	else if(%nr >= 31 && %nr <= 35 && %val)
	{
		%client.getControlObject().special(%nr-30);
	}
	else if(%nr == 39 && %val)
	{		
		serverCmdToggleCamera(%client);
	}
}

//-----------------------------------------------------------------------------
// Inventory server commands
//-----------------------------------------------------------------------------

function serverCmdUseInv(%client, %data)
{
	if( %client.player == 0 ) return false;
	%client.getControlObject().use(%data);
}

function serverCmdUseWeapon(%client, %nr)
{
	if( %client.player == 0 ) return false;
	%client.getControlObject().useWeapon(%nr);
}

function serverCmdUseNextWeapon(%client)
{
	if( %client.player == 0 ) return false;
	%image = %client.player.getDataBlock().weapon[%client.currWeapon+1];
	if( isObject(%image) )
	{
		%client.player.mountWeaponImage(%image);
		%client.currWeapon++;
	}
}

function serverCmdUsePrevWeapon(%client)
{
	if( %client.player == 0 ) return false;
	%image = %client.player.getDataBlock().weapon[%client.currWeapon-1];
	if( isObject(%image) && %client.currWeapon-1 != 0 )
	{
		%client.player.mountWeaponImage(%image);
		%client.currWeapon--;
	}
}

//-----------------------------------------------------------------------------

function serverCmdToggleCamera(%client)
{
	if ($Server::TestCheats || $Server::ServerType $= "SinglePlayer")
	{
		%control = %client.getControlObject();
		if (%control == %client.player)
		{
			%control = %client.camera;
			%control.mode = toggleCameraFly;
		}
		else
		{
			%control = %client.player;
			%control.mode = observerFly;
		}
		%client.setControlObject(%control);
	}
}

function serverCmdDropPlayerAtCamera(%client)
{
	if ($Server::TestCheats)
	{
		if (!%client.player.isMounted())
			%client.player.setTransform(%client.camera.getTransform());
		%client.player.setVelocity("0 0 0");
		%client.setControlObject(%client.player);
	}
}

function serverCmdDropCameraAtPlayer(%client)
{
	if ($Server::TestCheats)
	{
		%client.camera.setTransform(%client.player.getEyeTransform());
		%client.camera.setVelocity("0 0 0");
		%client.setControlObject(%client.camera);
	}
}

//-----------------------------------------------------------------------------

function serverCmdJoinTeam(%client, %teamId)
{
	%client.joinTeam(%teamId);
}

function serverCmdSuicide(%client)
{
	if (isObject(%client.player))
		%client.player.kill("Suicide");
}	

function serverCmdPlayParty(%client,%anim)
{
	if (isObject(%client.player))
		%client.player.playPartyAnimation(%anim);
}

function serverCmdPlayDeath(%client)
{
	if (isObject(%client.player))
		%client.player.playDeathAnimation();
}

function serverCmdSelectObject(%client, %mouseVec, %cameraPoint)
{
	//Determine how far should the picking ray extend into the world?
	%selectRange = 200;
	// scale mouseVec to the range the player is able to select with mouse
	%mouseScaled = VectorScale(%mouseVec, %selectRange);
	// cameraPoint = the world position of the camera
	// rangeEnd = camera point + length of selectable range
	%rangeEnd = VectorAdd(%cameraPoint, %mouseScaled);

	// Search for anything that is selectable � below are some examples
	%searchMasks = $TypeMasks::PlayerObjectType | $TypeMasks::CorpseObjectType |
						$TypeMasks::ItemObjectType | $TypeMasks::TriggerObjectType;

	// Search for objects within the range that fit the masks above
	// If we are in first person mode, we make sure player is not selectable by setting fourth parameter (exempt
	// from collisions) when calling ContainerRayCast
	%player = %client.player;
	if ($firstPerson)
	{
	  %scanTarg = ContainerRayCast (%cameraPoint, %rangeEnd, %searchMasks, %player);
	}
	else //3rd person - player is selectable in this case
	{
	  %scanTarg = ContainerRayCast (%cameraPoint, %rangeEnd, %searchMasks);
	}

	// a target in range was found so select it
	if (%scanTarg)
	{
		%targetObject = firstWord(%scanTarg);

		%client.setSelectedObj(%targetObject);
	}
}

function serverCmdInstantGrenadeThrow(%client)
{
    %player = %client.player;
	if(!isObject(%player))
        return;

    %player.doInstantGrenadeThrow = true;
    %player.setImageTrigger(2, true);
    %player.setImageTrigger(2, false);
}

//-----------------------------------------------------------------------------

function serverCmdMenuVisible(%client, %visible)
{
  %client.menuVisible = %visible;
}

//-----------------------------------------------------------------------------

