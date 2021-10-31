# InfiniteControls
Basic controls/UI for game design in XNA/MonoGame

These controls are designed to be simple and effective, as opposit to some other like NeoControls or EmptyKeys for example for which authors target a full GUI implementation, like the one offered by OS or window managers on Unix/Linux which are far too complex, not very suited for how XNA/MonoGame has been designed in the first place and which have a use only for a very few games like strategy games full of resizable windows.

Think of them more like what HTML4 (or even 3) offered to CGI designers, as most of the games need just a menu, a few buttons, checkboxes and sometimes dropdown list for settings, plus some cute progress bar, nothing more.

The sets contains a simple button class, a checkbox class (derived from the button), a more advanced button, with time and cooldown feature and a volume selector (the usual UI element to choose sound fx/music volume).
I plan to add also a dropdown list, mostly for choosing game resolution and most probably radio buttons group and menu which would handle both simple strings and pictures for items. Without forgetting the must-have progress bar with multiple feature to display or not progress/resource on or near the bar.

They're will be free to use as much as you want and need :).

Current status:
- buttons (simple, timed and checkbox) are working
- volume control need some more work
- progress bar is WIP, will be ready soon
- cleaning TBD
- radio button TBD
- menu need to be improved and cleaned a (big) little bit
- menu also needs picture/texture management (spritefont is very limited for now, especially as long as te issue regarding styles is not fixed)
- examples and documentation TBD
