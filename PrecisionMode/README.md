# Precision Mode
This mod adds a Precision Mode modifier to the game featuring:
* A smaller hit window (100ms compared to the standard 140ms)
* Smaller leniency timings
* Anti Ghosting

## Leniency Timings
Clone Hero has 3 Leniency Timers. Here's what they do and what their values have been changed to:
* Strum Leniency (Default: 84ms. Now: 28ms) - Strum Leniency allows a player to hit a note when they strummed before holding the correct fret (Strum first, fret later)
* Strum Leniency No Notes (Default: 48ms. Now: 16ms) - Same as normal Strum Leniency, except this one is used when no notes are currently in the timing window.
* HOPO Leniency (Default: 96ms. Now: 32ms) - HOPO Leniency allows the player to fret off of the note, but if the note reaches the timing window within this time, the note will still hit.

Currently, all the timings are 1/3 of their original values. This may change whether or not it is too easy or hard.

# Note
The timing values, or the Anti Ghosting logic may change across versions depending on how easy/hard this mod is. 
Anti Ghosting is still very much a work in progress so a lot of changes will be made to that over the next few releases. 

I also intend to add a shrinking timing window, much like Rock Band includes (but not to the extent of Rock Band's shrinking)
