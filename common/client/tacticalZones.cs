//------------------------------------------------------------------------------
// Alux
// Copyright (C) 2013 Michael Goldener <mg@wasted.ch>
//------------------------------------------------------------------------------

function computeZoneGrids(%progressCallback)
{
    %zones = new SimSet(TempTacticalZonesSet);
    
    for(%idx = 0; %idx < ServerConnection.getCount(); %idx++)
    {
		%obj = ServerConnection.getObject(%idx);
        if(%obj.getClassName() $= "TacticalZone")
            %zones.add(%obj);
    }
    
    for(%idx = 0; %idx < %zones.getCount(); %idx++)
    {
     	echo("Computing grid for zone " @ %idx+1 @ " / " @ %zones.getCount());
		%zone = %zones.getObject(%idx);
        %zone.computeGrid();
        if(%progressCallback !$= "")
            call(%progressCallback, %idx / %zones.getCount());
    }
    
    %zones.clear();
    %zones.delete();
}
