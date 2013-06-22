//------------------------------------------------------------------------------
// Alux Ethernet Prototype
// Copyright notices are in the file named COPYING.
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
 
//	if(%client.numWeapons == 0)
//		return;

//	if(%nr > %client.numWeapons)
//		return;

	if(%nr == 0)
   {
      if(%obj.currWeaponSlot == 1)
         %obj.currWeaponSlot = 2;
      else
         %obj.currWeaponSlot = 1;
   }
   else
      %obj.currWeaponSlot = %nr;

   %wpn = "";
   if(%obj.currWeaponSlot == 1)
   	%wpn = getWord(%obj.loadoutcode, 4);
   if(%obj.currWeaponSlot == 2)
   	%wpn = getWord(%obj.loadoutcode, 3);
    
   if(%wpn !$= "")
      %obj.mountImage($Server::Game.weapon[%wpn], 0, -1, true);
}

