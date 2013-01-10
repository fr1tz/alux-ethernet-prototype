//------------------------------------------------------------------------------
// Alux
// Copyright (C) 2013 Michael Goldener <mg@wasted.ch>
//------------------------------------------------------------------------------

function AboutWindow::onAddedAsWindow(%this)
{
	AboutFileList.entryCount = 0;
	AboutFileList.clear();

	%files[0] = "README";
	%files[1] = "NEWS";
	%files[2] = "AUTHORS";
	%files[3] = "COPYING";

	for(%i = 0; %i < 4; %i++)
	{
		%file = %files[%i];
		AboutFileList.fileName[AboutFileList.entryCount] = %file;
		AboutFileList.addRow(AboutFileList.entryCount, %file);
		AboutFileList.entryCount++;
	}
	AboutFileList.setSelectedRow(0);
}

function AboutFileList::onSelect(%this, %row)
{
	%fo = new FileObject();
	%fo.openForRead(%this.fileName[%row]);
	%text = "";
	while(!%fo.isEOF())
		%text = %text @ %fo.readLine() @ "\n";

	%fo.delete();

	AboutText.setText("<font:NovaSquare:14>" @ %text);
}

