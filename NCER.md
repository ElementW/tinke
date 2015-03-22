These files are used to build an image using some part of a big image, like making a puzzle. It's usually used to obtain the sprites
of NCGR file. In Tinke they have the file format **cell**.
They are recognized by their header NCER or RECN (_Nitro CEll Resource_) and they usually have the same extension.

# Structure #

The file has the following structure:
  * [Generic header](NCER#Generic_header.md)
  * [Section CEBK](NCER#Section_CEBK_(CEll_BanK).md)
    * [Banks](NCER#Banks.md)
      * [Cells](NCER#Cells.md)
  * [Section LBAL](NCER#Section_LABL_(optional).md)
  * [Section UEXT](NCER#Section_UEXT_(optional).md)

|_Offset_|_Name_|_Type_|_Description_|
|:-------|:-----|:-----|:------------|

**Generic header**

|0x00|ID|char`[4]`|NSCR o RCSN|
|:---|:-|:--------|:----------|
|0x04|Endianess|ushort|If = 0xFFFE the Magic ID must be reversed|
|0x06|Constant|ushort|Unknown|
|0x08|File size|uint|  |
|0x0C|Header size|ushort|Always 0x10|
|0x0E|Number of sections|ushort|It can be 0x01, 0x02 or 0x03|

## Section CEBK (CEll BanK) ##

|0x00|ID|char`[4]`|CEBK|
|:---|:-|:--------|:---|
|0x04|Size|uint|  |
|0x08|Nunmber of banks|ushort|  |
|0x0A|Type of banks|ushort|  |
|0x0C|Unknown|uint|It should be 0x18 (offset?)|
|0x10|Block shift|uint|Bit 0-7 -> "image offset" << "block shift"|
|0x14|Unknown|uint|If it's different to 0, you must add "tilepos" to the "image offset"|
|0x18|Unknown|ulong|  |
|0x20|Banks|Banks`[]`|  |

### Banks ###

|0x00|Number of cells|ushort||
|:---|:--------------|:-----|:|
|0x02|Unknown|ushort|  |
|0x04|Cells offset|ushort|Offset of the cell info|

if the bank is type 1, there are the following extra information:

|0x06|X maximum|ushort|Position X maximum|
|:---|:--------|:-----|:-----------------|
|0x08|Y maximum|ushort|Position Y maximum|
|0x0A|X minimum|ushort|Position X minimum|
|0x0C|Y minimum|ushort|Position Y minimum|

#### Cells ####

|0x00|Attribute 0|ushort|See below|
|:---|:----------|:-----|:--------|
|0x02|Attribute 1|ushort|See below|
|0x04|Attribute 2|ushort|See below|

More info-> [gbaTek](http://nocash.emubase.de/gbatek.htm#lcdobjoamattributes)

  * Attribute 0:

```
SS D M OO P R YYYYYYYY
 
S -> (shape) 0=square, 1=horizontal, 2=vertical, 3=prohibited
D -> (colore/palettes) 0=16/16, 1=256/1
M -> (mosaic) 0=off, 1=on
O -> (mode) 0=normal, 1=semi-transparent, 2=obj window, 3=prohibited
P -> if R=1 => (double size flag) 0=normal, 1=double
     if R=0 => (obj disable) 0=normal, 1=not displayed
R -> (Rotation or scale flag) 0=off, 1=on
Y -> Coordenada
```

  * Attribute 1:

```
SS PPPPP XXXXXXXXX

S -> (size) see the table below
P -> if in the attribute 0:
     R=0 => 9-11 ->(not used)
	        12 -> (horizontal flip) 0=normal, 1=mirrored
			13 -> (vertical flip) 0=normal, 1=mirrored
	 R=1 => 9-13 -> rotation or scale parameters
X -> Coordinate X (if it's bigger or equal to 0x100, you must substract 0x200)
```

  * Attribute 2:

```
IIII PP TTTTTTTTTT

I -> Number of the palette (not used in mode 256/1)
P -> Priority relative to background (0 high priority)
T -> Offset of the image tiles
```

Size table:
|Size|Shape|
|:---|:----|
|- |Square|Horizontal|Vertical|
|0 |8x8|16x8|8x16|
|1 |16x16|32x8|8x32|
|2 |32x32|32x16|16x32|
|3 |64x64|64x32|32x64|


## Section LABL (optional) ##

|0x00|ID|char`[4]`|LABL|
|:---|:-|:--------|:---|
|0x04|Size|uint|Section size|
|0x08|Pointers|uint`[]`|Offset to the names, relative to this section|
|----|Bank names|string`[]`|  |

## Section UEXT (optional) ##

|0x00|ID|char`[4]`|UEXT|
|:---|:-|:--------|:---|
|0x04|Size|uint|Section size|
|0x08|Unknown|uint|  |


# Detalles #

  1. Offset 0x14 in CEBK
If the variable is different to 0, then in each bank the first cell will start with 0 in the "tile offset", so
in order to show get the correct offset, you must add the offset of the last bank (the last offset plus the size of this cell).

  1. Block shift in CEBK
  * If it's less than 4, you must displace (bitwise operation) to the left the "block shift" value.
For instance, if the offset is 0x2F and the "block shift" is 3, then the final offset will be:

0x2F << 3 = 0x178

  * If it's greater or equal to 4 the image is already displayed correctly, so we only need to cut it and display this portion in the cell.
> In order to know the coordinate X and Y, where you must cut the image, you can use the following code (C#):
> Basically, the "tile offset" in this case contains the number of pixel horinzontally that you must read to get the correct position, that is,
> if the "tile offset" is bigger than the image width, then you must add 1 to the Y coordinate
```
 tileOffset /= (blockSize / 2);
 int imageWidth = tile.rahc.nTilesX;
 int imageHeight = tile.rahc.nTilesY;
 if (tile.order == TileOrder.Horizontal)
 {
    imageWidth *= 8;
    imageHeight *= 8;
 }

int posX = (int)(tileOffset % imageWidth);
int posY = (int)(tileOffset / imageWidth);

if (tile.rahc.depth == System.Windows.Forms.ColorDepth.Depth4Bit)
    posY *= (int)blockSize * 2;
else
    posY *= (int)blockSize;
if (posY >= imageHeight)
    posY = posY % imageHeight;
```

  1. Size
If the image is "No tile" then you must multiply by 8 the size of each cell.