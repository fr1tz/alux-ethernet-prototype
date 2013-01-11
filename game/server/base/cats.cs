//------------------------------------------------------------------------------
// Alux
// Copyright (C) 2013 Michael Goldener <mg@wasted.ch>
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
// Revenge Of The Cats - cats.cs
// Code for all CATs
//------------------------------------------------------------------------------

function executeCatScripts()
{
	echo(" ----- executing cat scripts ----- ");

	%i = 1;
	// Weapons...
	$CatEquipment::Blaster         = %i; %i++;
	$CatEquipment::BattleRifle     = %i; %i++;
	$CatEquipment::SniperRifle     = %i; %i++;
	$CatEquipment::MiniGun         = %i; %i++;
	$CatEquipment::RepelGun        = %i; %i++;
	$CatEquipment::GrenadeLauncher = %i; %i++;
	$CatEquipment::Python          = $CatEquipment::BattleRifle;
	$CatEquipment::Trident         = $CatEquipment::Blaster;
	$CatEquipment::Raptor          = $CatEquipment::RepelGun;
	$CatEquipment::Laserhawk       = $CatEquipment::SniperRifle;
	// Discs...
	$CatEquipment::SlasherDisc     = %i; %i++;
	$CatEquipment::RepelDisc       = %i; %i++;
	$CatEquipment::ExplosiveDisc   = %i; %i++;
	// Other...
	$CatEquipment::Damper          = %i; %i++;
	$CatEquipment::VAMP            = %i; %i++;
	$CatEquipment::Anchor          = %i; %i++;
	$CatEquipment::Stabilizer      = %i; %i++;
	$CatEquipment::Grenade         = %i; %i++;
	$CatEquipment::Bounce          = %i; %i++;
	$CatEquipment::Etherboard      = %i; %i++;
	$CatEquipment::Permaboard      = %i; %i++;
	$CatEquipment::Regeneration    = %i; %i++;
	
	exec("./cats.sfx.cs");
	exec("./cats.gfx.cs");

	// partytime!
	exec("./cats/partysounds.cs");
	exec("./cats/partyanims.cs");
}

executeCatScripts();

//------------------------------------------------------------------------------

function StandardCat::useWeapon(%this, %obj, %nr)
{
	%client = %obj.client;

	if(%nr == 4)
	{
		dropGreen(%obj);
		return;
	}

   if(%nr == -17)
   {
		if(%client.hasBounce)
			deployRepel3(%obj);
      return;
   }

	// discs...
	if($Game::GameType == $Game::Ethernet)
	{
		if(%nr == 6)
		{
			launchExplosiveDisc(%obj);
			return;
		}
	}
	
	if(%client.numWeapons == 0)
		return;

	if(%nr > %client.numWeapons)
		return;

	if(%nr == 0)
		%obj.currWeapon++;
	else
		%obj.currWeapon = %nr;
	
	if(%obj.currWeapon > %client.numWeapons)
		%obj.currWeapon = 1;	

	%wpn = %client.weapons[%obj.currWeapon-1];

	if(%wpn == $CatEquipment::Python)
	{
		%obj.mountImage(WpnBadgerImage, 0, -1, true);
	}
	else if(%wpn == $CatEquipment::MiniGun)
	{
		%obj.mountImage(WpnMinihawkImage, 0, -1, true);
	}
	else if(%wpn == $CatEquipment::Trident)
	{
		%obj.mountImage(WpnStyckImage, 0, -1, true);
	}
	else if(%wpn == $CatEquipment::Raptor)
	{
		%obj.mountImage(WpnRaptorImage, 0, -1, true);
	}
	else if(%wpn == $CatEquipment::Laserhawk)
	{
      %obj.mountImage(WpnLaserhawkImage, 0, -1, true);
	}
}

