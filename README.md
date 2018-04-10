# ComputerPlus
**LSPDFR Computer+ aims to recreate the old LCPDFR computer with some extra features.**

![banner](http://i.imgur.com/Ihf8uiE.png)

**Authors:** PieRGud, Stealth22, AlconH, Albo1125, fiskey111, ainesophaur, BejoIjo

**Current Features**
- Ability to search for persons and vehicles without keys conflicting with other mods.
- Uses the LSPDFR API to get ped and vehicle information. Also includes info left out of the regular LSPDFR police computer, like times stopped and number of citations.
- Uses the Traffic Policer API (if it is installed) to also show a vehicle's insurance information.
- Search fields are automatically filled out with the subject's information during a traffic stop, so all you have to do is press a button!
- Dynamic backgrounds! Depending on which police vehicle you're in, you'll get a specialized department background. Comes with RDE support.
- Looks practically identical to the LCPDFR computer (in terms of the layout).
- Active Calls screen, which shows all callouts sent by Dispatch, even the ones that AI units respond to! (Must have participating callout plugins installed for this to work)
- Computer+ now has its own API, for callout developers to use!!
- [NEW] Computer+ can now integrate UI interfaces from third party apps through API. Check the ExampleExternalUI sample under API Examples
- [NEW] Computer+ can now hide the background and unpause the game while the computer is active
- [NEW] Completely overhauled UI for Ped and Vehicle searches
- [NEW] Computer+ integrates with ALPR+ and can show ALPR alerts in the vehicle details view
- [NEW] Computer+ will now contain a list of recent searches either populated manually or via automatic searching
- [NEW] Computer+ will automatically add ALPR hit vehicles to the recent vehicle search list
- [NEW] Computer+ will automatically add Peds who have been stopped by the player to the collected ids ped search list
- [NEW] Computer+ will automatically add LSPDFR+ court case when you create an arrest resport
- [NEW] Computer+ now have ability to create citation for stopped ped on foot.
- [NEW] Computer+ now can be opened from a tablet while the player is on foot

**Troubleshooting**
- If your mouse does not appear in Computer+, then make sure the display resolution in game matches your windows display resolution. If the resolutions do not match, you will not see a mouse cursor
- If you play in windows or windows borderless mode, make sure to keep your mouse inside of the game window until the game fully loads

**Planned Improvements**
- Integration with Albo1125's British Policing Script.
- Combining Computer+ and fiskey111's LSPDFR PolicingMDT into one mod.

**Installation**
Place ComputerPlus.dll, ComputerPlus.ini and the ComputerPlus folder in Plugins\LSPDFR\.
LSPDFR will automatically load Computer+ when you go on duty.

**Usage**
While in a stationary police vehicle, hold E or the Right D-Pad button to bring up Computer+.

**Configuration**
If you open the ComputerPlus.ini file, you will notice a few things.
You can change the username and/or password that is displayed on the login screen to anything you want.
You can also adjust which backgrounds are shown for which vehicles.
To give an additional police vehicle a background, simply add another entry in the format vehiclename:bgfile.jpg
ENSURE THAT THE VEHICLE YOU'RE ADDING IS A POLICE VEHICLE AND THE BACKGROUND FILE IS IN .JPG FORMAT.

**Additional Info**
Computer+ DOES NOT replace the default LSPDFR computer.
So, if you don't always want to use Computer+ during your patrols, then you need not worry - the default computer is still there.


**Improvements on Computer+ Reloaded** (contributed by BejoIjo)
- replaced SQLite with LiteDB to make it more lightweight and stable
- now previous arrest report & citation will be displayed when viewing ped/vehicle info (without crashing)
- generate random past traffic citations & arrest reports for ped to improve gameplay
- reactivated traffic citation history
- reactivated arrest report history
- reactivated most recent action list on main window
- added wanted reason info when ped is wanted
- added driver license expiration days info (random)
- made ped & vehicle persistent during C+ session to avoid crashes (they will be freed when Computer+ exit)
- saving newly created arrest report or citation will also close the window for better UX
- vehicle type will be showed correctly when viewing the citation history records
- fixed "give ticket" animation. now the ticket should disappear in the right time
- fixed incorrect time data source on arrest report view
- added various checking and handling to prevent potential crashes.


** Version 1.3.6.3 change list (contributed by BejoIjo)
- new feature to create citation for stopped ped on foot
- added compatibility to the peds stopped by Stop The Ped plugin
- fixed bug on creating citation while the suspect is in player car back seat, the car model now will show "N/A"
- reduced frequent crashes when opening ped info or vehicle info
- added configuration option to enable/disable ped and vehicle images (only disable if you're still having crashes)

** Version 1.3.6.4 change list (contributed by BejoIjo)
- added gun permit information on ped info window. (compatible with Stop The Ped plugin)
- added feature to create court case for any citation given to the ped (LSPDFR+ plugin is needed)
- added color (red/greed) on flagged ped/vehicle data (e.g. license status, insurance status)
- fixed  truncated string on random history charge information
- reduced crash possibility by tweaking some components

** Version 1.3.6.5 change list (contributed by BejoIjo)
- Added stolen flag on vehicle info window
- Added keyboard shortcut to open Computer+ using tablet while player is on foot. (default: LControl + NumPad0)
- Added checkbox to toggle create court case for the citation (Notice to Appear) on the citation form
- Enlarged citation selection list for better UX
- Replace citation ticket 3D model given to the ped with more suitable one
- Fixed bug when driver takes off after being given the citation without waiting player return to the car. (during traffic stop)