//------------------------------------------------------------------------------
// Alux Prototype
// Copyright notices are in the file named COPYING.
//------------------------------------------------------------------------------

// *** callback function: called by engine
function GameBaseData::onRemove(%this, %obj)
{
   if(%obj.loadoutcode && %obj.client)
   {
      %pieces = sLoadoutcode2Pieces(%obj.loadoutcode);
      for(%f = 0; %f < getFieldCount(%pieces); %f++)
      {
         %field = getField(%pieces, %f);
         %piece = getWord(%field, 0);
         %count = getWord(%field, 1);
         %obj.client.inventory.pieceUsed[%piece] -= %count;
         if(%obj.zFormDestroyed)
            %obj.client.inventory.pieceCount[%piece] -= %count;
      }
   }
}

// Called by script code
function GameBase::setLoadoutCode(%this, %loadoutcode)
{
   %client = %this.client;
   %this.loadoutcode = %loadoutcode;
   %pieces = sLoadoutcode2Pieces(%this.loadoutcode);
   for(%f = 0; %f < getFieldCount(%pieces); %f++)
   {
      %field = getField(%pieces, %f);
      %piece = getWord(%field, 0);
      %count = getWord(%field, 1);
      %client.inventory.pieceUsed[%piece] += %count;
   }
}

// Called by script code
function GameBase::dematerializeFinish(%this)
{
   %this.getDataBlock().dematerializeFinish(%this);
}
