# [This project has been moved to Github](https://github.com/pleonex/tinke) #

**Note:** Current version 0.8.2 has a lot of bugs. New version 0.9.0 will be ready as soon as possible, meanwhile I recommend to use alpha version [Tinke rev153.zip](http://www.mediafire.com/download/zu8uu2iw7piipt9/Tinke_rev153.zip) (05/31/2013)

> Tinke is a program to see, convert, and edit the **files of NDS games**. You can see a lot of format files like images, text, sounds, fonts and textures. Furthermore it works with **plugins** made in NET Framework languages (C# and VB.NET) so it's so easy to support new formats.

To run the program you must have installed **.NET Framework 3.5**

Discussion group / _Foro de discusión_ : https://groups.google.com/forum/#!forum/tinke

Thread at GBAtemp.net: http://gbatemp.net/topic/303529-tinke-072/

# Features #

  * Show the ROM header with the banner and edit it.
  * Show and convert to common format a lot of files
  * Edit a lot of image files from BMP files (NCLR, NCGR, NSCR, NCER), audio files from WAV (SWAV, SWAR, STRM) and fonts (NFTR)
  * Hexadecimal visor
  * Change the content of the files and save the new ROM
  * Multilenguage support

# Supported formats #

## Images ##
  * NCLR => Nitro CoLouR(palette)
  * NCGR => Nitro Character Graphic Resource (tiles)
  * NBGR => Nitro Basic Graphic Resource (tiles)
  * NSCR => Nitro Screen Resource (map)
  * NCER => Nitro CEll Resource (cell/puzzle)
  * NANR => Nitro ANimation Resource (animation)
  * CHAR / CHR => CHARacter (tiles)
  * PLT / PAL => PaLeTte (palette)
  * NBFS => Nitro Basic File Screen (map)
  * NBFP => Nitro Basic File Palette (palette)
  * NBFC => Nitro Basic File Character (tiles)
  * NTFT => NiTro File Tiles (tiles)
  * NTFP => NiTro File Palette (palette)
  * RAW => Raw image (tiles)
  * MAP => Raw map info (map)
  * Common formats => PNG, JPG, TGA

## Textures ##
  * BTX0 (NSBTX)
  * BMD0 (NSBMD)

## Audio ##
  * SDAT => Sound DATa
  * SWAV => Sound WAVe
  * SWAR => Sound Wave ARchive
  * STRM => STReaM
  * SADL
  * Common formats => WAV

## Text ##
  * Sound definition => SADL, XSADL, SARC, SBDL, SMAP.
  * BMG => Pack text file
  * Common formats => TXT, XML, INI, H, BAT, C, MAKEFILE, LUA, CSV, BUILDTIME

## Compression ##
> Thanks to DSDEcmp library [DSDecmp](http://code.google.com/p/dsdecmp) (credits to **barubary**)
  * Huffman (id = 0x20)
  * LZ77    (id = 0x10)
  * LZSS    (id = 0x11)
  * RLE     (id = 0x30)

## Pack ##
  * NARC o ARC => Nintendo ARChives
  * Utility.bin => Wifi data files

# Specific plugin for games #
  * 999, nine hours nine persons nine doors (BSKE)
  * Itsu Demo Doko Demo Dekiru Igo (AIIJ)
  * Blood of Bahamut (CYJJ)
  * Dragon Ball Kai Ultimate Butouden (TDBJ)
  * Ace Attorney Investigation Miles Edgeworth (C32P, C32J)
  * Gyakuten Kenji 2 (BXOJ)
  * Kirby Squeak Squad (AKWE)
  * Last Window The secrete of Cape West (YLUP)
  * El profesor Layton y la Villa Misteriosa (A5FE, A5FP)
  * El profesor Layton y la Caja de Pandora (YLTS)
  * Maple Story DS (YMPK)
  * Ninokuni Shikkoku no Madoushi (B2KJ)
  * Rune Factory 3 (BRFE, BRFJ)
  * The world end with you (AWLJ)
  * Tetris DS (YLUP)
  * Tokimeki Memorial Girl's Side 3rd Story (B3SJ)

  * Cake Mania 2 (CAKX)

---


Link to web pages with NDS info:

  * http://llref.emutalk.net/docs
  * http://nocash.emubase.de/gbatek.htm
  * http://sites.google.com/site/kiwids/sdat.html

---

## Screenshots ##
https://lh5.googleusercontent.com/-GRKvfv-TAaI/ToBy1_eFrfI/AAAAAAAAASA/9WDkc_OQPC4/s800/Tinke%2525200.8.1.PNG
https://lh5.googleusercontent.com/-W6YUKmyV3JM/ToBzRa0_pwI/AAAAAAAAASI/D7g1JKFvgC8/s400/header%252520editor.PNG
https://lh5.googleusercontent.com/_H6ACRUcYPos/TV1ITC1_ceI/AAAAAAAAAG8/cYKNoa3du98/s400/inforom.PNG

https://lh4.googleusercontent.com/-0Rv5v3JQ0AQ/Tn-J8C1gaxI/AAAAAAAAARg/4HvC4j-5olU/s400/btx.PNG
https://lh6.googleusercontent.com/_H6ACRUcYPos/TV1IT9DBy8I/AAAAAAAAAHM/ePmPUmTa4w8/s400/ani.PNG
https://lh3.googleusercontent.com/-Un-1FO1rlD4/ToB0NvJ03ZI/AAAAAAAAASU/iNdHYvEehBc/s400/ncerV2.PNG
https://lh3.googleusercontent.com/_H6ACRUcYPos/TV8C0RtGTzI/AAAAAAAAAHk/wO9ps1DP-EU/s400/nanr.PNG
https://lh6.googleusercontent.com/-pSP4NY3Y9Rw/TqPSrsRc6eI/AAAAAAAAAUg/-QjuDfRdQc4/s400/nftr-2.PNG
https://lh4.googleusercontent.com/-VSJCC9q9TPQ/TmlKbnvgTaI/AAAAAAAAAOg/s7DFYgpeo3c/s400/sdat.PNG

https://lh3.googleusercontent.com/_H6ACRUcYPos/TV1ITRjI1WI/AAAAAAAAAHE/aClaJQdH7xU/s144/imgs2.PNG
https://lh6.googleusercontent.com/_H6ACRUcYPos/TV1ITJsYn5I/AAAAAAAAAHA/yAz7oiEKOa4/s144/imgs1.PNG
https://lh4.googleusercontent.com/_H6ACRUcYPos/TV1IYiOYTOI/AAAAAAAAAHQ/Vdf4K030mdU/s144/text.PNG