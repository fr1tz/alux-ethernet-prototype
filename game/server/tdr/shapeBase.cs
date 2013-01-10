//------------------------------------------------------------------------------
// Alux
// Copyright (C) 2013 Michael Goldener <mg@wasted.ch>
//------------------------------------------------------------------------------

// script function called by team drag race zone code
function ShapeBaseData::updateRacingLaneZone(%this, %obj, %enterZone, %leftZone)
{
	//error("updateRacingLaneZone():" SPC %obj.getDataBlock().getName());
	%this.updateZone(%obj, %enterZone, %leftZone);
	if(!%obj.zInOwnZone)
	{
		//echo(" not in own zone");
        %this.onLeaveMissionArea(%obj);  
   }
}
