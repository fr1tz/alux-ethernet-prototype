//------------------------------------------------------------------------------
// Alux Prototype
// Copyright notices are in the file named COPYING.
//------------------------------------------------------------------------------

//----------------------------------------

function JoinServerWindow::query(%this)
{
	hilightControl(JS_queryMaster, false);

	JS_RefreshServer.setActive(false);
	JS_JoinServer.setActive(false);

	queryMasterServer(
		0,			 // Query flags
		$Client::GameTypeQuery,		 // gameTypes
		$Client::MissionTypeQuery,	 // missionType
		0,			 // minPlayers
		100,		  // maxPlayers
		0,			 // maxBots
		2,			 // regionMask
		0,			 // maxPing
		100,		  // minCPU
		0			  // filterFlags
	);
}

//----------------------------------------

function JoinServerWindow::queryLan(%this)
{
	hilightControl(JS_queryMaster, false);

	JS_RefreshServer.setActive(false);
	JS_JoinServer.setActive(false);

	queryLANServers(
		29000,		// lanPort for local queries
		0,			 // Query flags
		$Client::GameTypeQuery,		 // gameTypes
		$Client::MissionTypeQuery,	 // missionType
		0,			 // minPlayers
		100,		  // maxPlayers
		0,			 // maxBots
		2,			 // regionMask
		0,			 // maxPing
		100,		  // minCPU
		0			  // filterFlags
	);
}

//----------------------------------------

function onServerQueryStatus(%status, %msg, %value)
{
	echo("ServerQuery: " SPC %status SPC %msg SPC %value);
	// Update query status
	// States: start, update, ping, query, done
	// value = % (0-1) done for ping and query states
	if (!JS_queryStatus.isVisible())
		JS_queryStatus.setVisible(true);

	switch$ (%status) {
		case "start":
			JS_joinServer.setActive(false);
			JS_queryLan.setActive(false);
			JS_queryMaster.setActive(false);
			JS_statusText.setText(%msg);
			JS_statusBar.setValue(0);
			JS_serverList.clear();

		case "ping":
			JS_statusText.setText("Ping Servers");
			JS_statusBar.setValue(%value);

		case "query":
			JS_statusText.setText("Query Servers");
			JS_statusBar.setValue(%value);

		case "done":
			JS_queryLan.setActive(true);
			JS_queryMaster.setActive(true);
			JS_queryStatus.setVisible(false);
			JS_status.setText(%msg);
			JoinServerWindow.update();
	}
}

//----------------------------------------

function JS_queryMaster::onAdd(%this)
{
	hilightControl(%this, true);
}

//----------------------------------------

function JoinServerWindow::onAddedAsWindow()
{
	//
}

function JoinServerWindow::onRemovedAsWindow()
{
	//
}

//----------------------------------------

function JoinServerWindow::onWake()
{
	JS_HeaderList.setRowById(0,
		"Arena" TAB
		"Ping" TAB
		"Players" TAB
		"Game" TAB
		"Environment" TAB
		"Server Index" // <- This will never be visible
	);
	JS_HeaderList.setActive(false);
	
	%haveServer = (getServerCount() > 0);
	JS_RefreshServer.setActive(%haveServer);
	JS_JoinServer.setActive(%haveServer);
}

//----------------------------------------

function JoinServerWindow::cancel(%this)
{
	cancelServerQuery();
	JS_queryStatus.setVisible(false);
	JS_queryLan.setActive(true);
	JS_queryMaster.setActive(true);
}


//----------------------------------------

function JoinServerWindow::join(%this)
{
	cancelServerQuery();
	%id = JS_ServerList.getSelectedId();

	// The server info index is stored in the row along with the
	// rest of displayed info.
	%index = getField(JS_ServerList.getRowTextById(%id), 5);
	if (setServerInfo(%index)) {
		disconnect();
		%conn = new GameConnection(ServerConnection);
		%conn.setConnectArgs($GameNameString, $GameVersionString, $Pref::Player::Name);
		%conn.setJoinPassword($Client::Password);
		%conn.connect($ServerInfo::Address);
		onConnectionInitiated();
	}
}

//----------------------------------------

function JoinServerWindow::refreshServer(%this)
{
	cancelServerQuery();
	%id = JS_ServerList.getSelectedId();

	// The server info index is stored in the row along with the
	// rest of displayed info.
	%serverindex = getField(JS_serverList.getRowTextById(%id), 5);
	if(setServerInfo(%serverindex)) {
		querySingleServer( $ServerInfo::Address, 0 );
	}
}

//----------------------------------------

function JoinServerWindow::exit(%this)
{
	cancelServerQuery();
	removeWindow(JoinServerWindow);
}

//----------------------------------------
function JoinServerWindow::update(%this)
{
	// Copy the servers into the server list.
	
	JS_queryStatus.setVisible(false);
	JS_ServerList.clear();
	
	%sc = getServerCount();
	for (%i = 0; %i < %sc; %i++) {
		setServerInfo(%i);

		%npos = strstr($ServerInfo::Info, "\n");
		if(%npos > 0)
			%shortInfo = getSubStr($ServerInfo::Info, 0, %npos);
		else
			%shortInfo = "(no summary)";

		JS_ServerList.addRow(%i,
			$ServerInfo::Name TAB
			$ServerInfo::Ping TAB
			$ServerInfo::PlayerCount @ "/" @ $ServerInfo::MaxPlayers TAB
			$ServerInfo::MissionType TAB
         $ServerInfo::MissionName TAB
			%i);  // ServerInfo index stored also
	}
 
    JS_MapHomepage.setText("-");
    JS_ServerInfo.setText("");
	
	JS_ServerList.sort(1, true);
	JS_ServerList.setSelectedRow(0);
	JS_ServerList.scrollVisible(0);
}

//----------------------------------------

function JS_ServerListScroll::onScroll(%this, %x, %y)
{
	JS_HeaderListScroll.setScrollPosition(%x, %y);
}

//----------------------------------------

function JS_ServerList::onSelect(%this, %id, %text)
{
	// The server info index is stored in the row along with the
	// rest of displayed info.
	%serverindex = getField(JS_ServerList.getRowTextById(%id), 5);
	
	if(setServerInfo(%serverindex))
	{
        JS_ServerInfo.setText($ServerInfo::Info);
		JS_RefreshServer.setActive(true);
		JS_JoinServer.setActive(true);
	}
}


//----------------------------------------

