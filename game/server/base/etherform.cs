//------------------------------------------------------------------------------
// Alux Prototype
// Copyright notices are in the file named COPYING.
//------------------------------------------------------------------------------

exec("./etherform.sfx.cs");
exec("./etherform.gfx.cs");

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
	%obj.playAudio(1, EtherformSingSound);
 
   %obj.updateVisuals();

	// Make sure grenade ammo bar is not visible...
	messageClient(%obj.client, 'MsgGrenadeAmmo', "", 1);

	// lights...
	if(%obj.getTeamId() == 1)
		%obj.mountImage(RedEtherformLightImage, 3);
	else
		%obj.mountImage(BlueEtherformLightImage, 3);

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

function Etherform::updateVisuals(%this)
{
   %client = %this.client;
   if(!isObject(%client))
      return;
      
   %used = %client.inventory.pieceUsed[0];
   %free = %client.inventory.pieceCount[0] - %used;
   
   %this.setDamageBufferLevel(%free > 0 ? 200 : 0);
}

