//------------------------------------------------------------------------------
// Alux
// Copyright (C) 2013 Michael Goldener <mg@wasted.ch>
//------------------------------------------------------------------------------

function cat_to_orb(%player)
{
   %client = %player.client;
	%tagged = %player.isTagged();
	%pos = %player.getPosition();

   if( %client.team == $Team1 )
      %data = RedInfantryCatOrb;
   else
      %data = BlueInfantryCatOrb;

   %player.setDataBlock(%data);
   %player.setBodyPose($PlayerBodyPose::Sliding);

}

function orb_to_cat(%player)
{



}

// callback function: called by engine
function Orb::onTrigger(%this, %obj, %triggerNum, %val)
{
	if(!%obj.client)
		return;

   if($Game::GameType == $Game::Infantry)
   {
      if(%triggerNum == 1 && !%val)
      {
         orb_to_cat(%obj);
         return;
      }

      if(%triggerNum == 5 && !%val)
      {
         orb_to_cat(%obj);
         return;
      }
   }
}




