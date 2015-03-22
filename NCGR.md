These files, which name could be "Nitro Character Graphic Resource", contain the data of the image
, that is, the colour of each pixels (**tiles**). They are recognized by their header (NCGR or RGCN) and frequently
have the same extension.

A tile is a block of 8x8 pixels.

# NCGR structure #

|_Offset_|_Type_|_Description_|
|:-------|:-----|:------------|

**Generic header**

|0x00|char`[4]`|Magic ID (NCGR)|
|:---|:--------|:--------------|
|0x04|ushort|Endianess (if = 0xFFFE the Magic ID must be reverse)|
|0x06|ushort|Constant (should be 0x1000)|
|0x08|uint|File size|
|0x0C|ushort|Header size|
|0x0E|ushort|# of sections|

**CHAR (CHARacter data) section**

|0x00|char`[4]`|Magic ID (CHAR)|
|:---|:--------|:--------------|
|0x04|uint|Section size|
|0x08|ushort|Height in tiles|
|0x0A|ushort|Width in tiles|
|0x0C|uint|Depth (if 0x03 == 4bpp; else (0x04) == 8bpp)|
|0x10|ushort|Unknown 1|
|0x12|ushort|Unknown 2|
|0x14|uint|Tiled flag (0-7 bit => if 0x00 == Horizontal tile order; else == No tile order)|
|0x18|uint|Tile data size|
|0x1C|uint|Unknown 3|

**Image data**

|0x00|byte`[]`|Image data|
|:---|:-------|:---------|

**SOPC section (optional)**

|0x00|char`[4]`|Magic ID (SOPC)|
|:---|:--------|:--------------|
|0x04|uint|Section size|
|0x08|uint|Unknown 1|
|0x0A|ushort|Unknown 2 (usually 0x10)|
|0x0C|ushort|Unknown 3 (usually 0x20)|

  * _Note 1:_ If the the image is "no tile order" then the width and height must be multiplicate by 8 (the size of one tile)

  * _Note 2:_ The "Tile data size" can be 0, then this value must be (Width `*` Height)

  * _Note 3:_ The image usually have a transparent colour which is the first of the palette.


# Image data #

If the Depth is 8bpp, each byte represent the colour of one pixel, the value of this byte indicates the place of the colour in
the palette data (ie: if it's 0x05 the colour of this pixel is the colour that is placed at the fith position (zero index)).

If the Depth is 4bpp, each byte represent two pixel: 0-3 bit = one pixel; 4-7 bit = second pixel. The value is the same as above.

If the image is Tile order (ie: Horizontal), this indicates that the pixel are stored in blocks of 8x8 pixels (one tile), as follow:

```
 0  1  2  3  4  5  6  7  64 65 66 67 68 69 70 71
 8  9 10 11 12 13 14 15  72 73 74 75 76 77 78 79
16 17 18 19 20 21 22 23  80 ...
24 25 26 27 28 29 30 31
32 33 34 35 36 37 38 39
40 41 42 43 44 45 46 47
48 49 50 51 52 53 54 55
56 57 58 59 60 61 62 63
```

If the image is No Tile order, then the pixels are store in a lineal form:

```
0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 ...
```

## SOPC section ##

This is an strange section that can not be in the file. I think that in a few files (not always) indicates the
subimages that contains one image. The "Unknown 2" value is the Width and Height of this subimage (always they are squared)
and the next value the number of subimages. I see this guest correctly only in one image...